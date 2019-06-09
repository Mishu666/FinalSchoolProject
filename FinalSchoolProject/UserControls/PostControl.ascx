<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PostControl.ascx.cs" Inherits="PostControl" ClassName="User_Controls_PostControl" %>
<%@ Import Namespace="System.IO" %>

<% PostsClass post = PostsClass.GetByID(PostID); %>
<% string page_name = Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); %>

<div class="card shadow-sm w-100 mb-3 post" data-post-id='<%= post.ID %>' data-rating='<%= post.UpvoteCount - post.DownvoteCount %>'
    data-date='<%= post.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") %>' data-isdeleted='<%= post.IsDeleted %>' data-isremoved='<%= post.IsRemoved %>'>

    <div class="card-body">
        <div class="card-head d-flex flex-row align-items-center justify-content-between">

            <%
                if (!post.IsDeleted && !post.IsRemoved)
                {
            %>

            <h4 class="font-weight-bold font-size-48 text-gray-900 post_title"><%= post.Title %>
                <%
                    if (post.IsEdited)
                    {
                %>

                <span class="text-secondary text-xs edited_sm_text_space">(Edited)</span>

                <%
                    }
                %>

            </h4>

            <%
                }
                else if (post.IsDeleted)
                {

            %>

            <h4 class="font-weight-bold font-size-48 text-danger post_title">[deleted]</h4>

            <%
                }
                else if (post.IsRemoved)
                {
            %>

            <h4 class="font-weight-bold font-size-48 text-danger post_title">[removed]</h4>

            <%
                }

            %>

            <%
                if (Session["Logged"] != null && (bool)Session["Logged"] == true && !post.IsDeleted && !post.IsRemoved)
                {

                    UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));
                    bool IsMod = current_user.IsModeratorFor(post.ConsultPageID);
            %>

            <div class="post_menu dropdown no-arrow">
                <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400 post_dropdown"></i>
                </a>
                <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink"
                    x-placement="bottom-end" style="position: absolute; will-change: transform; top: 0px; left: 0px; transform: translate3d(17px, 19px, 0px);">

                    <%
                        if ((IsMod || current_user.IsAdmin) && current_user.ID != post.AuthorID)
                        {
                    %>

                    <button class="RemovePostButton dropdown-item"><i class="fas fa-times mr-2 text-gray-400"></i>Remove</button>

                    <%
                        }
                    %>

                    <%
                        if (current_user.IsAdmin || IsMod)
                        {
                    %>

                    <button class="LockPostButton dropdown-item"><i class="fas fa-lock mr-2 text-gray-400"></i>Lock</button>

                    <%
                        }
                    %>

                    <%
                        if (current_user.ID == post.AuthorID)
                        {
                    %>

                    <%
                        if (page_name == "ViewPost")
                        {
                    %>

                    <button class="EditPostButton dropdown-item"><i class="fas fa-pencil-alt mr-2 text-gray-400"></i>Edit</button>

                    <%
                        }
                    %>

                    <button class="DeletePostButton dropdown-item"><i class="fas fa-trash-alt mr-2 text-gray-400"></i>Delete</button>

                    <%
                        }
                    %>

                    <button class="ReportPostButton dropdown-item"><i class="fas fa-flag mr-2 text-gray-400"></i>Report</button>
                </div>
            </div>

            <%
                }
            %>
        </div>
        <%
            if (!post.IsDeleted && !post.IsRemoved)
            {
        %>

        <div class="default_post_view card-text post_text">
            <%= post.Body %>
        </div>

        <%
            }
            else if (post.IsDeleted)
            {
        %>

        <div class="default_post_view card-text text-danger post_text">
            [deleted]
        </div>

        <%
            }
            else if (post.IsRemoved)
            {
        %>

        <div class="default_post_view card-text text-danger post_text">
            [removed]
        </div>

        <%
            }
        %>

        <div class="edit_post_view" style="display: none;">
            <div class="EditableAreaSpace mb-3">
                <div class="EditableArea"><%= post.Body %></div>
            </div>
            <button type="button" class="CancelPostEditButton btn btn-secondary">Cancel</button>
            <button type="button" class="ConfirmPostEditButton btn btn-primary">Confirm</button>
            <span class="EditPostWarningSpace text-danger ml-3" style="display: none;"></span>
        </div>
    </div>
    <div class="card-footer d-flex flex-row justify-content-between align-items-center text-gray-700">
        <div class="comments-block d-flex flex-row justify-content-center align-items-center">
            <i class="fas fa-comment mr-1"></i>
            <span class="comment_count d-flex flex-row justify-content-center align-items-center mx-3"
                style="max-width: 4rem;"><%= post.CommentCount %></span>
        </div>
        <div class="mr-auto d-flex flex-row">
            <span>potsed by&nbsp;</span>
            <%  if (post.IsDeleted)
                {
            %>

            <span class="text-danger post_author_name">[deleted]</span>

            <%
                }
                else if (post.IsRemoved)
                {
            %>

            <span class="text-danger post_author_name">[removed]</span>

            <%
                }
                else
                {
            %>

            <a href="ViewUser.aspx?user-id=<%= post.AuthorID %>" class="post_author_name"><%= UsersClass.GetByID(post.AuthorID).Username %></a>

            <%
                }
            %>
            <span>&nbsp;on&nbsp;</span>
            <div class="post_creation_date"><%= post.CreationDate.ToString("dd/MM/yyyy") %></div>
            <span>&nbsp;-&nbsp;</span><a href="ConsultPage.aspx?page-id=<%= post.ConsultPageID %>" class="post_consult_page">
                <%= ConsultPagesClass.GetByID(post.ConsultPageID).PageName %>
            </a>
        </div>
        <div class="vote-block d-flex flex-row">
            <a runat="server" role="button" id="save_button" class="mr-4 p-0 text-gray-500 action_space save_space dismissable_popover"
                data-placement="top" data-content="you must log in to save posts">
                <i tabindex="-1" class="save_post action fas fa-star"></i>
            </a>
            <a runat="server" role="button" id="upvote_button" class="mr-1 p-0 text-gray-500 action_space upvote_space dismissable_popover"
                data-placement="top" data-content="you must log in to vote">
                <i tabindex="-1" class="upvote action fas fa-arrow-up"></i>
            </a>
            <span class="upvote_counter d-flex flex-row justify-content-center align-items-center mx-3"
                style="max-width: 4rem;"><%= post.UpvoteCount %></span>

            <a runat="server" role="button" id="downvote_button" class="mr-1 p-0 text-gray-500 action_space downvote_space dismissable_popover"
                data-placement="top" data-content="you must log in to vote">
                <i tabindex="-1" class="downvote action fas fa-arrow-down"></i>
            </a>
            <span class="downvote_counter d-flex flex-row justify-content-center align-items-center mx-3"
                style="max-width: 4rem;"><%= post.DownvoteCount %></span>
        </div>
    </div>
</div>
