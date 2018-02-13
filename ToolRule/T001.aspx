<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T001.aspx.cs" Inherits="CustomizeRule.ToolRule.T001" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>T001</title>
</head>
<body>
    <form id="T001" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 500px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblToolInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, ToolInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust">
                                    <asp:Label ID="lblToolName" Text="<%$ Resources:Auto|RuleFace, ToolName %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbToolName" runat="server" AutoPostBack="true" OnTextChanged="ttbToolName_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD">
                                    <asp:Label ID="lblToolType" Text="<%$ Resources:Auto|RuleFace, ToolType %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbToolType" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD">
                                    <asp:Label ID="lblToolDescr" Text="<%$ Resources:Auto|RuleFace, ToolDescription %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbToolDescr" runat="server" ReadOnly="true" Height="60px" TextMode="MultiLine" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD">
                                    <asp:Label ID="lblChangeLine" Text="<%$ Resources:Auto|RuleFace, ChangeLine %>" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ttbChangeLine" runat="server" AutoPostBack="true" OnTextChanged="ttbChangeLine_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblChangeWO" runat="server" Text="<%$ Resources:Auto|RuleFace, ChangeWO %>"></asp:Label>
                        </legend>
                         <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <div style="width: 100%; height: 150px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvChangeWO" runat="server" Width="95%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="WORKRODER" HeaderText="<%$ Resources:Auto|RuleFace, WorkOrder %>" HeaderStyle-Width="15%"/>
                                                <asp:BoundField DataField="DEVICE" HeaderText="<%$ Resources:Auto|RuleFace, Model %>" HeaderStyle-Width="15%"/>
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
