// Base URL for all API calls
const API_BASE = '/api';

// Helper function to make API calls with error handling
async function fetchAPI(url, options = {}) {
    try {
        const response = await fetch(url, {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            },
            ...options
        });
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        alert('An error occurred. Please try again.');
        throw error;
    }
}

// Load and display 3 featured services on the home page
async function loadFeaturedServices() {
    const container = document.getElementById('featuredServices');
    if (!container) return;
    
    try {
        const services = await fetchAPI(`${API_BASE}/services`);
        const featured = services.slice(0, 3);
        
        container.innerHTML = featured.map(service => `
            <div class="service-card">
                <h3>${service.name}</h3>
                <p>${service.description}</p>
                <div class="price">$${service.price.toFixed(2)}</div>
                <span class="category">${service.category}</span>
                <a href="request-service.html?id=${service.id}" class="btn">Request Service</a>
            </div>
        `).join('');
    } catch (error) {
        container.innerHTML = '<p>Unable to load services. Please try again later.</p>';
    }
}

// Load all services on the services page
async function loadAllServices() {
    const container = document.getElementById('servicesList');
    if (!container) return;
    
    try {
        const services = await fetchAPI(`${API_BASE}/services`);
        renderServices(services);
    } catch (error) {
        container.innerHTML = '<p>Unable to load services. Please try again later.</p>';
    }
}

// Display services in the services grid
function renderServices(services) {
    const container = document.getElementById('servicesList');
    if (!container) return;
    
    container.innerHTML = services.map(service => `
        <div class="service-card" data-category="${service.category}">
            <h3>${service.name}</h3>
            <p>${service.description}</p>
            <div class="price">$${service.price.toFixed(2)}</div>
            <span class="category">${service.category}</span>
            <button class="btn" onclick="openRequestModal(${service.id}, '${service.name}')">Request Service</button>
        </div>
    `).join('');
}

// Filter services by category
async function filterByCategory(category) {
    const container = document.getElementById('servicesList');
    if (!container) return;
    
    try {
        if (category === 'all') {
            const services = await fetchAPI(`${API_BASE}/services`);
            renderServices(services);
        } else {
            const services = await fetchAPI(`${API_BASE}/services/category/${category}`);
            renderServices(services);
        }
    } catch (error) {
        container.innerHTML = '<p>Unable to load services. Please try again later.</p>';
    }
}

// Setup category filter buttons
function setupFilters() {
    const filterBtns = document.querySelectorAll('.filter-btn');
    
    filterBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            // Remove active class from all buttons
            filterBtns.forEach(b => b.classList.remove('active'));
            // Add active class to clicked button
            btn.classList.add('active');
            // Filter services by selected category
            filterByCategory(btn.dataset.category);
        });
    });
}

// Setup service request modal functionality
function setupModal() {
    const modal = document.getElementById('requestModal');
    const closeBtn = modal?.querySelector('.close');
    const form = document.getElementById('requestForm');
    
    // Close modal when clicking X button
    if (closeBtn) {
        closeBtn.addEventListener('click', () => {
            modal.classList.remove('show');
        });
    }
    
    // Close modal when clicking outside of it
    if (modal) {
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                modal.classList.remove('show');
            }
        });
    }
    
    // Handle form submission
    if (form) {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            // Collect form data
            const requestData = {
                serviceId: parseInt(document.getElementById('serviceId').value),
                customerName: document.getElementById('customerName').value,
                email: document.getElementById('email').value,
                phone: document.getElementById('phone').value,
                address: document.getElementById('address').value,
                preferredDate: document.getElementById('preferredDate').value
            };
            
            try {
                // Send request to API
                await fetchAPI(`${API_BASE}/servicerequests`, {
                    method: 'POST',
                    body: JSON.stringify(requestData)
                });
                
                alert('Request submitted successfully!');
                modal.classList.remove('show');
                window.location.href = 'my-requests.html';
            } catch (error) {
                console.error('Error submitting request:', error);
            }
        });
    }
}

// Open the service request modal and populate with service info
function openRequestModal(serviceId, serviceName) {
    const modal = document.getElementById('requestModal');
    const today = new Date().toISOString().split('T')[0];
    
    // Set service info
    document.getElementById('serviceId').value = serviceId;
    document.getElementById('serviceName').value = serviceName;
    // Set minimum date to today
    document.getElementById('preferredDate').setAttribute('min', today);
    
    // Clear previous form data
    document.getElementById('customerName').value = '';
    document.getElementById('email').value = '';
    document.getElementById('phone').value = '';
    document.getElementById('address').value = '';
    document.getElementById('preferredDate').value = '';
    
    modal.classList.add('show');
}

// Load customers service requests by email
async function loadMyRequests() {
    const email = document.getElementById('searchEmail').value;
    const container = document.getElementById('requestsList');
    
    if (!email) {
        alert('Please enter your email address');
        return;
    }
    
    try {
        const requests = await fetchAPI(`${API_BASE}/servicerequests/customer/${encodeURIComponent(email)}`);
        
        if (requests.length === 0) {
            container.innerHTML = '<p>No requests found for this email.</p>';
            return;
        }
        
        // Display each request with its details
        container.innerHTML = requests.map(req => `
            <div class="request-item">
                <h3>${req.service?.name || 'Service #' + req.serviceId}</h3>
                <p><strong>Date:</strong> ${new Date(req.preferredDate).toLocaleDateString()}</p>
                <p><strong>Address:</strong> ${req.address}</p>
                <p><strong>Phone:</strong> ${req.phone}</p>
                <span class="status status-${req.status.toLowerCase().replace(' ', '-')}">${req.status}</span>
                ${req.rating ? `<p><strong>Rating:</strong> ${'★'.repeat(req.rating)}${'☆'.repeat(5 - req.rating)}</p>` : ''}
                ${req.comments ? `<p><strong>Comments:</strong> ${req.comments}</p>` : ''}
                <div class="actions">
                    ${req.status === 'Completed' && !req.rating ? `<button class="btn" onclick="openRatingModal(${req.id})">Rate Service</button>` : ''}
                    ${req.status === 'Pending' ? `<button class="btn" onclick="cancelRequest(${req.id})">Cancel Request</button>` : ''}
                </div>
            </div>
        `).join('');
    } catch (error) {
        container.innerHTML = '<p>Unable to load requests. Please try again later.</p>';
    }
}

// Setup rating modal with star selection
function setupRatingModal() {
    const modal = document.getElementById('ratingModal');
    const closeBtn = modal?.querySelector('.close');
    const stars = document.querySelectorAll('.star');
    let selectedRating = 0;
    
    // Handle star click to select rating
    stars.forEach(star => {
        star.addEventListener('click', () => {
            selectedRating = parseInt(star.dataset.value);
            updateStars();
        });
    });
    
    // Update star display based on selected rating
    function updateStars() {
        stars.forEach(s => {
            s.classList.toggle('active', parseInt(s.dataset.value) <= selectedRating);
        });
    }
    
    // Close modal when clicking X
    if (closeBtn) {
        closeBtn.addEventListener('click', () => {
            modal.classList.remove('show');
        });
    }
    
    // Close modal when clicking outside
    if (modal) {
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                modal.classList.remove('show');
            }
        });
    }
    
    // Handle rating form submission
    const form = document.getElementById('ratingForm');
    if (form) {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const requestId = document.getElementById('ratingRequestId').value;
            const comments = document.getElementById('comments').value;
            
            try {
                // Submit rating to API
                await fetchAPI(`${API_BASE}/servicerequests/${requestId}/rating`, {
                    method: 'PUT',
                    body: JSON.stringify({ rating: selectedRating, comments })
                });
                
                alert('Rating submitted successfully!');
                modal.classList.remove('show');
                loadMyRequests();
            } catch (error) {
                console.error('Error submitting rating:', error);
            }
        });
    }
}

// Open rating modal for a specific request
function openRatingModal(requestId) {
    const modal = document.getElementById('ratingModal');
    document.getElementById('ratingRequestId').value = requestId;
    document.getElementById('comments').value = '';
    modal.classList.add('show');
}

// Cancel a service request
async function cancelRequest(requestId) {
    if (!confirm('Are you sure you want to cancel this request?')) {
        return;
    }
    
    try {
        await fetchAPI(`${API_BASE}/servicerequests/${requestId}`, {
            method: 'DELETE'
        });
        
        alert('Request cancelled successfully!');
        loadMyRequests();
    } catch (error) {
        console.error('Error cancelling request:', error);
    }
}