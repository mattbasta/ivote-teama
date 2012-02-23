using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseEntities;

public partial class tester : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DatabaseEntities.NHibernateHelper.CreateSessionFactoryAndGenerateSchema();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        emailer emailTest = new emailer();
        String[] addresses = new String[1];
        addresses[0] = email.Text;
        emailTest.sendEmailToList(addresses, "This is a test.", "Test Message");
    }
}