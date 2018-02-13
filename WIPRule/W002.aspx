<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W002.aspx.cs" Inherits="CustomizeRule.WIPRule.W002" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="W002" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width: 850px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblLotCheckInInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, LotCheckInInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 98%;">
                            <tr>
                                <td style="width: 2%;"></td>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, LotCheckInLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbLot" runat="server" AutoPostBack="true" OnTextChanged="ttbLot_TextChanged"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblQty" Text="<%$ Resources:Auto|RuleFace, Qty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 8%;"></td>
                            </tr>
                            <tr>
                                <td style="width: 2%;"></td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblEquip" Text="<%$ Resources:Auto|RuleFace, Equip %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbEquip" runat="server" ReadOnly="true" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 2%;"></td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblDevice" Text="<%$ Resources:Auto|RuleFace, MaterialId %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbDevice" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblDeviceDescr" Text="<%$ Resources:Auto|RuleFace, MaterialDesc %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbDeviceDescr" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 8%;"></td>
                            </tr>
                            <tr>
                                <td style="width: 2%;"></td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblRoute" Text="<%$ Resources:Auto|RuleFace, Route %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbRoute" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblOperation" Text="<%$ Resources:Auto|RuleFace, OperationStation %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                     <asp:TextBox ID="ttbOperation" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 8%;"></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset style="width: 100%;">
                        <legend>
                            <asp:Label ID="lblBadProduct" runat="server" Text="<%$ Resources:Auto|RuleFace, BadProduct %>"></asp:Label>
                        </legend>
                        <table style="width: 100%">
                            <tr>
                                <td class="CSTD" >
                                    <asp:Label ID="lblDefectReason" Text="<%$ Resources:Auto|RuleFace, DefectReason %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 23%;">
                                    <asp:DropDownList ID="ddlDefectReason" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="CSTD" colspan="2" >
                                    <asp:Label ID="lblDefectDesc" Text="<%$ Resources:Auto|RuleFace, DefectDesc %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 23%;">
                                     <asp:TextBox ID="ttbDefectDesc" runat="server" ></asp:TextBox>
                                </td>
                                <td style="width: 10%;">
                                     <CimesUI:CimesButton ID="CimesButtonAdd" runat="server" CssClass="CSOKButton"
                                     Text="<%$ Resources:Auto|RuleFace, Add %>" OnClick="btnAdd_Click"></CimesUI:CimesButton>
                                </td>
                                <td style="width: 1%;"></td>
                            </tr>
                            <tr>
                                <td class="CSTD">
                                    <asp:Label ID="lblComponentId" Text="<%$ Resources:Auto|RuleFace, ComponentId %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 23%;">
                                    <asp:DropDownList ID="ddlComponentId" runat="server" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlComponentId_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 23%;">
                                    <asp:TextBox ID="ttbComponentId" OnTextChanged="ttbComponentId_TextChanged" runat="server" AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td class="CSTD">
                                     <asp:Label ID="lblDefectQty" Text="<%$ Resources:Auto|RuleFace, DefectQty %>" runat="server"></asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="ttbDefectQty" runat="server" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="7">
                                    <div style="width: 98%; height: 250px; overflow: auto;">
                                        <CimesUI:CimesGridView ID="gvDefect" runat="server" Width="98%" AutoGenerateColumns="false"
                                            OnRowDeleting="gvDefect_RowDeleting" >
                                            <Columns>
                                                <CimesUI:CimesCommandField ShowDeleteButton="true" ChangeEditEventToSelect="true" HeaderStyle-Width="6%"></CimesUI:CimesCommandField>
                                                <asp:BoundField DataField="ComponentID" HeaderText="<%$ Resources:Auto|RuleFace, WorkpieceSerialNumber %>" HeaderStyle-Width="17%"/>
                                                <asp:BoundField DataField="Lot" HeaderText="<%$ Resources:Auto|RuleFace, WorkpieceLot %>" HeaderStyle-Width="17%"/>
                                                <asp:BoundField DataField="DefectReason" HeaderText="<%$ Resources:Auto|RuleFace, NGReason %>" HeaderStyle-Width="30%" />
                                                <asp:BoundField DataField="DefectDesc" HeaderText="<%$ Resources:Auto|RuleFace, NGReasonDesc %>" HeaderStyle-Width="30%" />
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
