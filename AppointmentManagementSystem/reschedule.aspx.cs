using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class reschedule : System.Web.UI.Page
{
    string primaryAttendee,dateofappointment;
    private object BindingSourceControl;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["amscurrentuser"] == null || Session["amscurrentuser"].Equals(""))
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            primaryAttendee = Session["amscurrentuser"].ToString();
        }
        appointmentGridView.Visible = false;
        selection_label.Visible = false;
    }

    protected void getappointment_Click(object sender, EventArgs e)
    {
        dateofappointment = dateofappointmenttxt.Text;

        doa_label.Visible = false;
        dateofappointmenttxt.Visible = false;
        getappointmentbutton.Visible = false;
        appointmentGridView.Visible = true;
        selection_label.Visible = true;
        load_gridview();
    }


    protected void appointmentGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(appointmentGridView, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["style"] = "cursor:pointer";
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = appointmentGridView.SelectedRow.RowIndex;
        string name = appointmentGridView.SelectedRow.Cells[0].Text;
        string country = appointmentGridView.SelectedRow.Cells[1].Text;
        string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
    }


    protected void load_gridview()
    {
        imsdb.SelectCommand = "select * from Appointments where (primary_amsid='"+primaryAttendee+"' or secondary_amsid='"+primaryAttendee+"') and appointmentdate='"+dateofappointment+"'";
    }

}