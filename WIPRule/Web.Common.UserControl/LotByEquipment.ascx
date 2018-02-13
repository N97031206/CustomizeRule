<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LotByEquipment.ascx.cs" Inherits="ciMes.Web.Common.UserControl.LotByEquipment" %>
<CimesUI:CimesGridView ID="gvQuery" runat="server" Width="99%" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="Lot" HeaderText="<%$ Resources:Auto|RuleFace, Lot %>" />
        <asp:BoundField DataField="Product" HeaderText="<%$ Resources:Auto|RuleFace, Product %>" />
        <asp:BoundField DataField="Device" HeaderText="<%$ Resources:Auto|RuleFace, Device %>" />
        <asp:BoundField DataField="UserID" HeaderText="<%$ Resources:Auto|RuleFace, UserID %>" />
        <asp:BoundField DataField="UpdateTime" HeaderText="<%$ Resources:Auto|RuleFace, StartWorkTime %>" />
    </Columns>
</CimesUI:CimesGridView>
