function logOut(){
    localStorage.removeItem("user-id")
}
window.onload = function () {
    const isAuthenticated = localStorage.getItem("user-id")
    if (isAuthenticated==null) {
        window.location.href = `/frontend/admin/login.html`;
    }
}