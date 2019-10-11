<%@ Page Title="Indstillinger" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserSettings.aspx.cs" Inherits="UserSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:ChangePassword ID="ChangePassword1" ContinueDestinationPageUrl="~/UserSettings" runat="server"
        CancelButtonText="Annuller"
        ChangePasswordButtonText="Skift Password"
        ChangePasswordFailureText="Forkert Password. Password minimum længde: {0}."
        ChangePasswordTitleText="Skift Password"
        ConfirmNewPasswordLabelText="Bekræft Password:"
        ContinueButtonText="Fortsæt"
        NewPasswordLabelText="Nyt Password:"
        SuccessText="Dit Password er blevet ændret!"
        SuccessTitleText="Skift Password færdigt"
        UserNameLabelText="MANR:"
        ConfirmPasswordCompareErrorMessage="Begge Passwords skal være ens."
        ConfirmPasswordRequiredErrorMessage="Du skal bekræfte dit Password."
        NewPasswordRegularExpressionErrorMessage="Venligst indtast et andet Password"
        NewPasswordRequiredErrorMessage="Nyt Password er påkrævet."
        PasswordRequiredErrorMessage="Password er påkrævet."
        UserNameRequiredErrorMessage="Brugernavn er påkrævet.">
    </asp:ChangePassword>
</asp:Content>

