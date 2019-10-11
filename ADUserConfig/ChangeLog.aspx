<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangeLog.aspx.cs" Inherits="ChangeLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:GridView ID="gvChangelog" runat="server" AutoGenerateColumns="False" CellPadding="5" DataKeyNames="ID" DataSourceID="sdsChangelog">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="False" />
            <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" SortExpression="Timestamp" />
            <asp:BoundField DataField="Text" HeaderText="Text" SortExpression="Text" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsChangelog" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" OnSelecting="sdsChangelog_Selecting" SelectCommandType="StoredProcedure" SelectCommand="GetChangeLog">
        <SelectParameters>
            <asp:Parameter Name="IsAdmin" Type="Boolean" />
            <asp:Parameter Name="OnlyTop" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>