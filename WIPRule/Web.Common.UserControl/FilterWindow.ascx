<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterWindow.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.FilterWindow" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnQuery" />
    </Triggers>
    <ContentTemplate>
        <cc1:ModalPopupExtender ID="ModalPopupExtenderQueryStr" runat="server" TargetControlID="btnShowHidden"
            PopupControlID="PanelQuerySetup" CancelControlID="btnCancel" BackgroundCssClass="modalBackground"
            X="200" Y="100">
        </cc1:ModalPopupExtender>
        <asp:Panel Style="border-right: black 2px solid; padding-right: 20px; border-top: black 2px solid;
            display: none; padding-left: 20px; padding-bottom: 20px; border-left: black 2px solid;
            width: 600px; padding-top: 20px; border-bottom: black 2px solid; background-color: white"
            ID="PanelQuerySetup" runat="server">
            <CimesUI:CimesButton ID="btnShowHidden" runat="server" Text="" Width="0px" Height="0px"  style="display:none;"/>
            <fieldset>
                <legend>
                    <asp:Label ID="lblInputTerm" runat="server" Text="Search Filter" CssClass="CSLabel"></asp:Label>
                </legend>
                <table width="100%">
                    <tr>
                        <td align="left">
                            <CimesUI:CimesButton ID="btnAdd" runat="server"  Text="<%$ Resources:auto|RuleFace, Add %>" CssClass="CSAddButton" OnClick="btnAdd_Click" />
                            <CimesUI:CimesButton ID="btnQuery" runat="server"  Text="<%$ Resources:auto|RuleFace, Searcg %>" CssClass="CSQueryButton" OnClick="btnSearch_Click" />
                            <CimesUI:CimesButton ID="btnCancel" runat="server"  Text="<%$ Resources:auto|RuleFace, Cancel %>" CssClass="CSCancelButton"
                                FunctionKey="ESC" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:GridView ID="gvSql" runat="server" AutoGenerateColumns="False" CssClass="CSGridViewStyle"
                                Width="100%" AllowPaging="True" AllowSorting="True" OnRowDeleting="gvSql_RowDeleting"
                                OnRowDataBound="gvSql_RowDataBound" OnRowCreated="gvSql_RowCreated">
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Link" CommandName="Delete" Text="<img src='../Images/bgButtonDelete.png' border='0'/>"
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
                                    <asp:TemplateField ItemStyle-Width="120px" HeaderText="FieldName">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlColumnName" runat="server"  Width="100%"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlColumnName_SelectedIndexChanged">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="140px" HeaderText="CheckCondition">
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
                                    <asp:TemplateField ItemStyle-Width="120px" HeaderText="Value">
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
                                    <asp:TemplateField ItemStyle-Width="60px" HeaderText="Logic">
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
                        </td>
                    </tr>
                </table>
            </fieldset>
            <p align="left">
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red" CssClass="CSLabel" Width="100%"></asp:Label>
            </p>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
