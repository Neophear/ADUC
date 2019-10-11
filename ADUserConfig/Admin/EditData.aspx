<%@ Page Title="Rediger ADUC data" ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditData.aspx.cs" Inherits="Admin_EditData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:Label ID="lblInfo" runat="server" CssClass="error" Text="Ikke rediger disse indstillinger medmindre du ved hvad du laver!"></asp:Label><br /><br />
    <ajaxToolkit:TabContainer ID="tcData" runat="server" Width="900">
        <ajaxToolkit:TabPanel runat="server" HeaderText="OU&#39;er" ID="tpOUs">
            <ContentTemplate>
                <asp:GridView ID="gvOUs" runat="server" BackColor="#CCCCCC" AutoGenerateColumns="False" AllowSorting="true" Width="100%" DataKeyNames="ID" DataSourceID="sdsOUs">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" Visible="false" ReadOnly="True" SortExpression="ID" />
                        <asp:TemplateField HeaderText="Vist navn" HeaderStyle-Width="170px" SortExpression="DisplayName">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDisplayName" Width="150px" MaxLength="50" runat="server" Text='<%# Bind("DisplayName") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDisplayName" ValidationGroup="Update" runat="server" Text="*" ControlToValidate="txtDisplayName" ForeColor="Red" ErrorMessage="Dette felt er påkrævet" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("DisplayName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DistinguishedName" SortExpression="DistinguishedName">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDistinguishedName" Width="95%" MaxLength="1000" runat="server" Text='<%# Bind("DistinguishedName") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDistinguishedName" ValidationGroup="Update" runat="server" Text="*" ControlToValidate="txtDistinguishedName" ForeColor="Red" ErrorMessage="Dette felt er påkrævet" Display="Dynamic"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDistinguishedName" runat="server" Text='<%# Bind("DistinguishedName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Regex" SortExpression="Regex">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRegex" Width="95%" MaxLength="100" runat="server" Text='<%# Bind("Regex") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRegex" runat="server" Text='<%# Bind("Regex") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Aktiv" SortExpression="Enabled">
                            <EditItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("Enabled") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# (bool)Eval("Enabled") ? "Ja" : "Nej" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" ValidationGroup="Update" CausesValidation="True" CommandName="Update" Text="Gem"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annuller"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Rediger"></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnDelete" runat="server" CausesValidation="False" CommandName="Delete" OnClientClick="return confirm('Er du sikker på at du vil slette denne OU?');" Text="Slet"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:DetailsView ID="dvOUs" runat="server" Width="700px" BackColor="#CCCCCC" FieldHeaderStyle-Width="150px" AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="sdsOUs" DefaultMode="Insert">
                    <Fields>
                        <asp:TemplateField HeaderText="Vist navn" SortExpression="DisplayName">
                            <InsertItemTemplate>
                                <asp:TextBox ID="txtDisplayName" runat="server" MaxLength="50" Text='<%# Bind("DisplayName") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDisplayName" runat="server" ValidationGroup="Insert" Text="*" ControlToValidate="txtDisplayName" ForeColor="Red" ErrorMessage="Dette felt er påkrævet" Display="Dynamic"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DistinguishedName" SortExpression="DistinguishedName">
                            <InsertItemTemplate>
                                <asp:TextBox ID="txtDistinguishedName" Width="95%" MaxLength="1000" runat="server" Text='<%# Bind("DistinguishedName") %>'></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDistinguishedName" ValidationGroup="Insert" runat="server" Text="*" ControlToValidate="txtDistinguishedName" ForeColor="Red" ErrorMessage="Dette felt er påkrævet" Display="Dynamic"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Regex" SortExpression="Regex">
                            <InsertItemTemplate>
                                <asp:TextBox ID="txtRegex" Width="95%" MaxLength="100" runat="server" Text='<%# Bind("Regex") %>'></asp:TextBox>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="Enabled" HeaderText="Aktiv" SortExpression="Enabled" />
                        <asp:TemplateField ShowHeader="false">
                            <InsertItemTemplate>
                                <asp:LinkButton ID="lnkbtnInsert" runat="server" CausesValidation="True" CommandName="Insert" ValidationGroup="Insert" Text="Indsæt"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annuller"></asp:LinkButton>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>
                <asp:SqlDataSource ID="sdsOUs" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" OnInserted="sdsOUs_Inserted" SelectCommand="SELECT * FROM [OUs] ORDER BY [DisplayName]" DeleteCommandType="StoredProcedure" DeleteCommand="OUDelete" InsertCommandType="StoredProcedure" InsertCommand="OUCreate" UpdateCommandType="StoredProcedure" UpdateCommand="OUUpdate">
                    <DeleteParameters>
                        <asp:Parameter Name="ID" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="DistinguishedName" Type="String" />
                        <asp:Parameter Name="DisplayName" Type="String" />
                        <asp:Parameter Name="Regex" Type="String" />
                        <asp:Parameter Name="Enabled" Type="Boolean" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="ID" Type="Int32" />
                        <asp:Parameter Name="DistinguishedName" Type="String" />
                        <asp:Parameter Name="DisplayName" Type="String" />
                        <asp:Parameter Name="Regex" Type="String" />
                        <asp:Parameter Name="Enabled" Type="Boolean" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel runat="server" HeaderText="Info" ID="tpInfo">
            <ContentTemplate>
                <asp:GridView ID="gvInfo" runat="server" BackColor="#CCCCCC" AutoGenerateColumns="False" OnRowUpdating="gvInfo_RowUpdating" Width="100%" DataKeyNames="ID" DataSourceID="sdsInfo">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" Visible="false" ReadOnly="True" SortExpression="ID" />
                        <asp:BoundField DataField="Description" ReadOnly="true" HeaderText="Beskrivelse" SortExpression="Description" />
                        <asp:TemplateField HeaderText="Værdi" SortExpression="Value">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtValue" Width="99%" TextMode="MultiLine" Height="100" runat="server" Text='<%# Eval("Value").ToString().Replace("<br />", "\n") %>' MaxLength="1000"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" ValidationGroup="Update" CausesValidation="True" CommandName="Update" Text="Gem"></asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annuller"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="False" CommandName="Edit" Text="Rediger"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sdsInfo" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [ID], [Description], [Value] FROM [Info]" UpdateCommand="UPDATE [Info] SET [Value] = @Value WHERE [ID] = @ID">
                    <UpdateParameters>
                        <asp:Parameter Name="Value" Type="String" />
                        <asp:Parameter Name="ID" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <br />
                <hr />
                <br />
                <asp:Label ID="lblBULKMsg" runat="server" Visible="false" Font-Bold="true"></asp:Label><br />
                <asp:Label ID="lblBULKFile" runat="server" Text="Upload nyt BULK-ark. Overskriver det gamle."></asp:Label><br />
                <asp:FileUpload ID="fuBULKFile" runat="server" />
                <asp:RequiredFieldValidator ID="rfvBULKFile" runat="server" ErrorMessage="Du skal vælge en fil." ValidationGroup="BULK" ControlToValidate="fuBULKFile" Text="*" CssClass="error" ToolTip="Du skal vælge en fil."></asp:RequiredFieldValidator>
                <asp:Button ID="btnBULKUpload" runat="server" Text="Upload" ValidationGroup="BULK" OnClick="btnBULKUpload_Click" />
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</asp:Content>

