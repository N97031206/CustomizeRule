<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeTemplateGrid.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.AttributeTemplateGrid" %>

<script language="javascript">
    function SelectItem() {
        var dgTable = document.getElementById("AttributeTemplateGrid1_gvQuery");
        if (dgTable != null) {
            var Item = dgTable.getElementsByTagName('input');
            for (i = 0; i < Item.length; i++) {
                var ckbCheckAll = Item[0];
                var Check = Item[i];
                if (Check.type == "checkbox") {
                    if (ckbCheckAll.checked == true)
                        Check.checked = "checked";
                    else
                        Check.checked = "";
                }
            }
        }
    }
</script>

<CimesUI:CimesGridView ID="gvQuery" runat="server" AutoGenerateColumns="False" UseAccessibleHeader="False"
    Width="100%" OnRowDataBound="gvQuery_RowDataBound">
    <Columns>
        <asp:TemplateField>
            <HeaderStyle Wrap="False" Width="3px"></HeaderStyle>
            <ItemStyle Wrap="False"></ItemStyle>
            <HeaderTemplate>
                <input id="ckbCheckAll" type="checkbox" name="ckbCheckAll" runat="server" onclick="SelectItem();" />
            </HeaderTemplate>
            <ItemTemplate>
                <input id="ckbCheckAttr" type="checkbox" name="ckbCheckLot" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ATTRIBUTENAME" HeaderText="<%$ Resources:Auto|RuleFace, AttributeName %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" Width="15%" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeValue %>">
            <HeaderStyle Wrap="False" />
            <ItemTemplate>
                <asp:TextBox ID="ttbChar" runat="server" Width="120px"></asp:TextBox>
                <asp:DropDownList ID="ddlItemMaster" runat="server" Width="120px">
                </asp:DropDownList>
                <asp:RadioButton ID="rdbY" runat="server" GroupName="BooleanGroup" Text="Y" Width="50px"
                    Checked="True" />
                <asp:RadioButton ID="rdbN" runat="server" GroupName="BooleanGroup" Text="N" Width="50px" />
            </ItemTemplate>
            <ItemStyle Wrap="False" Width="120px" />
        </asp:TemplateField>
        <asp:BoundField DataField="DEFAULTVALUE" Visible="True" HeaderText="<%$ Resources:Auto|RuleFace, DEFAULTVALUE %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" Width="10%" />
        </asp:BoundField>
        <asp:BoundField DataField="REQUESTFLAG" HeaderText="<%$ Resources:Auto|RuleFace, REQUESTFLAG %>"
            Visible="True">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" HorizontalAlign="Center" />
        </asp:BoundField>
        <asp:BoundField DataField="CHECKTYPE" Visible="True" HeaderText="<%$ Resources:Auto|RuleFace, CHECKTYPE %>">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" Width="10%" />
        </asp:BoundField>
        <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Description %>" DataField="DESCR"
            Visible="True">
            <HeaderStyle Wrap="False" />
            <ItemStyle Wrap="False" Width="35%" />
        </asp:BoundField>
    </Columns>
</CimesUI:CimesGridView>
