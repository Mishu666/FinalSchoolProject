<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserEditor.aspx.cs" Inherits="UserEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">

<%--        <asp:GridView ID="UsersGV" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="User Name">
                    <ItemTemplate>

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>--%>

    </div>

</asp:Content>

