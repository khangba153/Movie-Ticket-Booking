const SEAT_PRICE = 150000; // VND per seat
const VAT_RATE = 0.08; // 8%

let tempBooking = null;
let showtimeData = null;
let movieData = null;

document.addEventListener("DOMContentLoaded", async function () {
    console.log("📄 Bill page loaded");

    // Retrieve temporary booking from localStorage
    const savedBooking = localStorage.getItem("tempBooking");
    if (!savedBooking) {
        document.querySelector('.bill-card').innerHTML = '<p style="text-align:center;padding:40px;color:var(--fc-error)">Không tìm thấy dữ liệu đặt vé. Vui lòng quay lại trang chọn ghế.</p><div class="button-group"><button class="btn btn-secondary" onclick="window.history.back()">QUAY LẠI</button></div>';
        return;
    }

    tempBooking = JSON.parse(savedBooking);
    console.log("✅ Temporary booking loaded:", tempBooking);

    try {
        // Fetch showtime data
        showtimeData = await fetchShowtimeData(tempBooking.showtimeId);
        console.log("✅ Showtime data:", showtimeData);

        // Fetch movie data
        movieData = await fetchMovieData(showtimeData.movieId);
        console.log("✅ Movie data:", movieData);

        // Display information
        displayBillInfo();
        displayPricing();
        generateQRCode();

    } catch (error) {
        console.error("❌ Error loading bill data:", error);
        document.querySelector('.bill-card h1').textContent = 'Lỗi tải thông tin vé';
    }

    // Back button
    document.getElementById("backBtn").addEventListener("click", () => {
        // Clear temporary booking and go back
        localStorage.removeItem("tempBooking");
        window.history.back();
    });

    // Book button
    document.getElementById("bookBtn").addEventListener("click", submitBooking);
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
    // Movie title
    document.getElementById("movieTitle").textContent = movieData.title || "-";

    // Showtime date and time
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

    // Seat codes
    const seatCount = tempBooking.seatIds.length;
    document.getElementById("seatCodes").textContent = `${tempBooking.seatCodes}`;

    // Update title to show seat count
    document.querySelector("h1").textContent = `CHI TIẾT VÉ (${seatCount})`;
}

function displayPricing() {
    const seatCount = tempBooking.seatIds.length;
    const subTotal = seatCount * SEAT_PRICE;
    const vat = subTotal * VAT_RATE;
    const total = subTotal + vat;

    // Display seat price breakdown
    const seatPriceText = `${SEAT_PRICE.toLocaleString("vi-VN")} VND x${seatCount}`;
    document.getElementById("seatPriceDisplay").textContent = seatPriceText;

    // Display VAT
    const vatText = `${vat.toLocaleString("vi-VN")} VND`;
    document.getElementById("vatDisplay").textContent = vatText;

    // Display total
    const totalText = `${total.toLocaleString("vi-VN")} VND`;
    document.getElementById("totalDisplay").textContent = totalText;

    // Store for later use
    tempBooking.calculatedTotal = total;
    tempBooking.subTotal = subTotal;
}

function generateQRCode() {
    const qrData = `${movieData.title}|${tempBooking.seatCodes}|${tempBooking.calculatedTotal}`;
    
    try {
        // Clear previous QR code
        const canvas = document.getElementById("qrCanvas");
        canvas.innerHTML = "";
        
        // Generate QR code
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
        // Fallback: display placeholder
        document.getElementById("qrCanvas").innerHTML = "<p style='color:#999;font-size:12px;'>QR</p>";
    }
}

async function submitBooking() {
    console.log("📤 Submitting booking...");
    
    const bookBtn = document.getElementById("bookBtn");
    bookBtn.disabled = true;
    bookBtn.textContent = "ĐANG XỬ LÝ...";

    try {
        const bookingPayload = {
            userId: tempBooking.userId || parseInt(localStorage.getItem("userId")) || 1,
            showtimeId: tempBooking.showtimeId,
            seatIds: tempBooking.seatIds,
            totalPrice: tempBooking.calculatedTotal
        };

        console.log("📡 Sending booking request:", bookingPayload);

        const response = await fetch("/api/booking", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(bookingPayload)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Booking failed: ${response.status} - ${errorText}`);
        }

        const booking = await response.json();
        console.log("✅ Booking created successfully:", booking);

        // Clear temporary booking
        localStorage.removeItem("tempBooking");

        // Redirect to success page
        window.location.href = `/Success/success.html?bookingId=${booking.bookingId}`;

    } catch (error) {
        console.error("❌ Error submitting booking:", error);
        // Show inline error
        let errEl = document.getElementById('bookError');
        if (!errEl) {
            errEl = document.createElement('p');
            errEl.id = 'bookError';
            errEl.style.cssText = 'text-align:center;color:var(--fc-error);font-size:13px;margin-top:8px';
            document.querySelector('.button-group').after(errEl);
        }
        errEl.textContent = 'Lỗi khi đặt vé: ' + error.message;
        
        bookBtn.disabled = false;
        bookBtn.textContent = "ĐẶT VÉ";
    }
}
