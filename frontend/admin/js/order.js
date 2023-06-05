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