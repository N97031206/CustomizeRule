<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipmentLayerBlock.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.EquipmentLayerBlock" %>
<div style="width: 100%; height: 100%;">
    <div style="height: 5%; overflow: hidden;">
        <div style="width: 55%; float: left;">
            <div style="float: left;">
                <ajaxToolkit:SliderExtender id="SliderExtender1" runat="server" length="300" behaviorid="ttbSlider1"
                    targetcontrolid="ttbSlider1" decimals="2" minimum="0.1" maximum="2" tooltiptext="{0}"
                    boundcontrolid="" enablehandleanimation="true" />
                <asp:TextBox ID="ttbSlider1" runat="server" AutoPostBack="true" Style="right: 0px; display: none;"
                    Text="1" OnTextChanged="ttbSlider1_TextChanged" />
                <asp:HiddenField ID="hdSliderPostBack" runat="server" EnableViewState="false" Value="false" />
            </div>
            <div style="float: left; width:20%; display:table;text-align:right;">
                <asp:Label ID="lblCurrentFactory" runat="server" Text=""></asp:Label></div>
        </div>
        <div style="width: 40%; float: right; text-align: right;">
            <asp:CheckBox ID="cbDisplayOperation" runat="server" Text="<%$ Resources:Auto|RuleFace, DisplayOperation %>" AutoPostBack="True" OnCheckedChanged="btnMode_Click" />
            <asp:RadioButton ID="btnLayoutMode" runat="server" OnCheckedChanged="btnMode_Click" Text="<%$ Resources:Auto|RuleFace, LayoutMode %>"
                GroupName="Mode" Checked="true" AutoPostBack="true">
            </asp:RadioButton>
            <asp:RadioButton ID="btnListMode" runat="server" OnCheckedChanged="btnMode_Click" Text="<%$ Resources:Auto|RuleFace, ListMode %>"
                GroupName="Mode" AutoPostBack="true">
            </asp:RadioButton>
        </div>
    </div>
    <div style="width: 100%; height: 95%; background-repeat: no-repeat; position: relative;
        overflow: hidden;" id="divMaster" runat="server">
        <div style="width: 100%; height: 100%; background-repeat: no-repeat;" id="divMainLayout"
            runat="server">
        </div>
    </div>
    <div style="width: 100%; height: 95%; background-repeat: no-repeat; position: relative;
        overflow: auto;" id="divShowByList" runat="server" visible="false">
        <CimesUI:CimesGridView ID="gvEquipmentList" runat="server" AutoGenerateColumns="False" 
            Width="100%" OnRowDataBound="gvEquipmentList_RowDataBound">
            <Columns>
                <asp:BoundField DataField="EquipmentName" HeaderText="<%$ Resources:Auto|RuleFace, EquipmentName %>" />
                <asp:BoundField DataField="EquipmentType" HeaderText="<%$ Resources:Auto|RuleFace, EquipmentType %>" />
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, CurrentState %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblCurrentState"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, FAIResult %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblFAIResult"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, WorkOrder %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblWorkOrder"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="150px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Tool %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblTool"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, CurrentLotCount %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblCurrentLotCount"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, TodayOutQuantity %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblTodayOutQty"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, DefectQuantity %>">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ID="lblTodayDefectQty"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px" HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </CimesUI:CimesGridView>
    </div>
</div>
<asp:TextBox ID="ttbSelectEquipmentName" runat="server" Style="display: none;"></asp:TextBox>
<!-- 不需換成CimesButton，若需更換，需修改EquipmentMonitor.aspx的$("input[id$='btnLoadEquipmentData']").click(); -->
<asp:Button ID="btnLoadEquipmentData" runat="server" Text="Button" Style="display: none;"
    OnClick="btnLoadEquipmentData_Click" />
