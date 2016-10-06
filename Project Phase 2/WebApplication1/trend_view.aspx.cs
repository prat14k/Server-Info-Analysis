using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ServerInformationAnalysis
{
    public partial class TrendAnalysis : System.Web.UI.Page
    {

        static DataTable checkedServerInfo = new DataTable();
        static List<DataTable> queryOutputCollection = new List<DataTable>();

        protected void Page_Load(object sender, EventArgs e)
        {   
            //checking if the file is loaded for the first time or not
            if(!IsPostBack)
                //if loaded for the first time, then run the method
                SetInitialGridView();
        }


        private void SetInitialGridView()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=INPDDBA027\NGEP;Database=Dev_Server;Integrated Security=SSPI");
            SqlCommand command = new SqlCommand("Select FS_UID, Server_Name FROM FluorSchema.T_Server_Info",connection);
            connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            
            //making the gridView source to be our dataTable
            GridView1.DataSource = dataReader;

            //binding the data as per columns of the datatable and the gridview
            GridView1.DataBind();

            dataReader.Close();
            connection.Close();

            GridView1.Columns[1].Visible = false;
            //end of method

            addColumnsToDatatable();
            Export2ExcelButton.Visible = false;
        }


        private void addColumnsToDatatable()
        {
            checkedServerInfo.Columns.Clear();

            checkedServerInfo.Columns.Add("ServerName", typeof(String));
            checkedServerInfo.Columns.Add("ChartType", typeof(String));
            checkedServerInfo.Columns.Add("StartDate", typeof(String));
            checkedServerInfo.Columns.Add("EndDate", typeof(String));
        }

        protected void CPU_Chart_Info(Panel serverChartsHolder,String serverName,Panel InfoCollector,SqlDataReader dataReader)
        {
            serverChartsHolder.Controls.Add(new LiteralControl("<div class=\"row\">"));

            Panel CPU_Memory_Usage_Chart = new Panel();
            CPU_Memory_Usage_Chart.ID = serverName + "_CPU_Memory_Usage_Chart";
            CPU_Memory_Usage_Chart.CssClass = "col-sm-12 col-xs-12";

            serverChartsHolder.Controls.Add(CPU_Memory_Usage_Chart);

            HiddenField CPU_Memory_Info = new HiddenField();
            CPU_Memory_Info.ID = serverName + "CPU_Memory_Info";
            CPU_Memory_Info.Value = "";

            if (dataReader.HasRows)
            {
                DataTable dt = new DataTable(serverName + "_CPUMemoryOutput");
                dt.Columns.Add("Time_Stamp", typeof(String));
                dt.Columns.Add("CPU Usage", typeof(String));
                dt.Columns.Add("Memory Usage", typeof(String));
                DataRow newRow = null;
                while (dataReader.Read())
                {
                    newRow = dt.NewRow();
                    newRow["Time_Stamp"] = dataReader.GetDateTime(2).ToString();
                    newRow["CPU Usage"] = dataReader.GetString(0).ToString();
                    newRow["Memory Usage"] = dataReader.GetString(1).ToString();
                    dt.Rows.Add(newRow);
                    CPU_Memory_Info.Value += dataReader.GetDateTime(2).ToString() + "|" + dataReader.GetString(0).ToString() + "|" + dataReader.GetString(1) + "#";
                }
                queryOutputCollection.Add(dt);
            }
            if (CPU_Memory_Info.Value != "")
                CPU_Memory_Info.Value = CPU_Memory_Info.Value.Remove(CPU_Memory_Info.Value.Length - 1);
            
            InfoCollector.Controls.Add(CPU_Memory_Info);
        
            dataReader.Close();


            serverChartsHolder.Controls.Add(new LiteralControl("</div><br>"));
            
            //end of method
        }


        protected void Top_Queries_Table_Info(Panel serverChartsHolder, String serverName, Panel InfoCollector, SqlDataReader dataReader)
        {
            serverChartsHolder.Controls.Add(new LiteralControl("<div class=\"row\">"));

            Panel Top_Ten_queries = new Panel();
            Top_Ten_queries.CssClass = "col-sm-6 col-xs-12";
            Top_Ten_queries.ID = serverName + "_Top_Ten_queries";
            serverChartsHolder.Controls.Add(Top_Ten_queries);

            if (dataReader.HasRows)
            {
                DataTable dt = new DataTable(serverName + "_ExpensiveQueriesOutput");
                dt.Load(dataReader);

                GridView tableGrid = new GridView();
                tableGrid.ID = serverName + "_Grid";
                tableGrid.CssClass = "table table-striped table-hover";
                tableGrid.EmptyDataText = "There are no data records to display";
                tableGrid.DataSource = dt;
                tableGrid.DataBind();
                tableGrid.HeaderRow.Style["background-color"] = "#373a3c";
                tableGrid.HeaderRow.Style["color"] = "#fff";
                Top_Ten_queries.Controls.Add(tableGrid);

                queryOutputCollection.Add(dt);
            }


            dataReader.Close();

            serverChartsHolder.Controls.Add(new LiteralControl("</div><br>"));
            //end of method
        }


        protected void Multiple_Sessions_Chart_Info(Panel serverChartsHolder, String serverName, Panel InfoCollector, SqlDataReader dataReader)
        {
            serverChartsHolder.Controls.Add(new LiteralControl("<div class=\"row\">"));

            Panel Multiple_Sessions_Chart = new Panel();
            Multiple_Sessions_Chart.CssClass = "col-sm-6 col-xs-12";
            Multiple_Sessions_Chart.ID = serverName + "_Multiple_Sessions_Chart";

            serverChartsHolder.Controls.Add(Multiple_Sessions_Chart);
            
            
            HiddenField Multiple_Sessions_Info = new HiddenField();
            Multiple_Sessions_Info.ID = serverName + "Multiple_Sessions_Chart";
            Multiple_Sessions_Info.Value = "";

            if (dataReader.HasRows)
            {
                DataTable dt = new DataTable(serverName + "_MultipleSessionsOutput");
                dt.Columns.Add("Time_Stamp", typeof(String));
                dt.Columns.Add("User_ID", typeof(String));
                dt.Columns.Add("Session_Count", typeof(String));

                DataRow newRow = null;
                while (dataReader.Read())
                {
                    newRow = dt.NewRow();
                    newRow["Time_Stamp"] = dataReader.GetDateTime(2).ToString();
                    newRow["User_ID"] = dataReader.GetString(0).ToString();
                    newRow["Session_Count"] = dataReader.GetString(1).ToString();
                    //TempDB_FileNames.Value += dataReader.GetString(0).ToString() + "|";
                    dt.Rows.Add(newRow);
                    Multiple_Sessions_Info.Value += dataReader.GetDateTime(2).ToString() + "|" + dataReader.GetString(0).ToString() + "|" + dataReader.GetString(1) + "#";
                }
                queryOutputCollection.Add(dt);
            
            }
            if (Multiple_Sessions_Info.Value != "")
                Multiple_Sessions_Info.Value = Multiple_Sessions_Info.Value.Remove(Multiple_Sessions_Info.Value.Length - 1);

            InfoCollector.Controls.Add(Multiple_Sessions_Info);

            dataReader.Close();

            serverChartsHolder.Controls.Add(new LiteralControl("</div><br>"));

            //end of method
        }


        protected void TempDB_SizeGrowth_Chart_Info(Panel serverChartsHolder, String serverName, Panel InfoCollector, SqlDataReader dataReader)
        {
            serverChartsHolder.Controls.Add(new LiteralControl("<div class=\"row\">"));

            Panel Temp_DB_Chart = new Panel();
            Temp_DB_Chart.CssClass = "col-sm-6 col-xs-12";
            Temp_DB_Chart.ID = serverName + "_Temp_DB_Chart";

            serverChartsHolder.Controls.Add(Temp_DB_Chart);

            /*
            HiddenField TempDB_FileNames = new HiddenField();
            TempDB_FileNames.ID = serverName + "Temp_DB_Names";
            TempDB_FileNames.Value = "";

            */
            HiddenField TempDB_Info = new HiddenField();
            TempDB_Info.ID = serverName + "Temp_DB_Chart";
            TempDB_Info.Value = "";

             
            if (dataReader.HasRows)
            {
                DataTable dt = new DataTable(serverName + "_TempDBOutput");
                dt.Columns.Add("Time_Stamp", typeof(String));
                dt.Columns.Add("DB_FileName", typeof(String));
                dt.Columns.Add("DB_Growth_Value", typeof(String));
                
                DataRow newRow = null;
                while (dataReader.Read())
                {
                    newRow = dt.NewRow();
                    newRow["Time_Stamp"] = dataReader.GetDateTime(2).ToString();
                    newRow["DB_FileName"] = dataReader.GetString(0).ToString();
                    newRow["DB_Growth_Value"] = dataReader.GetString(1).ToString();
                    //TempDB_FileNames.Value += dataReader.GetString(0).ToString() + "|";
                    dt.Rows.Add(newRow);
                    TempDB_Info.Value += dataReader.GetDateTime(2).ToString() + "|" + dataReader.GetString(0).ToString() + "|" + dataReader.GetString(1) + "#";
                }
                queryOutputCollection.Add(dt);
            }
            if (TempDB_Info.Value != "")
            {
                TempDB_Info.Value = TempDB_Info.Value.Remove(TempDB_Info.Value.Length - 1);
                //TempDB_FileNames.Value = TempDB_FileNames.Value.Remove(TempDB_FileNames.Value.Length - 1);
            }
            
            InfoCollector.Controls.Add(TempDB_Info);
            //InfoCollector.Controls.Add(TempDB_FileNames);
            
            dataReader.Close();

            serverChartsHolder.Controls.Add(new LiteralControl("</div><br>"));

            //end of method
        }

        
        // method invoked at button click
        protected void analyseClick(object sender, EventArgs e)
        {
            queryOutputCollection.Clear();
            checkedServerInfo.Rows.Clear();
            SqlConnection connection = new SqlConnection(@"Data Source=INPDDBA027\NGEP;Database=Dev_Server;Integrated Security=SSPI");
            SqlCommand command = new SqlCommand("", connection);
            connection.Open();
            SqlDataReader dataReader;
            DataRow newRow = null;
            // just for loop and stuff
            int i=0;
            
            // clearing the asp panel with id 'Charts_Info'
            Charts_Info.Controls.Clear();

            //creating a new hidden field
            HiddenField hiddenFieldsRecords = new HiddenField();


            //setting hidden field's ID and value
            hiddenFieldsRecords.ID = "Hidden_Fields_Info";
            
            chartPanel.Controls.Clear();
            
            server_names.Value = "";
            
            hiddenFieldsRecords.Value = "";
            
            // loop to iterate through the checked fields by the user
            foreach (GridViewRow row in GridView1.Rows)
            {
                //get the checkbox of the particular column
                CheckBox cb = (CheckBox)row.FindControl("selectRow");
                //check if it is checked
                if (cb != null && cb.Checked)
                {
                    //get the dropdown list of the row
                    DropDownList chartType = (DropDownList)row.FindControl("ChartList");
                    
                    TextBox startDate = (TextBox)row.FindControl("startDate");
                    if (startDate.Text == "")
                    {
                        startDate.Text = ((DateTime.Now).AddMonths(-2)).ToString();
                    }
                    TextBox endDate = (TextBox)row.FindControl("endDate");
                    if (endDate.Text == "")
                    {
                        endDate.Text = (DateTime.Now).ToString();
                    }
                    

                    //try catch as we are handling exceptions that the file may not be there of we may not be able to read it 
                    try
                    {
                        // store it in the hidden field created to store the file names
                        server_names.Value = server_names.Value + row.Cells[2].Text + "#";

                        //create a hidden field storing the records of the files read 
                        Panel InformationCollector = new Panel();
                        
                        // setting the id of the hidden field
                        InformationCollector.ID = "Panel" + i.ToString();
                            
                        //adding the hidden field no. which will have the file information and the chart Type selected by the user to the hidden field created first
                        hiddenFieldsRecords.Value = hiddenFieldsRecords.Value + i.ToString() + "-" + chartType.SelectedValue + "|";


                        newRow = checkedServerInfo.NewRow();
                        newRow["ServerName"] = row.Cells[2].Text;
                        newRow["StartDate"] = startDate.Text;
                        newRow["EndDate"] = endDate.Text;
                        newRow["ChartType"] = chartType.SelectedValue;
                        checkedServerInfo.Rows.Add(newRow);

                        Panel serverChartsHolder = new Panel();
                        serverChartsHolder.ID = row.Cells[2].Text;
                        serverChartsHolder.Controls.Add(new LiteralControl("<h3 id=\"" + row.Cells[2].Text + "\">" + row.Cells[2].Text + "</h3>"));
                        serverChartsHolder.Controls.Add(new LiteralControl("<h5>" + startDate.Text + " To " + endDate.Text + "</h5>"));


                        command.CommandText = "SELECT CPU_Usage, Memory_Usage, Time_Stamp FROM FluorSchema.T_CPU_Memory WHERE (Server_UID='" + row.Cells[1].Text + "') AND (Time_Stamp > CONVERT(datetime, '" + startDate.Text + "'))  AND (Time_Stamp < CONVERT(datetime, '" + endDate.Text + "')) ORDER BY Time_Stamp ASC";
                        dataReader = command.ExecuteReader();
                        CPU_Chart_Info(serverChartsHolder, row.Cells[2].Text,InformationCollector,dataReader);


                        command.CommandText = "SELECT DB_File_Name ,DB_Growth_Value ,Time_Stamp  FROM FluorSchema.T_TempDB_Size WHERE (Server_UID='" + row.Cells[1].Text + "') AND (Time_Stamp > CONVERT(datetime, '" + startDate.Text + "'))  AND (Time_Stamp < CONVERT(datetime, '" + endDate.Text + "')) ORDER BY Time_Stamp ASC";
                        dataReader = command.ExecuteReader();
                        TempDB_SizeGrowth_Chart_Info(serverChartsHolder, row.Cells[2].Text, InformationCollector, dataReader);

                        
                        command.CommandText = "SELECT Session_User_ID ,Session_Count ,Time_Stamp  FROM FluorSchema.T_Multiple_Sessions WHERE (Server_UID='" + row.Cells[1].Text + "') AND (Time_Stamp > CONVERT(datetime, '" + startDate.Text + "'))  AND (Time_Stamp < CONVERT(datetime, '" + endDate.Text + "')) ORDER BY Time_Stamp ASC";
                        dataReader = command.ExecuteReader();
                        Multiple_Sessions_Chart_Info(serverChartsHolder, row.Cells[2].Text, InformationCollector, dataReader);
                        

                        command.CommandText = "SELECT Time_Stamp, EQ_Session_ID ,EQ_Host_Name ,EQ_Login_Name ,EQ_DB_Name ,EQ_Memory_Usage , EQ_Logical_Reads ,EQ_Text FROM FluorSchema.T_Expensive_Queries WHERE (Server_UID='" + row.Cells[1].Text + "') AND (Time_Stamp > CONVERT(datetime, '" + startDate.Text + "'))  AND (Time_Stamp < CONVERT(datetime, '" + endDate.Text + "')) ORDER BY Time_Stamp ASC";
                        dataReader = command.ExecuteReader();
                        Top_Queries_Table_Info(serverChartsHolder, row.Cells[2].Text, InformationCollector, dataReader);
                    
                        chartPanel.Controls.Add(serverChartsHolder);
                        
                        chartPanel.Controls.Add(new LiteralControl("<br /><br />"));
                        
                        
                        //add the hidden field in the asp Panel
                        Charts_Info.Controls.Add(InformationCollector);
                        
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:errorAlert(\"" + ex.ToString() + "\"); ", true);
                    }
                }
                i++; 
            }
            // if any file was chosen
            if (hiddenFieldsRecords.Value.Length != 0)
            {
                // remove the last character as an extra separator was added
                server_names.Value = server_names.Value.Remove(server_names.Value.Length - 1);
                hiddenFieldsRecords.Value = hiddenFieldsRecords.Value.Remove(hiddenFieldsRecords.Value.Length - 1);
                
                //add the hidden field in the asp Panel
                Charts_Info.Controls.Add(hiddenFieldsRecords);

                // call the javascript function to show the graph
                Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:showCharts(); ", true);

                Export2ExcelButton.Visible = true;
            }

        // end of method
        }

        
        
        protected void StylingExcelSheet(ExcelWorksheet ws, int rowsCount, int columnsCount)
        {
            ws.Cells[1, 2].Style.Font.Color.SetColor(Color.DodgerBlue);
            ws.Cells[1, 2].Style.Font.Bold = true;
            ws.Cells[1, 2].Style.Font.Italic = true;
            ws.Cells[1, 2].Style.Font.Size = 20;
            ws.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.DimGray);

            ws.Cells["B3:B5"].Style.Font.Color.SetColor(Color.Brown);
            ws.Cells["B3:B5"].Style.Font.Bold = true;
            ws.Cells["B3:B5"].Style.Font.Size = 15;
            ws.Cells["B3:B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["B3:B5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["C3:C5"].Style.Font.Color.SetColor(Color.DarkGreen);
            ws.Cells["C3:C5"].Style.Font.Bold = false;

            if ((rowsCount == 0) || (columnsCount == 0))
            {
                ws.Cells[7, 3].Style.Font.Color.SetColor(Color.DarkRed);
                ws.Cells[7, 3].Style.Font.Bold = true;
                ws.Cells[7, 3].Style.Font.Size = 16;
                ws.Cells[7, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[7, 3].Style.Fill.BackgroundColor.SetColor(Color.DarkSlateGray);
                ws.Cells.AutoFitColumns(0.00, 300.00);
                ws.Cells["C3:C5"].Style.WrapText = true;

            }


            // Styling for table headers
            ws.Cells[8, 2, 8, (2 + columnsCount - 1)].Style.Font.Color.SetColor(Color.Gray);
            ws.Cells[8,2,8,(2 + columnsCount - 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[8,2,8,(2 + columnsCount - 1)].Style.Fill.BackgroundColor.SetColor(Color.Black);
            ws.Cells[8,2,8,(2 + columnsCount - 1)].Style.Font.Size = 14;
            ws.Cells[8,2,8,(2 + columnsCount - 1)].Style.Font.Bold = true;
            ws.Cells[8,2,8,(2 + columnsCount - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[8,2,8,(2 + columnsCount - 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //Styling for Table Content content
            ws.Cells[9, 2, (rowsCount + 8), (2 + columnsCount - 1)].Style.Font.Color.SetColor(Color.Black);
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.Font.Size = 12;
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.Font.Bold = false;
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            ws.Cells.AutoFitColumns(0.00, 300.00);
            ws.Cells["C3:C7"].Style.WrapText = true;
            ws.Cells["B9:" + ((char)(columnsCount + 65)).ToString() + (rowsCount + 8).ToString()].Style.WrapText = true;

        }

        
        protected void createFrontSheet(ExcelWorksheet frontSheet)
        {
            frontSheet.Cells[2, 2].Value = "Server's Databases Trend Analysis Report";
            frontSheet.Cells[2, 2].Style.Font.Color.SetColor(Color.DarkGreen);
            frontSheet.Cells[2, 2].Style.Font.Bold = true;
            frontSheet.Cells[2, 2].Style.Font.Italic = true;
            frontSheet.Cells[2, 2].Style.Font.Size = 23;
            frontSheet.Cells[2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            frontSheet.Cells[2, 2].Style.Fill.BackgroundColor.SetColor(Color.Aqua);



            // get the location of the image
            string imagePath = Server.MapPath("images") + "\\logo.jpg";

            Bitmap image = new Bitmap(imagePath);
            ExcelPicture excelImage = null;
            if (image != null)
            {
                excelImage = frontSheet.Drawings.AddPicture("Fluor Daniel India", image);
                excelImage.From.Column = 1;
                excelImage.From.Row = 3;
                excelImage.SetSize(200, 100);
            }

            frontSheet.Cells[12, 2].Value = "This is a Report File of the trends analysis of the servers ";
            frontSheet.Cells[12, 2].Style.Font.Color.SetColor(Color.DarkGreen);
            frontSheet.Cells[12, 2].Style.Font.Size = 17;

            frontSheet.Cells[14, 2].Value = "Content :- ";
            frontSheet.Cells[14, 2].Style.Font.Bold = true;
            frontSheet.Cells[14, 2].Style.Font.Color.SetColor(Color.Maroon);
            frontSheet.Cells[14, 2].Style.Font.Size = 15;
            frontSheet.Cells[14, 2].Style.Font.UnderLine = true;
            frontSheet.Cells[14, 2].Style.Font.Italic = true;

            int i = 0; 

            //Loop through the GridView pages.
            foreach (DataRow tableRow in checkedServerInfo.Rows)
            {
                frontSheet.Cells["B" + (i + 16).ToString()].Formula = "=HYPERLINK(\"[.\\]'" + tableRow["ServerName"].ToString() + "'!A1\", \"" + tableRow["ServerName"].ToString() + "\")";
                i++;
            }

            frontSheet.Cells["B16:B" + (15 + checkedServerInfo.Rows.Count).ToString()].Style.Font.Color.SetColor(Color.DarkBlue);
            frontSheet.Cells["B16:B" + (15 + checkedServerInfo.Rows.Count).ToString()].Style.Font.Size = 13;
            frontSheet.Cells["B16:B" + (15 + checkedServerInfo.Rows.Count).ToString()].Style.Font.UnderLine = true;

            frontSheet.Cells["B" + (17 + checkedServerInfo.Rows.Count).ToString()].Formula = "=HYPERLINK(\"[.\\]'Summary'!A1\", \"Summary\")";
            frontSheet.Cells["B" + (17 + checkedServerInfo.Rows.Count).ToString()].Style.Font.Color.SetColor(Color.DarkBlue);
            frontSheet.Cells["B" + (17 + checkedServerInfo.Rows.Count).ToString()].Style.Font.Size = 13;
            frontSheet.Cells["B" + (17 + checkedServerInfo.Rows.Count).ToString()].Style.Font.UnderLine = true;

            frontSheet.Cells.AutoFitColumns(0.00, 300.00);
            frontSheet.Cells[12, 2].Style.WrapText = true;
        }

        protected void createSummary(ExcelWorksheet summary)
        {

            summary.Cells["C2"].Value = "Summary Sheet";
            summary.Cells[2, 3].Style.Font.Color.SetColor(Color.DarkGreen);
            summary.Cells[2, 3].Style.Font.Bold = true;
            summary.Cells[2, 3].Style.Font.Italic = true;
            summary.Cells[2, 3].Style.Font.Size = 23;
            summary.Cells[2, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            summary.Cells[2, 3].Style.Fill.BackgroundColor.SetColor(Color.Aqua);

            summary.Cells["C5"].Value = "Date of Report Creation : ";
            summary.Cells["D5"].Value = DateTime.Now.ToString("dd-MM-yyyy"); ;

            summary.Cells["C6"].Value = "Date of Report Creation : ";
            summary.Cells["D6"].Value = DateTime.Now.ToString("dd-MM-yyyy"); ;

            summary.Cells["C10"].Formula = "=HYPERLINK(\"[.\\]'Content'!A1\", \"Go to Contents Sheet\")";
            summary.Cells["C10"].Style.Font.Color.SetColor(Color.DarkOrange);
            summary.Cells["C10"].Style.Font.Bold = true;
            summary.Cells["C10"].Style.Font.Italic = true;
            summary.Cells["C10"].Style.Font.Size = 19;

            summary.Cells["C5:C6"].Style.Font.Color.SetColor(Color.DarkBlue);
            summary.Cells["C5:C6"].Style.Font.Size = 18;

            summary.Cells["D5:D6"].Style.Font.Color.SetColor(Color.Green);
            summary.Cells["D5:D6"].Style.Font.Size = 16;
            summary.Cells["D5:D6"].Style.Font.Bold = true;

            summary.Cells["C12"].Value = "Thank You";
            summary.Cells["C12"].Style.Font.Color.SetColor(Color.DarkMagenta);
            summary.Cells["C12"].Style.Font.Bold = true;
            summary.Cells["C12"].Style.Font.Italic = true;
            summary.Cells["C12"].Style.Font.Size = 30;
            summary.Cells["C12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            summary.Cells.AutoFitColumns(0.00, 300.00);

        }
        
        protected void Export2Excel(object sender, EventArgs e)
        {
            int tableMatcher = 0;
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the Front Page
                createFrontSheet(pck.Workbook.Worksheets.Add("Content"));

                //Loop through the GridView pages.
                foreach (DataRow tableRow in checkedServerInfo.Rows)
                {

                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(tableRow["ServerName"].ToString());

                    ws.Cells[1, 2].Value = "Trend Analysis Sheet";

                    ws.Cells[3, 2].Value = "Server Name - "; //insert titles to first row
                    ws.Cells[3, 3].Value = tableRow["ServerName"].ToString();

                    ws.Cells[4, 2].Value = "Initial Date of analysis - ";
                    ws.Cells[4, 3].Value = tableRow["StartDate"].ToString();

                    ws.Cells[5, 2].Value = "Final Date of Analysis - ";
                    ws.Cells[5, 3].Value = tableRow["EndDate"].ToString();

                    int table1_rows = 0, table2_rows = 0,table3_rows=0; 

                    DataTable dt = queryOutputCollection.ElementAt(tableMatcher);

                    if (dt.TableName != (tableRow["ServerName"].ToString() + "_CPUMemoryOutput"))
                    {
                        ws.Cells[7, 3].Value = "Nullset";
                        //StylingExcelSheet(ws, 0, 0);
                        continue;
                    }

                    table1_rows = dt.Rows.Count;

                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["B8"].LoadFromDataTable(dt, true);

                    tableMatcher++;

                    dt = queryOutputCollection.ElementAt(tableMatcher);
                    if (dt.TableName != (tableRow["ServerName"].ToString() + "_MultipleSessionsOutput"))
                    {
                        ws.Cells[8 + table1_rows + table2_rows , 3].Value = "Nullset";
                        //StylingExcelSheet(ws, 0, 0);
                        continue;
                    }

                    table2_rows = dt.Rows.Count;

                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells[(8 + table1_rows + 7),2].LoadFromDataTable(dt, true);

                    tableMatcher++;

                    dt = queryOutputCollection.ElementAt(tableMatcher);
                    if (dt.TableName != (tableRow["ServerName"].ToString() + "_TempDBOutput"))
                    {
                        ws.Cells[11 + table1_rows , 3].Value = "Nullset";
                        //StylingExcelSheet(ws, 0, 0);
                        continue;
                    }

                    table3_rows = dt.Rows.Count;
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells[(8 + table1_rows +table2_rows + 4), 2].LoadFromDataTable(dt, true);

                    tableMatcher++;

                    dt = queryOutputCollection.ElementAt(tableMatcher);
                    if (dt.TableName != (tableRow["ServerName"].ToString() + "_ExpensiveQueriesOutput"))
                    {
                        ws.Cells[8 + table1_rows + table2_rows + 7 + table3_rows, 3].Value = "Nullset";
                        //StylingExcelSheet(ws, 0, 0);
                        continue;
                    }

                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells[8 + table1_rows + table2_rows + table3_rows + 7 , 2].LoadFromDataTable(dt, true);

                    tableMatcher++;
                    
                    //StylingExcelSheet(ws, dt.Rows.Count, dt.Columns.Count);

                }

                //Create the Summary
                createSummary(pck.Workbook.Worksheets.Add("Summary"));

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Trend_Analysis@" + DateTime.Now.ToString() + ".xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();

            }


        }
      
    }
}