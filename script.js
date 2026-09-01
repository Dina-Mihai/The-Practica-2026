const jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwiVXNlcklkIjoiMiIsInVuaXF1ZV9uYW1lIjoiMTIzNCIsImp0aSI6ImM4MDUxNmJmLTgzNWQtNDAzNi05MjZkLWRlZTY0ZDljNTg3YSIsImV4cCI6MTc4ODE5NTI5OSwiaXNzIjoiQ2hhdEFwcCIsImF1ZCI6IkNoYXRBcHBVc2VycyJ9.Qho3CJXQq80bz5xN0-VQIUn_CQMLC_dE6asnHB61EOk";

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

connection.start()
    .then(function () {
        console.log("Conectat la ChatHub!");
    })
    .catch(function (err) {
        console.error("Eroare la conectare:", err);
    });

document.getElementById("sendButton").addEventListener("click", function () {
    const message = document.getElementById("messageInput").value;
    const conversationId = 1;

    connection.invoke("SendMessage", conversationId, "TestUser", message)
        .catch(function (err) {
            console.error("Eroare la trimitere:", err);
        });
    document.getElementById("messageInput").value = "";
});

