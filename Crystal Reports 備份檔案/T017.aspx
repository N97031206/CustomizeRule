﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T017.aspx.cs" Inherits="CustomizeRule.ToolRule.T017" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T017</title>
</head>
<body>
    <form id="T017" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 500px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblToolInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolPartsInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblToolName" Text="<%$ Resources:Auto|RuleFace, ToolParts %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbToolName" runat="server" AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblRepairPart" Text="<%$ Resources:Auto|RuleFace, RepairPart %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbRepairPart" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblReason" Text="<%$ Resources:Auto|RuleFace, RepairReasonCode %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:DropDownList ID="ddlReason" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblDate" Text="<%$ Resources:Auto|RuleFace, BackDate %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="ttbDate" runat="server" Width="93%" Enabled="true"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="ttbDate_CalendarExtender" runat="server" TargetControlID="ttbDate" PopupButtonID="ibtDate" Format="yyyy/MM/dd">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:ImageButton ID="ibtDate" runat="server" ImageUrl="../../Images/Calendar.JPG" Height="20px" />
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
