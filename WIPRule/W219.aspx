<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W219.aspx.cs" Inherits="CustomizeRule.WIPRule.W219" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W219</title>
</head>
<body>
    <form id="W219" runat="server">
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
                                <td class="CSTDMust">
                                    <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, MaterialLot %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbLot" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust">
                                    <asp:Label ID="lblDevice" Text="<%$ Resources:Auto|RuleFace, Device %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbDevice" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust">
                                    <asp:Label ID="lblQuantity" Text="<%$ Resources:Auto|RuleFace, Quantity %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbQuantity" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD">
                                    <asp:Label ID="lblLossItemSN" Text="<%$ Resources:Auto|RuleFace, LossItemSN %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbLossItemSN" runat="server"></asp:TextBox>
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
