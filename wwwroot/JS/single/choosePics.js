
let imagesURI = 'https://localhost:7169/api/Images';

function myfget() {
    fetch(imagesURI + "/by/" + myId)
        .then(response => response.json())
        .then(data => {            
            console.log(data)
        })
        .catch(error => {
            console.error(error);
        });
}

function myfpost() {
    const fileInput = document.querySelector("#fileUpload");
    const formData = new FormData();
    formData.append("file", fileInput.files[0]);
    const options = {
        method: "POST",
        body: formData
    };
    fetch(imagesURI + "/user/" + myId, options)
        .then(response => response.json())
        .then(data => {
            console.log(data)
        })
        .catch(error => {
            console.error(error);
        });
}

function reportChanges() {
    fetch("https://localhost:7169/api/Users"+"/changes/?up=true"+"&id=989");
}

function updateMyPic(imgobj){
    console.log(imgobj);
    boolUp = true;
}

function myfput() {
    const fileInput = document.querySelector("#fileUpload");
    const formData = new FormData();
    formData.append("file", fileInput.files[0]);
    const options = {
        method: "PUT",
        body: formData
    };
    fetch(imagesURI + "/put/" + myId, options)
        .then(response => response.json())
        .then(imgobj => updateMyPic(imgobj))
        .catch(error => { console.error(error); });
}

function openSettings() {
    var modalOpenSettings = document.getElementById("modalOpenSettings");    
    var closeModalButton = document.getElementById("closeModalButton");
    modalOpenSettings.style.display = "block";

    closeModalButton.onclick = function () {        
        modalOpenSettings.style.display = "none";
    }
    window.onclick = function (event) {
        if (event.target == modal) {
            modalOpenSettings.style.display = "block";
        }
    }

    var modalEditUser = document.getElementById("modalEditUser");
    var editUserButton = document.getElementsByClassName("editUserButton")[0];
    editUserButton.onclick = function () {
        modalEditUser.style.display = "block";
    }
    var closeEditUserlButton = document.getElementById("closeEditUser");
    closeEditUserlButton.onclick = function () {
        modalEditUser.style.display = "none";
    }

    var modalCountReport = document.getElementById("modalCountReport");
    var contadorButton = document.getElementsByClassName("contadorButton")[0];
    contadorButton.onclick = function () {
        getCountReport();
        modalCountReport.style.display = "block";
    }

    var closeCountReportlButton = document.getElementById("closeCountReport");
    closeCountReportlButton.onclick = function () {
        removeChildElements("elements");
        modalCountReport.style.display = "none";
    }
    var putUserButton = document.getElementsByClassName("putUser")[0];
    putUserButton.onclick = function () {
        putUser();
        modalEditUser.style.display = "none";        
    }
}
