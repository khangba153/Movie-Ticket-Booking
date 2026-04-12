const POST_AUTH_REDIRECT_KEY = "postAuthRedirect";

document.addEventListener("DOMContentLoaded", function () {
    const welcomeEl = document.getElementById("welcomeUser");
    const adminLink = document.getElementById("adminLink");
    const myTicketsLink = document.getElementById("myTicketsLink");
    const authActionBtn = document.getElementById("authActionBtn");

    const userId = parseInt(localStorage.getItem("userId"), 10);
    const username = localStorage.getItem("username");
    const role = localStorage.getItem("role");
    const isLoggedIn = Number.isInteger(userId) && userId > 0;

    if (welcomeEl) {
        welcomeEl.textContent = isLoggedIn
            ? `Xin chào, ${username || "bạn"}`
            : "Bạn đang xem với tư cách khách";
    }

    if (adminLink) {
        adminLink.style.display = isLoggedIn && role === "Admin" ? "" : "none";
    }

    if (myTicketsLink && !isLoggedIn) {
        myTicketsLink.addEventListener("click", function (event) {
            event.preventDefault();
            sessionStorage.setItem(POST_AUTH_REDIRECT_KEY, "/MyTickets/mytickets.html");
            window.location.href = "/auth.html";
        });
    }

    if (authActionBtn) {
        authActionBtn.textContent = isLoggedIn ? "Đăng xuất" : "Đăng nhập / Đăng ký";
        authActionBtn.addEventListener("click", function () {
            if (isLoggedIn) {
                clearAuthState();
                window.location.href = "/home.html";
                return;
            }

            sessionStorage.removeItem(POST_AUTH_REDIRECT_KEY);
            window.location.href = "/auth.html";
        });
    }

    loadMovies();
});

async function loadMovies() {
    console.log("🎬 Loading movies from API...");
    const container = document.getElementById("movie-list");
    try {
        const response = await fetch("/api/movie");
        console.log("✅ API Response:", response.status);

        if (!response.ok) {
            throw new Error("Server error: " + response.status);
        }

        const movies = await response.json();
        console.log("✅ Movies loaded:", movies.length, "movies");

        container.innerHTML = "";

        if (movies.length === 0) {
            container.innerHTML = "<p>Không có phim nào.</p>";
            return;
        }

        movies.forEach(movie => {
            const card = document.createElement("div");
            card.className = "movie-card";
            card.style.cursor = "pointer";

            card.innerHTML = `
                <img src="${movie.posterUrl}" class="poster" alt="${movie.title}">
                <h3>${movie.title}</h3>
            `;

            card.addEventListener("click", function () {
                console.log("🎬 Clicked on:", movie.title);
                window.location.href = "Movie/movie.html?id=" + movie.movieId;
            });

            console.log("📌 Created card for:", movie.title, "- Link: Movie/movie.html?id=" + movie.movieId);
            container.appendChild(card);
        });
        console.log("✅ All movie cards rendered");

    } catch (error) {
        console.error("Lỗi khi tải phim:", error);
        container.innerHTML = '<p style="color:#e50914;text-align:center;padding:40px;">Không thể tải danh sách phim. Vui lòng tải lại trang.</p>';
    }
}

function clearAuthState() {
    localStorage.removeItem("userId");
    localStorage.removeItem("username");
    localStorage.removeItem("role");
    sessionStorage.removeItem(POST_AUTH_REDIRECT_KEY);
}
