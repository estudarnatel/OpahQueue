
// REVER TODAS AS CONSTANTES
let myId = 0;
let myPosition = 0;
let myToken = "";
let myName = "";
let boolLoaded = false;
let boolUp = false;
let newPics = false;
let testBool = false;
let timeNewPics = "";
let queueIds = [];
let queueData = [];
let queueSize = 0;
let dataSize = 0;

function queueCount(itemCount) {
  const name = (itemCount === 1) ? 'pessoa' : 'pessoas';
  document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function changeScreen(bool) {  
  if(bool)
  {
    document.getElementById("login").hidden = true;    
    document.getElementById("main").hidden = false;
    document.getElementById("welcome").textContent = "Bem Vindo " + myName + "!";
  }
  else
  {
    document.getElementById('username').value = "";
    // fila.removeChild(nomes);  //PARA NÃO DUPLICAR AS DIVS //element.remove(); //NÃO FUNCIONA EM TODOS OS NAVEGADORES
    document.getElementById("login").hidden = false;    
    document.getElementById("main").hidden = true;
  }
  // if(newPics)
  // {
  //   testBool = true;
  //   newPics = false;
  // }
}

function logout() {
  putLogout();  
  myId = 0;
  myToken = "";
  boolLoaded = false;
  queueIds = [];
  // queueIds.splice(0,auxVector.length); // REMOVE TODOS OS ELEMENTOS
  changeScreen(boolLoaded);
}

function displayPosition(data) {// console.log(data);
  processando = true;
  // document.getElementById('position').textContent = data.user.position;
  document.getElementById('position').textContent = data.position;  
  // if(timeNewPics.localeCompare(data.timeNewPics) != 0)
  // {
  //   timeNewPics = data.timeNewPics;
  //   console.log("NOVO = " + timeNewPics);
  //   newPics = true;
  // }
  if (boolUp)
  {
    boolUp = false;
    // updateElements(data.list);
    // console.log("atualizannnnnndo local");
    // updateElements(queueData);
    // console.log("local ATUALIZADO ");
  }
  if(newPics)
  { // console.log("atualizannnnndo coleguinhas");
    // updateElements(queueData);
    // console.log("COLEGAS UP");
    // getImages();
    // newPics = false;
  }
  if(testBool)
  {
    // updateElements(data.list);
    testBool = false;
  }
  processando = false;
}

function removeChildElements(id) {
  let list = document.getElementById(id);
  while (list.hasChildNodes()) {
    list.removeChild(list.firstChild); // list.removeChild(list.lastChild);
  }
  return list;
}

function makeTDtag(tag, text) {
  let tD = document.createElement("td");
  tD.textContent = text;
  tag.appendChild(tD);
  return tag;
}

function fillTable(data) {
  console.log(data);
  let tR = undefined;
  let div = document.getElementById("elements");
  data.forEach(element => {
      tR = document.createElement("tr");
      for (let x in element) {
          tR = makeTDtag(tR, element[x]);
      }
      div.appendChild(tR);
  });
}