<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="reschedule.aspx.cs" Inherits="reschedule" EnableEventValidation="false" ViewStateMode="Enabled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin-top: 25px">
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
            <div style="padding-top:20px;padding-left:100px;padding-right:100px">
                <asp:GridView Visible="False" CssClass="table table-bordered table-hover"  OnSelectedIndexChanged="OnSelectedIndexChanged" ID="appointmentGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="primary_amsid,appointmentdate,slot" DataSourceID="imsdb" OnRowDataBound="appointmentGridView_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="primary_amsid" HeaderText="Primary Attendee" ReadOnly="True" SortExpression="primary_amsid" />
                        <asp:BoundField DataField="secondary_amsid" HeaderText="Secondary Attendee" SortExpression="secondary_amsid" />
                        <asp:BoundField DataField="appointmentdate" HeaderText="Appointment Date" ReadOnly="True" SortExpression="appointmentdate" />
                        <asp:BoundField DataField="slot" HeaderText="Appointment Slot" ReadOnly="True" SortExpression="slot" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="imsdb" runat="server" ConnectionString="<%$ ConnectionStrings:amsdbConnectionString %>" SelectCommand="SELECT * FROM [Appointments]"></asp:SqlDataSource>
            </div>

            <div class="form-group" style="text-align:center">
                    <asp:Label runat="server" ID="primaryAttendee_label" CssClass="col-md-4 control-label" Visible="false">Primary Attendee</asp:Label>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" Visible="false" ID="primaryAttendeeTxt" CssClass="form-control input-md"/>
                    </div>
            </div>
            <div class="form-group">
                    <asp:Label runat="server" ID="secondaryAttendee_label" CssClass="col-md-4 control-label" Visible="false">Secondary Attendee</asp:Label>
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="secondaryAttendeeTxt" CssClass="form-control input-md" Visible="false"/>
                    </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" id="dateofappointment_edit_label" class="col-md-4 control-label" Visible="false">Date of Appointment</asp:Label>
                <div class="col-md-2">
                    <asp:textbox runat="server" id="dateofappointment_edit_txt" cssclass="form-control input-md" Visible="false"/>
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

            <div class="form-group">
                <asp:Label runat="server" CssClass="col-md-4 control-label" Visible="false" ID="available_list_title" ><b>Available Slots</b></asp:Label>
                <div class="col-md-2">
                    <asp:ListBox runat="server" ID="appointment_edit_list" Visible="false" CssClass="text-center" Font-Bold="True" Font-Overline="False" Font-Size="Medium" Height="202px" Width="96px">
                    </asp:ListBox>
                </div>
            </div>

            <div class="form-group">
                    <asp:Label runat="server" CssClass="col-md-4 control-label"></asp:Label>    
                    <asp:Button ID="getavailableslots" runat="server" Text="Get Available Slots" CssClass="btn btn-primary" OnClick="getavailableslots_Click" Visible="false"/>
                    <asp:Button ID="rescheduleAppointment" runat="server" Text="Re-Schedule Appointment" CssClass="btn btn-primary" OnClick="reschedule_appointment_Click" Visible="false" />
            </div>
                        <div class="form-group">
                    <div class="col-md-2">
                        <asp:TextBox runat="server" ID="oldslot" CssClass="form-control input-md" Visible="false"/>
                    </div>
            </div>
      </fieldset>
    </div>
</asp:Content>

