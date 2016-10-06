//Initializing the queue variables
var front = 0, rear = 0;

// a queue for storing the server names
var server_names = [];

var ChartDivIds = new Array;

var id_Strings = new Array;


var arrayForChart = new Array;


// A queue for knowing the div number showing the chart and the chart type of that chart
var chart_Div_Queue = new Array;


// Load the Visualization API and the corechart package.
google.charts.load('current', { 'packages': ['corechart'] });

function grap() {
    // Set a callback to run when the Google Visualization API is loaded.
    google.charts.setOnLoadCallback(drawChart);

    // Callback that creates and populates a data table,
    // instantiates the pie chart, passes in the data and
    // draws it.
    function drawChart() {

        // Create the data table.
        var data = new google.visualization.DataTable();
        var chartTitle;
        // 'string' and 'number' here denote the datatype of the variable we are sending
        if (id_Strings[front][1] == 1) {
            chartTitle = "CPU Memory Usage";
            data.addColumn('string', 'Time Stamp');
            data.addColumn('number', 'CPU Usage');
            data.addColumn('number', 'Memory Usage');
        }

        else if (id_Strings[front][1] == 2) {
            chartTitle = "Temp DB Growth Trend";
            data.addColumn('string', 'Time Stamp');

            var TempDB_variable = new Array;
            
            
            var i;
            var file_names = [];

            for (i = 0; i < arrayForChart[front].length; i++) {
                file_names.push(arrayForChart[front][i][1]);
                if (file_names.indexOf(arrayForChart[front][i][1]) != (file_names.length-1)) {
                    file_names.pop();
                }
            }

            var columnCounter = 0;
            for (i = 0; i < file_names.length; i++) {
                data.addColumn('number', file_names[i]);
                columnCounter = columnCounter + 1;
            }

            if (i != 0) {

                TempDB_variable.push([]);
                TempDB_variable[TempDB_variable.length - 1].push(0);
                for (var k = 0; k < columnCounter; k++) {
                    TempDB_variable[TempDB_variable.length - 1].push(0);
                }
                for (i = 0; i < arrayForChart[front].length; i++) {

                    if (i == 0) {

                        TempDB_variable[0][0] = arrayForChart[front][i][0];
                        TempDB_variable[0][file_names.indexOf(arrayForChart[front][i][1]) + 1] = arrayForChart[front][i][2];
                        
                        
                    }
                    else {
                        if (arrayForChart[front][i][0] == TempDB_variable[TempDB_variable.length - 1][0]) {
                            TempDB_variable[TempDB_variable.length - 1][file_names.indexOf(arrayForChart[front][i][1]) + 1] = arrayForChart[front][i][2];
                            
                        }
                        else {

                            TempDB_variable.push([]);
                            TempDB_variable[TempDB_variable.length - 1].push(0);
                            for (var k = 0; k < columnCounter; k++) {
                                TempDB_variable[TempDB_variable.length - 1].push(0);
                            }
              

                            TempDB_variable[TempDB_variable.length - 1][0] = arrayForChart[front][i][0];
                            TempDB_variable[TempDB_variable.length - 1][file_names.indexOf(arrayForChart[front][i][1]) + 1] = arrayForChart[front][i][2];
                            
                        }
                    }    
                }
            }

            arrayForChart[front] = TempDB_variable;

        }
        else if (id_Strings[front][1] == 3) {
        
            chartTitle = "Multiple Session Analysis";
            data.addColumn('string', 'Time Stamp');

            var Multiple_Session_variable = new Array;


            var i;
            var user_names = [];

            for (i = 0; i < arrayForChart[front].length; i++) {
                user_names.push(arrayForChart[front][i][1]);
                if (user_names.indexOf(arrayForChart[front][i][1]) != (user_names.length - 1)) {
                    user_names.pop();
                }
            }

            //console.log(user_names);

            
            var columnCounter = 0;
            for (i = 0; i < user_names.length; i++) {
                data.addColumn('number', user_names[i]);
                columnCounter = columnCounter + 1;
            }

            if (i != 0) {

                Multiple_Session_variable.push([]);
                Multiple_Session_variable[Multiple_Session_variable.length - 1].push(0);
                for (var k = 0; k < columnCounter; k++) {
                    Multiple_Session_variable[Multiple_Session_variable.length - 1].push(0);
                }
                for (i = 0; i < arrayForChart[front].length; i++) {

                    if (i == 0) {

                        Multiple_Session_variable[0][0] = arrayForChart[front][i][0];
                        Multiple_Session_variable[0][user_names.indexOf(arrayForChart[front][i][1]) + 1] = arrayForChart[front][i][2];


                    }
                    else {
                        if (arrayForChart[front][i][0] == Multiple_Session_variable[Multiple_Session_variable.length - 1][0]) {
                            Multiple_Session_variable[Multiple_Session_variable.length - 1][user_names.indexOf(arrayForChart[front][i][1]) + 1] = arrayForChart[front][i][2];

                        }
                        else {

                            Multiple_Session_variable.push([]);
                            Multiple_Session_variable[Multiple_Session_variable.length - 1].push(0);
                            for (var k = 0; k < columnCounter; k++) {
                                Multiple_Session_variable[Multiple_Session_variable.length - 1].push(0);
                            }


                            Multiple_Session_variable[Multiple_Session_variable.length - 1][0] = arrayForChart[front][i][0];
                            Multiple_Session_variable[Multiple_Session_variable.length - 1][user_names.indexOf(arrayForChart[front][i][1]) + 1] = arrayForChart[front][i][2];

                        }
                    }
                }
            }

            
            arrayForChart[front] = Multiple_Session_variable;
            
        }


        //adding the data to show in the Google Datatable
        data.addRows(arrayForChart[front]);

        // chart options . These will be different for different charts
        var options;
        // Variable to recieve the chart object we make
        var chart;
        // To decide which kind of chart we have to load



        switch (id_Strings[front][2]) {
            // Denotes Bar Chart 
            case 1:
                var options = { 'title': chartTitle + ' Analytics',
                                 'width' : 700,
                                 'height' : 600
                             };
                chart = new google.visualization.ColumnChart(document.getElementById(id_Strings[front][0]));
                break;
            //Denotes Line Chart 
            case 2:
                var options = { 'title': chartTitle + ' Analytics',
                                    'width' : 700,
                                 'height' : 600,
                                 'pointSize' : 7
                              };
                
                chart = new google.visualization.LineChart(document.getElementById(id_Strings[front][0]));
                break;
            // Denotes XY-scatter chart
            case 3:
                var options = { 'title': chartTitle + ' Analytics',
                                    'width' : 700,
                                 'height' : 600
                              };
                
                chart = new google.visualization.ScatterChart(document.getElementById(id_Strings[front][0]));
                break;
            
        }
        
        //Call the draw() asynchronous method to draw the chart
        chart.draw(data, options);

        // incrementing the front's value
        front = front + 1;
    }

}



// Remember an important thing
//this is all working asynchronously
//that's why a queue was implemented
