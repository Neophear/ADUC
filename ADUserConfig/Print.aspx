<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="Print" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ADUC Print</title>
    <link type="text/css" rel="stylesheet" href="/Content/PrintCSS.css"/>
    <script type="text/javascript">
        window.print();
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="pnlSingle" CssClass="box-left-mini" runat="server">
            <div class="a5 front">
		        <div style="margin-top:30%;">
			        Brugernavn:<br />
                    <asp:Label ID="lblSingleUsername" runat="server" Text="Username"></asp:Label><br />
			        <br />
			        Password:<br />
                    <asp:Label ID="lblSinglePassword" CssClass="password" runat="server" Text="Password"></asp:Label><br />
                    <br />
                    WiFi:<br />
                    <asp:Label ID="lblWiFiName" runat="server" Text="WiFiName"></asp:Label>
			        <div class="a5_footer">
				        <asp:Label ID="lblSingleFooter" runat="server" Text="Footer"></asp:Label>
			        </div>
		        </div>
	        </div>
            <div class="behind_container">
                <div class="behind">
                    <asp:Image ID="imgBackground" Height="450" Width="450" ImageUrl="/Content/images/PIT.PNG" runat="server" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlBulk" runat="server">
            <asp:Repeater ID="rptA5" runat="server" Visible="false" DataSourceID="sdsBULK">
                <ItemTemplate>
                    <div class="pagebreak box-left-mini">
                        <div class="a5">
		                    <div class="front">
			                    <div style="margin-top:30%;">
				                    Brugernavn:<br />
				                    <%# Eval("ADUsername") %><br />
				                    <br />
				                    Password:<br />
				                    <asp:Label runat="server" CssClass="password"><%# Eval("Password") %></asp:Label><br />
                                    <br />
                                    WiFi:<br />
                                    <asp:Label ID="lblWiFiName" runat="server" Text='<%# wifiname %>'></asp:Label>
				                    <div class="a5_footer">
					                    <asp:Label ID="lblFooter" runat="server" Text='<%# footer %>'></asp:Label>
				                    </div>
			                    </div>
		                    </div>
                            <div class="behind_container">
                                <div class="behind">
                                    <asp:Image ID="imgBackground" Height="450" Width="450" ImageUrl="/Content/images/PIT.PNG" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rptA4" runat="server" Visible="false" DataSourceID="sdsBULK">
                <ItemTemplate>
                    <div class="a4main">
                        <div class="left">
                            <%# Eval("ADUsername") %>
                        </div>
                        <div class="right">
				            <asp:Label runat="server" CssClass="password"><%# Eval("Password") %></asp:Label><br />
                        </div><br />
                        <div class="footer left">
                            <asp:Label ID="lblFooter" runat="server" Text='<%# footer %>'></asp:Label>
                        </div>
                        <div class="footer right">
                            WiFi: <asp:Label ID="lblWiFiName" runat="server" Text='<%# wifiname %>'></asp:Label>
                        </div>
                    </div><br />
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="sdsBULK" runat="server" OnSelecting="sdsBULK_Selecting" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommandType="StoredProcedure" SelectCommand="GetBULKPrint">
                <SelectParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                    <asp:Parameter Name="CreatedBy" Type="String" />
                    <asp:Parameter Name="IsAdmin" Type="Boolean" />
                </SelectParameters>
            </asp:SqlDataSource>
        </asp:Panel>
    </form>
</body>
</html>