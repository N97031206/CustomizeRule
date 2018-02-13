<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipmentConnectionSetting.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.EquipmentConnectionSetting" %>
<div style="margin-left: auto; margin-right: auto; width: 900px; height: 220px;  overflow: hidden;">
    <div style="width: 24%; float: left; padding: 0px;">
        <table width="100%">
            <tr>
                <td class="CSTDMust" style="width: 35%;">
                    <asp:Label ID="lblPLCVendor" runat="server" Text="<%$ Resources:Auto|RuleFace, PLCVendor %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPLCVendor" runat="server" CssClass="CSDropDownListMust"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlPLCVendor_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="CSTDMust" style="width: 35%;">
                    <asp:Label ID="lblPLCType" runat="server" Text="<%$ Resources:Auto|RuleFace, PLCType %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPLCType" runat="server" CssClass="CSDropDownListMust" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlPLCType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="CSTDMust" style="width: 35%;">
                    <asp:Label ID="lblProtocol" runat="server" Text="<%$ Resources:Auto|RuleFace, Protocol  %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlProtocol" runat="server" CssClass="CSDropDownListMust">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="CSTDMust" style="width: 35%;">
                    <asp:Label ID="lblConnPara" runat="server" Text="<%$ Resources:Auto|RuleFace, Parameter  %>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="ttbConnPara" runat="server" ReadOnly="true" TextMode="MultiLine"
                        Height="68px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="CSTD" style="width: 35%;">
                </td>
                <td align="right">
                    <CimesUI:CimesButton ID="btnCommunSet" runat="server" Text="<%$ Resources:Auto|RuleFace, ConnectionSetting  %>"
                        CssClass="CSSetupButton" />
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 75%; float: right; padding: 0px;">
        <table width="100%">
            <tr>
                <td align="right">
                    <CimesUI:CimesButton ID="btnAddDataMap" runat="server" Text="<%$ Resources:Auto|RuleFace, DataMappingSetting %>"
                        OnClick="btnAddDataMap_Click" CssClass="CSAddButton" />
                    <CimesUI:CimesButton ID="btnAddPara" runat="server" Text="<%$ Resources:Auto|RuleFace, AddParameter %>"
                        OnClick="btnAddPara_Click" CssClass="CSAddButton" />
                </td>
            </tr>
        </table>
        <div style="width: 100%; height: 215px; overflow: auto;">
            <CimesUI:CimesGridView ID="gvEquipmentAttribute" runat="server" Width="100%" Height="1px"
                AutoGenerateColumns="False" OnRowDataBound="gvEquipmentAttribute_RowDataBound"
                OnRowDeleting="gvEquipmentAttribute_RowDeleting">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                Text="<%$ Resources:Auto|RuleFace, Delete %>" CssClass="CSGridDeleteButton" />
                        </ItemTemplate>
                        <ItemStyle Width="25px" />
                        <HeaderStyle Width="25px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeCode %>">
                        <ItemTemplate>
                            <asp:Label ID="lblAttributeCode" runat="server" Text='<%# Bind("ParameterID") %>'></asp:Label>
                            <asp:DropDownList ID="ddlAttributeCode" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                        <HeaderStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, AttributeDescr %>">
                        <ItemTemplate>
                            <asp:Label ID="lblDescr" runat="server" Text='<%# Bind("ParameterName") %>'></asp:Label>
                            <asp:TextBox ID="ttbDescr" runat="server" Text='<%# Bind("ParameterName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, DataAddress %>">
                        <HeaderTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblDataAddress" runat="server" Text="<%$ Resources:Auto|RuleFace, DataAddress %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 33%;">
                                        <asp:Label ID="lblType" runat="server" Text="<%$ Resources:Auto|RuleFace, Type %>"></asp:Label>
                                    </td>
                                    <td style="width: 33%;">
                                        <asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:Auto|RuleFace, DataAddress %>"></asp:Label>
                                    </td>
                                    <td style="width: 34%;">
                                        <asp:Label ID="lblQuantity" runat="server" Text="<%$ Resources:Auto|RuleFace, Quantity %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" style="border-style:none;">
                                <tr>
                                    <td style="width: 33%;">
                                        <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="ttbType" runat="server" Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 33%;">
                                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="ttbAddress" runat="server" Text=""></asp:TextBox>
                                    </td>
                                    <td style="width: 34%;">
                                        <asp:Label ID="lblQuantity" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="ttbQuantity" runat="server" Text=""></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, Gain %>">
                        <ItemTemplate>
                            <asp:Label ID="lblGain" runat="server" Text='<%# Bind("Gain") %>'></asp:Label>
                            <asp:TextBox ID="ttbGain" runat="server" Text='<%# Bind("Gain") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="50px" />
                        <HeaderStyle Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, DataBit %>">
                        <ItemTemplate>
                            <asp:Label ID="lblDataBit" runat="server" Text='<%# Bind("DataBit") %>'></asp:Label>
                            <asp:DropDownList ID="ddlDataBit" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, UseType %>">
                        <ItemTemplate>
                            <asp:Label ID="lblUseType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                            <asp:DropDownList ID="ddlUseType" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, ScanTime %>">
                        <ItemTemplate>
                            <asp:Label ID="lblScanTime" runat="server" Text='<%# Bind("ScanTime") %>'></asp:Label>
                            <asp:TextBox ID="ttbScanTime" runat="server" Text='<%# Bind("ScanTime") %>'></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Auto|RuleFace, MappingName %>">
                        <ItemTemplate>
                            <asp:Label ID="lblMappingName" runat="server" Text='<%# Bind("MappingName") %>'></asp:Label>
                            <asp:DropDownList ID="ddlMappingName" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Width="60px" />
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                </Columns>
            </CimesUI:CimesGridView>
        </div>
    </div>
</div>
<ajaxToolkit:ModalPopupExtender ID="MPE" runat="server" TargetControlID="btnCommunSet" PopupControlID="CommunicationMethod"
    BackgroundCssClass="modalBackground" DropShadow="false" OkControlID="" OnOkScript=""
    CancelControlID="btnCancel" PopupDragHandleControlID="">
</ajaxToolkit:ModalPopupExtender>

<div id="CommunicationMethod" style="display: none; background-color: White; width: 600px;"
    runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
<ContentTemplate>
    <table width="300px">
        <tr>
            <td class="CSTDMust" style="width: 42%;">
                <asp:Label ID="lblMethod" runat="server" Text="<%$ Resources:Auto|RuleFace, Communication  %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMethod" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMethod_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <fieldset id="fsEthernet" runat="server">
        <legend>
            <asp:Label ID="lblEthernet" runat="server" Text="<%$ Resources:Auto|RuleFace, Ethernet  %>"></asp:Label></legend>
        <table width="100%">
            <tr>
                <td class="CSTDMust" style="width: 20%;">
                    <asp:Label ID="lblIP" runat="server" Text="<%$ Resources:Auto|RuleFace, IPAddress  %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbIP" runat="server" Text=""></asp:TextBox>
                </td>
                <td class="CSTDMust" style="width: 20%;">
                    <asp:Label ID="lblPort" runat="server" Text="<%$ Resources:Auto|RuleFace, Port  %>"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:TextBox ID="ttbPort" runat="server" Text=""></asp:TextBox>
                </td>
            </tr>
            </tr>
        </table>
    </fieldset>
    <fieldset id="fsSerial" runat="server">
        <legend>
            <asp:Label ID="lblSerial" runat="server" Text="<%$ Resources:Auto|RuleFace, Serial  %>"></asp:Label></legend>
        <table width="100%">
            <tr>
                <tr>
                    <td class="CSTDMust" style="width: 20%;">
                        <asp:Label ID="lblComPort" runat="server" Text="<%$ Resources:Auto|RuleFace, ComPort  %>"></asp:Label>
                    </td>
                    <td style="width: 30%;">
                        <asp:TextBox ID="ttbComPort" runat="server" Text=""></asp:TextBox>
                    </td>
                    <td class="CSTDMust" style="width: 20%;">
                        <asp:Label ID="lblBaudRate" runat="server" Text="<%$ Resources:Auto|RuleFace, BaudRate  %>"></asp:Label>
                    </td>
                    <td style="width: 30%;">
                        <asp:TextBox ID="ttbBaudRate" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="CSTDMust" style="width: 20%;">
                        <asp:Label ID="lblParity" runat="server" Text="<%$ Resources:Auto|RuleFace, Parity  %>"></asp:Label>
                    </td>
                    <td style="width: 30%;">
                        <asp:DropDownList ID="ddlParity" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="CSTDMust" style="width: 20%;">
                        <asp:Label ID="lblDataBit" runat="server" Text="<%$ Resources:Auto|RuleFace, DataBit  %>"></asp:Label>
                    </td>
                    <td style="width: 30%;">
                        <asp:TextBox ID="ttbDataBit" runat="server" Text=""></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="CSTDMust" style="width: 20%;">
                        <asp:Label ID="lblStopBit" runat="server" Text="<%$ Resources:Auto|RuleFace, StopBit  %>"></asp:Label>
                    </td>
                    <td style="width: 30%;">
                        <asp:DropDownList ID="ddlStopBit" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="CSTDMust" style="width: 20%;">
                    </td>
                    <td style="width: 30%;">
                    </td>
                </tr>
        </table>
    </fieldset>
    <table width="100%">
        <tr>
            <td align="center">
                <CimesUI:CimesButton ID="btnSave" runat="server" CssClass="CSSaveButton" Text="<%$ Resources:Auto|RuleFace, Save  %>" />
                <CimesUI:CimesButton ID="btnCancel" runat="server" CssClass="CSCancelButton" Text="<%$ Resources:Auto|RuleFace, Cancel  %>" />
            </td>
        </tr>
    </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnCancel" />
        </Triggers>
</asp:UpdatePanel>
</div>