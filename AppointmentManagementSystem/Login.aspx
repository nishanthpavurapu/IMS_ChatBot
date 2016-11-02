<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Login - Appointment Management System</title>

    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet"/>

    <!-- Custom stylesheet-->
    <link href="css/Custom.css" rel="stylesheet" />

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="js/bootstrap.min.js"></script>
    <form id="form1" runat="server">
        <div class="container">
            <div class="form-horizontal">
                <div class="form-group">
                    <div id="titlediv" class="titledevcss">
                        <img src="images/Icon.png" height="100" /><br />
                        <h1 class="nowrap">Appointment Management System</h1>
                    </div>
                    <br />
                    <br />
                    <label class="col-xs-11 control-toppad">Username / Email</label>
                    <div class="col-xs-11">
                        <asp:TextBox ID="txtUsernameLogin" runat="server" Class="form-control" placeholder="Username or Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ErrorMessage="Username is Required!!" ControlToValidate="txtUsernameLogin" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>

                    <label class="col-xs-11 control-toppad">Password</label>
                    <div class="col-xs-11">
                        <asp:TextBox ID="txtPasswordLogin" runat="server" Class="form-control" type="password" placeholder="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ErrorMessage="Password cannot be empty!!" ControlToValidate="txtPasswordLogin" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-xs-11 space-vert center">
                        <asp:Button ID="buttonLogin" runat="server" Text="Login" class="btn btn-success" OnClick="buttonLogin_Click" />
                        <asp:Button ID="buttonSignup" runat="server" Text="New User?" class="btn btn-default" CausesValidation="false" OnClick="buttonSignup_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
