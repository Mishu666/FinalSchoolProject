<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PostView.aspx.cs" Inherits="PostView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="Server">

    <div class="page_content overflow-auto px-3 w-100">

        <asp:Repeater ID="PostViewRepeater" runat="server" OnItemDataBound="PostViewRepeater_ItemDataBound">
            <ItemTemplate>
                <div id="post_view" class="card shadow-sm w-100">

                    <div class="card-body">
                        <h5 class="card-title text-gray-900"><%# Eval("Title") %></h5>
                        <p class="card-text ">
                            <%# Eval("Body") %>
                        </p>
                        <div class="d-flex flex-row justify-content-between align-items-end text-gray-700">
                            <div class="comments-block d-flex flex-row justify-content-center align-items-center">
                                <i class="fas fa-comment mr-1"></i>
                                <span class="comment_count d-flex flex-row justify-content-center align-items-center mx-auto" style="width: 4rem;">
                                    <%# Eval("CommentCount") %>
                                </span>
                            </div>
                            <div class="mr-auto">
                                potsed by <a href="#"><%# PostsClass.GetAuthorUsername(Convert.ToInt32(Eval("ID"))) %></a> on  <%# PostsClass.GetCreationDate(Convert.ToInt32(Eval("ID"))) %>
                            </div>
                            <div class="vote-block d-flex flex-row">
                                <a role="button" id="upvote_button" runat="server" class="btn mr-1 p-0  text-gray-500 upvote" data-post-id='<%# Convert.ToInt32(Eval("ID")) %>'
                                    data-toggle="popover" data-placement="top" data-content="you must log in to vote" data-trigger="focus">
                                    <i tabindex="-1" class="fas fa-arrow-up"></i>
                                </a>
                                <span id="upvote_counter" data-post-id='<%# Eval("ID") %>' class="d-flex flex-row justify-content-center align-items-center mx-auto" style="width: 4rem;">
                                    <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("UpvoteCount"))) %>
                                </span>
                                <a role="button" id="downvote_button" runat="server" class="btn mr-1 p-0 text-gray-500 downvote" data-post-id='<%# Convert.ToInt32(Eval("ID")) %>'
                                    data-toggle="popover" data-placement="top" data-content="you must log in to vote" data-trigger="focus">
                                    <i tabindex="-1" class="fas fa-arrow-down"></i>
                                </a>
                                <span id="downvote_counter" data-post-id='<%# Eval("ID") %>' class="d-flex flex-row justify-content-center align-items-center mx-auto" style="width: 4rem;">
                                    <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("DownvoteCount"))) %>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div id="comment_view">
        <asp:Repeater ID="CommentViewRepeater" runat="server">
            <ItemTemplate>

                <div class="card w-100">
                    <h5 class="card-title text-gray-900"><%# Eval("Title") %></h5>
                </div>

            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>

