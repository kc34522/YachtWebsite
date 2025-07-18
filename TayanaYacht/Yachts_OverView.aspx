<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Yachts_OverView.aspx.cs" Inherits="TayanaYacht.Yachts_OverView" %>

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
    <div class="box1">
        <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>
    </div>
    <div class="box3">
        <h4>
            <asp:Literal ID="LiteralNumber" runat="server"></asp:Literal>
            DIMENSIONS</h4>
        <table class="table02">
            <tbody>
                <tr>
                    <td class="table02td01">
                        <table>
                            <tbody>
                                <asp:Repeater ID="RepeaterSpec" runat="server">
                                    <ItemTemplate>
                                        <tr class='<%# Container.ItemIndex %2==1?"tr003":"" %>'>
                                            <th><%# Eval("SpecLabel") %></th>
                                            <td><%# Eval("SpecValue") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </td>
                    <td>
                        <asp:Image ID="ImageDimension" runat="server" AlternateText="Tayana" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <p class="topbuttom">
        <a href="#">
            <img src="<%= ResolveUrl("~/Front_Assets/images/top.gif") %>" alt="top" />
        </a>
    </p>
    <!--下載開始-->
    <div class="downloads">
        <p>
            <img src="<%= ResolveUrl("~/Front_Assets/images/downloads.gif") %>" alt="&quot;&quot;" />
        </p>
        <ul>
            <asp:Repeater ID="RepeaterDownloads" runat="server">
                <ItemTemplate>
                    <li><a href='<%# Eval("FilePath") %>' target="_blank"><%# Eval("DisplayName") %></a></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <!--下載結束-->

</asp:Content>
