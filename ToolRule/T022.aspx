<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T022.aspx.cs" Inherits="CustomizeRule.ToolRule.T022" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>T022</title>
</head>
<body>
    <form id="T022" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <table style="width: 600px; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td>
                            <asp:Label ID="lblMillName" runat="server" Text="<%$ Resources:Auto|RuleFace, MillName %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ttbToolName" runat="server"  AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
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
