<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W029.aspx.cs" Inherits="CustomizeRule.QCRule.W029" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W029</title>
</head>
<body>
    <form id="W029" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 600px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblEquipInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, EquipInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 20%;">
                                    <asp:Label ID="lblEquipOrCompLot" Text="<%$ Resources:Auto|RuleFace, EquipOrCompLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;" colspan ="3">
                                     <asp:TextBox ID="ttbEquipOrCompLot" runat="server" AutoPostBack="true" OnTextChanged="ttbEquipOrCompLot_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>    
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblQCList" runat="server" Text="<%$ Resources:Auto|RuleFace,QCList %>"></asp:Label>
                        </legend>
                        <table style="width: 100%;">
                             <tr>
                                <td align="center">
                                    <div style="height: 335px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvQC" runat="server" Width="95%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="Equipment" HeaderText="<%$ Resources:Auto|RuleFace, Equip %>" HeaderStyle-Width="25%" />
                                                <asp:BoundField DataField="Device" HeaderText="<%$ Resources:Auto|RuleFace, Device %>" HeaderStyle-Width="25%"/>
                                                <asp:BoundField DataField="SN" HeaderText="<%$ Resources:Auto|RuleFace, SN %>" HeaderStyle-Width="25%"/>
                                                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Auto|RuleFace, Status %>" HeaderStyle-Width="25%"/>
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
