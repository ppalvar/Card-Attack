window.addEventListener("load", () => {
    const editorEl1 = document.getElementById("power-1");
    const editor1 = window.ace.edit(editorEl1);
    editor1.session.setMode(`ace/mode/csharp`);
    // editor1.setTheme(`ace/theme/terminal`);
    editor1.on('change', data => {
        console.log('edited result', data);
    });

    const editorEl2 = document.getElementById("power-2");
    const editor2 = window.ace.edit(editorEl2);
    editor2.session.setMode(`ace/mode/csharp`);
    // editor2.setTheme(`ace/theme/terminal`);
    editor2.on('change', data => {
        console.log('edited result', data);
    });

    const editorEl3 = document.getElementById("power-3");
    const editor3 = window.ace.edit(editorEl3);
    editor3.session.setMode(`ace/mode/csharp`);
    // editor3.setTheme(`ace/theme/terminal`);
    editor3.on('change', data => {
        console.log('edited result', data);
    });

    const createBtn = document.getElementById("create-btn");

    createBtn.addEventListener("click", () => {
        //retrieve data
        const code1 = editorEl1.children[2].innerText;
        const code2 = editorEl2.children[2].innerText;
        const code3 = editorEl3.children[2].innerText;
        const image = document.getElementById("img-frame").src.split('/')[4];
        const name = document.getElementById("card-name").value;
        const element = document.getElementById("card-element").value;
        const aProb = Number.parseFloat(document.getElementById("card-probability").value);

        //validate data
        if (!Number.isNaN(aProb) && image !== "" && name !== "" && element !=="") {
            console.log("valid");//todo: fetch this
        }
        else {
            alert("There is some wrong data");
        }
    });
});