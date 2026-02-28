document.addEventListener("DOMContentLoaded", async function () {
    console.log("🎉 Success page loaded");

    const params = new URLSearchParams(window.location.search);
    const bookingId = params.get("bookingId");

    if (!bookingId) {
        document.querySelector('.success-card').innerHTML = '<p style="text-align:center;padding:40px;color:var(--fc-error)">Không tìm thấy mã xác nhận.</p><div class="button-group"><button class="btn btn-primary" onclick="window.location.href=\'/home.html\'">TRANG CHỦ</button></div>';
        return;
    }

    document.getElementById("bookingId").textContent = bookingId;

    try {
        // Fetch booking details
        const response = await fetch(`/api/booking/${bookingId}`);
        if (!response.ok) {
            throw new Error(`Failed to fetch booking: ${response.status}`);
        }

        const booking = await response.json();
        console.log("✅ Booking data:", booking);

        // Fetch showtime details
        const showtimeResponse = await fetch(`/api/showtime/${booking.showtimeId}`);
        if (!showtimeResponse.ok) {
            throw new Error(`Failed to fetch showtime: ${showtimeResponse.status}`);
        }

        const showtime = await showtimeResponse.json();
        console.log("✅ Showtime data:", showtime);

        // Fetch movie details
        const movieResponse = await fetch(`/api/movie/${showtime.movieId}`);
        if (!movieResponse.ok) {
            throw new Error(`Failed to fetch movie: ${movieResponse.status}`);
        }

        const movie = await movieResponse.json();
        console.log("✅ Movie data:", movie);

        // Display information
        displayBookingInfo(booking, showtime, movie);

    } catch (error) {
        console.error("❌ Error loading booking info:", error);
        document.querySelector('h1').textContent = 'Lỗi tải thông tin vé';
    }

    // Home button
    document.getElementById("homeBtn").addEventListener("click", () => {
        window.location.href = "/home.html";
    });

    // My Tickets button
    document.getElementById("downloadBtn").addEventListener("click", () => {
        window.location.href = "/MyTickets/mytickets.html";
    });
});

function displayBookingInfo(booking, showtime, movie) {
    // Movie title
    document.getElementById("movieTitle").textContent = movie.title || "-";

    // Showtime date and time
    const startTime = new Date(showtime.startTime);
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

    document.getElementById("showtimeDateTime").textContent = `${dateStr} ${timeStr}`;

    // Seat codes - extract from booking details
    if (booking.bookingDetails && booking.bookingDetails.length > 0) {
        const seatCodes = booking.bookingDetails
            .map(bd => bd.seat ? `${bd.seat.row}${bd.seat.number}` : '')
            .filter(code => code)
            .join(', ');
        document.getElementById("seatCodes").textContent = seatCodes || "-";
    } else {
        document.getElementById("seatCodes").textContent = "-";
    }

    // Total price
    const totalText = booking.totalPrice.toLocaleString("vi-VN") + " VND";
    document.getElementById("totalPrice").textContent = totalText;
}

async function fetchAndDisplaySeatCodes(bookingId) {
    // This function is no longer needed as we display seat codes directly from booking data
    // Keep it for backward compatibility
}
