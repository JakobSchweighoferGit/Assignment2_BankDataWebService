// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function loadView(status) {
    var apiUrl = '/api/login/defaultview';
    if (status === "logout")
        apiUrl = '/api/logout';
    if (status === "adminInformation")
        apiUrl = '/api/AdminInformationManagement/adminInformation';
    if (status === "createUser")
        apiUrl = '/api/AdminCreateUSer/createUserView';

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            document.getElementById('main').innerHTML = data;
        })
        .catch(error => {
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


    fetch('/api/AdminInformationManagement/editAdmin', {
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



function searchForUserWithSearchString() {
    const data = {
        SearchString: document.getElementById('ap_SearchString').value,
    };

    fetch('/api/AdminUserManagement/adminUserManagmentListInformation', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(r => r.text())
        .then(html => {
            document.getElementById('main').innerHTML = html;
        })
}

function openEditUserPage(handle) {
    const data = {
        handle
    };

    fetch('/api/AdminUserManagement/adminUserEditInformation', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(r => r.text())
        .then(html => {
            document.getElementById('main').innerHTML = html;
        })
}


function updateUserProfile() {
    const data = {
        FirstName: document.getElementById('ap_firstName').value.trim(),
        LastName: document.getElementById('ap_lastName').value.trim(),
        Email: document.getElementById('ap_email').value.trim(),
        Phone: document.getElementById('ap_phone').value.trim(),
        Address: document.getElementById('ap_address').value.trim(),
        PicturePath: document.getElementById('ap_picture').value.trim(),
        Password: document.getElementById('ap_newPassword').value,
        Handle: document.getElementById('ap_handle').value
    };


    fetch('/api/AdminUserManagement/adminEditUser', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
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
            } else {
                alert("Update failed: " + (data.message || "unknown error"));
            }
        })
        .catch(error => {
            console.error('Error updating profile:', error);
            alert("An error occurred while updating your profile.");
        });
}

function createUser() {
    const data = {
        handle: document.getElementById('cu_handle').value.trim(),
        firstName: document.getElementById('cu_firstName').value.trim(),
        lastName: document.getElementById('cu_lastName').value.trim(),
        email: document.getElementById('cu_email').value.trim(),
        password: document.getElementById('cu_password').value, 
        address: document.getElementById('cu_address').value.trim(),
        phone: document.getElementById('cu_phone').value.trim(),
        picturePath: document.getElementById('cu_picturePath').value.trim(),
        admin: document.getElementById('cu_admin').value === 'true'
    };


    if (!data.handle || !data.firstName || !data.lastName || !data.email || !data.password) {
        alert('Handle, first name, last name, email and password are required.');
        return;
    }

    fetch('/api/adminCreateUser/admincreatesUserWithData', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(r => r.json())
        .then(d => {
            if (d.success) {
                alert('User created.');
                loadView('createUser'); 
            } else {
                alert('Create failed: ' + (d.message || 'unknown error'));
            }
        })
        .catch(err => {
            console.error(err);
            alert('Network error');
        });
}