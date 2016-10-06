using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;

namespace ServerInformationAnalysis
{

    
    public partial class demo : System.Web.UI.Page
    {
        static List<DataTable> queryOutputCollection = new List<DataTable>(); 
        static DataTable queryInformationCollector = new DataTable();
        static List<string> sheet_Names = new List<string>();

        protected void StylingExcelSheet(ExcelWorksheet ws, int rowsCount, int columnsCount)
        {
            ws.Cells[1, 2].Style.Font.Color.SetColor(Color.DodgerBlue);
            ws.Cells[1, 2].Style.Font.Bold = true;
            ws.Cells[1, 2].Style.Font.Italic = true;
            ws.Cells[1, 2].Style.Font.Size = 20;
            ws.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.DimGray);

            ws.Cells["B3:B7"].Style.Font.Color.SetColor(Color.Brown);
            ws.Cells["B3:B7"].Style.Font.Bold = true;
            ws.Cells["B3:B7"].Style.Font.Size = 15;
            ws.Cells["B3:B7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["B3:B7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["C3:C7"].Style.Font.Color.SetColor(Color.DarkGreen);
            ws.Cells["C3:C7"].Style.Font.Bold = false;
            
            if ((rowsCount == 0) || (columnsCount == 0))
            {
                ws.Cells[9, 3].Style.Font.Color.SetColor(Color.DarkRed);
                ws.Cells[9, 3].Style.Font.Bold = true;
                ws.Cells[9, 3].Style.Font.Size = 16;
                ws.Cells[9, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[9, 3].Style.Fill.BackgroundColor.SetColor(Color.DarkSlateGray);
                ws.Cells.AutoFitColumns(0.00, 300.00);
                ws.Cells["C3:C7"].Style.WrapText = true;
                return;
            }


            // Styling for table headers
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.Font.Color.SetColor(Color.LightGray);
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.Fill.BackgroundColor.SetColor(Color.Black);
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.Font.Size = 14;
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.Font.Bold = true;
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[10, 2, 10, (2 + columnsCount - 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //Styling for Table Content content
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.Font.Color.SetColor(Color.Black);
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.Font.Size = 12;
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.Font.Bold = false;
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
           
            ws.Cells.AutoFitColumns(0.00, 300.00);
            ws.Cells["C3:C7"].Style.WrapText = true;
            ws.Cells[11, 2, (rowsCount + 11 -1), (2 + columnsCount - 1)].Style.WrapText = true;

        }


        protected void createFrontSheet(ExcelWorksheet frontSheet)
        {
            frontSheet.Cells[2,2].Value = "Live View And Analysis Report";
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
            
            frontSheet.Cells[12, 2].Value = "This is a Report File of the live view and analysis of the server - ";
            frontSheet.Cells[12, 2].Style.Font.Color.SetColor(Color.DarkGreen);
            frontSheet.Cells[12, 2].Style.Font.Size = 17;

            frontSheet.Cells[12, 3].Value = RadioButtonsServerList.SelectedItem.Value.ToString();
            frontSheet.Cells[12, 3].Style.Font.Color.SetColor(Color.MediumSlateBlue);
            frontSheet.Cells[12, 3].Style.Font.Size = 17;
            frontSheet.Cells[12, 3].Style.Font.Bold = true;

            frontSheet.Cells[14, 2].Value = "Content :- ";
            frontSheet.Cells[14, 2].Style.Font.Bold = true;
            frontSheet.Cells[14, 2].Style.Font.Color.SetColor(Color.Maroon);
            frontSheet.Cells[14, 2].Style.Font.Size = 15;
            frontSheet.Cells[14, 2].Style.Font.UnderLine = true;
            frontSheet.Cells[14, 2].Style.Font.Italic = true;

            for(int i=0;i<sheet_Names.Count();i++)
            {
                frontSheet.Cells["B" + (i + 16).ToString()].Formula = "=HYPERLINK(\"[.\\]'" + sheet_Names[i] + "'!A1\", \"" + sheet_Names[i] + "\")";
            }

            frontSheet.Cells[16, 2, (16 + sheet_Names.Count() - 1), 2].Style.Font.Color.SetColor(Color.DarkBlue);
            frontSheet.Cells[16, 2, (16 + sheet_Names.Count() - 1), 2].Style.Font.Size = 13;
            frontSheet.Cells[16, 2, (16 + sheet_Names.Count() - 1), 2].Style.Font.UnderLine = true;

            frontSheet.Cells[16 + sheet_Names.Count() + 1, 2].Formula = "=HYPERLINK(\"[.\\]'Summary'!A1\", \"Summary\")";
            frontSheet.Cells[16 + sheet_Names.Count() + 1, 2].Style.Font.Color.SetColor(Color.DarkBlue);
            frontSheet.Cells[16 + sheet_Names.Count() + 1, 2].Style.Font.Size = 13;
            frontSheet.Cells[16 + sheet_Names.Count() + 1, 2].Style.Font.UnderLine = true;

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
            
            summary.Cells["C8"].Value = "XML File Name : ";
            summary.Cells["D8"].Value = "Query Info.xml";

            summary.Cells["C6"].Value = "Time of Report Creation : ";
            summary.Cells["D6"].Value = DateTime.Now.ToString("HH:mm:ss"); ;

            summary.Cells["C7"].Value = "Path to the file on server : ";
            summary.Cells["D7"].Value = "~\\App_Data\\"; ;

            summary.Cells["C10"].Formula = "=HYPERLINK(\"[.\\]'Content'!A1\", \"Go to Contents Sheet\")";
            summary.Cells["C10"].Style.Font.Color.SetColor(Color.DarkOrange);
            summary.Cells["C10"].Style.Font.Bold = true;
            summary.Cells["C10"].Style.Font.Italic = true;
            summary.Cells["C10"].Style.Font.Size = 19;

            summary.Cells["C5:C8"].Style.Font.Color.SetColor(Color.DarkBlue);
            summary.Cells["C5:C8"].Style.Font.Size = 18;

            summary.Cells["D5:D8"].Style.Font.Color.SetColor(Color.Green);
            summary.Cells["D5:D8"].Style.Font.Size = 16;
            summary.Cells["D5:D8"].Style.Font.Bold = true;

            summary.Cells["C12"].Value = "Thank You";
            summary.Cells["C12"].Style.Font.Color.SetColor(Color.DarkMagenta);
            summary.Cells["C12"].Style.Font.Bold = true;
            summary.Cells["C12"].Style.Font.Italic = true;
            summary.Cells["C12"].Style.Font.Size = 30;
            summary.Cells["C12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            summary.Cells.AutoFitColumns(0.00, 300.00);
        
        }

        protected void ExportExcel(object sender, EventArgs e)
        {
            int counter = 0;
            int tableMatcher = 0;
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the Front Page
                createFrontSheet(pck.Workbook.Worksheets.Add("Content"));

                //Loop through the GridView pages.
                foreach (DataRow tableRow in queryInformationCollector.Rows)
                {
                    
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(tableRow["QueryName"].ToString());

                    ws.Cells[1, 2].Value = "Live Demo Sheet";

                    ws.Cells[3, 2].Value = "Query Name - "; //insert titles to first row
                    ws.Cells[3, 3].Value = tableRow["QueryName"].ToString();
                    
                    ws.Cells[4, 2].Value = "Query - ";
                    ws.Cells[4, 3].Value = tableRow["Query"].ToString();
                    
                    ws.Cells[5, 2].Value = "DB Table - ";
                    ws.Cells[5, 3].Value = tableRow["DBTable"].ToString();
                    
                    ws.Cells[6, 2].Value = "Column Information - ";
                    ws.Cells[6, 3].Value = tableRow["ColumnInformation"].ToString();
                    
                    ws.Cells[7, 2].Value = "Active Status - ";
                    ws.Cells[7, 3].Value = tableRow["Active"].ToString();

                    DataTable dt = queryOutputCollection.ElementAt(tableMatcher);

                    if (dt.TableName != counter.ToString())
                    {
                        ws.Cells[9,3].Value = "Nullset";
                        counter++;
                        StylingExcelSheet(ws, 0, 0);
                        continue;
                    }
                    
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["B10"].LoadFromDataTable(dt, true);

                    tableMatcher++;                        
                    counter++;

                    StylingExcelSheet(ws, dt.Rows.Count, dt.Columns.Count);

                }

                //Create the Summary
                createSummary(pck.Workbook.Worksheets.Add("Summary"));

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + RadioButtonsServerList.SelectedItem.Value.ToString() + "_Live_Analysis.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
                
            }


        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                fillGridView();
        }


        private void addColumnsToDatatable()
        {
            queryInformationCollector.Columns.Add("QueryName", typeof(String));
            queryInformationCollector.Columns.Add("Query", typeof(String));
            queryInformationCollector.Columns.Add("DBTable", typeof(String));
            queryInformationCollector.Columns.Add("ColumnInformation", typeof(String));
            queryInformationCollector.Columns.Add("Active", typeof(String));

        }


        private void fillGridView()
        {
            DataSet ds = new DataSet();
            SqlConnection connection = new SqlConnection(@"Data Source=INPDDBA027\NGEP;Database=Dev_Server;Integrated Security=SSPI");
            SqlCommand command = new SqlCommand("Select Server_Name FROM FluorSchema.T_Server_Info",connection);
            connection.Open();

            SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
                RadioButtonsServerList.Items.Add(dataReader.GetString(0));

            dataReader.Close();
            connection.Close();
            queryInformationCollector.Clear();
            queryInformationCollector.Columns.Clear();
            //end of method


            addColumnsToDatatable();

        }
        protected void fillView(object sender, EventArgs e)
        {
            try{
               
                // get the location of the file
                string SaveLocation = Server.MapPath("App_Data") + "\\Query Info.xml";

                // check if it is there 
                if (System.IO.File.Exists(@SaveLocation))
                {
                    // load the XML Document
                    //Here Linq To XML is used to read the file
                    XDocument document = XDocument.Load(@SaveLocation);

                    // Get the records of the XML file
                    var queryInfo = from row in document.Descendants("QueryInfo")
                                select new
                                {
                                    queryName = row.Element("QueryName").Value,
                                    query = row.Element("Query").Value,
                                    dbTable = row.Element("DBTable").Value,
                                    columnInfo = row.Element("ColumnInformation").Value,
                                    isActive = row.Element("IsActive").Value
                                };
                    
                    SqlConnection connection = new SqlConnection(@"Data Source=" + RadioButtonsServerList.SelectedItem.Value.ToString() + ";Database=Master;Integrated Security=SSPI");
                    connection.Open();
                    int recordsCounter = 0;
                    int totalGridViews = 0;

                    DataRow queryInformationCollectorRow = null;

                    foreach (var queryInfoRow in queryInfo)
                    {

                        queryInformationCollectorRow = queryInformationCollector.NewRow();
                        queryInformationCollectorRow["QueryName"] = queryInfoRow.queryName;
                        queryInformationCollectorRow["Query"] = queryInfoRow.query;
                        queryInformationCollectorRow["DBTable"] = queryInfoRow.dbTable;
                        queryInformationCollectorRow["ColumnInformation"] = queryInfoRow.columnInfo;
                        queryInformationCollectorRow["Active"] = queryInfoRow.isActive;

                        queryInformationCollector.Rows.Add(queryInformationCollectorRow);

                        InfoCollector.Controls.Add(new LiteralControl("<h3>Query Information "+ (recordsCounter+1).ToString() +"</h3>"));

                        sheet_Names.Add(queryInfoRow.queryName);
                        string DefaultInfo ="<div class=\"row\"><div class=\"col-xs-12\">"+
                                                "<span class=\"queryInfoHeader\">Query Name </span> : <span class=\"queryInfoContent\">" + queryInfoRow.queryName + "</span><br />" +
                                                "<span class=\"queryInfoHeader\">DB Table Name  </span> : <span class=\"queryInfoContent\">" + queryInfoRow.dbTable + "</span><br />" +
                                                "<span class=\"queryInfoHeader\">Column Information  </span> : <span class=\"queryInfoContent\">" + queryInfoRow.columnInfo + "</span><br />" +
                                                "<span class=\"queryInfoHeader\">Active  </span> : <span class=\"queryInfoContent\">" + queryInfoRow.isActive + "</span><br />" +
                                            "</div></div><br>"+
                                            "<div class=\"row\" ><div class=\"col-xs-12\">"+
                                                "<h4> Output : </h4><br />";

                        InfoCollector.Controls.Add(new LiteralControl(DefaultInfo));
                        
                        SqlCommand command = new SqlCommand(queryInfoRow.query.Trim(),connection);
                        SqlDataReader dataReader = command.ExecuteReader();
                        
                        if (dataReader.HasRows)
                        {
                            DataTable dt = new DataTable(recordsCounter.ToString());
                            dt.Load(dataReader);
                            queryOutputCollection.Add(dt);
                            InfoCollector.Controls.Add(new LiteralControl("</div></div><div class=\"row\" ><div class=\"col-xs-12 text-center gridViewDiv\">"));
                        
                            GridView g = new GridView();
                            g.ID = "Grid_" + totalGridViews.ToString();
                            g.CssClass = "table table-striped table-hover";
                            g.EmptyDataText = "There are no data records to display";
                            g.DataSource = dt;
                            g.DataBind();
                            g.HeaderRow.Style["background-color"] = "#373a3c";
                            g.HeaderRow.Style["color"] = "#fff";
                            InfoCollector.Controls.Add(g);
                            InfoCollector.Controls.Add(new LiteralControl("<br />"));
                            totalGridViews++;
                        }
                        else
                        {
                            InfoCollector.Controls.Add(new LiteralControl("<h3> Null-Set </h3><br />"));                        
                        }
                        recordsCounter++;
                        InfoCollector.Controls.Add(new LiteralControl("</div></div><br />"));
                        dataReader.Close();
                    }
                    connection.Close();
                }
            }
            catch(Exception ex){
                ExportButton.Visible = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "error", "javascript:errorAlert(\""+ex.Message+"\");", true);
            }
            if (queryInformationCollector.Rows.Count != 0)
                ExportButton.Visible = true;
            else
                ExportButton.Visible = false;
        }

    }
    // <asp:Image runat="server" ImageUrl ="<%# //Eval("IsActive_image"))%>" />
}