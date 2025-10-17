using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Web.Configuration;
using P12RTD_DLL;



public partial class SmartSampling_PM : Page
{
    DataSet DS = new DataSet();
    string vFunctionName = "SMART_SAMPLING_EQP_RECOVER";
    string vTableName = "RTD.SMART_SAMPLING_EQP_RECOVER";
    string vTableName_case = "RTD.SMART_SAMPLING_TRB_CASE";
    string vTableName_EQudata = "EQDM.EQ_UDATA";
    string vTableName_recipe = "RTD.OPE_NO_LRECIPE_EQP";
    string vUsername, vPassword, vCheckResult;


    Table_Info_control RTD_TABLE_INFO = new Table_Info_control();
    Access_control RTD_ACCESS_CONTROL = new Access_control();
    OraDB_SQL RTD_OraDB = new OraDB_SQL();

    OleDbConnection Conn = new OleDbConnection(WebConfigurationManager.ConnectionStrings["Readuser_RTD"].ConnectionString);
    OleDbDataAdapter Adpt_temp;
    
    bool user_check_flag;
    string error_string = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        GridView3.BorderColor = ctlColorSet.DG_Border;
        GridView3.AlternatingRowStyle.BackColor = ctlColorSet.DG_BC;
        GridView3.RowStyle.BackColor = ctlColorSet.DG_AB;
        GridView3.HeaderStyle.BackColor = ctlColorSet.DG_HS;
        GridView3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        GridView3.HeaderStyle.VerticalAlign = VerticalAlign.Middle;
        GridView3.HeaderStyle.Font.Bold = true;

        if (!Page.IsPostBack)
        {
            dd_function.SelectedValue = "請選擇";
            //dd_prod_eqp();
            myDBInit();

        }

        DataSet ds_temp = new DataSet();
        ds_temp = RTD_TABLE_INFO.Find_Data(vFunctionName);
        if (ds_temp.Tables[vFunctionName].Rows.Count == 0)
        {
            last_updateTime.Text = "最近一次資料更新時間 : ";
        }
        else
        {
            last_updateTime.Text = "最近一次資料更新時間 : " + ds_temp.Tables[vFunctionName].Rows[0]["Last_Update_time"].ToString();
        }   

    }
    void dd_prod_eqp()
    {
        string SQL_prod_eqp = "select '請選擇' as eqp_id,0 as seq from dual "
                    + "union all SELECT DISTINCT substr(EQ_ID,1,2) as eqp_id,1 as seq FROM EQDM.EQ_UDATA WHERE (EQP_FLAG = 1) and substr(EQ_ID,1,1)='P' "
                    + "order by seq,eqp_id";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_prod_eqp, "DDL_prod_eqp");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        dd_eqp_pre2.DataSource = DS.Tables["DDL_prod_eqp"];
        dd_eqp_pre2.DataTextField = "EQP_ID";
        dd_eqp_pre2.DataValueField = "EQP_ID";
        dd_eqp_pre2.DataBind();
        DS.Clear();

    }
    void Bind_dd_event()
    {
        string vSQL_event ="";
        vSQL_event = " select event_type from (SELECT '請選擇' as EVENT_TYPE FROM DUAL UNION ALL SELECT EVENT_TYPE FROM " + vTableName_case + ") ";
        vSQL_event +="  order by decode( event_type,'請選擇',1,2),event_type ";

        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(vSQL_event, "DDL_event");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_event 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        dd_event.DataSource = DS.Tables["DDL_event"];
        dd_event.DataTextField = "event_type";
        dd_event.DataValueField = "event_type";
        dd_event.DataBind();
        DS.Clear();

    }
    void Bind_dd_eqpg()
    {
        string vSQL_eqpg = "";
        vSQL_eqpg = " SELECT '請選擇' AS EQ_GROUP_NAME, 0 AS SEQ FROM DUAL UNION ALL SELECT DISTINCT EQ_GROUP_NAME, 1 AS SEQ FROM " + vTableName_EQudata + " WHERE EQP_FLAG = 1 AND EQ_ID LIKE 'P%' AND EQ_GROUP_NAME <> 'Mask Stocker' ORDER BY SEQ, EQ_GROUP_NAME";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(vSQL_eqpg, "DDL_eqpg");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_eqpg 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        dd_eqpg.DataSource = DS.Tables["DDL_eqpg"];
        dd_eqpg.DataTextField = "EQ_GROUP_NAME";
        dd_eqpg.DataValueField = "EQ_GROUP_NAME";
        dd_eqpg.DataBind();
        DS.Clear();

    }
    void Bind_dd_eqp_pre2()
    {
        string SQL_prod_eqp = "select '請選擇' as eqp_id,0 as seq from dual "
                   + "union all SELECT DISTINCT substr(EQ_ID,1,2) as eqp_id,1 as seq FROM " + vTableName_EQudata + " WHERE (EQP_FLAG = 1) and substr(EQ_ID,1,1)='P' "
                   + "order by seq,eqp_id";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_prod_eqp, "DDL_prod_eqp");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        dd_eqp_pre2.DataSource = DS.Tables["DDL_prod_eqp"];
        dd_eqp_pre2.DataTextField = "EQP_ID";
        dd_eqp_pre2.DataValueField = "EQP_ID";
        dd_eqp_pre2.DataBind();
        dd_prodg_eqp_pre2.DataSource = DS.Tables["DDL_prod_eqp"];
        dd_prodg_eqp_pre2.DataTextField = "EQP_ID";
        dd_prodg_eqp_pre2.DataValueField = "EQP_ID";
        dd_prodg_eqp_pre2.DataBind();
        DS.Clear();
    }
    void Bind_chklst_prod_eqp()
    {
        string SQL_prod_eqp = " select * "
                            + " from (select DISTINCT EQ_ID as eqp_id "
                            + "       from " + vTableName_EQudata
                            + "       where substr(EQ_ID,1,2)='" + dd_eqp_pre2.SelectedValue + "' and (EQP_FLAG = 1) and substr(EQ_ID,1,1)='P') "
                            + " order by eqp_id ";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_prod_eqp, "DDL_chklst_prod_eqp");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_chklst_prod_eqp 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        chklst_eqp.DataSource = DS.Tables["DDL_chklst_prod_eqp"];
        chklst_eqp.DataTextField = "EQP_ID";
        chklst_eqp.DataValueField = "EQP_ID";
        chklst_eqp.DataBind();
        DS.Clear();
    }
    void Bind_chklst_prodg_eqp()
    {
        string SQL_prodg_eqp = " select * "
                            + " from (select DISTINCT EQ_ID as eqp_id "
                            + "       from " + vTableName_EQudata
                            + "       where substr(EQ_ID,1,2)='" + dd_prodg_eqp_pre2.SelectedValue + "' and (EQP_FLAG = 1) and substr(EQ_ID,1,1)='P') "
                            + " order by eqp_id ";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_prodg_eqp, "DDL_chklst_prodg_eqp");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_chklst_prodg_eqp 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        chklst_prodg_eqp.DataSource = DS.Tables["DDL_chklst_prodg_eqp"];
        chklst_prodg_eqp.DataTextField = "EQP_ID";
        chklst_prodg_eqp.DataValueField = "EQP_ID";
        chklst_prodg_eqp.DataBind();
        DS.Clear();
    }
    protected void dd_function_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dd_function.SelectedValue.ToString() == "By Case") 
        {   pnl_case.Visible = true; pnl_recipe.Visible = false; pnl_prod.Visible = false; pnl_prodg.Visible=false;
            Bind_dd_event();
            
        }
        if (dd_function.SelectedValue.ToString() == "By Recipe")
        {   pnl_case.Visible = false; pnl_recipe.Visible = true; pnl_prod.Visible = false; pnl_prodg.Visible=false;
            Bind_dd_eqpg();
        }
        if (dd_function.SelectedValue.ToString() == "By Prod")
        {   pnl_case.Visible = false; pnl_recipe.Visible = false; pnl_prod.Visible = true; pnl_prodg.Visible=false;
            Bind_dd_eqp_pre2();
        }
        if (dd_function.SelectedValue.ToString() == "By Prodg")
        {   pnl_case.Visible = false; pnl_recipe.Visible = false; pnl_prod.Visible = false; pnl_prodg.Visible=true;
            Bind_dd_eqp_pre2();
        }
        if (dd_function.SelectedValue.ToString() == "請選擇") { pnl_case.Visible = false; pnl_recipe.Visible = false; pnl_prod.Visible = false;  pnl_prodg.Visible=false;}
    }
    protected void dd_event_SelectedIndexChanged(object sender, EventArgs e)
    {
        string SQL_event="";

        SQL_event = " select event_type,basic_volume,future_hold,pm_duration,force_type,force_type_code,memo as subject ";
        SQL_event += " from " + vTableName_case + " where event_type='" + dd_event.SelectedValue + "' ";

        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_event, "DDL_event_type");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_event_type SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        GridView1.DataSource = DS.Tables["DDL_event_type"];
        GridView1.DataBind();

        GridView2.Visible = true;

        string SQL_EQPG = "SELECT DISTINCT EQ_GROUP_NAME,'' AS EQP_LIST FROM " + vTableName_EQudata + " WHERE EQP_FLAG = 1 and eq_id like 'P%' and EQ_GROUP_NAME<>'Mask Stocker' ";
        
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_EQPG, "event_list");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : event_list SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        GridView2.DataSource = DS.Tables["event_list"];
        GridView2.DataBind();
        DS.Tables["event_list"].Clear();

        foreach (GridViewRow row in GridView2.Rows)
        {

            CheckBox chk_eqpg = (CheckBox)row.FindControl("chk_eqpg");
            //Response.Write(chk_eqpg.Text);
            string SQL_EQP = "SELECT DISTINCT EQ_ID FROM " + vTableName_EQudata + " WHERE (EQP_FLAG = 1) AND EQ_GROUP_NAME='" + chk_eqpg.Text + "' ORDER BY EQ_ID";
            try
            {
                DS = RTD_OraDB.Exec_SelectSQL(SQL_EQP, "event_eqp");
            }
            catch (OleDbException exSQL)
            {
                ErrorLabel_a.Text = "SQL Error : event_eqp SQL 有問題,請Call工程師處理 <BR>";
                return;
            }
            catch (Exception ex)
            {
                ErrorLabel_a.Text = "Other Error : " + ex.ToString();
                return;
            }

            CheckBoxList cbl_eqp = (CheckBoxList)row.FindControl("cbl_eqp");

            cbl_eqp.DataSource = DS.Tables["event_eqp"];
            cbl_eqp.DataTextField = "EQ_ID";
            cbl_eqp.DataBind();
            DS.Tables["event_eqp"].Clear();
        }
        
    }
    protected void dd_eqpg_SelectedIndexChanged(object sender, EventArgs e)
    {
        string vSQL_eqplist = "select '請選擇' as eq_id,0 as seq from dual union all SELECT DISTINCT EQ_ID,1 as seq FROM " + vTableName_EQudata + " WHERE (EQP_FLAG = 1) AND EQ_GROUP_NAME='" + dd_eqpg.SelectedValue.ToString() + "' ORDER BY seq,EQ_ID";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(vSQL_eqplist, "DDL_eqplist");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_eqplist 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        dd_eqpid.DataSource = DS.Tables["DDL_eqplist"];
        dd_eqpid.DataTextField = "EQ_ID";
        //dd_eqpid.DataValueField = "EQ_ID";
        dd_eqpid.DataBind();
        DS.Clear();

    }
    protected void dd_eqpid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string vSQL_recipe = "select '請選擇' as recipe,0 as seq from dual union all select distinct substr(m_recipe,8,7) as recipe,1 as seq  from " + vTableName_recipe + " where eqp_id='" + dd_eqpid.SelectedValue.ToString() + "' order by seq,recipe";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(vSQL_recipe, "DDL_recipe");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_recipe 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        dd_recipe_pre6.DataSource = DS.Tables["DDL_recipe"];
        dd_recipe_pre6.DataTextField = "recipe";
        dd_recipe_pre6.DataBind();
        DS.Clear();
    }
    protected void dd_recipe_pre6_SelectedIndexChanged(object sender, EventArgs e)
    {
        string SQL_RECIPE = " select distinct substr(m_recipe,8,8) as recipe from " + vTableName_recipe +" where eqp_id='" + dd_eqpid.SelectedValue.ToString() + "' and substr(m_recipe,8,7)='" + dd_recipe_pre6.SelectedValue.ToString() + "' order by recipe";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_RECIPE, "DDL_recipelist");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_recipelist 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        chklst_recipe.DataSource = DS.Tables["DDL_recipelist"];
        chklst_recipe.DataTextField = "recipe";
        chklst_recipe.DataBind();
        DS.Clear();
    }
    protected void dd_eqp_pre2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_chklst_prod_eqp();

        string sql_prod_ctg = " SELECT '請選擇' as CT_G, 0 as seq FROM dual "
                    + " UNION SELECT DISTINCT CT_G,1 as seq FROM PWEB.PRODG_MFGG "
                    + " order by seq,ct_g ";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(sql_prod_ctg, "DDL_ctg");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_ctg 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        dd_prod_ctg.DataSource = DS.Tables["DDL_ctg"];
        dd_prod_ctg.DataTextField = "CT_G";
        dd_prod_ctg.DataValueField = "CT_G";
        dd_prod_ctg.DataBind();
        DS.Clear();
    }
    protected void dd_prodg_eqp_pre2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind_chklst_prodg_eqp();

     
    }
    protected void dd_prod_ctg_SelectedIndexChanged(object sender, EventArgs e)
    {

        string sql_prod_prodg = " SELECT '請選擇' AS PRODG,0 AS SEQ FROM DUAL"
                              + " UNION SELECT DISTINCT A.PRODG1 AS \"PRODG\",1 AS SEQ "
                              + "       FROM PWEB.PRODUCT A , PWEB.PRODG_MFGG B"
                              + "       WHERE A.PRODG1=B.PROD_G AND B.CT_G='" + dd_prod_ctg.SelectedValue + "' "
                              + " ORDER BY SEQ,PRODG";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(sql_prod_prodg, "DDL_prodg");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_prodg 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        dd_prod_prodg.DataSource = DS.Tables["DDL_prodg"];
        dd_prod_prodg.DataTextField = "PRODG";
        dd_prod_prodg.DataValueField = "PRODG";
        dd_prod_prodg.DataBind();
        DS.Clear();
    }
    protected void dd_prod_prodg_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql_prod_prodspec = //" select 'ALL' prodspec_id,0 as seq from dual union all " +
                                   " select distinct a.prodspec_id,1 as seq " +
                                   " from RTD.V_PRODG_INFO_240219 a " +
                                   " where a.prodg1='" + dd_prod_prodg.SelectedValue + "' " +                                  
                                   " order by seq, prodspec_id";
        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(sql_prod_prodspec, "PRODSPEC");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_prodspec 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        chklst_prodspec.DataSource = DS.Tables["PRODSPEC"];
        chklst_prodspec.DataTextField = "PRODSPEC_ID";
        chklst_prodspec.DataValueField = "PRODSPEC_ID";
        chklst_prodspec.DataBind();

        string sql_prod_ope = " SELECT DISTINCT OPE_NO, 1 as seq FROM pweb.op_prodspec_inhibit WHERE substr(ope_no,5,1)='P'"
                            + " UNION select '請選擇' as ope_no,0 as seq from dual"
                            + " ORDER BY seq,OPE_NO";

        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(sql_prod_ope, "OPE");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : DDL_prodspec 初始SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }
        dd_prod_openo.DataSource = DS.Tables["OPE"];
        dd_prod_openo.DataTextField = "OPE_NO";
        dd_prod_openo.DataValueField = "OPE_NO";
        dd_prod_openo.DataBind();

        DS.Clear();
    }
    
    protected void Btn_add_Click(object sender, EventArgs e)
    {
        int eqp_cnt = 0;
        string sMemoText = "";
        foreach (GridViewRow row in GridView2.Rows)
        {   CheckBoxList cbl_eqp = (CheckBoxList)row.FindControl("cbl_eqp");
            foreach (ListItem oItem in cbl_eqp.Items)
            {   if (oItem.Selected == true)
                {
                    eqp_cnt = eqp_cnt + 1;
                }
            }
        }
        vUsername = txt_id.Text;
        vPassword = txt_password.Text;
        
        if (dd_event.SelectedValue == "請選擇" || eqp_cnt == 0)
        {
            Response.Write("<scr" + "ipt> alert('尚有必要輸入欄位未輸入!!(event,eqp)'); </scr" + "ipt>");
            return;
        }

         //進行MM帳密驗證
        if (MMuserid.Text == "")
        {
            vCheckResult = RTD_ACCESS_CONTROL.Check_Authority(vFunctionName, vUsername, vPassword);

            if (vCheckResult == "Authentication Fail")
            {
                lbl_nodata.Text = vCheckResult;
                return;
            }
            else
            {   //進行MM帳密驗證
                Response.Write("<script> window.open('../MM_verification.aspx?action=insert1', '', config='height=250,width=350',toolbar=0,resizable=1) <" + HtmlTextWriter.SlashChar + "script>");
            }
            
        }
        
        //驗證OK執行新增動作
        if (MMuserid.Text != "")
        {
            sMemoText = txt_memo3.Text.Replace(",", "").Replace("|", "").Replace("，", "");
            string EVENT_TYPE = GridView1.Rows[0].Cells[0].Text;
            string BASIC_VOLUME = GridView1.Rows[0].Cells[1].Text;
            string FUTURE_HOLD = GridView1.Rows[0].Cells[2].Text;
            string FORCE_TYPE = GridView1.Rows[0].Cells[4].Text;
            string FORCE_TYPE_CODE = GridView1.Rows[0].Cells[5].Text;
            string UPDATE_USER = MMuserid.Text;
            string UPDATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string sub_insert_sql = "";
            string insert_sql = "";
            
            foreach (GridViewRow row in GridView2.Rows)
            {
                CheckBoxList cbl_eqp = (CheckBoxList)row.FindControl("cbl_eqp");
                foreach (ListItem oItem in cbl_eqp.Items)
                {
                    if (oItem.Selected == true)
                    {
                        string EQP_ID = oItem.Text;
                        //string insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,ID) "
                          //        + " VALUES('" + EQP_ID + "', '" + EVENT_TYPE + "', " + BASIC_VOLUME + ", '" + FUTURE_HOLD + "', '" + FORCE_TYPE + "', " + FORCE_TYPE_CODE + ", '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "',dbms_random.string('A', 5)) ";
                        sub_insert_sql += "UNION ALL  SELECT '" + EQP_ID + "', '" + EVENT_TYPE + "', " + BASIC_VOLUME + ", '" + FUTURE_HOLD + "', '" + FORCE_TYPE + "', " + FORCE_TYPE_CODE + ", '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "',dbms_random.string('A', 5) FROM DUAL ";
                    }
                }
            }
            //lbl_nodata.Text = sub_insert_sql.Trim().Substring(9);   
            insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,ID) ";
            insert_sql += sub_insert_sql.Trim().Substring(9);
            
            try
            {
                RTD_OraDB.Exec_IUD(insert_sql);

            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    lbl_nodata.Text = ex.ToString();
                }
            }

            RTD_TABLE_INFO.Update_LastUpdateTime(vFunctionName, MMuserid.Text);
            GridView2.Visible = false;
            myDBInit();
            txt_memo3.Text = "";

            if (error_string != "")
            {
                Response.Write("<scr" + "ipt> alert('以下資料未新增;資料重覆:" + error_string + "'); </scr" + "ipt>");
                return;

            }
        }

    }
    protected void Btn_add_recipe_Click(object sender, EventArgs e)
    {
        string sMemoText = "";
        lbl_nodata.Text = "";
        vUsername = txt_id.Text;
        vPassword = txt_password.Text;
        
        if (dd_eqpg.SelectedValue == "請選擇" || dd_eqpid.SelectedValue == "請選擇" || dd_recipe_pre6.SelectedValue == "請選擇")
        {
            Response.Write("<scr" + "ipt> alert('尚有必要輸入欄位未輸入!!(eqpg,eqpid,recipe)'); </scr" + "ipt>");
            return;
        }
        else
        {
            //確認User有勾選Recipe 
            string selectrecipe = "";
            for (int i = 0; i < chklst_recipe.Items.Count; i++)
            {
                if (chklst_recipe.Items[i].Selected == true)
                {
                    selectrecipe = "1";
                    break;
                }
            }
            if (selectrecipe != "1")
            {
                Response.Write("<scr" + "ipt> alert('注意!!請勾選recipe'); </scr" + "ipt>");
                return;
            }

            //進行MM帳密驗證
            if (MMuserid.Text == "")
            {
                vCheckResult = RTD_ACCESS_CONTROL.Check_Authority(vFunctionName, vUsername, vPassword);

                if (vCheckResult == "Authentication Fail")
                {
                    lbl_nodata.Text = vCheckResult;
                    return;
                }
                else
                {   //進行MM帳密驗證
                    Response.Write("<script> window.open('../MM_verification.aspx?action=insert2', '', config='height=250,width=350',toolbar=0,resizable=1) <" + HtmlTextWriter.SlashChar + "script>");
                }

            }
            //驗證OK執行新增動作
            if (MMuserid.Text != "")
            {
                sMemoText = txt_memo1.Text.Replace(",", "").Replace("|", "").Replace("，", "");
                string UPDATE_USER = MMuserid.Text;
                string UPDATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string sub_insert_sql = "";
                string insert_sql = "";

                for (int i = 0; i < chklst_recipe.Items.Count; i++)
                {

                    if (chklst_recipe.Items[i].Selected == true)
                    {

                        //string insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,EVENT,ID) "
                        //                   + " VALUES('" + dd_eqpid.Text + "', 'RECIPE', 0, 'Y', '', null, '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "','" + chklst_recipe.Items[i].Value + "',dbms_random.string('A', 5)) ";
                        sub_insert_sql += "union all select '" + dd_eqpid.Text + "', 'RECIPE', 0, 'Y', '', null, '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "','" + chklst_recipe.Items[i].Value + "',dbms_random.string('A', 5) from dual ";
                    }
                }

                insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,EVENT,ID) ";
                insert_sql += sub_insert_sql.Trim().Substring(9);

                try
                {
                    RTD_OraDB.Exec_IUD(insert_sql);
                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {
                        lbl_nodata.Text = ex.ToString();
                    }
                }

                RTD_TABLE_INFO.Update_LastUpdateTime(vFunctionName, MMuserid.Text);
                myDBInit();
                txt_memo1.Text = "";

                if (error_string != "")
                {
                    Response.Write("<scr" + "ipt> alert('以下資料未新增;資料重覆:" + error_string + "'); </scr" + "ipt>");
                    return;
                }
            }
        }
    }
    protected void Btn_add_prod_Click(object sender, EventArgs e)
    {
        string sMemoText = "";

        vUsername = txt_id.Text;
        vPassword = txt_password.Text;

        //"確認該選資料都有選"
        if (dd_prod_ctg.SelectedValue == "請選擇" || dd_prod_prodg.SelectedValue == "請選擇" || dd_prod_openo.SelectedValue == "請選擇")
        {
            Response.Write("<scr" + "ipt> alert('尚有欄位未選擇!!(CT_G,PRODG,OPE_NO)'); </scr" + "ipt>");
            return;
        }

        //確認user有勾選 eqp_id 
        string selecteqpid = "";
        for (int i = 0; i < chklst_eqp.Items.Count; i++)
        {
            if (chklst_eqp.Items[i].Selected == true)
            {
                selecteqpid = "1";
                break;
            }
        }
        if (selecteqpid != "1")
        {
            Response.Write("<scr" + "ipt> alert('注意!!請勾選eqp_id'); </scr" + "ipt>");
            return;
        }

        //確認user有勾選 prodspec_id 
        string selectprodspec = "";
        for (int i = 0; i < chklst_prodspec.Items.Count; i++)
        {
            if (chklst_prodspec.Items[i].Selected == true)
            {
                selectprodspec = "1";
                break;
            }
        }
        if (selectprodspec != "1")
        {
            Response.Write("<scr" + "ipt> alert('注意!!請勾選prodspec_id'); </scr" + "ipt>");
            return;
        }

         //進行MM帳密驗證
        if (MMuserid.Text == "")
        {
            vCheckResult = RTD_ACCESS_CONTROL.Check_Authority(vFunctionName, vUsername, vPassword);

            if (vCheckResult == "Authentication Fail")
            {
                lbl_nodata.Text = vCheckResult;
                return;
            }
            else
            {   //進行MM帳密驗證
                Response.Write("<script> window.open('../MM_verification.aspx?action=insert3', '', config='height=250,width=350',toolbar=0,resizable=1) <" + HtmlTextWriter.SlashChar + "script>");
            }
            
        }
        //驗證OK執行新增動作
        if (MMuserid.Text != "")
        {
            sMemoText = txt_memo2.Text.Replace(",", "").Replace("|", "").Replace("，", "");
            string UPDATE_USER = MMuserid.Text;
            string UPDATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string sub_insert_sql = "";
            string insert_sql = "";

            for (int j = 0; j < chklst_eqp.Items.Count; j++)
            {
                if (chklst_eqp.Items[j].Selected == true)
                {

                    for (int i = 0; i < chklst_prodspec.Items.Count; i++)
                    {

                        if (chklst_prodspec.Items[i].Selected == true)
                        {
                            //string insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,PRODSPEC_ID,OPE_NO,ID)  "
                              //      + " VALUES('" + chklst_eqp.Items[j].Value + "', 'PROD', 0, 'Y', '', null, '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "','" + chklst_prodspec.Items[i].Value + "','" + dd_prod_openo.SelectedValue + "',dbms_random.string('A', 5))";
                            sub_insert_sql += "union all select '" + chklst_eqp.Items[j].Value + "', 'PROD', 0, 'Y', '', null, '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "','" + chklst_prodspec.Items[i].Value + "','" + dd_prod_openo.SelectedValue + "',dbms_random.string('A', 5) from dual ";
                        }
                    }
                }
            }

            insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,PRODSPEC_ID,OPE_NO,ID)  ";
            insert_sql += sub_insert_sql.Trim().Substring(9);
            try
            {
                RTD_OraDB.Exec_IUD(insert_sql);
            }
            catch (Exception ex)
            {
                if (ex != null)
                {
                    lbl_nodata.Text = ex.ToString();
                }
            }

            RTD_TABLE_INFO.Update_LastUpdateTime(vFunctionName, MMuserid.Text);
            myDBInit();
            txt_memo2.Text = "";

            if (error_string != "")
            {
                Response.Write("<scr" + "ipt> alert('以下資料未新增;資料重覆:" + error_string + "'); </scr" + "ipt>");
                return;
            }


        }
    

    }
    protected void Btn_add_prodg_Click(object sender, EventArgs e)
    {
        string sMemoText = "";

        vUsername = txt_id.Text;
        vPassword = txt_password.Text;

        //"確認該選資料都有選"
        if (txt_prodg_prodg.Text == "" || txt_prodg_openo.Text == "" )
        {
            Response.Write("<scr" + "ipt> alert('尚有欄位未選擇!!(PRODG,OPE_NO)'); </scr" + "ipt>");
            return;
        }

        //確認user有勾選 eqp_id 
        string selecteqpid = "";
        for (int i = 0; i < chklst_prodg_eqp.Items.Count; i++)
        {
            if (chklst_prodg_eqp.Items[i].Selected == true)
            {
                selecteqpid = "1";
                break;
            }
        }
        if (selecteqpid != "1")
        {
            Response.Write("<scr" + "ipt> alert('注意!!請勾選eqp_id'); </scr" + "ipt>");
            return;
        }

        //確認user有勾選 prodspec_id 
        /*   string selectprodspec = "";
        for (int i = 0; i < chklst_prodspec.Items.Count; i++)
        {
            if (chklst_prodspec.Items[i].Selected == true)
            {
                selectprodspec = "1";
                break;
            }
        }
        if (selectprodspec != "1")
        {
            Response.Write("<scr" + "ipt> alert('注意!!請勾選prodspec_id'); </scr" + "ipt>");
            return;
        }
        */
       

        //進行MM帳密驗證
        if (MMuserid.Text == "")
        {

            vCheckResult = RTD_ACCESS_CONTROL.Check_Authority(vFunctionName, vUsername, vPassword);

            if (vCheckResult == "Authentication Fail")
            {
                lbl_nodata.Text = vCheckResult;
                return;
            }
            else
            {   //進行MM帳密驗證
                Response.Write("<script> window.open('../MM_verification.aspx?action=insert4', '', config='height=250,width=350',toolbar=0,resizable=1) <" + HtmlTextWriter.SlashChar + "script>");
            }
            
        }
        //驗證OK執行新增動作
        if (MMuserid.Text != "")
        {    //txt_memo2.Text=txt_memo2.Text.Replace(",", ";");
                sMemoText = txt_prodg_memo.Text.Replace(",", "").Replace("|", "").Replace("，", "");
                string UPDATE_USER = MMuserid.Text;
                string UPDATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                for (int j = 0; j < chklst_prodg_eqp.Items.Count; j++)
                {
                    if (chklst_prodg_eqp.Items[j].Selected == true)
                    {

                        
                                string insert_sql = " INSERT INTO " + vTableName + " (EQP_ID, EVENT_TYPE, BASIC_VOLUME, FUTURE_HOLD, FORCE_TYPE, FORCE_TYPE_CODE, UPDATE_USER, UPDATE_TIME,MEMO,PRODG,OPE_NO,ID)  "
                                        + " VALUES('" + chklst_prodg_eqp.Items[j].Value + "', 'PRODG', 0, 'Y', '', null, '" + MMuserid.Text + "', to_date('" + UPDATE_TIME + "', 'yyyy/mm/dd hh24:mi:ss'),'" + sMemoText + "','" + txt_prodg_prodg.Text + "','" + txt_prodg_openo.Text + "',dbms_random.string('A', 5))";

                                //RTD_TABLE_INFO.Update_LastUpdateTime(vFunctionName, MMuserid.Text);
                                try
                                {
                                    RTD_OraDB.Exec_IUD(insert_sql);
                                }
                                catch (Exception ex)
                                {
                                    if (ex != null)
                                    {
                                        error_string = error_string + ";";
                                    }
                                }
                          
                    }
                }

                RTD_TABLE_INFO.Update_LastUpdateTime(vFunctionName, MMuserid.Text);
                myDBInit();
                txt_prodg_memo.Text = "";

                if (error_string != "")
                {
                    Response.Write("<scr" + "ipt> alert('以下資料未新增;資料重覆:" + error_string + "'); </scr" + "ipt>");
                    return;
                }
        }
    }

    protected void chk_eqpg_CheckedChanged(object sender, EventArgs e)
    {        
        GridViewRow row = (sender as CheckBox).Parent.Parent as GridViewRow;        
        CheckBox chk_eqpg = (CheckBox)row.FindControl("chk_eqpg");
        CheckBoxList cbl_eqp = (CheckBoxList)row.FindControl("cbl_eqp");
                
        if (chk_eqpg.Checked == true)
        {
            foreach (ListItem oItem in cbl_eqp.Items)
            {
                oItem.Selected = true;
            }
            
        }
        else
        {
            foreach (ListItem oItem in cbl_eqp.Items)
            {
                oItem.Selected = false;
            }
        }
    }
    
    protected void myDBInit()
    {
        GridView3.Visible = true;

        if (chk_remember_me.Checked == false)
        {
            MMuserid.Text = "";
        }

        string SQL_SELECT = " select distinct EQP_ID,EVENT_TYPE,BASIC_VOLUME,FUTURE_HOLD,PM_DURATION,FORCE_TYPE,TRACK_RECIPE,PRODSPEC_ID,OPE_NO,MEMO,"
                          + "        UPDATE_USER,UPDATE_TIME,ID,FORCE_STATUS,PRODG,TOTAL_WAFER "
                          + " from ("
                          + "       select EQP_ID,EVENT_TYPE,BASIC_VOLUME,FUTURE_HOLD,PM_DURATION,FORCE_TYPE,EVENT AS TRACK_RECIPE,PRODSPEC_ID,OPE_NO,MEMO,"
                          + "              UPDATE_USER,UPDATE_TIME,a.ID,b.FORCE_STATUS,a.PRODG, c.TOTAL_WAFER"
                          + "       from " + vTableName + " a,"
                          + "            (select distinct id,TO_CHAR(wm_concat(status)) as force_status"
                          + "             from("
                          + "                  select distinct a.id,b.lot_id||';'||c.ope_no||';'||c.lot_proc_state as status"
                          + "                  from " + vTableName + " a, RTD.CHSMARTSAMPINFOREGHS b,ods.lot c"
                          + "                  where b.claim_memo like '%'||a.ID||'%' and b.category='FORCE SAMPLING' and substr(c.lot_id,1,6)||'.'||substr(c.lot_id,7,3)=b.lot_id"
                          + "                  )"
                          + "              group by id"
                          + "             )b, "
                          + "            (select distinct id,sum(wafer_qty) as total_wafer"
                          + "             from("
                          + "                  select distinct a.id,b.lot_id, c.wafer_qty"
                          + "                  from " + vTableName + " a, RTD.CHSMARTSAMPINFOREGHS b,ods.lot c"
                          + "                  where b.claim_memo like '%'||a.ID||'%' and b.category='HOLD' and substr(c.lot_id,1,6)||'.'||substr(c.lot_id,7,3)=b.lot_id"
                          + "                  )"
                          + "              group by id"
                          + "             ) c "
                          + "        where a.id=b.id(+)"
                          + "        and a.id=c.id(+)   "
                          + "       )";
        SQL_SELECT = SQL_SELECT + where.Text + " ORDER BY UPDATE_TIME DESC ";

        try
        {
            DS = RTD_OraDB.Exec_SelectSQL(SQL_SELECT, "LIST");
        }
        catch (OleDbException exSQL)
        {
            ErrorLabel_a.Text = "SQL Error : LIST SQL 有問題,請Call工程師處理 <BR>";
            return;
        }
        catch (Exception ex)
        {
            ErrorLabel_a.Text = "Other Error : " + ex.ToString();
            return;
        }

        if ((ViewState["sorting"] == null) || (ViewState["SortField"] == null))
        {
            GridView3.DataSource = DS.Tables["LIST"];
            GridView3.DataBind();
        }
        else
        {
            using (DataView dv = DS.Tables["LIST"].DefaultView)
            {
                string strSort = ViewState["SortField"].ToString();
                if (ViewState["sorting"].ToString() == SortDirection.Descending.ToString())
                    strSort += " DESC";

                dv.Sort = strSort;

                GridView3.DataSource = dv;
                GridView3.DataBind();
            }

        }
        DS.Tables["LIST"].Clear();


    }
    protected void delete_all(object sender, EventArgs e)
    {
        int i = 0;
        vUsername = txt_id.Text;
        vPassword = txt_password.Text;

        foreach (GridViewRow row in GridView3.Rows)
        {
            CheckBox chk_flag = (CheckBox)row.Cells[0].FindControl("chk_del");
            if (chk_flag.Checked == true)
            {
                i = i + 1;
            }
        }

        if (i == 0)
        {
            Response.Write("<scr" + "ipt> alert('請選擇要刪除的項目'); </scr" + "ipt>");
            return;
        }
        else
        {   //進行MM帳密驗證
            if (MMuserid.Text == "")
            {
                vCheckResult = RTD_ACCESS_CONTROL.Check_Authority(vFunctionName, vUsername, vPassword);

                if (vCheckResult == "Authentication Fail")
                {
                    lbl_nodata.Text = vCheckResult;
                    return;
                }
                else
                {   //進行MM帳密驗證
                    //Response.Write("<script> window.open('SMART_SAMPLING_PM_new_disp.aspx?action=insert', '', config='height=200,width=250',toolbar=0,resizable=1)</script>");
                    Response.Write("<script> window.open('../MM_verification.aspx?action=delete', '', config='height=250,width=350',toolbar=0,resizable=1) <" + HtmlTextWriter.SlashChar + "script>");

                }
            }
            //驗證OK執行新增動作
            else//if (MMuserid.Text != "")//驗證OK執行新增動作
            {    string UPDATE_TIME = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                string error_string = "";
                foreach (GridViewRow row in GridView3.Rows)
                {
                    CheckBox chk_flag = (CheckBox)row.Cells[0].FindControl("chk_del");
                    //Label ID1 = (Label)row.Cells[9].FindControl("ID");
                    string ID = GridView3.Rows[row.RowIndex].Cells[15].Text;
                    
                    if (chk_flag.Checked == true)
                    {
                        string delete_sql = " DELETE " + vTableName + " WHERE ID ='" + ID + "' ";

                        //RTD_TABLE_INFO.Update_LastUpdateTime(vFunctionName, MMuserid.Text);
                        try
                        {
                            //Response.Write(delete_sql);
                            RTD_OraDB.Exec_IUD(delete_sql);                            
                        }
                        catch (Exception ex)
                        {
                            if (ex != null)
                            {
                                error_string = error_string + ";";
                            }
                        }
                    }
                    

                }
                myDBInit();
            }
                
        }
    }
    protected void chk_all_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk_all_flag = (CheckBox)GridView3.HeaderRow.FindControl("chk_all");


        foreach (GridViewRow row in GridView3.Rows)
        {

            CheckBox chk_flag = (CheckBox)row.Cells[0].FindControl("chk_del");


            if (chk_all_flag.Checked == true)
            {
                chk_flag.Checked = true;

            }
            if (chk_all_flag.Checked == false)
            {
                chk_flag.Checked = false;
            }

        }

    }
    protected void gridview3_sorting(object sender, GridViewSortEventArgs e)
    {
        if (ViewState["sorting"] == null)
        {
            e.SortDirection = SortDirection.Ascending;
            ViewState["sorting"] = "Ascending";
        }

        else
        {
            if (ViewState["sorting"].ToString() == "Ascending")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["sorting"] = "Descending";

            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["sorting"] = "Ascending";
            }

        }
        ViewState["SortField"] = e.SortExpression;
        //Response.Write("Fidle:" + ViewState["SortField"].ToString() + ",sorting:" + ViewState["sorting"].ToString());
        myDBInit();
    }
    //----------------加入FILTER資料----------------------------
    protected void onClick_addCondition(object sender, EventArgs e)
    {
        if (txt_filterCondition.Text != "")
        {
            txt_filterCondition.Text += "and \r\n";
            txt_filterCondition.Rows += 1;
        }

        switch (DD_filterCondition.SelectedItem.Value)
        {
            case "equals":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " ='" + txt_filter.Text + "' ";
                break;

            case "graterThan":
                txt_filterCondition.Text += "to_char(" + DD_filterColumn.SelectedItem.Value + ") >'" + txt_filter.Text + "' ";
                break;

            case "smallerThan":
                txt_filterCondition.Text += "to_char(" + DD_filterColumn.SelectedItem.Value + ") <'" + txt_filter.Text + "' ";
                break;

            case "notEquals":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " <>'" + txt_filter.Text + "' ";
                break;

            case "startWith":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " like '" + txt_filter.Text + "%' ";
                break;

            case "endWith":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " like '%" + txt_filter.Text + "' ";
                break;

            case "notStartWith":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " not like '" + txt_filter.Text + "%' ";
                break;

            case "notEndWith":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " not like '%" + txt_filter.Text + "' ";
                break;

            case "contain":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " like '%" + txt_filter.Text + "%' ";
                break;

            case "notContain":
                txt_filterCondition.Text += DD_filterColumn.SelectedItem.Value + " not like '%" + txt_filter.Text + "%' ";
                break;
        }

        showCondition.Visible = true;
    }
    //---------------執行FILTER FUNCTION------------------
    protected void onClick_filter(object sender, EventArgs e)
    {
        if (txt_filterCondition.Text != "")
        {
            where.Text = " where " + txt_filterCondition.Text;

            myDBInit();
        }
    }
    //----------------清除FILTER資料----------------------------
    protected void onClick_clear_search(object sender, EventArgs e)
    {
        showCondition.Visible = false;
        txt_filterCondition.Text = "";
        txt_filterCondition.Rows = 1;
        where.Text = " ";
        myDBInit();
    }
    protected void cb_all_recipe_CheckedChanged(object sender, EventArgs e)
    {

        if (cb_all_recipe.Checked == true)
        {
            for (int i = 0; i < chklst_recipe.Items.Count; i++)
            {
                chklst_recipe.Items[i].Selected = true;

            }
        }
        else
        {
            for (int i = 0; i < chklst_recipe.Items.Count; i++)
            {
                chklst_recipe.Items[i].Selected = false;

            }
            
        }


    }
    protected void cb_all_prodspec_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_all_prodspec.Checked == true)
        {
            for (int i = 0; i < chklst_prodspec.Items.Count; i++)
            {
                chklst_prodspec.Items[i].Selected = true;

            }
        }
        else
        {
            for (int i = 0; i < chklst_prodspec.Items.Count; i++)
            {
                chklst_prodspec.Items[i].Selected = false;

            }

        }
    }
    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[14].Text = e.Row.Cells[14].Text.Replace(",", "<br/>");
            
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#22cc22'");
            //設定光棒顏色，當滑鼠 onMouseOver 時驅動    

            if (e.Row.RowState == DataControlRowState.Alternate)
            {
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
                //偶數行也就是 Alternate Column
            }
            else
            {
                //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#EFF3FB'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#E7FEE2'");//ctlColorSet.DG_AB
                //奇數行
                //記得不論奇數或偶數行，當 onMouseOut 也就是滑鼠移開時，要恢復原本的顏色
            }
        }
    }
   
}
