window.addEventListener("load", () => {
    let selectedCard = null;
    let selectedMonster = null;
    let cardSlotsA = [];
    let cardSlotsB = [];

    for (let i = 1; i <= 5; i++) {
        cardSlotsA.push(document.getElementById(`card-slot-${i}a`));
        cardSlotsB.push(document.getElementById(`card-slot-${i}b`));
    }

    let tableSlotsA = [];
    let tableSlotsB = [];

    for (let i = 1; i <= 5; i++) {
        tableSlotsA.push(document.getElementById(`table-slot-${i}a`));
        tableSlotsB.push(document.getElementById(`table-slot-${i}b`));
    }

    let modalFlag = false;

    window.addEventListener("keyup", ev => {
        if (ev.key == "Escape") {
            if (!modalFlag) {
                $("#end-game-modal").modal("show");
            }
            else {
                $("#end-game-modal").modal("hide");
            }

            modalFlag = !modalFlag;
        }
        if (ev.key == " ") {
            endTurn();
        }
    });

    async function postData(url='', data={}) {
        const response = await fetch(url, {
            method: 'POST', 
            mode: 'cors', 
            cache: 'no-cache', 
            credentials: 'same-origin', 
            headers: {
                'Content-Type': 'application/json'                
            },
            redirect: 'follow', 
            referrerPolicy: 'no-referrer', 
            body: JSON.stringify(data) 
        });
        
        return response.json(); // parses JSON response into native JavaScript objects
    }

    async function getData(url='', data={}) {
        const response = await fetch(url, {
            method: 'GET', 
            mode: 'cors', 
            cache: 'no-cache', 
            credentials: 'same-origin', 
            headers: {
                'Content-Type': 'application/json'                
            },
            redirect: 'follow', 
            referrerPolicy: 'no-referrer', 
            body: JSON.stringify(data) 
        });

        return response.json();
    }

    const matchType = document.getElementById("match-type"). value;

    let playerA = [];
    let playerB = [];

    try {
        playerA = JSON.parse(localStorage["player-deck-player-A"]);
    }
    catch {
        playerA = [];
    }

    try {
        playerB = JSON.parse(localStorage["player-deck-player-B"]);
    }
    catch {
        playerB = [];
    }

    let playerHand = postData(`/api/new-game/${matchType}`, {playerA:playerA, playerB:playerB});

    function showCards(hand) {
        setTimeout(() => {
            hand.then(val => {
                let i = 0;
                for (let slot of cardSlotsA) {
                    try {
                        slot.firstChild.remove();
                    }
                    catch {

                    }
                    
                    if (val[i] === undefined)continue;

                    const color = !val[i].isMonster ? "rgb(117, 64, 164)" : "rgb(146, 139, 65)";

                    const node = document.createElement("div");
                    node.id = `${slot.id}-${i}`;
                    node.innerHTML =   `<div name="card" class="card">
                                            <img src="/img/front.png" id="card-img" alt="background" class="card-image"
                                            style="background-color: ${color};">
                                            <h6 class="card-title ">${val[i].name}</h6>
                                            <img src="/img/${val[i].image}" alt="pollo" class="card-picture">
                                            <p class="card-description">${val[i].description}</p>
                                        </div>`;
                    slot.appendChild(node);
                    flipAnimation(slot);
                    addJS(slot, val[i]);
                    i++;
                }
                selectedCard = null;
                selectedMonster = null;
            }, 600);
        });
    }

    function flipAnimation(node) {
        node.animate([{transform: "scaleX(0)"}, {transform: "scaleX(1)"}], {duration:400, iterations:1});
    }

    function flipReverseAnimation(node) {
        node.animate([{transform: "scaleX(1)"}, {transform: "scaleX(0)"}], {duration:400, iterations:1});
    }

    function select(node) {
        node.classList.add("selected");
    }

    function unselect(node) {
        node.classList.remove("selected");
    }

    function addJS(node, context) {
        node.onclick = () => {
            select(node);                
            if (!context.isMonster) {
                selectedCard = node;
            }
            else if (selectedCard != null) {
            }
        }
        
        node.ondblclick = () => {
            if (context.isMonster) {
                let i = 0;

                for (let slot of tableSlotsA) {
                    i ++;
                    if (slot.children.length == 0) {
                        postData("/api/play", {CardIndex : context.index}).then(val => {
                            if (val.canMove){
                                const tmp = document.createElement("div");
                                const progress = document.createElement("div");

                                tmp.innerHTML = node.innerHTML;
                                progress.classList.add("progress","my-1");
                                progress.innerHTML = `<div class="progress-bar progress-bar-animated progress-bar-striped bg-success" style="width:100%;color:black;overflow:visible;">HP: ${context.hp}|Attack: ${context.attack}</div>`;

                                node.firstChild.remove();

                                slot.appendChild(progress);
                                slot.appendChild(tmp);

                                slot.onclick = () => {
                                    if (selectedMonster !== null && tableSlotsB.includes(slot)) {
                                        postData(`/api/attack/${selectedMonster.index}/${i - 1}`).then(val => {
                                            if (val.canMove) {
                                                context.hp -= selectedMonster.attack;console.log(context.hp);
                                                context.hp = Math.max(context.hp, 0);
                                                slot.firstChild.firstChild.style.width = `${context.hp / context.initialHp * 100}%`;
                                                slot.firstChild.firstChild.innerText = `HP: ${context.hp}|Attack: ${context.attack}`;
                                                
                                                setTimeout(() => {
                                                    if (context.hp <= 0) {
                                                        slot.lastChild.remove();
                                                        slot.firstChild.remove();
                                                    }
                                                }, 100);

                                                if (val.gameEnds)window.location.replace("/game-over");

                                                selectedMonster = null;
                                            }

                                            if (val.turnEnds)endTurn(true);
                                        });
                                    }
                                    else if (tableSlotsA.includes(slot)) {
                                        selectedMonster = {...context};
                                        selectedMonster.index = i - 1;
                                    }
                                }
                            }
                        });
                        break;
                    }
                }
            }
        }
    }

    function endTurn(auto = false) {
        for (let slot of cardSlotsA) {
            flipReverseAnimation(slot);
            setTimeout (() => slot.classList.add("d-none"), 400);
        }

        let tmp = cardSlotsA;
        cardSlotsA = cardSlotsB;
        cardSlotsB = tmp;

        tmp = tableSlotsA;
        tableSlotsA = tableSlotsB;
        tableSlotsB = tmp;

        for (let slot of cardSlotsA) {
            slot.classList.remove("d-none");
        }

        playerHand = postData(`/api/new-turn/${auto}`, {turnEnds: true});
        showCards(playerHand);
    }

    showCards(playerHand);//todo: remove this
});
