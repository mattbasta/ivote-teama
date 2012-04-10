using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class wwwroot_finalsite_ResultDetail : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {   //gets the position data from the query string
            string fullPath = Request.PathInfo;
            if (fullPath != "")
            {
                string[] categoryList;
                categoryList = fullPath.Substring(1).TrimEnd('/').Split('/');
            
                //get/bind data to gridview
                dbLogic.selectTallyInfoForPosition(dbLogic.selectPositionFromID(categoryList[0]));
                DataSet ds = dbLogic.getResults();
                GridViewData.DataSource = ds;
                GridViewData.DataBind();

                LabelPosition.Text = "Total vote count for each cadidate for <u>" + dbLogic.selectPositionFromID(categoryList[0]) + "</u>";
            }
        }
    }
}