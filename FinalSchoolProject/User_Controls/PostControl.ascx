﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PostControl.ascx.cs" Inherits="User_Controls_PostControl" ClassName="User_Controls_PostControl" %>

<div class="card shadow-sm w-100 mb-3 post" id="post_card" runat="server">
    <div class="card-body">
        <h5 class="card-title text-gray-900" id="post_title" runat="server"></h5>
        <p class="card-text" id="post_text" runat="server">
        </p>
    </div>
    <div class="card-footer d-flex flex-row justify-content-between align-items-center text-gray-700" id="post_footer">
        <div class="comments-block d-flex flex-row justify-content-center align-items-center">
            <i class="fas fa-comment mr-1"></i>
            <span class="comment_count d-flex flex-row justify-content-center align-items-center mx-3" style="max-width: 4rem;" id="post_comment_count" runat="server"></span>
        </div>
        <div class="mr-auto d-flex flex-row">
            <span>potsed by&nbsp;</span><a href="#" id="post_author_name" runat="server"></a>
            <span>&nbsp;on&nbsp; </span>
            <div id="post_creation_date" runat="server"></div>
            <span>&nbsp;-&nbsp; </span><a href='#' id="post_consult_page" runat="server"></a>
        </div>
        <div class="vote-block d-flex flex-row">
            <a role="button" id="upvote_button" runat="server" class="mr-1 p-0 text-gray-500 action_space upvote_space dismissable_popover"
                data-placement="top" data-content="you must log in to vote">
                <i tabindex="-1" class="upvote action fas fa-arrow-up"></i>
            </a>
            <span id="upvote_counter" runat="server" class="upvote_counter d-flex flex-row justify-content-center align-items-center mx-3" style="max-width: 4rem;"></span>
            <a role="button" id="downvote_button" runat="server" class="mr-1 p-0 text-gray-500 action_space downvote_space dismissable_popover"
                data-placement="top" data-content="you must log in to vote">
                <i tabindex="-1" class="downvote action fas fa-arrow-down"></i>
            </a>
            <span id="downvote_counter" runat="server" class="downvote_counter d-flex flex-row justify-content-center align-items-center mx-3" style="max-width: 4rem;"></span>
        </div>
    </div>
</div>
