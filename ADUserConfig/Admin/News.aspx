<%@ Page Title="Nyheder" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="Admin_News" %>
<%@ Register Src="~/Controls/AdvancedTextbox.ascx" TagPrefix="uc1" TagName="AdvancedTextbox" %>
<%@ Import Namespace="Stiig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:DropDownList ID="ddlNews" runat="server" OnSelectedIndexChanged="ddlNews_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="sdsAllNews" DataTextField="Text" DataValueField="ID">
        <asp:ListItem>-Opret ny-</asp:ListItem>
    </asp:DropDownList>
    <asp:SqlDataSource ID="sdsAllNews" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [ID], (CONVERT(VARCHAR, [ID]) + ' - ' + [Title]) AS [Text] FROM [News] ORDER BY [ID] DESC"></asp:SqlDataSource>
    <br /><br />
    <asp:Panel ID="pnlShow" Visible="true" runat="server">
        <table cellpadding="5" style="width:900px;">
            <tr>
                <td class='<% Response.Write("infoTitle infobox" + (AdminOnly ? " adminbg" : "")); %>'>
                    <div style="float:left;">
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    </div>
                    <div style="float:right;">
                        <asp:Label ID="lblTimeStamp" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class='<% Response.Write("infoTitle infobox" + (AdminOnly ? " adminbg" : "")); %>'>
                    <asp:Label ID="lblText" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
    <asp:DetailsView ID="dvNews" runat="server" OnDataBound="dvNews_DataBound" AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="sdsNews" OnItemUpdating="dvNews_ItemUpdating" OnItemInserting="dvNews_ItemInserting">
        <Fields>
            <asp:TemplateField HeaderText="Titel" SortExpression="Title">
                <EditItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server" Text='<%# Bind("Title") %>' MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="Du skal udfylde en titel" ControlToValidate="txtTitle" CssClass="error" Text="*"></asp:RequiredFieldValidator>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server" Text='<%# Bind("Title") %>' MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="Du skal udfylde en titel" ControlToValidate="txtTitle" CssClass="error" Text="*"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tekst" SortExpression="Text">
                <EditItemTemplate>
                    <uc1:AdvancedTextbox runat="server" Text='<%# Bind("Text") %>' Width="600" ID="advtxtText" />
                    <asp:RequiredFieldValidator ID="rfvText" runat="server" ErrorMessage="Du skal udfylde en tekst" ControlToValidate="advtxtText:txtText" CssClass="error" Text="*"></asp:RequiredFieldValidator>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <uc1:AdvancedTextbox runat="server" Text='<%# Bind("Text") %>' Width="600" ID="advtxtText" />
                    <asp:RequiredFieldValidator ID="rfvText" runat="server" ErrorMessage="Du skal udfylde en tekst" ControlToValidate="advtxtText:txtText" CssClass="error" Text="*"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Synlighed" SortExpression="AdminOnly">
                <EditItemTemplate>
                    <asp:RadioButtonList ID="rblAdminOnly" RepeatDirection="Horizontal" SelectedValue='<%# Bind("AdminOnly") %>' runat="server">
                        <asp:ListItem Value="True">Admins</asp:ListItem>
                        <asp:ListItem Value="False">Alle</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:CheckBox ID="chkbxSticky" Checked='<%# Bind("Sticky") %>' Text="Sticky" runat="server" />
                    <asp:CheckBox ID="chkbxActive" Checked='<%# Bind("Active") %>' Text="Aktiv" runat="server" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:RadioButtonList ID="rblAdminOnly" RepeatDirection="Horizontal" SelectedValue='<%# Bind("AdminOnly") %>' runat="server">
                        <asp:ListItem Value="True" Selected="True">Admins</asp:ListItem>
                        <asp:ListItem Value="False">Alle</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:CheckBox ID="chkbxSticky" Checked='<%# Bind("Sticky") %>' Text="Sticky" runat="server" />
                    <asp:CheckBox ID="chkbxActive" Checked='<%# Bind("Active") %>' Text="Aktiv" runat="server" />
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName="Update" Text="Gem"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annuller"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Slet" OnClientClick="return confirm('Er du sikker på at du vil slette denne nyhed?')"></asp:LinkButton>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:LinkButton ID="lnkbtnCreate" runat="server" CausesValidation="True" CommandName="Insert" Text="Opret"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annuller"></asp:LinkButton>
                </InsertItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="sdsNews" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" OnSelecting="sdsNews_Selecting" OnSelected="sdsNews_Selected" OnDeleted="sdsNews_Deleted" OnUpdated="sdsNews_Updated" OnInserted="sdsNews_Inserted" DeleteCommand="NewsDelete" DeleteCommandType="StoredProcedure" InsertCommandType="StoredProcedure" InsertCommand="NewsCreate" SelectCommand="SELECT * FROM [News] WHERE [ID] = @ID" UpdateCommandType="StoredProcedure" UpdateCommand="NewsUpdate">
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="AdminOnly" Type="Boolean" />
            <asp:Parameter Name="Sticky" Type="Boolean" />
            <asp:Parameter Name="Title" Type="String" />
            <asp:Parameter Name="Text" Type="String" />
            <asp:Parameter Name="Active" Type="Boolean" />
            <asp:Parameter Name="NewID" Direction="Output" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="AdminOnly" Type="Boolean" />
            <asp:Parameter Name="Sticky" Type="Boolean" />
            <asp:Parameter Name="Title" Type="String" />
            <asp:Parameter Name="Text" Type="String" />
            <asp:Parameter Name="Active" Type="Boolean" />
            <asp:Parameter Name="ID" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>