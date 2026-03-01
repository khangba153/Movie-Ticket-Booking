document.addEventListener("DOMContentLoaded", function () {
    // Check login
    const userId = localStorage.getItem("userId");
    const username = localStorage.getItem("username");
    if (!userId) {
        window.location.href = "/auth.html";
        return;
    }

    // Show welcome
    const welcomeEl = document.getElementById("welcomeUser");
    if (welcomeEl && username) {
        welcomeEl.textContent = "Xin chào, " + username;
    }

    // Show admin link if admin
    const adminLink = document.getElementById("adminLink");
    if (adminLink && localStorage.getItem("role") === "Admin") {
        adminLink.style.display = "";
    }

    // Logout
    const logoutBtn = document.getElementById("logoutBtn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", function () {
            localStorage.clear();
            window.location.href = "/auth.html";
        });
    }

    loadMovies();
});

async function loadMovies() {
    console.log("🎬 Loading movies from API...");
    try {
        const response = await fetch("/api/movie");
        console.log("✅ API Response:", response.status);
        const movies = await response.json();
        console.log("✅ Movies loaded:", movies.length, "movies");

        const container = document.getElementById("movie-list");
        container.innerHTML = "";

        if (movies.length === 0) {
            container.innerHTML = "<p>Không có phim nào.</p>";
            return;
        }

        movies.forEach(movie => {
    const card = document.createElement("div");
    card.className = "movie-card";
    card.style.cursor = "pointer";
    
    // Toàn bộ card đều có thể click để chuyển đến trang chi tiết phim
    card.innerHTML = `
        <img src="${movie.posterUrl}" class="poster" alt="${movie.title}">
        <h3>${movie.title}</h3>
    `;
    
    // Thêm click handler trực tiếp
    card.addEventListener("click", function() {
        console.log("🎬 Clicked on:", movie.title);
        window.location.href = "Movie/movie.html?id=" + movie.movieId;
    });
    
    console.log("📌 Created card for:", movie.title, "- Link: Movie/movie.html?id=" + movie.movieId);
    container.appendChild(card);
});
        console.log("✅ All movie cards rendered");

    } catch (error) {
        console.error("Lỗi khi tải phim:", error);
    }
}