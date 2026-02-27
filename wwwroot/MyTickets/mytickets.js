// My Tickets - JavaScript Logic

let allTickets = [];
let currentTab = 'current'; // 'current' or 'history'

// Initialize page
document.addEventListener('DOMContentLoaded', () => {
    setupEventListeners();
    loadUserTickets();
});

// Setup event listeners
function setupEventListeners() {
    // Tab switching
    const tabButtons = document.querySelectorAll('.tab-btn');
    tabButtons.forEach(btn => {
        btn.addEventListener('click', () => {
            const tab = btn.dataset.tab;
            switchTab(tab);
        });
    });

    // Logout button
    const logoutBtn = document.querySelector('.logout-btn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', logout);
    }
}

// Switch between tabs
function switchTab(tab) {
    currentTab = tab;

    // Update active tab button
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
        if (btn.dataset.tab === tab) {
            btn.classList.add('active');
        }
    });

    // Re-render tickets
    renderTickets();
}

// Load user tickets from API
async function loadUserTickets() {
    showLoading();

    try {
        // Get userId from localStorage (assuming it's stored there after login)
        const userId = localStorage.getItem('userId') || 1; // Default to 1 for testing

        const response = await fetch(`/api/booking/user/${userId}`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        allTickets = await response.json();
        console.log('Loaded tickets:', allTickets);
        
        renderTickets();
    } catch (error) {
        console.error('Failed to load tickets:', error);
        showError('Không thể tải danh sách vé. Vui lòng thử lại sau.');
    }
}

// Render tickets based on current tab
function renderTickets() {
    const grid = document.getElementById('tickets-grid');
    const emptyState = document.getElementById('empty-state');
    const loadingState = document.getElementById('loading-state');

    // Hide loading
    loadingState.style.display = 'none';

    // Filter tickets based on tab
    const filteredTickets = allTickets.filter(ticket => {
        if (currentTab === 'current') {
            return ticket.isUpcoming;
        } else {
            return !ticket.isUpcoming;
        }
    });

    // Show empty state if no tickets
    if (filteredTickets.length === 0) {
        grid.innerHTML = '';
        emptyState.style.display = 'block';
        emptyState.querySelector('p').textContent = 
            currentTab === 'current' 
                ? 'Bạn chưa có vé nào sắp tới.' 
                : 'Bạn chưa có lịch sử đặt vé.';
        return;
    }

    // Hide empty state and render tickets
    emptyState.style.display = 'none';
    grid.innerHTML = '';

    filteredTickets.forEach(ticket => {
        const ticketCard = createTicketCard(ticket);
        grid.appendChild(ticketCard);
    });

    // Generate QR codes after rendering
    setTimeout(() => {
        filteredTickets.forEach(ticket => {
            generateQRCode(ticket.bookingId);
        });
    }, 100);
}

// Create ticket card HTML
function createTicketCard(ticket) {
    const card = document.createElement('div');
    card.className = 'ticket-card';
    card.dataset.bookingId = ticket.bookingId;

    // Format date
    const showDate = new Date(ticket.showDate);
    const formattedDate = showDate.toLocaleDateString('vi-VN', {
        weekday: 'short',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });

    // Format price
    const formattedPrice = ticket.totalPrice.toLocaleString('vi-VN') + ' đ';

    // Status badge
    const statusClass = ticket.isUpcoming ? 'status-upcoming' : 'status-past';
    const statusText = ticket.isUpcoming ? 'Sắp chiếu' : 'Đã chiếu';

    card.innerHTML = `
        <div class="status-badge ${statusClass}">${statusText}</div>
        
        <div class="ticket-info">
            <div class="ticket-movie-title">${ticket.movieTitle}</div>
            
            <div class="ticket-detail-row">
                <span class="ticket-label">📅 Ngày chiếu:</span>
                <span class="ticket-value">${formattedDate}</span>
            </div>
            
            <div class="ticket-detail-row">
                <span class="ticket-label">🕐 Suất chiếu:</span>
                <span class="ticket-value">${ticket.showTime}</span>
            </div>
            
            <div class="ticket-detail-row">
                <span class="ticket-label">🎬 Rạp:</span>
                <span class="ticket-value">${ticket.cinemaName}</span>
            </div>
            
            <div class="ticket-detail-row">
                <span class="ticket-label">💺 Ghế:</span>
                <div class="ticket-seats">
                    ${ticket.seats.map(seat => `<span class="seat-badge">${seat}</span>`).join('')}
                </div>
            </div>
            
            <div class="ticket-price">💰 ${formattedPrice}</div>
        </div>

        <div class="ticket-actions">
            <div class="qr-container" id="qr-${ticket.bookingId}">
                <!-- QR code will be generated here -->
            </div>
            <button class="view-btn" onclick="viewTicketDetail(${ticket.bookingId})">
                Xem vé
            </button>
        </div>
    `;

    return card;
}

// Generate QR code for a ticket
function generateQRCode(bookingId) {
    const container = document.getElementById(`qr-${bookingId}`);
    if (!container || container.querySelector('canvas')) {
        return; // Already generated
    }

    // Clear container
    container.innerHTML = '';

    // Find ticket data
    const ticket = allTickets.find(t => t.bookingId === bookingId);
    if (!ticket) return;

    // Create QR code data
    const qrData = JSON.stringify({
        bookingId: ticket.bookingId,
        movie: ticket.movieTitle,
        seats: ticket.seats.join(', '),
        date: ticket.showDate,
        time: ticket.showTime
    });

    // Generate QR code
    try {
        new QRCode(container, {
            text: ticket.qrCodeData || `BOOKING-${bookingId}`,
            width: 100,
            height: 100,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H
        });
    } catch (error) {
        console.error('Failed to generate QR code:', error);
        container.innerHTML = '<p style="color: #666; font-size: 12px;">QR không khả dụng</p>';
    }
}

// View ticket details in modal
function viewTicketDetail(bookingId) {
    const ticket = allTickets.find(t => t.bookingId === bookingId);
    if (!ticket) return;

    // Format date
    const showDate = new Date(ticket.showDate);
    const formattedDate = showDate.toLocaleDateString('vi-VN', {
        weekday: 'long',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });

    // Format price
    const formattedPrice = ticket.totalPrice.toLocaleString('vi-VN') + ' đ';

    // Update modal content
    document.getElementById('detail-movie-title').textContent = ticket.movieTitle;
    
    const statusBadge = document.getElementById('detail-status-badge');
    statusBadge.textContent = ticket.isUpcoming ? 'Sắp chiếu' : 'Đã chiếu';
    statusBadge.className = ticket.isUpcoming ? 'detail-status-badge status-upcoming' : 'detail-status-badge status-past';
    
    document.getElementById('detail-show-date').textContent = formattedDate;
    document.getElementById('detail-show-time').textContent = ticket.showTime;
    document.getElementById('detail-cinema-name').textContent = ticket.cinemaName;
    
    // Display seats
    const seatsList = document.getElementById('detail-seats-list');
    seatsList.innerHTML = ticket.seats.map(seat => `<span class="seat-badge">${seat}</span>`).join('');
    
    document.getElementById('detail-total-price').textContent = formattedPrice;

    // Generate QR code in modal
    const qrContainer = document.getElementById('detail-qr-code');
    qrContainer.innerHTML = '';  // Clear previous QR
    
    try {
        new QRCode(qrContainer, {
            text: ticket.qrCodeData || `BOOKING-${bookingId}`,
            width: 200,
            height: 200,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H
        });
    } catch (error) {
        console.error('Failed to generate QR code:', error);
        qrContainer.innerHTML = '<p style="color: #666;">QR không khả dụng</p>';
    }

    // Show modal
    const modal = document.getElementById('ticket-detail-modal');
    modal.style.display = 'flex';
    console.log('Opened detail modal for booking:', bookingId);
}

// Close ticket detail modal
function closeTicketDetail() {
    const modal = document.getElementById('ticket-detail-modal');
    modal.style.display = 'none';
}

// Show loading state
function showLoading() {
    document.getElementById('loading-state').style.display = 'block';
    document.getElementById('empty-state').style.display = 'none';
    document.getElementById('tickets-grid').innerHTML = '';
}

// Show error message
function showError(message) {
    const loadingState = document.getElementById('loading-state');
    const emptyState = document.getElementById('empty-state');
    
    loadingState.style.display = 'none';
    emptyState.style.display = 'block';
    emptyState.querySelector('p').textContent = message;
}

// Logout function
function logout() {
    localStorage.removeItem('userId');
    localStorage.removeItem('username');
    localStorage.removeItem('userToken');
    window.location.href = '../auth.html';
}

// Export for onclick handlers
window.viewTicketDetail = viewTicketDetail;
window.closeTicketDetail = closeTicketDetail;
