<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit"%>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div style="margin-top: 50px">
        <fieldset>

            <asp:ScriptManager ID="ScriptManager1" runat="server"
                EnablePageMethods="true">
            </asp:ScriptManager>
            <!-- Form Name -->
            <div class="row col-xs-offset-2">
                <div class="col-xs-9 space-vert center">
                    <div class="panel-group">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <h4>Create Appointment</h4>
                            </div>
                            <div class="panel-body">
                                 <br />
                                 <div class="row">
                                    <label class="col-md-4 control-label center" for="userid">AMS User ID</label>
                                    <div class="col-md-4 center">
                                        <asp:TextBox runat="server" ID="userid" placeholder="AMS User ID" CssClass="form-control input-md" required=""/>
                                        <ajaxToolkit:AutoCompleteExtender ID="userid_AutoCompleteExtender" runat="server" BehaviorID="userid_AutoCompleteExtender" DelimiterCharacters="" ServiceMethod="SearchUsers" MinimumPrefixLength="2" CompletionInterval="100"
                                             EnableCaching="false" CompletionSetCount="10" FirstRowSelected = "false" TargetControlID="userid">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </div>
                                 </div>
                                <br />
                                 <div class="row" id="secondary_user_div" runat="server">
                                    <label class="col-md-4 control-label center" for="secondary_userid">AMS Secondary User ID</label>
                                    <div class="col-md-4 center">
                                        <asp:TextBox runat="server" ID="secondary_userid" placeholder="AMS User ID" CssClass="form-control input-md" required=""/>
                                        <asp:CompareValidator ID="usernames_compare_validator" Display="Dynamic" ControlToValidate="secondary_userid" ControlToCompare="userid" style="color:red" Operator="NotEqual" runat="server" ErrorMessage="Users can't be same!"></asp:CompareValidator>
                                        <ajaxToolkit:AutoCompleteExtender ID="secondary_userid_AutoCompleteExtender" runat="server" BehaviorID="secondary_userid_AutoCompleteExtender" DelimiterCharacters="" ServiceMethod="SearchUsers" MinimumPrefixLength="2" CompletionInterval="100"
                                             EnableCaching="false" CompletionSetCount="10" FirstRowSelected = "false" TargetControlID="secondary_userid">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </div>
                                 </div>
                                <br />

                                <div class="row">
                                    <label class="col-md-4 control-label center" for="appointmentDate">Date of Appointment</label>
                                    <div class="col-md-4 center">
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
                                <br />
                                <div class="row">
                                    <label class="col-md-4 control-label center" for="getavailableslots"></label>
                                    <div class="col-md-4 center">
                                        <asp:Button ID="getavailableslots" runat="server" style="float:left" Text="Get Available timings" CssClass="btn btn-primary" OnClick="getavailableslots_Click" />
                                    </div>
                                </div>
                                <br />
                                <div class="row" id="listbox_div" runat="server" visible="false">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label center" ID="available_list_title" for="Button1"><b>Available Slots</b></asp:Label>
                                    <div class="col-md-4">
                                        <asp:ListBox runat="server" data-style="btn-primary" ID="Listbox1" style="float:left" CssClass=" nav nav-pills text-center" Font-Bold="True" Font-Overline="False" Font-Size="Medium" Height="202px" Width="96px" SelectionMode="Multiple"></asp:ListBox>
                                    </div>
                                </div>
                                <br />
                                <div class="row" id="agenda_div" runat="server" visible="false">
                                    <label class="col-md-4 control-label center" for="appointmentAgenda">Agenda</label>
                                    <div class="col-md-4 center">
                                        <asp:TextBox runat="server" ID="appointmentAgenda" placeholder="Agenda for meetings" TextMode="MultiLine" CssClass="form-control input-md"/>
                                    </div>
                                 </div>
                                <br />
                                <div class="row">
                                    <label class="col-md-4 control-label center" for="bookappointment"></label>
                                    <div class="col-md-4 center">
                                        <asp:Button ID="bookappointment" runat="server" style="float:left" Text="Book Appointment" CssClass="btn btn-primary" OnClick="bookappointment_Click" Visible="false" />
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
