function table_to_Json() {
    var myRows = [];
    var $headers = $("th");
    //var $rows =
    $("tbody tr").each(function (index) {
        $cells = $(this).find("td");
        myRows[index] = {};
        $cells.each(function (cellIndex) {
            myRows[index][$($headers[cellIndex]).html()] = $(this).html();
        });
    });
    // Let's put this in the object like you want and convert to JSON (Note: jQuery will also do this for you on the Ajax request)
    var myObj = {};
    myObj.myrows = myRows;
    return myObj
}
window.saveFile = function saveFile() {
    var data = table_to_Json()
    console.log(data)
    var data1 = data["myrows"];
    var opts = [{ sheetid: 'One', header: true }, { sheetid: 'Two', header: false }];
    //var res =
        alasql('SELECT * INTO XLSX("export.xlsx",?) FROM ?', [opts, [data1]]);
}
