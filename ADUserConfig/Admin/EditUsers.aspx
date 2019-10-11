 <%@ Page Title="Rediger ADUC brugere" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditUsers.aspx.cs" Inherits="Admin_EditUsers" %>
<%@ Register Src="~/Controls/PasswordField.ascx" TagPrefix="uc1" TagName="PasswordField" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" Runat="Server">
    <ajaxToolkit:TabContainer ID="tcUserManagement" runat="server" Width="700px">
        <ajaxToolkit:TabPanel ID="tpEdit" HeaderText="Rediger bruger" runat="server">
            <ContentTemplate>
                <asp:Table ID="tblEditUser" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:Label ID="lblEditMsg" Font-Bold="true" runat="server" Visible="false" Text="lblEditMsg"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="lblEditUser" runat="server" AssociatedControlID="ddlEditUser" Text="Bruger:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlEditUser" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="DisplayName" DataValueField="MANR" OnSelectedIndexChanged="ddlEditUser_SelectedIndexChanged" runat="server">
                                <asp:ListItem>-Vælg bruger-</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="lblEditFirstname" runat="server" AssociatedControlID="txtEditFirstname" Text="Fornavn(e):"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtEditFirstname" Enabled="false" runat="server" MaxLength="100"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="lblEditLastname" runat="server" AssociatedControlID="txtEditLastname" Text="Efternavn:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtEditLastname" Enabled="false" runat="server" MaxLength="50"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="lblEditEnabled" runat="server" AssociatedControlID="chkbxEditEnabled" Text="Aktiv:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:CheckBox ID="chkbxEditEnabled" runat="server" Enabled="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="lblEditAdmin" runat="server" AssociatedControlID="chkbxEditAdmin" Text="Admin:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:CheckBox ID="chkbxEditAdmin" runat="server" Enabled="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="lblEditPassword" runat="server" AssociatedControlID="txtEditPassword" Text="Skift password:"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:CheckBox ID="chkbxEditPassword" Enabled="false" Checked="false" runat="server" />
                            <asp:TextBox ID="txtEditPassword" runat="server" ReadOnly="True" Width="180" MaxLength="50" Enabled="False"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Button ID="btnEditUser" runat="server" Text="Opdater bruger" Enabled="false" OnClick="btnEditUser_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnEditUnlock" runat="server" Visible="false" OnClick="btnEditUnlock_Click" Text="Lås op" />&nbsp;
                            <asp:Button ID="btnEditDelete" runat="server" Text="Slet bruger" OnClientClick="return confirm('Er du sikker på at du vil slette denne bruger?')" OnClick="btnEditDelete_Click" Enabled="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="tpCreateUser" HeaderText="Opret bruger" runat="server">
            <ContentTemplate>
                <asp:CreateUserWizard ID="cuwCreateUser" runat="server" LoginCreatedUser="False" UserNameLabelText="MANR" OnCreatedUser="cuwCreateUser_CreatedUser" RequireEmail="False" ContinueDestinationPageUrl="~/Admin/EditUsers">
                    <WizardSteps>
                        <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server" Title="">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">MANR:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" CssClass="error" ControlToValidate="UserName" ErrorMessage="MANR er påkrævet." ToolTip="MANR er påkrævet." ValidationGroup="cuwCreateUser">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Password" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" CssClass="error" ControlToValidate="Password" ErrorMessage="Password er påkrævet." ToolTip="Password er påkrævet." ValidationGroup="cuwCreateUser">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblFirstname" runat="server" AssociatedControlID="txtFirstname">Fornavn(e):</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFirstname" runat="server" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFirstname" runat="server" CssClass="error" ControlToValidate="txtFirstname" ErrorMessage="Fornavn er påkrævet." ToolTip="Fornavn er påkrævet." ValidationGroup="cuwCreateUser">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblLastname" runat="server" AssociatedControlID="txtLastname">Efternavn:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLastname" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLastname" runat="server" CssClass="error" ControlToValidate="txtLastname" ErrorMessage="Efternavn er påkrævet." ToolTip="Efternavn er påkrævet." ValidationGroup="cuwCreateUser">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="color:Red;">
                                            <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:CreateUserWizardStep>
                        <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                </asp:CreateUserWizard>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    <br />
    <br />
    <asp:LinkButton ID="lnkbtnShowUsers" OnClick="lnkbtnShowUsers_Click" runat="server">Vis nuværende brugere</asp:LinkButton>
    <asp:GridView ID="gvUsers" CellPadding="5" AllowSorting="true" OnSorting="gvUsers_Sorting" OnRowDataBound="gvUsers_RowDataBound" AutoGenerateColumns="false" runat="server">
        <Columns>
            <asp:BoundField DataField="MANR" HeaderText="MANR" SortExpression="MANR" />
            <asp:BoundField DataField="Fullname" HeaderText="Navn" SortExpression="Fullname" />
            <asp:TemplateField HeaderText="Godkendt" SortExpression="IsApproved">
                <ItemTemplate>
                    <asp:Label ID="lblIsApproved" runat="server" Text='<%# (bool)Eval("IsApproved") ? "Ja" : "Nej" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Låst" SortExpression="IsLockedOut">
                <ItemTemplate>
                    <asp:Label ID="lblIsLockedOut" runat="server" Text='<%# (bool)Eval("IsLockedOut") ? "Ja" : "Nej" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Admin" SortExpression="IsAdmin">
                <ItemTemplate>
                    <asp:Label ID="lblIsAdmin" runat="server" Text='<%# (bool)Eval("IsAdmin") ? "Ja" : "Nej" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedDate" HeaderText="Oprettet" SortExpression="CreatedDate" />
            <asp:BoundField DataField="LastLoginDate" HeaderText="Sidste login" SortExpression="LastLoginDate" />
        </Columns>
    </asp:GridView>
</asp:Content>