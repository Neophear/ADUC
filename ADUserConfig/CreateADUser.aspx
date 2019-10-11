<%@ Page Title="Opret AD bruger" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateADUser.aspx.cs" Inherits="CreateADUser" %>

<%@ Register Src="~/Controls/PasswordField.ascx" TagPrefix="uc1" TagName="PasswordField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" runat="Server">
    <asp:Panel ID="pnlCreateUser" DefaultButton="btnCreate" runat="server">
        <table>
            <tr>
                <td style="width: 50%;">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text="lblMessage" Visible="false"></asp:Label>
                    <asp:HyperLink ID="hplPrint" Target="_blank" Font-Bold="true" Visible="false" runat="server">Print</asp:HyperLink>
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblMANR" runat="server" AssociatedControlID="txtMANR" Text="MANR:"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtMANR" runat="server" MaxLength="12"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMANR" runat="server" ErrorMessage="MANR er påkrævet." CssClass="error" ToolTip="MANR er påkrævet." Text="*" ControlToValidate="txtMANR"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblFirstname" runat="server" AssociatedControlID="txtFirstname" Text="Fornavn(e):"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtFirstname" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFirstname" runat="server" ErrorMessage="Fornavn(e) er påkrævet." CssClass="error" ToolTip="Fornavn(e) er påkrævet." Text="*" ControlToValidate="txtFirstname"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblLastname" runat="server" AssociatedControlID="txtLastname" Text="Efternavn:"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:TextBox ID="txtLastname" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLastname" runat="server" ErrorMessage="Efternavn er påkrævet." CssClass="error" ToolTip="Efternavn er påkrævet." Text="*" ControlToValidate="txtLastname"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblOU" runat="server" AssociatedControlID="ddlOU" Text="Enhed:"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <asp:DropDownList ID="ddlOU" runat="server" DataSourceID="sdsOU" DataTextField="DisplayName" AppendDataBoundItems="true" DataValueField="DistinguishedName">
                                    <asp:ListItem Selected="True">-Vælg enhed-</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvOU" runat="server" ErrorMessage="Enhed er påkrævet." CssClass="error" ToolTip="Enhed er påkrævet." ControlToValidate="ddlOU" InitialValue="-Vælg enhed-" Text="*"></asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="sdsOU" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [DistinguishedName], [DisplayName] FROM [OUs] WHERE ([Enabled] = @Enabled) ORDER BY [DisplayName]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="True" Name="Enabled" Type="Boolean" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblPassword" runat="server" AssociatedControlID="pwfPassword:txtPassword" Text="Password:"></asp:Label>
                            </td>
                            <td style="text-align: right;">
                                <uc1:PasswordField runat="server" ID="pwfPassword" />
                            </td>
                        </tr>
                        <asp:Panel ID="pnlForceChangePassword" Visible='<%# User.IsInRole("Admin") %>' runat="server">
                            <tr class="adminbg">
                                <td align="right">
                                    <asp:Label ID="lblForceChangePassword" runat="server" AssociatedControlID="chkbxForceChangePassword" Text="Skal skifte password:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <script type="text/javascript">
                                        function checkchkbx(sender) {
                                            document.getElementById('<%= chkbxForceChangePassword.ClientID %>').checked = (sender.attributes["data-password"].value !== sender.value)
                                        };
                                    </script>
                                    <asp:CheckBox ID="chkbxForceChangePassword" runat="server" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td class="alignright">
                                <asp:Label ID="lblExpires" runat="server" Text="Udløber:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpires" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="cvExpires" runat="server" CssClass="error" OnServerValidate="cvExpires_ServerValidate" ControlToValidate="txtExpires" ClientValidationFunction="validateExpireDate" ErrorMessage="Udløbsdato er ikke valid" Text="*"></asp:CustomValidator>
                                <script type="text/javascript">
                                    function validateExpireDate(s, args) {
                                        args.IsValid = false;
                                        var date = parseDate(args.Value);
                                        var isDate = date != null;

                                        if (isDate) {
                                            var today = new Date();
                                            today.setHours(0, 0, 0, 0);
                                            args.IsValid = date >= today;
                                        }
                                    }
                                    function parseDate(str) {
                                        var m = str.match(/^(((0[1-9]|[12]\d|3[01])\-(0[13578]|1[02])\-((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\-(0[13456789]|1[012])\-((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\-02\-((19|[2-9]\d)\d{2}))|(29\-02\-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/g);
                                        var date = null;

                                        if (m) {
                                            var parts = m[0].split('-');
                                            date = new Date(parts[2], parts[1] - 1, parts[0]);
                                        }
                                        return date;
                                    }
                                </script>
                                <ajaxToolkit:CalendarExtender ID="ceExpires" Format="dd-MM-yyyy" TodaysDateFormat="dd. MMMM, yyyy" TargetControlID="txtExpires" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnCreate" runat="server" Text="Opret" OnClick="btnCreate_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSendEmail" CausesValidation="false" runat="server" OnClick="btnSendEmail_Click" Text="Send Email" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top; padding: 20px;">
                    <asp:Label ID="lblInfo" runat="server" CssClass="info"></asp:Label>
                    <p class="error">Har bruger ikke MANR, skal fødselsdato + initialer bruger!</p>
                    <p class="error">Fx John Doe, født 14. februar 1970, oprettes således:
                        <br />140270JD</p>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
