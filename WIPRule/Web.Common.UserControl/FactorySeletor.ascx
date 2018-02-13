<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FactorySeletor.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.FactorySeletor" %>
<%--<div style="position: relative; height: 100%;">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr id="tHeader" style="height: 24px;">
            <td style="width: 8px; background-image:url(../../images/topLeft.png);">
            </td>
            <td style=" background-image:url(../../images/topCenter.png); ">
                <asp:Label ID="lblFactorySeletor" runat="server" Text="<%$ Resources:Auto|RuleFace, FactorySeletor %>"
                    Style="text-align: center; font-size: large; display: block;"></asp:Label>
            </td>
            <td style="width: 8px; background-image:url(../../images/topRight.png);">
            </td>
        </tr>
        <tr id="tBody">
            <td style="width: 8px;">
            </td>
            <td>
                <table width="100%">
                    <tr>
                        <td class="CSTD" style="width: 40%;">
                            <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Auto|RuleFace, Department %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDepartment" runat="server"  AutoPostBack="True"
                                Width="100%" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="CSTD" style="width: 40%;">
                            <asp:Label ID="lblLayout" runat="server" Text="<%$ Resources:Auto|RuleFace, Layout %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLayout" runat="server"  AutoPostBack="True"
                                Width="100%" OnSelectedIndexChanged="ddlLayout_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 8px; background-image:url(../../images/centerRight.png);">
            </td>
        </tr>
        <tr id="tFooter" style="height:8px;">
            <td style="width: 8px; background-image:url(../../images/bottomLeft.png);">
            </td>
            <td style="background-image:url(../../images/bottomCenter.png);">
            </td>
            <td style="width: 8px; background-image:url(../../images/bottomRight.png);">
            </td>
        </tr>
    </table>
</div>--%>
<div>
    <h2 style="background-color: #497CD1; color: White; border: solid 1px #3C69BD; background-image: url(../../Images/titleBg.png);
        display: block; margin: 0px;">
        <asp:Label ID="lblFactorySeletor" runat="server" Text="<%$ Resources:Auto|RuleFace, Factory %>"
            Style="text-align: center; font-size: medium; display: block;"></asp:Label></h2>
    <div style="border: 1px solid #ABABAB; display: block; vertical-align: top; height: 300px;
        overflow: hidden;">
        <table width="100%" style="height: 1px;">
            <tr>
                <td class="CSTD" style="width: 40%;">
                    <asp:Label ID="lblDepartment" runat="server" Text="<%$ Resources:Auto|RuleFace, Department %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDepartment" runat="server"  AutoPostBack="True"
                        Width="100%" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display:none;">
                <td class="CSTD" style="width: 40%;">
                    <asp:Label ID="lblLayout" runat="server" Text="<%$ Resources:Auto|RuleFace, Layer %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLayout" runat="server"  AutoPostBack="True"
                        Width="100%" OnSelectedIndexChanged="ddlLayout_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div style="width: 100%; height: 275px; overflow: auto;" class="ScrollPanel" id="Panel1" runat="server">
            <CimesUI:CimesGridView ID="gvLayout" runat="server" Width="99%"  AutoGenerateColumns="false"
                OnRowDataBound="gvLayout_RowDataBound" >
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div style="height: 70px;">
                                <div style="width: 35%; float: left; overflow: hidden;">
                                    <asp:Image ID="Image1" runat="server" Width="70px" Height="70px" />
                                </div>
                                <div style="width: 60%; float: left; overflow: hidden; vertical-align:middle;height: 70px; display:table-cell;" runat="server" id="divDescription">
                                    <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click"></asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </cimesUI:CimesGridView>
        </div>
    </div>
</div>
 <asp:TextBox ID="txtScroll" runat="server" CssClass="ScrollTemp" Style="display: none"></asp:TextBox>