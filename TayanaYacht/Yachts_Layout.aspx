<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Yachts_Layout.aspx.cs" Inherits="TayanaYacht.Yachts_Layout" %>

<%@ Register Src="~/UserControls/YachtSubMenu.ascx" TagPrefix="uc" TagName="YachtSubMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Front_Assets/css/jquery.ad-gallery.css") %>">
    <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bannerMask" runat="server">
    <img src="<%= ResolveUrl("~/Front_Assets/images/banner01_masks.png") %>" alt="&quot;&quot;" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bannerSmall" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="banner" runat="server">
    <div class="banner">
        <div id="gallery" class="ad-gallery">
            <div class="ad-image-wrapper"></div>
            <div class="ad-controls" style="display: none"></div>
            <div class="ad-nav">
                <div class="ad-thumbs">
                    <ul class="ad-thumb-list">
                        <asp:Repeater ID="RepeaterCarousel" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href='<%# Eval("ImagePath") %>' style="width: 100%">
                                        <img src='<%# Eval("ImagePath") %>' height="63" />
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>

                </div>
            </div>
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <uc:YachtSubMenu ID="YachtSubMenu1" runat="server" YachtId='<%# Request.QueryString["id"] %>' />
    <div class="box6">
        <p>Layout &amp; deck plan</p>

        <ul>
            <asp:Repeater ID="RepeaterLayout" runat="server">
                <ItemTemplate>
                    <li>
                        <img src='<%# Eval("ImagePath") %>' alt="&quot;&quot;" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

    </div>
    <div class="clear"></div>
</asp:Content>
