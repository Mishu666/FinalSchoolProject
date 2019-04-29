<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ViewUser.aspx.vb" Inherits="ViewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/ViewUser.js"></script>
    <link rel="stylesheet" href="Style/ViewUser.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">
        <div id="main_space" class="card shadow-sm d-flex flex-column w-75 h-100 p-3">

            <ul class="nav nav-tabs">
                <li class="nav-item">
                    <asp:Button runat="server" class="nav-link active" ID="ProfileMyPostsTab" Text="My Posts" />
                </li>
                <li class="nav-item">
                    <asp:Button runat="server" class="nav-link" ID="ProfileSavedPostsTab" Text="Saved" />
                </li>
                <li class="nav-item">
                    <asp:Button runat="server" class="nav-link" ID="ProfileUpvotedPostsTab" Text="Saved" />
                </li>
                <li class="nav-item">
                    <asp:Button runat="server" class="nav-link" ID="ProfileDownvotedPostsTab" Text="Saved" />
                </li>
            </ul>

            <div id="posts_space" class="d-flex flex-column overflow-auto pr-3 bg-danger">
                
            </div>

        </div>
        <div id="user_space" class="card shadow-sm ml-5 d-flex flex-column h-100 w-25">
            <img src="#" alt="profile picture" />
        </div>

    </div>
</asp:Content>

