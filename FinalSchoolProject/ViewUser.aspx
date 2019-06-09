<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ViewUser.aspx.cs" Inherits="ViewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="Script/ViewUser.js"></script>
    <link rel="stylesheet" href="Style/ViewUser.css" />

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">

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
<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">

        <% UsersClass user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"])); %>
        <% UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"])); %>
        <% bool IsMod = user.IsModerator(); %>

        <div id="main_space" class="card shadow-sm d-flex flex-column w-75 h-100 p-3">

            <ul class="nav nav-tabs">
                <li class="nav-item">
                    <a runat="server" class="nav-link active" id="ProfileMyPostsTab" visible="false" data-post-type="submitted"
                        onserverclick="ProfilePostsTab_ServerClick">Submitted</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileSavedPostsTab" visible="false" data-post-type="saved"
                        onserverclick="ProfilePostsTab_ServerClick">Saved</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileUpvotedPostsTab" visible="false" data-post-type="upvoted"
                        onserverclick="ProfilePostsTab_ServerClick">Upvoted</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileDownvotedPostsTab" visible="false" data-post-type="downvoted"
                        onserverclick="ProfilePostsTab_ServerClick">Downvoted</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileMessagesTab" visible="false"
                        onserverclick="ProfileMessagesTab_ServerClick">Messages</a>
                </li>
                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileConvoTab" visible="false"
                        onserverclick="ProfileConvoTab_ServerClick">Conversation</a>
                </li>

                <li class="nav-item">
                    <a runat="server" class="nav-link" id="ProfileNewMessageTab" visible="false"
                        onserverclick="NewMessageTab_ServerClick">Write Message</a>
                </li>
            </ul>

            <div id="user_posts_space" runat="server" class="d-flex flex-column pr-3 mt-3"
                style="overflow-y: scroll !important; overflow-x: hidden !important;">

                <asp:Repeater ID="ProfilePostsRepeater" runat="server">
                    <ItemTemplate>
                        <uc:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />
                    </ItemTemplate>
                </asp:Repeater>

            </div>

            <div id="user_messages_space" runat="server" class="d-flex flex-column pr-3 mt-3 h-100"
                style="overflow-y: scroll !important; overflow-x: hidden !important;" visible="false">
                
                <div class="accordion" id="MessagesAccordion">

                    <asp:Repeater ID="ProfileMessagesRepeater" runat="server">
                        <ItemTemplate>
                            <div class="card shadow-sm mb-3">
                                <div class="card-header d-flex flex-row justify-content-between align-items-baseline" id="heading_<%# Container.ItemIndex %>">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link" type="button" data-toggle="collapse" data-target="#collapse_<%# Container.ItemIndex %>" 
                                            aria-expanded="true" aria-controls="collapse_<%# Container.ItemIndex %>">
                                            Message From <%# Eval("Username") %>
                                        </button>
                                    </h2>
                                    <span class="text-md"><%# Eval("SendDate") %></span>
                                </div>

                                <div id="collapse_<%# Container.ItemIndex %>" class="collapse" 
                                    aria-labelledby="heading_<%# Container.ItemIndex %>" data-parent="#MessagesAccordion">
                                    <div class="card-body">
                                        <%# Eval("Body") %>
                                    </div>
                                    <div class="card-footer">
                                        <a class="btn btn-link" href='ViewUser.aspx?user-id=<%# Eval("SenderID") %>'>View Sender</a>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>


            </div>

            <div id="user_convo_space" runat="server" class="mt-3 w-100 pr-3 h-100" style="overflow-x: hidden !important; overflow-y: scroll !important;" visible="false">

                <asp:Repeater ID="ProfileConvoRepeater" runat="server" OnItemDataBound="ProfileConvoRepeater_ItemDataBound">
                    <ItemTemplate>
                        <div id="message_card" runat="server" class="card shadow-sm mb-3 w-100">
                            <div class="card-body">
                                <div class="card-text">
                                    <div><%# Eval("Body") %></div>
                                </div>
                            </div>
                            <div class="card-footer d-flex flex-row align-items-center justify-content-between py-1">
                                <div class="text-xs"><%# UsersClass.GetByID(Convert.ToInt32(Eval("SenderID"))).Username %></div>
                                <div class="text-xs"><%# Convert.ToDateTime(Eval("SendDate")).ToString("dd/MM/yyyy HH:mm") %></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

            <div id="new_message_space" runat="server" visible="false" class="d-flex flex-column justify-content-between align-items-baseline w-100 h-100">

                <div class="input-group h-100 my-3 EditableAreaSpace">
                    <div id="NewMessageInput" class="form-control h-100" aria-describedby="basic-addon-new-messagge"></div>
                </div>

                <div id="NewMessageFooter" class="w-100">
                    <button id="SendMessageButton" class="btn btn-primary" data-recipient-id='<%= Convert.ToInt32(ViewState["ViewUserID"]) %>'>
                        Send<i class="fas fa-paper-plane ml-2"></i>
                    </button>

                    <label id="NewMessageLabel" class="text-success text-md-left ml-5"></label>
                </div>

            </div>

        </div>

        <div id="user_space" class="card shadow-sm ml-3 d-flex flex-column h-100" style="width: 30% !important;">
            <img src="<%= user.ProfilePictureDir %>" alt="profile picture" onerror="this.remove();" class="card-img-top" />
            <div class="card-body" id="default_user_view">
                <h5 class="card-title mr-1">

                    <%= user.Username %>

                    <%
                        if (user.IsAdmin)
                        {
                    %>

                    <span id="admin_badge" class="badge badge-success">admin</span>

                    <%
                        }
                    %>

                    <%
                        if (IsMod)
                        {
                    %>

                    <span id="mod_badge" class="badge badge-danger">mod</span>

                    <%
                        }
                    %>

                </h5>

                <span class="font-weight-bold text-lg-center">Points: <%= user.Points %></span>

                <div class="card-text" style="overflow-y: auto !important; overflow-x: hidden !important; max-height: 5rem;"><%= user.Bio %></div>

                <% 
                    if (current_user != null && current_user.ID == user.ID)
                    {
                %>
                
                <button id="EditUserButton" type="button" class="btn btn-primary mt-3">
                    <i class="far fa-edit mr-2"></i>Edit
                </button>
                
                <button id="DeleteUserButton" type="button" class="btn btn-danger mt-3" data-user-id='<%= (int)Session["CurrentUserID"] %>'>
                    <i class="fas fa-times mr-2"></i>Delete
                </button>

                <%
                    }
                %>

                <span class="text-lg mt-3 d-block">Moderated Pages:</span>
                <ul class="list-group pr-3" style="height: 30% !important; overflow-y: visible !important;">
                    <asp:Repeater ID="ModeratedPagesRepeater" runat="server">
                        <ItemTemplate>
                            <li class="list-group-item d-flex justify-content-between align-items-center"><%# Eval("PageName") %>
                                <span class="badge badge-secondary badge-pill"><%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("SubscriberCount"))) %> members</span>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="NoPagesLabel" runat="server" CssClass="list-group-item d-flex justify-content-between align-items-center"
                                Visible='<%# ModeratedPagesRepeater.Items.Count == 0 %>' Text="None" />
                        </FooterTemplate>
                    </asp:Repeater>
                </ul>

            </div>
            <div class="card-body" id="edit_user_view" style="display: none;">

                <div class="input-group mb-3">
                    <input id="EditUsernameInput" type="text" class="form-control" placeholder="Username" aria-label="Username" aria-describedby="basic-addon-username"
                        value="<%= user.Username %>">
                </div>

                <div class="input-group mb-3 EditableAreaSpace">
                    <div id="EditBioInput" class="form-control EditableArea" aria-describedby="basic-addon-username"><%= user.Bio %></div>
                </div>

                <div class="input-group mb-3">
                    <input id="EditNewPasswordInput" type="password" class="form-control" placeholder="New password"
                        aria-label="New Password" aria-describedby="basic-addon-new-password">
                </div>
                <div class="input-group mb-3">
                    <input id="EditConfirmNewPasswordInput" type="password" class="form-control" placeholder="Confirm new password"
                        aria-label="Confirm new Password" aria-describedby="basic-addon-confirm-new-password">
                </div>

                <div class="input-group mb-3">
                    <input id="EditConfirmPasswordInput" type="password" class="form-control" placeholder="Current password"
                        aria-label="Confirm Password" aria-describedby="basic-addon-confirm-password">
                </div>


                <button id="CancelEditButton" type="button" class="btn btn-secondary">Cancel</button>
                <button id="ConfirmEditButton" type="button" class="btn btn-primary">Confirm</button>
                <div class="w-100 mt-3" id="edit_user_warning_space"></div>

            </div>
            <div class="card-footer d-flex flex-row justify-content-around align-items-center">
                <span>Member since <%= user.CreationDate.ToShortDateString() %></span>
            </div>
        </div>
    </div>

</asp:Content>
