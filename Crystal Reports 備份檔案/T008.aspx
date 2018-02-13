<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T008.aspx.cs" Inherits="CustomizeRule.ToolRule.T008" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc4" %>
<%@ Register Src="~/Web.Common.UserControl/AttributeSetupGrid.ascx" TagName="AttributeSetupGrid"
    TagPrefix="uc1" %>
<%@ Register Src="~/PMS/UserControl/PMCounterSetup.ascx" TagName="PMCounterSetup"
    TagPrefix="uc3" %>
<%@ Register Src="~/Web.Common.UserControl/UserDefineColumnSet.ascx" TagName="UserDefineColumnSet"
    TagPrefix="uc5" %>
<%@ Register Src="~/Web.Common.UserControl/SystemAttribute.ascx" TagName="SystemAttribute"
    TagPrefix="uc7" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>T008</title>

    <script type="text/javascript">
        //關閉ToolOperSetTool.aspx
        function closeMasterWindow() {

           // document.getElementById('btnQuery').click();

            ClosejQueryDialog();
        }

        function ShowLoading() {
            var img = document.getElementById('imgProgress');
            img.style.visibility = 'visible';
            return true;
        }
    </script>
</head>
<body>
    <form id="T008" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc4:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <br />
                <div style="width: 920px; height: 200px; overflow: auto; margin-left: auto; margin-right: auto; margin-top: auto;">
                    <CimesUI:CimesGridView ID="gvToolList" runat="server" Width="99.9%" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="5" EnableCimesFilter="True" EnableCimesSort="true"
                        OnPageIndexChanging="gvToolList_PageIndexChanging" OnRowDeleting="gvToolList_RowDeleting"
                        OnSelectedIndexChanging="gvToolList_SelectedIndexChanging" OnRowDataBound="gvToolList_RowDataBound"
                        OnDataSourceChanged="gvToolList_DataSourceChanged" OnCimesFiltered="gvToolList_CimesFiltered"
                        OnCimesSorted="gvToolList_CimesSorted">
                        <columns>
                            <CimesUI:CimesCommandField ShowDeleteButton="true" ShowEditButton="true" ChangeEditEventToSelect="true">
                                <HeaderTemplate>
                                    <CimesUI:CimesGridViewFilterButton ID="CimesGridViewFilterButton1" runat="server" />
                                </HeaderTemplate>
                            </CimesUI:CimesCommandField>
                            <%--<asp:CommandField ShowSelectButton="false" SelectText="<%$ Resources:Auto|RuleFace, Edit %>"
                                DeleteText="<%$ Resources:Auto|RuleFace, Delete %>" ControlStyle-CssClass="CSGridDeleteButton"
                                ButtonType="Button" ShowDeleteButton="True">
                                <ControlStyle CssClass="CSGridDeleteButton" />
                                <ItemStyle Width="45px" Wrap="false" />
                            </asp:CommandField>
                            <asp:CommandField ShowSelectButton="True" SelectText="<%$ Resources:Auto|RuleFace, Edit %>"
                                DeleteText="<%$ Resources:Auto|RuleFace, Delete %>" ControlStyle-CssClass="CSGridEditButton"
                                ButtonType="Button" ShowDeleteButton="false">
                                <ControlStyle CssClass="CSGridEditButton" />
                                <ItemStyle Width="45px" Wrap="false" />
                            </asp:CommandField>--%>
                            <asp:BoundField DataField="TOOLNAME" HeaderText="<%$ Resources:Auto|RuleFace, MillNumber %>" />
                            <asp:BoundField DataField="UsingStatus" HeaderText="<%$ Resources:Auto|RuleFace, Status %>" />
                            <asp:BoundField DataField="TOOLTYPE" HeaderText="<%$ Resources:Auto|RuleFace, MillTypes %>" />
                            <asp:BoundField DataField="CURRENTSTATE" HeaderText="<%$ Resources:Auto|RuleFace, CurrentState %>" />
                            <asp:BoundField DataField="Identity" HeaderText="<%$ Resources:Auto|RuleFace, ToolStatus %>" />
                            <asp:BoundField DataField="Description" HeaderText="<%$ Resources:Auto|RuleFace, Description %>" />
                            <asp:BoundField DataField="LOCATION" HeaderText="<%$ Resources:Auto|RuleFace, Position %>" />
                        </columns>
                    </CimesUI:CimesGridView>
                </div>
                <table width="920px" style="margin-right: auto; margin-left: auto;" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <CimesUI:CimesTab ID="rtsDetail" runat="server" MultiPageID="rmpDetail" SelectedIndex="0">
                                <tabs>
                                    <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, BasicSetting %>" Target="rpvBasicSetting"
                                        Value="BasicSetting" Selected="True"></CimesUI:CimesTabItem>
                                    <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, SystemAttribute %>" Target="rpvSysAttribute"
                                        Selected="False" Visible="false" Value="SysAttribute"></CimesUI:CimesTabItem>
                                    <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, CustomAttribute %>" Target="rpvAttribute"
                                        Value="Attribute" Visible="false" Enabled="false"></CimesUI:CimesTabItem>
                                    <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, PMUseCount %>" Target="rpvPMCounter"
                                        Value="PMCounter" Visible="false" Enabled="false"></CimesUI:CimesTabItem>
                                </tabs>
                            </CimesUI:CimesTab>
                        </td>
                        <td align="right">
                            <CimesUI:CimesButton ID="btnInspectionQuery" runat="server" CssClass="CSQueryButton" Text="<%$ Resources:Auto|RuleFace, InspectionQuery %>"
                                OnClick="btnInspectionQuery_Click" Visible="false">
                            </CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnPictureQuery" runat="server" CssClass="CSQueryButton" Text="<%$ Resources:Auto|RuleFace, PictureQuery %>"
                                OnClick="btnPictureQuery_Click" Visible="false">
                            </CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnAdd" runat="server" CssClass="CSAddButton" Text="<%$ Resources:Auto|RuleFace, Add %>"
                                OnClick="btnAdd_Click">
                            </CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnSave" runat="server" CssClass="CSSaveButton" Text="<%$ Resources:Auto|RuleFace, Save %>"
                                OnClick="btnSave_Click" Visible="false">
                            </CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnPrint" runat="server" CssClass="CSSaveButton" Text="<%$ Resources:Auto|RuleFace, Print %>"
                                OnClick="btnPrint_Click" Visible="false">
                            </CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" Text="<%$ Resources:Auto|RuleFace, Cancel %>"
                                OnClick="btnCancel_Click" Visible="false">
                            </CimesUI:CimesButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <CimesUI:CimesMultiPage ID="rmpDetail" runat="server" SelectedIndex="0" ScrollBars="Auto"
                                CssClass="RadPageView" Height="100%" Width="100%">
                                <CimesUI:CimesPageView ID="rpvBasicSetting" runat="server" Width="100%" Height="100%">
                                    <table style="width: 99%;">
                                        <tr>
                                            <td class="CSTDMust" style="width: 15%;">
                                                <asp:Label ID="lblType" runat="server" Text="<%$ Resources:Auto|RuleFace, MillTypes %>"></asp:Label>
                                            </td>
                                            <td style="width: 35%;">
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="CSDropDownListMust" Width="100%"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <%--<td rowspan="5" style="height: 100%;" valign="top">
                                                <uc5:UserDefineColumnSet ID="UserDefineColumnSet1" runat="server" />
                                            </td>--%>
                                            <td class="CSTDMust">
                                                <asp:Label ID="lblPurchasingProd" runat="server" Text="<%$ Resources:Auto|RuleFace, PurchasingProduction %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ttbPurchasingProd" Width="100%" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="CSTDMust" style="width: 15%;">
                                                <asp:Label ID="lblStates" runat="server" Text="<%$ Resources:Auto|RuleFace, Status %>"></asp:Label>
                                            </td>
                                            <td style="width: 35%;" align="left">
                                                <asp:RadioButton ID="rbtEnable" runat="server" Text="<%$ Resources:Auto|RuleFace, Enable %>"
                                                    Width="80px" GroupName="status"></asp:RadioButton>
                                                <asp:RadioButton ID="rbtDisable" runat="server" Text="<%$ Resources:Auto|RuleFace, Disable %>"
                                                    Width="80px" GroupName="status"></asp:RadioButton>
                                            </td>
                                            
                                            <td class="CSTDMust">
                                                <asp:Label ID="lblSupplier" runat="server" Text="<%$ Resources:Auto|RuleFace, Supplier %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlSupplier" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="CSTDMust" style="width: 15%;">
                                                <asp:Label ID="lblToolName" runat="server" Text="<%$ Resources:Auto|RuleFace, MillNumber %>"></asp:Label>
                                            </td>
                                            <td style="width: 35%;">
                                                <asp:TextBox ID="ttbToolName" Width="99%" CssClass="CSInputMust" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td class="CSTDMust">
                                                <asp:Label ID="lblToolQty" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolQty %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ttbToolQty" Width="100%" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="CSTD">
                                                <asp:Label ID="lblToolTypeSpec" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolTypeSpecification %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ttbToolTypeSpec" Width="100%" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                             <td class="CSTD">
                                                <asp:Label ID="lblInspectionReport" runat="server" Text="<%$ Resources:Auto|RuleFace, InspectionReport %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="80%" Style="float: left;" />
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="CSTD">
                                                <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:Auto|RuleFace, Units %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ttbUnit" Width="100%" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <td class="CSTD">
                                                <asp:Label ID="lblPreview" Text="<%$ Resources:Auto|RuleFace, Preview %>" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:HyperLink ID="hlShowFile" runat="server" Target="_blank"></asp:HyperLink>
                                                <asp:Image runat="server" Style="float: right;visibility: hidden" ID="imgProgress" ImageUrl="~/Images/Loading.gif" Height="16px" Width="16px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="CSTD">
                                                <asp:Label ID="lblState" runat="server" Text="<%$ Resources:Auto|RuleFace, CurrentState %>"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlState" runat="server" Width="100%" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="CSTD">
                                                <asp:Label ID="Label1" runat="server" Text=" "></asp:Label>
                                            </td>
                                            <td>
                                                <CimesUI:CimesButton ID="btnUpload" runat="server" CssClass="CSAddButton" OnClientClick="return ShowLoading();" Text="<%$ Resources:Auto|RuleFace, UploadFile %>"
                                                    OnClick="btnUpload_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="CSTD">
                                                <asp:Label ID="lblDescription" runat="server" Text="<%$ Resources:Auto|RuleFace, Description %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ttbDescr" Width="100%" runat="server" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </CimesUI:CimesPageView>
                                <CimesUI:CimesPageView ID="rpvSysAttribute" runat="server" Width="100%" Height="70%">
                                    <uc7:systemattribute id="SystemAttribute1" runat="server" />
                                </CimesUI:CimesPageView>
                                <CimesUI:CimesPageView ID="rpvAttribute" runat="server" Width="100%" Height="70%">
                                    <uc1:AttributeSetupGrid ID="AttributeSetupGrid1" runat="server" />
                                </CimesUI:CimesPageView>
                                <CimesUI:CimesPageView ID="rpvPMCounter" runat="server" Width="100%" Height="70%">
                                    <div style="width: 100%; height: 100%; overflow: auto">
                                        <uc3:PMCounterSetup ID="PMCounterSetup1" runat="server" />
                                    </div>
                                </CimesUI:CimesPageView>
                            </CimesUI:CimesMultiPage>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>
