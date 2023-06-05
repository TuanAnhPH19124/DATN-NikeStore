function closeModal(modalId) {
    $(modalId).modal('hide');
  }
  function selectButton(button) {
    // Deselect all buttons in the group
    var buttons = button.parentElement.children;
    for (var i = 0; i < buttons.length; i++) {
      buttons[i].classList.remove("selected");
    }

    // Select the clicked button
    button.classList.add("selected");
  }
  function increment() {
    const input = document.getElementById("number");
    const newValue = parseInt(input.value) + 1;
    input.value = newValue;
  }

  function decrement() {
    const input = document.getElementById("number");
    let newValue = parseInt(input.value) - 1;
    if(newValue<0){
      newValue=0;
    }
    input.value = newValue;
  }
  $('#productModal').on('shown.bs.modal', function() {
    $(this).find('#productModal').focus();
 });