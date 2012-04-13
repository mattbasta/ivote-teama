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
            loadApprovalInfo();
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

    protected void loadApprovalInfo()
    {
        foreach (ListViewDataItem eachItem in ListViewApproval.Items)
        {
            HiddenField eligible = (HiddenField)eachItem.FindControl("HiddenFieldEligible");
            RadioButton approve = (RadioButton)eachItem.FindControl("RadioButton1");
            RadioButton deny = (RadioButton)eachItem.FindControl("RadioButton2");
            if (eligible.Value == "1")
            {
                approve.Checked = true;
            }
            else if(eligible.Value == "0")
                deny.Checked = true;
        }
    }
    
    

    //saves user changes
    protected void Click_ButtonSave(object sender, EventArgs e)
    {
        int count = 0;
        foreach (ListViewDataItem eachItem in ListViewApproval.Items)
        {
            bool changeMade = false;
            HiddenField id = (HiddenField)eachItem.FindControl("HiddenFieldID");
            HiddenField eligible = (HiddenField)eachItem.FindControl("HiddenFieldEligible");
            Label fullname = (Label)eachItem.FindControl("LabelFullname");
            Label position = (Label)eachItem.FindControl("LabelPosition");
            Label statement = (Label)eachItem.FindControl("LabelStatement");
            RadioButton approve = (RadioButton)eachItem.FindControl("RadioButton1");
            RadioButton deny = (RadioButton)eachItem.FindControl("RadioButton2");

            ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
            User u = DatabaseEntities.User.FindUser(session, int.Parse(id.Value));

            nEmailHandler emailer = new nEmailHandler();

            if (approve.Checked == true && u != null)
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
            else if (deny.Checked == true || u == null)
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