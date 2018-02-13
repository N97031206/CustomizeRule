<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemAttribute.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.SystemAttribute" %>
<cimesui:cimesgridview id="gvQuery" runat="server" autogeneratecolumns="False" useaccessibleheader="False"
    width="98%" onrowdatabound="gvQuery_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ATTRIBUTENAME" HeaderText="<%$ Resources:Auto|RuleFace, ATTRIBUTENAME %>">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ATTRIBUTEVALUE %>">
                <HeaderStyle Wrap="False" />
                <ItemTemplate>
                    <asp:TextBox ID="ttbChar" runat="server" Width="200px"></asp:TextBox>
                    <asp:DropDownList ID="ddlItemMaster" runat="server" Width="200px">
                    </asp:DropDownList>
                    <asp:RadioButton ID="rdbY" runat="server" GroupName="BooleanGroup" Text="Y" Width="60px"
                        Checked="True" />
                    <asp:RadioButton ID="rdbN" runat="server" GroupName="BooleanGroup" Text="N" Width="60px" />
                </ItemTemplate>
                <ItemStyle Wrap="False" Width="200px" />
            </asp:TemplateField>
            <asp:BoundField DataField="DEFAULTVALUE" Visible="True" HeaderText="<%$ Resources:Auto|RuleFace, DEFAULTVALUE %>">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="REQUESTFLAG" HeaderText="<%$ Resources:Auto|RuleFace, REQUESTFLAG %>"
                Visible="True">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5%" />
            </asp:BoundField>
            <asp:BoundField DataField="DataType" Visible="false" HeaderText="<%$ Resources:Auto|RuleFace, DataType %>">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="CHECKTYPE" Visible="false" HeaderText="<%$ Resources:Auto|RuleFace, CHECKTYPE %>">
                <HeaderStyle Wrap="False" />
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField HeaderText="<%$ Resources:Auto|RuleFace, Description %>" DataField="Description"
                Visible="True">
                <HeaderStyle Wrap="False" />
                <ItemStyle Width="35%"  Wrap="true"/>
            </asp:BoundField>
        </Columns>
    </cimesui:cimesgridview>
