<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T011.aspx.cs" Inherits="CustomizeRule.ToolRule.T011" %>

<%@ Register Assembly="Ares.Cimes.IntelliService.Web" Namespace="Ares.Cimes.IntelliService.Web.UI" TagPrefix="CimesUI" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="T011" runat="server">
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
                                    <asp:Label ID="lblTakeBaseInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, TakeBaseInfo %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblTakeUserName" runat="server" Text="<%$ Resources:Auto|RuleFace, TakeUserName %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbTakeUserName" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblTakeQuantity" runat="server" Text="<%$ Resources:Auto|RuleFace, TakeQuantity %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbTakeQuantity" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTD">
                                            <asp:Label ID="lblTakeDate" runat="server" Text="<%$ Resources:Auto|RuleFace, TakeDate %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbTakeDate" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblTakeReason" runat="server" Text="<%$ Resources:Auto|RuleFace, TakeReason %>"></asp:Label>
                                        </td>
                                        <td>
                                            <cimesui:csreason labelvisible="false" id="csReason" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblEquipment" runat="server" Text="<%$ Resources:Auto|RuleFace, TakeEquipment %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbEquipment" runat="server" OnTextChanged="ttbEquipment_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblOperation" runat="server" Text="<%$ Resources:Auto|RuleFace, Operation %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOperation" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblMillName" runat="server" Text="<%$ Resources:Auto|RuleFace, MillNumber %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbMillName" runat="server" OnTextChanged="ttbMillName_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                        <td class="CSTD">
                                            <asp:Label ID="lblIdentity" runat="server" Text="<%$ Resources:Auto|RuleFace, Identity %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ttbIdentity" runat="server" ReadOnly="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="CSTDMust">
                                            <asp:Label ID="lblMillHeader" runat="server" Text="<%$ Resources:Auto|RuleFace, MillHeader %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMillHeader" runat="server"></asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <cimesui:cimesbutton id="btnAdd" runat="server" cssclass="CSOKButton"
                                                text="<%$ Resources:Auto|RuleFace, Add %>" onclick="btnAdd_Click"></cimesui:cimesbutton>
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
                                    <asp:Label ID="lblMillList" runat="server" Text="<%$ Resources:Auto|RuleFace, MillList %>"></asp:Label>
                                </legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 98%; vertical-align: top;">
                                            <div style="height: 280px; overflow: auto;">
                                                <cimesui:cimesgridview id="gvMillHeader" runat="server" autogeneratecolumns="False" width="100%" onrowdeleting="gvMillHeader_RowDeleting" onrowdatabound="gvMillHeader_RowDataBound">
                                                    <columns>
                                                        <CimesUI:CimesCommandField ShowEditButton="false" ShowDeleteButton="true" ShowCancelButton="false"  />
                                                        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Operation %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOperation" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, MillName %>" DataField="ToolName" />
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, MillHeader %>" DataField="HEAD" />
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, MillLife %>" DataField="LIFE" />
                                                        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, UseCount %>" DataField="USECOUNT" />
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
