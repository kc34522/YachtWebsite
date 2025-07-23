<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewsDetail.aspx.cs" Inherits="TayanaYacht.NewsDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
    <style>
        /* 限制 CKEditor 內容區塊中的圖片不超出容器寬度 */
        .box3 img {
            max-width: 100%;
            height: auto;
            display: block;
            margin: 4px 0;
        }

        .box3 p {
           float:left;
        }

        .downloads p {
            display: flex;
            justify-content: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bannerMask" runat="server">
    <img src="<%= ResolveUrl("~/Front_Assets/images/banner02_masks.png") %>" alt="&quot;&quot;" />
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
        <h4>
            <asp:Label ID="LabelTitle" runat="server"></asp:Label></h4>

        <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>

        <asp:Repeater ID="RepeaterImages" runat="server">
            <ItemTemplate>
                <p>
                    <img alt="<%# Eval("AltText") %>" src="<%# Eval("ImagePath") %>" style="width: 660px;" />
                </p>
            </ItemTemplate>
        </asp:Repeater>
    </div>

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

    <div class="buttom001">
        <a href="NewsList.aspx">
            <img src="<%= ResolveUrl("~/Front_Assets/images/back.gif") %>" alt="&quot;&quot;" width="55" height="28" /></a>
    </div>
</asp:Content>
