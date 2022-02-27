function setupPasteEvent(helper){
    const textArea = document.getElementById("roundData");
    textArea.addEventListener("paste", async (e) => {
       let paste = (e.clipboardData || window.clipBoardData).getData("text");
       await helper.invokeMethodAsync("CheckResult", paste);
    });
}