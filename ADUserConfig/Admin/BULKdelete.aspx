<%@ Page Title="BULK slet" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BULKdelete.aspx.cs" Inherits="Admin_BULKdelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    Én bruger pr. linje! Foranstillede nuller og mellemrum bliver trimmet fra<br />
    <div style="float:left;margin-right:30px;">
        <asp:TextBox ID="txtUsersToDelete" TextMode="MultiLine" runat="server" Height="300px"></asp:TextBox><br />
        <asp:Button ID="btnRun" OnClick="btnRun_Click" runat="server" Text="Slet" />
    </div>
    <div style="float:left;">
        <asp:TextBox ID="txtOutput" placeholder="Output" TextMode="MultiLine" Width="400px" ReadOnly="true" Height="300px" runat="server"></asp:TextBox>
    </div>
</asp:Content>