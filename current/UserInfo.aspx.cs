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

public partial class wwwroot_phase1aSite_UserInfo : System.Web.UI.Page
{
    //global variable "only" used for changing users roles
    int id = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request.QueryString["id"] == null)
            throw new HttpException(400, "Missing user ID.");

        try {
            id = Convert.ToInt32(Request.QueryString["id"]);
        } catch {
            throw new HttpException(400, "Invaild user ID.");
        }

        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();

        if(!Page.IsPostBack) {
            //Populate dropdown menu from DepartmentType enum.
            foreach (DatabaseEntities.DepartmentType dept in Enum.GetValues(typeof(DatabaseEntities.DepartmentType)))
                DeptDropDown.Items.Add(dept.ToString());

            foreach(DatabaseEntities.Committee c in session.CreateCriteria(typeof(DatabaseEntities.Committee)).List())
                CurrentCommittee.Items.Add(new ListItem(c.Name, c.ID.ToString()));
        }

        DatabaseEntities.User user = DatabaseEntities.User.FindUser(session, id);
        if(user == null)
            throw new HttpException(404, "User '" + id.ToString() + "' not found.");

        if (!Page.IsPostBack)
        {
            SetupUser(user);
            ButtonDelete.OnClientClick = "javascript:return confirm('Are you sure what want to PERMANENTLY delete this user account?')";
        }

        if(User.Identity.Name == user.Email)
        {
            ButtonDelete.Enabled = false;
            ButtonDelete.CssClass = "btn btn-danger disabled pull-right";

            CanVote.Enabled = false;
        }
    }

    protected void SetupUser(DatabaseEntities.User user)
    {
        Page.Title = user.FirstName + " " + user.LastName;

        //name, first and last
        Email.Text = user.Email;
        FirstName.Text = user.FirstName;
        LastName.Text = user.LastName;
        DeptDropDown.SelectedItem.Text = user.Department.ToString();
        CurrentCommittee.SelectedValue = user.CurrentCommittee.ToString();
        LabelFullname.Text = "Edit " + user.FirstName + " " + user.LastName;

        IsAdmin.Checked = user.IsAdmin;
        IsNEC.Checked = user.IsNEC;
        IsFaculty.Checked = user.IsFaculty;
        IsTenured.Checked = user.IsTenured;
        IsUnion.Checked = user.IsUnion;
        IsBU.Checked = user.IsBargainingUnit;

        CanVote.Checked = user.CanVote;

    }

    //ADD CHECK AGAINST EMAIL IN THE DATABASE
    protected void ButtonSave_Clicked(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        DatabaseEntities.User user = DatabaseEntities.User.FindUser(session, id);

        user.FirstName = FirstName.Text;
        user.LastName = LastName.Text;
        user.Email = Email.Text;
        if (!user.AddToCommittee(session,
            Committee.FindCommittee(session,
                Convert.ToInt32(CurrentCommittee.SelectedValue)).Name))
        {
            FailurePanel.Visible = true;
            return;
        }
        else
            FailurePanel.Visible = false;
        user.Department = (DatabaseEntities.DepartmentType)Enum.Parse(typeof(DatabaseEntities.DepartmentType), DeptDropDown.SelectedValue);

        user.IsAdmin = IsAdmin.Checked;
        user.IsNEC = IsNEC.Checked;
        user.IsFaculty = IsFaculty.Checked;
        user.IsTenured = IsTenured.Checked;
        user.IsUnion = IsUnion.Checked;
        user.IsBargainingUnit = IsBU.Checked;

        user.CanVote = CanVote.Checked;

        session.SaveOrUpdate(user);

        SuccessPanel.Visible = true;

        DatabaseEntities.NHibernateHelper.Finished(transaction);

    }

    protected void ButtonDelete_Clicked(object sender, EventArgs e)
    {
        ISession session = DatabaseEntities.NHibernateHelper.CreateSessionFactory().OpenSession();
        ITransaction transaction = session.BeginTransaction();

        session.Delete(DatabaseEntities.User.FindUser(session, id));

        DatabaseEntities.NHibernateHelper.Finished(transaction);
        Response.Redirect("users.aspx");
    }
}
