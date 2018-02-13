<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T009Set.aspx.cs" Inherits="CustomizeRule.ToolRule.T009Set" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>T009Set</title>
    <style type="text/css">
        #Cimesdatetimepicker1 {
            display: none;
        }
    </style>
</head>
<body class="Model">
    <form id="T009Set" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <CimesUI:CimesDateTimePicker ID="Cimesdatetimepicker1" runat="server" />
                <table style="width: 700px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td align="right">
                            <CimesUI:CimesButton ID="btnAdd" runat="server" CssClass="CSAddButton" OnClick="btnAdd_Click"
                                Text="<%$ Resources:Auto|RuleFace, Add %>"></CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnSave" runat="server" CssClass="CSSaveButton" OnClick="btnSave_Click"
                                Text="<%$ Resources:Auto|RuleFace, Save %>"></CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnExit" runat="server" CssClass="CSExitButton" OnClick="btnExit_Click"
                                Text="<%$ Resources:Auto|RuleFace, Exit %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset>
                                <legend>
                                    <asp:Label ID="lblBaseInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, TargetInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%; margin-left: auto; margin-right: auto;">
                                    <tr>
                                        <td class="CSTDMust" style="width: 20%;">
                                            <asp:Label ID="lblDevice" runat="server" Text="<%$ Resources:Auto|RuleFace, Device %>"></asp:Label>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:TextBox ID="ttbDevice" runat="server" ReadOnly="True" CssClass="CSInputMust"></asp:TextBox>
                                        </td>
                                        <td class="CSTDMust" style="width: 20%;">
                                            <asp:Label ID="lblEquipment" runat="server" Text="<%$ Resources:Auto|RuleFace, Equipment %>"></asp:Label>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:TextBox ID="ttbEquipment" runat="server" ReadOnly="True" CssClass="CSInputMust"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust" style="width: 20%;">
                                            <asp:Label ID="lblToolType" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolTypes %>"></asp:Label>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:DropDownList ID="ddlToolType" runat="server" CssClass="CSDropDownListMust"></asp:DropDownList>
                                        </td>
                                        <td class="CSTDMust" style="width: 20%;">
                                            <asp:Label ID="lblOperation" runat="server" Text="<%$ Resources:Auto|RuleFace, Operation %>"></asp:Label>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:DropDownList ID="ddlOperation" runat="server" CssClass="CSDropDownListMust"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust" style="width: 20%;">
                                            <asp:Label ID="lblQuantity" runat="server" Text="<%$ Resources:Auto|RuleFace, Quantity %>"></asp:Label>
                                        </td>
                                        <td style="width: 30%;">
                                            <asp:TextBox ID="ttbQuantity" runat="server" CssClass="CSInputMust"></asp:TextBox>
                                        </td>                                        
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table style="width: 690px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td align="center">
                            <div style="overflow: auto; margin-left: auto; margin-right: auto; width: 100%; height: 200px;">
                                <CimesUI:CimesGridView ID="gvQuery" runat="server" Width="100%" AutoGenerateColumns="false" 
                                    OnRowDeleting="gvQuery_RowDeleting" OnPageIndexChanging="gvQuery_PageIndexChanging"
                                    AllowPaging="True">
                                    <Columns>
                                        <CimesUI:CimesCommandField ShowDeleteButton="true" HeaderStyle-Width="10%"></CimesUI:CimesCommandField>
                                        <asp:BoundField DataField="ToolType" HeaderText="<%$ Resources:Auto|RuleFace, ToolTypes %>" HeaderStyle-Width="30%"/>
                                        <asp:BoundField DataField="OperationName" HeaderText="<%$ Resources:Auto|RuleFace, Operation %>" HeaderStyle-Width="30%"/>
                                        <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:Auto|RuleFace, Quantity %>" HeaderStyle-Width="30%"/>                                        
                                    </Columns>
                                </CimesUI:CimesGridView>
                            </div>
                        </td>
                    </tr>
                </table>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
