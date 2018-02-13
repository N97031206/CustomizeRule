<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W034.aspx.cs" Inherits="CustomizeRule.WORule.W034"
    EnableEventValidation="false" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WOMaster</title>
    <style type="text/css">
        .prodline
        {
            color:red !important;
        }
    </style>
</head>
<body tabindex="-1">
    <form id="TemplatePage" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <table style="width: 940px; margin-left: auto; margin-right: auto;" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td class="CSTD" style="width: 100px">
                                        <asp:Label ID="lblWOSelect" runat="server" Text="<%$ Resources:Auto|RuleFace, WO %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ttbWOSelect" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                    <td class="CSTD" style="width: 100px">
                                        <asp:Label ID="lblStatusSelect" runat="server" Text="<%$ Resources:Auto|RuleFace, Status %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="cbxStatusUnAll" runat="server" Text="All" GroupName="FlagSearch"
                                            Checked="true" />
                                        <asp:RadioButton ID="cbxStatusUnRelease" runat="server" Text="UnRelease" GroupName="FlagSearch" />
                                        <asp:RadioButton ID="cbxStatusRelease" runat="server" Text="Release" GroupName="FlagSearch" />
                                        <asp:RadioButton ID="cbxStatusCreated" runat="server" Text="Created" GroupName="FlagSearch" />
                                    </td>
                                    <td>
                                        <cimesui:cimesbutton id="btnSearch" runat="server" text="<%$ Resources:Auto|RuleFace, Search %>"
                                            cssclass="CSQueryButton" onclick="btnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset>
                                <div style="overflow: auto; width: 930px; height: 210px">
                                    <cimesui:cimesgridview id="gvQuery" runat="server" autogeneratecolumns="False" cssclass="CSGridViewStyle"
                                        width="55px" allowpaging="True" pagesize="5" onpageindexchanging="gvQuery_PageIndexChanging"
                                        onrowcreated="gvQuery_RowCreated" onrowdeleting="gvQuery_RowDeleting" onrowdatabound="gvQuery_RowDataBound"
                                        onselectedindexchanging="gvQuery_SelectedIndexChanging" onrowclicked="gvQuery_RowClicked"
                                        enablecimessort="true" enablerowclick="true" enablecimesfilter="false" oncimesfiltered="gvQuery_CimesFiltered"
                                        oncimessorted="gvQuery_CimesSorted" ondatasourcechanged="gvQuery_DataSourceChanged">
                                <Columns>
                                    <CimesUI:CimesCommandField ShowDeleteButton="true" ShowEditButton="true" ChangeEditEventToSelect="true">
                                        <HeaderStyle Width="55px" />
                                        <ItemStyle Wrap="False" />
                                        <HeaderTemplate>
                                            <CimesUI:CimesGridViewFilterButton ID="CimesGridViewFilterButton1" runat="server" />
                                        </HeaderTemplate>
                                    </CimesUI:CimesCommandField>
                                </Columns>
                            </cimesui:cimesgridview>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td style="width: 30%;">
                                        <cimesui:cimestab id="rtsDetail" runat="server" multipageid="rmpDetail" selectedindex="0"
                                            width="100%" cssclass="tabStrip">
                                        <Tabs>
                                            <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, BasicSetting %>" Target="rpvBasicSetting"
                                                Value="BasicSetting" Selected="True" Enabled="false">
                                            </CimesUI:CimesTabItem>
                                            <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, LotTemplate %>" Target="rpvBasicLotTemplate"
                                                Value="LotTemplate" Selected="False" Enabled="false" Visible="false">
                                            </CimesUI:CimesTabItem>
                                            <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, RuncardLotList %>" Target="rpvWOLotTemplate"
                                                Value="WOLotTemplate" Selected="False" Enabled="false">
                                            </CimesUI:CimesTabItem>
                                        </Tabs>
                                    </cimesui:cimestab>
                                    </td>
                                    <td align="right" style="width: 70%;" valign="baseline">
                                        <CimesUI:CimesButton ID="btnAllPrint" runat="server" CssClass="CSPrintButton" Text="<%$ Resources:Auto|RuleFace, AllPrint %>"
                                            OnClick="btnAllPrint_Click"></CimesUI:CimesButton>
                                        <cimesui:cimesbutton id="btnAdd" runat="server" cssclass="CSAddButton" visible="false" text="<%$ Resources:Auto|RuleFace, Add %>"
                                            onclick="btnAdd_Click"></cimesui:cimesbutton>
                                        <cimesui:cimesbutton id="btnRelease" runat="server" cssclass="CSReleaseButton" text="<%$ Resources:Auto|RuleFace, Issue %>"
                                            onclick="btnRelease_Click"></cimesui:cimesbutton>
                                        <cimesui:cimesbutton id="btnUnRelease" runat="server" cssclass="CSUnReleaseButton"
                                            text="<%$ Resources:Auto|RuleFace, ReleaseCancel %>" onclick="btnUnRelease_Click">
                                    </cimesui:cimesbutton>
                                        <cimesui:cimesbutton id="btnSave" runat="server" cssclass="CSSaveButton" text="<%$ Resources:Auto|RuleFace, Save %>"
                                            onclick="btnSave_Click" tabindex="180"></cimesui:cimesbutton>
                                        <cimesui:cimesbutton id="btnCancel" runat="server" cssclass="CSCancelButton" text="<%$ Resources:Auto|RuleFace, Cancel %>"
                                            onclick="btnCancel_Click"></cimesui:cimesbutton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <cimesui:cimesmultipage id="rmpDetail" runat="server" selectedindex="0" scrollbars="Auto"
                                            cssclass="RadPageView" height="250px">
                                        <CimesUI:CimesPageView ID="rpvBasicSetting" runat="server" Width="99%" Enabled="false">
                                            <table width="98%">
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblWO" runat="server" Text="<%$ Resources:Auto|RuleFace, WO %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:TextBox ID="ttbWO" runat="server" Enabled="False" Text="automatically" TabIndex="-1"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 10%; ">
                                                    </td>
                                                    <td style="width: 23%; text-align:left;">
                                                        <asp:RadioButton ID="rtbManual" runat="server" Text="<%$ Resources:Auto|RuleFace, Manual %>" GroupName="PRODLINE" class="prodline" Style="color:red"/>
                                                        <asp:RadioButton ID="rtbAuto" runat="server" Text="<%$ Resources:Auto|RuleFace, Auto %>" GroupName="PRODLINE" class="prodline" Style="color:red" />
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblStatus" Text="<%$ Resources:Auto|RuleFace, Status %>" runat="server"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:Label ID="lblShowFlag" runat="server" BorderStyle="None" ForeColor="Maroon"></asp:Label>
                                                    </td>                                                    
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblCustomer" runat="server" Text="<%$ Resources:Auto|RuleFace, Customer %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:DropDownList ID="ddlCustomer" runat="server" TabIndex="10">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblSO" runat="server" Text="<%$ Resources:Auto|RuleFace, SO %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:TextBox ID="ttbSO" runat="server" TabIndex="80"></asp:TextBox>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblPriority" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, Priority %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 24%">
                                                        <asp:DropDownList ID="ddlPriority" TabIndex="150" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblProduct" runat="server" Text="<%$ Resources:Auto|RuleFace, Product %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" TabIndex="20"
                                                            OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblScheduleDate" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, ScheduleDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:TextBox ID="ttbScheduleDate" runat="server" Width="80%" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="ttbScheduleDate_CalendarExtender" runat="server" TargetControlID="ttbScheduleDate"
                                                            PopupButtonID="ibtScheduleDate" Format="yyyy/MM/dd">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <asp:ImageButton ID="ibtScheduleDate" runat="server" ImageUrl="../../Images/Calendar.JPG"
                                                            Height="20px" TabIndex="90" />
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblCritical" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, Critical %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 24%">
                                                        <asp:DropDownList ID="ddlCritical" TabIndex="160" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblDevice" runat="server" Text="<%$ Resources:Auto|RuleFace, Device %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:DropDownList ID="ddlDevice" runat="server" TabIndex="30" OnSelectedIndexChanged="ddlDevice_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblDueDate" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, DueDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:TextBox ID="ttbDueDate" runat="server" Width="80%" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="ttbDueDate_CalendarExtender" runat="server" TargetControlID="ttbDueDate"
                                                            PopupButtonID="ibtDueDate" Format="yyyy/MM/dd">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <asp:ImageButton ID="ibtDueDate" runat="server" ImageUrl="../../Images/Calendar.JPG"
                                                            Height="20px" TabIndex="100" />
                                                    </td>
                                                    <td class="CSTD" style="width: 10%" rowspan="5">
                                                        <asp:Label ID="lblRemark" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, Remark %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 24%" rowspan="5" valign="top">
                                                        <asp:TextBox ID="ttbRemark" TabIndex="170" runat="server" TextMode="MultiLine" Height="150px"
                                                            Width="95%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblRoute" runat="server" Text="<%$ Resources:Auto|RuleFace, Route %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:DropDownList ID="ddlRoute" runat="server" TabIndex="40">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblCommitDate" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, CommitDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%" align="left">
                                                        <asp:TextBox ID="ttbCommitDate" runat="server" Width="80%" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="ttbCommitDate_CalendarExtender" runat="server" TargetControlID="ttbCommitDate"
                                                            PopupButtonID="ibtCommitDate" Format="yyyy/MM/dd">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <asp:ImageButton ID="ibtCommitDate" runat="server" ImageUrl="../../Images/Calendar.JPG"
                                                            Height="20px" TabIndex="110" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblQuantity" runat="server" Text="<%$ Resources:Auto|RuleFace, Quantity %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:TextBox ID="ttbQuantity" runat="server" TabIndex="50"></asp:TextBox>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblSecondQty" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, SecondQuantity %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:TextBox ID="ttbSQuantity" TabIndex="120" runat="server">0</asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Auto|RuleFace, Unit %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:DropDownList ID="ddlUnit" runat="server" TabIndex="60">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblSecondUnit" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, SecondUnit %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:DropDownList ID="ddlSUnit" TabIndex="130" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTDMust" style="width: 10%">
                                                        <asp:Label ID="lblLotType" runat="server" Text="<%$ Resources:Auto|RuleFace, LotType %>"
                                                            Width="100%"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:DropDownList ID="ddlLotType" runat="server" Width="100%" TabIndex="70">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="CSTD" style="width: 10%">
                                                        <asp:Label ID="lblOwner" runat="server" Width="100%" Text="<%$ Resources:Auto|RuleFace, Owner %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 23%">
                                                        <asp:DropDownList ID="ddlOwner" TabIndex="140" runat="server" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </CimesUI:CimesPageView>
                                        <CimesUI:CimesPageView ID="rpvBasicLotTemplate" runat="server" Width="99%" Enabled="false">
                                            <table width="80%">
                                                <tr>
                                                    <td class="CSTD" style="width: 20%">
                                                        <asp:Label ID="lblLotTemplate" runat="server" Text="<%$ Resources:Auto|RuleFace, LotTemplate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="ddlLotTemplate" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLotTemplate_SelectedIndexChanged"
                                                            Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div style="width: 99%; height: 200px; overflow: auto;">
                                                            <%--<asp:GridView ID="gvLotTemplate" runat="server" Width="99%" Height="1px" AutoGenerateColumns="False"
                                                                BorderStyle="Ridge" CellPadding="3" BorderWidth="1px" AllowSorting="True" Font-Size="10pt"
                                                                Style="border-right: darkgray thin solid; border-top: darkgray thin solid; border-left: darkgray thin solid;
                                                                border-bottom: darkgray thin solid" PageSize="3" OnRowDataBound="gvLotTemplate_RowDataBound">
                                                                <SelectedRowStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedRowStyle>
                                                                <EditRowStyle BackColor="#F7CB31"></EditRowStyle>
                                                                <AlternatingRowStyle BackColor="White" BorderColor="#8C8C8C" BorderWidth="1px"></AlternatingRowStyle>
                                                                <RowStyle ForeColor="Black" BackColor="#EAECEB" BorderColor="#848E78" BorderStyle="Ridge">
                                                                </RowStyle>
                                                                <HeaderStyle HorizontalAlign="Center" ForeColor="Black" VerticalAlign="Middle" BackColor="#A1ABC3">
                                                                </HeaderStyle>
                                                                <FooterStyle Font-Size="Large" Wrap="False" ForeColor="#4A3C8C" BackColor="#B5C7DE">
                                                                </FooterStyle>
                                                                <Columns>
                                                                    <asp:BoundField DataField="ATTRIBUTENAME" HeaderText="Attribute Name">
                                                                        <HeaderStyle Width="40%"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Value">
                                                                        <HeaderStyle Width="60%"></HeaderStyle>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="ttbValue" runat="server" Width="95%"></asp:TextBox>
                                                                            <asp:DropDownList ID="ddlValue" runat="server" Width="95%" Visible="False">
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ATTRSID" HeaderText="ATTRSID" Visible="False">
                                                                        <FooterStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" Wrap="False" />
                                                                        <HeaderStyle Width="1%" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                                            Font-Strikeout="False" Font-Underline="False" Wrap="False"></HeaderStyle>
                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                            Font-Underline="False" Wrap="False" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <PagerStyle HorizontalAlign="Left" ForeColor="Black" BackColor="#A1ABC3"></PagerStyle>
                                                                <PagerSettings Mode="Numeric" PageButtonCount="20" />
                                                            </asp:GridView>--%>

                                                            <CimesUI:CimesGridView ID="gvLotTemplate" runat="server" Width="99.9%" AutoGenerateColumns="False"  allowpaging="False" 
	                                                        OnRowDataBound="gvLotTemplate_RowDataBound" >
	                                                            <columns>
		                                                            <asp:BoundField DataField="ATTRIBUTENAME" HeaderText="<%$ Resources:Auto|RuleFace, ATTRIBUTENAME %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
		                                                            <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Value %>">
			                                                            <ItemStyle HorizontalAlign="Center" />
			                                                            <ItemTemplate>
				                                                            <asp:TextBox ID="ttbValue" runat="server" Width="95%"></asp:TextBox>
				                                                            <asp:DropDownList ID="ddlValue" runat="server" Width="95%" Visible="False">
				                                                            </asp:DropDownList>
			                                                            </ItemTemplate>
		                                                            </asp:TemplateField>
	                                                            </columns>
                                                            </CimesUI:CimesGridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </CimesUI:CimesPageView>
                                            <CimesUI:CimesPageView ID="rpvWOLotTemplate" runat="server" Width="99%" Enabled="false">
                                            <table width="98%">
                                                <tr>
                                                    <CimesUI:CimesGridView ID="gvWOLotTemplate" runat="server" Width="99.9%" AutoGenerateColumns="False"  allowpaging="False" 
	                                                         >
	                                                            <columns>
		                                                            <asp:BoundField DataField="WOSEQUENCE" HeaderText="<%$ Resources:Auto|RuleFace, SN %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
		                                                            <asp:BoundField DataField="WOLOT" HeaderText="<%$ Resources:Auto|RuleFace, WorkOrderLot %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
                                                                    <asp:BoundField DataField="INVLOT" HeaderText="<%$ Resources:Auto|RuleFace, InventoryNO %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
                                                                    <asp:BoundField DataField="MATERIALLOT" HeaderText="<%$ Resources:Auto|RuleFace, MaterialLot %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
                                                                    <asp:BoundField DataField="QUANTITY" HeaderText="<%$ Resources:Auto|RuleFace, Quantity %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
                                                                    <asp:BoundField DataField="CREATEFLAG" HeaderText="<%$ Resources:Auto|RuleFace, Status %>">
			                                                            <HeaderStyle Wrap="false" />
		                                                            </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <HeaderStyle Wrap="false" />  
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <CimesUI:CimesButton ID="btnPrint" runat="server" CssClass="CSPrintButton" Text="<%$ Resources:Auto|RuleFace, Print %>"
                                                                                    OnClick="btnPrint_Click"></CimesUI:CimesButton>
                                                                            </ItemTemplate>                                           
                                                                    </asp:TemplateField>
	                                                            </columns>
                                                            </CimesUI:CimesGridView>
                                                </tr>
                                            </table>
                                        </CimesUI:CimesPageView>
                                    </cimesui:cimesmultipage>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
