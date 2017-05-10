<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="calendar.aspx.cs" Inherits="calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="appointment_selection_div">
        <div class="row" style="margin-top: 50px; margin-left: 25px; margin-right: 25px">
            <div class="row">
                <div class="col-md-2">
                    <label class="control-label center" for="filter_month">Filter by Month</label>
                    <div class="center">
                        <asp:DropDownList runat="server" ID="month_list" CssClass="form-control">
                            <asp:ListItem Selected="True">Select Month</asp:ListItem>
                            <asp:ListItem>Select All</asp:ListItem>
                            <asp:ListItem>January</asp:ListItem>
                            <asp:ListItem>February</asp:ListItem>
                            <asp:ListItem>March</asp:ListItem>
                            <asp:ListItem>April</asp:ListItem>
                            <asp:ListItem>May</asp:ListItem>
                            <asp:ListItem>June</asp:ListItem>
                            <asp:ListItem>July</asp:ListItem>
                            <asp:ListItem>August</asp:ListItem>
                            <asp:ListItem>September</asp:ListItem>
                            <asp:ListItem>October</asp:ListItem>
                            <asp:ListItem>November</asp:ListItem>
                            <asp:ListItem>December</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2">
                    <label class="control-label center" for="filter_month">Filter by User</label>
                    <div class="center">
                        <asp:DropDownList runat="server" ID="users_list" CssClass="form-control" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="Name">
                            <asp:ListItem>Select User</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT DISTINCT [Name] FROM [Users] WHERE ([amsid] &lt;&gt; @amsid)">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="admin" Name="amsid" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
                <div class="col-md-2">
                    <label class="control-label center" for="filter_month">Filter by Type</label>
                    <div class="center">
                        <asp:DropDownList runat="server" ID="appointment_type_list" CssClass="form-control">
                            <asp:ListItem Selected="True">Select Type</asp:ListItem>
                            <asp:ListItem>Select All</asp:ListItem>
                            <asp:ListItem>Past Appointments</asp:ListItem>
                            <asp:ListItem>Future Appointments</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2">
                    <label class="control-label center" for="filter_month"></label>
                    <div class="left">

                        <asp:Button ID="filter" runat="server" Text="Apply Filter" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

