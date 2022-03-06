function setupPasteEvent(helper){
    const textArea = document.getElementById("roundData");
    textArea.addEventListener("paste", async (e) => {
       let paste = (e.clipboardData || window.clipBoardData).getData("text");
       await helper.invokeMethodAsync("CheckResult", paste);
    });
}

function switchTab(id){
    const elements = document.getElementsByClassName("tab-pane");
    const elementsBtn = document.getElementsByClassName("nav-link");
    const target = document.getElementById(id);
    const targetBtn = document.getElementById(`btn${id}`);
    
    Array.from(elements).map((t) => {
            t.classList.remove("show");  
            t.classList.remove("active");  
        } );
    Array.from(elementsBtn).map((b) => b.classList.remove("active"));
    target.classList.add("show");
    target.classList.add("active");
    targetBtn.classList.add("active");
}