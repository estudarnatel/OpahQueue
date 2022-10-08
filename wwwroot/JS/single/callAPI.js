
let usersURI = "https://localhost:7169/api/Users";
let callsURI = "https://localhost:7169/api/Calls";
let protocolsURI = "https://localhost:7169/api/Protocols";
let reportsURI = "https://localhost:7169/api/Reports";

let httpRequestPut = {
    method: 'PUT',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }
};

function objCustomRequest(boolFurarFila, boolLogout) {
    return {
        Id: myId,
        FurarFila: boolFurarFila,
        Logout: boolLogout
    };
}

let imagesList = [];

let ti = undefined;
let ex = undefined;

let objPostCP = {
}

let objPostOptionsCP = {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json;charset=utf-8'
    }
};

function porSegundo(){
    // setInterval ( "doSomething()", 1000);
    // function doSomething() {
    //     // (do something here)
    // }
    // RETIRAR O refreshScreen DO SCRIPT DO INDEX.HTML E TROCAR POR ESSA AQUI
    setInterval(pegaFila, 1000);
    setInterval(fonteDAimagem, 1000);
}

let processando = false;

function getPosition() {
    if (!processando) {
        if (boolLoaded) {
            fetch(usersURI + "/dtnow/changes/single/?up="+boolUp+"&id="+myId)
                .then(response => response.json())
                .then(data => displayPosition(data))
                .catch(error => console.error('Unable to get items.', error));
        }
    }
    else
    {
        console.log("N ~ A O GET");
    }
}

function getCountReport() {
    fetch(reportsURI)
        .then(response => response.json())
        .then(data => fillTable(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getUserData(data) {
    if (data.hasOwnProperty('id')) {        
        myId = data.id;
        myName = data.name;
        myToken = data.token;
        boolLoaded = true;
        changeScreen(boolLoaded);
    }
}

function postLogin() {
    let obj_user = {
        "Username": document.getElementById('username').value,
        "Password": document.getElementById('password').value
    }
    let objPostOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(obj_user)
    };
    fetch(usersURI + '/authenticate', objPostOptions)
        .then(response => response.json())
        .then(responseJson => getUserData(responseJson))
        .catch(error => console.error('Unable to get items.', error));
}

function putFetchCountReport(route, objJson)
{
    fetch(route, objJson)
        .then(response => response.json())
        .then(responseJson => console.log(responseJson))
        .catch(error => console.error('Unable to get items.', error));
}

function postFetchCP(route, objJson)
{
    fetch(route, objJson)
        .then(response => response.json())
        .then(responseJson => console.log(responseJson))
        .catch(error => console.error('Unable to get items.', error));
}

function postFetchEXCEL()
{
    let objPostOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
    };
    console.log(objPostOptions);
    
    fetch("https://localhost:7169/api/Users/excel")
        .then(response => response.blob())
        .then(blob => {
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = "filename.xlsx";
            document.body.appendChild(a);
            a.click();    
            a.remove();
        });
}


function getFetchEXCEL()
{
    fetch("https://localhost:7169/api/Reports/exportFullReport")
        .then(response => response.blob())
        .then(blob => {
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = "filename.xlsx";
            document.body.appendChild(a);
            a.click();    
            a.remove();
        });
}

function postRegister(json) {
    if (json.hasOwnProperty('isFirst'))
    {
        console.log("QUER FURAR FILA ?");
        console.log(json);
        var modal = document.getElementById("modal");
        var btnManter = document.getElementsByClassName("manter")[0];
        var btnFurar = document.getElementsByClassName("furar")[0];
        
        modal.style.display = "block";

        btnManter.onclick = function () {
            modal.style.display = "none";
        }
        btnFurar.onclick = function () {
            putFurarFila();
            modal.style.display = "none";
        }
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "block";
            }
        }
    }
    else
    {
        console.log("POSIÇÃO ALTERADA");
        console.log(json);
        
        objPostCP["UserId"] = myId;
        
        if(ex == "" && ti == "")
            console.log("INFORME UM CHAMADO. TI ATENDE OU EXCELLER PARA CONTINUAR");
        else if(ti != "" && ex == "")
        {
            console.log("TI ATENDE");
            objPostCP["Called"] = ti;
            objPostOptionsCP["body"] = JSON.stringify(objPostCP);
            console.log(objPostOptionsCP);        
            postFetchCP(callsURI, objPostOptionsCP);
            putFetchCountReport(reportsURI+"/countReport/?UserId="+myId+"&TI="+true+"&ex="+false+"", httpRequestPut);
        }    
        else if(ex != "" && ti == "")
        {
            console.log("exceller");
            objPostCP["Protocol"] = ex;
            objPostOptionsCP["body"] = JSON.stringify(objPostCP);
            console.log(objPostOptionsCP);
            postFetchCP(protocolsURI, objPostOptionsCP);
            putFetchCountReport(reportsURI+"/countReport/?UserId="+myId+"&TI="+false+"&ex="+true+"", httpRequestPut);
        }
        else
            console.log("VOCÊ SÓ PODE ATENDER UM CHAMADO POR VEZ. TI ATENDE OU EXCELLER. APAGUE UM DOS CAMPOS");
    }
}

function putFetch(httpRequestPut) {
    fetch(usersURI, httpRequestPut)
        .then(response => response.json())
        .then(responseJson => postRegister(responseJson));
}

function putPosition() {    
    ti = document.getElementById('ti').value;
    ex = document.getElementById('ex').value;
    httpRequestPut["body"] = JSON.stringify(objCustomRequest(false, false));
    putFetch(httpRequestPut);
}

function putFurarFila() {    
    httpRequestPut["body"] = JSON.stringify(objCustomRequest(true, false));
    putFetch(httpRequestPut);
}

function putLogout() {    
    httpRequestPut["body"] = JSON.stringify(objCustomRequest(false, true));
    fetch(usersURI + "/logout", httpRequestPut)
        .then(response => response.json())
        .then(responseJson => console.log(responseJson));
}

function putUser() {
    httpRequestPut["headers"]["Authorization"] = "Bearer " + myToken;
    let obj_user = {
        "Id": myId,
        "Name": document.getElementById('putName').value,
        "Password": document.getElementById('putPassword').value
    }
    httpRequestPut["body"] = JSON.stringify(obj_user);
    console.log(httpRequestPut);
    console.log(typeof tempid);
    fetch(usersURI + "/updateUser", httpRequestPut)
        .then(response => response.json())
        .then(responseJson => console.log(responseJson));
}
