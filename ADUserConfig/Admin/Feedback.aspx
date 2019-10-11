<%@ Page Title="Admin/Feedback" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Admin_Feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:GridView ID="gvFeedback" runat="server" AutoGenerateColumns="False" CellPadding="5" DataKeyNames="ID" DataSourceID="sdsFeedback" AllowSorting="True">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="Fullname" HeaderText="Fullname" SortExpression="Fullname" ReadOnly="True" />
            <asp:BoundField DataField="TimeStamp" HeaderText="TimeStamp" SortExpression="TimeStamp" />
            <asp:TemplateField HeaderText="Text" SortExpression="Text">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Text").ToString().Replace("\n", "<br />") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsFeedback" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommandType="StoredProcedure" SelectCommand="GetFeedback"></asp:SqlDataSource>
</asp:Content>