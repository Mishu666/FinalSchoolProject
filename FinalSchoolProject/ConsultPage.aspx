<%@ Page Title="" Language="C#" MasterPageFile="~/PostPagesMasterPage.master" AutoEventWireup="true" CodeFile="ConsultPage.aspx.cs" Inherits="ConsultPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="master_head" runat="Server">

</asp:Content>

<asp:Content ID="navbar_content" ContentPlaceHolderID="nav_items_cph" runat="server">

    <!-- Nav Item - Add -->

    <li class="nav-item dropdown no-arrow mx-1" runat="server" visible="true">
        <a class="nav-link dropdown-toggle" href="#" id="addPostDropDown" role="button" data-toggle="modal" aria-haspopup="true" aria-expanded="false" data-target="#AddPostModal">
            <i class="fas fa-fw fa-plus"></i>
        </a>
    </li>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="sidebar" runat="Server">

    <% ConsultPagesClass page = ConsultPagesClass.GetByID(Convert.ToInt32(ViewState["PageID"]));  %>

    <div class="card ml-5 shadow-sm h-100">
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

</asp:Content>

