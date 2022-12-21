window.addEventListener("load", () => {
    const editorEl1 = document.getElementById("power");
    const editor1 = window.ace.edit(editorEl1);
    editor1.session.setMode(`ace/mode/csharp`);
    // editor1.setTheme(`ace/theme/terminal`);
    editor1.on('change', data => {
        console.log('edited result', data);
    });

    const createBtn = document.getElementById("create-btn");

    createBtn.addEventListener("click", () => {
        //retrieve data
        const code = editor1.getSession().getValue();
        const image = document.getElementById("img-frame").src.split('/')[4];
        const name = document.getElementById("card-name").value;
        const description = document.getElementById("card-description").value;
        const aProb = Number.parseFloat(document.getElementById("card-probability").value);

        //validate data
        if (!Number.isNaN(aProb) && image !== "" && name !== "") {
            postData("/api/new-effect-card", {
                name : name,
                description : description,
                image : image,
                appearingProbability : aProb,
                powerName : name,
                powerCode : code,
            }).then (val => {
                if (val.isValid) {
                    alert("Success!");
                    window.location.replace("/");
                }
                else {
                    alert(val.errors);
                }
            });
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
        return response.json();
    }
});