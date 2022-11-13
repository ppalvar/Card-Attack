window.addEventListener("load", () => {
    let selectedCard = null;
    let cardSlots = [];

    for (let i = 1; i <= 5; i++) {
        cardSlots.push(document.getElementById(`card-slot-${i}a`));
        cardSlots.push(document.getElementById(`card-slot-${i}b`));
    }

    for (let cardSlot of cardSlots) {
        cardSlot.addEventListener("click", () => {
            selectedCard = cardSlot;
        });

        cardSlot.addEventListener("dblclick", () => {
            
        });
    }
});
