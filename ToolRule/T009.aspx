<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T009.aspx.cs" Inherits="CustomizeRule.ToolRule.T009" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T009</title>

    <script type="text/javascript">
        //關閉ToolOperSetTool.aspx
        function closeMasterWindow() {

            document.getElementById('btnQuery').click();

            ClosejQueryDialog();
        }
    </script>
</head>
<body>
    <form id="T009" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <table style="width: 1000px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td align="right">
                            <cimesui:cimesbutton id="btnCopy" runat="server" cssclass="CSCopyButton" onclick="btnCopy_Click"
                                text="<%$ Resources:Auto|RuleFace, Copy %>"></cimesui:cimesbutton>
                            <cimesui:cimesbutton id="btnAdd" runat="server" cssclass="CSAddButton" onclick="btnAdd_Click"
                                text="<%$ Resources:Auto|RuleFace, Add %>"></cimesui:cimesbutton>
                            <cimesui:cimesbutton id="btnQuery" runat="server" cssclass="CSQueryButton" onclick="btnQuery_Click"
                                text="<%$ Resources:Auto|RuleFace, Query %>"></cimesui:cimesbutton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblBaseInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, BaseInfo %>"></asp:Label>
                                </legend>
                                <table id="table1" style="width: 100%; margin-left: auto; margin-right: auto;">
                                    <tr>
                                        <td class="CSTDMust" style="width: 10%;">
                                            <asp:Label ID="lblProduct" runat="server" Text="<%$ Resources:Auto|RuleFace, Product %>"></asp:Label>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="ttbProductFilter" runat="server"
                                                OnTextChanged="ttbProductFilter_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="CSTDMust" style="width: 10%;">
                                            <asp:Label ID="lblDevice" runat="server" Text="<%$ Resources:Auto|RuleFace, Device %>"></asp:Label>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="ttbDeviceFilter" runat="server"
                                                OnTextChanged="ttbDeviceFilter_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:DropDownList ID="ddlDevice" runat="server"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlDevice_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust" style="width: 10%;">
                                            <asp:Label ID="lblEquipGroup" runat="server" Text="<%$ Resources:Auto|RuleFace, EquipmentGroup %>"></asp:Label>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="ttbEquipGroupFilter" runat="server" AutoPostBack="True"
                                                OnTextChanged="ttbEquipGroupFilter_TextChanged"></asp:TextBox>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:DropDownList ID="ddlEquipGroup" runat="server" OnSelectedIndexChanged="ddlEquipGroup_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="CSTDMust" style="width: 10%;">
                                            <asp:Label ID="lblEquipment" runat="server" Text="<%$ Resources:Auto|RuleFace, Equipment %>"></asp:Label>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:TextBox ID="ttbEquipmentFilter" runat="server" AutoPostBack="True"
                                                OnTextChanged="ttbEquipmentFilter_TextChanged"></asp:TextBox>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:DropDownList ID="ddlEquipment" runat="server" OnSelectedIndexChanged="ddlEquipment_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 1000px; overflow: auto; height: 400px; margin-left: auto; margin-right: auto;">
                                <cimesui:cimesgridview id="gvQuery" runat="server" width="100%" autogeneratecolumns="False"
                                    pagesize="3" onrowdatabound="gvQuery_RowDataBound" onpageindexchanging="gvQuery_PageIndexChanging"
                                    allowpaging="True" onrowdeleting="gvQuery_RowDeleting" onselectedindexchanging="gvQuery_SelectedIndexChanging">
                                    <Columns>
                                        <CimesUI:CimesCommandField ShowDeleteButton="true" ShowEditButton="true" ChangeEditEventToSelect="true" HeaderStyle-Width="10%"></CimesUI:CimesCommandField>
                                        <asp:BoundField DataField="DeviceName" HeaderText="<%$ Resources:Auto|RuleFace, Device %>" HeaderStyle-Width="20%"/>
                                        <asp:BoundField DataField="EquipmentName" HeaderText="<%$ Resources:Auto|RuleFace, Equipment %>" HeaderStyle-Width="20%"/>
                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ToolList %>" HeaderStyle-Width="50%">
                                            <ItemTemplate>
                                                <div style="width: 100%; height: 100px; overflow: auto;">
                                                    <CimesUI:CimesGridView ID="gvTool" runat="server" TabIndex="15" Width="99%" CssClass="CSGridViewStyle"
                                                        AutoGenerateColumns="False" PageSize="2">
                                                        <Columns>
                                                            <asp:BoundField DataField="ToolType" HeaderText="<%$ Resources:Auto|RuleFace, ToolTypes %>">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OperationName" HeaderText="<%$ Resources:Auto|RuleFace, Operation %>">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>                                                            
                                                            <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:Auto|RuleFace, Quantity %>">
                                                                <HeaderStyle Wrap="false" />
                                                                <ItemStyle Wrap="False" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </CimesUI:CimesGridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </cimesui:cimesgridview>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <%-- <Triggers>
                <asp:PostBackTrigger ControlID="btnCopy" />
            </Triggers>--%>
        </asp:UpdatePanel>
    </form>
</body>
</html>

