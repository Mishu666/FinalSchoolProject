<%@ Page Title="" Language="C#" MasterPageFile="~/PostPagesMasterPage.master" AutoEventWireup="true" CodeFile="ViewPost.aspx.cs" Inherits="ViewPost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="master_head" runat="Server">

    <style>
        #content_space {
            flex-direction: column !important;
        }

        #post_space {
            width: 100% !important;
            overflow: unset !important;
            padding: 0 !important;
        }

        #sidebar_container {
            width: 100% !important;
            overflow: auto;
            padding: 0 !important;
        }
    </style>

</asp:Content>

<asp:Content ID="navbar_content" ContentPlaceHolderID="nav_items_cph" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="sidebar" runat="Server">

    <div id="comment_view" runat="server">

    </div>

</asp:Content>


