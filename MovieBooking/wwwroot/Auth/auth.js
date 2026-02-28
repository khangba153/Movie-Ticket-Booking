// ==============================
//  AUTH PAGE - FoxCinema
// ==============================

const container   = document.getElementById("authContainer");
const goRegister  = document.getElementById("goRegister");
const goLogin     = document.getElementById("goLogin");
const loginForm   = document.getElementById("loginForm");
const registerForm= document.getElementById("registerForm");
const loginMsg    = document.getElementById("loginMessage");
const registerMsg = document.getElementById("registerMessage");

// ── Toggle mode ──
goRegister.addEventListener("click", () => {
    container.classList.add("is-register");
    clearMessages();
});

goLogin.addEventListener("click", () => {
    container.classList.remove("is-register");
    clearMessages();
});

function clearMessages() {
    loginMsg.textContent    = "";
    loginMsg.className      = "message";
    registerMsg.textContent = "";
    registerMsg.className   = "message";
}

function showMsg(el, text, type) {
    el.textContent = text;
    el.className   = "message " + type;     // "error" | "success"
}

// ── LOGIN ──
loginForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    clearMessages();

    const username = document.getElementById("loginUsername").value.trim();
    const password = document.getElementById("loginPassword").value;

    if (!username || !password) {
        showMsg(loginMsg, "Vui lòng nhập đầy đủ thông tin.", "error");
        return;
    }

    const btn = document.getElementById("loginBtn");
    btn.disabled = true;
    btn.textContent = "Đang xử lý...";

    try {
        const res = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        const data = await res.json();

        if (!res.ok) {
            showMsg(loginMsg, data.message || "Đăng nhập thất bại.", "error");
            return;
        }

        // Save user info
        localStorage.setItem("userId", data.userId);
        localStorage.setItem("username", data.username);

        // Redirect to home
        window.location.href = "/home.html";

    } catch (err) {
        console.error("Login error:", err);
        showMsg(loginMsg, "Lỗi kết nối. Vui lòng thử lại.", "error");
    } finally {
        btn.disabled = false;
        btn.textContent = "Đăng nhập";
    }
});

// ── REGISTER ──
registerForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    clearMessages();

    const username = document.getElementById("regUsername").value.trim();
    const password = document.getElementById("regPassword").value;
    const confirm  = document.getElementById("regConfirm").value;

    // Frontend validation
    if (!username || !password || !confirm) {
        showMsg(registerMsg, "Vui lòng nhập đầy đủ thông tin.", "error");
        return;
    }

    if (password.length < 6) {
        showMsg(registerMsg, "Mật khẩu phải có ít nhất 6 ký tự.", "error");
        return;
    }

    if (password !== confirm) {
        showMsg(registerMsg, "Mật khẩu nhập lại không khớp.", "error");
        return;
    }

    const btn = document.getElementById("registerBtn");
    btn.disabled = true;
    btn.textContent = "Đang xử lý...";

    try {
        const res = await fetch("/api/auth/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        const data = await res.json();

        if (!res.ok) {
            showMsg(registerMsg, data.message || "Đăng ký thất bại.", "error");
            return;
        }

        // Success → switch to login mode
        registerForm.reset();
        container.classList.remove("is-register");
        showMsg(loginMsg, "Đăng ký thành công! Vui lòng đăng nhập.", "success");

    } catch (err) {
        console.error("Register error:", err);
        showMsg(registerMsg, "Lỗi kết nối. Vui lòng thử lại.", "error");
    } finally {
        btn.disabled = false;
        btn.textContent = "Đăng ký";
    }
});
