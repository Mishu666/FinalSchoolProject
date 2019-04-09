<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="home_head" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="home_main_content" ContentPlaceHolderID="main_content" runat="Server">


    <asp:Repeater ID="HomePostsRepeater" runat="server" OnItemDataBound="HomePostsRepeater_ItemDataBound">

        <ItemTemplate>

            <div class="card w-100 mb-3">
                <div class="card-body">
                    <h5 class="card-title text-gray-900"><%# Eval("Title") %></h5>
                    <p class="card-text">
                        <%# Eval("Body") %>
                    </p>
                    <div class="d-flex justify-content-between align-items-end text-gray-700">
                        <div class="comments-block w-25">
                            <i class="fas fa-comment"></i>
                            <span class="comment_count">
                                <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("CommentCount"))) %>
                            </span>
                        </div>
                        <div class="vote-block d-flex flex-row justify-content-between">
                            <button id="upvote_button" runat="server" class="btn mr-3 p-0 text-gray-500 upvote mr-2">
                                <i class="fas fa-arrow-up"></i>
                            </button>
                            <button id="downvote_button" runat="server" class="btn mr-3 p-0 text-gray-500 downvote mr-2">
                                <i class="fas fa-arrow-down"></i>
                            </button>
                            <span id="vote_count" class="d-flex flex-row justify-content-center align-items-center">
                                <%# GlobalFunctions.FormatNumber(Convert.ToInt32(Eval("VoteCount"))) %>
                            </span>
                        </div>
                    </div>
                </div>
            </div>

        </ItemTemplate>

    </asp:Repeater>

</asp:Content>

