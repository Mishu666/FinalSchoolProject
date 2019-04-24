<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewPost.aspx.cs" Inherits="ViewPost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>

<asp:Content ID="navbar_content" ContentPlaceHolderID="nav_items_cph" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-column py-3 px-4">

        <Amir:Post runat="server" PostID='<%# Convert.ToInt32(ViewState["PostID"]) %>' />

        <div id="comment_view" runat="server">
        </div>

    </div>
</asp:Content>


