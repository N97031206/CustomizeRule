<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOByEquipmentInst.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.WOByEquipmentInst" %>
<cimesui:cimesgridview id="gvQuery" runat="server" width="99%"
    autogeneratecolumns="False">
    <Columns>
        <asp:BoundField DataField="WO" HeaderText="<%$ Resources:Auto|RuleFace, WorkOrder %>" />
        <asp:BoundField DataField="UpdateTime" HeaderText="<%$ Resources:Auto|RuleFace, StartWorkTime %>" />
    </Columns>
</cimesui:cimesgridview>
