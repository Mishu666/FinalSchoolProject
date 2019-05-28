<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewPost.aspx.cs" Inherits="ViewPost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>

<asp:Content ID="navbar_content" ContentPlaceHolderID="nav_items_cph" runat="server">

    <!-- Nav Item - Add Comment -->

    <li class="nav-item dropdown no-arrow mx-1" visible="true">
        <a class="nav-link dropdown-toggle dismissable_popover" href="#" id="addOriginalCommentButton" role="button" data-consult-page-id="<%= Convert.ToInt32(ViewState["PageID"]) %>"
            data-placement="top" data-content="you must log in to comment">
            <i class="fas fa-comment-medical"></i>
        </a>
    </li>


    <!-- Nav Item - Sort Direction-->

    <li class="nav-item dropdown no-arrow mx-1" visible="true">
        <a class="nav-link dropdown-toggle" href="#" id="sortDirectionDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw"></i>
        </a>
    </li>

    <!-- Nav Item - Sort -->

    <li class="nav-item dropdown no-arrow mx-1" visible="true">
        <a class="nav-link dropdown-toggle" id="sortDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-sort-amount-down fa-fw"></i>
        </a>
        <!-- Dropdown - Sort Options -->
        <div class="dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in w-auto" aria-labelledby="sortOptionsDropdown" id="sortOptions">
            <a class="dropdown-item d-flex align-items-center" id="sortByNew">New
            </a>
            <a class="dropdown-item d-flex align-items-center" id="sortByRating">Top
            </a>
        </div>
    </li>

    <li class="nav-item dropdown no-arrow mx-1" runat="server" visible="true">
        <div class="nav-link" id="sort_option_text">
            New
        </div>
    </li>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-column py-3 px-4 h-100">

        <div id="post_space" runat="server">
            <uc:Post ID="ViewPostPostControl" runat="server" />
        </div>

        <div class="card shadow-sm w-100 mb-3 hidden_comment" id="hidden_comment_card" style="display: none;">
            <div class="card-body">

                <div class="input-group mb-3">
                    <textarea id="addCommentBody" class="form-control" placeholder="Comment body"></textarea>
                </div>

                <div id="add_comment_warning_space" class="w-100"></div>
                <div id="add_comment_footer" class="d-flex flex-row justify-content-end">
                    <button class="btn btn-secondary mr-2" id="cancel_comment_button">Cancel</button>
                    <button class="btn btn-primary" id="confirm_comment_button" data-page-id='<%= ViewState["PostID"].ToString() %>'>Submit</button>
                </div>
            </div>
        </div>

        <div id="comment_space" runat="server" class="pr-3 original_comments" style="overflow-y: scroll !important; overflow-x: hidden !important;">
        </div>

    </div>
</asp:Content>


