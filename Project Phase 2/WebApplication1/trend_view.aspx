<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trend_view.aspx.cs" Inherits="ServerInformationAnalysis.TrendAnalysis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!-- XHTML validation -->
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">

<!-- HEAD tag -->
<head runat="server">
    <!-- Meta tags -->
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>

    <!-- Web Page Title -->
    <title> Server Information Analysis </title>

    <!-- Bootstrap Core CSS  -->
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="Content/myCSS.css" rel="stylesheet" />

    <!-- Custom Fonts -->   
    <link href="font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet" type="text/css"/>
    <link href="http://fonts.googleapis.com/css?family=Lato:400,700,400italic,700italic" rel="stylesheet" type="text/css"/>

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->

    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/css/datepicker.min.css">

    <!--Load the Google Charts API-->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <!-- Custom JS for Charts  -->
    <script type="text/javascript" src="Script/myJS_1.js"></script>

</head>

<!-- Body starts -->
<body id="page-top" class="index">

    <!-- Navigation Bar -->
    <nav class="navbar navbar-default navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <!-- Company Name -->
              <a class="navbar-brand" href="#page-top">Fluor Daniel India Pvt. Ltd.</a>
            </div>
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav navbar-right">
                    <li class="hidden">
                        <a href="#page-top"></a>
                    </li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>

    <div class="container">
        <br /><br /><br />
        <a class="nav navbar-right" href="index.aspx" >
            <!-- Company Logo --> 
             <img id="logo" class="img-responsive" src="images/logo.jpg" alt="Fluor Logo"/> 
        </a>
           <br /><br /><br /><br /><br /><br /><br /><br />
        
        <!-- About the webPage -->
        <div class="jumbotron">
            <div class="container">
                

                <div class="row text-center">
                    <div class="col-xs-12 col-sm-5 col-sm-push-7">
                        <img class="img-responsive" src="images/trends2.png" />
                    </div>
                    <div class="col-xs-12 col-sm-7 col-sm-pull-5">
                        <div class="col-xs-12"> <h2> Server Information Analysis </h2><hr class="star-light" /></div><br /><br /><br /><br /><br /><br /><br />
                        <div class="col-xs-12">
                            <h4> 
                                This site with show the analytic charts of the Server Information available.
                            </h4>
                        </div>
                    </div>
                </div>

            </div>
        
        </div>
        <br /><br />
        
        <!-- Main form starts -->
        <form id="form1" runat="server">
            <br />
            <div id="main" class="jumbotron">
                <!-- Heading -->
                <div class="row">
                    <div class="col-lg-12 text-center">
                        <h2>Analysis</h2>
                        <hr class="star-light" />
                    </div>
                </div>
                <br /><br />
                <div class="container">
                <div class="row">
                    <div class="col-lg-12 text-center">
                        <!-- GridView to show the file in an order with a dropdown list individually for it-->
                        <asp:GridView ID="GridView1" runat="server" align="center" CssClass="fileTable" ShowFooter="false" ShowHeader="false" AutoGenerateColumns="false" AutoGenerateDeleteButton="false" AutoGenerateEditButton="false" AutoGenerateSelectButton="false" EmptyDataText="No Servers To analyse !!!">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectRow" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FS_UID" HeaderText="Unique ID"  ItemStyle-Width="150" />
                                <asp:BoundField DataField="Server_Name" HeaderText="Server Name" ItemStyle-Width="350" />
                                <asp:TemplateField HeaderText="Chart Type" ItemStyle-Width="250">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ChartList" runat="server" CssClass="form-control" disabled="true" >
                                            <asp:ListItem Text="BarChart" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="LineChart" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="XY-Scatter Chart" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:TextBox ID="startDate" runat="server" ToolTip="Enter the start date from where you want to analyse" placeholder="Start Date" CssClass="form-control startDatePicker" disabled="true" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:TextBox ID="endDate" runat="server" ToolTip="Enter the end date to where you want to analyse" placeholder="End Date" CssClass="form-control endDatePicker" disabled="true" ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                    <br /><br /><br /><br /><br />
                    <!-- Bootstrap class -->
                    <div class="row">
                        <!-- Bootstrap class -->
                        <div class="col-lg-12 text-center">
                            <!-- Asp.net button for starting the process of reading the xml files and analysing it on clicking it,by calling 'analyseClick()' method defined in it CodeBehind file. -->
                            <asp:Button ID="Button1" runat="server" Text="Analyse" OnClick="analyseClick" CssClass="btn btn-info" />
                        </div>
                        <!-- Bootstrap class -->
                        <div class="col-lg-12 text-center">
                            <!-- Asp.net button for starting the process of reading the xml files and analysing it on clicking it,by calling 'analyseClick()' method defined in it CodeBehind file. -->
                            <asp:Button ID="Export2ExcelButton" runat="server" Text="Export To Excel" OnClick="Export2Excel" CssClass="btn btn-outline exportBtnCss" Visible=false/>
                        </div>
                    </div>
                    <br /><br />
                    

                    <div class="row">
                        <!-- Bootstrap class -->
                        <ul id="links" class="col-lg-12">
                        </ul>
                    </div>
                    <br /><br />
                    
                    <!-- For showing charts on the mainWindow. These will be static. -->
                    <asp:Panel ID="chartPanel" CssClass="text-center" runat="server">
                    </asp:Panel>

                    <!-- This Asp panel will become a div when converted to HTML and will hold Hidden Fields which will be having the information fetched from the XML files -->
                    <asp:Panel ID="Charts_Info" runat="server"></asp:Panel>
                <!-- End OF Container -->
                </div>
                <br /><br />
                <!-- End of Main Jumbotron -->
            </div>
        
            <!-- JavaScript file having the function which will parse the information recieved and start the process -->
            <script type="text/javascript" src="Script/myJS_2.js"></script>
     
        <!-- Hidden Field having the names of all the files selected by the user -->
        <asp:HiddenField ID="server_names" runat="server" />

        <!-- End of form -->
        </form>
        <div class="row text-center">
            
            <div class="col-xs-12">
                <a href="index.aspx">
                    <button type="button" class="btn btn-default">
                      <span class="glyphicon glyphicon-home"></span> Home
                    </button>
                </a>
            </div>
            
        </div>
    <!-- End of container div -->
    </div>
    
    <!--#include virtual="footer.html"-->
    
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.3.0/js/bootstrap-datepicker.min.js"></script>

<!-- End of body -->
</body>

<!-- End of page -->
</html>


