<%@ Page Title="Log ind" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="public_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:Login ID="Login1" TitleText="Log ind" runat="server" OnLoginError="Login1_LoginError" FailureText="Du blev ikke logget ind. Prøv igen." LoginButtonText="Log Ind" RememberMeText="Husk mig." UserNameLabelText="MANR:" PasswordRequiredErrorMessage="Password er påkrævet." UserNameRequiredErrorMessage="MANR er påkrævet." DestinationPageUrl="~/"></asp:Login>
</asp:Content>