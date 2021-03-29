<%@ Page Title="Admin" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <div style="float:left;">
        <asp:Table ID="tblOverview" CellPadding="10" GridLines="Horizontal" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HyperLink ID="hplBULKs" runat="server" NavigateUrl="~/Admin/BULKs">BULKs</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblBULKs" runat="server" Text="Administrér BULKs"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HyperLink ID="hplLog" runat="server" NavigateUrl="~/Admin/Log">Log</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblLog" runat="server" Text="Vis log"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HyperLink ID="hplEditUsers" runat="server" NavigateUrl="~/Admin/EditUsers">Brugere</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblEditUsers" runat="server" Text="Rediger brugere her på siden (ikke AD)"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HyperLink ID="hplEditData" runat="server" NavigateUrl="~/Admin/EditData">Data</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblEditData" runat="server" Text="Rediger data"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HyperLink ID="hplNews" runat="server" NavigateUrl="~/Admin/News">News</asp:HyperLink>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblNews" runat="server" Text="Opret/rediger nyheder"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="tbrwADUCMobile" runat="server">
                <asp:TableCell>
                    <asp:HyperLink NavigateUrl="~/Admin/ADUCMobile.apk" runat="server">ADUCMobile.Android</asp:HyperLink>                    
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" Text="Android app" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div style="float:right;">
        <asp:GridView ID="gvUserCount" runat="server" CellPadding="5" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="OU" SortExpression="DisplayName">
                    <ItemTemplate>
                        <asp:Label ID="lblDisplayName" runat="server" ToolTip='<%# Eval("DistinguishedName") %>' Text='<%# Bind("DisplayName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Antal brugere" SortExpression="UserCount">
                    <ItemTemplate>
                        <asp:Label ID="lblUserCount" runat="server" Text='<%# Bind("UserCount") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>