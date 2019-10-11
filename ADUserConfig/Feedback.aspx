<%@ Page Title="Feedback" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:Label ID="lblMsg" runat="server" Text="lblMsg"></asp:Label>
    <asp:Panel ID="pnlFeedback" runat="server">
        <table>
            <tr>
                <td style="width:50%;">
                    <asp:RequiredFieldValidator ID="rfvText" ControlToValidate="txtText" runat="server" CssClass="error" ToolTip="Feltet må ikke være tomt" ErrorMessage="Feltet må ikke være tomt" Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                    <asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" Height="115px" Width="100%"></asp:TextBox><br />
                    <asp:Button ID="btnSend" OnClick="btnSend_Click" runat="server" Text="Send" />
                </td>
                <td style="vertical-align:top;padding:20px;">
                    <asp:Label ID="lblInfo" CssClass="info" runat="server" Text="Her kan du give feedback i form af ris, ros, forslag eller andet."></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>