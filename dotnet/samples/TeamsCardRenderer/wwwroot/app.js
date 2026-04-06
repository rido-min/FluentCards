(async () => {
  const params = new URLSearchParams(window.location.search);
  const cardType = params.get('card') || 'approval';

  const res = await fetch(`/api/cards/${cardType}`);
  if (!res.ok) {
    document.getElementById('card-container').textContent = `Error loading card: ${res.status}`;
    return;
  }
  const cardJson = await res.json();

  const adaptiveCard = new AdaptiveCards.AdaptiveCard();
  adaptiveCard.hostConfig = new AdaptiveCards.HostConfig({
    fontFamily: '"Segoe UI", sans-serif',
    fontSizes: { small: 12, default: 14, medium: 17, large: 21, extraLarge: 26 },
  });
  adaptiveCard.parse(cardJson);

  const rendered = adaptiveCard.render();
  if (rendered) {
    document.getElementById('card-container').appendChild(rendered);
    document.body.setAttribute('data-rendered', 'true');
  }
})();
