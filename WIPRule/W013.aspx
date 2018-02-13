<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W013.aspx.cs" Inherits="CustomizeRule.WIPRule.W013" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W013</title>
</head>
<body>
    <form id="W013" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 500px">
                    <table style="width: 100%;">
                        <tr>
                            <td class="CSTDMust" style="width: 20%;">
                                <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, RuncardLot %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 80%;">
                                <asp:TextBox ID="ttbLot" runat="server" AutoPostBack="true" OnTextChanged="ttbLot_TextChanged"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="CSTD" style="width: 20%;">
                                <asp:Label ID="lblItemSN" Text="<%$ Resources:Auto|RuleFace, ItemSN %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 80%;">
                                <asp:TextBox ID="ttbItemSN" runat="server" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="CSTD" style="width: 20%;">
                                <asp:Label ID="lblJudgeReason" Text="<%$ Resources:Auto|RuleFace, JudgeReason %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 80%;">
                                <asp:TextBox ID="ttbJudgeReason" runat="server" ReadOnly="true" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="CSTDMust" style="width: 20%;">
                                <asp:Label ID="lblRepairResult" Text="<%$ Resources:Auto|RuleFace, RepairResult %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 80%;">
                                <asp:RadioButton ID="rdbOK" runat="server" Text="<%$ Resources:Auto|RuleFace, RepairOK %>" GroupName="RepairResult" />
                                <asp:RadioButton ID="rdbNG" runat="server" Text="<%$ Resources:Auto|RuleFace, RepairNG %>" GroupName="RepairResult" />
                            </td>
                        </tr>
                        <tr>
                            <td class="CSTD" style="width: 20%;">
                                <asp:Label ID="lblRepairReasonCode" Text="<%$ Resources:Auto|RuleFace, RepairReasonCode %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 80%;">
                                <asp:DropDownList ID="ddlRepairReasonCode" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="CSTD" style="width: 20%;">
                                <asp:Label ID="lblRepairDescr" Text="<%$ Resources:Auto|RuleFace, Description %>" runat="server"></asp:Label>
                            </td>
                            <td style="width: 80%;">
                                <asp:TextBox ID="ttbRepairDescr" runat="server" Height="60px" TextMode="MultiLine" ></asp:TextBox>
                            </td>
                        </tr>
                         <tr>
                            <td align="center" colspan="2">
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

