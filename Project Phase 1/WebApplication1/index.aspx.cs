using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Xml.Linq;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            //checking if the file is loaded for the first time or not
            if(!IsPostBack)
                //if loaded for the first time, then run the method
                SetInitialRow();
        }
        private void SetInitialRow()
        {
            // get all the file names in the folder "App_Data" in the server directory
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/App_Data/"));
            
            // make a DataTable
            DataTable dt = new DataTable();
            //make a DataTable Row. Initially it is null as we don't have any file name to put yet
            DataRow dr = null;

            // Add the columns in the DataTable
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("FullPath", typeof(string)));
            
            //loop through the file names to insert them in the DataTable
            foreach (string filePath in filePaths)
            {
                //Check if the file extension is XML or not
                if (Path.GetExtension(filePath) == ".xml")
                {
                    //if .xml , then add a row with its information
                    //Creating a new row
                    dr = dt.NewRow();

                    //entering file name
                    dr["Name"] = Path.GetFileName(filePath);

                    //entering file Path in the server directory
                    dr["FullPath"] = filePath;

                    // adding the row in the DataTable
                    dt.Rows.Add(dr);
                }
            }
            
            //Making Our DataTable to be the current Visible table
            ViewState["CurrentTable"] = dt;

            //making the gridView source to be our dataTable
            GridView1.DataSource = dt;


            //binding the data as per columns of the datatable and the gridview
            GridView1.DataBind();

            //end of method
        }

        // method invoked at button click
        protected void analyseClick(object sender, EventArgs e)
        {
            // just for loop and stuff
            int i=0;
            
            // clearing the asp panel with id 'Charts_Info'
            Charts_Info.Controls.Clear();

            //creating a new hidden field
            HiddenField hidden = new HiddenField();
            //setting hidden field's ID and value
            hidden.ID = "Hidden_Fields_Info";
            hidden.Value = "";

            // loop to iterate through the checked fields by the user
            foreach (GridViewRow row in GridView1.Rows)
            {
                //get the checkbox of the particular column
                CheckBox cb = (CheckBox)row.FindControl("chkRow");
                //check if it is checked
                if (cb != null && cb.Checked)
                {
                    //get the dropdown list of the row
                    DropDownList chartType = (DropDownList)row.FindControl("ChartList");
                    
                    //try cathc as we are handling exceptions that the file may not be there of we may not be able to read it 
                    try
                    {
                        // get the location of the file
                        string SaveLocation = Server.MapPath("App_Data") + "\\" + row.Cells[1].Text;

                        // check if it is there 
                        if (System.IO.File.Exists(@SaveLocation))
                        {
                            // store it in the hidden field created to store the file names
                            file_names.Value = file_names.Value + row.Cells[1].Text + "#";

                            // load the XML Document
                            //Here Linq To XML is used to read the file
                            XDocument document = XDocument.Load(@SaveLocation);

                            // Get the records of the XML file
                            var books = from r in document.Descendants("value")
                                        select new
                                        {
                                            name = r.Element("name").Value,
                                            int1 = r.Element("int1").Value,
                                            float1 = r.Element("float1").Value
                                        };
                            
                            //create a hidden field storing the records of the files read 
                            HiddenField h = new HiddenField();
                            // setting the id of the hidden field
                            h.ID = "hidd" + i.ToString();
                            
                            //adding the hidden field no. which will have the file information and the chart Type selected by the user to the hidden field created first
                            hidden.Value = hidden.Value + i.ToString() + "-" + chartType.SelectedValue + "|";

                            i++;
                            // get the count of records
                            int lenn = books.Count();
                            // initialising the strings storing the information
                            string s1="", s2="", s3="";
                            // loop variable
                            int k;

                            // loop to add the information in the string
                            for (k = 0; k < lenn - 1; k++)
                            {
                                s1 += books.ElementAt(k).name + "|";
                                s2 += books.ElementAt(k).int1 + "|";
                                s3 += books.ElementAt(k).float1 + "|";

                            }

                            //add the last value
                            s1 += books.ElementAt(k).name;
                            s2 += books.ElementAt(k).int1;
                            s3 += books.ElementAt(k).float1;

                            // add all these values in the hidden field
                            h.Value = s1 + "#" + s2 + "#" + s3; 
                            
                            //add the hidden field in the asp Panel
                            Charts_Info.Controls.Add(h);

                            
                        }
                        else
                        {
                         //   datas.InnerHtml = "File Not Available";
                        }

                    }
                    catch (Exception ex)
                    {

                        // any exception
                    }
                }
                   
            }
            // if any file was chosen
            if (hidden.Value.Length != 0)
            {
                // remove the last character as an extra separator was added
                file_names.Value = file_names.Value.Remove(file_names.Value.Length - 1);
                hidden.Value = hidden.Value.Remove(hidden.Value.Length - 1);
                
                //add the hidden field in the asp Panel
                Charts_Info.Controls.Add(hidden);

                // call the javascript function to show the graph
                Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:showCharts(); ", true);
                
                // set the label's text to open the modal window
                LabelModal.Text = "<script language=\"JavaScript\">openModal()</script>";
            }

        // end of method
        }
      
    }
}