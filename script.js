const jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwiVXNlcklkIjoiMiIsIlVzZXJOYW1lIjoiMTIzNCIsInVuaXF1ZV9uYW1lIjoiMTIzNCIsImp0aSI6IjUzMTE2M2QwLTVmNWEtNDMyOS1hZDY0LWVhZDllMDk2MzBiZiIsImV4cCI6MTc4ODM0MzQ2NSwiaXNzIjoiQ2hhdEFwcCIsImF1ZCI6IkNoYXRBcHBVc2VycyJ9.5aSwyd-GSeOzkBZsdiewYwbeV60I4QkDjt4tIONz4WI";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7279/chatHub", {
        accessTokenFactory: () => jwtToken
    })
    .build();

connection.on("ReceiveMessage", function (user, message) {
    const li = document.createElement("li");
    li.textContent = user + ": " + message;
    document.getElementById("messagesList").appendChild(li);
});

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

connection.start()
    .then(async function () {
        console.log("Conectat la ChatHub!");
        await loadHistory("1");
        return connection.invoke("JoinConversation", "1");
    })
    .then(function () {
        console.log("M-am alaturat conversatiei 1");
    })
    .catch(function (err) {
        console.error("Eroare la conectare/join:", err);
    });

document.getElementById("sendButton").addEventListener("click", function () {
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToConversation", "1", message)   
        .catch(function (err) {
            console.error("Eroare la trimitere:", err);
        });
    document.getElementById("messageInput").value = "";
});