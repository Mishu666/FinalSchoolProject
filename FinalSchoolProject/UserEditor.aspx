<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserEditor.aspx.cs" Inherits="UserEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="Script/UserEditor.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">

        <asp:GridView ID="UsersGV" BorderWidth="0" runat="server" AutoGenerateColumns="false" CssClass="table w-100" style="table-layout: fixed !important;" 
            GridLines="None" HeaderStyle-CssClass="thead-dark" AlternatingRowStyle-CssClass="bg-gray-400" AllowPaging="true" PageSize="15" PagerSettings-FirstPageText="First"
            PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-PageButtonCount="5"
            OnPageIndexChanging="UsersGV_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Username">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="UsernameLabel" Text='<%# Bind("UserName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="IsAdmin">
                    <ItemTemplate>
                        <div class="custom-control custom-switch">
                            <input type="checkbox" checked='<%# Bind("IsAdmin") %>' class='custom-control-input IsAdminSwitch'
                                id="IsAdminSwitch" runat="server" disabled />
                            <label class="custom-control-label" for="#">Admin</label>
                        </div>
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
                        <div class="custom-control custom-switch">
                            <input type="checkbox" checked='<%# Bind("IsSuspended") %>' class='custom-control-input IsSuspendedSwitch'
                                id="IsSuspendedSwitch" runat="server" disabled />
                            <label class="custom-control-label" for="#">Suspended</label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ban"></asp:TemplateField>
                <asp:TemplateField HeaderText="Delete"></asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>

</asp:Content>

