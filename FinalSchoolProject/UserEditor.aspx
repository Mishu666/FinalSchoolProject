<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserEditor.aspx.cs" Inherits="UserEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="Script/UserEditor.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">

        <asp:GridView ID="UsersGV" BorderWidth="0" runat="server" AutoGenerateColumns="false" CssClass="UsersGV table w-100 border"
            GridLines="None" HeaderStyle-CssClass="thead-dark" EmptyDataText="No Users Found" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" 
            ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:TemplateField HeaderText="Username">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="UsernameLabel" Text='<%# Eval("Username") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IsAdmin">
                    <ItemTemplate>
                        <label class="switch mb-0">
                            <input type="checkbox" id="IsAdminSwitch" class="IsAdminSwitch" runat="server" checked='<%# Bind("IsAdmin") %>' data-user-id='<%# Eval("ID") %>'>
                            <span class="slider round"></span>
                        </label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flags">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="FlagsLabel" Text='<%# Bind("Flags") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Points">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="PointsLabel" Text='<%# Bind("Points") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IsSuspended">
                    <ItemTemplate>
                        <label class="switch mb-0">
                            <input type="checkbox" id="IsSuspendedSwitch" class="IsSuspendedSwitch" runat="server" checked='<%# Bind("IsSuspended") %>' data-user-id='<%# Eval("ID") %>'>
                            <span class="slider round"></span>
                        </label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <button type="button" class="DeleteUserButton btn btn-link" data-user-id='<%# Eval("ID") %>'>Delete</button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-Width="100">
                    <ItemTemplate>
                        <div class="loading_spinner spinner-border text-primary" role="status" style="display: none;">
                            <span class="sr-only">Loading...</span>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>

