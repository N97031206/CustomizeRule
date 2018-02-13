<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W043.aspx.cs" Inherits="CustomizeRule.WIPRule.W043" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>UnPack</title>
</head>
<body>
    <form id="UnPack" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc2:programinformationblock id="ProgramInformationBlock1" runat="server" />
                <table style="margin-left: auto; margin-right: auto; width: 700px;">
                    <tr>
                        <td>
                            <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="lblBaseInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, PackInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblUnPackingInventoryLot" runat="server" Text="<%$ Resources:Auto|RuleFace, UnPackingInventoryLot %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbBoxNo" runat="server" OnTextChanged="ttbBoxNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset class="CSFieldset">
                                <legend>
                                    <asp:Label ID="lblComponentInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ComponentInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <div style="height: 355px; overflow: auto;">
                                                <cimesui:cimesgridview id="gvComponent" runat="server" autogeneratecolumns="False" width="100%">
                                                    <columns>
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Lot %>" DataField="CurrentLot" />
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Component %>" DataField="ComponentID" />
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, ComponentQuantity %>" DataField="ComponentQuantity" />
                                                    </columns>
                                                </cimesui:cimesgridview>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
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
