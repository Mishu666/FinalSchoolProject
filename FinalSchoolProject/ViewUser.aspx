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

        <% UsersClass user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"])); %>

        <div id="user_space" class="card shadow-sm ml-3 d-flex flex-column h-100" style="width: 30% !important;">
            <img src="<%= user.ProfilePictureDir %>" alt="profile picture" onerror="this.remove();" class="card-img-top" />
            <div class="card-body" id="default_user_view">
                <h5 class="card-title mr-1"><%= user.Username %>

                    <%
                        if (user.IsAdmin)
                        {
                    %>

                    <span id="admin_badge" class="badge badge-dark">admin</span>

                    <%
                        }
                    %>

                </h5>

                <%
                    if (!string.IsNullOrWhiteSpace(user.Bio))
                    {
                %>

                <p class="card-text"><%= user.Bio %></p>

                <%
                    }
                %>

                <button id="EditUserButton" runat="server" visible="false" type="button" class="EditUserButton btn btn-primary">Edit<i class="far fa-edit ml-2"></i></button>
                <button id="FollowUserButton" runat="server" visible="false" type="button" class="FollowUserButton btn btn-primary">Follow<i class="fas fa-plus ml-2"></i></button>
                <button id="UnfollowUserButton" runat="server" visible="false" type="button" class="UnfollowUserButton btn btn-secondary">Following<i class="fas fa-check ml-2"></i></button>

            </div>
            <div class="card-body" id="edit_user_view" style="display: none;">

                <div class="input-group mb-3">
                    <input id="EditUsernameInput" type="text" class="form-control" placeholder="Username" aria-label="Username" aria-describedby="basic-addon-username"
                        value="<%= user.Username %>" data-default-value="<%= user.Username %>">
                </div>

                <div class="input-group mb-3">
                    <textarea id="EditBioInput" class="form-control" placeholder="Username" aria-label="Username" rows="5"
                       data-default-value="<%= user.Bio %>" aria-describedby="basic-addon-username"><%= user.Bio %></textarea>
                </div>

                <div class="input-group mb-3">
                    <input id="EditConfirmPasswordInput" type="text" class="form-control" placeholder="Confirm password"
                        aria-label="Confirm Password" aria-describedby="basic-addon-confirm-password">
                </div>

                <div class="custom-control custom-switch mb-3">
                    <input type="checkbox" class="custom-control-input" id="EditIsPrivateSwitch"
                        data-default-value="<%= user.IsPrivate %>" checked="<%= user.IsPrivate %>">
                    <label class="custom-control-label" for="EditIsPrivateSwitch">Private</label>
                </div>

                <button id="CancelEditButton" type="button" class="btn btn-secondary">Cancel</button>
                <button id="ConfirmEditButton" type="button" class="btn btn-primary">Confirm</button>
                <div class="w-100 mt-3" id="edit_user_warning_space"></div>

            </div>
            <div class="card-footer d-flex flex-row justify-content-around align-items-center">
                <span><%= user.MyFollowersCount %> Followers</span>
                <span><%= user.FollowingCount %> Following</span>
            </div>
        </div>
    </div>

</asp:Content>
