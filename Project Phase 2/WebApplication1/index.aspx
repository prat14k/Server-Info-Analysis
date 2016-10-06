<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ServerInformationAnalysis.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!-- XHTML validation -->
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">

<!-- HEAD tag -->
<head id="Head1" runat="server">
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

</head>

<!-- Body starts -->
<body id="page-top" class="index">

    <!-- Navigation Bar -->
    <nav class="navbar navbar-default navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <!-- Company Name -->
              <a class="navbar-brand text-center" href="#page-top">Fluor Daniel India Pvt. Ltd.</a>
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

    <!-- Header -->
    <header>
    <div class="jumbotron">
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <img class="img-responsive" src="images/img.jpg" alt="" />
                    <div class="intro-text">
                        <span class="name">Server Information Analysis</span>
                        <hr class="star-light" />
                        <span class="skills">Gather , See ,  Analyse your server !!! </span>
                    </div>
                </div>
            </div>
        </div>
        </div>
    </header>




    <div class="container">
        
        <br /><br /><br />
        
        <!-- About the webPage -->
        <div class="jumbotron">
            <div class="container">
                
                <div class="row">
                <h2> What All can you do ?? </h2>
                <br />
                <h4> 
                    By clicking on the "Trend" button , you can see the the various analysis of the server using the past information. 
                     <br /><br />
                    And , By Clicking on Live Demo , you can see the status of the server as to that point of time !!!!
                </h4>
                    
                    </div>
                    <br />
                    <div class="row text-center">
                    
                        <div class="col-lg-6">
                            <a href="trend_view.aspx">
                                <input id="trend" type="button" value="Trend" class="btn btn-success" />
                            </a>
                        </div>
                    
                        <div class="col-lg-6">
                            <a href="demo.aspx">
                                <input id="demo" type="button" value="Live Demo" class="btn btn-info"/>
                            </a>
                        </div>
                    
                    </div>


            </div>
        
        </div>
    </div>
         <!--#include virtual="footer.html"-->
    
        <!-- jQuery -->
        <script type="text/javascript" src="Script/jquery.js"></script>

     </body>
</html>
