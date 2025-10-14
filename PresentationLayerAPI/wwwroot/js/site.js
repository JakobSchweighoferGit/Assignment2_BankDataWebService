// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function loadView(status) {
    var apiUrl = '/api/login/defaultview';
    if (status === "logout")
        apiUrl = '/api/logout';

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


