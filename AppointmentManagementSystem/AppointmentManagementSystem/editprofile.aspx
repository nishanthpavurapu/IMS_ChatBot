<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="editprofile.aspx.cs" Inherits="editprofile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="container">
        <h1 class="text-nowrap center">Edit/View Profile</h1>
        <div class="container-div">
        <div class="row">
        <label class="col-lg-12 control-toppad">Username</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtusername" runat="server" Class="form-control" placeholder="Username"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ErrorMessage="Username cannot be empty!!" ControlToValidate="txtusername" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad">Full name</label>
        <div class="col-lg-8 ">
             <asp:TextBox ID="txtfullname" runat="server" Class="form-control" placeholder="Last Name           First Name"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name cannot be empty!!" ControlToValidate="txtfullname" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            
            </div>
        <div class="row">
         <label class="col-lg-12 control-toppad">Email</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtemail" runat="server" Class="form-control" placeholder="Email Address"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email cannot be empty!!" ControlToValidate="txtemail" CssClass="text-danger"></asp:RequiredFieldValidator>
        </div>
            
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad">New Password</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtpassword" runat="server" type="password" Class="form-control" placeholder="Password"></asp:TextBox>
        </div>
            
            </div>
        <div class="row">
         <label class="col-lg-12 control-toppad">Confirm New Password</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtconfirmpassword" type="password" runat="server" Class="form-control" placeholder="Confirm Password"></asp:TextBox>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords donot match" ControlToValidate="txtconfirmpassword" CssClass="text-danger" ControlToCompare="txtpassword"></asp:CompareValidator>
        </div>
            
            </div>
        <div class="row">
        <label class="col-lg-12 control-toppad">Phone Number</label>
        <div class="col-lg-8">
             <asp:TextBox ID="txtphonenumber" runat="server" Class="form-control" placeholder="Phone number"></asp:TextBox>
        </div>
            
            </div>            
        <div class="row">
        <label class="col-lg-12 control-toppad">Available Days</label>
            <asp:SqlDataSource ID="SqlDataSourceProfile" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [profile]"></asp:SqlDataSource>
        <div class="col-lg-12">
            <asp:CheckBox id="mondayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Mon"/>
            <asp:CheckBox id="tuesdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Tue"/>
            <asp:CheckBox id="wednesdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Wed"/>
            <asp:CheckBox id="thursdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Thu"/>
            <asp:CheckBox id="fridayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Fri"/>
            <asp:CheckBox id="saturdayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Sat"/>
            <asp:CheckBox id="sundayCheckbox" CssClass="checkbox-inline" runat="server" Checked="true" text="Sun"/>
        </div>
        </div>
        <div class="row">
        <div class="col-lg-12 space-vert center" style="padding-right:255px;">
            <asp:SqlDataSource ID="SqlDataSourceUsers" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [Users]"></asp:SqlDataSource>
            <asp:Button ID="buttonUpdateProfile" runat="server" Text="Update Profile" class="btn btn-success" OnClick="buttonUpdateProfile_Click"/>
            <asp:Button ID="buttonCancel" runat="server" Text="Cancel" class="btn btn-default" OnClick="buttonCancel_Click"/>
        </div>
            </div>
            </div>
    </div>
    
</asp:Content>

