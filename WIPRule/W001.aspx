<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="W001.aspx.cs" Inherits="CustomizeRule.WIPRule.W001" %>

<%@ Register Src="~/Web.Common.UserControl/ProgramInformationBlock.ascx" TagName="ProgramInformationBlock" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LotCheckIn</title>
</head>
<body>
    <form id="LotCheckIn" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
        <asp:Updatepanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:ProgramInformationBlock ID="ProgramInformationBlock1" runat="server" />
                <div style="margin: 0 auto; width:700px;">
                    <fieldset style="width: 100%;">
                        <legend>
                           <asp:Label ID="lblCheckInInfo" runat="server" Text="<%$ Resources:Auto|RuleFace, CheckInInfo %>"></asp:Label>
                        </legend>
                        <table style="width: 97%;">
                            <tr>
                                <td class="CSTDMust" style="width: 15%;">
                                    <asp:Label ID="lblLot" Text="<%$ Resources:Auto|RuleFace, LotCheckInLot %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbLot" runat="server" AutoPostBack="true" OnTextChanged="ttbLot_TextChanged"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblQty" Text="<%$ Resources:Auto|RuleFace, Qty %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbQty" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblEquip" Text="<%$ Resources:Auto|RuleFace, Equip %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                    <asp:DropDownList ID="ddlEquip" runat="server"></asp:DropDownList>
                                </td>
                                <%--<td style="width: 35%;">
                                     <asp:TextBox ID="ttbEquip" runat="server" AutoPostBack="true" OnTextChanged="ttbEquip_TextChanged"></asp:TextBox>
                                </td>--%>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblDevice" Text="<%$ Resources:Auto|RuleFace, MaterialId %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbDevice" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblDeviceDescr" Text="<%$ Resources:Auto|RuleFace, MaterialDesc %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbDeviceDescr" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblRoute" Text="<%$ Resources:Auto|RuleFace, Route %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbRoute" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="CSTD" style="width: 15%;">
                                    <asp:Label ID="lblOperation" Text="<%$ Resources:Auto|RuleFace, OperationStation %>" runat="server"></asp:Label>
                                </td>
                                <td style="width: 35%;">
                                     <asp:TextBox ID="ttbOperation" runat="server" ReadOnly="true"></asp:TextBox>
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
