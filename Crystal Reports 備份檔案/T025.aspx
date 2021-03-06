<%@ Page Language="c#" CodeBehind="T025.aspx.cs" AutoEventWireup="True" Inherits="ciMes.Tool.Admin.T025" %>

<%@ Register Src="~/Web.Common.UserControl/AttributeSetupGrid.ascx" TagName="AttributeSetupGrid"
    TagPrefix="uc4" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc1" %>
<%@ Register Src="~/Web.Common.UserControl/SystemAttribute.ascx" TagName="SystemAttribute"
    TagPrefix="uc7" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>T025</title>
    <CimesUI:CimesScriptBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript" language="javascript">
            var bDelete = false;
            var DelKey = "";
            function DoSubmit(bDelete) {
                if (bDelete) {
                    if (confirm(DelKey + ' <%=DelConfirmStr%>'))
                        return true;
                    else
                        return false;
                }
            }
        </script>

    </CimesUI:CimesScriptBlock>
</head>
<body>
    <form id="T025" onsubmit="return DoSubmit(bDelete);" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <input id="hfdEquipAttrName" type="hidden" name="hfdEquipAttrName" />
                <input id="hfdEquipAttrAdd" type="hidden" name="hfdEquipAttrAdd" />
                <table id="table1" style="width: 926px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td class="CSTD" style="width: 15%;">
                            <asp:Label ID="lblToolType" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolType %>"></asp:Label>
                        </td>
                        <td style="width: 35%;">
                            <asp:TextBox ID="ttbToolType" runat="server" Width="99%"></asp:TextBox>
                        </td>
                        <td style="width: 50%;" align="right">
                            <CimesUI:CimesButton ID="btnQuery" runat="server" CssClass="CSQueryButton" Text="<%$ Resources:Auto|RuleFace, Query %>"
                                OnClick="btnQuery_Click"></CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnAdd" runat="server" CssClass="CSAddButton" Text="<%$ Resources:Auto|RuleFace, Add %>"
                                OnClick="btnAdd_Click"></CimesUI:CimesButton>
                        </td>
                    </tr>
                </table>
                <table id="table2" style="width: 920px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td>
                            <CimesUI:CimesGridView ID="gvQuery" CssClass="CSGridViewStyle" runat="server" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" PageSize="4" EnableCimesSort="true"
                                OnPreRender="gvQuery_PreRender" OnRowCreated="gvQuery_RowCreated" OnPageIndexChanging="gvQuery_PageIndexChanging"
                                OnRowCancelingEdit="gvQuery_CancelCommand" OnRowEditing="gvQuery_RowEditing"
                                OnCimesSorted="gvQuery_SortCommand" OnRowUpdating="gvQuery_RowUpdating"
                                OnRowDeleting="gvQuery_RowDeleting" OnRowDataBound="gvQuery_RowDataBound" OnDataSourceChanged="gvQuery_DataSourceChanged">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnDelete" CausesValidation="false" CommandName="Delete"
                                                CssClass="CSGridDeleteButton" Text="<%$ Resources:Auto|RuleFace, Delete %>" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" Wrap="false" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Button runat="server" ID="btnUpdate" Width="30px" CommandName="Update" Text="<%$ Resources:Auto|RuleFace, Save %>"
                                                CssClass="CSGridSaveButton" />
                                            <asp:Button runat="server" ID="btnCancel" CausesValidation="false" CommandName="Cancel"
                                                CssClass="CSGridEditButtonEven" Text="<%$ Resources:Auto|RuleFace, Cancel %>" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnEdit" Width="30px" CausesValidation="false" CommandName="Edit"
                                                CssClass="CSGridEditButton" Text="<%$ Resources:Auto|RuleFace, Edit %>" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" Wrap="false" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="TYPE" HeaderText="<%$ Resources:Auto|RuleFace, ToolType %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="ttbType" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="30%" Wrap="false" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Status" HeaderText="<%$ Resources:Auto|RuleFace, Status %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:RadioButton ID="rbEnable" runat="server" Text="<%$ Resources:Auto|RuleFace, Enable %>"
                                                GroupName="Status" ForeColor="Black"></asp:RadioButton>
                                            <asp:RadioButton ID="rbDisable" runat="server" Text="<%$ Resources:Auto|RuleFace, Disable %>"
                                                GroupName="Status" ForeColor="Black"></asp:RadioButton>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10%" Wrap="false" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Description %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Width="100%"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="ttbDescription" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="40%" Wrap="false" />
                                    </asp:TemplateField>
                                </Columns>
                            </CimesUI:CimesGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <CimesUI:CimesTab ID="rtsDetail" runat="server" MultiPageID="rmpDetail" SelectedIndex="0">
                                <Tabs>
                                    <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, SystemAttribute %>" Target="rpvSysAttribute"
                                        Selected="False" Value="SysAttribute"></CimesUI:CimesTabItem>
                                    <CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, CustomAttribute %>"
                                        Target="rpvAttribute" Value="Attribute" Enabled="false" Visible="false"></CimesUI:CimesTabItem>
                                    <%--<CimesUI:CimesTabItem Text="<%$ Resources:Auto|RuleFace, PMCounter %>" Target="rpvPMCounter"
                                        Value="PMCounter" Enabled="false">
                                    </CimesUI:CimesTabItem>--%>
                                </Tabs>
                            </CimesUI:CimesTab>
                            <CimesUI:CimesMultiPage ID="rmpDetail" runat="server" SelectedIndex="0" ScrollBars="Auto"
                                Height="200px" Width="99%" CssClass="RadPageView">
                                <CimesUI:CimesPageView ID="rpvSysAttribute" runat="server" Width="100%">
                                    <uc7:SystemAttribute ID="SystemAttribute1" runat="server" />
                                </CimesUI:CimesPageView>
                                <CimesUI:CimesPageView ID="rpvAttribute" runat="server" Width="100%" Height="100%">
                                    <uc4:AttributeSetupGrid ID="AttributeSetupGrid1" runat="server" />
                                </CimesUI:CimesPageView>
                            </CimesUI:CimesMultiPage>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
