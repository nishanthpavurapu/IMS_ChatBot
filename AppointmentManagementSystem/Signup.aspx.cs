using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Signup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        

    }

    protected void buttonSignup_Click(object sender, EventArgs e)
    {
        String[] workinghours_val = new String[2];
        String workinghours_start, workinghours_end, nonavailabledays="";

        workinghours_val = workinghours.Text.Split(',');
        workinghours_start = workinghours_val[0];
        workinghours_end = workinghours_val[1];

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

        
        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
           SqlCommand cmd = new SqlCommand("insert into Users values('" + txtusername.Text + "','" + txtfullname.Text + "','" + txtemail.Text + "','" + txtpassword.Text + "','" + txtphonenumber.Text + "')",con);
           con.Open();
           cmd.ExecuteNonQuery();
           con.Close();
        
           SqlCommand cmd_profile = new SqlCommand("insert into profile values('" + txtusername.Text + "','" + workinghours_start+ "','" + workinghours_end + "','" + nonavailabledays + "')", con);
            con.Open();
            cmd_profile.ExecuteNonQuery();
           Response.Redirect("Login.aspx");
        }

    }
}