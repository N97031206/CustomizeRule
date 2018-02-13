<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W102.aspx.cs" Inherits="CustomizeRule.WIPRule.W102" %>
<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>W102</title>
</head>
<body>
    <form id="W102" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width:800px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblWorkpieceInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblCompLot" Text="<%$ Resources:Auto|RuleFace, ItemSN %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbCompLot" runat="server" AutoPostBack="true" OnTextChanged="ttbCompLot_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblWOLot" Text="<%$ Resources:Auto|RuleFace, RuncardLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                    <asp:TextBox ID="ttbWOLot" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                 <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblOperation" Text="<%$ Resources:Auto|RuleFace, Operation %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbOperation" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblWOQty" Text="<%$ Resources:Auto|RuleFace, WOQty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbWOQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblMergeQty" Text="<%$ Resources:Auto|RuleFace, MergeQty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbMergeQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblNotMergeQty" Text="<%$ Resources:Auto|RuleFace, NotMergeQty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbNotMergeQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblHaveMergeQty" Text="<%$ Resources:Auto|RuleFace, HaveMergeQty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbHaveMergeQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblRoute" Text="<%$ Resources:Auto|RuleFace, Route %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                    <asp:DropDownList ID="ddlRoute" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblRouteOperation" Text="<%$ Resources:Auto|RuleFace, RouteOperation %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                    <asp:DropDownList ID="ddlRouteOperation" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblWorkpieceList" runat="server" Text="<%$ Resources:Auto|RuleFace, WorkpieceList %>"></asp:Label>
                        </legend>
                         <table style="width: 100%;">
                            <tr>
                                <td style="width: 3%;"></td>
                                <td style="width: 30%; vertical-align: top;">
                                    <div style="width: 100%; height: 150px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvMergeCompLot" runat="server" Width="95%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="ComponentLot" HeaderText="<%$ Resources:Auto|RuleFace, MergeSN %>" HeaderStyle-Width="100%"/>
                                            </Columns>
                                        </CimesUI:CimesGridView>
                                    </div>
                                </td>
                                <td style="width: 10%;"></td>
                                <td style="width: 60%; vertical-align: top;">
                                    <div style="width: 100%; height: 150px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvNotMergeCompLot" runat="server" Width="95%" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundField DataField="ComponentLot" HeaderText="<%$ Resources:Auto|RuleFace, NotMergeSN %>" HeaderStyle-Width="40%"/>
                                                <asp:BoundField DataField="OperationName" HeaderText="<%$ Resources:Auto|RuleFace, Operation %>" HeaderStyle-Width="40%"/>
                                                <asp:BoundField DataField="Status" HeaderText="<%$ Resources:Auto|RuleFace, Status %>" HeaderStyle-Width="20%"/>
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
