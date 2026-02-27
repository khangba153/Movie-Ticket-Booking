const SEAT_PRICE = 150000; // VND

let showtimeId = null;
let allSeats = [];
let selectedSeats = []; // Array of seatIds

document.addEventListener("DOMContentLoaded", async function () {
    console.log("🎬 Seat selection page loaded");

    const params = new URLSearchParams(window.location.search);
    showtimeId = parseInt(params.get("showtimeId"), 10);
    console.log("📍 ShowtimeId:", showtimeId);

    if (!showtimeId) {
        document.querySelector('.seat-selection').innerHTML = '<p style="text-align:center;padding:40px;color:var(--fc-error)">Không tìm thấy suất chiếu.</p>';
        return;
    }

    try {
        // Load seats
        const response = await fetch(`/api/seat/showtime/${showtimeId}`);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        allSeats = await response.json();
        console.log("✅ Seats loaded:", allSeats);
        renderSeats(allSeats);

    } catch (error) {
        console.error("❌ Error loading seats:", error);
        document.querySelector('.seat-selection').innerHTML = '<p style="text-align:center;padding:40px;color:var(--fc-error)">Lỗi tải danh sách ghế. Vui lòng thử lại.</p>';
    }

    // Confirm button handler
    document.getElementById("confirmBtn").addEventListener("click", () => {
        if (selectedSeats.length === 0) {
            return;
        }

        // Save temporary booking data to localStorage
        const totalPrice = selectedSeats.length * SEAT_PRICE;
        const seatCodes = selectedSeats.map(s => s.seatCode).join(",");
        
        const tempBooking = {
            userId: parseInt(localStorage.getItem("userId")) || 1,
            showtimeId: showtimeId,
            seatIds: selectedSeats.map(s => s.seatId),
            seatCodes: seatCodes,
            totalPrice: totalPrice
        };
        
        console.log("💾 Saving temporary booking to localStorage:", tempBooking);
        localStorage.setItem("tempBooking", JSON.stringify(tempBooking));
        
        // Redirect to bill page
        window.location.href = `/Bill/bill.html`;
    });
});

function renderSeats(seats) {
    const container = document.getElementById("seatsGrid");
    container.innerHTML = "";

    // Group seats by row
    const seatsByRow = {};
    seats.forEach(seat => {
        if (!seatsByRow[seat.row]) {
            seatsByRow[seat.row] = [];
        }
        seatsByRow[seat.row].push(seat);
    });

    // Render each row
    Object.keys(seatsByRow).sort().forEach(row => {
        const rowSeats = seatsByRow[row];
        const rowContainer = document.createElement("div");
        rowContainer.className = "seats-row";

        // Split into groups: 1-3, gap, 4-6, gap, 7-9
        const groups = [
            rowSeats.filter(s => s.number >= 1 && s.number <= 3),
            rowSeats.filter(s => s.number >= 4 && s.number <= 6),
            rowSeats.filter(s => s.number >= 7 && s.number <= 9)
        ];

        groups.forEach((group, groupIdx) => {
            if (groupIdx > 0) {
                const gap = document.createElement("div");
                gap.className = "seat-gap";
                rowContainer.appendChild(gap);
            }

            group.forEach(seat => {
                const seatBtn = document.createElement("button");
                seatBtn.className = "seat-btn";
                seatBtn.textContent = seat.seatCode;
                seatBtn.dataset.seatId = seat.seatId;
                seatBtn.dataset.seatCode = seat.seatCode;

                if (seat.isBooked) {
                    seatBtn.classList.add("booked");
                    seatBtn.disabled = true;
                    seatBtn.title = "Ghế đã được đặt";
                } else {
                    seatBtn.classList.add("available");
                    seatBtn.addEventListener("click", () => toggleSeat(seat));
                }

                rowContainer.appendChild(seatBtn);
            });
        });

        container.appendChild(rowContainer);
    });

    console.log("✅ Seats rendered");
}

function toggleSeat(seat) {
    const seatBtn = document.querySelector(`[data-seat-id="${seat.seatId}"]`);

    // Check if already selected
    const existingIndex = selectedSeats.findIndex(s => s.seatId === seat.seatId);

    if (existingIndex >= 0) {
        // Deselect
        selectedSeats.splice(existingIndex, 1);
        seatBtn.classList.remove("selected");
    } else {
        // Select
        selectedSeats.push({
            seatId: seat.seatId,
            seatCode: seat.seatCode
        });
        seatBtn.classList.add("selected");
    }

    updateBookingPanel();
}

function updateBookingPanel() {
    // Update total price
    const totalPrice = selectedSeats.length * SEAT_PRICE;
    document.getElementById("totalPrice").textContent = totalPrice.toLocaleString("vi-VN") + " VND";

    // Update selected seats list
    const selectedSeatsContainer = document.getElementById("selectedSeats");
    selectedSeatsContainer.innerHTML = "";

    selectedSeats.forEach(seat => {
        const seatTag = document.createElement("span");
        seatTag.className = "seat-tag";
        seatTag.textContent = seat.seatCode;
        selectedSeatsContainer.appendChild(seatTag);
    });

    // Enable/disable confirm button
    document.getElementById("confirmBtn").disabled = selectedSeats.length === 0;
}
