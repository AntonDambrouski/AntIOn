"use strict";

let connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5004/hubs/chat")
  .build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveAnonymous", (message) => {
  let li = document.createElement("li");
  document.getElementById("messagesList").appendChild(li);

  li.textContent = `Anonymous user ${message.from.username}: ${message.text}`;
});

connection.on("NewUserNotification", (notification) => {
  let li = document.createElement("li");
  document.getElementById("messagesList").appendChild(li);

  li.textContent = `${notification.text}`;
});

connection.on("ReceiveAuthorized", (message) => {
  let li = document.createElement("li");
  document.getElementById("messagesList").appendChild(li);

  li.textContent = `Authorized user ${message.from.username}: ${message.text}`;
});

connection
  .start()
  .then(function () {
    document.getElementById("sendButton").disabled = false;
  })
  .catch(function (err) {
    return console.error(err.toString());
  });

document
  .getElementById("sendButton")
  .addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendAnonymous", { Text: message }).catch(function (err) {
      return console.error(err.toString());
    });
    event.preventDefault();
  });
