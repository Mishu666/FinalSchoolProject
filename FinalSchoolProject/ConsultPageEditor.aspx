<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ConsultPageEditor.aspx.cs" Inherits="ConsultPageEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="nav_items_cph" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main_content" runat="Server">

    <div class="h-100 w-100 d-flex flex-column justify-content-center align-items-center py-3 px-4">

        <div id="ConsultPageSelectionBar" class="w-100 mb-3">

            <asp:DropDownList ID="ConsultPagesDDL" runat="server" CssClass="form-control w-25" AutoPostBack="true" 
                OnSelectedIndexChanged="ConsultPagesDDL_SelectedIndexChanged">
            </asp:DropDownList>

        </div>

        <div id="ConsultPageGVSection" class="w-100 flex-grow-1 d-flex flex-row justify-content-center align-items-center">

            <div id="UsersInPageSection" class="h-100 mr-3" style="width: 45% !important;">

                <asp:GridView ID="UsersInPageGV" runat="server" CssClass="table table-striped"></asp:GridView>

            </div>
            <div id="MoveUsersButtonSpace" class="flex-grow-1 h-100">

                <asp:Button ID="KickFromPageButton" runat="server" Text="hi" />

            </div>
            <div id="UsersNotInPageSection" class="h-100 ml-3" style="width: 45% !important;"></div>

        </div>

    </div>

</asp:Content>

