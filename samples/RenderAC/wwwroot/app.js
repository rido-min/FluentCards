class AdaptiveCardRenderer {
    constructor() {
        this.cardHost = document.getElementById('cardHost');
        this.cardInfo = document.getElementById('cardInfo');
        this.cardTitle = document.getElementById('cardTitle');
        this.cardDescription = document.getElementById('cardDescription');
        this.cardButtons = document.querySelectorAll('.card-btn');
        this.currentCardType = null;
        
        this.initializeEventListeners();
        this.loadCardTypes();
    }
    
    initializeEventListeners() {
        this.cardButtons.forEach(button => {
            button.addEventListener('click', (e) => {
                const cardType = e.target.dataset.card;
                this.loadCard(cardType);
                this.setActiveButton(e.target);
            });
        });
    }
    
    setActiveButton(activeButton) {
        this.cardButtons.forEach(btn => btn.classList.remove('active'));
        activeButton.classList.add('active');
    }
    
    async loadCardTypes() {
        try {
            const response = await fetch('/api/cards/types');
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const types = await response.json();
            
            // Store card info for later use
            this.cardTypeInfo = {};
            types.forEach(type => {
                this.cardTypeInfo[type.id] = {
                    name: type.name,
                    description: type.description
                };
            });
            
            // Load welcome card by default
            this.loadCard('welcome');
            this.setActiveButton(document.querySelector('[data-card="welcome"]'));
        } catch (error) {
            console.error('Failed to load card types:', error);
            this.showError('Failed to load card types. Please check your internet connection.');
        }
    }
    
    async loadCard(cardType) {
        this.currentCardType = cardType;
        this.showLoading();
        
        try {
            const response = await fetch(`/api/cards/${cardType}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            // API now returns proper JSON, no need to parse
            const cardJson = await response.json();
            console.log('Card JSON loaded:', cardJson);
            
            this.renderCard(cardJson);
            this.showCardInfo(cardType);
        } catch (error) {
            console.error('Failed to load card:', error);
            this.showError(`Failed to load card: ${error.message}`);
        }
    }
    
    showCardInfo(cardType) {
        const info = this.cardTypeInfo[cardType];
        if (info) {
            this.cardTitle.textContent = info.name;
            this.cardDescription.textContent = info.description;
            this.cardInfo.style.display = 'block';
        }
    }
    
    renderCard(cardJson) {
        // Clear previous card
        this.cardHost.innerHTML = '';
        
        console.log('renderCard called with:', cardJson);
        
        // Check if AdaptiveCards is available
        if (typeof AdaptiveCards === 'undefined') {
            console.error('AdaptiveCards library not found');
            this.showError('AdaptiveCards library not loaded. Please check your internet connection and refresh the page.');
            return;
        }
        
        try {
            // Create adaptive card
            console.log('Creating AdaptiveCard...');
            const adaptiveCard = new AdaptiveCards.AdaptiveCard();
            console.log('AdaptiveCard created:', adaptiveCard);
            
            // Handle card actions
            adaptiveCard.onExecuteAction = (action) => {
                console.log('Action executed:', action);
                this.handleAction(action);
            };
            
            // Parse and render card
            console.log('Parsing card...');
            adaptiveCard.parse(cardJson);
            console.log('Card parsed, rendering...');
            
            const renderedCard = adaptiveCard.render();
            console.log('Card rendered:', renderedCard);
            
            if (renderedCard && renderedCard.nodeType) {
                this.cardHost.appendChild(renderedCard);
                console.log('Card added to DOM');
            } else {
                console.error('Render returned invalid element');
                this.showError('Card rendering failed - invalid output from renderer');
            }
            
        } catch (error) {
            console.error('Error rendering card:', error);
            console.error('Error stack:', error.stack);
            this.showError(`Error rendering card: ${error.message}`);
        }
    }
    
    async handleAction(action) {
        try {
            console.log('Action executed:', action);
            console.log('Action type:', action.constructor.name);
            
            // Version 2.3.0 uses different detection
            if (action.getJsonType) {
                const actionType = action.getJsonType();
                if (actionType === 'Action.Submit') {
                    await this.handleSubmitAction(action);
                } else if (actionType === 'Action.OpenUrl') {
                    window.open(action.url, '_blank');
                }
            } else if (action.jsonType) {
                // Alternative detection
                const actionType = action.jsonType;
                if (actionType === 'Action.Submit') {
                    await this.handleSubmitAction(action);
                } else if (actionType === 'Action.OpenUrl') {
                    window.open(action.url, '_blank');
                }
            } else {
                // Fallback detection
                if (action.url) {
                    window.open(action.url, '_blank');
                } else if (action.data || action.title) {
                    await this.handleSubmitAction(action);
                }
            }
        } catch (error) {
            console.error('Error handling action:', error);
            this.showError(`Error handling action: ${error.message}`);
        }
    }
    
    async handleSubmitAction(action) {
        try {
            // Handle different data structures
            let actionData = {};
            if (action.data) {
                actionData = action.data;
            } else if (action.title) {
                actionData = { submitted: true };
            }
            
            console.log('Submitting action data:', actionData);
            
            const response = await fetch('/api/cards/submit', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    cardType: this.currentCardType,
                    data: actionData
                })
            });
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            const result = await response.json();
            this.showSuccess(result.message || 'Form submitted successfully!');
            
        } catch (error) {
            console.error('Submission error:', error);
            this.showError(`Submission error: ${error.message}`);
        }
    }
    
    showLoading() {
        this.cardHost.innerHTML = '<div class="loading">Loading card...</div>';
        this.cardInfo.style.display = 'none';
    }
    
    showError(message) {
        this.cardHost.innerHTML = `<div class="error">Error: ${message}</div>`;
        this.cardInfo.style.display = 'none';
    }
    
    showSuccess(message) {
        const successDiv = document.createElement('div');
        successDiv.className = 'success';
        successDiv.textContent = message;
        this.cardHost.appendChild(successDiv);
        
        // Remove success message after 3 seconds
        setTimeout(() => {
            if (successDiv.parentNode) {
                successDiv.parentNode.removeChild(successDiv);
            }
        }, 3000);
    }
}

// Initialize renderer when libraries are loaded
function initializeApp() {
    console.log('Initializing AdaptiveCardRenderer...');
    new AdaptiveCardRenderer();
}

// Wait for window to load and libraries to be available
window.addEventListener('load', function() {
    console.log('Window loaded, checking for AdaptiveCards...');
    
    // Check if libraries are loaded, if not wait a bit more
    let attempts = 0;
    const maxAttempts = 20;
    
    function checkAndInit() {
        attempts++;
        
        if (typeof window.AdaptiveCards !== 'undefined') {
            console.log('AdaptiveCards found, initializing...');
            initializeApp();
        } else if (attempts < maxAttempts) {
            console.log(`Waiting for AdaptiveCards... attempt ${attempts}`);
            setTimeout(checkAndInit, 500);
        } else {
            console.error('AdaptiveCards failed to load after all attempts');
            const cardHost = document.getElementById('cardHost');
            if (cardHost) {
                cardHost.innerHTML = 
                    '<div class="error">Error: Adaptive Cards library failed to load. Please check your internet connection and refresh the page.</div>';
            }
        }
    }
    
    checkAndInit();
});