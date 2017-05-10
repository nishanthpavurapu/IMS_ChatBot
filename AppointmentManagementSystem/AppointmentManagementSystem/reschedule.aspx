<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="reschedule.aspx.cs" Inherits="reschedule" EnableEventValidation="false" ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin-top: 25px">
        <fieldset>
            <!-- Form Name -->

            <div class="row col-xs-offset-2">
                <div class="col-xs-9 space-vert center">
                    <div class="panel-group">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h4>Re-schedule Appointment</h4>
                            </div>
                            <div class="panel-body">
                          <!--       <br />  -->
                                 <div class="row" id="dateofappointment_textbox_div" runat="server" visible="false">
                                     <asp:Label runat="server" id="doa_label" CssClass="col-md-4 control-label center" for="userid">Date of Appointment</asp:Label>
                                    <div class="col-md-4 center">
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
                                <!--       <br />  -->
                                <div class="row" id="dateofappointment_button_div" runat="server" style="padding-top:10px" visible="false">
                                    <label class="col-md-4 control-label center" for="getappointments"></label>
                                    <div class="col-md-4 center">
                                        <asp:Button ID="getappointmentbutton" runat="server" Text="Get Appointments!" CssClass="btn btn-primary" OnClick="getappointment_Click" />
                                    </div>
                                </div>
                                <!--       <br />  -->
                                <div class="row" id="gridview_div" runat="server" >
                                    <div class="row col-xs-offset-2">
                                        <div class="col-md-8 text-center">
                                            <asp:Label runat="server" ID="selection_label">Select any upcoming appointment below to Re-Schedule</asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 text-center" style="padding-top:20px">
                                        <asp:GridView CssClass="table table-bordered table-hover" EmptyDataText="No appointments found!" OnSelectedIndexChanged="OnSelectedIndexChanged" ID="appointmentGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="primary_amsid,appointmentdate,slot" DataSourceID="imsdb" OnRowDataBound="appointmentGridView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="primary_amsid" HeaderStyle-CssClass="text-center" HeaderText="Primary Attendee" ReadOnly="True" SortExpression="primary_amsid">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="secondary_amsid" HeaderStyle-CssClass="text-center" HeaderText="Secondary Attendee" SortExpression="secondary_amsid">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="appointmentdate" HeaderStyle-CssClass="text-center" HeaderText="Appointment Date" ReadOnly="True" SortExpression="appointmentdate">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="slot" HeaderStyle-CssClass="text-center" HeaderText="Appointment Slot" ReadOnly="True" SortExpression="slot">
                                                    <HeaderStyle CssClass="text-center"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="comments" HeaderText="Agenda" SortExpression="comments" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="imsdb" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [Appointments]"></asp:SqlDataSource>
                                    </div>
                                </div>
                                <div id="userdetails_div" runat="server" class="row col-xs-offset-2"  visible="false">
                                    <div class="col-xs-10 space-vert center">
                                        <div  class="panel-group">
                                            <div class="row" runat="server" style="padding-top: 10px">
                                                <asp:Label runat="server" ID="primaryAttendee_label" CssClass="col-md-4 control-label left">Primary Attendee</asp:Label>
                                                <div class="col-md-4 center">
                                                    <asp:TextBox runat="server" ID="primaryAttendeeTxt" CssClass="form-control input-md"/>
                                                </div>
                                            </div>
                                             <div class="row" runat="server" style="padding-top: 10px">
                                                <asp:Label runat="server" ID="secondaryAttendee_label" CssClass="col-md-4 control-label left">Secondary Attendee</asp:Label>
                                                <div class="col-md-4 center">
                                                    <asp:TextBox runat="server" ID="secondaryAttendeeTxt" CssClass="form-control input-md"/>
                                                </div>
                                            </div>
                                            <div class="row" runat="server" style="padding-top: 10px">
                                                <asp:Label runat="server" id="dateofappointment_edit_label" class="col-md-4 control-label">Date of Appointment</asp:Label>
                                                <div class="col-md-4 center">
                                                    <asp:textbox runat="server" id="dateofappointment_edit_txt" AutoPostBack="true" OnTextChanged="dateofappointmenttxt_TextChanged" cssclass="form-control input-md"/>
                                                    <script type="text/javascript">
                                                        $(function () {
                                                            $('#ContentPlaceHolder1_dateofappointment_edit_txt').datetimepicker(
                                                                {
                                                                    format: 'MM/DD/YYYY'
                                                                }
                                                                );
                                                        });
                                                    </script>
                                                </div>
                                            </div>
                                            <div class="row" runat="server" style="padding-top: 10px">
                                                <asp:Label runat="server" ID="available_list_title" CssClass="col-md-4 control-label center" Visible="false"><b>Available Slots</b></asp:Label>
                                                <div class="col-md-4 center">
                                                    <asp:ListBox runat="server" ID="appointment_edit_list" Visible="false" style="float:left" CssClass="text-center" Font-Bold="True" Font-Overline="False" Font-Size="Medium" Height="202px" Width="96px"></asp:ListBox>
                                                </div>
                                            </div>
                                            <div class="row" runat="server" style="padding-top: 20px">
                                                <asp:Button ID="getavailableslots" runat="server" Text="Get Available Slots" CssClass="btn btn-primary" OnClick="getavailableslots_Click"/>
                                                <asp:Button ID="rescheduleAppointment" runat="server" Text="Re-Schedule Appointment" CssClass="btn btn-primary" OnClick="reschedule_appointment_Click" Visible="false" />
                                            </div>
                                            <div class="row" runat="server" style="padding-top: 10px">
                                                <asp:Label runat="server" CssClass="col-md-4 control-label center"></asp:Label>
                                                <div class="col-md-4 center">
                                                    <asp:TextBox runat="server" ID="oldslot" CssClass="form-control input-md" Visible="false"/>
                                                </div>
                                            </div>
                                        </div>
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

