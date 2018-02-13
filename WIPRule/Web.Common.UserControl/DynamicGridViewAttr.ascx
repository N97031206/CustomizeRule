<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicGridViewAttr.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.DynamicGridViewAttr" %>

<cimesui:cimesgridview id="gvGridViewAttr" runat="server" autogeneratecolumns="False"
    allowpaging="True" pagesize="10" width="100%" onrowcancelingedit="gvGridViewAttr_RowCancelingEdit"
    onrowediting="gvGridViewAttr_RowEditing" onpageindexchanging="gvGridViewAttr_PageIndexChanging"
    onrowupdating="gvGridViewAttr_RowUpdating" onrowdatabound="gvGridViewAttr_RowDataBound">
    <Columns>
        <CimesUI:CimesCommandField ShowDeleteButton="true" ShowEditButton="true" ShowCancelButton="true"
            ControlStyle-Width="10%">
            <ItemStyle HorizontalAlign="Center" />
        </CimesUI:CimesCommandField>
        <asp:BoundField DataField="Key" HeaderText="<%$ Resources:Auto|RuleFace, Attribute %>"
            ReadOnly="True">
            <HeaderStyle Width="20%" Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField SortExpression="AttributeValue" HeaderText="<%$ Resources:Auto|RuleFace, AttributeValue %>">
            <HeaderStyle Width="40%" Wrap="false"></HeaderStyle>
             <ItemStyle  Width="40%" Wrap="true"></ItemStyle>
           <ItemTemplate>
                <asp:Label ID="lblValue" Text='<%# Bind("AttributeValue") %>' runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="ttbValue" runat="server"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="AttributeDescr" HeaderText="<%$ Resources:Auto|RuleFace, Description %>"
            ReadOnly="True">
            <HeaderStyle Width="30%" Wrap="true" />
        </asp:BoundField>
    </Columns>
</cimesui:cimesgridview>
