window.addEventListener("load", () => {
    const createBtn = document.getElementById("create-btn");

    createBtn.addEventListener("click", () => {
        //retrieve info
        const image = document.getElementById("img-frame").src.split('/')[4];
        const name = document.getElementById("card-name").value;
        const description = document.getElementById("card-description").value;
        const element = document.getElementById("card-element").value;
        const aProb = Number.parseFloat(document.getElementById("card-probability").value);
        const hp = Number.parseInt(document.getElementById("monster-hp").value);
        const attack = Number.parseInt(document.getElementById("monster-attack").value);

        // console.log(`Img:${image}\nName:${name}\nElement:${element}\nProb:${aProb}\nHP:${hp}\nAttack:${attack}`);

        //validate info
        if (Number.isInteger(hp) && Number.isInteger(attack) && !Number.isNaN(aProb) && image !== "" && name !== "" && element !== "") {
            try {
                let response = postData("/api/new-monster-card", {
                    name : name,
                    description : description,
                    image : image,
                    appearingProbability : aProb,
                    element : element,
                    attack : attack,
                    hp : hp,
                });
            }
            catch (e) {
                
            }
            finally {
                window.location.replace("/");
            }
        }
        else {
            alert("There is some wrong data");
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
        //return response.json(); // parses JSON response into native JavaScript objects
    }
});