<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultPage.aspx.cs" Inherits="ConsultPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="Server">

    <% ConsultPagesClass page = ConsultPagesClass.GetByID(Convert.ToInt32(ViewState["PageID"]));  %>
    <div id="content_space" class="overflow-hidden d-flex flex-row py-3 px-4 h-100">
        <div id="posts_space" class="d-flex flex-column w-75 overflow-auto pr-3">

            <asp:Repeater ID="ConsultPageRepeater" runat="server">

                <ItemTemplate>

                    <uc:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />

                </ItemTemplate>

            </asp:Repeater>
        </div>

        <div id="page_info_space" class="ml-5 d-flex flex-column h-100 w-25">
            <div class="card shadow-sm h-100">
                <h1 class="card-header"><%= page.PageName %></h1>
                <div class="card-body">
                    <div class="card-text">
                        <%= page.Describtion %>
                    </div>
                </div>
                <div class="card-footer">
                    <%= page.SubscriberCount %> members
                </div>
            </div>
        </div>
    </div>
</asp:Content>

