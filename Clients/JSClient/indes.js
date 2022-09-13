"use strict";

let connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5004/hubs/chat", {
    accessTokenFactory: () =>
      "eyJhbGciOiJSUzI1NiIsImtpZCI6IkM5QzQxNjlDRkJENEUyODYzNDQzOTEwODkxQkU1NUI3IiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIiwibmJmIjoxNjYyOTk3MTUyLCJpYXQiOjE2NjI5OTcxNTIsImV4cCI6MTY2MzAwMDc1Miwic2NvcGUiOlsid29ya291dC5hcGkiXSwiY2xpZW50X2lkIjoiQW50SU9uIiwianRpIjoiRUQzMjE2REQ1RDI3NEE0NDQwNzQ1QzlCMjNDQkEwNEYifQ.CBsh7nnaimBvgJvSQcDNeIDg8yvVzaiFNFjYm1_IKFFj4NnSP1WT-UVdFK0S0TUYA_7GKlwUwF0CFmpvKYxjjgLdG3PQkFLf-VZuzfIMSDEeCOGuybDD3O0kqCKj_mvzSuVwXwqBQIXlpYMmyE_Pz7ZyF8trkBT5OIh_SU2dINgPn4EqChIBfZSz7Gk3WhZJIPEOHcG8rt1QgduRZmODM40BfAAQLCh2c94hA9VCbs2avnV3_bmsZAbxnjKLBBwK3mfE5Ws_QEOz1xuBGApk_i3sipUBH0iG8lBKC67yNcrR8jNOpAqHsTuYY32cPkXY0OsDt4KBsZbFKl3hDfjZJw",
  })
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
