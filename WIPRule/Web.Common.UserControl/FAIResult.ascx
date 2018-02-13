<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAIResult.ascx.cs" Inherits="ciMes.Web.Common.UserControl.FAIResult" %>
<asp:DataList ID="DataList1" runat="server" Width="99%" OnItemDataBound="DataList1_ItemDataBound">
    <AlternatingItemTemplate>
        <asp:Label ID="lblWorkOrder" runat="server" Text="" Style="text-align: center; display: block;"></asp:Label>
        <CimesUI:CimesGridView ID="GridView1" runat="server" AutoGenerateColumns="false"
            OnPreRender="GridView1_PreRender" Width="100%">
            <Columns>
                <asp:BoundField DataField="FAIDATE" HeaderText="<%$ Resources:Auto|RuleFace, FAIDATE %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FAITIME" HeaderText="<%$ Resources:Auto|RuleFace, FAITIME %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FAIRESULT" HeaderText="<%$ Resources:Auto|RuleFace, FAIRESULT %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CheckQty" HeaderText="<%$ Resources:Auto|RuleFace, CheckQty %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="REMARK" HeaderText="<%$ Resources:Auto|RuleFace, REMARK %>" />
            </Columns>
            
        </CimesUI:CimesGridView>
        <p />
    </AlternatingItemTemplate>
    <ItemTemplate>
        <asp:Label ID="lblWorkOrder" runat="server" Text="" Style="text-align: center; display: block;"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" CssClass="CSGridViewStyle" AutoGenerateColumns="false"
            OnPreRender="GridView1_PreRender" Width="100%">
            <Columns>
                <asp:BoundField DataField="FAIDATE" HeaderText="<%$ Resources:Auto|RuleFace, FAIDATE %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FAITIME" HeaderText="<%$ Resources:Auto|RuleFace, FAITIME %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FAIRESULT" HeaderText="<%$ Resources:Auto|RuleFace, FAIRESULT %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CheckQty" HeaderText="<%$ Resources:Auto|RuleFace, CheckQty %>"
                    ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="REMARK" HeaderText="<%$ Resources:Auto|RuleFace, REMARK %>" />
            </Columns>
            <HeaderStyle CssClass="CSGridHeader" />
            <AlternatingRowStyle CssClass="CSGridAlterItem" />
            <RowStyle CssClass="CSGridItem" />
        </asp:GridView>
        <p />
    </ItemTemplate>
</asp:DataList>