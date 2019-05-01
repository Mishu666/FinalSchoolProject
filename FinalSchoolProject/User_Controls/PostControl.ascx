<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PostControl.ascx.cs" Inherits="User_Controls_PostControl" ClassName="User_Controls_PostControl" %>

<% PostsClass post = PostsClass.GetByID(PostID); %>

<div class="card shadow-sm w-100 mb-3 post" data-post-id='<%= post.ID %>' data-rating='<%= post.UpvoteCount - post.DownvoteCount %>'
    data-date='<%= post.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") %>'>
    <div class="card-body">
        <h5 class="card-title text-gray-900 post_title""><%= post.Title %></h5>
        <p class="card-text post_text"><%= post.Body %></p>
    </div>
    <div class="card-footer d-flex flex-row justify-content-between align-items-center text-gray-700">
        <div class="comments-block d-flex flex-row justify-content-center align-items-center">
            <i class="fas fa-comment mr-1"></i>
            <span class="comment_count d-flex flex-row justify-content-center align-items-center mx-3" 
                style="max-width: 4rem;"><%= post.CommentCount %></span>
        </div>
        <div class="mr-auto d-flex flex-row">
            <span>potsed by&nbsp;</span><a href="ViewUser.aspx?user-id=<%= post.AuthorID %>" class="post_author_name"><%= UsersClass.GetByID(post.AuthorID).Username %></a>
            <span>&nbsp;on&nbsp;</span>
            <div class="post_creation_date"><%= post.CreationDate.ToString("dd/MM/yyyy") %></div>
            <span>&nbsp;-&nbsp;</span><a href="ConsultPage.aspx?page-id=<%= post.ConsultPageID %>" class="post_consult_page">
                <%= ConsultPagesClass.GetByID(post.ConsultPageID).PageName %>
            </a>
        </div>
        <div class="vote-block d-flex flex-row">
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
