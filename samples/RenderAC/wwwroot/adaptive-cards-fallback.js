// Fallback minimal AdaptiveCards implementation if CDN fails
if (typeof window.AdaptiveCards === 'undefined') {
    console.warn('AdaptiveCards not loaded, using fallback implementation');
    
    window.AdaptiveCards = {
        AdaptiveCard: function() {
            this.hostConfig = {};
            this.onExecuteAction = function() {};
            this.parse = function(cardJson) {
                this.card = cardJson;
                console.log('Fallback: Parsed card:', cardJson);
            };
            this.render = function() {
                const div = document.createElement('div');
                div.style.border = '2px solid #ccc';
                div.style.padding = '20px';
                div.style.borderRadius = '8px';
                div.style.backgroundColor = '#f9f9f9';
                
                const title = document.createElement('h3');
                title.textContent = 'Adaptive Card (Fallback Mode)';
                div.appendChild(title);
                
                const content = document.createElement('pre');
                content.style.background = '#fff';
                content.style.padding = '10px';
                content.style.borderRadius = '4px';
                content.style.overflow = 'auto';
                content.style.maxHeight = '300px';
                content.textContent = JSON.stringify(this.card, null, 2);
                div.appendChild(content);
                
                const notice = document.createElement('p');
                notice.style.color = '#666';
                notice.style.fontStyle = 'italic';
                notice.textContent = 'Note: Adaptive Cards library failed to load. Showing raw JSON.';
                div.appendChild(notice);
                
                return div;
            };
        },
        SubmitAction: function() {},
        OpenUrlAction: function() {}
    };
}