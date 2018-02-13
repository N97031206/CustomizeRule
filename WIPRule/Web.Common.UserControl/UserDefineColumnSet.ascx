<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDefineColumnSet.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.UserDefineColumnSet" %>
<asp:GridView ID="gvQuery" runat="server" AutoGenerateColumns="False" CssClass="CSGridViewStyle"
    Width="99%" OnRowDataBound="gvQuery_RowDataBound"  ShowHeader="False">
    <Columns>
        <asp:TemplateField >
            <ItemTemplate>
                <asp:Label ID="lblCaption" runat="server" Text="Caption"></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="40%" Wrap="false" CssClass="CSTD" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:TextBox ID="ttbValue" runat="server"></asp:TextBox>
            </ItemTemplate>
            <ItemStyle Width="60%" Wrap="false" />
        </asp:TemplateField>
    </Columns>
    <HeaderStyle CssClass="CSGridHeader" HorizontalAlign="Center" />
    <RowStyle CssClass="CSGridItem" />
    <AlternatingRowStyle CssClass="CSGridAlterItem" />
    <SelectedRowStyle CssClass="CSGridSelectItem" />
    <EditRowStyle CssClass="CSGridEditItem" />
    <PagerStyle CssClass="CSGridPage" />
</asp:GridView>
