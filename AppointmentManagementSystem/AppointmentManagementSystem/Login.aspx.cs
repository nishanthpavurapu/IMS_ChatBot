using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void buttonSignup_Click(object sender, EventArgs e)
    {
        Response.Redirect("Signup.aspx");
    }

    protected void buttonLogin_Click(object sender, EventArgs e)
    {
        String cs = ConfigurationManager.ConnectionStrings["amsdbConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd_username = new SqlCommand("select amsid,isadmin,Email from Users where amsid='" + txtUsernameLogin.Text + "' and password='"+txtPasswordLogin.Text+"'",con);
            con.Open();
            SqlDataReader reader = cmd_username.ExecuteReader();
            if (reader.Read())
            {
                Session["amscurrentuser"] = txtUsernameLogin.Text;
                Session["isuseradmin"] = reader["isadmin"];
                Session["primaryuseremail"] = reader["Email"];
                Response.Redirect("Default.aspx");
            }
            else
            {
                // Closing previously opened reader
                reader.Close();

                SqlCommand cmd_phone = new SqlCommand("select amsid,isadmin,Email from Users where Email='" + txtUsernameLogin.Text + "' and password='" + txtPasswordLogin.Text + "'", con);
                reader = cmd_phone.ExecuteReader();
                if (reader.Read())
                {
                    Session["amscurrentuser"] = reader["amsid"];
                    Session["isuseradmin"] = reader["isadmin"];
                    Session["primaryuseremail"] = reader["Email"];
                    Response.Redirect("Default.aspx");

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Application Message", "alert('Invalid Credentials!');", true);
                    txtPasswordLogin.Text = "";
                    txtUsernameLogin.Text = "";
                }
            }


            
        }
    }
}