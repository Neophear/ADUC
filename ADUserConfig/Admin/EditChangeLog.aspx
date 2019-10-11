<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditChangeLog.aspx.cs" Inherits="Admin_EditChangeLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SiteContent" Runat="Server">
    <asp:DetailsView ID="dvChangeLog" runat="server" AutoGenerateRows="False" DefaultMode="Insert" OnItemUpdated="dvChangeLog_ItemUpdated" OnItemInserted="dvChangeLog_ItemInserted" DataKeyNames="ID" DataSourceID="sdsWriteChangeLog">
        <Fields>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="Text" HeaderText="Text" SortExpression="Text" />
            <asp:CheckBoxField DataField="AdminOnly" HeaderText="AdminOnly" SortExpression="AdminOnly" />
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Opdater"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Annuller"></asp:LinkButton>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert" Text="Indsæt"></asp:LinkButton>
                </InsertItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="sdsWriteChangeLog" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" InsertCommand="INSERT INTO [ChangeLog] ([Text], [AdminOnly]) VALUES (@Text, @AdminOnly)" SelectCommand="SELECT [ID], [Text], [AdminOnly] FROM [ChangeLog] WHERE ([ID] = @ID)" UpdateCommand="UPDATE [ChangeLog] SET [Text] = @Text, [AdminOnly] = @AdminOnly WHERE [ID] = @ID">
        <InsertParameters>
            <asp:Parameter Name="Text" Type="String" />
            <asp:Parameter Name="AdminOnly" Type="Boolean" />
        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="gvChangeLog" Name="ID" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="Text" Type="String" />
            <asp:Parameter Name="ID" Type="Int32" />
            <asp:Parameter Name="AdminOnly" Type="Boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <br />
    <asp:GridView ID="gvChangeLog" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvChangeLog_SelectedIndexChanged" DataKeyNames="ID" DataSourceID="sdsChangeLog">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" SortExpression="Timestamp" />
            <asp:BoundField DataField="Text" HeaderText="Text" SortExpression="Text" />
            <asp:BoundField DataField="AdminOnly" HeaderText="AdminOnly" SortExpression="AdminOnly" />
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sdsChangeLog" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [ChangeLog] ORDER BY [Timestamp] DESC">
    </asp:SqlDataSource>
</asp:Content>