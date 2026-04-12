const SEAT_PRICE = 150000; // VND per seat
const VAT_RATE = 0.08; // 8%
const PAYMENT_STORAGE_KEY = "pendingPayment";
const POST_AUTH_REDIRECT_KEY = "postAuthRedirect";

let tempBooking = null;
let showtimeData = null;
let movieData = null;
let loginRequiredModal = null;

document.addEventListener("DOMContentLoaded", async function () {
    console.log("📄 Bill page loaded");
    sessionStorage.removeItem(PAYMENT_STORAGE_KEY);

    loginRequiredModal = document.getElementById("loginRequiredModal");
    bindLoginRequiredModal();

    const savedBooking = localStorage.getItem("tempBooking");
    if (!savedBooking) {
        document.querySelector(".bill-card").innerHTML = '<p style="text-align:center;padding:40px;color:var(--fc-error)">Không tìm thấy dữ liệu đặt vé. Vui lòng quay lại trang chọn ghế.</p><div class="button-group"><button class="btn btn-secondary" onclick="window.history.back()">QUAY LẠI</button></div>';
        return;
    }

    tempBooking = JSON.parse(savedBooking);
    console.log("✅ Temporary booking loaded:", tempBooking);

    try {
        showtimeData = await fetchShowtimeData(tempBooking.showtimeId);
        movieData = await fetchMovieData(showtimeData.movieId);

        displayBillInfo();
        displayPricing();
        generateQRCode();
    } catch (error) {
        console.error("❌ Error loading bill data:", error);
        document.querySelector(".bill-card h1").textContent = "Lỗi tải thông tin vé";
    }

    document.getElementById("backBtn").addEventListener("click", () => {
        closeLoginRequiredModal();
        sessionStorage.removeItem(POST_AUTH_REDIRECT_KEY);
        localStorage.removeItem("tempBooking");
        window.history.back();
    });

    document.getElementById("bookBtn").addEventListener("click", continueToPayment);
});

async function fetchShowtimeData(showtimeId) {
    const response = await fetch(`/api/showtime/${showtimeId}`);
    if (!response.ok) {
        throw new Error(`Failed to fetch showtime: ${response.status}`);
    }

    return await response.json();
}

async function fetchMovieData(movieId) {
    const response = await fetch(`/api/movie/${movieId}`);
    if (!response.ok) {
        throw new Error(`Failed to fetch movie: ${response.status}`);
    }

    return await response.json();
}

function displayBillInfo() {
    document.getElementById("movieTitle").textContent = movieData.title || "-";

    const startTime = new Date(showtimeData.startTime);
    const dateStr = startTime.toLocaleDateString("vi-VN", {
        weekday: "short",
        year: "numeric",
        month: "2-digit",
        day: "2-digit"
    });
    const timeStr = startTime.toLocaleTimeString("vi-VN", {
        hour: "2-digit",
        minute: "2-digit"
    });

    document.getElementById("showtimeDate").textContent = dateStr;
    document.getElementById("showtimeTime").textContent = timeStr;

    const seatCount = tempBooking.seatIds.length;
    document.getElementById("seatCodes").textContent = `${tempBooking.seatCodes}`;
    document.querySelector("h1").textContent = `CHI TIẾT VÉ (${seatCount})`;
}

function displayPricing() {
    const seatCount = tempBooking.seatIds.length;
    const subTotal = seatCount * SEAT_PRICE;
    const vat = subTotal * VAT_RATE;
    const total = subTotal + vat;

    document.getElementById("seatPriceDisplay").textContent = `${SEAT_PRICE.toLocaleString("vi-VN")} VND x${seatCount}`;
    document.getElementById("vatDisplay").textContent = `${vat.toLocaleString("vi-VN")} VND`;
    document.getElementById("totalDisplay").textContent = `${total.toLocaleString("vi-VN")} VND`;

    tempBooking.seatPrice = SEAT_PRICE;
    tempBooking.calculatedTotal = total;
    tempBooking.subTotal = subTotal;
    tempBooking.vat = vat;
    localStorage.setItem("tempBooking", JSON.stringify(tempBooking));
}

function generateQRCode() {
    const qrData = `${movieData.title}|${tempBooking.seatCodes}|${tempBooking.calculatedTotal}`;

    try {
        const canvas = document.getElementById("qrCanvas");
        canvas.innerHTML = "";

        new QRCode(canvas, {
            text: qrData,
            width: 150,
            height: 150,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H
        });

        console.log("✅ QR code generated");
    } catch (error) {
        console.warn("⚠️ QR code generation failed:", error);
        document.getElementById("qrCanvas").innerHTML = "<p style='color:#999;font-size:12px;'>QR</p>";
    }
}

function continueToPayment() {
    console.log("💳 Preparing payment page...");

    try {
        if (!tempBooking || !showtimeData || !movieData) {
            throw new Error("Thông tin vé chưa sẵn sàng. Vui lòng thử lại.");
        }

        const activeUserId = getActiveUserId();
        const startTime = new Date(showtimeData.startTime);

        const paymentDraft = {
            userId: activeUserId || tempBooking.userId || null,
            showtimeId: tempBooking.showtimeId,
            seatIds: tempBooking.seatIds,
            seatCodes: tempBooking.seatCodes,
            seatCount: tempBooking.seatIds.length,
            seatPrice: tempBooking.seatPrice || SEAT_PRICE,
            subTotal: tempBooking.subTotal || tempBooking.seatIds.length * SEAT_PRICE,
            vat: tempBooking.vat || (tempBooking.seatIds.length * SEAT_PRICE * VAT_RATE),
            total: tempBooking.calculatedTotal || tempBooking.totalPrice || 0,
            movieTitle: movieData.title || "-",
            showtimeDate: startTime.toLocaleDateString("vi-VN", {
                weekday: "short",
                year: "numeric",
                month: "2-digit",
                day: "2-digit"
            }),
            showtimeTime: startTime.toLocaleTimeString("vi-VN", {
                hour: "2-digit",
                minute: "2-digit"
            })
        };

        paymentDraft.showtimeDateTime = `${paymentDraft.showtimeDate} ${paymentDraft.showtimeTime}`.trim();
        sessionStorage.setItem(PAYMENT_STORAGE_KEY, JSON.stringify(paymentDraft));
        localStorage.setItem("tempBooking", JSON.stringify({
            ...tempBooking,
            userId: paymentDraft.userId,
            seatPrice: paymentDraft.seatPrice,
            subTotal: paymentDraft.subTotal,
            vat: paymentDraft.vat,
            calculatedTotal: paymentDraft.total
        }));

        if (!activeUserId) {
            sessionStorage.setItem(POST_AUTH_REDIRECT_KEY, "/Payment/payment.html");
            openLoginRequiredModal();
            return;
        }

        sessionStorage.removeItem(POST_AUTH_REDIRECT_KEY);
        window.location.href = "/Payment/payment.html";
    } catch (error) {
        console.error("❌ Error preparing payment:", error);
        showBillError("Không thể chuyển sang trang thanh toán: " + error.message);
    }
}

function bindLoginRequiredModal() {
    if (!loginRequiredModal) {
        return;
    }

    document.getElementById("closeLoginModal").addEventListener("click", closeLoginRequiredModal);
    document.getElementById("cancelLoginModalBtn").addEventListener("click", closeLoginRequiredModal);
    document.getElementById("goToAuthBtn").addEventListener("click", goToAuthPage);

    loginRequiredModal.addEventListener("click", function (event) {
        if (event.target === loginRequiredModal) {
            closeLoginRequiredModal();
        }
    });
}

function openLoginRequiredModal() {
    if (loginRequiredModal) {
        loginRequiredModal.classList.add("is-open");
    }
}

function closeLoginRequiredModal() {
    if (loginRequiredModal) {
        loginRequiredModal.classList.remove("is-open");
    }
}

function goToAuthPage() {
    sessionStorage.setItem(POST_AUTH_REDIRECT_KEY, "/Payment/payment.html");
    window.location.href = "/auth.html";
}

function getActiveUserId() {
    const userId = parseInt(localStorage.getItem("userId"), 10);
    return Number.isInteger(userId) && userId > 0 ? userId : null;
}

function showBillError(message) {
    let errEl = document.getElementById("bookError");
    if (!errEl) {
        errEl = document.createElement("p");
        errEl.id = "bookError";
        errEl.style.cssText = "text-align:center;color:var(--fc-error);font-size:13px;margin-top:8px";
        document.querySelector(".button-group").after(errEl);
    }

    errEl.textContent = message;
}
