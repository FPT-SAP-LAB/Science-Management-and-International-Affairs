function deleteCost(ele) {
    ele.closest("tr").remove();
    calculateTotalCost();
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ",");
}

function calculateCost(ele) {
    let parentTr = ele.closest("tr");
    let targetCost = parentTr.querySelector(".targetCost");
    try {
        let resultCal = eval(ele.value.replaceAll(",", ""));
        if (resultCal == undefined)
            targetCost.innerHTML = "0 đ"
        else
            targetCost.innerHTML = numberWithCommas(resultCal) + " đ"
    } catch (e) {
        targetCost.innerHTML = "0 đ"
    }
    calculateTotalCost();
}

function calculateTotalCost() {
    let totalCost = 0;
    let partialCosts = document.getElementsByClassName("targetCost");
    for (var i = 0; i < partialCosts.length; i++) {
        totalCost += parseInt(partialCosts[i].innerHTML.replaceAll(",", "").split(" ")[0])
    }
    document.getElementById("totalCost").innerHTML = numberWithCommas(totalCost) + " đ"
}