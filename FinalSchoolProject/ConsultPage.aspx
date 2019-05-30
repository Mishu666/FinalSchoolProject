<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultPage.aspx.cs" Inherits="ConsultPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>

<asp:Content ContentPlaceHolderID="nav_items_cph" runat="server">

    <!-- Nav Item - Add Post -->

    <li class="nav-item dropdown no-arrow mx-1" visible="true">
        <a class="nav-link dropdown-toggle dismissable_popover" href="#" id="addPostInConsultPageButton" role="button"
            data-consult-page-id="<%= Convert.ToInt32(ViewState["PageID"]) %>"
            data-placement="top" data-content="you must log in to post">
            <i class="fas fa-plus"></i>
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

    <li class="nav-item dropdown no-arrow mx-1">
        <div class="nav-link" id="sort_option_text">
            New
        </div>
    </li>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="Server">

    <% ConsultPagesClass page = ConsultPagesClass.GetByID(Convert.ToInt32(ViewState["PageID"]));  %>
    <div id="content_space" class="overflow-hidden d-flex flex-row py-3 px-4 h-100">
        <div id="main_space" class="d-flex flex-column w-75">

            <div class="card shadow-sm w-100 mb-3 hidden_post" id="hidden_post_card" style="display: none;">
                <div class="card-body">

                    <div class="input-group mb-3">
                        <input id="addPostTitle" type="text" class="form-control" placeholder="Title" aria-label="Username">
                    </div>
                    <div class="input-group mb-3">
                        <textarea id="addPostBody" class="form-control" placeholder="Post body"></textarea>
                    </div>

                    <div id="add_post_warning_space" class="w-100"></div>
                    <div id="add_post_footer" class="d-flex flex-row justify-content-end">
                        <div class="mr-auto">Will be posted to - <%= page.PageName %></div>
                        <button class="btn btn-secondary mr-2" id="cancel_post_button">Cancel</button>
                        <button class="btn btn-primary" id="confirm_post_button" data-page-id="<%= page.ID %>">Submit</button>
                    </div>
                </div>
            </div>

            <div id="post_space" class="pr-3" style="overflow-y: scroll !important; overflow-x: hidden !important;">
                <asp:Repeater ID="ConsultPageRepeater" runat="server">

                    <ItemTemplate>

                        <uc:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />

                    </ItemTemplate>

                </asp:Repeater>
            </div>
        </div>

        <div id="page_info_space" class="ml-5 d-flex flex-column h-100 w-25">
            <div class="card shadow-sm h-100">
                <h1 class="card-header"><%= page.PageName %></h1>
                <div class="card-body">
                    <div class="card-text">
                        <%= page.Description %>
                    </div>
                </div>

                <%
                    if (Session["Logged"] != null && (bool)Session["Logged"])
                    {
                        UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));
                        if (!current_user.IsSubscribedTo(page.ID))
                        {
                %>

                <button id="SubscribeButton" type="button" class="btn btn-primary my-3">Subscribe</button>
                
                <%
                        }
                        else
                        {
                %>
                
                <button id="UnsubscribeButton" type="button" class="btn btn-secondary my-3">Unsubscribe</button>

                <%
                        }
                    }
                %>

                <div class="card-footer text-md-center">
                    <%= page.SubscriberCount %> members
                </div>
            </div>
        </div>
    </div>
</asp:Content>

