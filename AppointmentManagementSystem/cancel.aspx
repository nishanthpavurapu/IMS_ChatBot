<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cancel.aspx.cs" Inherits="cancel" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div style="margin-top: 25px">
        <fieldset>
            <!-- Form Name -->
            <legend class="center">Cancel Appointment</legend>
            <!-- Search input-->
            <div class="form-group">
                <asp:Label runat="server" id="doa_label" class="col-md-4 control-label" for="userid">Date of Appointment</asp:Label>
                <div class="col-md-2">
                    <asp:textbox runat="server" id="dateofappointmenttxt" cssclass="form-control input-md" required="" />
                    <script type="text/javascript">
                    $(function () {
                        $('#ContentPlaceHolder1_dateofappointmenttxt').datetimepicker(
                            {
                                format: 'MM/DD/YYYY'
                            }
                            );
                    });
                </script>
                </div>
            </div>

            <div class="form-group">
                <label class="col-md-4 control-label" for="getappointments"></label>
                <div class="col-md-2">
                    <asp:Button ID="getappointmentbutton" runat="server" Text="Get Appointments!" CssClass="btn btn-primary" OnClick="getappointment_Click"/>
                </div>
            </div>

            <div style="text-align:center">
                <asp:Label runat="server" id="selection_label" Visible="false">Select Appointment below to Cancel</asp:Label>
            </div>
            <div style="padding-top:20px;padding-left:100px;padding-right:100px">
                <asp:GridView Visible="False" CssClass="table table-bordered table-hover"  OnSelectedIndexChanged="OnSelectedIndexChanged" ID="appointmentGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="primary_amsid,appointmentdate,slot" DataSourceID="imsdb" OnRowDataBound="appointmentGridView_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="primary_amsid" HeaderText="primary_amsid" ReadOnly="True" SortExpression="primary_amsid" />
                        <asp:BoundField DataField="appointmentdate" HeaderText="appointmentdate" SortExpression="appointmentdate" ReadOnly="True" />
                        <asp:BoundField DataField="secondary_amsid" HeaderText="secondary_amsid" SortExpression="secondary_amsid" />
                        <asp:BoundField DataField="slot" HeaderText="slot" ReadOnly="True" SortExpression="slot" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="imsdb" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [Appointments]"></asp:SqlDataSource>
            </div>

            <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
               confirm_value.name = "confirm_value";
            if (confirm("Are you sure to cancel the appointment?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            $('ContentPlaceHolder1_temp').text = "hiiiiii";

        }
    </script>
           <!-- <asp:Button ID="btnConfirm" runat="server" Text = "Raise Confirm" OnClientClick = "Confirm()"/>
            <asp:TextBox runat="server" ID="temp"></asp:TextBox> -->
      </fieldset>
    </div>
</asp:Content>

