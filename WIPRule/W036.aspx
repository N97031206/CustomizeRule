<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W036.aspx.cs" Inherits="CustomizeRule.WIPRule.W036" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>W036</title>
</head>
<body>
    <form id="W036" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <table style="width: 600px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td style="text-align:right">
                            <asp:Label ID="lblLot" runat="server" Text="<%$ Resources:Auto|RuleFace, BoxNO %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ttbWOLot" runat="server"  AutoPostBack="true" OnTextChanged="ttbWOLot_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <CimesUI:CimesButton ID="btnPrint" runat="server" CssClass="CSPrintButton" Text="<%$ Resources:Auto|RuleFace, Print %>"
                                OnClick="btnPrint_Click"></CimesUI:CimesButton>
                            <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" Text="<%$ Resources:Auto|RuleFace, Exit %>"
                                OnClick="btnCancel_Click"></CimesUI:CimesButton>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
