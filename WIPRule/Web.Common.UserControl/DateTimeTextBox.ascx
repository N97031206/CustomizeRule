<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimeTextBox.ascx.cs"
    Inherits="ciMes.Web.Common.UserControl.DateTimeTextBox" %>
<asp:TextBox ID="ttbdate" runat="server" CssClass="CSInput" Width="100%"></asp:TextBox>
<ajaxToolkit:calendarextender ID="Calender" runat="server" targetcontrolid="ttbDate" format="yyyy/MM/dd hh:mm:ss"
    popupbuttonid="ttbDate" />
