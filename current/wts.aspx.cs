using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Cfg;

public partial class experimental_WTS : System.Web.UI.Page
{

    databaseLogic dbLogic = new databaseLogic();
    DataSet ds = new DataSet();
    
    string positionTitle;
    
    string query = "";
    string id;
    string[] info;
    DatabaseEntities.User userObject;
    protected void Page_Load(object sender, EventArgs e)
    {

        string position = Request.QueryString["position"];
        int positionID;

        if (position != null && int.TryParse(position, out positionID))
        {
            positionTitle = dbLogic.selectPositionFromID(positionID.ToString());
            
            LabelHeader.Text = positionTitle;
            HiddenFieldPosition.Value = positionTitle;

            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            userObject = DatabaseEntities.User.FindUser(session, Page.User.Identity.Name.ToString());
            id = userObject.ID.ToString();
            
            if(dbLogic.isUserWTS(int.Parse(id), HiddenFieldPosition.Value)) {
                Fieldset2.Visible = false;
                Confirm.Visible = true;
                return;
            }

            //ds = dbLogic.getResults();
            info = new string[3] { id, id, HiddenFieldPosition.Value };

            if (!String.IsNullOrEmpty(positionTitle))
                Submit.Enabled = true;
        } else
            throw new HttpException(400, "Invalid position ID");
    }

    protected void submit(object sender, EventArgs e)
    {
        AcceptError.Visible = false;
        wtsPanelLength.Visible = false;
        
        if (!Accept.Checked) {
            AcceptError.Visible = true;
            return;
        }
            
        if(Statement.Text.Length > 10000) {
            wtsPanelLength.Visible = true;
            return;
        }
        
        //Handle legacy WTS
        if (!dbLogic.isUserWTS(int.Parse(id), HiddenFieldPosition.Value))
        {
            dbLogic.insertIntoWTS(id, Statement.Text, HiddenFieldPosition.Value);
            if (!dbLogic.isUserNominated(int.Parse(id), HiddenFieldPosition.Value))
                dbLogic.insertNominationAccept(info);
            
            dbLogic.userAcceptedNom(id, HiddenFieldPosition.Value);
            AcceptError.Visible = false;
        }
        
        Fieldset2.Visible = false;
        Confirm.Visible = true;
    }
}