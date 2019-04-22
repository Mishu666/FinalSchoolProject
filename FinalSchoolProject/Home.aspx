<%@ Page Title="" Language="C#" MasterPageFile="~/PostPagesMasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="home_head" ContentPlaceHolderID="master_head" runat="Server">

    <script type="text/javascript" src="Script/Home.js"></script>
    <link rel="stylesheet" href="Style/Home.css" />

</asp:Content>

<asp:Content ID="home_sidebar" ContentPlaceHolderID="sidebar" runat="Server">

    <div id="user_calendar" class="mb-3"></div>
    <div class="w-100 flex-grow-1 border"></div>

</asp:Content>
