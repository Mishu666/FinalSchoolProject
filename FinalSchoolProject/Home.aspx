<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="home_head" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript" src="Script/Home.js"></script>
    <link rel="stylesheet" href="Style/Home.css" />

    <script type="text/javascript" src="Script/post_actions.js"></script>
    <link rel="stylesheet" href="Style/post_actions.css" />

</asp:Content>

<asp:Content ID="home_sidebar" ContentPlaceHolderID="main_content" runat="Server">

    <div id="content_space" class="overflow-hidden d-flex flex-row h-100 py-3 px-4">

        <div id="posts_space" class="d-flex flex-column w-75 overflow-auto pr-3">
            <asp:Repeater runat="server" ID="HomePostRepeater">

                <ItemTemplate>
                    <uc:Post runat="server" PostID='<%# Convert.ToInt32(Eval("ID")) %>' />
                </ItemTemplate>

            </asp:Repeater>
        </div>

        <div id="calendar_space" class="ml-5 d-flex flex-column h-100 w-25">
            <div id="user_calendar" class="mb-3"></div>
            <div class="w-100 flex-grow-1 border"></div>
        </div>

    </div>
</asp:Content>
