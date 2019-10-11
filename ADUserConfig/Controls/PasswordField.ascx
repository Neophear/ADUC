<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PasswordField.ascx.cs" Inherits="PasswordField" %>

<script type="text/javascript">
    var txtPassword;

    window.onload = function () {
        txtPassword = document.getElementById('<%= txtPassword.ClientID %>');

        if (txtPassword.value != "" && !txtPassword.readOnly)
            checkPassword();
    }
    function checkPassword() {
        var checkRules = false;
        var checkLength = false;
        var checkAccount = false;
        var isValid = false;

        var pc0 = document.getElementById("pc0");
        var pc1 = document.getElementById("pc1");
        var pc2 = document.getElementById("pc2");
        var pc3 = document.getElementById("pc3");
        var pc4 = document.getElementById("pc4");
        var pc5 = document.getElementById("pc5");
        var pc6 = document.getElementById("pc6");

        if (txtPassword.value.length == 0) {
            pc0.style.color = "black";
            pc1.style.color = "black";
            pc2.style.color = "black";
            pc3.style.color = "black";
            pc4.style.color = "black";
            pc5.style.color = "black";
            pc6.style.color = "black";
        }
        else {
            var counter = 0;

            //Check for capital letter
            if (/[A-Z]/.test(txtPassword.value)) {
                counter++;
                pc1.style.color = "forestgreen";
            }
            else
                pc1.style.color = "black";

            //Check for small letter
            if (/[a-z]/.test(txtPassword.value)) {
                counter++;
                pc2.style.color = "forestgreen";
            }
            else
                pc2.style.color = "black";

            //Check for number
            if (/[0-9]/.test(txtPassword.value)) {
                counter++;
                pc3.style.color = "forestgreen";
            }
            else
                pc3.style.color = "black";

            //Check for special characters
            if (/[\W]/.test(txtPassword.value)) {
                counter++;
                pc4.style.color = "forestgreen";
            }
            else
                pc4.style.color = "black";

            //Check if 3 or more rules are followed
            if (counter >= 3) {
                checkRules = true;
                pc0.style.color = "forestgreen";
            }
            else
                pc0.style.color = "black";

            //Check for length
            if (txtPassword.value.length >= 8) {
                checkLength = true;
                pc5.style.color = "forestgreen";
            }
            else
                pc5.style.color = "black";

            //Check for names
            //var nameControls = [txtbxNamesToJavaArray()];
            //var fullName = "";

            //for (var i = 0; i < nameControls.length; i++) {
            //    fullName += document.getElementById(nameControls[i]).value.toLowerCase() + " ";
            //}

            var accountName = document.getElementById('<%= TextBoxWithAccount %>').value.toLowerCase();

            //var names = fullName.split(' ');
            //names = names.filter(function (e) { return e; });
            var accountError = false;

            //for (var i = 0; i < (names.length) ; i++) {
            //    if (names[i].length > 2 && txtPassword.value.toLowerCase().indexOf(names[i]) != -1) {
            //        nameError = true;
            //        break;
            //    }
            //}

            if (accountName != "" && txtPassword.value.toLowerCase().indexOf(accountName) != -1) {
                accountError = true;
            }

            if (accountError)
                pc6.style.color = "black";
            else {
                checkAccount = true;
                pc6.style.color = "forestgreen";
            }

            isValid = (checkRules && checkLength && checkAccount);
        }

        //Sets background depending on if password meets complexity
        txtPassword.style.backgroundColor = isValid ? "#99FF99" : "#FFCCCC";
        return isValid;
    }
</script>
<asp:Button ID="btnGeneratePassword" runat="server" CausesValidation="false" OnClick="btnGeneratePassword_Click" Text="Generér" />
<asp:TextBox ID="txtPassword" OnKeyUp="checkPassword(this);" Width="129" MaxLength="50" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password er påkrævet." CssClass="error" ToolTip="Password er påkrævet." Text="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
<ajaxToolkit:PopupControlExtender ID="pcePasswordComplexity" TargetControlID="txtPassword" Position="Right" PopupControlID="pnlPasswordComplexity" runat="server" OffsetX="100"></ajaxToolkit:PopupControlExtender>
<asp:Panel ID="pnlPasswordComplexity" Wrap="true" Width="350" runat="server" CssClass="popupbox">
    <div style="display: block; font-size: smaller;">
        VIGTIGT: Hvis du indtaster et manuelt password, vil brugeren blive tvunget til at ændre password ved næste login.
        Dette betyder at brugeren <i>ikke</i> kan logge på WiFi før vedkommende har logget på en almindelig Internet-PC og ændret password!<br />
        Gælder ikke hvis du bruger Generér-knappen.
    </div>
    <br />
    <span id="pc0">Passwordet skal indeholde 3 af de 4 følgende tegn:</span>
    <ul>
        <li><span id="pc1">1 stort bogstav</span></li>
        <li><span id="pc2">1 småt bogstav</span></li>
        <li><span id="pc3">1 tal</span></li>
        <li><span id="pc4">1 specialtegn</span></li>
    </ul>
    <span id="pc5">Passwordet skal være på minimum 8 tegn</span><br />
    <span id="pc6">Passwordet må ikke indeholde MANR</span>
</asp:Panel>
