<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T009Copy.aspx.cs" Inherits="CustomizeRule.ToolRule.T009Copy" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>T009Copy</title>
    <style type="text/css">
        #Cimesdatetimepicker1 {
            display: none;
        }
    </style>
</head>
<body class="Model">
    <form id="T009Copy" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <CimesUI:CimesDateTimePicker ID="Cimesdatetimepicker1" runat="server" />
                <table style="width: 1000px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td>
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblTargetInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, TargetInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%; margin-left: auto; margin-right: auto;">
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
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblSourceInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, SourceInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%; margin-left: auto; margin-right: auto;">
                                    <tr>
                                         <td style="width: 50%; vertical-align: top;"" >
                                             <table style="width: 100%; margin-left: auto; margin-right: auto;">
                                                <tr>
                                                    <td class="CSTD" style="width: 10%;" colspan="2">
                                                        <asp:Label ID="lblSourceDevice" runat="server" Text="<%$ Resources:Auto|RuleFace, Device %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%;">
                                                        <asp:TextBox ID="ttbSourceDevice" runat="server" ReadOnly="True" CssClass="CSInputMust"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="CSTD" style="width: 10%;"  colspan="2">
                                                        <asp:Label ID="lblSourceEquipment" runat="server" Text="<%$ Resources:Auto|RuleFace, Equipment %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%;">
                                                        <asp:TextBox ID="ttbSourceEquipment" runat="server" ReadOnly="True" CssClass="CSInputMust"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                         <td style="width: 50%; vertical-align: top;"">
                                            <div style="overflow: auto; margin-left: auto; margin-right: auto; width: 500px; height: 300px;">
                                                <CimesUI:CimesGridView ID="gvQuery" runat="server" Width="99.9%" AutoGenerateColumns="false" 
                                                    OnPageIndexChanging="gvQuery_PageIndexChanging" AllowPaging="True">
                                                    <Columns>
                                                        <asp:BoundField DataField="ToolType" HeaderText="<%$ Resources:Auto|RuleFace, ToolTypes %>" HeaderStyle-Width="50%"/>
                                                        <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:Auto|RuleFace, Quantity %>" HeaderStyle-Width="50%"/>
                                                    </Columns>
                                                </CimesUI:CimesGridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                             <CimesUI:CimesButton ID="btnCopy" runat="server" CssClass="CSSaveButton" OnClick="btnCopy_Click"
                                Text="<%$ Resources:Auto|RuleFace, Copy %>"></CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnExit" runat="server" CssClass="CSExitButton" OnClick="btnExit_Click"
                                Text="<%$ Resources:Auto|RuleFace, Exit %>" />   
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
