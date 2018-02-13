<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T019.aspx.cs" Inherits="CustomizeRule.ToolRule.T019" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T019</title>
</head>
<body>
    <form id="T019" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 500px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblToolInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolPartsInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblToolName" Text="<%$ Resources:Auto|RuleFace, ToolParts %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 79%;">
                                    <asp:TextBox ID="ttbToolName" runat="server" AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <div style="height: 200px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvToolHead" runat="server" Width="95%" AutoGenerateColumns="false" OnRowDataBound="gvToolHead_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="Head" HeaderText="<%$ Resources:Auto|RuleFace, Head %>" HeaderStyle-Width="25%" />
                                                <asp:BoundField DataField="Life" HeaderText="<%$ Resources:Auto|RuleFace, Life %>" HeaderStyle-Width="25%"/>
                                                <asp:BoundField DataField="UseCount" HeaderText="<%$ Resources:Auto|RuleFace, UseCount %>" HeaderStyle-Width="25%"/>
                                                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ChangeLife %>" HeaderStyle-Width="25%" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                         <asp:TextBox ID="ttbChangeLife" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </CimesUI:CimesGridView>
                                    </div>
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
