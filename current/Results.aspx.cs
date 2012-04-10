using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

public partial class finalsite_results : System.Web.UI.Page
{
    databaseLogic dbLogic = new databaseLogic();
    voteCounter vc = new voteCounter();
    
    DataSet ds = new DataSet();
    /*string query;
    ArrayList positions = new ArrayList();*/

    protected void Page_Load(object sender, EventArgs e)
    {
        bindPositions();

        if(User.IsInRole("nec") && !dbLogic.checkNecApprove())
        {
            necApprove.Visible = true;
            necButton.Visible = true;
        }
    }

    
    protected void bindPositions()
    {
        /*
        query = "SELECT * FROM election_position;";
        dbLogic.genericQuerySelector(query);
        ds = dbLogic.getResults();
        int i = 0;
        while (i < ds.Tables[0].Rows.Count)
        {
            positions.Add(ds.Tables[0].Rows[i].ItemArray[2]);
            i++;
        }

        for (i = 0; i < positions.Count; i++ )
        {
            query = "SELECT * FROM tally WHERE position='" + positions[i] + "' ORDER BY count DESC;";
            dbLogic.genericQuerySelector(query);
            ds = dbLogic.getResults();

            query = "INSERT INTO results (position, id_union) VALUES ('" + positions[i] + "', " + ds.Tables[0].Rows[0].ItemArray[0] + ");";
            dbLogic.genericQueryInserter(query);
        }
     */ 
        vc.tally();
        dbLogic.getPosAndWinner();
        ds = dbLogic.getResults();
        resultList.DataSource = ds;
        resultList.DataBind();
    }
     

    protected void necButton_OnClick(Object sender, EventArgs e)
    {
        dbLogic.insertNecApprove();
        necApproved.Visible = true;
        necApprove.Visible = false;
        necButton.Visible = false;
    }

}