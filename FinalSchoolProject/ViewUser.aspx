<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewUser.aspx.cs" Inherits="ViewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/ViewUser.js"></script>
    <link rel="stylesheet" href="Style/ViewUser.css" />

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">
        <div id="main_space" class="card shadow-sm d-flex flex-column w-75 h-100 p-3">

            <ul class="nav nav-tabs">
                <li class="nav-item">
                    <a runat="server" class="nav-link active" id="ProfileMyPostsTab" data-post-type="submitted" onserverclick="ProfilePostsTab_ServerClick">Submitted</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileSavedPostsTab" data-post-type="saved" onserverclick="ProfilePostsTab_ServerClick">Saved</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileUpvotedPostsTab" data-post-type="upvoted" onserverclick="ProfilePostsTab_ServerClick">Upvoted</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileDownvotedPostsTab" data-post-type="downvoted" onserverclick="ProfilePostsTab_ServerClick">Downvoted</a>
                </li>
            </ul>

            <div id="posts_space" class="d-flex flex-column pr-3 mt-3" style="overflow-y: scroll !important; overflow-x: hidden !important;">

                <asp:Repeater ID="ProfilePostsRepeater" runat="server">
                    <ItemTemplate>
                        <uc:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />
                    </ItemTemplate>
                </asp:Repeater>

            </div>

        </div>
        <div id="user_space" class="card shadow-sm ml-5 d-flex flex-column h-100 w-25">
            <img src="#" alt="profile picture" />
        </div>

    </div>
</asp:Content>
