<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HSRP.Login" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    
    <link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />
    <title>HSRP Application Ver1.0</title>
        <script type="text/javascript" src="windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <style type="text/css">
        body {
            background-color: #fff;
            color: #5a5656;
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 16px;
            line-height: 1.5em;
            background-image:url('../images/bgimage.png'); 
    
            background-repeat:no-repeat;
            background-size:cover;
            background-position:center;
            position:relative;
        }

       
        body,*,*:after,*:before{
            margin:0;
            padding:0; 
        }

        body:after{
            position:absolute;
            content:'';
            display:block;
            top:0;
            left:0;
            width:100%;
            height:100%;
            background:rgba(0,0,0,0.7);
          
        }

        

        a {
            text-decoration: none;
        }

        h1 {
            font-size: 1em;
        }

        h1, p {
            margin-bottom: 10px;
        }

        strong {
            font-weight: bold;
        }

        .uppercase {
            text-transform: uppercase;
        }

        /* ---------- LOGIN ---------- */
       
        .content{
            display:grid;
            place-content:center;
            width:100vw;
            height:100vh;
        
        }

        .content #login{
                z-index:1;
                padding:1.5rem 2em;
                border-radius:12px;
                background:#fff;
        }


        form  input[type="text"], input[type="password"] {
            background-color: #e5e5e5;
            border: none;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            color: #5a5656;
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            font-size: 14px;
            height: 50px;
            outline: none;
            padding: 0px 8px;
            width: 280px;
            -webkit-appearance: none;
        }

       input[type="submit"] {
            background-color: #008dde;
            border: none;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            color: #f4f4f4;
            cursor: pointer;
            font-family: 'Open Sans', Arial, Helvetica, sans-serif;
            height: 50px;
            text-transform: uppercase;
            width: 300px;
            -webkit-appearance: none;
       }

        form  a {
            color: #5a5656;
            font-size: 10px;
        }

        form  a:hover {
            text-decoration: underline;
        }

        .btn-round {
            background-color: #5a5656;
            border-radius: 50%;
            -moz-border-radius: 50%;
            -webkit-border-radius: 50%;
            color: #f4f4f4;
            display: block;
            font-size: 12px;
            height: 50px;
            line-height: 50px;
            margin: 30px 125px;
            text-align: center;
            text-transform: uppercase;
            width: 50px;
        }

        form[name=form1]{
            z-index:20000;
        }

    </style>

    
    <script type="text/javascript">
        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes
            googlewin = dhtmlwindow.open("googlebox", "iframe", "ForgotPassword.aspx", "Change Password", "width=700px,height=400px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'Login.aspx';
                return true;
            }
        }
        function validateForm() {

            var username = document.getElementById("txtUsername").value;
            var pass = document.getElementById("txtpassword").value;
            if (username == '') {
                swal("Please Provide User Name!", "", "error");
                document.getElementById("txtUsername").focus();
                return false;
            }
            if (pass == '') {
                swal('Please Provide Password.', '', 'error');
                document.getElementById("txtpassword").focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <div class="content">

  
    <div id="login">
       
        <form method="post" runat="server">
         
         
                 <img src="images/utsav_logo.png" style="height: 50px;" />
            
                <p>
                    <asp:TextBox ID="txtUsername" CssClass="fieldset" placeholder="Username" AutoCompleteType="none" runat="server" TabIndex="1" MaxLength="20" ></asp:TextBox>
                </p>
                <p>
                    <asp:TextBox ID="txtpassword" runat="server" CssClass="fieldset" TextMode="Password" AutoCompleteType="none" placeholder="**********" TabIndex="2" MaxLength="20" ></asp:TextBox>
               <%-- <p><a onclick="AddNewPop();" href="#" style="color: blue;">Forgot Password?</a></p>--%>
                <p>
                    <asp:Button ID="btnSubmit" Text="Login" runat="server" CssClass="fieldset" OnClientClick="javascript: return validateForm();" OnClick="btnSubmit_Click" />
                </p>
           <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text=""></asp:Label>
        </form>        
    </div>

          </div>
    <!-- end login -->
</body>
</html>
