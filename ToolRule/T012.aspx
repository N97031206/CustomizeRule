<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T012.aspx.cs" Inherits="CustomizeRule.ToolRule.T012" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="T012" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc2:programinformationblock id="ProgramInformationBlock1" runat="server" />
                <table style="margin-left: auto; margin-right: auto; width: 850px;">
                    <tr>
                        <td>
                            <fieldset class="CSFieldset">
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

                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
    <%--                        <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="lblMillList" runat="server" Text="<%$ Resources:Auto|RuleFace, MillList %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 98%; vertical-align: top;">--%>
                                            <div style="width: 1000px; overflow: auto; height: 400px; margin-left: auto; margin-right: auto;">
                                                <cimesui:cimesgridview id="gvQuery" runat="server" autogeneratecolumns="False" width="100%">
                                                    <columns>
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, ToolParts %>" DataField="ToolName" />
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, ToolTypes %>" DataField="ToolType" />
                                                        <asp:BoundField DataField="UserDefineColumn08" HeaderText="<%$ Resources:Auto|RuleFace, Operation %>" />
                                                    </columns>
                                                </cimesui:cimesgridview>
                                            </div>
      <%--                                  </td>
                                    </tr>
                                </table>
                            </fieldset>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
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
