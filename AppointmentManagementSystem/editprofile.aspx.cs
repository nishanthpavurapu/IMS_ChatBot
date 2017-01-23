using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

public partial class editprofile : System.Web.UI.Page
{
    string primaryAttendee, available_text_ret,nonavailabledays = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["amscurrentuser"] == null || Session["amscurrentuser"].Equals(""))
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            primaryAttendee = Session["amscurrentuser"].ToString();
            txtusername.ReadOnly = true;
            if (!IsPostBack) {             
            getProfile();            
            }
        }
    }

    protected void buttonUpdateProfile_Click(object sender, EventArgs e)
    {
        if (txtconfirmpassword.Text == "" || txtpassword.Text == "")
        {
         //   "update Appointments set slot='" + appointment_edit_list.SelectedItem.Text + "' where primary_amsid='" + primaryAttendee + "' and appointmentdate='" + dateofappointmenttxt.Text + "' and slot='" + oldslot.Text + "'";
           SqlDataSourceUsers.UpdateCommand = "update Users set Name='"+txtfullname.Text+"',Email='"+txtemail.Text+"',Phone='"+txtphonenumber.Text+"' where amsid='"+primaryAttendee+"'";
        }
        else
        {
            SqlDataSourceUsers.UpdateCommand = "update Users set Name='" + txtfullname.Text + "',Email='" + txtemail.Text + "',Phone='" + txtphonenumber.Text + "',Password='" + txtpassword.Text + "' where amsid='" + primaryAttendee + "'";
        }

        //Updating the users table with the modifies values
        SqlDataSourceUsers.Update();

        //Reading the non available check box values

        if (!mondayCheckbox.Checked)
        {
            nonavailabledays = mondayCheckbox.Text + ",";
        }
        if (!tuesdayCheckbox.Checked)
        {
            nonavailabledays = nonavailabledays + tuesdayCheckbox.Text + ",";
        }
        if (!wednesdayCheckbox.Checked)
        {
            nonavailabledays = nonavailabledays + wednesdayCheckbox.Text + ",";
        }
        if (!thursdayCheckbox.Checked)
        {
            nonavailabledays = nonavailabledays + thursdayCheckbox.Text + ",";
        }
        if (!fridayCheckbox.Checked)
        {
            nonavailabledays = nonavailabledays + fridayCheckbox.Text + ",";
        }
        if (!saturdayCheckbox.Checked)
        {
            nonavailabledays = nonavailabledays + saturdayCheckbox.Text + ",";
        }
        if (!sundayCheckbox.Checked)
        {
            nonavailabledays = nonavailabledays + sundayCheckbox.Text;
        }

        //Updating the profile table with the modifies values

        SqlDataSourceProfile.UpdateCommand = "update profile set nonavailabledays='"+nonavailabledays+"' where amsid='"+primaryAttendee+"'";
        SqlDataSourceProfile.Update();

        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Profile has been updated successfully !');window.location='Default.aspx';</script>'");

    }

    protected void getProfile()
    {
        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_get_profile_users = new SqlCommand("select * from Users where amsid='" + primaryAttendee + "'", con);
            SqlCommand cmd_get_profile_profile = new SqlCommand("select * from profile where amsid='" + primaryAttendee + "'", con);

            con.Open();

            SqlDataReader reader_primary = cmd_get_profile_users.ExecuteReader();

            if (reader_primary.HasRows)
            {
                while (reader_primary.Read())
                {
                    txtusername.Text = reader_primary.GetString(0);
                    txtfullname.Text = reader_primary.GetString(1);
                    txtemail.Text = reader_primary.GetString(2);
                    txtphonenumber.Text = reader_primary.GetString(4);                 
                }
            }
            reader_primary.Close();
         

            reader_primary = cmd_get_profile_profile.ExecuteReader();

            if (reader_primary.HasRows)
            {
                while (reader_primary.Read())
                {
                    available_text_ret = reader_primary.GetString(3);
                }
            }

            string[] available_array = new string[7];
            available_array = available_text_ret.Split(',');
            foreach (string item in available_array)
            {
                if (item == "Sat")
                {
                    saturdayCheckbox.Checked = false;
                }
                if (item == "Sun")
                {
                    sundayCheckbox.Checked = false;
                }
                if (item == "Mon")
                {
                    mondayCheckbox.Checked = false;
                }
                if (item == "Tue")
                {
                    tuesdayCheckbox.Checked = false;
                }
                if (item == "Wed")
                {
                    wednesdayCheckbox.Checked = false;
                }
                if (item == "Thu")
                {
                    thursdayCheckbox.Checked = false;
                }
                if (item == "Fri")
                {
                    fridayCheckbox.Checked = false;
                }

            }
            reader_primary.Close();
            con.Close();

        }
    }

    protected void buttonCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}