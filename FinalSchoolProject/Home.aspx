<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="home_head" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

    <script type="text/javascript" src="Script/Home.js"></script>
    <link rel="stylesheet" href="Style/Home.css" />

    <!-- Full Calendar -->
    <link rel="stylesheet" href="vendor/fullcalendar/core/main.css" />
    <link rel="stylesheet" href="vendor/fullcalendar/daygrid/main.css" />
    <link rel="stylesheet" href="vendor/fullcalendar/list/main.css" />
    <link rel="stylesheet" href="vendor/fullcalendar/timegrid/main.css" />
    <link rel="stylesheet" href="vendor/fullcalendar/bootstrap/main.css" />

    <script type="text/javascript" src="vendor/fullcalendar/core/main.js"></script>
    <script type="text/javascript" src="vendor/fullcalendar/daygrid/main.js"></script>
    <script type="text/javascript" src="vendor/fullcalendar/list/main.js"></script>
    <script type="text/javascript" src="vendor/fullcalendar/timegrid/main.js"></script>
    <script type="text/javascript" src="vendor/fullcalendar/bootstrap/main.js"></script>

</asp:Content>

<asp:Content ID="hom_nav_items" ContentPlaceHolderID="nav_items_cph" runat="server">

    <!-- Nav Item - Sort Direction-->

    <li class="nav-item dropdown no-arrow mx-1" runat="server" visible="true">
        <a class="nav-link dropdown-toggle" href="#" id="sortDirectionDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            <i class="fas fa-fw"></i>
        </a>
    </li>

    <!-- Nav Item - Sort -->

    <li class="nav-item dropdown no-arrow mx-1" runat="server" visible="true">
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

<asp:Content ID="home_main_content" ContentPlaceHolderID="main_content" runat="Server">

    <div class="page_content overflow-auto px-5 w-75">

        <asp:Repeater ID="HomePostsRepeater" runat="server" OnItemDataBound="HomePostsRepeater_ItemDataBound">

            <ItemTemplate>

                <div class="card shadow w-100 mb-3 post" data-date="<%# Eval("CreationDate") %>" data-rating="<%# Convert.ToInt32(Eval("UpvoteCount")) - Convert.ToInt32(Eval("DownvoteCount")) %>">
                    <div class="card-body">
                        <h5 class="card-title text-gray-900"><%# Eval("Title") %></h5>
                        <p class="card-text ">
                            <%# Eval("Body") %>
                        </p>
                        <div class="d-flex flex-row justify-content-between align-items-end text-gray-700">
                            <div class="comments-block d-flex flex-row justify-content-center align-items-center">
                                <i class="fas fa-comment mr-1"></i>
                                <span class="comment_count d-flex flex-row justify-content-center align-items-center mx-auto" style="width: 4rem;">
                                    <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("CommentCount"))) %>
                                </span>
                            </div>
                            <div class="mr-auto">
                                potsed by <a href="#"><%# PostsClass.GetAuthorUsername(Convert.ToInt32(Eval("ID"))) %></a> on <%# PostsClass.GetCreationDate(Convert.ToInt32(Eval("ID"))).ToShortDateString() %>
                            </div>
                            <div class="vote-block d-flex flex-row">
                                <a role="button" id="upvote_button" runat="server" class="btn mr-1 p-0  text-gray-500 upvote" data-post-id='<%# Convert.ToInt32(Eval("ID")) %>'
                                    data-toggle="popover" data-placement="top" data-content="you must log in to vote" data-trigger="focus">
                                    <i tabindex="-1" class="fas fa-arrow-up"></i>
                                </a>
                                <span id="upvote_counter" data-post-id='<%# Convert.ToInt32(Eval("ID")) %>' class="d-flex flex-row justify-content-center align-items-center mx-auto" style="width: 4rem;">
                                    <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("UpvoteCount"))) %>
                                </span>
                                <a role="button" id="downvote_button" runat="server" class="btn mr-1 p-0 text-gray-500 downvote" data-post-id='<%# Convert.ToInt32(Eval("ID")) %>'
                                    data-toggle="popover" data-placement="top" data-content="you must log in to vote" data-trigger="focus">
                                    <i tabindex="-1" class="fas fa-arrow-down"></i>
                                </a>
                                <span id="downvote_counter" data-post-id='<%# Convert.ToInt32(Eval("ID")) %>' class="d-flex flex-row justify-content-center align-items-center mx-auto" style="width: 4rem;">
                                    <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("DownvoteCount"))) %>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>

            </ItemTemplate>

        </asp:Repeater>

    </div>

    <div id="calendar_container" class="d-flex flex-column w-50 h-100 px-5">
        <div id="user_calendar" class="mb-3"></div>
        <div class="w-100 flex-grow-1 border"></div>
    </div>

</asp:Content>

