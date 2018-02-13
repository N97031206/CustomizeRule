<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LightAttributeSetupGrid.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.LightAttributeSetupGrid" %>

<div id="LASGContext" runat="server">
    <cimesui:cimesgridview id="gvQuery" runat="server" autogeneratecolumns="false" enablecimesfilter="false"
        onrowdatabound="gvQuery_RowDataBound">
    <Columns>
        <asp:BoundField DataField="AttributeName" HeaderText="<%$ Resources:Auto|RuleFace, AttributeName %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeValue %>">
            <ItemTemplate>
                <asp:TextBox ID="ttbChar" runat="server" TextMode="MultiLine"></asp:TextBox>
                <asp:DropDownList ID="ddlItemMaster" runat="server" >
                </asp:DropDownList>
                <asp:RadioButton ID="rdbY" runat="server" GroupName="BooleanGroup" Text="Y" 
                    Checked="True" />
                <asp:RadioButton ID="rdbN" runat="server" GroupName="BooleanGroup" Text="N"  />
            </ItemTemplate>
            <ItemStyle Wrap="False"  />
            <HeaderStyle Wrap="False" />
        </asp:TemplateField>
    </Columns>
                                            </cimesui:cimesgridview>
</div>
<script type="text/javascript">

</script>
