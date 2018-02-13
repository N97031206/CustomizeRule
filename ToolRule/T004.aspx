<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T004.aspx.cs" Inherits="CustomizeRule.ToolRule.T004" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T004</title>
</head>
<body>
    <form id="T004" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 500px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblToolInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblToolName" Text="<%$ Resources:Auto|RuleFace, ToolModeName %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbToolName" runat="server" AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblToolType" Text="<%$ Resources:Auto|RuleFace, ToolModeType %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 60%;">
                                <asp:TextBox ID="ttbToolType" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblToolDescr" Text="<%$ Resources:Auto|RuleFace, ToolModeDescription %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbToolDescr" runat="server" ReadOnly="true" Height="60px" TextMode="MultiLine" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblEquip" Text="<%$ Resources:Auto|RuleFace, Equip %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbEquip" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblCheckOutReasonCode" Text="<%$ Resources:Auto|RuleFace, CheckOutReasonCode %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:DropDownList ID="ddlCheckOutReasonCode" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblCheckOutDescr" Text="<%$ Resources:Auto|RuleFace, CheckOutDescr %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbCheckOutDescr" runat="server" Height="60px" TextMode="MultiLine" ></asp:TextBox>
                                </td>
                            </tr>
                            
                        </table>
                    </fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <CimesUI:CimesButton ID="btnOK" runat="server" CssClass="CSOKButton"
                                        Text="<%$ Resources:Auto|RuleFace, OK %>" OnClick="btnOK_Click"></CimesUI:CimesButton>
                                <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" OnClick="btnCancel_Click"
                                        Text="<%$ Resources:Auto|RuleFace, Exit %>" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
