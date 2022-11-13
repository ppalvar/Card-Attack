window.addEventListener("load", () => {
    const createBtn = document.getElementById("create-btn");

    createBtn.addEventListener("click", () => {
        //retrieve info
        const image = document.getElementById("img-frame").src.split('/')[4];
        const name = document.getElementById("card-name").value;
        const element = document.getElementById("card-element").value;
        const aProb = Number.parseFloat(document.getElementById("card-probability").value);
        const hp = Number.parseInt(document.getElementById("monster-hp").value);
        const attack = Number.parseInt(document.getElementById("monster-attack").value);
        
        // console.log(`Img:${image}\nName:${name}\nElement:${element}\nProb:${aProb}\nHP:${hp}\nAttack:${attack}`);
        
        //validate info
        if (Number.isInteger(hp) && Number.isInteger(attack) && !Number.isNaN(aProb)
            && image !== "" && name !== "" && element !=="") {
            console.log("valid");//todo: fetch this
        }
        else {
            alert("There is some wrong data");
        }
    });
});