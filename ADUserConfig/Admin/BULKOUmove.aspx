<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BULKOUmove.aspx.cs" Inherits="Admin_BULKOUmove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <table>
        <tr>
            <td>
                <asp:TextBox ID="txtInput" runat="server" Width="300" Height="400" placeholder="123456;TRR-XXXX" TextMode="MultiLine"></asp:TextBox><br />
                <asp:Button ID="btnRun" runat="server" Text="Kør" OnClick="btnRun_Click" />
            </td>
            <td>
                <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>