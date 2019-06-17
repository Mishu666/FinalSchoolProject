<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ConsultPageEditor.aspx.cs" Inherits="ConsultPageEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Script/ConsultPageEditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">
    <div id="content_space" class="overflow-hidden d-flex flex-column h-100 w-100 py-3 px-4">
        <div id="GVSpace" class="w-100">
            <asp:GridView ID="ConsultPagesGV" BorderWidth="0" runat="server" AutoGenerateColumns="false"
                CssClass="ConsultPagesGV table w-100 border" GridLines="None" HeaderStyle-CssClass="thead-dark"
                HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"
                Style="table-layout: fixed !important;">
                <Columns>
                    <asp:TemplateField HeaderText="PageName">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="PageNameLabel" Text='<%# Eval("PageName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subscriber Count">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="SubscriberCountLabel" Text='<%# Bind("SubscriberCount") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <button type="button" class="DeletePageButton btn btn-link" data-page-id='<%# Eval("ID") %>'>
                                Delete</button>
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
        <div class="w-100 d-flex flex-row align-items-center mt-3 justify-content-start">
            <div class="input-group w-25">
                <input id="NewConsultPageNameInput" class="form-control" placeholder="New Page Name" />
            </div>
            <button class="btn btn-primary mx-3" id="NewConsultPageInsertButton">
                Add</button>
        </div>
        <div id="AddPageWarningSpace" class="mt-3">
        </div>
    </div>
</asp:Content>
