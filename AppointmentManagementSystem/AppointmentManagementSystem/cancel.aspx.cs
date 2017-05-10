using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cancel : System.Web.UI.Page
{
    string primaryAttendee, dateofappointment, appointmentSlot_retrieved, primaryAttendee_retrieved, secondaryAttendee_retrieved, appointmentDate_retrieved;
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
        load_gridview();

    }

    protected void getappointment_Click(object sender, EventArgs e)
    {
        dateofappointment = dateofappointmenttxt.Text;

        dateofappointment_button_div.Visible = false;
        dateofappointment_textbox_div.Visible = false;

        gridview_div.Visible = true;

        load_gridview();
    }

    protected void load_gridview()
    {
        if (Session["isuseradmin"].Equals("yes"))
        {
            imsdb.SelectCommand = "select * from Appointments where appointmentdate='" + dateofappointment + "'";
        }
        else
        {
            imsdb.SelectCommand = "select * from Appointments where (primary_amsid='"+primaryAttendee+"' or secondary_amsid='"+primaryAttendee+"')";
           // imsdb.SelectCommand = "select * from Appointments where primary_amsid='" + primaryAttendee + "' and appointmentdate='" + dateofappointment + "'";
        }
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
        int gridIndexSelected_retrieved = appointmentGridView.SelectedRow.RowIndex;

        primaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[0].Text;
        secondaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[1].Text;
        appointmentDate_retrieved = appointmentGridView.SelectedRow.Cells[2].Text;
        appointmentSlot_retrieved = appointmentGridView.SelectedRow.Cells[3].Text;    

      ClientScript.RegisterStartupScript(GetType(), "Cancel Confirmation", "<script type='text/javascript'>Confirm()</script>'");
    OnConfirm();

        primaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[0].Text;

    }

    protected void cancel_appointment()
    {
        imsdb.DeleteCommand = "delete from Appointments where primary_amsid='" + primaryAttendee_retrieved + "' and slot='" + appointmentSlot_retrieved + "'";
        imsdb.Delete();
        
        //Sending Confirmation Email
        SendAmsEmail obj = new SendAmsEmail(primaryAttendee, secondaryAttendee_retrieved, appointmentSlot_retrieved);
        obj.sendDeleteConfirmation();

        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Appointment has been cancelled successfully !');window.location='cancel.aspx';</script>'");
    }

    public void OnConfirm()
    {
//string confirmValue = temp.Text;
  //      if (confirmValue == "Yes")
    //    {
            cancel_appointment();
      //  }
    }
}