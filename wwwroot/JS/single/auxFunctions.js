
function beforeWhiteSpace(str) {
    let i = str.search(" ");
    if (i == -1)
        i = str.length;
    // let name = str.slice(0, i);
    // let sname = str.slice(i+1, str.length-1);
    // let pre = "<pre>" + name + "<br></br>" + sname + "</pre>";
    // let pre = name + "<br></br>" + sname;
    // return pre;
    return str.slice(0, i);
}

function afterWhiteSpace(str) {
    let i = str.search(" ");
    return str.slice(i + 1, str.length);
}

function msgError(error) {
    document.getElementById("errormsg").hidden = false;
    console.error('Unable to get items.', error);
}

function getPositionDiv() {
    ac = document.getElementById("fila");
    // ac.style.textAlign = "center"
    // nomes.style.textAlign = "center";
    console.log("ALINHAMENTO");
    console.log(ac.style.textAlign);
    console.log("ALINHAMENTO");
}
