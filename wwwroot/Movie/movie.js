document.addEventListener("DOMContentLoaded", async function () {
    console.log("🎬 Movie detail page loaded");
    console.log("📍 Current URL:", window.location.href);

    const params = new URLSearchParams(window.location.search);
    const id = params.get("id");
    console.log("🔍 Movie ID from URL:", id);

    if (!id) {
        document.body.innerHTML = '<p style="text-align:center;margin-top:40px;color:var(--fc-error)">Không tìm thấy phim.</p>';
        return;
    }

    try {
        console.log("📡 Fetching movie data from: /api/movie/" + id);
        const response = await fetch(`/api/movie/${id}`);
        console.log("✅ API Response status:", response.status);

        if (!response.ok) {
            console.error("❌ Movie not found");
            document.body.innerHTML = '<p style="text-align:center;margin-top:40px;color:var(--fc-error)">Phim không tồn tại.</p>';
            return;
        }

        const movie = await response.json();
        console.log("✅ Movie data loaded:", movie);

        const titleEl = document.getElementById("title");
        titleEl.innerText = movie.title;
        titleEl.dataset.movieId = movie.movieId;
        selectedShowtime.movieId = parseInt(movie.movieId, 10);
        document.getElementById("poster").src = movie.posterUrl;
        document.getElementById("description").innerText = movie.description;
        document.getElementById("genre").innerText = movie.genre;
        document.getElementById("releaseDate").innerText =
            new Date(movie.releaseDate).toLocaleDateString();
        document.getElementById("duration").innerText =
            movie.durationMinutes + " phút";
        document.getElementById("age").innerText = movie.ageRestriction;
        document.getElementById("cast").innerText = movie.cast;
        document.getElementById("director").innerText = movie.director;
        document.getElementById("producer").innerText = movie.producer;
        console.log("✅ All movie details rendered successfully");

        renderDates(id);
        await loadShowtimesByDate(id, new Date());

    } catch (error) {
        console.error("❌ Lỗi tải chi tiết phim:", error);
    }
});

const selectedShowtime = {
    movieId: null,
    cinemaId: null,
    showtimeId: null
};

function renderDates(movieId) {
    const container = document.getElementById("dateSelector");
    container.innerHTML = "";

    for (let i = 0; i < 7; i++) {
        const date = new Date();
        date.setDate(date.getDate() + i);

        const button = document.createElement("button");
        button.className = "date-btn";
        button.innerText = date.getDate() + "/" + (date.getMonth() + 1);

        button.onclick = () => {
            const buttons = container.querySelectorAll(".date-btn");
            buttons.forEach(b => b.classList.remove("active"));
            button.classList.add("active");
            loadShowtimesByDate(movieId, date);
        };

        if (i === 0) {
            button.classList.add("active");
        }

        container.appendChild(button);
    }
}

async function loadShowtimesByDate(movieId, date) {
    try {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const day = String(date.getDate()).padStart(2, "0");
        const formatted = `${year}-${month}-${day}`;
        console.log(`📡 Fetching showtimes for movie ${movieId} on ${formatted}`);
        const response = await fetch(`/api/showtime/movie/${movieId}/date/${formatted}`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const showtimes = await response.json();
        console.log("✅ Showtimes loaded:", showtimes);
        renderShowtimes(showtimes);

    } catch (error) {
        console.error("❌ Lỗi tải lịch chiếu:", error);
        const container = document.getElementById("cinemaContainer");
        container.innerHTML = "<p>Không thể tải lịch chiếu. Vui lòng tải lại trang.</p>";
    }
}

function renderShowtimes(showtimes) {
    const container = document.getElementById("cinemaContainer");
    container.innerHTML = "";

    if (!showtimes || showtimes.length === 0) {
        container.innerHTML = "<p>Chưa có lịch chiếu.</p>";
        console.log("⚠️ No showtimes available");
        return;
    }

    // Group showtimes by cinema
    const grouped = {};

    showtimes.forEach(s => {
        const cinemaId = s.cinema ? s.cinema.cinemaId : "unknown";

        if (!grouped[cinemaId]) {
            grouped[cinemaId] = {
                cinema: s.cinema,
                items: []
            };
        }

        grouped[cinemaId].items.push(s);
    });

    console.log("📊 Grouped showtimes by cinema:", grouped);

    // Render cinema sections
    Object.values(grouped).forEach((group, index) => {
        const cinemaName = group.cinema ? group.cinema.name : "Rạp không xác định";
        const cinemaAddress = group.cinema ? group.cinema.address : "";

        const wrapper = document.createElement("div");
        wrapper.className = "cinema-card";
        if (index > 0) {
            wrapper.classList.add("collapsed");
        }

        const sorted = group.items
            .slice()
            .sort((a, b) => new Date(a.startTime) - new Date(b.startTime));
        const renderButtons = (items) => items
            .map(s => {
                const startTime = new Date(s.startTime).toLocaleTimeString("vi-VN", {
                    hour: "2-digit",
                    minute: "2-digit"
                });
                return `
                    <button class="showtime-btn"
                        data-showtime-id="${s.showtimeId}"
                        data-cinema-id="${s.cinemaId}">
                        ${startTime}
                    </button>`;
            })
            .join("");

        wrapper.innerHTML = `
            <button class="cinema-toggle" aria-expanded="${index === 0}">
                <div class="cinema-header">
                    <div class="cinema-logo">M</div>
                    <div class="cinema-meta">
                        <div class="cinema-name">${cinemaName}</div>
                        <div class="cinema-address">${cinemaAddress}</div>
                    </div>
                </div>
                <span class="chevron"></span>
            </button>
            <div class="cinema-body">
                <div class="format-group">
                    <div class="format-title">Suất chiếu</div>
                    <div class="showtime-grid">
                        ${renderButtons(sorted)}
                    </div>
                </div>
                <div class="cinema-actions">
                    <button class="book-btn" disabled>Đặt vé</button>
                </div>
            </div>
        `;

        const toggle = wrapper.querySelector(".cinema-toggle");
        toggle.addEventListener("click", () => {
            wrapper.classList.toggle("collapsed");
            const expanded = !wrapper.classList.contains("collapsed");
            toggle.setAttribute("aria-expanded", expanded.toString());
        });

        const buttons = wrapper.querySelectorAll(".showtime-btn");
        buttons.forEach(btn => {
            btn.addEventListener("click", () => {
                const allButtons = container.querySelectorAll(".showtime-btn");
                allButtons.forEach(b => b.classList.remove("selected"));
                btn.classList.add("selected");

                selectedShowtime.cinemaId = parseInt(btn.dataset.cinemaId, 10);
                selectedShowtime.showtimeId = parseInt(btn.dataset.showtimeId, 10);

                const bookBtn = wrapper.querySelector(".book-btn");
                bookBtn.disabled = false;
            });
        });

        wrapper.querySelector(".book-btn").addEventListener("click", () => {
            if (selectedShowtime.showtimeId) {
                window.location.href = `/Seat/seat.html?showtimeId=${selectedShowtime.showtimeId}`;
            }
        });

        container.appendChild(wrapper);
    });

    console.log("✅ Showtimes rendered successfully");
}