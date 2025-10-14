// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function loadView(status) {
    var apiUrl = '/api/login/defaultview';
    if (status === "logout")
        apiUrl = '/api/logout';
    if (status === "adminInformation")
        apiUrl = '/api/admin/adminInformation';

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('main').innerHTML = data;
            //if (status === "logout") {
            //    document.getElementById('LogoutButton').style.display = "none";
            //}
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}

document.addEventListener("DOMContentLoaded", loadView);

function performAuth() {
    const data = {
        Handle: document.getElementById('SHandle').value,
        PassWord: document.getElementById('SPass').value
    };

    fetch('/api/login/authenticate', {
        method: 'POST',                     
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(r => r.text())                  
        .then(html => {
            //document.getElementById('LogoutButton').style.display = "block";
            document.getElementById('main').innerHTML = html;
        })
        .catch(err => console.error('Error:', err));
}

function updateAdminProfile() {
    const payload = {
        FirstName: document.getElementById('ap_firstName').value.trim(),
        LastName: document.getElementById('ap_lastName').value.trim(),
        Email: document.getElementById('ap_email').value.trim(),
        Phone: document.getElementById('ap_phone').value.trim(),
        Address: document.getElementById('ap_address').value.trim(),
        PicturePath: document.getElementById('ap_picture').value.trim(),
        Password: document.getElementById('ap_newPassword').value,
        Handle: document.getElementById('ap_handle').value
    };

    //if (payload.newPassword && payload.newPassword !== payload.confirmPassword) {
    //    alert("Passwords do not match.");
    //    return;
    //}

    fetch('/api/admin/editUser', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                alert("Profile successfully updated!");
                loadView('adminInformation');
            } else {
                alert("Update failed: " + (data.message || "unknown error"));
            }
        })
        .catch(error => {
            console.error('Error updating profile:', error);
            alert("An error occurred while updating your profile.");
        });
}
