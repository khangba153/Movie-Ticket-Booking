const STORAGE_KEYS = {
    tempBooking: "tempBooking",
    pendingPayment: "pendingPayment",
    latestPaymentResult: "latestPaymentResult"
};

const DEFAULT_SEAT_PRICE = 150000;
const VAT_RATE = 0.08;

const PAYMENT_METHODS = [
    { key: "momo", name: "Momo", detail: "Ví điện tử nhanh gọn", badge: "Mo", accent: "linear-gradient(135deg, #ff5f8f, #d81b60)" },
    { key: "vnpay", name: "VNPay", detail: "Quét mã hoặc ví liên kết", badge: "VN", accent: "linear-gradient(135deg, #2563eb, #1d4ed8)" },
    { key: "zalopay", name: "ZaloPay", detail: "Thanh toán không tiền mặt", badge: "Za", accent: "linear-gradient(135deg, #06b6d4, #2563eb)" },
    { key: "atm", name: "ATM", detail: "Thẻ nội địa và Internet Banking", badge: "ATM", accent: "linear-gradient(135deg, #6b7280, #374151)" },
    { key: "visa-master", name: "Visa/MasterCard", detail: "Thẻ tín dụng hoặc ghi nợ", badge: "VM", accent: "linear-gradient(135deg, #f59e0b, #ef4444)" },
    { key: "counter", name: "Pay at counter", detail: "Giữ vé và thanh toán tại quầy", badge: "QT", accent: "linear-gradient(135deg, #fb7185, #f59e0b)" }
];

let paymentDraft = null;
let selectedMethodKey = "";
let completedBookingId = null;

document.addEventListener("DOMContentLoaded", async function () {
    bindEvents();
    renderPaymentMethods();

    try {
        paymentDraft = await loadPaymentDraft();
        if (!paymentDraft) {
            renderEmptyState();
            return;
        }

        renderSummary(paymentDraft);
    } catch (error) {
        console.error("❌ Error loading payment page:", error);
        renderLoadError(error);
    }
});

function bindEvents() {
    document.getElementById("backBtn").addEventListener("click", handleBack);
    document.getElementById("payNowBtn").addEventListener("click", handlePayment);
    document.getElementById("viewTicketBtn").addEventListener("click", function () {
        if (!completedBookingId) {
            return;
        }

        window.location.href = `/Success/success.html?bookingId=${completedBookingId}`;
    });
    document.getElementById("homeBtn").addEventListener("click", function () {
        window.location.href = "/home.html";
    });
}

function renderPaymentMethods() {
    const methodGrid = document.getElementById("methodGrid");
    methodGrid.innerHTML = PAYMENT_METHODS.map(method => `
        <button class="method-option" type="button" data-method="${method.key}" aria-pressed="false">
            <span class="method-badge" style="background:${method.accent}">${method.badge}</span>
            <span class="method-content">
                <span class="method-name">${method.name}</span>
                <span class="method-detail">${method.detail}</span>
            </span>
        </button>
    `).join("");

    methodGrid.querySelectorAll(".method-option").forEach(button => {
        button.addEventListener("click", function () {
            selectPaymentMethod(button.dataset.method || "");
        });
    });
}

function selectPaymentMethod(methodKey) {
    selectedMethodKey = methodKey;

    document.querySelectorAll(".method-option").forEach(button => {
        const isSelected = button.dataset.method === methodKey;
        button.classList.toggle("is-selected", isSelected);
        button.setAttribute("aria-pressed", isSelected ? "true" : "false");
    });

    const method = getSelectedMethod();
    const selectedMethodDisplay = document.getElementById("selectedMethodDisplay");
    selectedMethodDisplay.textContent = method ? method.name : "Chưa chọn";
    selectedMethodDisplay.classList.toggle("value-muted", !method);
    clearMessage();
}

async function loadPaymentDraft() {
    const storedDraft = readJson(sessionStorage, STORAGE_KEYS.pendingPayment);
    if (storedDraft) {
        return normalizePaymentDraft(storedDraft);
    }

    const tempBooking = readJson(localStorage, STORAGE_KEYS.tempBooking);
    if (!tempBooking) {
        return null;
    }

    const showtimeData = await fetchShowtimeData(tempBooking.showtimeId);
    const movieData = await fetchMovieData(showtimeData.movieId);
    const draft = buildPaymentDraft(tempBooking, showtimeData, movieData);
    sessionStorage.setItem(STORAGE_KEYS.pendingPayment, JSON.stringify(draft));
    return draft;
}

function buildPaymentDraft(tempBooking, showtimeData, movieData) {
    const seatIds = Array.isArray(tempBooking.seatIds) ? tempBooking.seatIds : [];
    const seatCount = seatIds.length;
    const seatPrice = Number(tempBooking.seatPrice || DEFAULT_SEAT_PRICE);
    const subTotal = Number(tempBooking.subTotal || (seatCount * seatPrice));
    const vat = Number(tempBooking.vat || (subTotal * VAT_RATE));
    const total = Number(tempBooking.calculatedTotal || tempBooking.total || (subTotal + vat));
    const startTime = new Date(showtimeData.startTime);

    return normalizePaymentDraft({
        ...tempBooking,
        seatIds,
        seatCount,
        seatPrice,
        subTotal,
        vat,
        total,
        movieTitle: movieData.title || "-",
        showtimeDate: formatDate(startTime),
        showtimeTime: formatTime(startTime),
        showtimeDateTime: `${formatDate(startTime)} ${formatTime(startTime)}`.trim()
    });
}

function normalizePaymentDraft(draft) {
    const seatIds = Array.isArray(draft.seatIds) ? draft.seatIds : [];
    const seatCount = draft.seatCount || seatIds.length;
    const seatPrice = Number(draft.seatPrice || DEFAULT_SEAT_PRICE);
    const subTotal = Number(draft.subTotal || (seatCount * seatPrice));
    const vat = Number(draft.vat || (subTotal * VAT_RATE));
    const total = Number(draft.total || draft.calculatedTotal || (subTotal + vat));

    return {
        ...draft,
        seatIds,
        seatCount,
        seatPrice,
        subTotal,
        vat,
        total,
        seatCodes: formatSeatCodes(draft.seatCodes || ""),
        movieTitle: draft.movieTitle || "-",
        showtimeDate: draft.showtimeDate || "-",
        showtimeTime: draft.showtimeTime || "-",
        showtimeDateTime: draft.showtimeDateTime || [draft.showtimeDate, draft.showtimeTime].filter(Boolean).join(" ").trim()
    };
}

function renderSummary(draft) {
    setText("summaryMovie", draft.movieTitle);
    setText("summaryDate", draft.showtimeDate);
    setText("summaryTime", draft.showtimeTime);
    setText("summarySeats", draft.seatCodes || "-");
    setText("summarySeatPrice", `${formatCurrency(draft.seatPrice)} x${draft.seatCount}`);
    setText("summarySubTotal", formatCurrency(draft.subTotal));
    setText("summaryVat", formatCurrency(draft.vat));
    setText("summaryTotal", formatCurrency(draft.total));
}

function handleBack() {
    sessionStorage.removeItem(STORAGE_KEYS.pendingPayment);

    if (window.history.length > 1) {
        window.history.back();
        return;
    }

    window.location.href = "/Bill/bill.html";
}

async function handlePayment() {
    if (!paymentDraft) {
        showMessage("Không tìm thấy thông tin thanh toán. Vui lòng quay lại bước trước.", "error");
        return;
    }

    const activeUserId = paymentDraft.userId || getActiveUserId();
    if (!activeUserId) {
        showMessage("Vui lòng đăng nhập để thanh toán và hoàn tất đặt vé.", "error");
        return;
    }

    const selectedMethod = getSelectedMethod();
    if (!selectedMethod) {
        showMessage("Vui lòng chọn một phương thức thanh toán để tiếp tục nhé.", "error");
        return;
    }

    showMessage(`Đang xử lý thanh toán qua ${selectedMethod.name}...`, "processing");
    setProcessingState(true);

    const startedAt = Date.now();

    try {
        const booking = await createBooking();
        await ensureMinimumDelay(startedAt, 2000);

        const paymentResult = {
            bookingId: booking.bookingId,
            paymentMethod: selectedMethod.name,
            transactionCode: generateTransactionCode(selectedMethod.key),
            paymentTime: formatPaymentTimestamp(new Date()),
            amount: paymentDraft.total
        };

        completedBookingId = booking.bookingId;
        sessionStorage.setItem(STORAGE_KEYS.latestPaymentResult, JSON.stringify(paymentResult));
        sessionStorage.removeItem(STORAGE_KEYS.pendingPayment);
        localStorage.removeItem(STORAGE_KEYS.tempBooking);

        showSuccessState(paymentResult);
    } catch (error) {
        await ensureMinimumDelay(startedAt, 2000);
        console.error("❌ Payment simulation failed:", error);
        showMessage(resolveErrorMessage(error), "error");
        setProcessingState(false);
    }
}

async function createBooking() {
    const payload = {
        userId: paymentDraft.userId || getActiveUserId(),
        showtimeId: paymentDraft.showtimeId,
        seatIds: paymentDraft.seatIds
    };

    if (!payload.userId) {
        throw new Error("Vui lòng đăng nhập để thanh toán và hoàn tất đặt vé.");
    }

    const response = await fetch("/api/booking", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
    });

    if (!response.ok) {
        const errorText = (await response.text()).trim();
        throw new Error(errorText || `Không thể tạo booking (${response.status}).`);
    }

    return await response.json();
}

function showSuccessState(paymentResult) {
    document.getElementById("paymentFlow").hidden = true;
    document.getElementById("paymentSuccess").hidden = false;

    setText("resultMethod", paymentResult.paymentMethod);
    setText("resultTransactionCode", paymentResult.transactionCode);
    setText("resultPaymentTime", paymentResult.paymentTime);
    setText("resultBookingId", `#${paymentResult.bookingId}`);
    setText("resultAmount", formatCurrency(paymentResult.amount));

    clearMessage();
}

function setProcessingState(isProcessing) {
    const payNowBtn = document.getElementById("payNowBtn");
    const backBtn = document.getElementById("backBtn");

    payNowBtn.disabled = isProcessing;
    backBtn.disabled = isProcessing;
    payNowBtn.textContent = isProcessing ? "ĐANG XỬ LÝ..." : "THANH TOÁN NGAY";
}

function showMessage(message, type) {
    const paymentMessage = document.getElementById("paymentMessage");
    paymentMessage.textContent = message;
    paymentMessage.classList.remove("fc-msg-error", "fc-msg-success", "is-processing");

    if (type === "error") {
        paymentMessage.classList.add("fc-msg-error");
    } else if (type === "success") {
        paymentMessage.classList.add("fc-msg-success");
    } else if (type === "processing") {
        paymentMessage.classList.add("is-processing");
    }
}

function clearMessage() {
    const paymentMessage = document.getElementById("paymentMessage");
    paymentMessage.textContent = "";
    paymentMessage.classList.remove("fc-msg-error", "fc-msg-success", "is-processing");
}

function getSelectedMethod() {
    return PAYMENT_METHODS.find(method => method.key === selectedMethodKey) || null;
}

function generateTransactionCode(methodKey) {
    const now = new Date();
    const datePart = [
        String(now.getFullYear()).slice(-2),
        String(now.getMonth() + 1).padStart(2, "0"),
        String(now.getDate()).padStart(2, "0")
    ].join("");
    const methodPart = methodKey.replace(/[^a-z0-9]/gi, "").toUpperCase().slice(0, 4).padEnd(4, "X");
    const randomPart = Math.random().toString(36).slice(2, 8).toUpperCase();
    return `FC-${methodPart}-${datePart}-${randomPart}`;
}

function resolveErrorMessage(error) {
    if (error instanceof Error && error.message) {
        return `Thanh toán chưa hoàn tất: ${error.message}`;
    }

    return "Thanh toán chưa hoàn tất. Vui lòng thử lại.";
}

function renderEmptyState() {
    document.querySelector(".payment-card").innerHTML = `
        <div class="page-header">
            <h1>THANH TOÁN VÉ</h1>
            <p class="page-subtitle">Không tìm thấy thông tin đặt vé để tiếp tục thanh toán.</p>
        </div>
        <div class="section-card">
            <p style="text-align:center;color:var(--fc-text-muted);line-height:1.8;">Vui lòng quay lại trang chi tiết vé hoặc chọn ghế lại từ đầu.</p>
            <div class="button-group">
                <button class="btn btn-secondary" type="button" onclick="window.history.back()">QUAY LẠI</button>
                <button class="btn btn-primary" type="button" onclick="window.location.href='/home.html'">TRANG CHỦ</button>
            </div>
        </div>
    `;
}

function renderLoadError(error) {
    document.querySelector(".payment-card").innerHTML = `
        <div class="page-header">
            <h1>THANH TOÁN VÉ</h1>
            <p class="page-subtitle">Không thể tải thông tin thanh toán.</p>
        </div>
        <div class="section-card">
            <p style="text-align:center;color:var(--fc-error);line-height:1.8;">${escapeHtml(resolveErrorMessage(error))}</p>
            <div class="button-group">
                <button class="btn btn-secondary" type="button" onclick="window.history.back()">QUAY LẠI</button>
                <button class="btn btn-primary" type="button" onclick="window.location.reload()">THỬ LẠI</button>
            </div>
        </div>
    `;
}

async function fetchShowtimeData(showtimeId) {
    const response = await fetch(`/api/showtime/${showtimeId}`);
    if (!response.ok) {
        throw new Error(`Không thể tải suất chiếu (${response.status}).`);
    }
    return await response.json();
}

async function fetchMovieData(movieId) {
    const response = await fetch(`/api/movie/${movieId}`);
    if (!response.ok) {
        throw new Error(`Không thể tải thông tin phim (${response.status}).`);
    }
    return await response.json();
}

function readJson(storage, key) {
    try {
        const rawValue = storage.getItem(key);
        return rawValue ? JSON.parse(rawValue) : null;
    } catch (error) {
        console.warn(`⚠️ Could not parse storage key "${key}":`, error);
        return null;
    }
}

function getActiveUserId() {
    const userId = parseInt(localStorage.getItem("userId"), 10);
    return Number.isInteger(userId) && userId > 0 ? userId : null;
}

function setText(elementId, value) {
    const element = document.getElementById(elementId);
    if (element) {
        element.textContent = value;
    }
}

function formatCurrency(value) {
    return `${Number(value || 0).toLocaleString("vi-VN")} VND`;
}

function formatSeatCodes(seatCodes) {
    return String(seatCodes || "")
        .split(",")
        .map(code => code.trim())
        .filter(Boolean)
        .join(", ");
}

function formatDate(date) {
    return date.toLocaleDateString("vi-VN", {
        weekday: "short",
        year: "numeric",
        month: "2-digit",
        day: "2-digit"
    });
}

function formatTime(date) {
    return date.toLocaleTimeString("vi-VN", {
        hour: "2-digit",
        minute: "2-digit"
    });
}

function formatPaymentTimestamp(date) {
    return date.toLocaleString("vi-VN", {
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        day: "2-digit",
        month: "2-digit",
        year: "numeric"
    });
}

function ensureMinimumDelay(startedAt, durationMs) {
    const remaining = durationMs - (Date.now() - startedAt);
    if (remaining <= 0) {
        return Promise.resolve();
    }

    return new Promise(resolve => {
        window.setTimeout(resolve, remaining);
    });
}

function escapeHtml(text) {
    return String(text || "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#39;");
}

