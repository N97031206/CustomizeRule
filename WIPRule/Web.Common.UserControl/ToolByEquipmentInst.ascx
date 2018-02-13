<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ToolByEquipmentInst.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.ToolByEquipmentInst" %>
<CimesUI:CimesGridView ID="gvQuery" runat="server" Width="99%" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="ToolName" HeaderText="<%$ Resources:Auto|RuleFace, ToolName %>" />
        <asp:BoundField DataField="UpdateTime" HeaderText="<%$ Resources:Auto|RuleFace, LoadToolTime %>" />
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, LoadToolUser %>">
            <ItemTemplate>
                <asp:Label ID="lblUserID" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</CimesUI:CimesGridView>
