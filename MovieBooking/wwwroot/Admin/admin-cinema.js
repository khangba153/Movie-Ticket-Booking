(function () {
    const userId = localStorage.getItem("userId");
    const role = localStorage.getItem("role");
    if (role !== "Admin") { window.location.href = "/auth.html"; return; }

    const API = "/api/admin/cinemas";
    const headers = { "Content-Type": "application/json", "X-User-Id": userId };

    document.getElementById("welcomeAdmin").textContent = "Admin: " + (localStorage.getItem("username") || "");
    document.getElementById("logoutBtn").addEventListener("click", () => { localStorage.clear(); window.location.href = "/auth.html"; });

    const tbody = document.getElementById("cinemaTableBody");
    const modal = document.getElementById("cinemaModal");
    const confirmModalEl = document.getElementById("confirmModal");
    const form = document.getElementById("cinemaForm");
    let allCinemas = [];
    let deleteId = null;

    async function loadCinemas() {
        try {
            const res = await fetch(API, { headers });
            allCinemas = await res.json();
            renderTable(allCinemas);
        } catch (err) { console.error(err); }
    }

    function renderTable(cinemas) {
        tbody.innerHTML = cinemas.length === 0
            ? '<tr><td colspan="4" class="empty-state">Chưa có rạp nào.</td></tr>'
            : cinemas.map(c => `
                <tr>
                    <td>${c.cinemaId}</td>
                    <td>${c.name}</td>
                    <td>${c.address}</td>
                    <td>
                        <button class="btn-edit" data-id="${c.cinemaId}">Sửa</button>
                        <button class="btn-delete" data-id="${c.cinemaId}">Xóa</button>
                    </td>
                </tr>
            `).join("");

        tbody.querySelectorAll(".btn-edit").forEach(btn => btn.addEventListener("click", () => openEdit(parseInt(btn.dataset.id))));
        tbody.querySelectorAll(".btn-delete").forEach(btn => btn.addEventListener("click", () => { deleteId = parseInt(btn.dataset.id); confirmModalEl.style.display = "flex"; }));
    }

    document.getElementById("searchBox").addEventListener("keyup", function () {
        const q = this.value.toLowerCase();
        renderTable(allCinemas.filter(c => c.name.toLowerCase().includes(q) || c.address.toLowerCase().includes(q)));
    });

    document.getElementById("btnAdd").addEventListener("click", () => {
        form.reset();
        document.getElementById("cinemaId").value = "";
        document.getElementById("modalTitle").textContent = "Thêm Rạp";
        modal.style.display = "flex";
    });

    function openEdit(id) {
        const c = allCinemas.find(x => x.cinemaId === id);
        if (!c) return;
        document.getElementById("cinemaId").value = c.cinemaId;
        document.getElementById("fName").value = c.name;
        document.getElementById("fAddress").value = c.address;
        document.getElementById("modalTitle").textContent = "Sửa Rạp";
        modal.style.display = "flex";
    }

    document.getElementById("modalClose").addEventListener("click", () => modal.style.display = "none");
    modal.addEventListener("click", e => { if (e.target === modal) modal.style.display = "none"; });

    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const id = document.getElementById("cinemaId").value;
        const body = { name: document.getElementById("fName").value, address: document.getElementById("fAddress").value };
        try {
            const url = id ? `${API}/${id}` : API;
            const method = id ? "PUT" : "POST";
            const res = await fetch(url, { method, headers, body: JSON.stringify(body) });
            const data = await res.json();
            if (!res.ok) { alert(data.message || "Lỗi."); return; }
            modal.style.display = "none";
            loadCinemas();
        } catch (err) { alert("Lỗi kết nối."); }
    });

    document.getElementById("btnCancelDelete").addEventListener("click", () => confirmModalEl.style.display = "none");
    confirmModalEl.addEventListener("click", e => { if (e.target === confirmModalEl) confirmModalEl.style.display = "none"; });

    document.getElementById("btnConfirmDelete").addEventListener("click", async () => {
        if (!deleteId) return;
        try {
            const res = await fetch(`${API}/${deleteId}`, { method: "DELETE", headers });
            const data = await res.json();
            if (!res.ok) { alert(data.message || "Lỗi."); }
            confirmModalEl.style.display = "none";
            loadCinemas();
        } catch (err) { alert("Lỗi kết nối."); }
    });

    loadCinemas();
})();
