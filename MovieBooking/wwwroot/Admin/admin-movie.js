(function () {
    const userId = localStorage.getItem("userId");
    const role = localStorage.getItem("role");
    if (role !== "Admin") { window.location.href = "/auth.html"; return; }

    const API = "/api/admin/movies";
    const headers = { "Content-Type": "application/json", "X-User-Id": userId };

    const welcomeEl = document.getElementById("welcomeAdmin");
    welcomeEl.textContent = "Admin: " + (localStorage.getItem("username") || "");

    document.getElementById("logoutBtn").addEventListener("click", () => {
        localStorage.clear();
        window.location.href = "/auth.html";
    });

    const tbody = document.getElementById("movieTableBody");
    const modal = document.getElementById("movieModal");
    const confirmModalEl = document.getElementById("confirmModal");
    const form = document.getElementById("movieForm");
    let allMovies = [];
    let deleteId = null;

    // Load movies
    async function loadMovies() {
        try {
            const res = await fetch(API, { headers });
            if (res.status === 403) { alert("Không có quyền truy cập."); return; }
            allMovies = await res.json();
            renderTable(allMovies);
        } catch (err) {
            console.error("Error loading movies:", err);
        }
    }

    function renderTable(movies) {
        tbody.innerHTML = movies.length === 0
            ? '<tr><td colspan="7" class="empty-state">Chưa có phim nào.</td></tr>'
            : movies.map(m => `
                <tr>
                    <td><img src="${m.posterUrl || '/images/Logo.png'}" class="poster-thumb" alt="" style="width:45px;height:65px;object-fit:cover;"></td>
                    <td>${m.title}</td>
                    <td>${m.genre || '-'}</td>
                    <td>${m.releaseDate ? new Date(m.releaseDate).toLocaleDateString("vi-VN") : '-'}</td>
                    <td>${m.durationMinutes} phút</td>
                    <td class="${m.isActive ? 'status-active' : 'status-inactive'}">${m.isActive ? 'Hoạt động' : 'Ngưng'}</td>
                    <td>
                        <button class="btn-edit" data-id="${m.movieId}">Sửa</button>
                        <button class="btn-delete" data-id="${m.movieId}">${m.isActive ? 'Tắt' : 'Bật'}</button>
                    </td>
                </tr>
            `).join("");

        tbody.querySelectorAll(".btn-edit").forEach(btn =>
            btn.addEventListener("click", () => openEdit(parseInt(btn.dataset.id)))
        );
        tbody.querySelectorAll(".btn-delete").forEach(btn =>
            btn.addEventListener("click", () => openConfirmDelete(parseInt(btn.dataset.id)))
        );
    }

    // Search
    document.getElementById("searchBox").addEventListener("keyup", function () {
        const q = this.value.toLowerCase();
        renderTable(allMovies.filter(m => m.title.toLowerCase().includes(q)));
    });

    // Add
    document.getElementById("btnAdd").addEventListener("click", () => {
        form.reset();
        document.getElementById("movieId").value = "";
        document.getElementById("fIsActive").checked = true;
        document.getElementById("modalTitle").textContent = "Thêm Phim";
        modal.style.display = "flex";
    });

    // Edit
    function openEdit(id) {
        const m = allMovies.find(x => x.movieId === id);
        if (!m) return;
        document.getElementById("movieId").value = m.movieId;
        document.getElementById("fTitle").value = m.title;
        document.getElementById("fDescription").value = m.description || "";
        document.getElementById("fGenre").value = m.genre || "";
        document.getElementById("fDuration").value = m.durationMinutes;
        document.getElementById("fReleaseDate").value = m.releaseDate ? m.releaseDate.substring(0, 10) : "";
        document.getElementById("fAgeRestriction").value = m.ageRestriction || "";
        document.getElementById("fCast").value = m.cast || "";
        document.getElementById("fDirector").value = m.director || "";
        document.getElementById("fProducer").value = m.producer || "";
        document.getElementById("fPosterUrl").value = m.posterUrl || "";
        document.getElementById("fIsActive").checked = m.isActive;
        document.getElementById("modalTitle").textContent = "Sửa Phim";
        modal.style.display = "flex";
    }

    // Close modal
    document.getElementById("modalClose").addEventListener("click", () => modal.style.display = "none");
    modal.addEventListener("click", e => { if (e.target === modal) modal.style.display = "none"; });

    // Submit form
    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const id = document.getElementById("movieId").value;
        const body = {
            title: document.getElementById("fTitle").value,
            description: document.getElementById("fDescription").value || null,
            genre: document.getElementById("fGenre").value || null,
            durationMinutes: parseInt(document.getElementById("fDuration").value),
            releaseDate: document.getElementById("fReleaseDate").value || null,
            ageRestriction: document.getElementById("fAgeRestriction").value || null,
            cast: document.getElementById("fCast").value || null,
            director: document.getElementById("fDirector").value || null,
            producer: document.getElementById("fProducer").value || null,
            posterUrl: document.getElementById("fPosterUrl").value || "",
            isActive: document.getElementById("fIsActive").checked
        };

        try {
            const url = id ? `${API}/${id}` : API;
            const method = id ? "PUT" : "POST";
            const res = await fetch(url, { method, headers, body: JSON.stringify(body) });
            const data = await res.json();
            if (!res.ok) { alert(data.message || "Lỗi xảy ra."); return; }
            modal.style.display = "none";
            loadMovies();
        } catch (err) {
            alert("Lỗi kết nối.");
        }
    });

    // Delete / Toggle
    function openConfirmDelete(id) {
        deleteId = id;
        const m = allMovies.find(x => x.movieId === id);
        document.getElementById("confirmText").textContent = m && m.isActive
            ? `Bạn có muốn vô hiệu hóa phim "${m.title}"?`
            : `Bạn có muốn kích hoạt lại phim "${m.title}"?`;
        document.getElementById("btnConfirmDelete").textContent = m && m.isActive ? "Vô hiệu hóa" : "Kích hoạt";
        confirmModalEl.style.display = "flex";
    }

    document.getElementById("btnCancelDelete").addEventListener("click", () => confirmModalEl.style.display = "none");
    confirmModalEl.addEventListener("click", e => { if (e.target === confirmModalEl) confirmModalEl.style.display = "none"; });

    document.getElementById("btnConfirmDelete").addEventListener("click", async () => {
        if (!deleteId) return;
        try {
            const m = allMovies.find(x => x.movieId === deleteId);
            if (m && !m.isActive) {
                // Re-enable: use PUT
                const body = { title: m.title, description: m.description, genre: m.genre, durationMinutes: m.durationMinutes, releaseDate: m.releaseDate, ageRestriction: m.ageRestriction, cast: m.cast, director: m.director, producer: m.producer, posterUrl: m.posterUrl, isActive: true };
                await fetch(`${API}/${deleteId}`, { method: "PUT", headers, body: JSON.stringify(body) });
            } else {
                await fetch(`${API}/${deleteId}`, { method: "DELETE", headers });
            }
            confirmModalEl.style.display = "none";
            loadMovies();
        } catch (err) {
            alert("Lỗi kết nối.");
        }
    });

    loadMovies();
})();
