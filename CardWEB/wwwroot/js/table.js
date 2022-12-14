window.addEventListener("load", () => {
    let selectedCard = null;
    let selectedCardContext = null;
    let selectedMonster = null;

    let selectedPower = null;

    let currentPlayer = "player A";

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

    async function postData(url = '', data = {}) {
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

    async function getData(url = '', data = {}) {
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

    const matchType = document.getElementById("match-type").value;

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

    let playerHand = postData(`/api/new-game/${matchType}`, { playerA: playerA, playerB: playerB });

    function showCards(hand, flip=true) {
        setTimeout(() => {
            hand.then(val => {
                let i = 0;
                for (let slot of cardSlotsA) {
                    try {
                        slot.firstChild.remove();
                    }
                    catch {

                    }

                    if (val[i] === undefined) continue;

                    const color = !val[i].isMonster ? "rgb(117, 64, 164)" : "rgb(146, 139, 65)";

                    const node = document.createElement("div");
                    node.id = `${slot.id}-${i}`;
                    node.classList.add("px-0", "py-0");
                    node.innerHTML = `<div name="card" class="card">
                                            <img src="/img/front.png" id="card-img" alt="background" class="card-image"
                                            style="background-color: ${color};">
                                            <h6 class="card-title ">${val[i].name}</h6>
                                            <img src="/img/${val[i].image}" alt="pollo" class="card-picture">
                                            <p class="card-description">${val[i].description}</p>
                                        </div>`;
                    slot.appendChild(node);

                    if (flip) {
                        flipAnimation(slot);
                    }
                    
                    addJS(slot, val[i]);
                    i++;
                }
                selectedCard = null;
                selectedMonster = null;
            }, 600);
        });
    }

    function flipAnimation(node) {
        node.animate([{ transform: "scaleX(0)" }, { transform: "scaleX(1)" }], { duration: 400, iterations: 1 });
    }

    function flipReverseAnimation(node) {
        node.animate([{ transform: "scaleX(1)" }, { transform: "scaleX(0)" }], { duration: 400, iterations: 1 });
    }

    function select(node=null, context=null) {
        selectedPower = null;

        showInfo(context, node);
        
        if (selectedCard != null) {
            selectedCard.classList.remove("selected");
        }
        if (node == null || node === selectedCard) {
            selectedCard = null;
            selectedMonster = null;
            selectedPower = null;
            showInfo();
            return;
        }
        
        selectedCard = node;
        selectedCardContext = {...context};
        node.classList.add("selected");

    }
    
    function showInfo(context=null, node=null) {
        const cardInfo = document.getElementById("card-info");
        
        cardInfo.querySelector("#powers").innerHTML = "";

        try {
            cardInfo.querySelector("#drop-btn-container").firstChild.remove()
        }
        catch {
            //do nothing
        }
        
        if (node == null) {
            cardInfo.querySelector("#image").innerHTML = `<img src="/img/back.png" alt="back" class="player-info-img">`;    
            cardInfo.querySelector("#info").innerText = "";
            return;
        }
        
        cardInfo.querySelector("#image").innerHTML = node.innerHTML;
        cardInfo.querySelector("#info").innerText = `${context.isMonster? `Monster [HP: ${context.hp}; Attack: ${context.attack}]: ` : "Effect: "} ${context.description}`;

        if (!context.isMonster) {
            cardInfo.querySelector("#powers").innerHTML = `<p class="text-success py-2 px-2">Power: ${context.powers[0]}</p>`;
        }
        
        else if (context.powers.length != 0){
            context.powers.forEach(el => {
                const div = document.createElement("div");
                const btn = document.createElement("button");

                div.classList.add("d-flex", "justify-content-center");

                btn.classList.add("btn", "btn-success", "mx-0", "my-2");
                btn.style.maxWidth = "85%";
                btn.style.minWidth = "85%";
                btn.innerText = el;

                btn.onclick = () => {
                    selectedPower = el;
                }

                div.append(btn);
                
                cardInfo.querySelector("#powers").append(div);
            });
        }

        
        if (cardSlotsA.includes(node.parentNode)) {
            const btn = document.createElement("button");
            
            btn.classList.add("btn", "btn-danger");
            btn.innerText = "Drop this card";
    
            btn.onclick = () => {
                postData(`/api/drop-card/${context.index}`).then(val => {
                    if (val.canMove) {
                        node.remove();
                        select();
                    }
    
                    if (val.gameEnds)window.location.replace(`/game-over/${currentPlayer}`); 
                });
            };
    
            cardInfo.querySelector("#drop-btn-container").append(btn);
        }
    }

    function addJS(node, context) {
        node.onclick = () => {
            select(node.firstChild, context);
        }

        node.ondblclick = () => {
            if (context.isMonster) {
                let i = 0;

                for (let slot of tableSlotsA) {
                    i++;
                    if (slot.children.length == 0) {
                        postData("/api/play", { CardIndex: context.index }).then(val => {
                            if (val.canMove) {
                                select(null);
                                const tmp = document.createElement("div");
                                const progress = document.createElement("div");

                                tmp.innerHTML = node.innerHTML;
                                tmp.classList.add("px-0", "py-0");
                                progress.classList.add("progress", "my-1");
                                progress.innerHTML = `<div class="progress-bar progress-bar-animated progress-bar-striped bg-success" style="width:100%;color:black;overflow:visible;">HP: ${context.hp}|Attack: ${context.attack}</div>`;

                                node.firstChild.remove();

                                slot.appendChild(progress);
                                slot.appendChild(tmp);

                                slot.onclick = () => {
                                    if (selectedMonster !== null && tableSlotsB.includes(slot)) {
                                        if (selectedPower != null) {
                                            postData(`/api/use-power/${selectedMonster.index}/${i - 1}/${selectedPower}`).then(val => {
                                                if (val.canMove) {
                                                    select();
                                                    showCards(new Promise((onResolve) => {onResolve(val.Hand)}), false);

                                                    if (val.turnEnds) {
                                                        updateTable(tableSlotsA, val.TableB);
                                                        updateTable(tableSlotsB, val.TableA);
                                                    }
                                                    else {
                                                        updateTable(tableSlotsA, val.TableA);
                                                        updateTable(tableSlotsB, val.TableB);
                                                    }

                                                    if (val.gameEnds) window.location.replace(`/game-over/${currentPlayer}`);

                                                    selectedMonster = null;
                                                }

                                                if (val.turnEnds) endTurn(true);
                                            });

                                            
                                        }
                                        else {
                                            postData(`/api/attack/${selectedMonster.index}/${i - 1}`).then(val => {
                                                if (val.canMove) {
                                                    select();
                                                    
                                                    if (val.turnEnds) {
                                                        updateTable(tableSlotsA, val.TableB);
                                                        updateTable(tableSlotsB, val.TableA);
                                                    }
                                                    else {
                                                        updateTable(tableSlotsA, val.TableA);
                                                        updateTable(tableSlotsB, val.TableB);
                                                    }
    
                                                    if (val.gameEnds) window.location.replace(`/game-over/${currentPlayer}`);
    
                                                    selectedMonster = null;
                                                }
    
                                                if (val.turnEnds) endTurn(true);
                                            });
                                        }
                                    }
                                    else if (tableSlotsA.includes(slot)) {
                                        if (selectedCard != null && !selectedCardContext.isMonster) {
                                            postData(`/api/equip/${selectedCardContext.index}/${i - 1}`).then(val => {
                                                if (val.canMove) {
                                                    context.powers.push(selectedCardContext.powers[0]);
                                                    const tmp = selectedCard;
                                                    select();
                                                    tmp.remove();
                                                }
                                            });
                                        }
                                        else {
                                            selectedMonster = { ...context };
                                            selectedMonster.index = i - 1;
                                            select(slot.lastChild, context);
                                        }
                                        
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

    function updateTable(tableSlots, newValues) {
        for (let i = 0; i < tableSlots.length; i++) {

            const slot = tableSlots[i];
            const val  = newValues[i];

            if (val === null) {
                try {
                    slot.firstChild.remove();
                    slot.lastChild.remove();
                }
                catch {
                    //do nothing
                }
            }

            else if (slot.children.length == 0) {
                const node = document.createElement("div");
                node.id = `${slot.id}-${i}`;
                node.classList.add("px-0", "py-0");
                node.innerHTML = `<div name="card" class="card px-0 py-0">
                                        <img src="/img/front.png" id="card-img" alt="background" class="card-image"
                                        style="background-color: rgb(146, 139, 65);">
                                        <h6 class="card-title ">${val.name}</h6>
                                        <img src="/img/${val.image}" alt="pollo" class="card-picture">
                                        <p class="card-description">${val.description}</p>
                                    </div>`;
                
                const progress = document.createElement("div");
                progress.classList.add("progress", "my-1");
                progress.innerHTML = `<div class="progress-bar progress-bar-animated progress-bar-striped bg-success" style="width:100%;color:black;overflow:visible;">HP: ${val.hp}|Attack: ${val.attack}</div>`;
                
                node.onclick = () => {
                    if (selectedMonster !== null) {
                        postData(`/api/attack/${selectedMonster.index}/${val.index}`).then(_val => {
                            if (_val.canMove) {
                                select();
                                
                                if (_val.turnEnds) {
                                    updateTable(tableSlotsA, _val.TableB);
                                    updateTable(tableSlotsB, _val.TableA);
                                }
                                else {
                                    updateTable(tableSlotsA, _val.TableA);
                                    updateTable(tableSlotsB, _val.TableB);
                                }

                                if (_val.gameEnds) window.location.replace(`/game-over/${currentPlayer}`);

                                selectedMonster = null;
                            }

                            if (_val.turnEnds) endTurn(true);
                        });
                    }
                };

                slot.appendChild(progress);
                slot.appendChild(node);
            }

            else {
                const newHp = val.hp;
                const oldHp = Number.parseInt(slot.querySelector(".progress-bar").innerText.match(/HP: [0-9]+/)[0].match(/[0-9]+/));
                
                const oldWidth = Number.parseInt(slot.querySelector(".progress-bar").style.width.slice(0, -1));
                const newWidth = Math.min((newHp / ((100 * oldHp) / oldWidth)) * 100, 100) + "%";
                
                slot.querySelector(".progress-bar").innerText = `HP: ${newHp}|Attack: ${val.attack}`;
                slot.querySelector(".progress-bar").style.width = newWidth;
            }
        }
    }

    function endTurn(auto = false) {
        if (matchType !== "ai") {
            for (let slot of cardSlotsA) {
                flipReverseAnimation(slot);
                setTimeout(() => slot.classList.add("d-none"), 400);
            }
        }

        let tmp = cardSlotsA;
        cardSlotsA = cardSlotsB;
        cardSlotsB = tmp;

        tmp = tableSlotsA;
        tableSlotsA = tableSlotsB;
        tableSlotsB = tmp;

        if (currentPlayer == "player A") currentPlayer = "player B";
        else currentPlayer = "player A";

        if (matchType !== "ai") {
            for (let slot of cardSlotsA) {
                slot.classList.remove("d-none");
            }
        }
        
        if (matchType === "ai" && currentPlayer === "player B" && !auto) {
            postData((`/api/new-turn/${auto}`)).then(val => {
                updateTable(tableSlotsA, val.TableA);
                updateTable(tableSlotsB, val.TableB);
            });
            endTurn(true);
            
        }
        else {
            playerHand = postData(`/api/new-turn/${auto}`);
            showCards(playerHand);
        }

        select();
    }

    showCards(playerHand);//todo: remove this
});
