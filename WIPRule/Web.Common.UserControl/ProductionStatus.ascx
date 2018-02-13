<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductionStatus.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.ProductionStatus" %>
<div>
    <h2 style="background-color: #497CD1; color: White; border: solid 1px #3C69BD; background-image: url(../../Images/titleBg.png);
        display: block; margin: 0px;">
        <asp:Label ID="lblProductionStatus" runat="server" Text="<%$ Resources:Auto|RuleFace, ProductionStatus %>"
            Style="text-align: center; font-size: medium; display: block;"></asp:Label></h2>
    <div style="border: 1px solid #ABABAB; display: block; vertical-align: top; height: 192px;">
        <table width="100%">
            <tr>
                <td colspan="2">
                    <div style="width: 100%; height: 150px; overflow: auto;">
                        <CimesUI:CimesGridView ID="gvStatus" runat="server" AutoGenerateColumns="false" ShowHeader="false"
                            Width="99%" onrowdatabound="gvStatus_RowDataBound" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div id="divStatus" runat="server" style="float: left; width: 40%; height: 24px;
                                            overflow: hidden; display:table-cell; vertical-align:middle; ">
                                        </div>
                                        <div id="divQty" runat="server" style="float: left; width: 58%; height: 24px; overflow: hidden; border:1px; border-color:#eeeeee; border-style:solid;">
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="false" />
                                </asp:TemplateField>
                            </Columns>
                        </CimesUI:CimesGridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="CSTD" style="width: 55%;">
                    <asp:Label ID="lblActualWork" runat="server" Text="<%$ Resources:Auto|RuleFace, ActualWork %>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="ttbActualWork" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
</div>
