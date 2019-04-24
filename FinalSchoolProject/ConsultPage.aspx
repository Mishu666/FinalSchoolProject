<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultPage.aspx.cs" Inherits="ConsultPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main_content" runat="Server">

    <% ConsultPagesClass page = ConsultPagesClass.GetByID(Convert.ToInt32(ViewState["PageID"]));  %>

    <asp:Repeater ID="ConsultPageRepeater" runat="server">

        <ItemTemplate>

            <Amir:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />

        </ItemTemplate>

    </asp:Repeater>

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

