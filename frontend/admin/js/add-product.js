var loadFile = function (event) {
    var image = document.getElementById('output');
    image.src = URL.createObjectURL(event.target.files[0]);
};
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