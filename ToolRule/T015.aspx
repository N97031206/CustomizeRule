<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T015.aspx.cs" Inherits="CustomizeRule.ToolRule.T015" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="T015" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc2:programinformationblock id="ProgramInformationBlock1" runat="server" />
                <table style="margin-left: auto; margin-right: auto; width: 850px;">
                    <tr>
                        <td colspan="2">
                            <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="lblBaseInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, BaseInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblEquipment" runat="server" Text="<%$ Resources:Auto|RuleFace, EquipmentName %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbEquipmentName" runat="server" OnTextChanged="ttbEquipmentName_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblDevice1" runat="server" Text="<%$ Resources:Auto|RuleFace, Device %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbDevice1" runat="server" OnTextChanged="ttbDevice1_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblDevice2" runat="server" Text="<%$ Resources:Auto|RuleFace, Device %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbDevice2" runat="server" OnTextChanged="ttbDevice2_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; vertical-align:top;">
                            <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="lblVerifiy1" runat="server" Text="<%$ Resources:Auto|RuleFace, VerifiyInfo %>"></asp:Label>
                                </legend>
                            <div style="height:380px; overflow:auto;">
                                <cimesui:cimesgridview id="gvVerifiy1" runat="server" autogeneratecolumns="False" width="100%" OnRowDataBound="gvVerifiy_RowDataBound">
                                    <columns>
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Operation %>" DataField="Operation" />
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Milltype %>" DataField="TOOLTYPE" />
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, NeedQty %>" DataField="NEEDQTY" />
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, UseQty %>" DataField="EQPTOOLCOUNT" />
                                    </columns>
                                </cimesui:cimesgridview>
                            </div>
                            </fieldset>
                        </td>
                        <td style="width: 50%; vertical-align:top;">
                            <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Auto|RuleFace, VerifiyInfo %>"></asp:Label>
                                </legend>
                            <div style="height:380px; overflow:auto;">
                                <cimesui:cimesgridview id="gvVerifiy2" runat="server" autogeneratecolumns="False" width="100%" OnRowDataBound="gvVerifiy_RowDataBound">
                                    <columns>
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Milltype %>" DataField="TOOLTYPE" />
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, NeedQty %>" DataField="NEEDQTY" />
                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, UseQty %>" DataField="EQPTOOLCOUNT" />
                                    </columns>
                                </cimesui:cimesgridview>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
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
