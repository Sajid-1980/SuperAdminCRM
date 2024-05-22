"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

// Variable to keep track of the notification count
var notificationCount = 0;

// Log connection state changes
connection.onclose(function (error) {
    console.log("Connection closed. Trying to restart...");
    if (error) {
        console.error(error);
    }
    // Attempt to restart the connection after a delay
    setTimeout(function () {
        connection.start().then(function () {
            console.log("Connection restarted.");
            // Invoke LoadNotifications once the connection is re-established
            connection.invoke("LoadNotifications");
        }).catch(function (err) {
            console.error(err.toString());
        });
    }, 5000); // 5 seconds delay, adjust as needed
});

// Log received notifications
connection.on("ReceiveNotifications", function (notifications) {
    console.log("Received notifications:", notifications);
    // Filter out already read notifications
    var unreadNotifications = notifications.filter(function (notification) {
        return !notification.isRead; // Assuming there is an isRead property in your Notification model
    });

   if (unreadNotifications.length > 0) {
        notificationCount += unreadNotifications.length;

        // Update the notification number in the UI
        updateNotificationNumber();
    }

    // Clear existing notifications in the view
    var msgList = document.getElementById("msgList");
    // Check if the msgList element exists
    if (!msgList) {
        console.error("msgList element not found in the document.");
        return;
    }
    msgList.innerHTML = "";

    // Display notifications in the view
    notifications.forEach(function (notification) {
        var li = document.createElement("li");
        li.textContent = notification.message; // Assuming there is a property "Message" in your Notification model
        msgList.appendChild(li);

        // Increment the notification count for each new notification
    });

    // Update the notification number in the UI
    updateNotificationNumber();
});

// Function to update the notification number in the UI
function updateNotificationNumber() {
    var badgeNumber = document.querySelector("#notificationIcon .badge-number");

    // Check if the badgeNumber element exists
    if (!badgeNumber) {
        console.error("badgeNumber element not found in the document.");
        return;
    }

    // Update the UI with the current notification count
    badgeNumber.textContent = notificationCount;

    // Show or hide the badge based on the notification count
    if (notificationCount > 0) {
        badgeNumber.style.display = "inline";
    } else {
        badgeNumber.style.display = "none";
    }
}

// Event handler for clicking the bell icon
document.querySelector("#notificationIcon").addEventListener("click", function () {
    // Reset the notification count when the bell icon is clicked
    notificationCount = 0;

    // Update the notification number in the UI
    updateNotificationNumber();
});

// Start the connection
connection.start().then(function () {
    console.log("Connection started.");
    // Invoke LoadNotifications once the connection is established
    connection.invoke("LoadNotifications").then(function () {
        console.log("LoadNotifications invoked successfully hi.");
    }).catch(function (err) {
        console.error("Error invoking LoadNotifications:", err.toString());
    });
}).catch(function (err) {
    console.error("Error starting connection:", err.toString());
});
