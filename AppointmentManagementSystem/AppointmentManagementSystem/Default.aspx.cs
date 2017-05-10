using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;

public partial class _Default : System.Web.UI.Page
{

    string secondaryamsid, dateof_appointment,primaryAttendee;
    string available_time_start, available_time_end, nonavailable_days, day_of_appointment;
    string appointmentTime;
    string primaryUserEmail, secondaryUserEmail;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["amscurrentuser"] == null || Session["amscurrentuser"].Equals(""))
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            listbox_div.Visible = false;
            if (Session["isuseradmin"].Equals("yes"))
            {
                secondaryamsid = secondary_userid.Text;
                dateof_appointment = appointmentDate.Text;
                primaryAttendee = userid.Text;
            }
            else
            { 
                secondaryamsid = userid.Text;
                dateof_appointment = appointmentDate.Text;
                primaryUserEmail = Session["primaryuseremail"].ToString();
                primaryAttendee = Session["amscurrentuser"].ToString();
            }
        }

        if (Session["isuseradmin"].Equals("yes"))
        {
            secondary_user_div.Visible = true;
        }
        else
        {
            secondary_user_div.Visible = false;
        }

    }

    protected void get_availableslots(string startTime,string EndTime)
    {
        string[] slots_array= new string[48];
       // List<String> dropdown_list = new List<String>();
        string booked_slot_db;
        int i = 0,count=0;
        string hr=startTime;

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
                listbox_div.Visible = true;
                bookappointment.Visible = true;
                agenda_div.Visible = true;
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

        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
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
        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
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
        
        string date_Pattern = "MM/dd/yyyy";

        if (Session["isuseradmin"].Equals("yes"))
        {
            primaryAttendee = userid.Text;
            secondaryamsid = secondary_userid.Text;
        }

        DateTime dateParsed;
        DateTime.TryParseExact(dateof_appointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
        day_of_appointment = dateParsed.ToString("ddd");

        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            SqlCommand cmd_available_timings = new SqlCommand("select availabletimestart,availabletimeend,nonavailabledays from profile where amsid='" + secondaryamsid + "'", con);
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
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('User is not available today');", true);
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
            reader.Close();
            con.Close();
        }
    }

    protected void bookappointment_Click(object sender, EventArgs e)
    {

        string appointmentTimeText="";

        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            foreach (ListItem item in Listbox1.Items)
            {
                if (item.Selected)
                {
                    appointmentTime = item.Value;
                    appointmentTimeText = appointmentTimeText + ", " + appointmentTime;

                    SqlCommand cmd = new SqlCommand("insert into Appointments values('" + primaryAttendee + "','" + dateof_appointment + "','" + secondaryamsid + "','" + appointmentTime + "','" + appointmentAgenda.Text + "')", con);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.ErrorCode == 2627)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('Duplicate appointment! Please contact Administrator.');", true);
                        }
                    }
                }
            }
            con.Close();
            
            //Sending Confirmation Email
            SendAmsEmail obj = new SendAmsEmail(primaryAttendee, secondaryamsid, dateof_appointment + ", " + appointmentTimeText);
            obj.sendCreateConfirmation();
            ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('Appointment has been confirmed!');", true);

        }
        listbox_div.Visible = false;
        bookappointment.Visible = false;
        agenda_div.Visible = false;        
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> SearchUsers(string prefixText, int count)
    {
        string cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
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
}