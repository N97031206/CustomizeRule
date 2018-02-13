<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CimesFilterWindow.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.CimesFilterWindow" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel Style="border-right: none; padding-right: 20px; border-top: none; text-align:center;
    padding-left: 20px; padding-bottom: 0px; border-left: none; width: 600px;
    padding-top: 0px; border-bottom: none; background-color: white" ID="PanelQuerySetup"
    runat="server">
    <asp:Button ID="btnShowHidden" runat="server" Text="" Width="0px" Height="0px" style="display:none;" />
    <fieldset>
        <legend>
            <asp:Label ID="lblInputTerm" runat="server" Text="<%$ Resources:Auto|RuleFace, InputTerm %>" CssClass="CSLabel"></asp:Label>
        </legend>
        <table width="100%">
            <tr>
                <td align="left">
                    <CimesUI:CimesButton ID="btnAdd" runat="server" Text="<%$ Resources:Auto|RuleFace, Add %>" CssClass="CSAddButton" OnClick="btnAdd_Click" />
                    <CimesUI:CimesButton ID="btnQuery" runat="server" Text="<%$ Resources:Auto|RuleFace, Query %>" CssClass="CSQueryButton" OnClick="btnSearch_Click" />
                    <CimesUI:CimesButton ID="btnCancel" runat="server" Text="<%$ Resources:Auto|RuleFace, Cancel %>" CssClass="CSCancelButton"
                        FunctionKey="ESC" onclick="btnCancel_Click" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:GridView ID="gvSql" runat="server" AutoGenerateColumns="False" CssClass="CSGridViewStyle"
                        Width="100%" AllowPaging="True" AllowSorting="True" OnRowDeleting="gvSql_RowDeleting"
                        OnRowDataBound="gvSql_RowDataBound" OnRowCreated="gvSql_RowCreated" 
                        onpageindexchanging="gvSql_PageIndexChanging">
                        <Columns>
                            <asp:ButtonField ButtonType="Link" CommandName="Delete" Text="<img src='../../Images/bgButtonDelete.png' border='0'/>"
                                ItemStyle-HorizontalAlign="Center"></asp:ButtonField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    (
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbxLeft" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="120px" HeaderText="<%$ Resources:Auto|RuleFace, FieldName %>">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlColumnName" runat="server"  Width="100%"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlColumnName_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="140px" HeaderText="<%$ Resources:Auto|RuleFace, CheckCondition %>">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlOperator" runat="server" AutoPostBack="True" 
                                        Width="100%" OnSelectedIndexChanged="ddlOperator_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                                        <asp:ListItem Text="&lt;&gt;" Value="&lt;&gt;"></asp:ListItem>
                                        <asp:ListItem Text="&gt;" Value="&gt;"></asp:ListItem>
                                        <asp:ListItem Text="&lt;" Value="&lt;"></asp:ListItem>
                                        <asp:ListItem Text="&gt;=" Value="&gt;="></asp:ListItem>
                                        <asp:ListItem Text="&lt;=" Value="&lt;="></asp:ListItem>
                                        <asp:ListItem Text="LIKE(完全匹配)" Value="LIKE1"></asp:ListItem>
                                        <asp:ListItem Text="LIKE(開頭部份匹配)" Value="LIKE2"></asp:ListItem>
                                        <asp:ListItem Text="IS NULL" Value="IS NULL"></asp:ListItem>
                                        <asp:ListItem Text="IS NOT NULL" Value="IS NOT NULL"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Width="140px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="120px" HeaderText="<%$ Resources:Auto|RuleFace, Value %>">
                                <ItemTemplate>
                                    <asp:TextBox ID="ttbValue" runat="server" CssClass="CSInput" Width="95%" Visible="false"></asp:TextBox>
                                    <asp:DropDownList ID="ddlValue" runat="server"  Width="95%"
                                        Visible="false">
                                    </asp:DropDownList>
                                    <asp:RadioButtonList ID="rdlValue" runat="server" Width="95%" Visible="false">
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    )
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbxRight" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="<%$ Resources:Auto|RuleFace, Logic %>">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlLogic" runat="server"  Width="100%">
                                        <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                        <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <ItemStyle Width="60px" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="CSGridAlterItem" Height="1px" />
                        <EditRowStyle CssClass="CSGridEditItem" ForeColor="Black" Height="1px" />
                        <SelectedRowStyle CssClass="CSGridEditItem" ForeColor="Black" Height="1px" />
                        <HeaderStyle CssClass="CSGridHeader" Height="1px" />
                        <PagerStyle CssClass="CSGridPage" Height="1px" />
                        <RowStyle CssClass="CSGridItem" Height="1px" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:TextBox ID="ttbOtherSqlString" runat="server" TextMode="MultiLine" Width="98%" Height="50px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </fieldset>
    <p align="left">
        <asp:Label ID="lblStatus" runat="server" ForeColor="Red" EnableViewState="false" Width="100%"></asp:Label>
    </p>
</asp:Panel>
