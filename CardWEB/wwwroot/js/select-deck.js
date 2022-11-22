window.addEventListener("load", () => {
    //cards layout
    let cardOptions = {};
    
    for (let i in cards) {
        const card = document.getElementById(`card-${i}`);
        cardOptions[cards[i]] = card;
    }

    let selectedCards = [];
    const maxMonster = 5;
    const maxEffects = 20;

    const selectedGallery = document.getElementById("selected-cards-gallery");
    const cardOptionsHtml = document.getElementById("card-options");

    for (const card in cardOptions) {
        cardOptions[card].addEventListener("click", () => {
            if (selectedCards.includes(card)) {
                selectedCards = selectedCards.filter(val => val != card);
                cardOptionsHtml.appendChild(cardOptions[card]);
            }
            else {
                selectedGallery.appendChild(cardOptions[card]);
                selectedCards.push(card);
            }
        });
    }

    //buttons
    const accept = document.getElementById("accept-btn");
    const cancel = document.getElementById("cancel-btn");

    const player = document.getElementById("player").value;

    cancel.addEventListener("click", () => {
        window.location.replace("/");
    });

    accept.addEventListener("click", () => {
        localStorage.setItem(`player-deck-${player}`, JSON.stringify(selectedCards));
        window.location.replace("/");
        alert("Deck saved");
    });
});