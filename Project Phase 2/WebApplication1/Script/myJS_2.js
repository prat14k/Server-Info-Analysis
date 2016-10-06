
function errorAlert(errorMessage) {
    console.log(errorMessage);
    alert(errorMessage);
}


//function called by the c# backend code after reading the information from the xml file
function showCharts() {



    // A queue for storing the Server Information read from the file
    var CPU_Memory_Info_queue;
    var TempDB_Info_queue;
    var Multiple_Sessions_Info_queue;


    //. It is joined by a separator '|'. So to get them, we need to split it. 
    var hiddenGraphsValues = document.getElementById("Hidden_Fields_Info").value.split("|");

    // Hidden Field with id 'file_names' has the names of the files selected by the user. It is joined by using a separator '#'. So, to get them , we split them.
    server_names = document.getElementById("server_names").value.split("#");
    document.getElementById("links").innerHTML = "";

    // a loop to show the graphs
    for (var num = 0; num < hiddenGraphsValues.length; num++) {


        // Each of the values in the hiddenGraphsValues contains a pair having the hidden field number of the hidden fields as their id's are just prefixed by 'hidd'
        // and the chart type for them
        // they are joined by '-' . So, again we need to split them to get them individually 
        var hiddenGraphsValuesArray = hiddenGraphsValues[num].split("-");

        var UserChosenChartType = parseInt(hiddenGraphsValuesArray[1]);

        // checked if their is not an error regarding the value of the chart Type as the value should range between 1 to 5
        // if there is an error, revert back and continue with the next values 
        if (UserChosenChartType < 1 || UserChosenChartType > 3) {
            continue;
        }


        // a new div Title is created for the showing of charts
        document.getElementById("links").innerHTML = document.getElementById("links").innerHTML + "<li><h4><a href=\"#" + server_names[rear] + "\"> " + server_names[rear] + " Analysis </a></h4></li>";


        xmlInfo = document.getElementById("Panel" + hiddenGraphsValuesArray[0]).getElementsByTagName("input");

        CPU_Memory_Info_queue = xmlInfo[0].value.split("#");
        for (var i = 0; i < CPU_Memory_Info_queue.length; i++) {
            CPU_Memory_Info_queue[i] = CPU_Memory_Info_queue[i].split("|");
            CPU_Memory_Info_queue[i][2] = parseFloat(CPU_Memory_Info_queue[i][2]);
            CPU_Memory_Info_queue[i][1] = parseFloat(CPU_Memory_Info_queue[i][1]);
        }
        id_Strings.push([server_names[rear] + "_CPU_Memory_Usage_Chart", 1, UserChosenChartType]);
        arrayForChart.push(CPU_Memory_Info_queue)


        grap();

        TempDB_Info_queue = xmlInfo[1].value.split("#");
        for (i = 0; i < TempDB_Info_queue.length; i++) {
            TempDB_Info_queue[i] = TempDB_Info_queue[i].split("|");
            TempDB_Info_queue[i][2] = parseFloat(TempDB_Info_queue[i][2]);
        }
        id_Strings.push([server_names[rear] + "_Temp_DB_Chart", 2, UserChosenChartType]);
        arrayForChart.push(TempDB_Info_queue)


        Multiple_Sessions_Info_queue = xmlInfo[2].value.split("#");
        for (i = 0; i < Multiple_Sessions_Info_queue.length; i++) {
            Multiple_Sessions_Info_queue[i] = Multiple_Sessions_Info_queue[i].split("|");
            Multiple_Sessions_Info_queue[i][2] = parseFloat(Multiple_Sessions_Info_queue[i][2]);
        }
        id_Strings.push([server_names[rear] + "_Multiple_Sessions_Chart", 3, UserChosenChartType]);
        arrayForChart.push(Multiple_Sessions_Info_queue)
        grap();        
        


        //rear value is incremented
        rear = rear + 1;

        // the function is called for the showing of the chart
        grap();

    }

}



// This is Javascript code to just disable and enable the dropdown list if the checkbox is selected for the file or not
// It also puts an onClick Event Listener on the checkBoxes of the same thing.

window.onload = function () {
    var row = document.getElementById("GridView1").getElementsByTagName("td");
    for (var i = 0; i < row.length; i += 5) {
        if (row[i].children[0].checked == true) {
            var columns = row[i].parentElement.getElementsByTagName("td");
            columns[2].children[0].removeAttribute("disabled");
            columns[3].children[0].removeAttribute("disabled");
            columns[4].children[0].removeAttribute("disabled");
        }
        row[i].onclick = function () {

            var columns = this.parentElement.getElementsByTagName("td");
            if (this.children[0].checked == true) {
                columns[2].children[0].removeAttribute("disabled");
                columns[3].children[0].removeAttribute("disabled");
                columns[4].children[0].removeAttribute("disabled");
            }
            else {
                columns[2].children[0].setAttribute("disabled", "true");
                columns[3].children[0].setAttribute("disabled", "true");
                columns[4].children[0].setAttribute("disabled", "true");
            }
        }
    }

    $(".startDatePicker").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true
    });

    var date = new Date();
    date.setMonth(date.getMonth() - 2);
    $(".startDatePicker").datepicker('setStartDate', date);
    $(".startDatePicker").datepicker('setEndDate', new Date());


    $(".endDatePicker").datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true
    });

    $(".endDatePicker").datepicker('setStartDate', date);
    $(".endDatePicker").datepicker('setEndDate', new Date());


    $(".startDatePicker").on('changeDate', function (selected) {
        var node = selected.currentTarget.parentElement.parentElement.children[4].children[0];

        if (selected.date != null) {
            date = new Date(selected.date.valueOf());
            $(node).datepicker('setStartDate', date);
        }
        else {
            var date = new Date();
            date.setMonth(date.getMonth() - 2);
            $(node).datepicker('setStartDate', date);
        }
    });

    $(".endDatePicker").on('changeDate', function (selected) {
        var node = selected.currentTarget.parentElement.parentElement.children[3].children[0];
        if (selected.date != null) {
            date = new Date(selected.date.valueOf());
            $(node).datepicker('setEndDate', date);
        }
        else {
            $(node).datepicker('setEndDate', new Date());
        }
    });

}