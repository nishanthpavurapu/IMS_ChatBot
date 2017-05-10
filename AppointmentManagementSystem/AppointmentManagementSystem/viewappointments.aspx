<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="viewappointments.aspx.cs" Inherits="cancel" EnableEventValidation="false" %>

<script runat="server">

    protected void filter_Click(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin-top: 25px">
        <fieldset>
            <asp:ScriptManager ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>

            <div class="row col-xs-offset-2">
                <div class="col-xs-9 space-vert center">
                    <div class="panel-group">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h4>View Appointments</h4>
                            </div>
                            <div class="panel-body">
                                <div id="appointment_selection_div">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="center">
                                            <label class="control-label center" for="filter_month">Filter by Month</label>
                                                <asp:TextBox runat="server" ID="monthYearFilterText" CssClass="form-control input-md" placeholder="mm/yyyy" />
                                                <script type="text/javascript">
                                                    $(function () {
                                                        $('#ContentPlaceHolder1_monthYearFilterText').datetimepicker(
                                                            {
                                                                viewMode: 'years',
                                                                format: 'MM/YYYY'
                                                            }
                                                            );
                                                    });
                                                </script>
                                                </div>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="control-label center" for="filter_month">Filter by User</label>
                                            <div class="center">
                                                <asp:DropDownList runat="server" ID="users_list" CssClass="form-control" AppendDataBoundItems="true" DataSourceID="SqlDataSource1" DataTextField="Name" DataValueField="Name">
                                                    <asp:ListItem>Select User</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT DISTINCT [Name] FROM [Users] WHERE ([amsid] &lt;&gt; @amsid)">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="admin" Name="amsid" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="control-label center" for="filter_month">Filter by Type</label>
                                            <div class="center">
                                                <asp:DropDownList runat="server" ID="appointment_type_list" CssClass="form-control">
                                                    <asp:ListItem Selected="True">Select Type</asp:ListItem>
                                                    <asp:ListItem>Today's Appointments</asp:ListItem>
                                                    <asp:ListItem>Past Appointments</asp:ListItem>
                                                    <asp:ListItem>Future Appointments</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="control-label center" for="filter_month"></label>
                                            <div class="left">
                                                <asp:Button ID="filter_button" runat="server" Text="Apply Filter" CssClass="btn btn-primary" OnClick="filterbutton_click" />
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                                <div class="row" id="dateofappointment_textbox_div" runat="server" visible="false">
                                    <label class="col-md-4 control-label center" for="userid">Date of Appointment</label>
                                    <div class="col-md-4 center">
                                        <asp:TextBox runat="server" ID="dateofappointmenttxt" CssClass="form-control input-md" required="" placeholder="mm/dd/yyyy" />
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
                                <br />
                                <div class="row" id="amsid_textbox_div" runat="server" visible="false">
                                    <label class="col-md-4 control-label center" for="userid">AMS User ID</label>
                                    <div class="col-md-4 center">
                                        <asp:TextBox runat="server" ID="userid" placeholder="AMS ID" CssClass="form-control input-md" />
                                        <ajaxToolkit:AutoCompleteExtender ID="userid_AutoCompleteExtender" runat="server" BehaviorID="userid_AutoCompleteExtender" DelimiterCharacters="" ServiceMethod="SearchUsers" MinimumPrefixLength="2" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" FirstRowSelected="false" TargetControlID="userid">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </div>
                                    <label class="col-md-4 control-label" style="text-align: left; color: brown" for="userid">(Optional)</label>
                                </div>

                                <div class="row" id="dateofappointment_button_div" runat="server" style="padding-top: 10px" visible="false">
                                    <label class="col-md-4 control-label center" for="getappointments"></label>
                                    <div class="col-md-4 center">
                                        <asp:Button ID="getappointmentbutton" runat="server" Text="Get Appointments!" CssClass="btn btn-primary" OnClick="getappointment_Click" />
                                    </div>
                                </div>
                                <div class="row" id="gridview_div" runat="server" style="margin-left:10px;margin-right:10px">
                                    <div class="col-lg-12 text-center" style="padding-top: 20px">
                                        <asp:GridView CssClass="table table-bordered table-hover" EmptyDataText="No appointments found!" OnRowCommand="appointmentgridview_row_command" OnRowDeleting="appointmentGridView_RowDeleted" ID="appointmentGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="primary_amsid,appointmentdate,slot" DataSourceID="imsdb" OnRowDataBound="appointmentGridView_RowDataBound">
                                            <Columns>
                                                <asp:CommandField ShowDeleteButton="True" DeleteText="Cancel" />
                                                <asp:ButtonField Text="Re-schedule" CommandName="reschedule_appointment_grid" />
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
                                                <asp:BoundField DataField="comments" HeaderText="Agenda" HeaderStyle-CssClass="text-center" SortExpression="comments" />
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

