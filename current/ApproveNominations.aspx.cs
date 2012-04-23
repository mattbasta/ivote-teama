using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using DatabaseEntities;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;

public partial class wwwroot_experimental_ApproveNominations : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dbLogic.selectInfoForApprovalTable();
            DataSet emailSet = dbLogic.getResults();
            ListViewApproval.DataSource = emailSet;
            ListViewApproval.DataBind();
            foreach (ListViewDataItem eachItem in ListViewApproval.Items)
            {
                HiddenField eligible = (HiddenField)eachItem.FindControl("HiddenFieldEligible");
                RadioButton approve = (RadioButton)eachItem.FindControl("RadioButton1");
                RadioButton deny = (RadioButton)eachItem.FindControl("RadioButton2");
                if (eligible.Value == "1")
                    approve.Checked = true;
                else if(eligible.Value == "0")
                    deny.Checked = true;
            }
        }
    }
    
    protected string GetName(int UserID) {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        User u = DatabaseEntities.User.FindUser(session, UserID);
        return u.FirstName + " " + u.LastName;
    }
    
    protected string GetSummary(string statement) {
        if(statement.Length == 0)
            return "(no statement)";
        if(statement.Length > 140)
            return statement.Substring(0, 137) + "...";
        return statement;
    }
    
    //saves user changes
    protected void Click_ButtonSave(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        int count = 0;
        foreach (ListViewDataItem eachItem in ListViewApproval.Items)
        {
            bool changeMade = false;
            HiddenField id = (HiddenField)eachItem.FindControl("HiddenFieldID");
            HiddenField userid = (HiddenField)eachItem.FindControl("HiddenUserID");
            HiddenField eligible = (HiddenField)eachItem.FindControl("HiddenFieldEligible");
            
            RadioButton approve = (RadioButton)eachItem.FindControl("RadioButton1");
            RadioButton deny = (RadioButton)eachItem.FindControl("RadioButton2");

            User u = DatabaseEntities.User.FindUser(session, int.Parse(userid.Value));

            nEmailHandler emailer = new nEmailHandler();

            if (approve.Checked && u != null)
            {
                dbLogic.updateEligible(id.Value, "1");
                //email to user saying they were accepted
                emailer.sendApproveEligibility(u);

                if (eligible.Value != "1")
                {
                    count += 1;
                    eligible.Value = "1";
                }
            }
            else if (deny.Checked || u == null)
            {
                dbLogic.updateEligible(id.Value, "0");
                //email to user saying they were rejected
                if(u != null)
                    emailer.sendDenyEligibility(u);

                if (eligible.Value != "0")
                {
                    count += 1;
                    eligible.Value = "0";
                }
            }
            
        }
        if (count > 0) {
            SavedConfirmation.Visible = true;
            LabelFeedbackAlert.Visible = false;
        } else {
            SavedConfirmation.Visible = false;
            LabelFeedbackAlert.Visible = true;
            LabelFeedback.Text = "No changes made.";
            LabelFeedbackAlert.CssClass = "alert";
        }
    }

    //hides popup
    protected void ButtonDone_Click(object sender, EventArgs e)
    {
        ModalPopupExtender1.Hide();
    }

    //shows full statement
    protected void showFullStatement(object sender, ListViewCommandEventArgs e)
    {
        if (String.Equals(e.CommandName, "statement"))
        {
            LabelFullStatment.Text = "\"" + e.CommandArgument.ToString() + "\"";
            ModalPopupExtender1.Show();
            PanelStatement.Visible = true;
        }
    }

}