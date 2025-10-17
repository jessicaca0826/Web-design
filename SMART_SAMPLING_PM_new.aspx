<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Smart_Sampling_PM_new.aspx.cs" Inherits="SmartSampling_PM" %>
<%@ Register TagPrefix="PSC_MFG" TagName="ColorSet" Src="..\global\ColorSet_RTD.ascx" %>
<%@ import namespace="System.Net.Sockets"%>
<%@ import namespace="System.Net"%>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Data" %>

<head runat="server">
    <title>Smart_Sampling_PM</title>
    <PSC_MFG:COLORSET id="ctlColorSet" runat="server"></PSC_MFG:COLORSET>
    <link href="../css/style_RTD.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .auto-style2 {
            width: 170px;
        }
        .auto-style3 {
            width: 610px;
        }
        .auto-style4 {
            width: 149px;
        }
        .auto-style5 {
            width: 510px;
        }
        .auto-style6 {
            width: 1599px;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
            display: block;
            height: 5px;
            margin-left: 0;
            margin-right: 0;
            margin-top: 0;
            margin-bottom: 40px;
        }
        .auto-style7 {
            width: 266px;
        }
        .auto-style8 {
            width: 267px;
        }
        .style7
        {
            width: 50%;
            height: 66px;
        }
        .style8
        {
            height: 66px;
        }
        .style9
        {
            width: 90px;
        }
        .style10
        {
            width: 70px;
        }
         .hiddencol
        {
            display:none;
        }
        .viscol
        {
            display:block;
        }
        .style22
        {
            width: 200px;
        }
        .style30
        {
            width: 430px;
        }
        .style32
        {
            width: 100px;
        }
        .style33
        {
            width: 80px;
        }
        .style34
        {
            width: 65px;
        }
         .style41
        {
            color: #000000;
            background-color: #99CCFF;
        }

        </style>
</head>
<body>
    <table class="TitleCss" id="Table_Title" >
		<tr>
			<td><asp:label id="Label_Title" runat="server" width="100%" CssClass="LabelCss_Title">RTD SMART SAMPLING PM</asp:label></td>
		</tr>
	</table>
    <asp:Label ID="last_updateTime" runat="server" CssClass="LabelCss_DT" Font-Size="11" /> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="last_uploadTime" runat="server" CssClass="LabelCss_DT" Font-Size="11" /> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="Label12" runat="server" Font-Size="Small" Text="網頁如有問題請call#6598"></asp:Label>
    
    <form id="form1" runat="server">

    <table class="LabelCss_HL" width="100%">
	    <tr>
		    <td><asp:label id="lblFunName" runat="server" CssClass="LabelCss_HL"  Width="100%" >P12 Smart Sampling PM &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='../help/RTD可續不出系統操作SOP.ppt' target=_blank>RTD可續不出系統操作SOP</a> &nbsp;&nbsp; </asp:label></td>
             <td width="60%" align=Right>
                ID:<asp:TextBox Height="20" ID="txt_id" MaxLength="20" runat="server" Width="80"  />		
                Password:<asp:TextBox ID="txt_password" Height="20" MaxLength="20" runat="server"  TextMode="Password" Width="80"/>
				<asp:CheckBox ID="chk_remember_me" Text="記住密碼" runat="server"  Font-Size="Small" />
                <asp:TextBox ID="MMuserid" runat="server"  Text="" style="display:none"></asp:TextBox>
            </td>
		</tr>

	</table>
    <asp:label id="lbl_nodata" runat="server" width="100%"  CssClass="style5"></asp:label>
	<asp:label id="ErrorLabel_a" runat="server" CssClass="LabelCss_ERR"  style="font-family: Arial, Helvetica, sans-serif"></asp:label>
    <br/>

    <div>
    <table width="100%">
        <tr>
            <td align="left" valign=top width ="40%">
                <asp:Panel ID="pnl_function" runat="server" Height="16px" Width="250">
                    <strong>
                        <asp:DropDownList ID="dd_function" runat="server" AutoPostBack="True" 
                            OnSelectedIndexChanged="dd_function_SelectedIndexChanged">
                            <asp:ListItem Text="請選擇" Value="請選擇" />
                            <asp:ListItem Text="By Case" Value="By Case" />
                            <asp:ListItem Text="By Recipe" Value="By Recipe" />
                            <asp:ListItem Text="By Prod" Value="By Prod" />
                            <asp:ListItem Text="By Prodg" Value="By Prodg" />
                        </asp:DropDownList>
                    </strong>
                </asp:Panel>
            </td>
                
            <td align="left" >
                <table align="left" border="1" cellspacing="2" bordercolor="#CCCCCC"  style="width:50%">
                    <tr>
                        <td height="25" bgcolor="#99FF66"><b>Filter:</b></td>
                        <td bgcolor="#99FF66">
                            <asp:DropDownList ID="DD_filterColumn" runat="server">
                            <asp:ListItem Text="EQP_ID" Value="EQP_ID"/>
                            <asp:ListItem Text="EVENT_TYPE" Value="EVENT_TYPE"/>
                            <asp:ListItem Text="PRODSPEC_ID" Value="PRODSPEC_ID"/>
                            <asp:ListItem Text="OPE_NO" Value="OPE_NO"/>
                            <asp:ListItem Text="UPDATE_USER" Value="UPDATE_USER"/>                                                             
                            </asp:DropDownlist>                 
                        </td>
                        <td bgcolor="#99FF66">
                            <asp:DropDownList ID="DD_filterCondition" runat="server">
                            <asp:ListItem Text="等於" Value="equals"/>                               
                            <asp:ListItem Text="不等於" Value="notEquals"/>                
                            <asp:ListItem Text="開頭以" Value="startWith"/>                
                            <asp:ListItem Text="結尾以" Value="endWith"/>                
                            <asp:ListItem Text="不開頭以" Value="notStartWith"/>                
                            <asp:ListItem Text="不結尾以" Value="notEndWith"/>                
                            <asp:ListItem Text="包含" Value="contain"/>                
                            <asp:ListItem Text="不包含" Value="notContain"/>                
                            </asp:DropDownlist>                  
                        </td><!--87CEFA -->
                        <td bgcolor="#99FF66"><asp:TextBox ID="txt_filter" Width="80" runat="server" /></td>
                        <td bgcolor="#99FF66"><asp:Button ID="Btn_addCondition" Text="加入" OnClick="onClick_addCondition" Runat="server" /></td>
                        <td bgcolor="#99FF66"><asp:Button ID="Btn_giveup_Search" Text="清除篩選" OnClick="onClick_clear_search" Runat="server" /></td>
                    </tr>
                    <tr id="showCondition" runat="Server">
                        <td colspan="5" bgcolor="#FFFFFF"><font color="#000066" size="2" face="Arial">
                            <asp:TextBox BorderStyle="none" Font-Overline="false" Font-Underline="false" ID="txt_filterCondition" runat="server" TextMode="MultiLine" Width="380" Rows="1"/>                
                        </font></td>
                        <td> 
                            <asp:Button ID="Btn_Search" Text="執行篩選" OnClick="onClick_filter" runat="server" /> 
                        </td>
                    </tr>
                </table>
            </td> 
        </tr>
    </table>	    

    <br />

    <asp:Panel ID="pnl_case" runat="server" Visible="False" Width="100%">
        <table  align="left" border="1" cellspacing="2" bordercolor="#CCCCCC" >
            <tr bgcolor="#87CEFA"><td colspan=2>By Case &nbsp;&nbsp;&nbsp;<asp:Button ID="insert_exe1" runat="server" OnClick="Btn_add_Click" Text="啟動" /></td></tr>
            <tr bgcolor="#87CEFA">
                <td>Event_Type<asp:DropDownList ID="dd_event" runat="server" 
                        AutoPostBack="True" DataTextField="EVENT_TYPE" DataValueField="EVENT_TYPE" 
                        OnSelectedIndexChanged="dd_event_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:GridView ID="GridView1" runat="server" BackColor="White" 
                        BorderColor="White" BorderStyle="Ridge" BorderWidth="2px" CellPadding="3" 
                        CellSpacing="1" EnableModelValidation="True" GridLines="None">
                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr bgcolor="#87CEFA">
                <td colspan=2>
                    <asp:Label ID="Label1" runat="server" Text="MEMO"></asp:Label>
                    <asp:TextBox ID="txt_memo3" runat="server" Width="400px"></asp:TextBox>
                    <br><font color="red" size="-1">請勿輸入{&quot;|&quot;, &quot;,&quot;,&quot; ，&quot;}字元。</font>
                    </br>
                </td>
            </tr>
            
            <tr bgcolor="#87CEFA">
                <td colspan="2">
                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                             EnableModelValidation="True" CssClass="EU_DataTable" BackColor="White" 
                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            GridLines="Vertical">
                            <AlternatingRowStyle BackColor="#DCDCDC" />
                            <Columns>
                                <asp:TemplateField HeaderText="EQP_G">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_eqpg" runat="server" AutoPostBack="True" 
                                            OnCheckedChanged="chk_eqpg_CheckedChanged" Text='<%#Bind("EQ_GROUP_NAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EQP_LIST">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="cbl_eqp" runat="server" RepeatColumns="12">
                                        </asp:CheckBoxList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnl_recipe" runat="server" Visible="False" Width="100%">
        <table align="left" border="1" cellspacing="2" bordercolor="#CCCCCC" >
            <tr bgcolor="#87CEFA"><td colspan=4>By Recipe  &nbsp;&nbsp;&nbsp; <asp:Button ID="insert_exe2" runat="server" OnClick="Btn_add_recipe_Click" Text="啟動" /></td></tr>
            <tr bgcolor="#87CEFA">
                <td class="style32">EQP_G</td>
                <td class="style33">EQP_ID </td>
                <td class="style22">RECIPE&nbsp; <asp:CheckBox ID="cb_all_recipe" runat="server" Text="全選" AutoPostBack="True" oncheckedchanged="cb_all_recipe_CheckedChanged" /></td>
                <td class="style30">MEMO</td>
            </tr>
            <tr bgcolor="#87CEFA" valign=top>
                <td><asp:DropDownList ID="dd_eqpg" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dd_eqpg_SelectedIndexChanged"></asp:DropDownList></td>
                <td><asp:DropDownList ID="dd_eqpid" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dd_eqpid_SelectedIndexChanged"></asp:DropDownList></td>
                <td><asp:DropDownList ID="dd_recipe_pre6" runat="server" AutoPostBack="True" onselectedindexchanged="dd_recipe_pre6_SelectedIndexChanged"></asp:DropDownList>
                    <asp:CheckBoxList ID="chklst_recipe" runat="server"></asp:CheckBoxList>
                </td>
                <td><asp:TextBox ID="txt_memo1" runat="server" Width="430px"></asp:TextBox>
                    <br></br>
                    <font color="red" size="-1">請勿輸入{&quot;|&quot;, &quot;,&quot;,&quot; ，&quot;}字元。</font>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="pnl_prod" runat="server" Visible="False" Width="100%">
        <table align="left" border="1" cellspacing="2" bordercolor="#CCCCCC" >
            <tr bgcolor="#87CEFA" ><td colspan=6>By Prod  &nbsp;&nbsp;&nbsp; <asp:Button ID="insert_exe3" runat="server" OnClick="Btn_add_prod_Click" Text="啟動" /></td></tr>
            <tr bgcolor="#87CEFA">
                <td>EQP_ID</td>
                <td>CT_G</td>
                <td>PROD_G</td>
                <td>PRODSPEC_ID <asp:CheckBox ID="cb_all_prodspec" runat="server" Text="全選" AutoPostBack="True" oncheckedchanged="cb_all_prodspec_CheckedChanged" /></td>
                <td>OPE_NO</td>
                <td>MEMO</td>
            </tr>
            <tr bgcolor="#87CEFA" valign=top>
                <td><asp:DropDownList ID="dd_eqp_pre2" runat="server" AutoPostBack="True" onselectedindexchanged="dd_eqp_pre2_SelectedIndexChanged" ></asp:DropDownList>
                    <asp:CheckBoxList ID="chklst_eqp" runat="server"></asp:CheckBoxList>
                </td>
                <td><asp:DropDownList ID="dd_prod_ctg" runat="server" AutoPostBack="True" onselectedindexchanged="dd_prod_ctg_SelectedIndexChanged"></asp:DropDownList></td>
                <td><asp:DropDownList ID="dd_prod_prodg" runat="server" onselectedindexchanged="dd_prod_prodg_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                <td><asp:CheckBoxList ID="chklst_prodspec" runat="server"></asp:CheckBoxList></td>
                <td><asp:DropDownList ID="dd_prod_openo" runat="server"></asp:DropDownList></td>
                <td><asp:TextBox ID="txt_memo2" runat="server" Width="430px"></asp:TextBox>
                    <font color="red" size="-1">請勿輸入{&quot;|&quot;, &quot;,&quot;,&quot; ，&quot;}字元。</font>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnl_prodg" runat="server" Visible="False" Width="100%">
        <table align="left" border="1" cellspacing="2" bordercolor="#CCCCCC" >
            <tr bgcolor="#87CEFA" ><td colspan=4>By Prod  &nbsp;&nbsp;&nbsp; <asp:Button ID="insert_exe4" runat="server" OnClick="Btn_add_prodg_Click" Text="啟動" /></td></tr>
            <tr bgcolor="#87CEFA">
                <td>EQP_ID</td>                
                <td>PROD_G</td>                
                <td>OPE_NO</td>
                <td>MEMO</td>
            </tr>
            <tr bgcolor="#87CEFA" valign=top>
                <td><asp:DropDownList ID="dd_prodg_eqp_pre2" runat="server" AutoPostBack="True" onselectedindexchanged="dd_prodg_eqp_pre2_SelectedIndexChanged" ></asp:DropDownList>
                    <asp:CheckBoxList ID="chklst_prodg_eqp" runat="server"></asp:CheckBoxList>
                </td>                
                <td><asp:TextBox ID="txt_prodg_prodg" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_prodg_openo" runat="server"></asp:TextBox></td>
                <td><asp:TextBox ID="txt_prodg_memo" runat="server" Width="430px"></asp:TextBox>
                    <font color="red" size="-1">請勿輸入{&quot;|&quot;, &quot;,&quot;,&quot; ，&quot;}字元。</font>
                </td>
            </tr>
        </table>
    </asp:Panel>                
    <br/>    
    <table class="LabelCss_HL" width="100%">
		<tr>
			<td><asp:label id="lblFunList" runat="server" CssClass="LabelCss_HL" Width="100%">啟動中</asp:label></td>
        </tr>
	</table>
    <asp:Button ID="del_exe" runat="server" OnClick="delete_all" Text="刪除" />
    <br/>    
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
            EnableModelValidation="True" AllowSorting="True"  
            OnSorting="gridview3_sorting" CssClass="DataGridCss"
            onrowdatabound="GridView3_RowDataBound">
            <HeaderStyle BackColor="#A55129" Font-Size="12px" ForeColor="Black" Height="30px" />
            <Columns>                 
                 <asp:TemplateField HeaderText="select all">
                        <HeaderTemplate>
                            </font>                            
                            <asp:CheckBox ID="chk_all" runat="server" AutoPostBack="True" OnCheckedChanged="chk_all_CheckedChanged" />
                        </HeaderTemplate>                        
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_del" runat="server" />
                        </ItemTemplate>                      
                </asp:TemplateField>
                <asp:BoundField HeaderText="HOLD PCS" DataField="TOTAL_WAFER" SortExpression="TOTAL_WAFER"/>
                <asp:BoundField HeaderText="EQP ID" DataField="EQP_ID" SortExpression="EQP_ID"/>
                <asp:BoundField HeaderText="EVENT TYPE" DataField="EVENT_TYPE" SortExpression="EVENT_TYPE"/>
                <asp:BoundField HeaderText="BASIC VOLUME" DataField="BASIC_VOLUME" SortExpression="BASIC_VOLUME"/>
                <asp:BoundField HeaderText="FUTURE HOLD" DataField="FUTURE_HOLD" SortExpression="FUTURE_HOLD"/>
                <asp:BoundField HeaderText="PM DURATION" DataField="PM_DURATION" SortExpression="PM_DURATION"/>
                <asp:BoundField HeaderText="FORCE TYPE" DataField="FORCE_TYPE" SortExpression="FORCE_TYPE"/>
                <asp:BoundField HeaderText="TRACK RECIPE" DataField="TRACK_RECIPE" SortExpression="TRACK_RECIPE" />
                <asp:BoundField HeaderText="PRODSPEC ID" DataField="PRODSPEC_ID" SortExpression="PRODSPEC_ID" />
                <asp:BoundField HeaderText="OPE NO" DataField="OPE_NO" SortExpression="OPE_NO" />
                <asp:BoundField HeaderText="MEMO" DataField="MEMO" SortExpression="MEMO" />
                <asp:BoundField HeaderText="UPDATE USER" DataField="UPDATE_USER" SortExpression="UPDATE_USER"/>
                <asp:BoundField HeaderText="UPDATE TIME" DataField="UPDATE_TIME" SortExpression="UPDATE_TIME"/>              
                <asp:BoundField HeaderText="PRODG" DataField="Prodg" SortExpression="Prodg"/>
                <asp:BoundField HeaderText="ID" DataField="ID" SortExpression="ID" ItemStyle-cssclass="hiddencol" HeaderStyle-CssClass="hiddencol">                
                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                 </asp:BoundField>
                <asp:BoundField HeaderText="FORCE STATUS" DataField="FORCE_STATUS" SortExpression="FORCE_STATUS" />
                <asp:EditCommandColumn ButtonType="LinkButton" HeaderText="manu trace lot" EditText="新增" CancelText="取消" UpdateText="儲存" />
                <asp:TemplateField HeaderText="FH HISTORY" SortExpression="FH_HISTORY">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" 
                                NavigateUrl='<%# "SMART_SAMPLING_PM_his.aspx?ID=" + DataBinder.Eval(Container.DataItem,"ID") %>'
                                Target="_blank"
                                Text="CLICK ME"></asp:HyperLink>
                        </ItemTemplate>                     
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>
        </div>
       <asp:Label ID="where" Visible="false" runat="server" />
    </form>
</body>
</html>
