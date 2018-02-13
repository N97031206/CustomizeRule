<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W044.aspx.cs" Inherits="CustomizeRule.WIPRule.W044" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>W044</title>
</head>
<body>
    <form id="W044" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc2:programinformationblock id="ProgramInformationBlock1" runat="server" />
                <table style="margin-left: auto; margin-right: auto; width: 700px;">
                    <tr>
                        <td class="CSTDMust">
                            <asp:Label ID="lblWOLot" runat="server" Text="<%$ Resources:Auto|RuleFace, WOLot %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ttbWOLot" runat="server" AutoPostBack="true" OnTextChanged="ttbWOLot_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CSTDMust">
                            <asp:Label ID="lblttbWorkpiece" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceSerialNumber %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ttbWorkpiece" runat="server" AutoPostBack="true" OnTextChanged="ttbWorkpiece_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CSTDMust">
                            <asp:Label ID="lblOperation" runat="server" Text="<%$ Resources:Auto|RuleFace, Operation %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOperation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CSTDMust">
                            <asp:Label ID="lblDefectReason" Text="<%$ Resources:Auto|RuleFace, DefectReason %>" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDefectReason" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CSTD">
                            <asp:Label ID="lblDefectDesc" Text="<%$ Resources:Auto|RuleFace, DefectDesc %>" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ttbDefectDesc" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <cimesui:cimesbutton id="btnOK" runat="server" cssclass="CSOKButton"
                                text="<%$ Resources:Auto|RuleFace, OK %>" onclick="btnOK_Click"></cimesui:cimesbutton>

                            <cimesui:cimesbutton id="btnCancel" runat="server" cssclass="CSCancelButton" onclick="btnCancel_Click"
                                text="<%$ Resources:Auto|RuleFace, Exit %>" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
