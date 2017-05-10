<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Signup.aspx.cs" Inherits="Signup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Sign Up</title>

    <!-- Bootstrap -->
    <link href="css/bootstrap.min.css" rel="stylesheet"/>

    <!-- Custom stylesheet-->
    <link href="css/Custom.css" rel="stylesheet" />
    <link href="css/bootstrap-slider.min.css" rel="stylesheet"/>

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
    <script src="Scripts/bootstrap-slider.min.js"></script>



    <form id="form1" runat="server">
        
    <div class="container">
        <h1 class="text-nowrap center">Account Registration</h1>
        <div class="container-div">
        <div class="row">
        <label class="col-lg-12 control-toppad">Username</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtusername" runat="server" Class="form-control" placeholder="Username"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ErrorMessage="Username cannot be empty!!" ControlToValidate="txtusername" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad" style="margin-top:-5px">Full name</label>
        <div class="col-lg-8 ">
             <asp:TextBox ID="txtfullname" runat="server" Class="form-control" placeholder="Last Name           First Name"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name cannot be empty!!" ControlToValidate="txtfullname" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            
            </div>
        <div class="row">
         <label class="col-lg-12 control-toppad" style="margin-top:-5px">Email</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtemail" runat="server" Class="form-control" placeholder="Email Address"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email cannot be empty!!" ControlToValidate="txtemail" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad" style="margin-top:-5px">Password</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtpassword" runat="server" type="password" Class="form-control" placeholder="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Password cannot be empty!!" ControlToValidate="txtpassword" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            
            </div>
        <div class="row">
         <label class="col-lg-12 control-toppad" style="margin-top:-5px">Confirm Password</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtconfirmpassword" type="password" runat="server" Class="form-control" placeholder="Confirm Password"></asp:TextBox>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords donot match" ControlToValidate="txtconfirmpassword" CssClass="text-danger" ControlToCompare="txtpassword"></asp:CompareValidator>
        </div>
            
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad" style="margin-top:-5px">Phone Number</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtphonenumber" runat="server" Class="form-control" placeholder="Phone number"></asp:TextBox>
        </div>
            
            </div>
        <div class="row">
         <label class="col-lg-12 control-toppad" style="margin-top:15px">Working Hours (00 : 24 hrs)</label>
      
        <div class="col-lg-8">
            <div id="sliderDiv">
                <asp:TextBox ID="workinghours" runat="server"></asp:TextBox>
              <!--  <span><input id="workinghours" type="text"/></span> -->
                <script type="text/javascript">
                    $("#workinghours").slider({ min: 00, max: 24, value: [09, 18], focus: true });
                </script>
                
            </div>
        </div>
            
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad" style="margin-top:15px">Available Days</label>
        <div class="col-lg-12">
            <asp:CheckBox id="mondayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Mon"/>
            <asp:CheckBox id="tuesdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Tue"/>
            <asp:CheckBox id="wednesdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Wed"/>
            <asp:CheckBox id="thursdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Thu"/>
            <asp:CheckBox id="fridayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Fri"/>
            <asp:CheckBox id="saturdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="false" text="Sat"/>
            <asp:CheckBox id="sundayCheckbox" CssClass="checkbox-inline" runat="server" Checked="false" text="Sun"/>
        </div>
        </div>
        <div class="row">
        <div class="col-lg-12 space-vert center" style="padding-right:255px;">
            <asp:Button ID="buttonSignup" runat="server" Text="Sign Up" class="btn btn-md btn-primary" OnClick="buttonSignup_Click"/>
        </div>
            </div>
            </div>
    </div>
    </form>
    
</body>
</html>
