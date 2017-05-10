<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cancel.aspx.cs" Inherits="cancel" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top: 25px">
        <fieldset>

            <div class="row col-xs-offset-2">
                <div class="col-xs-9 space-vert center">
                    <div class="panel-group">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h4>Cancel Appointment</h4>
                            </div>
                            <div class="panel-body">
                                <div class="row" id="dateofappointment_textbox_div" runat="server" visible="false">
                                    <asp:Label runat="server" ID="doa_label" class="col-md-4 control-label center" for="userid">Date of Appointment</asp:Label>
                                    <div class="col-md-4 center">
                                        <asp:TextBox runat="server" ID="dateofappointmenttxt" CssClass="form-control input-md" required="" />
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
                                <div class="row" id="dateofappointment_button_div" runat="server" style="padding-top: 10px" visible="false">
                                    <label class="col-md-4 control-label center" for="getappointments"></label>
                                    <div class="col-md-4 center">
                                        <asp:Button ID="getappointmentbutton" runat="server" Text="Get Appointments!" CssClass="btn btn-primary" OnClick="getappointment_Click" />
                                    </div>
                                </div>
                                <div class="row" id="gridview_div" runat="server">
                                    <div class="row col-xs-offset-2">
                                        <div class="col-md-8 text-center">
                                            <asp:Label runat="server" ID="selection_label">Select Appointment below to Cancel</asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 text-center" style="padding-top: 20px">
                                        <asp:GridView CssClass="table table-bordered table-hover" EmptyDataText="No appointments found!" OnSelectedIndexChanged="OnSelectedIndexChanged" ID="appointmentGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="primary_amsid,appointmentdate,slot" DataSourceID="imsdb" OnRowDataBound="appointmentGridView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="primary_amsid" HeaderText="Primary Attendee" HeaderStyle-CssClass="text-center" ReadOnly="True" SortExpression="primary_amsid">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="secondary_amsid" HeaderText="Secondary Attendee" HeaderStyle-CssClass="text-center" SortExpression="secondary_amsid">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="appointmentdate" HeaderText="Appointment Date" HeaderStyle-CssClass="text-center" ReadOnly="True" SortExpression="appointmentdate">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="slot" HeaderText="Appointment Slot" HeaderStyle-CssClass="text-center" ReadOnly="True" SortExpression="slot">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="comments" HeaderText="Agenda" SortExpression="comments" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="imsdb" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [Appointments]"></asp:SqlDataSource>
                                        <script type="text/javascript">
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
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>

