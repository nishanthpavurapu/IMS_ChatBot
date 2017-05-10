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
    string appointmentSlot_retrieved, appointmentSlot_retrieved_backup;
    string primaryAttendee, dateofappointment, day_of_appointment, secondaryAttendee_retrieved, appointmentDate_retrieved;
    string available_time_start, available_time_end, nonavailable_days;
    private object BindingSourceControl;
    string primaryAttendee_retrieved;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["amscurrentuser"] == null || Session["amscurrentuser"].Equals(""))
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            //if (Session["isuseradmin"].Equals("yes"))
            //{
            //    primaryAttendee = primaryAttendee_retrieved;
            //}
            //else
            //{
                if (Request.QueryString["primaryamsid"] != null)
                {
                    primaryAttendee_retrieved = Request.QueryString["primaryamsid"].ToString();
                    secondaryAttendee_retrieved = Request.QueryString["secondaryamsid"].ToString();
                    appointmentDate_retrieved = Request.QueryString["dateofappointment"].ToString();
                    appointmentSlot_retrieved = Request.QueryString["timeslot"].ToString();

                    dateofappointment_button_div.Visible = false;
                    dateofappointment_textbox_div.Visible = false;

                    userdetails_div.Visible = true;

                    primaryAttendee = primaryAttendee_retrieved;
                    primaryAttendeeTxt.Text = primaryAttendee_retrieved;
                    primaryAttendeeTxt.Enabled = false;

                    secondaryAttendeeTxt.Text = secondaryAttendee_retrieved;
                    secondaryAttendeeTxt.Enabled = false;

                    if(! IsPostBack)
                    { 
                        dateofappointment_edit_txt.Text = appointmentDate_retrieved;
                        dateofappointmenttxt.Text = appointmentDate_retrieved;
                    }
                    gridview_div.Visible = false;
                    oldslot.Text = appointmentSlot_retrieved;
                }
                else
                {
                if (Session["isuseradmin"].Equals("yes"))
                {
                    primaryAttendee = primaryAttendee_retrieved;
                }
                else
                {
                    primaryAttendee = Session["amscurrentuser"].ToString();
                }
                if (!IsPostBack)
                { 
                    load_gridview();
                }
            }
           // }            
        }
    }

    protected void getappointment_Click(object sender, EventArgs e)
    {
        dateofappointment = dateofappointmenttxt.Text;
        gridview_div.Visible = true;
        dateofappointment_button_div.Visible = false;
        dateofappointment_textbox_div.Visible = false;
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

        userdetails_div.Visible = true;

        int gridIndexSelected_retrieved = appointmentGridView.SelectedRow.RowIndex;
        primaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[0].Text;
        secondaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[1].Text;
        appointmentDate_retrieved = appointmentGridView.SelectedRow.Cells[2].Text;
        appointmentSlot_retrieved = appointmentGridView.SelectedRow.Cells[3].Text;

        primaryAttendeeTxt.Text = primaryAttendee_retrieved;
        primaryAttendeeTxt.Enabled = false;

        secondaryAttendeeTxt.Text = secondaryAttendee_retrieved;
        secondaryAttendeeTxt.Enabled = false;

        dateofappointment_edit_txt.Text = appointmentDate_retrieved;

        oldslot.Text = appointmentSlot_retrieved;

        if (Session["isuseradmin"].Equals("yes"))
        {
            primaryAttendee = primaryAttendee_retrieved;
        }
        gridview_div.Visible = false;

    }


    protected void load_gridview()
    {
        string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
        if (Session["isuseradmin"].Equals("yes"))
        {
            imsdb.SelectCommand = "select * from Appointments";
        }
        else
        {
            imsdb.SelectCommand = "select * from Appointments where (primary_amsid='"+primaryAttendee+"' or secondary_amsid='"+primaryAttendee+"') and (cast(appointmentdate as date) >='"+currentDate+"')";
        //imsdb.SelectCommand = "select * from Appointments where primary_amsid='"+primaryAttendee+"' and appointmentdate='"+dateofappointment+"'";
        }
    }


    protected void reschedule_appointment_Click(object sender, EventArgs e)
    {
        if (appointment_edit_list.SelectedItem == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please select any slot to proceed!');", true);
        }
        else
        {
            imsdb.UpdateCommand = "update Appointments set slot='" + appointment_edit_list.SelectedItem.Text + "',appointmentdate='" + dateofappointment_edit_txt.Text + "' where primary_amsid='" + primaryAttendeeTxt.Text.ToString() + "' and appointmentdate='" + dateofappointmenttxt.Text + "' and slot='" + oldslot.Text + "'";
            imsdb.Update();

            //Sending Confirmation Email
            SendAmsEmail obj = new SendAmsEmail(primaryAttendeeTxt.Text, secondaryAttendeeTxt.Text, dateofappointment_edit_txt.Text + ", " + appointment_edit_list.SelectedItem.Text);
            obj.sendEditConfirmation();

            ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Appointment has been re-scheduled successfully !');window.location='Default.aspx';</script>'");
        }
    }

    protected void generate_slots_list(string startTime, string EndTime)
    {
        String[] slots_array = new String[48];
        // List<String> dropdown_list = new List<String>();
        String booked_slot_db;
        int i = 0, count = 0;
        String hr = startTime;

        //Clearing all list items if exists

        appointment_edit_list.Items.Clear();

        while (true)
        {
            slots_array[i] = hr + ":00";
            i++;
            slots_array[i] = hr + ":30";
            i++;
            count = count + 2;
            if ((Convert.ToInt16(hr) + 1) == Convert.ToInt16(EndTime))
            {
                appointment_edit_list.Visible = true;
                available_list_title.Visible = true;
                rescheduleAppointment.Visible = true;
                for (int j = 0; j < count; j++)
                {
                    appointment_edit_list.Items.Add(slots_array[j]);
                    //dropdown_list.Add(slots_array[j]);

                }
                break;
            }
            else
            {
                hr = Convert.ToString(Convert.ToInt16(hr) + 1);
            }
        }

        //Logic to remove the booked slots entries from the list box

        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_available_timings_primary = new SqlCommand("select slot from Appointments where primary_amsid='" + primaryAttendee + "' and appointmentdate='" + dateofappointment_edit_txt.Text + "'", con);

            SqlCommand cmd_available_timings_secondary = new SqlCommand("select slot from Appointments where primary_amsid='" + secondaryAttendeeTxt.Text + "' and appointmentdate='" + dateofappointment_edit_txt.Text + "'", con);

            con.Open();

            SqlDataReader reader_primary = cmd_available_timings_primary.ExecuteReader();

            if (reader_primary.HasRows)
            {
                while (reader_primary.Read())
                {
                    booked_slot_db = reader_primary.GetString(0);
                    appointment_edit_list.Items.Remove(appointment_edit_list.Items.FindByValue(booked_slot_db));
                }
            }
            reader_primary.Close();
            con.Close();
        }
        remove_secondary_slots();
    }

    protected void remove_secondary_slots()
    {
        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_available_timings_secondary = new SqlCommand("select slot from Appointments where primary_amsid='" + secondaryAttendeeTxt.Text + "' and appointmentdate='" + dateofappointment_edit_txt.Text + "'", con);
            con.Open();

            SqlDataReader reader_secondary = cmd_available_timings_secondary.ExecuteReader();
            if (reader_secondary.HasRows)
            {
                while (reader_secondary.Read())
                {
                    string booked_slot_db_sec = reader_secondary.GetString(0);
                    appointment_edit_list.Items.Remove(appointment_edit_list.Items.FindByValue(booked_slot_db_sec));
                }
            }
            reader_secondary.Close();
            con.Close();
        }
    }
    
    protected void getavailableslots_Click(object sender, EventArgs e)
    {
        String date_Pattern = "MM/dd/yyyy";
        
        DateTime dateParsed;
        DateTime.TryParseExact(dateofappointment_edit_txt.Text, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
        day_of_appointment = dateParsed.ToString("ddd");


        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_available_timings = new SqlCommand("select availabletimestart,availabletimeend,nonavailabledays from profile where amsid='" + secondaryAttendeeTxt.Text + "'", con);
            con.Open();
            SqlDataReader reader = cmd_available_timings.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    available_time_start = reader.GetString(0);
                    available_time_end = reader.GetString(1);
                    nonavailable_days = reader.GetString(2);

                    if (nonavailable_days.Contains(day_of_appointment))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('user is not available today');", true);
                        appointment_edit_list.Items.Clear();
                    }
                    else
                    {
                        generate_slots_list(available_time_start, available_time_end);
                    }
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('No rows found');", true);
            }
        }

    }

    protected void dateofappointmenttxt_TextChanged(object sender, EventArgs e)
    {
       
    }
}