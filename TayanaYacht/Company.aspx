<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="TayanaYacht.Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
     <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bannerMask" runat="server">
    <img src="<%= ResolveUrl("~/Front_Assets/images/company.jpg") %>" alt="&quot;&quot;" width="967" height="371" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bannerSmall" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="banner" runat="server">
    <div class="banner">
        <ul>
            <li>
                <img src="<%= ResolveUrl("~/Front_Assets/images/newbanner.jpg") %>" alt="Tayana Yachts" /></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="box3">
        <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>
    </div>
</asp:Content>
