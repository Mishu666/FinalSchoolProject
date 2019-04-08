<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="home_head" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="home_main_content" ContentPlaceHolderID="main_content" runat="Server">


    <asp:Repeater ID="HomePostsRepeater" runat="server">

        <ItemTemplate>

            <div class="card w-100 my-3">
                <div class="card-body">
                    <h5 class="card-title text-gray-900"><%# Eval("Title") %></h5>
                    <p class="card-text">
                        <%# Eval("Body") %>
                    </p>
                </div>
            </div>

        </ItemTemplate>

    </asp:Repeater>



</asp:Content>

