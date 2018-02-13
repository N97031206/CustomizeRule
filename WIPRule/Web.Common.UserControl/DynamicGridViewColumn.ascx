<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicGridViewColumn.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.DynamicGridViewColumn" %>
<CimesUI:CimesGridView ID="gvGridViewColumn" runat="server" AutoGenerateColumns="False"
    AllowPaging="True" PageSize="10" Width="1400px" OnPageIndexChanging="gvGridViewColumn_PageIndexChanging"
    OnRowCancelingEdit="gvGridViewColumn_RowCancelingEdit" OnRowEditing="gvGridViewColumn_RowEditing"
    OnRowUpdating="gvGridViewColumn_RowUpdating" OnRowDeleting="gvGridViewColumn_RowDeleting"
    OnRowDataBound="gvGridViewColumn_RowDataBound" OnRowCreated="gvGridViewColumn_RowCreated"
    EnableCimesSort="True" OnDataSourceChanged="gvGridViewColumn_DataSourceChanged">
    <Columns>
        <CimesUI:CimesCommandField ShowDeleteButton="true" ShowEditButton="true" ShowCancelButton="true">
            <ControlStyle Width="25px" />
            <ItemStyle HorizontalAlign="Center" />
        </CimesUI:CimesCommandField>
        <asp:BoundField DataField="Sequence" HeaderText="<%$ Resources:Auto|RuleFace, Sequence %>"
            ReadOnly="True">
            <HeaderStyle Width="40px" Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField>
            <HeaderStyle Width="50px" Wrap="false" />
            <ItemStyle HorizontalAlign="Center" Wrap="false" />
            <ItemTemplate>
                <asp:Button ID="btnSeqUp" runat="server" CssClass="CSGridUpButton" OnClick="btnSeqUp_Click"
                    Width="15px" />
                <asp:Button ID="btnSeqDown" runat="server" CssClass="CSGridDownButton" OnClick="btnSeqDown_Click"
                    Width="15px" />
            </ItemTemplate>
            <EditItemTemplate>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, CultureText %>">
            <HeaderStyle Width="60px" Wrap="false"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" Wrap="false"/>
            <ItemTemplate>
                <asp:LinkButton ID="lkbtnEdit" runat="server" Text="<%$ Resources:Auto|RuleFace, Edit %>"> ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="AttributeName" HeaderText="<%$ Resources:Auto|RuleFace, ColumnName %>">
            <HeaderStyle Width="150px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblAttribute" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="ttbAttributeName" runat="server"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="DataType" HeaderText="<%$ Resources:Auto|RuleFace, DataType %>">
            <HeaderStyle Width="100px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblDataType" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlDataType" runat="server" AutoPostBack="True">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="MultiChar">MultiChar</asp:ListItem>
                    <asp:ListItem Value="Radio">Radio</asp:ListItem>
                    <asp:ListItem Value="Select">Select</asp:ListItem>
                    <asp:ListItem Value="Text">Text</asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="CheckType" HeaderText="<%$ Resources:Auto|RuleFace, CheckType %>">
            <HeaderStyle Width="100px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblCheckType" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlCheckType" runat="server" Width="100%" AutoPostBack="True">
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, CheckCondition %>">
            <HeaderStyle Width="180px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblCheckCondition" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="ttbCheckCondition" runat="server"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, RequestFlag %>">
            <HeaderStyle Width="100px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:RadioButton ID="rbtRequest" runat="server" GroupName="RequestFlag" Width="40px"
                    Text="<%$ Resources:Auto|RuleFace, Yes %>" Checked="true" />
                <asp:RadioButton ID="rbtUnRequest" runat="server" GroupName="RequestFlag" Width="40px"
                    Text="<%$ Resources:Auto|RuleFace, No %>" />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblSelectRequestFlag" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ControlName %>">
            <HeaderStyle Width="80px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:TextBox ID="ttbControlName" runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblControl" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Width %>">
            <HeaderStyle Width="80px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:TextBox ID="ttbWidth" runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblWidth" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Description %>">
            <HeaderStyle Width="150px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:TextBox ID="ttbDescr" runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblDescr" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</CimesUI:CimesGridView>