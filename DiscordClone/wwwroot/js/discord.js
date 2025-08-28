// Discord Clone JavaScript
document.addEventListener('DOMContentLoaded', function() {
    initializeDiscordApp();
});

function initializeDiscordApp() {
    // Add server/channel modals
    initializeModals();
    
    // Initialize sidebar navigation
    initializeSidebarNavigation();
    
    // Initialize Discord logo click
    initializeDiscordLogo();
}

// Modal functionality
function initializeModals() {
    // Create Server Modal
    const addServerBtn = document.querySelector('.add-server');
    const createServerModal = document.getElementById('createServerModal');
    const closeModalBtn = document.getElementById('closeModal');
    const closeModalBtn2 = document.getElementById('closeModal2');
    const closeModalBtn3 = document.getElementById('closeModal3');
    const serverOptions = document.querySelectorAll('.server-option');
    const joinServerBtn = document.querySelector('.join-server-btn');
    
    // Step navigation
    const backToStep1Btn = document.getElementById('backToStep1');
    const backToStep2Btn = document.getElementById('backToStep2');
    const skipBtn = document.querySelector('.skip-btn');
    const purposeOptions = document.querySelectorAll('.purpose-option');
    const createServerBtn = document.getElementById('createServerBtn');
    const serverNameInput = document.getElementById('serverName');
    const serverIconInput = document.getElementById('serverIcon');

    // Open modal when clicking add server button
    if (addServerBtn) {
        addServerBtn.addEventListener('click', function() {
            createServerModal.classList.add('show');
            document.body.style.overflow = 'hidden';
            showStep(1); // Always start with step 1
        });
    }

    // Close modal when clicking close buttons
    [closeModalBtn, closeModalBtn2, closeModalBtn3].forEach(btn => {
        if (btn) {
            btn.addEventListener('click', function() {
                closeModal();
            });
        }
    });

    // Close modal when clicking outside
    if (createServerModal) {
        createServerModal.addEventListener('click', function(e) {
            if (e.target === this) {
                closeModal();
            }
        });
    }

    // Close modal with Escape key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape' && createServerModal.classList.contains('show')) {
            closeModal();
        }
    });

    // Handle server option selection (Step 1)
    serverOptions.forEach(option => {
        option.addEventListener('click', function() {
            // Remove selected class from all options
            serverOptions.forEach(opt => opt.classList.remove('selected'));
            
            // Add selected class to clicked option
            this.classList.add('selected');
            
            // Get the template type
            const template = this.dataset.template;
            
            // For custom template, go to step 2
            if (template === 'custom') {
                setTimeout(() => {
                    showStep(2);
                }, 300);
            } else {
                // For other templates, go directly to step 3 with predefined values
                setTimeout(() => {
                    setTemplateDefaults(template);
                    showStep(3);
                }, 300);
            }
        });
    });

    // Handle purpose option selection (Step 2)
    purposeOptions.forEach(option => {
        option.addEventListener('click', function() {
            // Remove selected class from all options
            purposeOptions.forEach(opt => opt.classList.remove('selected'));
            
            // Add selected class to clicked option
            this.classList.add('selected');
            
            // Go to step 3
            setTimeout(() => {
                showStep(3);
            }, 300);
        });
    });

    // Skip button (Step 2)
    if (skipBtn) {
        skipBtn.addEventListener('click', function() {
            setTimeout(() => {
                showStep(3);
            }, 300);
        });
    }

    // Back buttons
    if (backToStep1Btn) {
        backToStep1Btn.addEventListener('click', function() {
            showStep(1);
        });
    }

    if (backToStep2Btn) {
        backToStep2Btn.addEventListener('click', function() {
            showStep(2);
        });
    }

    // Handle join server button
    if (joinServerBtn) {
        joinServerBtn.addEventListener('click', function() {
            closeModal();
            alert('Join Server feature coming soon!');
        });
    }

    // Handle server creation (Step 3)
    if (createServerBtn) {
        createServerBtn.addEventListener('click', function() {
            createServer();
        });
    }

    // Character count for server name
    if (serverNameInput) {
        serverNameInput.addEventListener('input', function() {
            const charCount = this.value.length;
            const charCountElement = document.querySelector('.char-count');
            if (charCountElement) {
                charCountElement.textContent = `${charCount}/100`;
            }
            
            // Enable/disable create button based on input
            if (createServerBtn) {
                createServerBtn.disabled = charCount === 0;
            }
        });
    }

    // Handle server icon upload
    if (serverIconInput) {
        serverIconInput.addEventListener('change', function(e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    const iconPreview = document.querySelector('.icon-preview');
                    if (iconPreview) {
                        iconPreview.innerHTML = `<img src="${e.target.result}" alt="Server Icon" style="width: 100%; height: 100%; object-fit: cover; border-radius: 14px;">`;
                    }
                };
                reader.readAsDataURL(file);
            }
        });
    }
}

function showStep(stepNumber) {
    // Hide all steps
    const steps = document.querySelectorAll('.modal-step');
    steps.forEach(step => {
        step.style.display = 'none';
    });
    
    // Show the selected step
    const currentStep = document.getElementById(`step${stepNumber}`);
    if (currentStep) {
        currentStep.style.display = 'block';
    }
}

function setTemplateDefaults(template) {
    const serverNameInput = document.getElementById('serverName');
    if (serverNameInput) {
        switch(template) {
            case 'gaming':
                serverNameInput.value = 'Gaming Server';
                break;
            case 'friends':
                serverNameInput.value = 'Friends Server';
                break;
            case 'study':
                serverNameInput.value = 'Study Group';
                break;
            case 'club':
                serverNameInput.value = 'School Club';
                break;
            default:
                serverNameInput.value = '';
        }
        
        // Trigger input event to update character count
        serverNameInput.dispatchEvent(new Event('input'));
    }
}

function createServer() {
    const serverName = document.getElementById('serverName').value.trim();
    const serverIconInput = document.getElementById('serverIcon');
    
    if (!serverName) {
        alert('Please enter a server name');
        return;
    }
    
    // Show loading state
    const createBtn = document.getElementById('createServerBtn');
    const originalText = createBtn.textContent;
    createBtn.textContent = 'Creating...';
    createBtn.disabled = true;
    
    // Prepare server data
    const serverData = {
        name: serverName,
        description: null, // Can be added later
        icon: null, // Can be added later
        template: null, // Can be added later
        purpose: null // Can be added later
    };
    
    // If there's an uploaded icon, convert to base64
    if (serverIconInput && serverIconInput.files[0]) {
        const reader = new FileReader();
        reader.onload = function(e) {
            serverData.icon = e.target.result;
            sendCreateServerRequest(serverData, createBtn, originalText);
        };
        reader.readAsDataURL(serverIconInput.files[0]);
    } else {
        sendCreateServerRequest(serverData, createBtn, originalText);
    }
}

function sendCreateServerRequest(serverData, createBtn, originalText) {
    // Send request to API
    fetch('/Home/CreateServer', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(serverData)
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            // Close modal
            closeModal();
            
            // Show success message
            alert(data.message);
            
            // Redirect to server page with the new server ID
            window.location.href = `/Home/Server?serverId=${data.serverId}`;
        } else {
            alert('Error: ' + data.message);
        }
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error creating server. Please try again.');
    })
    .finally(() => {
        // Reset button
        createBtn.textContent = originalText;
        createBtn.disabled = false;
    });
}

function closeModal() {
    const createServerModal = document.getElementById('createServerModal');
    if (createServerModal) {
        createServerModal.classList.remove('show');
        document.body.style.overflow = '';
        
        // Reset all selections
        const serverOptions = document.querySelectorAll('.server-option');
        const purposeOptions = document.querySelectorAll('.purpose-option');
        
        serverOptions.forEach(opt => opt.classList.remove('selected'));
        purposeOptions.forEach(opt => opt.classList.remove('selected'));
        
        // Reset form
        const serverNameInput = document.getElementById('serverName');
        const serverIconInput = document.getElementById('serverIcon');
        const iconPreview = document.querySelector('.icon-preview');
        
        if (serverNameInput) serverNameInput.value = '';
        if (serverIconInput) serverIconInput.value = '';
        if (iconPreview) iconPreview.innerHTML = '<i class="fas fa-image"></i>';
        
        // Reset character count
        const charCount = document.querySelector('.char-count');
        if (charCount) charCount.textContent = '0/100';
        
        // Disable create button
        const createBtn = document.getElementById('createServerBtn');
        if (createBtn) {
            createBtn.disabled = true;
        }
        
        // Go back to step 1
        showStep(1);
    }
}

function initializeSidebarNavigation() {
    // Handle section item clicks
    document.querySelectorAll('.section-item').forEach(item => {
        item.addEventListener('click', function() {
            const itemType = this.dataset.item;
            
            // Remove active class from all items
            document.querySelectorAll('.section-item').forEach(i => i.classList.remove('active'));
            
            // Add active class to clicked item
            this.classList.add('active');
            
            // Handle navigation based on item type
            handleNavigation(itemType);
        });
    });
}

function initializeDiscordLogo() {
    // Discord logo click handler
    const discordLogo = document.querySelector('.discord-logo');
    if (discordLogo) {
        discordLogo.addEventListener('click', function() {
            // Redirect to home page (Add Friend view)
            window.location.href = '/';
        });
    }
}

function handleNavigation(itemType) {
    switch(itemType) {
        case 'friends':
            // Stay on current page (Friends view)
            console.log('Navigating to Friends');
            break;
        case 'shop':
            // Navigate to shop (placeholder)
            console.log('Navigating to Shop');
            alert('Shop feature coming soon!');
            break;
        default:
            console.log('Unknown navigation:', itemType);
    }
}

// Handle server creation success
function onServerCreated(serverName) {
    // Redirect to server page
    window.location.href = '/Home/Server';
}

// Utility functions
function showNotification(message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.textContent = message;
    
    // Add styles
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 12px 16px;
        border-radius: 4px;
        color: white;
        font-weight: 500;
        z-index: 1000;
        animation: slideIn 0.3s ease;
    `;
    
    if (type === 'success') {
        notification.style.backgroundColor = '#43b581';
    } else if (type === 'error') {
        notification.style.backgroundColor = '#f04747';
    } else {
        notification.style.backgroundColor = '#5865f2';
    }
    
    document.body.appendChild(notification);
    
    // Remove after 3 seconds
    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => {
            document.body.removeChild(notification);
        }, 300);
    }, 3000);
}

// Add CSS animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);


