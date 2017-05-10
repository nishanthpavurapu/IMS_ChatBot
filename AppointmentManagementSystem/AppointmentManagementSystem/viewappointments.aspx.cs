using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

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
        users_list.Items.Remove(getUserName(primaryAttendee));

    }

    protected void getappointment_Click(object sender, EventArgs e)
    {
        dateofappointment = dateofappointmenttxt.Text;

        dateofappointment_button_div.Visible = false;
        dateofappointment_textbox_div.Visible = false;
        amsid_textbox_div.Visible = false;

        gridview_div.Visible = true;
        load_gridview();
    }

    protected void load_gridview()
    {
        // string secondary_userid="";
        //    if (userid.Text != "")
        //   {
        //       secondary_userid = userid.Text;
        //      imsdb.SelectCommand = "select * from Appointments where primary_amsid='" + primaryAttendee + "' and appointmentdate='" + dateofappointment + "' and secondary_amsid='" + secondary_userid + "'";
        //  }
        //  else
        // {
        if (Session["isuseradmin"].Equals("yes"))
        {
            imsdb.SelectCommand = "select * from Appointments";
        }
        else
        { 
            imsdb.SelectCommand = "select * from Appointments where primary_amsid='" + primaryAttendee + "' or secondary_amsid='"+ primaryAttendee + "'";
        }// }
        //imsdb.SelectCommand = "select * from Appointments where (primary_amsid='"+primaryAttendee+"' or secondary_amsid='"+primaryAttendee+"') and appointmentdate='"+dateofappointment+"'";
    }

    //protected void appointmentGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(appointmentGridView, "Select$" + e.Row.RowIndex);
    //        e.Row.Attributes["style"] = "cursor:pointer";
    //    }
    //}

    //protected void OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int gridIndexSelected_retrieved = appointmentGridView.SelectedRow.RowIndex;

    //    primaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[0].Text;
    //    secondaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[1].Text;
    //    appointmentDate_retrieved = appointmentGridView.SelectedRow.Cells[2].Text;
    //    appointmentSlot_retrieved = appointmentGridView.SelectedRow.Cells[3].Text;    

    //  ClientScript.RegisterStartupScript(GetType(), "Cancel Confirmation", "<script type='text/javascript'>Confirm()</script>'");
    //OnConfirm();

    //    primaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[0].Text;

    //}

    //protected void cancel_appointment()
    //{
    //    imsdb.DeleteCommand = "delete from Appointments where primary_amsid='" + primaryAttendee_retrieved + "' and slot='" + appointmentSlot_retrieved + "'";
    //    imsdb.Delete();
    //    //cancelButton.Visible = false;
    //    ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Appointment has been cancelled successfully !');window.location='cancel.aspx';</script>'");

    //}

//    public void OnConfirm()
//    {
////string confirmValue = temp.Text;
//  //      if (confirmValue == "Yes")
//    //    {
//            cancel_appointment();
//      //  }
//    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> SearchUsers(string prefixText, int count)
    {
        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select amsid from Users where " +
                "amsid like @SearchText + '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Connection = con;
                con.Open();
                List<string> users = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        users.Add(sdr["amsid"].ToString());
                    }
                }
                con.Close();
                return users;
            }
        }
    }

    protected void appointmentGridView_RowDeleted(object sender, GridViewDeleteEventArgs e)
    {
        primaryAttendee_retrieved = appointmentGridView.DataKeys[e.RowIndex].Values[0].ToString();
        appointmentDate_retrieved = appointmentGridView.DataKeys[e.RowIndex].Values[1].ToString();
        appointmentSlot_retrieved = appointmentGridView.DataKeys[e.RowIndex].Values[2].ToString();
        secondaryAttendee_retrieved = appointmentGridView.Rows[e.RowIndex].Cells[3].Text.ToString();

        imsdb.DeleteCommand = "delete from Appointments where primary_amsid='" + primaryAttendee_retrieved + "' and slot='" + appointmentSlot_retrieved + "' and appointmentdate='" + appointmentDate_retrieved + "'";
        imsdb.Delete();

        //Sending Confirmation Email
        SendAmsEmail obj = new SendAmsEmail(primaryAttendee_retrieved, secondaryAttendee_retrieved, appointmentDate_retrieved + ", " + appointmentSlot_retrieved);
        obj.sendDeleteConfirmation();
    }

    protected void appointmentgridview_row_command(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "reschedule_appointment_grid")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = appointmentGridView.Rows[index];

            primaryAttendee_retrieved = row.Cells[2].Text.ToString();
            secondaryAttendee_retrieved = row.Cells[3].Text.ToString();
            appointmentDate_retrieved = row.Cells[4].Text.ToString();
            appointmentSlot_retrieved = row.Cells[5].Text.ToString();


            string url = "reschedule.aspx?primaryamsid=" + primaryAttendee_retrieved + "&secondaryamsid=" + secondaryAttendee_retrieved + "&dateofappointment=" + appointmentDate_retrieved + "&timeslot=" + appointmentSlot_retrieved;
            Response.Redirect(url);
        }
    }

    protected void appointmentGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != appointmentGridView.EditIndex)
        {
            (e.Row.Cells[0].Controls[0] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to cancel this appointment?');";
        }
    }

    protected void filterbutton_click(object sender, EventArgs e)
    {
        string userFilterText, typeFilterText;

        string currentDate = DateTime.Now.ToString("MM/dd/yyyy");

        string futureDate_condition = "and (cast(appointmentdate as date) >= '" + currentDate + "')";
        string pastDate_condition = "and (cast(appointmentdate as date) <= '" + currentDate + "')";
        string currentDate_condition = "and (cast(appointmentdate as date) = '" + currentDate + "')";

        string userCondition;
        string typecondition;
        string monthCondition;

        string monthFilterText = monthYearFilterText.Text;
        userFilterText = getUserAmsid(users_list.SelectedValue.ToString());
        typeFilterText = appointment_type_list.SelectedValue.ToString();


        //Evaluating appointment type filter
        if (appointment_type_list.SelectedValue == "Today's Appointments")
        {
            typecondition = currentDate_condition;
        }
        else if (appointment_type_list.SelectedValue == "Past Appointments")
        {
            typecondition = pastDate_condition;
        }
        else if (appointment_type_list.SelectedValue == "Future Appointments")
        {
            typecondition = futureDate_condition;
        }
        else
        {
            typecondition = "";
        }

        //Evaluating user filter
        if (users_list.SelectedValue == "Select User")
        {
            userCondition = "(primary_amsid='" + primaryAttendee + "' or secondary_amsid='" + primaryAttendee + "')";
            if (Session["isuseradmin"].Equals("yes"))
            {
                userCondition = "";
            }
        }
        else
        {
            if (Session["isuseradmin"].Equals("yes"))
            {
                primaryAttendee = getUserAmsid(users_list.SelectedValue.ToString());
                userCondition = "(primary_amsid IN('" + primaryAttendee + "', '" + userFilterText + "') or secondary_amsid IN('" + primaryAttendee + "', '" + userFilterText + "'))";
            }
            else
            {
                userCondition = "(primary_amsid IN('" + primaryAttendee + "', '" + userFilterText + "') and secondary_amsid IN('" + primaryAttendee + "', '" + userFilterText + "'))";
            }            
        }

        //Evaluating month/year filter
        if (monthYearFilterText.Text == "")
        {
            monthCondition = "";
        }
        else
        {
            string[] date_split = monthYearFilterText.Text.Split('/');
            monthCondition = "and (month(CAST(appointmentdate as date)) = " + Convert.ToInt32(date_split[0]) + " and year(cast(appointmentdate as date)) = " + Convert.ToInt32(date_split[1]) + ")";
        }
        //applying filters
        if (userCondition == "" && typecondition == "" && monthCondition == "")
        {
            imsdb.SelectCommand = "select * from Appointments";
        }
        else
        { 
            imsdb.SelectCommand = "select * from Appointments where " + userCondition + typecondition + monthCondition;
        }
        appointmentGridView.DataBind();        

    }

    protected string getUserAmsid(string userName)
    {
        string amsid="";

        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_getUserEmails = new SqlCommand("select amsid from Users where Name='"+ userName + "'", con);
            con.Open();

            SqlDataReader reader_primary = cmd_getUserEmails.ExecuteReader();

            if (reader_primary.HasRows)
            {
                if (reader_primary.Read())
                {
                    amsid = reader_primary[0].ToString();
                }
            }
            reader_primary.Close();
            con.Close();
            return amsid;
        }
    }

    protected string getUserName(string amsid)
    {
        string userName = "";

        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_getUserName = new SqlCommand("select Name from Users where amsid='" + amsid + "'", con);
            con.Open();

            SqlDataReader reader_primary = cmd_getUserName.ExecuteReader();

            if (reader_primary.HasRows)
            {
                if (reader_primary.Read())
                {
                    userName = reader_primary[0].ToString();
                }
            }
            reader_primary.Close();
            con.Close();
            return userName;
        }
    }
}