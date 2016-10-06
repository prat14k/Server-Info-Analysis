
//function called by the c# backend code after reading the information from the xml file
function showCharts() {
    
    //. It is joined by a separator '|'. So to get them, we need to split it. 
    var hiddenGraphsValues = document.getElementById("Hidden_Fields_Info").value.split("|");
    
    // Hidden Field with id 'file_names' has the names of the files selected by the user. It is joined by using a separator '#'. So, to get them , we split them.
    file_names = document.getElementById("file_names").value.split("#");


    // a loop to show the graphs
    for (var num = 0; num < hiddenGraphsValues.length; num++) {

        // Each of the values in the hiddenGraphsValues contains a pair having the hidden field number of the hidden fields as their id's are just prefixed by 'hidd'
        // and the chart type for them
        // they are joined by '-' . So, again we need to split them to get them individually 
        var arrayy = hiddenGraphsValues[num].split("-");

        //both pushed into the queue
        chart_Div_Queue.push([parseInt(arrayy[0]), parseInt(arrayy[1])]);

        // checked if their is not an error regarding the value of the chart Type as the value should range between 1 to 5
        // if there is an error, revert back and continue with the next values 
        if (chart_Div_Queue[rear][1] < 1 || chart_Div_Queue[rear][1] > 5) {
            continue;
        }

        // get the xml file information stored in the hidden field 
        // these are three fields name,int1,float1 , all separated by '#'
        xmlInfo = document.getElementById("hidd" + arrayy[0]).value.split("#");

        var name, int1, float1;
        // Inside all the xml fields,(i.e., name,int1,float1) , they are joined by a separator '|' 
        name = xmlInfo[0].split("|");
        int1 = xmlInfo[1].split("|");
        float1 = xmlInfo[2].split("|");

        // this is done to make a new array in the server_info array
        server_info.push([]);

        // information is pushed into the array
        for (var i = 0; i < name.length; i++) {
            server_info[rear].push([name[i].trim(), parseFloat(float1[i].trim())]);
        }

        // a new div is created for the showing of chart
        document.getElementById("chart_div").innerHTML = document.getElementById("chart_div").innerHTML + "<div id=\"charts" + rear.toString() + "\"></div><br><br>";
        document.getElementById("mainWindowCharts").innerHTML = document.getElementById("mainWindowCharts").innerHTML + "<div id=\"MainWindowCharts" + rear.toString() + "\"></div><br><br>";

        //rear value is incremented
        rear = rear + 1;

        // the function is called for the showing of the chart
        grap();

    }

}


window.onload = function () {
    var row = document.getElementById("GridView1").getElementsByTagName("td");
    for (var i = 0; i < row.length; i += 3) {
        if (row[i].children[0].checked == true) {
            row[i].parentElement.getElementsByTagName("td")[2].children[0].removeAttribute("disabled");
        }
        row[i].onclick = function () {

            if (this.children[0].checked == true) {
                this.parentElement.getElementsByTagName("td")[2].children[0].removeAttribute("disabled");
            }
            else {
                this.parentElement.getElementsByTagName("td")[2].children[0].setAttribute("disabled", "true");
            }
        }
    }

}