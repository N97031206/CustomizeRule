<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W006.aspx.cs" Inherits="CustomizeRule.WIPRule.W006" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>W006</title>
</head>
<body>
    <form id="W006" method="post" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <table style="width: 400px; margin-left: auto; margin-right: auto;">
                    <tr>
                            <td style="text-align:right; width:20%" >
                            <asp:Label ID="lblSearchCondition" runat="server" Text="<%$ Resources:Auto|RuleFace, SearchCondition %>"></asp:Label>
                        </td>
                        <td style="text-align:left; width:80%">
                            <asp:RadioButton ID="cbxSN" runat="server" Text="<%$ Resources:Auto|RuleFace, SN %>" GroupName="SearchType"/>
                            <asp:RadioButton ID="cbxLot" runat="server" Text="<%$ Resources:Auto|RuleFace, Lot %>" GroupName="SearchType" />
                            <asp:RadioButton ID="cbxWO" runat="server" Text="<%$ Resources:Auto|RuleFace, Workorder %>" GroupName="SearchType" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:right; width:20%">
                            <asp:Label ID="lblLot" runat="server" Text="<%$ Resources:Auto|RuleFace, SearchData %>"></asp:Label>
                        </td>
                        <td style="width:80%">
                            <asp:TextBox ID="ttbWOLot" runat="server" AutoPostBack="true" OnTextChanged="ttbWOLot_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <cimesui:cimesbutton id="btnPrint" runat="server" cssclass="CSPrintButton" text="<%$ Resources:Auto|RuleFace, Print %>"
                                onclick="btnPrint_Click"></cimesui:cimesbutton>
                            <cimesui:cimesbutton id="btnCancel" runat="server" cssclass="CSCancelButton" text="<%$ Resources:Auto|RuleFace, Exit %>"
                                onclick="btnCancel_Click"></cimesui:cimesbutton>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
