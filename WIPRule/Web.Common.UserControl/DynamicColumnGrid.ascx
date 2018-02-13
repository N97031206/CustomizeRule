<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicColumnGrid.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.DynamicColumnGrid" %>

<cimesui:cimesgridview id="gvDynamicColumn" runat="server" autogeneratecolumns="False"
    allowpaging="True" pagesize="10" width="1750px" onpageindexchanging="gvDynamicColumn_PageIndexChanging"
    onrowcancelingedit="gvDynamicColumn_RowCancelingEdit" onrowediting="gvDynamicColumn_RowEditing"
    onrowupdating="gvDynamicColumn_RowUpdating" onrowdeleting="gvDynamicColumn_RowDeleting"
    onrowdatabound="gvDynamicColumn_RowDataBound" onrowcreated="gvDynamicColumn_RowCreated"
    enablecimessort="True" ondatasourcechanged="gvDynamicColumn_DataSourceChanged">
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
            <HeaderStyle Width="80px" Wrap="false"></HeaderStyle>
            <ItemStyle HorizontalAlign="Center" Wrap="false" />
            <ItemTemplate>
                <asp:LinkButton ID="lkbtnEdit" runat="server" Text="<%$ Resources:Auto|RuleFace, Edit %>"> ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="ColumnName" HeaderText="<%$ Resources:Auto|RuleFace, ColumnName %>">
            <HeaderStyle Width="140px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblAttribute" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlColumnName" Width="160px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlColumnName_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:TextBox ID="ttbColumnName" Width="160px" runat="server"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="DataType" HeaderText="<%$ Resources:Auto|RuleFace, DataType %>">
            <HeaderStyle Width="100px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblDataType" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlDataType" runat="server" Width="90px" AutoPostBack="True">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="Button">Button</asp:ListItem>
                    <asp:ListItem Value="Date">Date</asp:ListItem>
                    <asp:ListItem Value="MultiChar">MultiChar</asp:ListItem>
                    <asp:ListItem Value="MultiSelect">MultiSelect</asp:ListItem>                 
                    <asp:ListItem Value="Select">Select</asp:ListItem>
                    <asp:ListItem Value="Text">Text</asp:ListItem>
                    <asp:ListItem Value="Radio-V">Radio-V</asp:ListItem>
                    <asp:ListItem Value="Radio-H-02">Radio-H-02</asp:ListItem>
                    <asp:ListItem Value="Radio-H-03">Radio-H-03</asp:ListItem>
                    <asp:ListItem Value="Radio-H-04">Radio-H-04</asp:ListItem>
                    <asp:ListItem Value="Radio-H-05">Radio-H-05</asp:ListItem>
                    <asp:ListItem Value="Radio-H-99">Radio-H-99</asp:ListItem>                    
                </asp:DropDownList>
                <br />
                 <asp:Label ID="lblCSS" runat="server" Text="CSS" Visible="false"></asp:Label>
               <asp:TextBox ID="ttbCSSButton" Width="100px" runat="server" Visible="false"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="CheckType" HeaderText="<%$ Resources:Auto|RuleFace, CheckType %>">
            <HeaderStyle Width="70px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblCheckType" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlCheckType" runat="server" Width="100%" AutoPostBack="True">
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ControlCount %>">
            <HeaderStyle Width="100px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblControlCount" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="ttbControlCount" Enabled="false" runat="server"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, CheckCondition %>">
            <HeaderStyle Width="180px" Wrap="false"></HeaderStyle>
            <ItemTemplate>
                <asp:Label ID="lblCheckCondition" runat="server"></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="ttbCheckCondition" Width="180px" runat="server"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, RequestFlag %>">
            <HeaderStyle Width="120px" Wrap="false"></HeaderStyle>
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
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AutoPostBack %>">
            <HeaderStyle Width="120px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:RadioButton ID="rbtPostBack" runat="server" GroupName="PostBackFlag" Width="40px"
                    Text="<%$ Resources:Auto|RuleFace, Yes %>" Checked="true" />
                <asp:RadioButton ID="rbtUnPostBack" runat="server" GroupName="PostBackFlag" Width="40px"
                    Text="<%$ Resources:Auto|RuleFace, No %>" />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblPostBackFlag" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Associate %>">
            <HeaderStyle Width="140px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlAssociate" runat="server" Width="160px">
                </asp:DropDownList>
                <asp:TextBox ID="ttbAssociate" Visible ="false" Width="160px" runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblAssociate" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeType %>">
            <HeaderStyle Width="220px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:RadioButton ID="rbtSysType" runat="server" GroupName="AttributeTypeFlag" Width="90px"
                    Text="<%$ Resources:Auto|RuleFace, SystemAttribute %>" Checked="true" />
                <asp:RadioButton ID="rbtCustType" runat="server" GroupName="AttributeTypeFlag" Width="90px"
                    Text="<%$ Resources:Auto|RuleFace, CustomAttribute %>" />
                <asp:RadioButton ID="rbtNonTyoe" runat="server" GroupName="AttributeTypeFlag" Width="40px"
                    Text="<%$ Resources:Auto|RuleFace, None %>" />
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblAttributeTypeFlag" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeName %>">
            <HeaderStyle Width="80px" Wrap="false"></HeaderStyle>
            <EditItemTemplate>
                <asp:TextBox ID="ttbCustAttrName" Width="140px" runat="server"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblCustAttrName" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</cimesui:cimesgridview>
