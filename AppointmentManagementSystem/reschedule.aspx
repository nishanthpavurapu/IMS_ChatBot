<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="reschedule.aspx.cs" Inherits="reschedule" EnableEventValidation="false" ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin-top: 50px">
        <fieldset>
            <!-- Form Name -->
            <legend class="center">Re-schedule Appointment</legend>
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
                <asp:Label runat="server" id="selection_label" Visible="false">Select Appointment below to Re-Schedule</asp:Label>
            </div>
            <div style=" padding-top:20px;padding-left:100px;padding-right:100px">
                <asp:GridView Visible="False" CssClass="table table-bordered table-hover"  OnSelectedIndexChanged="OnSelectedIndexChanged" ID="appointmentGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="primary_amsid,appointmentdate,slot" DataSourceID="imsdb" OnRowDataBound="appointmentGridView_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="primary_amsid" HeaderText="primary_amsid" ReadOnly="True" SortExpression="primary_amsid" />
                        <asp:BoundField DataField="appointmentdate" HeaderText="appointmentdate" ReadOnly="True" SortExpression="appointmentdate" />
                        <asp:BoundField DataField="secondary_amsid" HeaderText="secondary_amsid" SortExpression="secondary_amsid" />
                        <asp:BoundField DataField="slot" HeaderText="slot" ReadOnly="True" SortExpression="slot" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="imsdb" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [Appointments]"></asp:SqlDataSource>
            </div>
        </fieldset>
    </div>
</asp:Content>

