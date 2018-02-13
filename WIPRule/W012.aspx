<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W012.aspx.cs" Inherits="CustomizeRule.WIPRule.W012" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W012</title>
</head>
<body>
    <form id="W012" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width:900px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblFailInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, FailInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 100%;">
                            <tr>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, ProductionNumber %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbLot" runat="server" AutoPostBack="true" OnTextChanged="ttbLot_TextChanged"></asp:TextBox>
                                </td>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblJudgeDefect" Text="<%$ Resources:Auto|RuleFace, JudgeDefect %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <%--<asp:TextBox ID="ttbJudgeDefect" runat="server" ReadOnly="true"></asp:TextBox>--%>
                                    <asp:DropDownList ID="ddlJudgeDefect" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlJudgeDefect_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblItemSN" Text="<%$ Resources:Auto|RuleFace, ItemSN %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbItemSN" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                 <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblQty" Text="<%$ Resources:Auto|RuleFace, Qty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblJudgeResult" Text="<%$ Resources:Auto|RuleFace, JudgeResult %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 85%;" colspan ="3">
                                    <asp:RadioButton ID="rdbGoods" runat="server" Text="<%$ Resources:Auto|RuleFace, Goods %>" GroupName="JudgeResult"/>
                                    <asp:RadioButton ID="rdbRepair" runat="server" Text="<%$ Resources:Auto|RuleFace, Repair %>" GroupName="JudgeResult" />
                                    <asp:RadioButton ID="rdbDefectInv" runat="server" Text="<%$ Resources:Auto|RuleFace, DefectInv %>" GroupName="JudgeResult" />
                                    <asp:RadioButton ID="rdbScrapInv" runat="server" Text="<%$ Resources:Auto|RuleFace, ScrapInv %>" GroupName="JudgeResult" />
                                </td>
                                <td style="width: 20%;"></td>
                                <td style="width: 30%;"></td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblJudgeReason" Text="<%$ Resources:Auto|RuleFace, JudgeReason %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                    <asp:DropDownList ID="ddlJudgeReason" runat="server"></asp:DropDownList>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblJudgeDescr" Text="<%$ Resources:Auto|RuleFace, JudgeDescr %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbJudgeDescr" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <table style="width: 100%;">
                        <tr>
                            <td align="center">
                                <CimesUI:CimesButton ID="btnOK" runat="server" CssClass="CSOKButton"
                                        Text="<%$ Resources:Auto|RuleFace, OK %>" OnClick="btnOK_Click"></CimesUI:CimesButton>
                                <CimesUI:CimesButton ID="btnPrint" runat="server" CssClass="CSPrintButton"
                                        Text="<%$ Resources:Auto|RuleFace, Print %>" OnClick="btnPrint_Click"></CimesUI:CimesButton>
                                <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" 
                                        Text="<%$ Resources:Auto|RuleFace, Exit %>" OnClick="btnCancel_Click"></CimesUI:CimesButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

