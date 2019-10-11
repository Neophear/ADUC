<%@ Page Title="Administrér BULKs" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BULKs.aspx.cs" Inherits="Admin_BULKs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        document.onkeyup = Esc;
        function Esc() {
            if (event.keyCode == 27) { $find("mpe").hide(); }
        }
        function pageLoad() {
            var mpe = $find("mpe");
            mpe.add_shown(onShown);
        }
        function onShown() {
            var background = $find("mpe")._backgroundElement;
            background.onclick = function () { $find("mpe").hide(); }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:UpdatePanel ID="udpBULK" UpdateMode="Conditional" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvBULKS" />
        </Triggers>
        <ContentTemplate>
            <asp:GridView ID="gvBULKS" CellPadding="10" style="width:100%" runat="server" OnSelectedIndexChanged="gvBULKS_SelectedIndexChanged" OnRowDataBound="gvBULKS_RowDataBound" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsBULKs">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                    <asp:BoundField DataField="TimeStamp" HeaderText="Oprettet" SortExpression="TimeStamp" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="Oprettet af" SortExpression="CreatedBy" />
                    <asp:TemplateField HeaderText="Skift password hvis bruger findes" SortExpression="ChangePasswordIfExist">
                        <ItemTemplate>
                            <asp:Label ID="lblChangePasswordIfExist" runat="server" Text='<%# (bool)Eval("ChangePasswordIfExist") ? "Ja" : "Nej" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Linjer kørt" SortExpression="Lines">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("SuccessfulLines") + "/" + Eval("Lines") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RunTimestamp" HeaderText="Afvist/godkendt tid" SortExpression="RunTimestamp" />
                    <asp:TemplateField HeaderText="Print">
                        <ItemTemplate>
                            <asp:HyperLink ID="hplPrintList" runat="server" Target="_blank" Visible='<%# (int)Eval("SuccessfulLines") > 0 %>' NavigateUrl='<%# String.Format("~/BULKPrint/list/{0}", Eval("ID")) %>'>Liste</asp:HyperLink>                 
                            <asp:HyperLink ID="hplPrintA5" runat="server" Target="_blank" Visible='<%# (int)Eval("SuccessfulLines") > 0 %>' NavigateUrl='<%# String.Format("~/BULKPrint/a5/{0}", Eval("ID")) %>'>A5</asp:HyperLink>                 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnShow" runat="server" CausesValidation="False" CommandName="Select" Text="Vis"></asp:LinkButton>  
                            <asp:HyperLink ID="hplDownload" runat="server" Visible='<%# (int)Eval("SuccessfulLines") > 0 %>' NavigateUrl='<%# "~/download.ashx?id=" + Eval("ID") %>'>Download</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sdsBULKs" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [BULKsOverView] ORDER BY [ID] DESC">
            </asp:SqlDataSource>
            <asp:LinkButton ID="lnkbtnFake" Text="" runat="server"></asp:LinkButton>
            <ajaxToolkit:ModalPopupExtender ID="mpeShowBULK" CancelControlID="btnClose" TargetControlID="lnkbtnFake" BehaviorID="mpe" BackgroundCssClass="modalBackground" PopupControlID="pnlShowBULK" runat="server"></ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlShowBULK" CssClass="pnlBackGround" runat="server">
                <div style="max-height:800px;overflow:auto;">
                    <asp:UpdatePanel ID="udpShowBULK" UpdateMode="Conditional" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnClose" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:DetailsView ID="dvShowBULK" runat="server" AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="sdsShowBULK">
                                <Fields>
                                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                                    <asp:BoundField DataField="TimeStamp" HeaderText="Oprettet" SortExpression="TimeStamp" />
                                    <asp:TemplateField HeaderText="Skift password hvis bruger findes" SortExpression="ChangePasswordIfExist">
                                        <ItemTemplate>
                                            <asp:Label ID="lblChangePasswordIfExist" runat="server" Text='<%# (bool)Eval("ChangePasswordIfExist") ? "Ja" : "Nej" %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Godkendelse" SortExpression="Approved">
                                        <ItemTemplate>
                                            <asp:MultiView ID="mvApprove" runat="server" ActiveViewIndex='<%# Eval("Approved") == DBNull.Value ? 0 : 1 %>'>
                                                <asp:View ID="vNotChosen" runat="server">
                                                    <asp:TextBox ID="txtMessage" runat="server" MaxLength="150"></asp:TextBox>
                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="wmeMessage" WatermarkCssClass="watermark" TargetControlID="txtMessage" WatermarkText="Afvist fordi... (ikke påkrævet)" runat="server" /><br />
                                                    <asp:LinkButton ID="lnkbtnApprove" CommandArgument='<%# Eval("ID") %>' OnClick="lnkbtnApprove_Click" runat="server">Godkend</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkbtnReject" CommandArgument='<%# Eval("ID") %>' OnClick="lnkbtnReject_Click" runat="server">Afvis</asp:LinkButton>
                                                </asp:View>
                                                <asp:View ID="vChosen" runat="server">
                                                    <asp:Label ID="lblApproved" runat="server" Text='<%# Eval("Approved") == DBNull.Value ? "" : (bool)Eval("Approved") ? "Godkendt" : "Afvist" %>'></asp:Label>
                                                </asp:View>
                                            </asp:MultiView>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="RunTimestamp" HeaderText="Kørt tidspunkt" SortExpression="RunTimestamp" />
                                    <asp:BoundField DataField="Message" HeaderText="Besked" SortExpression="Message" />
                                </Fields>
                            </asp:DetailsView>
                            <asp:SqlDataSource ID="sdsShowBULK" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [BULKs] WHERE ([ID] = @ID)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="gvBULKS" Name="ID" PropertyName="SelectedValue" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <br />
                            <asp:GridView ID="gvShowBULK" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="sdsShowBULKDetails">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" Visible="false" SortExpression="ID" />
                                    <asp:CheckBoxField DataField="Successful" HeaderText="Succesfuld" SortExpression="Successful" />
                                    <asp:BoundField DataField="Message" HeaderText="Besked" SortExpression="Message" />
                                    <asp:BoundField DataField="RowNumber" HeaderText="Linie" SortExpression="RowNumber" />
                                    <asp:BoundField DataField="ADUsername" HeaderText="MANR" SortExpression="ADUsername" />
                                    <asp:BoundField DataField="Firstname" HeaderText="Fornavn" SortExpression="Firstname" />
                                    <asp:BoundField DataField="Lastname" HeaderText="Efternavn" SortExpression="Lastname" />
                                    <asp:BoundField DataField="OUName" HeaderText="Enhed" SortExpression="OUName" />
                                    <asp:BoundField DataField="DistinguishedName" HeaderText="DistinguishedName" SortExpression="DistinguishedName" />
                                    <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                                    <asp:CheckBoxField DataField="MustChangePassword" HeaderText="Skal skifte password" SortExpression="MustChangePassword" />
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="sdsShowBULKDetails" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [BULKsView] WHERE [BULKRefID] = @BULKRefID">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="gvBULKS" Name="BULKRefID" PropertyName="SelectedValue" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:Button ID="btnClose" runat="server" Text="Luk" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>