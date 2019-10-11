<%@ Page Title="Fejl!" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <div style="width:100%;text-align:center;">
        <br />
        <br />
        <asp:Label ID="lblErrorMessage" CssClass="error" runat="server" Text="lblErrorMessage"></asp:Label>
    </div>
</asp:Content>