<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="M007.aspx.cs" Inherits="CustomizeRule.MMSRule.M007" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>M007</title>
</head>
<body>
    <form id="M007" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width:900px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblCheckInInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, Info %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 10%;">
                                    <asp:Label ID="lblIQCRunID" Text="<%$ Resources:Auto|RuleFace, IQCRunID %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                     <asp:TextBox ID="ttbIQCRunID" runat="server" ></asp:TextBox>
                                </td>
                                <td class="CSTDMust" style="width: 10%;">
                                    <asp:Label ID="lblIQCLot" Text="<%$ Resources:Auto|RuleFace, IQCLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                     <asp:TextBox ID="ttbIQCLot" runat="server" AutoPostBack="true" OnTextChanged="ttbIQCLot_TextChanged"></asp:TextBox>
                                </td>
                                <td class="CSTDMust" style="width: 10%;">
                                    <asp:Label ID="lblIQCTime" Text="<%$ Resources:Auto|RuleFace, IQCTime %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                    <asp:DropDownList ID="ddlIQCTime" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIQCTime_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="CSTD" style="width: 10%;">
                                    <asp:Label ID="lblOldQuantity" Text="<%$ Resources:Auto|RuleFace, OldQuantity %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                     <asp:TextBox ID="ttbOldQuantity" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTDMust" style="width: 10%;">
                                    <asp:Label ID="lblNewQuantity" Text="<%$ Resources:Auto|RuleFace, ModifyQuantity %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 25%;">
                                     <asp:TextBox ID="ttbNewQuantity" runat="server" AutoPostBack="true" OnTextChanged="ttbNewQuantity_TextChanged"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
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
