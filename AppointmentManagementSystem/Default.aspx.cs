using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;

public partial class _Default : System.Web.UI.Page
{

    String secondaryamsid, dateof_appointment,primaryAttendee;
    String available_time_start, available_time_end, nonavailable_days, day_of_appointment;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["amscurrentuser"] == null || Session["amscurrentuser"].Equals(""))
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            secondaryamsid = userid.Text;
            dateof_appointment = appointmentDate.Text;
            primaryAttendee = Session["amscurrentuser"].ToString();

        }

    }

    protected void get_availableslots(String startTime,String EndTime)
    {
        String[] slots_array= new String[48];
       // List<String> dropdown_list = new List<String>();
        String booked_slot_db;
        int i = 0,count=0;
        String hr=startTime;

        //Clearing all list items if exists

        Listbox1.Items.Clear();

        while(true)
        {
            slots_array[i] = hr + ":00";
                i++;
            slots_array[i] = hr + ":30";
            i++;
            count = count + 2;
            if ((Convert.ToInt16(hr) + 1) == Convert.ToInt16(EndTime))
            {
                Listbox1.Visible = true;
                available_list_title.Visible = true;
                bookappointment.Visible = true;
                for (int j = 0; j < count; j++)
                {
                    Listbox1.Items.Add(slots_array[j]);
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
            SqlCommand cmd_available_timings_primary = new SqlCommand("select slot from Appointments where primary_amsid='" + primaryAttendee  + "' and appointmentdate='"+dateof_appointment+"'", con);

            SqlCommand cmd_available_timings_secondary = new SqlCommand("select slot from Appointments where primary_amsid='" + secondaryamsid + "' and appointmentdate='" + dateof_appointment + "'", con);

            con.Open();

            SqlDataReader reader_primary = cmd_available_timings_primary.ExecuteReader();

            if (reader_primary.HasRows)
            {
                while (reader_primary.Read())
                {
                    booked_slot_db = reader_primary.GetString(0);
                   Listbox1.Items.Remove(Listbox1.Items.FindByValue(booked_slot_db));
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
            SqlCommand cmd_available_timings_secondary = new SqlCommand("select slot from Appointments where primary_amsid='" + secondaryamsid + "' and appointmentdate='" + dateof_appointment + "'", con);
            con.Open();

            SqlDataReader reader_secondary = cmd_available_timings_secondary.ExecuteReader();
            if (reader_secondary.HasRows)
            {
                while (reader_secondary.Read())
                {
                    string booked_slot_db_sec = reader_secondary.GetString(0);
                    Listbox1.Items.Remove(Listbox1.Items.FindByValue(booked_slot_db_sec));
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
        DateTime.TryParseExact(dateof_appointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
        day_of_appointment = dateParsed.ToString("ddd");


        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_available_timings = new SqlCommand("select availabletimestart,availabletimeend,nonavailabledays from profile where amsid='" + secondaryamsid + "'", con);
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
                    }
                    else
                    {
                        get_availableslots(available_time_start, available_time_end);
                    }
                }
                
                
           
            }
            else
            {
                    ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('No rows found');", true);
            }
        }

    }

    protected void bookappointment_Click(object sender, EventArgs e)
    {

         String appointmentTime;


         appointmentTime = Listbox1.SelectedItem.Value; 


        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("insert into Appointments values('" + primaryAttendee + "','" + dateof_appointment + "','" + secondaryamsid + "','" + appointmentTime + "')", con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('Appointment has been confirmed!');", true);
            }
            catch (SqlException ex)
            {
                if (ex.ErrorCode == 2627)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('Duplicate appointment! Please contact Administrator.');", true);
                }
            }
           
        }
        Listbox1.Visible = false;
        available_list_title.Visible = false;
        bookappointment.Visible = false;

                
    }
}