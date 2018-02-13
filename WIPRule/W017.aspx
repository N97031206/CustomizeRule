<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W017.aspx.cs" Inherits="CustomizeRule.WIPRule.W017" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W017</title>
</head>
<body>
    <form id="W017" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 600px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblDefectInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, DefectInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblDefectINo" Text="<%$ Resources:Auto|RuleFace, DefectNo %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 80%;" colspan ="3">
                                        <asp:TextBox ID="ttbDefectNo" runat="server" ReadOnly="true" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblDefectCount" Text="<%$ Resources:Auto|RuleFace,DefectLotCount %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                        <asp:TextBox ID="ttbDefectCount" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                    <td class="CSTD" style="width: 20%;">
                                    <asp:Label ID="lblDefectQty" Text="<%$ Resources:Auto|RuleFace, DefectQuantity %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                        <asp:TextBox ID="ttbDefectQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblDefectList" runat="server" Text="<%$ Resources:Auto|RuleFace,DefectList %>"></asp:Label>
                        </legend>
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <div style="width: 100%; height: 300px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvDefect" runat="server" Width="95%" AutoGenerateColumns="false" OnRowDataBound="gvDefect_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Auto|RuleFace, Item %>" HeaderStyle-Width="10%" HeaderStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ckbDefectSelect" runat="server" AutoPostBack="true" OnCheckedChanged="ckbDefectSelect_CheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Lot" HeaderText="<%$ Resources:Auto|RuleFace, WorkpieceLot %>" HeaderStyle-Width="40%" />
                                                <asp:BoundField DataField="Quantity" HeaderText="<%$ Resources:Auto|RuleFace, Qty %>" HeaderStyle-Width="40%"/>
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