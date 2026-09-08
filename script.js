let jwtToken = null;
let connection = null;
let currentConversationId = null;

document.getElementById("loginButton").addEventListener("click", async function () {
    const username = document.getElementById("loginUsername").value;
    const password = document.getElementById("loginPassword").value;
    const statusEl = document.getElementById("loginStatus");
    try {
        const response = await fetch("https://localhost:7279/api/auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                loginEmail: username,
                loginPassword: password
            })
        });
        if (!response.ok) {
            statusEl.textContent = "Login esuat: " + response.status;
            return;
        }
        const data = await response.json();
        jwtToken = data.token;
        statusEl.textContent = "Autentificat cu succes!";
        document.getElementById("loginSection").style.display = "none";

        await startChatConnection();
    } catch (err) {
        statusEl.textContent = "Eroare la login: " + err;
    }
});

async function startChatConnection() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7279/chatHub", {
            accessTokenFactory: () => jwtToken
        })
        .build();

    connection.on("ReceiveMessage", function (user, message) {
        const li = document.createElement("li");
        li.textContent = user + ": " + message;
        document.getElementById("messagesList").appendChild(li);
    });

    await connection.start();
    console.log("Conectat la ChatHub!");

    document.getElementById("conversationSection").style.display = "block";
    await loadMyConversations();
}

async function loadMyConversations() {
    const response = await fetch("https://localhost:7279/api/conversations", {
        headers: { "Authorization": "Bearer " + jwtToken }
    });

    if (!response.ok) {
        console.error("Eroare la incarcarea conversatiilor:", response.status);
        return;
    }

    const conversations = await response.json();
    const listEl = document.getElementById("conversationsList");
    listEl.innerHTML = "";

    conversations.forEach(function (conv) {
        const li = document.createElement("li");
        const btn = document.createElement("button");
        btn.textContent = conv.conversationName || `Conversatie #${conv.conversationID}`;
        btn.addEventListener("click", function () {
            selectConversation(conv.conversationID);
        });
        li.appendChild(btn);
        listEl.appendChild(li);
    });
}

document.getElementById("createConversationButton").addEventListener("click", async function () {
    const idsText = document.getElementById("participantIdsInput").value;
    const name = document.getElementById("conversationNameInput").value || null;

    const participantIds = idsText
        .split(",")
        .map(s => parseInt(s.trim()))
        .filter(n => !isNaN(n));

    if (participantIds.length === 0) {
        alert("Introdu cel putin un ID valid.");
        return;
    }

    const conversationId = await createOrGetConversation(participantIds, name);
    if (conversationId !== null) {
        await selectConversation(conversationId);
    }
});

async function createOrGetConversation(participantIds, conversationName) {
    const response = await fetch("https://localhost:7279/api/conversations", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + jwtToken
        },
        body: JSON.stringify({
            participantIds: participantIds,
            conversationName: conversationName
        })
    });

    if (!response.ok) {
        console.error("Eroare la crearea conversatiei:", response.status);
        return null;
    }

    const data = await response.json();
    return data.conversationId;
}

async function selectConversation(conversationId) {
    if (currentConversationId !== null) {
        await connection.invoke("LeaveConversation", currentConversationId.toString());
    }

    currentConversationId = conversationId;

    document.getElementById("conversationSection").style.display = "none";
    document.getElementById("chatSection").style.display = "block";

    await loadHistory(currentConversationId);
    await connection.invoke("JoinConversation", currentConversationId.toString());
    console.log("M-am alaturat conversatiei " + currentConversationId);
}

async function loadHistory(conversationId) {
    try {
        const response = await fetch(`https://localhost:7279/api/conversations/${conversationId}/messages?page=1&pageSize=20`, {
            headers: {
                "Authorization": "Bearer " + jwtToken
            }
        });
        if (!response.ok) {
            console.error("Eroare la incarcarea istoricului:", response.status);
            return;
        }
        const messages = await response.json();
        document.getElementById("messagesList").innerHTML = "";
        messages.reverse().forEach(function (msg) {
            const li = document.createElement("li");
            li.textContent = `[${msg.sentTimeAndDate}] ${msg.senderName}: ${msg.content}`;
            document.getElementById("messagesList").appendChild(li);
        });
    } catch (err) {
        console.error("Eroare la fetch istoric:", err);
    }
}

document.getElementById("sendButton").addEventListener("click", function () {
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToConversation", currentConversationId.toString(), message)
        .catch(function (err) {
            console.error("Eroare la trimitere:", err);
        });
    document.getElementById("messageInput").value = "";
});