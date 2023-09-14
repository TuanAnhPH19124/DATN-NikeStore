function logOut(){
    localStorage.removeItem("user-id")
}
window.onload = function () {
    const isAuthenticated = localStorage.getItem("user-id")
    if (isAuthenticated==null) {
        window.location.href = `/frontend/admin/login.html`;
    }
}
const role = localStorage.getItem("role")
console.log(role)
if(role=="Employee"){
    var navItems = document.querySelectorAll('.nav-item');

    // Indices of the nav-items to hide
    var indicesToHide = [0, 3, 4, 5];
    
    // Loop through each nav-item
    navItems.forEach(function(navItem, index) {
        // Check if the current index should be hidden
        if (indicesToHide.includes(index)) {
            navItem.style.display = "none"; // Hide the element
        }
    });
}