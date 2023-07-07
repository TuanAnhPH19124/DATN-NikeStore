const icon = document.querySelectorAll(".icon")
const text = document.querySelectorAll(".text")
const progress = document.querySelectorAll(".progres")
let index = 0;

function update(){
    icon[index].style.visibility="visible";
    text[index].style.visibility="visible";
    progress[index].classList.add("active");
    index++
}
function increment() {
  const input = document.getElementById("number");
  let newValue = parseInt(input.value) + 1;
  if(isNaN(newValue)){
    newValue=0;
  }
  input.value = newValue;
}

function decrement() {
  const input = document.getElementById("number");
  let newValue = parseInt(input.value) - 1;
  if(newValue<0||isNaN(newValue)){
    newValue=0;
  }
  input.value = newValue;
}