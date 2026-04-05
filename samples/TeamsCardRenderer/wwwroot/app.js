(async () => {
  const params = new URLSearchParams(window.location.search);
  const cardType = params.get('card') || 'approval';

  const uuid = crypto.randomUUID();
  const iframe = document.getElementById('ac-renderer');

  window.addEventListener('message', async (event) => {
    if (event.origin !== 'https://adaptivecards.microsoft.com') return;

    const { type, payload } = event.data;

    if (type === 'ac-renderer-ready') {
      const res = await fetch(`/api/cards/${cardType}`);
      if (!res.ok) return;
      const card = await res.json();
      iframe.contentWindow.postMessage(
        { type: 'cardPayload', payload: { card } },
        'https://adaptivecards.microsoft.com'
      );
    }

    if (type === 'ac-dimensions-changed') {
      const height = payload?.height;
      if (height) iframe.style.height = height + 'px';
      document.body.setAttribute('data-rendered', 'true');
    }
  });

  iframe.src = `https://adaptivecards.microsoft.com/renderer.html?id=${uuid}`;
})();
