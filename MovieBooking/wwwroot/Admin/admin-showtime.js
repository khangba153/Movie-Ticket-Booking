(function () {
    const userId = localStorage.getItem("userId");
    const role = localStorage.getItem("role");
    if (role !== "Admin") { window.location.href = "/auth.html"; return; }

    const API = "/api/admin/showtimes";
    const headers = { "Content-Type": "application/json", "X-User-Id": userId };

    document.getElementById("welcomeAdmin").textContent = "Admin: " + (localStorage.getItem("username") || "");
    document.getElementById("logoutBtn").addEventListener("click", () => { localStorage.clear(); window.location.href = "/auth.html"; });

    const tbody = document.getElementById("showtimeTableBody");
    const modal = document.getElementById("showtimeModal");
    const confirmModalEl = document.getElementById("confirmModal");
    const form = document.getElementById("showtimeForm");
    let movies = [], cinemas = [];
    let deleteId = null;

    function fmtMoney(v) { return new Intl.NumberFormat("vi-VN").format(v) + " đ"; }
    function fmtDT(v) { return new Date(v).toLocaleString("vi-VN"); }

    async function loadDropdowns() {
        try {
            const [mRes, cRes] = await Promise.all([
                fetch("/api/admin/movies", { headers }),
                fetch("/api/admin/cinemas", { headers })
            ]);
            movies = await mRes.json();
            cinemas = await cRes.json();

            const filterMovie = document.getElementById("filterMovie");
            const filterCinema = document.getElementById("filterCinema");
            const fMovieId = document.getElementById("fMovieId");
            const fCinemaId = document.getElementById("fCinemaId");

            movies.forEach(m => {
                filterMovie.innerHTML += `<option value="${m.movieId}">${m.title}</option>`;
                fMovieId.innerHTML += `<option value="${m.movieId}">${m.title}</option>`;
            });
            cinemas.forEach(c => {
                filterCinema.innerHTML += `<option value="${c.cinemaId}">${c.name}</option>`;
                fCinemaId.innerHTML += `<option value="${c.cinemaId}">${c.name}</option>`;
            });
        } catch (err) { console.error(err); }
    }

    async function loadShowtimes() {
        const movieId = document.getElementById("filterMovie").value;
        const cinemaId = document.getElementById("filterCinema").value;
        const date = document.getElementById("filterDate").value;
        let url = API + "?";
        if (movieId) url += `movieId=${movieId}&`;
        if (cinemaId) url += `cinemaId=${cinemaId}&`;
        if (date) url += `date=${date}&`;

        try {
            const res = await fetch(url, { headers });
            const data = await res.json();
            renderTable(data);
        } catch (err) { console.error(err); }
    }

    function renderTable(showtimes) {
        tbody.innerHTML = showtimes.length === 0
            ? '<tr><td colspan="6" class="empty-state">Không có lịch chiếu.</td></tr>'
            : showtimes.map(s => `
                <tr>
                    <td>${s.movieTitle}</td>
                    <td>${s.cinemaName}</td>
                    <td>${fmtDT(s.startTime)}</td>
                    <td>${fmtDT(s.endTime)}</td>
                    <td class="money">${fmtMoney(s.price)}</td>
                    <td>
                        <button class="btn-edit" data-id="${s.showtimeId}" data-movie="${s.movieId}" data-cinema="${s.cinemaId}" data-start="${s.startTime}" data-price="${s.price}">Sửa</button>
                        <button class="btn-delete" data-id="${s.showtimeId}">Xóa</button>
                    </td>
                </tr>
            `).join("");

        tbody.querySelectorAll(".btn-edit").forEach(btn => btn.addEventListener("click", () => {
            document.getElementById("showtimeId").value = btn.dataset.id;
            document.getElementById("fMovieId").value = btn.dataset.movie;
            document.getElementById("fCinemaId").value = btn.dataset.cinema;
            // Format datetime-local
            const dt = new Date(btn.dataset.start);
            const local = new Date(dt.getTime() - dt.getTimezoneOffset() * 60000).toISOString().slice(0, 16);
            document.getElementById("fStartTime").value = local;
            document.getElementById("fPrice").value = btn.dataset.price;
            document.getElementById("modalTitle").textContent = "Sửa Lịch Chiếu";
            modal.style.display = "flex";
        }));

        tbody.querySelectorAll(".btn-delete").forEach(btn => btn.addEventListener("click", () => {
            deleteId = parseInt(btn.dataset.id);
            confirmModalEl.style.display = "flex";
        }));
    }

    // Filters
    document.getElementById("filterMovie").addEventListener("change", loadShowtimes);
    document.getElementById("filterCinema").addEventListener("change", loadShowtimes);
    document.getElementById("filterDate").addEventListener("change", loadShowtimes);

    // Add
    document.getElementById("btnAdd").addEventListener("click", () => {
        form.reset();
        document.getElementById("showtimeId").value = "";
        document.getElementById("modalTitle").textContent = "Thêm Lịch Chiếu";
        modal.style.display = "flex";
    });

    // Close
    document.getElementById("modalClose").addEventListener("click", () => modal.style.display = "none");
    modal.addEventListener("click", e => { if (e.target === modal) modal.style.display = "none"; });

    // Submit
    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const id = document.getElementById("showtimeId").value;
        const body = {
            movieId: parseInt(document.getElementById("fMovieId").value),
            cinemaId: parseInt(document.getElementById("fCinemaId").value),
            startTime: document.getElementById("fStartTime").value,
            price: parseFloat(document.getElementById("fPrice").value)
        };
        try {
            const url = id ? `${API}/${id}` : API;
            const method = id ? "PUT" : "POST";
            const res = await fetch(url, { method, headers, body: JSON.stringify(body) });
            const data = await res.json();
            if (!res.ok) { alert(data.message || "Lỗi."); return; }
            modal.style.display = "none";
            loadShowtimes();
        } catch (err) { alert("Lỗi kết nối."); }
    });

    // Delete
    document.getElementById("btnCancelDelete").addEventListener("click", () => confirmModalEl.style.display = "none");
    confirmModalEl.addEventListener("click", e => { if (e.target === confirmModalEl) confirmModalEl.style.display = "none"; });

    document.getElementById("btnConfirmDelete").addEventListener("click", async () => {
        if (!deleteId) return;
        try {
            const res = await fetch(`${API}/${deleteId}`, { method: "DELETE", headers });
            const data = await res.json();
            if (!res.ok) { alert(data.message || "Lỗi."); }
            confirmModalEl.style.display = "none";
            loadShowtimes();
        } catch (err) { alert("Lỗi kết nối."); }
    });

    loadDropdowns().then(loadShowtimes);
})();
