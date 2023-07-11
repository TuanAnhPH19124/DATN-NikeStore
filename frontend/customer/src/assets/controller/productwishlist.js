document.addEventListener("DOMContentLoaded", function() {
    var heartIcon = document.querySelector(".fa-heart");
    var favoriteCount = heartIcon.parentElement.dataset.favoriteCount;

    heartIcon.addEventListener("click", function() {
        // Tăng số lượng yêu thích
        favoriteCount++;
        
        // Cập nhật số lượng yêu thích trên giao diện
        heartIcon.parentElement.dataset.favoriteCount = favoriteCount;
        heartIcon.parentElement.setAttribute("title", "Đã thêm vào yêu thích (" + favoriteCount + ")");
    });
});