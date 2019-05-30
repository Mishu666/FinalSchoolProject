<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="home_head" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Script/Home.js"></script>
    <link rel="stylesheet" href="Style/Home.css" />
    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="nav_items_cph" runat="server">
    <!-- Nav Item - Sort Direction-->
    <li class="nav-item dropdown no-arrow mx-1" visible="true"><a class="nav-link dropdown-toggle"
        href="#" id="sortDirectionDropdown" role="button" data-toggle="dropdown" aria-haspopup="true"
        aria-expanded="false"><i class="fas fa-fw"></i></a></li>
    <!-- Nav Item - Sort -->
    <li class="nav-item dropdown no-arrow mx-1" visible="true"><a class="nav-link dropdown-toggle"
        id="sortDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <i class="fas fa-sort-amount-down fa-fw"></i></a>
        <!-- Dropdown - Sort Options -->
        <div class="dropdown-list dropdown-menu dropdown-menu-right shadow animated--grow-in w-auto"
            aria-labelledby="sortOptionsDropdown" id="sortOptions">
            <a class="dropdown-item d-flex align-items-center" id="sortByNew">New </a><a class="dropdown-item d-flex align-items-center"
                id="sortByRating">Top </a>
        </div>
    </li>
    <li id="Li1" class="nav-item dropdown no-arrow mx-1" runat="server" visible="true">
        <div class="nav-link" id="sort_option_text">
            New
        </div>
    </li>
</asp:Content>

<asp:Content ID="home_content" ContentPlaceHolderID="main_content" runat="Server">

    <div id="posts_space" class="d-flex flex-column w-100 px-4 py-3" style="overflow-y: scroll !important; overflow-x: hidden !important;">
        <asp:Repeater runat="server" ID="HomePostRepeater">
            <ItemTemplate>
                <uc:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />
            </ItemTemplate>
        </asp:Repeater>

    </div>

</asp:Content>
