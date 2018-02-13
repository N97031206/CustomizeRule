<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EquipmentParameterHistoryChart.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.EquipmentParameterHistoryChart" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<table width="600px" border="0">
    <tr>
        <td style="width: 100%" align="Center">
            <asp:Chart ID="chart1" runat="server" Width="550px" Height="280px" BackColor="211, 223, 240"
                BorderDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px" BorderColor="#1A3B69"
                BorderlineColor="" EnableViewState="True" Style="position: relative; margin: auto;">
                <BorderSkin BackColor="" />
                <Series>
                    <asp:Series Name="Series" ShadowColor="" BorderColor="180, 26, 59, 105" Color="Red"
                        Legend="Legend1" IsValueShownAsLabel="False" LabelForeColor="Red" IsVisibleInLegend="False"
                        ChartType="Line" MarkerStyle="Square" MarkerSize="3">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                        <AxisX TitleAlignment="Far">
                            <MajorGrid Enabled="False" />
                        </AxisX>
                        <AxisY IsStartedFromZero="False" TextOrientation="Stacked" Title="" TitleAlignment="Far">
                        </AxisY>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblErrorMessage" runat="server" Text="" ForeColor="Red" EnableViewState="false"></asp:Label>
        </td>
    </tr>
</table>
