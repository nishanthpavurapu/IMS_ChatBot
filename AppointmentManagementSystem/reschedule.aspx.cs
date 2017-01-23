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
    string primaryAttendee, dateofappointment, day_of_appointment;
    string available_time_start, available_time_end, nonavailable_days;
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
        primaryAttendeeTxt.Visible = true;
        primaryAttendee_label.Visible = true;
        secondaryAttendeeTxt.Visible = true;
        secondaryAttendee_label.Visible = true;
        dateofappointment_edit_txt.Visible = true;
        dateofappointment_edit_label.Visible = true;
        getavailableslots.Visible = true;

        int gridIndexSelected_retrieved = appointmentGridView.SelectedRow.RowIndex;
        string primaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[0].Text;
        string secondaryAttendee_retrieved = appointmentGridView.SelectedRow.Cells[1].Text;
        string appointmentDate_retrieved = appointmentGridView.SelectedRow.Cells[2].Text;
        appointmentSlot_retrieved = appointmentGridView.SelectedRow.Cells[3].Text;

        primaryAttendeeTxt.Text = primaryAttendee_retrieved;
        primaryAttendeeTxt.Enabled = false;

        secondaryAttendeeTxt.Text = secondaryAttendee_retrieved;
        secondaryAttendeeTxt.Enabled = false;

        dateofappointment_edit_txt.Text = appointmentDate_retrieved;

        oldslot.Text = appointmentSlot_retrieved;


    }


    protected void load_gridview()
    {
        //imsdb.SelectCommand = "select * from Appointments where (primary_amsid='"+primaryAttendee+"' or secondary_amsid='"+primaryAttendee+"') and appointmentdate='"+dateofappointment+"'";
        imsdb.SelectCommand = "select * from Appointments where primary_amsid='"+primaryAttendee+"' and appointmentdate='"+dateofappointment+"'";
    }


    protected void reschedule_appointment_Click(object sender, EventArgs e)
    {
        if (appointment_edit_list.SelectedItem == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please select any slot to proceed!');", true);
        }
        else
        {
            imsdb.UpdateCommand = "update Appointments set slot='" + appointment_edit_list.SelectedItem.Text + "' where primary_amsid='" + primaryAttendee + "' and appointmentdate='" + dateofappointmenttxt.Text + "' and slot='" + oldslot.Text + "'";
            imsdb.Update();
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
}