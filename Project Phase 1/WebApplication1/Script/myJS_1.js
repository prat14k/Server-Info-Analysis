//Initializing the queue variables
var front = 0, rear = 0;

// a queue for storing the File names
var file_names = [];

// A queue for storing the Server Information read from the file
var server_info = new Array;

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

        // 'string' and 'number' here denote the datatype of the variable we are sending
        data.addColumn('string', 'Name');
        data.addColumn('number', 'Float');

        //adding the data to show in the Google Datatable
        data.addRows(server_info[front]);

        // chart options . These will be different for different charts
        var options;
        // Variable to recieve the chart object we make
        var chart1,chart2;
        // To decide which kind of chart we have to load
        switch (chart_Div_Queue[front][1]) {
            // Denotes Bar Chart 
            case 1:
                var options = { 'title': file_names[front] + ' Analytics',
                                 'width' : 700,
                                 'height' : 600
                              };
                        chart1 = new google.visualization.ColumnChart(document.getElementById("charts" + front.toString()));
                        chart2 = new google.visualization.ColumnChart(document.getElementById("MainWindowCharts" + front.toString()));
                break;
            //Denotes Line Chart 
            case 2:
                var options = { 'title': file_names[front] + ' Analytics',
                                    'width' : 700,
                                 'height' : 600
                              };

                             chart1 = new google.visualization.LineChart(document.getElementById("charts" + front.toString()));
                             chart2 = new google.visualization.LineChart(document.getElementById("MainWindowCharts" + front.toString()));
                break;
            //Denotes Pie Chart
            case 3:
                var options = { 'title': file_names[front] + ' Analytics',
                                'is3D' : 'true',
                                'width' : 700,
                                 'height' : 600      
                              };

                             chart2 = new google.visualization.PieChart(document.getElementById("MainWindowCharts" + front.toString()));
                             chart1 = new google.visualization.PieChart(document.getElementById("charts" + front.toString()));
                break;
            // Denotes XY-scatter chart
            case 4:
                var options = { 'title': file_names[front] + ' Analytics',
                                    'width' : 700,
                                 'height' : 600
                              };

                             chart2 = new google.visualization.ScatterChart(document.getElementById("MainWindowCharts" + front.toString()));
                             chart1 = new google.visualization.ScatterChart(document.getElementById("charts" + front.toString()));
                break;
            //Denotes Donut Chart
            case 5:
                var options = { 'title': file_names[front] + ' Analytics',
                                'pieHole' : 0.4,
                                'width' : 700,
                                 'height' : 600
                              };

                             chart2 = new google.visualization.PieChart(document.getElementById("MainWindowCharts" + front.toString()));
                             chart1 = new google.visualization.PieChart(document.getElementById("charts" + front.toString()));    
                break;
        }
        
        //Call the draw() asynchronous method to draw the chart
        chart1.draw(data, options);
        chart2.draw(data, options);
        
        // incrementing the front's value
        front = front + 1;
    }

}



// Remember an important thing
//this is all working asynchronously
//that's why a queue was implemented
