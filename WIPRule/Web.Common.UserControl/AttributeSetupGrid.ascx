<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeSetupGrid.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.AttributeSetupGrid" %>
<cimesui:cimesgridview id="gvQuery" runat="server" autogeneratecolumns="false" enablecimesfilter="false"
    pagesize="13" width="99.9%" enablecimessort="false" onrowdatabound="gvQuery_RowDataBound"
    ondatasourcechanged="gvQuery_DataSourceChanged">
    <Columns>
    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, HasBeenSet %>">
            <HeaderStyle Wrap="False" />
            <ItemTemplate>
            <asp:Label ID="lblSetFlag" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle Wrap="False" HorizontalAlign="Center"  Width="5%" />
        </asp:TemplateField>
        <asp:BoundField DataField="AttributeName" HeaderText="<%$ Resources:Auto|RuleFace, AttributeName %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeValue %>">
            <HeaderStyle Wrap="False" />
            <ItemTemplate>
                <asp:TextBox ID="ttbChar" runat="server" Width="200px" TextMode="MultiLine"></asp:TextBox>
                <asp:DropDownList ID="ddlItemMaster" runat="server" Width="200px" CssClass="CSInput">
                </asp:DropDownList>
                <asp:RadioButton ID="rdbY" runat="server" GroupName="BooleanGroup" Text="Y" Width="60px"
                    Checked="True" />
                <asp:RadioButton ID="rdbN" runat="server" GroupName="BooleanGroup" Text="N" Width="60px" />
            </ItemTemplate>
            <ItemStyle Wrap="False" Width="200px" />
        </asp:TemplateField>
        <asp:BoundField DataField="DefaultValue" Visible="True" HeaderText="<%$ Resources:Auto|RuleFace, DefaultValue %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField DataField="RequestFlag" HeaderText="<%$ Resources:Auto|RuleFace, RequestFlag %>"
            Visible="false">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5%" />
        </asp:BoundField>
        <asp:BoundField DataField="CheckType" Visible="false" HeaderText="<%$ Resources:Auto|RuleFace, CheckType %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Description %>"
            DataField="Description" Visible="True">
            <HeaderStyle Wrap="False" />
            <ItemStyle Width="35%" />
        </asp:BoundField>
    </Columns>
  </cimesui:cimesgridview>
