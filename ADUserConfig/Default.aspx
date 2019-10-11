<%@ Page Title="Forside" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="Stiig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <meta http-equiv="refresh" content="60" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <h2>Velkommen til ADUC</h2>
    <table class="landingpageTable">
        <tr>
            <td style="vertical-align:top;">
                <asp:ListView ID="lvNews" runat="server" DataSourceID="sdsNews" DataKeyField="ID">
                    <LayoutTemplate>
                        <table cellpadding="5" style="border-collapse:collapse;">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr <%# (bool)Eval("Sticky") ? "class='newsStickyTop'" : "" %>>
                            <td class='<%# "infoTitle infobox" + ((bool)Eval("AdminOnly") ? " adminbg" : "") %>'>
                                <div style="float:left;">
                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                </div>
                                <div style="float:right;">
                                    <asp:Label ID="lblTimeStamp" runat="server" Text='<%# Utilities.GetFriendlyTime((DateTime)Eval("TimeStamp")) %>'></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr <%# !(bool)Eval("Sticky") ? "" : ((bool)User.IsInRole("Admin") ? "class='newsSticky'" : "class='newsStickyBottom'") %>>
                            <td class='<%# "infobox" + ((bool)Eval("AdminOnly") ? " adminbg" : "") %>'>
                                <asp:Label ID="lblText" runat="server" Text='<%# Utilities.BBToHTML(Eval("Text").ToString()) %>'></asp:Label>
                            </td>
                        </tr>
                        <asp:Panel ID="pnlEdit" Visible='<%# User.IsInRole("Admin") %>' runat="server">
                            <tr <%# (bool)Eval("Sticky") ? "class='newsStickyBottom'" : "" %>>
                                <td class="newsFooter infobox adminbg">
                                    <asp:HyperLink ID="hplEdit" CssClass="newsFooter" NavigateUrl='<%# "~/Admin/News/" + Eval("ID") %>' runat="server">Rediger</asp:HyperLink>
                                </td>
                            </tr>
                        </asp:Panel>
                    </ItemTemplate>
                    <ItemSeparatorTemplate>
                        <tr>
                            <td style="height:10px;"></td>
                        </tr>
                    </ItemSeparatorTemplate>
                </asp:ListView><br />
                <asp:DataPager ID="dpNews" runat="server" PagedControlID="lvNews" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField FirstPageText="Første" LastPageText="Sidste" NextPageText="Næste" PreviousPageText="Forrige" />
                        <asp:NumericPagerField />
                    </Fields>
                </asp:DataPager>
                <asp:SqlDataSource ID="sdsNews" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" OnSelected="sdsNews_Selected" OnSelecting="sdsNews_Selecting" SelectCommand="GetNews" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="IsAdmin" Type="Boolean" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td></td>
            <td style="vertical-align:top;">
                <asp:ListView ID="lvChangeLog" runat="server" DataSourceID="sdsChangeLog">
                    <LayoutTemplate>
                        <table cellpadding="5" style="border-collapse:collapse;width:150px;">
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                            <tr>
                                <td style="text-align:right;">
                                    <asp:HyperLink ID="hplChangeLog" Font-Size="Smaller" NavigateUrl="~/ChangeLog" runat="server">Se alle ændringer</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class='<%# "changelog infobox" + ((bool)Eval("AdminOnly") ? " adminbg" : "") %>'>
                                <div style="float:right;">
                                    <asp:Label ID="lblTimeStamp" runat="server" Text='<%# Utilities.GetFriendlyTime((DateTime)Eval("Timestamp")) %>'></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="changelog infobox">
                                <asp:Label ID="lblText" runat="server" Text='<%# Eval("Text") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <ItemSeparatorTemplate>
                        <tr>
                            <td></td>
                        </tr>
                    </ItemSeparatorTemplate>
                </asp:ListView>
                <asp:SqlDataSource ID="sdsChangeLog" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" OnSelecting="sdsChangeLog_Selecting" SelectCommandType="StoredProcedure" SelectCommand="GetChangeLog">
                    <SelectParameters>
                        <asp:Parameter Name="IsAdmin" Type="Boolean" />
                        <asp:Parameter Name="OnlyTop" Type="Boolean" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>