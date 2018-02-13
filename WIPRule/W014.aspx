<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W014.aspx.cs" Inherits="CustomizeRule.WIPRule.W014" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Packing</title>
</head>
<body>
    <form id="Packing" runat="server">
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
                                    <asp:Label ID="lblBaseInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, DeviceInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblDeviceName" runat="server" Text="<%$ Resources:Auto|RuleFace, DeviceName %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbDeviceName" runat="server" OnTextChanged="ttbDeviceName_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblPackType" runat="server" Text="<%$ Resources:Auto|RuleFace, PackType %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbPackType" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblRelativeDevice" runat="server" Text="<%$ Resources:Auto|RuleFace, RelativeDevice %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbRelativeDevice" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblMaxPackSize" runat="server" Text="<%$ Resources:Auto|RuleFace, MaxPackSize %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbMaxPackSize" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblWorkpiece" runat="server" Text="DMC"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbWorkpiece" runat="server" OnTextChanged="ttbWorkpiece_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblInspector" runat="server" Text="<%$ Resources:Auto|RuleFace, Inspector %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInspector" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:CheckBox ID="ckbNoControl" runat="server" Text="<%$ Resources:Auto|RuleFace, NoControl %>" AutoPostBack="true" OnCheckedChanged="ckbNoControl_CheckedChanged" />
                                        </td>
                                        <td></td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblTempWorkpiece" runat="server" Text="<%$ Resources:Auto|RuleFace, TempWorkpiece %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbTempWorkpiece" runat="server" AutoPostBack="true" OnTextChanged="ttbTempWorkpiece_TextChanged"></asp:TextBox>
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
                                    <asp:Label ID="lblWorkpieceList" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceList %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%; vertical-align: top;">
                                            <div style="height: 320px; overflow: auto;">
                                                <cimesui:cimesgridview id="gvWorkpiece" runat="server" autogeneratecolumns="False" width="100%"
                                                    onrowdeleting="gvWorkpiece_RowDeleting">
                                                <columns>
                                                    <CimesUI:CimesCommandField ShowEditButton="false" ShowDeleteButton="true" ShowCancelButton="false"  />
                                                    <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Item %>" DataField="Item" />
                                                    <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Device %>" DataField="Device" />  
                                                     <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, DMC %>" DataField="DMC" />                                                    
                                                </columns>
                                                </cimesui:cimesgridview>
                                            </div>
                                        </td>
                                        <td style="width: 50%; vertical-align: top;">
                                            <div style="height: 320px; overflow: auto;">
                                                <cimesui:cimesgridview id="gvRelativeWorkpiece" runat="server" autogeneratecolumns="False" width="100%"
                                                    onrowdeleting="gvRelativeWorkpiece_RowDeleting">
                                                <columns>
                                                    <CimesUI:CimesCommandField ShowEditButton="false" ShowDeleteButton="true" ShowCancelButton="false" />
                                                    <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Item %>" DataField="Item" />
                                                    <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, RelativeDevice %>" DataField="Device" /> 
                                                     <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, DMC %>" DataField="DMC" />                                                          
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
                            <cimesui:cimesbutton id="btnTempSave" runat="server" cssclass="CSSaveButton"
                                text="<%$ Resources:Auto|RuleFace, TempSave %>" onclick="btnTempSave_Click"></cimesui:cimesbutton>
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
