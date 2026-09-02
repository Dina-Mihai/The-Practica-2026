let jwtToken = null;
let connection = null;

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
        document.getElementById("chatSection").style.display = "block";

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

    await loadHistory("1");
    await connection.invoke("JoinConversation", "1");
    console.log("M-am alaturat conversatiei 1");
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
    connection.invoke("SendMessageToConversation", "1", message)
        .catch(function (err) {
            console.error("Eroare la trimitere:", err);
        });
    document.getElementById("messageInput").value = "";
});