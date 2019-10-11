<%@ Page Title="Log" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Log.aspx.cs" Inherits="Admin_Log" %>
<%@ Import Namespace="Stiig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:Panel ID="pnlLog" DefaultButton="btnLoadLog" runat="server">
        <asp:Label ID="lblError" runat="server" Text="lblError" Visible="false" CssClass="error"></asp:Label><br />
        <asp:CompareValidator ID="cvDateFrom" runat="server" CssClass="error" ErrorMessage="Ikke en valid Fra-dato" ControlToValidate="txtDateFrom" Operator="DataTypeCheck" SetFocusOnError="True" Type="Date" Text="Ikke en valid Fra-dato" Display="Dynamic"></asp:CompareValidator>
        <asp:CompareValidator ID="cvDateTo" runat="server" ErrorMessage="Ikke en valid Til-dato" ControlToValidate="txtDateTo" CssClass="error" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" Type="Date" Text="Ikke en valid Til-dato"></asp:CompareValidator>
        <script type="text/javascript">
            //$(document).ready(function(){
            //   ShowHideDates();
            //});
            function ShowHideDates() {
                var chkbxWithDates = document.getElementById('<%= chkbxWithDates.ClientID %>');
                var txtDateFrom = document.getElementById('<%= txtDateFrom.ClientID %>');
                var txtDateTo = document.getElementById('<%= txtDateTo.ClientID %>');

                txtDateFrom.hidden = !chkbxWithDates.checked;
                txtDateTo.hidden = !chkbxWithDates.checked;
            }
        </script>
        <asp:CheckBox ID="chkbxWithDates" Checked="true" runat="server" onclick="ShowHideDates()" Text="Med datoer" /><br />
        <asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" MaxLength="50"></asp:TextBox>
        <asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" MaxLength="50"></asp:TextBox>
        <asp:TextBox ID="txtSearchTerm" placeholder="Søgetekst" runat="server" Height="21"></asp:TextBox>
        <asp:DropDownList ID="ddlTypes" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Value="">-Alle-</asp:ListItem>
        </asp:DropDownList>
        <asp:CheckBox ID="chkbxOnlyCurrentUser" runat="server" Text="Kun mine" Checked="false" />
        <asp:Button ID="btnLoadLog" runat="server" Text="Hent log" OnClick="btnLoadLog_Click" />
        <br /><br />
        <asp:GridView ID="gvLog" runat="server" style="width:100%" CellPadding="5" AutoGenerateColumns="False" OnRowDataBound="gvLog_RowDataBound" AllowSorting="True" DataKeyNames="ID" DataSourceID="sdsLog">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" ReadOnly="True" SortExpression="ID" />
                <asp:TemplateField HeaderText="Tid" SortExpression="TimeStamp">
                    <ItemTemplate>
                        <asp:Label ID="lblTimeStamp" runat="server" Text='<%# ((DateTime)Eval("TimeStamp")).ToString("dd-MM-yy HH:mm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ActionID" HeaderText="ActionID" Visible="false" SortExpression="ActionID" />
                <asp:BoundField DataField="Action" HeaderText="Handling" SortExpression="Action" />
                <asp:BoundField DataField="ObjectType" HeaderText="Type" SortExpression="ObjectType" />
                <asp:BoundField DataField="ObjectName" HeaderText="Objekt" SortExpression="ObjectName" />
                <asp:BoundField DataField="Password" HeaderText="Kode" SortExpression="Password" />
                <asp:TemplateField HeaderText="Skift" SortExpression="MustChangePassword">
                    <ItemTemplate>
                        <asp:Label ID="lblMustChangePassword" runat="server" Text='<%# Eval("MustChangePassword") == DBNull.Value ? "" : (bool)Eval("MustChangePassword") ? "Ja" : "Nej" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lavet af" SortExpression="Fullname">
                    <ItemTemplate>
                        <asp:Label ID="lblDisplayName" runat="server" ToolTip='<%# Eval("Fullname") %>' Text='<%# Eval("DisplayName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sdsLog" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure" SelectCommand="GetLog">
            <SelectParameters>
                <asp:Parameter DbType="Date" Name="DateFrom" ConvertEmptyStringToNull="true" />
                <asp:Parameter DbType="Date" Name="DateTo" ConvertEmptyStringToNull="true" />
                <asp:Parameter DbType="Int32" Name="CategoryID" ConvertEmptyStringToNull="true" />
                <asp:Parameter DbType="String" Name="Username" DefaultValue="" ConvertEmptyStringToNull="true" />
                <asp:Parameter DbType="String" Name="SearchTerm" ConvertEmptyStringToNull="true" />
            </SelectParameters>
        </asp:SqlDataSource>
    </asp:Panel>
</asp:Content>