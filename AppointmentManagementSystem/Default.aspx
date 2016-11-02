<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div style="margin-top: 50px">
        <fieldset>

            <!-- Form Name -->

            <legend class="center">Create Appointment</legend>
            <!-- Search input-->
            <div class="form-group">
                <label class="col-md-4 control-label" for="userid">AMS User ID</label>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="userid" placeholder="Email / User ID" CssClass="form-control input-md" required="" />
                </div>
            </div>

            <!-- Text input-->
            <div class="form-group">
                <label class="col-md-4 control-label" for="appointmentDate">Date of Appointment</label>
                <div class="col-md-2">

                    <asp:TextBox runat="server" id="appointmentDate" CssClass="form-control input-append date" />
                </div>
                <script type="text/javascript">
                    $(function () {
                        $('#ContentPlaceHolder1_appointmentDate').datetimepicker(
                            {
                                format: 'MM/DD/YYYY'
                            }
                            );

                    });
                </script>
            </div>

            <!-- Button -->
            <div class="form-group">
                <label class="col-md-4 control-label" for="getavailableslots"></label>
                <div class="col-md-2">
                    <asp:Button ID="getavailableslots" runat="server" Text="Get Available timings" CssClass="btn btn-primary" OnClick="getavailableslots_Click" />
                </div>
            </div>

             <div class="form-group">
                <asp:Label runat="server" CssClass="col-md-4 control-label" Visible="false" ID="available_list_title" for="Button1"><b>Available Slots</b></asp:Label>
                <div class="col-md-2">
                    <asp:ListBox runat="server" ID="Listbox1" Visible="false" CssClass="text-center" Font-Bold="True" Font-Overline="False" Font-Size="Medium" Height="202px" Width="96px">
                    </asp:ListBox>
                </div>
            </div>

            <div class="form-group">
                <label class="col-md-4 control-label" for="bookappointment"></label>
                <div class="col-md-2">
                    <asp:Button ID="bookappointment" runat="server" Text="Book Appointment" CssClass="btn btn-primary" OnClick="bookappointment_Click" Visible="false" />
                </div>
            </div>


        </fieldset>
    </div>
</asp:Content>
