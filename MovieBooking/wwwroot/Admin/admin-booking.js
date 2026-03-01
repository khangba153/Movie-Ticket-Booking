(function () {
    const userId = localStorage.getItem("userId");
    const role = localStorage.getItem("role");
    if (role !== "Admin") { window.location.href = "/auth.html"; return; }

    const API = "/api/admin/bookings";
    const headers = { "Content-Type": "application/json", "X-User-Id": userId };

    document.getElementById("welcomeAdmin").textContent = "Admin: " + (localStorage.getItem("username") || "");
    document.getElementById("logoutBtn").addEventListener("click", () => { localStorage.clear(); window.location.href = "/auth.html"; });

    const tbody = document.getElementById("bookingTableBody");

    function fmtMoney(v) { return new Intl.NumberFormat("vi-VN").format(v) + " đ"; }
    function fmtDT(v) { return new Date(v).toLocaleString("vi-VN"); }

    // Load dropdowns for filter
    async function loadDropdowns() {
        try {
            const [mRes, cRes] = await Promise.all([
                fetch("/api/admin/movies", { headers }),
                fetch("/api/admin/cinemas", { headers })
            ]);
            const movies = await mRes.json();
            const cinemas = await cRes.json();

            const fm = document.getElementById("filterMovie");
            const fc = document.getElementById("filterCinema");
            movies.forEach(m => { fm.innerHTML += `<option value="${m.movieId}">${m.title}</option>`; });
            cinemas.forEach(c => { fc.innerHTML += `<option value="${c.cinemaId}">${c.name}</option>`; });
        } catch (err) { console.error(err); }
    }

    async function loadBookings() {
        const movieId = document.getElementById("filterMovie").value;
        const cinemaId = document.getElementById("filterCinema").value;
        const fromDate = document.getElementById("filterFromDate").value;
        const toDate = document.getElementById("filterToDate").value;

        let url = API + "?";
        if (movieId) url += `movieId=${movieId}&`;
        if (cinemaId) url += `cinemaId=${cinemaId}&`;
        if (fromDate) url += `fromDate=${fromDate}&`;
        if (toDate) url += `toDate=${toDate}&`;

        try {
            const res = await fetch(url, { headers });
            const data = await res.json();
            renderTable(data);
        } catch (err) { console.error(err); }
    }

    function renderTable(bookings) {
        tbody.innerHTML = bookings.length === 0
            ? '<tr><td colspan="8" class="empty-state">Không có đơn vé.</td></tr>'
            : bookings.map(b => `
                <tr>
                    <td>#${b.bookingId}</td>
                    <td>${b.username}</td>
                    <td>${b.movieTitle}</td>
                    <td>${b.cinemaName}</td>
                    <td>${fmtDT(b.startTime)}</td>
                    <td>${b.seats.join(", ")}</td>
                    <td class="money">${fmtMoney(b.totalPrice)}</td>
                    <td>${fmtDT(b.bookingDate)}</td>
                </tr>
            `).join("");
    }

    // Filters
    document.getElementById("filterMovie").addEventListener("change", loadBookings);
    document.getElementById("filterCinema").addEventListener("change", loadBookings);
    document.getElementById("filterFromDate").addEventListener("change", loadBookings);
    document.getElementById("filterToDate").addEventListener("change", loadBookings);

    loadDropdowns().then(loadBookings);
})();
