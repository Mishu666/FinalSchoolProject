<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ModsEditor.aspx.cs" Inherits="ModsEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/ModsEditor.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div class="h-100 w-100 d-flex flex-column justify-content-center align-items-center py-3 px-4">

        <div id="ConsultPageSelectionBar" class="w-100 mb-3 d-flex flex-row justify-content-start align-items-center">

            <asp:DropDownList ID="ConsultPagesDDL" runat="server" CssClass="ConsultPagesDDL form-control w-25 mr-5">
            </asp:DropDownList>

            <div id="loading_spinner" class="spinner-border text-primary" role="status" style="display: none;">
                <span class="sr-only">Loading...</span>
            </div>
            <asp:Label ID="WarningLabel" runat="server" CssClass="text-danger" Visible="false"></asp:Label>

        </div>
        <div id="GVSection" class="w-100 flex-grow-1 d-flex flex-row justify-content-center align-items-center">

            <div id="ModsSection" class="h-100 mr-3" style="width: 45% !important;">

                <asp:GridView ID="PageModsGV" BorderWidth="0" runat="server" AutoGenerateColumns="false" CssClass="PageModsGV table border w-100" Style="table-layout: fixed !important;"
                    GridLines="None" HeaderStyle-CssClass="thead-dark" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true">

                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="70">
                            <HeaderTemplate>
                                <label class="switch mb-0">
                                    <input type="checkbox" class="SelectAllInGroupSwitch">
                                    <span class="slider round"></span>
                                </label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <label class="switch mb-0">
                                    <input type="checkbox" class="SelectInGroupSwitch" data-user-id='<%# Eval("ID") %>'>
                                    <span class="slider round"></span>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Username">
                            <ItemTemplate>
                                <asp:Label ID="UsernameLabel" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Demote">
                            <ItemTemplate>
                                <button type="button" class="btn btn-link DemoteUserButton ToggleModButton" data-user-id='<%# Eval("ID") %>'>
                                    <i class="fas fa-times mr-2"></i>Kick
                                </button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>

            </div>
            <div id="MoveUsersButtonSpace" class="flex-grow-1 h-100 d-flex flex-column justify-content-start align-items-center border-left border-right">

                <button type="button" class="btn btn-primary w-75 my-5" id="DemoteMultipleButton">
                    <i class="fas fa-arrow-right"></i>
                </button>

                <button type="button" class="btn btn-primary w-75" id="PromoteMultipleButton">
                    <i class="fas fa-arrow-left"></i>
                </button>

            </div>

            <div id="NotModsSection" class="h-100 ml-3" style="width: 45% !important;">

                <asp:GridView ID="NotModsGV" BorderWidth="0" runat="server" AutoGenerateColumns="false" CssClass="NotModsGV table w-100 border" Style="table-layout: fixed !important;"
                    GridLines="None" HeaderStyle-CssClass="thead-dark" HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true">

                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="70">
                            <HeaderTemplate>
                                <label class="switch mb-0">
                                    <input type="checkbox" class="SelectAllNotInGroupSwitch">
                                    <span class="slider round"></span>
                                </label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <label class="switch mb-0">
                                    <input type="checkbox" class="SelectNotInGroupSwitch" data-user-id='<%# Eval("ID") %>'>
                                    <span class="slider round"></span>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Username">
                            <ItemTemplate>
                                <asp:Label ID="UsernameLabel" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Promote">
                            <ItemTemplate>
                                <button type="button" class="btn btn-link PromoteUserButton ToggleModButton" data-user-id='<%# Eval("ID") %>'>
                                    <i class="fas fa-plus mr-2"></i>Include
                                </button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>

    </div>

</asp:Content>

