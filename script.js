const jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyIiwiVXNlcklkIjoiMiIsInVuaXF1ZV9uYW1lIjoiMTIzNCIsImp0aSI6ImMzYWY0OTAwLTE0YzUtNDg1Zi05YTUyLWY5MmVlZGU3MWFjMCIsImV4cCI6MTc4ODI2NTYwNSwiaXNzIjoiQ2hhdEFwcCIsImF1ZCI6IkNoYXRBcHBVc2VycyJ9.U9yQv4UQt6ZReUfs5-RoO1eG7InnrTLS2oO87xM6rsU";

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
    connection.invoke("SendMessageToConversation", "1", "TestUser", message)
        .catch(function (err) {
            console.error("Eroare la trimitere:", err);
        });
    document.getElementById("messageInput").value = "";
});