<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipmentInfoBlock.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.EquipmentInfoBlock" %>
<div style="width: 64.5%; float: left; border-width: 0px;">
    <div style="width: 80px; float: left; overflow: hidden;">
        <div style="text-align: center;">
            <asp:Image ID="Image1" runat="server" BorderStyle="Solid" Width="75px" Height="75px"
                ImageUrl="~/Images/noimage.png" BorderWidth="1px" />
        </div>
        <div>
            <asp:Label ID="lblManual" runat="server" Text="<%$ Resources:Auto|RuleFace, Manual %>"
                Style="float: left; width: 40px; display: inline;"></asp:Label>
            <asp:Label ID="lblAutomatic" runat="server" Text="<%$ Resources:Auto|RuleFace, Automatic %>"
                Style="float: right; width: 40px; display: inline; text-align: right;"></asp:Label>
        </div>
        <div>
            <div id="divCurrentState" runat="server" style="display: table-cell; width: 100%;
                margin-top: 2px; height: 22px; vertical-align: middle;">
            </div>
        </div>
    </div>
    <div style="float: left; width: 405px;">
        <table width="100%">
            <tr>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblEquipmentName" runat="server" Text="<%$ Resources:Auto|RuleFace, Name %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbEquipmentName" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
                    <!-- 此頁面的QueryButton不需換成CimesButton -->
                    <asp:Button ID="btnMoreDetail" runat="server" Text="" CssClass="CSQueryDetailButton"
                        Style="float: right;" />
                </td>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblWorkOrder" runat="server" Text="<%$ Resources:Auto|RuleFace, WO %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbWorkOrder" runat="server" ReadOnly="true" Width="90px" Style="float: left;"></asp:TextBox>
                    <asp:Button ID="btnWorkOrderQuery" runat="server" Text="" CssClass="CSQueryDetailButton"
                        Style="float: right;" />
                </td>
            </tr>
            <tr>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblEquipmentType" runat="server" Text="<%$ Resources:Auto|RuleFace, Type %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbEquipmentType" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblEquipmentTool" runat="server" Text="<%$ Resources:Auto|RuleFace, Tool %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbEquipmentTool" runat="server" ReadOnly="true" Width="90px" Style="float: left;"></asp:TextBox>
                    <asp:Button ID="btnEquipmentToolQuery" runat="server" Text="" CssClass="CSQueryDetailButton"
                        Style="float: right;" />
                </td>
            </tr>
            <tr>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblFAIResult" runat="server" Text="<%$ Resources:Auto|RuleFace, FAIResult %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbFAIResult" runat="server" ReadOnly="true" Width="90px" Style="float: left;"></asp:TextBox>
                    <asp:Button ID="btnFAIResultQuery" runat="server" Text="" CssClass="CSQueryDetailButton"
                        Style="float: right;" />
                </td>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblDefectQty" runat="server" Text="<%$ Resources:Auto|RuleFace, DefectQuantity %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbDefectQty" runat="server" ReadOnly="true" Width="90px" Style="float: left;"></asp:TextBox>
                    <asp:Button ID="btnDefectQtyQuery" runat="server" Text="" CssClass="CSQueryDetailButton"
                        Style="float: right;" />
                </td>
            </tr>
            <tr>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblTodayOutQty" runat="server" Text="<%$ Resources:Auto|RuleFace, TodayOutQuantity %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbTodayOutQty" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="CSTD" style="width: 19%;">
                    <asp:Label ID="lblCurrentLotCount" runat="server" Text="<%$ Resources:Auto|RuleFace, CurrentLotCount %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbCurrentLotCount" runat="server" ReadOnly="true" Width="90px"
                        Style="float: left;"></asp:TextBox>
                    <asp:Button ID="btnCurrentLotCountQuery" runat="server" Text="" CssClass="CSQueryDetailButton"
                        Style="float: right;" />
                </td>
            </tr>
        </table>
    </div>
</div>