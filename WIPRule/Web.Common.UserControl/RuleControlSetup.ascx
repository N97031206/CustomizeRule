<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RuleControlSetup.ascx.cs" Inherits="ciMes.Web.Common.UserControl.RuleControlSetup" %>
<CimesUI:CimesGridView ID="rgQuery" runat="server" ShowHeader ="false" AutoGenerateColumns="False" Width="100%" OnRowDataBound="rgQuery_RowDataBound">
    <Columns>
            <asp:BoundField SortExpression="DisplayName" HeaderText="<%$ Resources:Auto|RuleFace, DisplayName %>" DataField="REMARK03">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" Width="30%"/>
            </asp:BoundField>  
            <asp:TemplateField HeaderText="Yes">
               <ItemStyle Wrap="False" Width="70%"></ItemStyle>
                  <ItemTemplate>
                    <asp:TextBox ID="ttbValue" runat="server" Width="100%"></asp:TextBox>
                    <asp:RadioButton ID="rdbY" runat="server" GroupName="BooleanGroup" Text="Y" Width="45%"
                        Checked="True" />
                    <asp:RadioButton ID="rdbN" runat="server" GroupName="BooleanGroup" Text="N" Width="45%" Checked="true" />
                 </ItemTemplate>
            </asp:TemplateField>    
    </Columns>
</CimesUI:CimesGridView> 
