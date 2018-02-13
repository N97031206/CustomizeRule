<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefectDetailByEquipmentInst.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.DefectDetailByEquipmentInst" %>
<CimesUI:CimesGridView ID="gvQuery" runat="server" Width="99%"
    AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="REASONCODE" HeaderText="<%$ Resources:Auto|RuleFace, REASONCODE %>" />
        <asp:BoundField DataField="DESCR" HeaderText="<%$ Resources:Auto|RuleFace, Description %>" />
        <asp:BoundField DataField="DEFECTQTY" HeaderText="<%$ Resources:Auto|RuleFace, DEFECTQTY %>" />
    </Columns>
</CimesUI:CimesGridView>
