<%@ Page Title="Rediger AD bruger" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditADUser.aspx.cs" Inherits="EditADUser" %>

<%@ Register Src="~/Controls/PasswordField.ascx" TagPrefix="uc1" TagName="PasswordField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" runat="Server">
    <table>
        <tr>
            <td style="width: 50%;">
                <asp:Panel ID="pnlEditUser" DefaultButton="btnSearch" runat="server">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="Larger" Text="lblMessage" Visible="false"></asp:Label>
                    <asp:HyperLink ID="hplPrint" Target="_blank" Font-Size="Larger" runat="server">Print</asp:HyperLink><br />
                    <table style="border-collapse: collapse; width:100%">
                        <tr>
                            <td class="alignright">
                                <asp:Label ID="lblMANR" runat="server" AssociatedControlID="txtMANR" Text="MANR:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMANR" runat="server" MaxLength="12"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMANR" runat="server" ErrorMessage="MANR er påkrævet." ValidationGroup="Search" CssClass="error" ToolTip="MANR er påkrævet." Text="*" ControlToValidate="txtMANR"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" ValidationGroup="Search" Text="Søg" />
                            </td>
                        </tr>
                        <asp:Panel ID="pnlEdit" Visible="false" runat="server">
                            <tr>
                                <td class="alignright">
                                    <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="Navn:"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtName" Enabled="false" ForeColor="Black" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="alignright">
                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="pwfPassword:txtPassword" Text="Password:"></asp:Label>
                                </td>
                                <td>
                                    <uc1:PasswordField runat="server" ID="pwfPassword" />
                                    <asp:CustomValidator ID="cvPassword" runat="server" ValidationGroup="Edit" CssClass="error" OnServerValidate="cvPassword_ServerValidate" ClientValidationFunction="validatePassword" ErrorMessage="Password opfylder ikke kravene" Display="None"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function validatePassword(s, args) {
                                            if (document.getElementById('<%= chkbxChangePassword.ClientID %>').checked)
                                                args.IsValid = checkPassword();
                                            else
                                                args.IsValid = true;
                                        }
                                    </script>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkbxChangePassword" runat="server" Text="Skift" />
                                </td>
                            </tr>
                            <tr>
                                <td class="alignright">
                                    <asp:Label ID="lblLocked" runat="server" Text="Låst:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblLockedStatus" runat="server" Text="lblLockedStatus"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkbxUnlock" Text="Lås op" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="alignright">
                                    <asp:Label ID="lblExpires" runat="server" Text="Udløber:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpires" OnKeyUp="setExpireCheckBox()" onchange="setExpireCheckBox()" runat="server"></asp:TextBox>
                                    <asp:CustomValidator ID="cvExpires" runat="server" ValidationGroup="Edit" OnServerValidate="cvExpires_ServerValidate" ControlToValidate="txtExpires" ClientValidationFunction="validateExpireDate" ErrorMessage="Udløbsdato er ikke valid" Display="None"></asp:CustomValidator>
                                    <script type="text/javascript">
                                        function validateExpireDate(s, args) {
                                            if (document.getElementById('<%= chkbxExpires.ClientID %>').checked) {
                                                args.IsValid = false;
                                                var date = parseDate(args.Value);
                                                var isDate = date != null;

                                                if (isDate) {
                                                    var today = new Date();
                                                    today.setHours(0, 0, 0, 0);
                                                    args.IsValid = date >= today;
                                                }
                                            }
                                            else
                                                args.IsValid = true;
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
                                        function setExpireCheckBox() {
                                            document.getElementById('<%= chkbxExpires.ClientID %>').checked = true;
                                        }
                                    </script>
                                    <ajaxToolkit:CalendarExtender ID="ceExpires" TodaysDateFormat="dd. MMMM, yyyy" Format="dd-MM-yyyy" TargetControlID="txtExpires" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkbxExpires" runat="server" Text="Skift" />
                                </td>
                            </tr>
                            <asp:Panel ID="pnlAdminOnly" Visible="false" runat="server">
                                <tr class="adminbg">
                                    <td class="alignright">
                                        <asp:Label ID="lblMustChangePassword" runat="server" Text="Tving skift:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMustChangePassword2" runat="server" Text="Skal skifte password"></asp:Label>
                                    </td>
                                    <td>
                                        <script type="text/javascript">
                                            function checkchkbx(sender) {
                                                document.getElementById('<%= chkbxChangePassword.ClientID %>').checked = true
                                                document.getElementById('<%= chkbxForceChangePassword.ClientID %>').checked = sender.attributes["data-password"].value !== sender.value
                                            };
                                        </script>
                                        <asp:CheckBox ID="chkbxForceChangePassword" runat="server" />
                                    </td>
                                </tr>
                                <tr class="adminbg">
                                    <td>
                                        <asp:Label ID="lblCurrentOU" runat="server" Text="Nuværende OU:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOU" Enabled="false" ForeColor="Black" Font-Size="Smaller" TextMode="MultiLine" Height="100" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="adminbg">
                                    <td class="alignright">
                                        <asp:Label ID="lblChangeOU" runat="server" Text="Skift OU:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOU" runat="server" DataSourceID="sdsOU" onchange="onOUChange(this)" DataTextField="DisplayName" AppendDataBoundItems="true" DataValueField="DistinguishedName">
                                            <asp:ListItem Value="0">-Vælg enhed-</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CustomValidator ID="cvOU" runat="server" ValidationGroup="Edit" ControlToValidate="ddlOU" CssClass="error" OnServerValidate="cvOU_ServerValidate" ClientValidationFunction="validateOU" ErrorMessage="Du skal vælge en OU" Display="None"></asp:CustomValidator>
                                        <script type="text/javascript">
                                            function onOUChange(ddlOU) {
                                                var chkbxMoveOU = document.getElementById('<%= chkbxMoveOU.ClientID %>');
                                                chkbxMoveOU.checked = ddlOU.selectedIndex != 0;
                                            }
                                            function validateOU(s, args) {
                                                if (document.getElementById('<%= chkbxMoveOU.ClientID %>').checked)
                                                    args.IsValid = args.Value != '0';
                                                else
                                                    args.IsValid = true;
                                            }
                                        </script>
                                        <asp:SqlDataSource ID="sdsOU" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [DistinguishedName], [DisplayName] FROM [OUs] WHERE ([Enabled] = @Enabled) ORDER BY [DisplayName]">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="True" Name="Enabled" Type="Boolean" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkbxMoveOU" runat="server" />
                                    </td>
                                </tr>
                                <tr class="adminbg">
                                    <td class="alignright">
                                        <asp:Label ID="lblGroups" runat="server" Text="Grupper:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <%--<asp:TextBox ID="txtGroups" Enabled="false" ForeColor="Black" Font-Size="Smaller" TextMode="MultiLine" Height="100" runat="server"></asp:TextBox>--%>
                                        <%--<asp:DropDownList ID="ddlGroups" SelectMethod="Multiple" runat="server"></asp:DropDownList>--%>
                                        <asp:ListBox ID="lstGroups" SelectionMode="Multiple" runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr class="adminbg">
                                    <td class="alignright">
                                        <asp:Label ID="lblDeleteUser" runat="server" Text="Slet bruger:"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btnDeleteUser" OnClick="btnDeleteUser_Click" runat="server" Text="Slet bruger" OnClientClick="return confirm('Er du sikker på at du vil slette denne bruger?')" />
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="btnSave" runat="server" ValidationGroup="Edit" OnClick="btnSave_Click" Text="Gem ændringer" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="btnSendEmail" runat="server" OnClick="btnSendEmail_Click" Text="Send Email" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td colspan="3">
                                <asp:ValidationSummary ID="vsSummery" DisplayMode="BulletList" ShowValidationErrors="true" ValidationGroup="Edit" ShowSummary="true" CssClass="error" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td style="vertical-align: top; padding: 20px;">
                <asp:Label ID="lblPasswordWarning" CssClass="warning" runat="server" Text="lblPasswordWarning" Visible="false"></asp:Label>
                <asp:Label ID="lblInfo" runat="server" CssClass="info"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
