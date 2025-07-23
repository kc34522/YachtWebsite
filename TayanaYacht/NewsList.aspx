<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="TayanaYacht.NewsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
    <style>
        /* 之前教你加的 .news-image-container 樣式可以刪掉了，
  我們現在改用 .image-wrapper
*/

        /* 1. 設定 <p> 標籤為定位基準 */
        .image-wrapper {
            position: relative; /* 關鍵！設定為定位基準點 */
            display: inline-block; /* 讓 <p> 像圖片一樣，寬高由內容撐開，而不是佔滿整行 */
            line-height: 0; /* 消除圖片底部可能的多餘空隙 */
            font-size: 0; /* 消除因換行或空格產生的潛在空隙 */
        }

        /* 2. TOP 標籤的樣式完全不用改，它會自動參考父層的 .image-wrapper */
        .top-news-tag {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 10;
            width: 50px; /* 這裡的尺寸請依你的圖片調整 */
            height: auto;
        }

        /* 3. (可選，但建議) 確保主圖尺寸一致 */
        .image-wrapper > img:first-child {
            width: 200px; /* 範例尺寸 */
            height: 150px; /* 範例尺寸 */
            object-fit: cover;
        }

        .pagination {
            display: inline-block;
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination > li {
                display: inline;
            }

                .pagination > li > a, .pagination > li > span {
                    position: relative;
                    float: left;
                    padding: 6px 12px;
                    margin-left: -1px;
                    line-height: 1.42857143;
                    color: #337ab7;
                    text-decoration: none;
                    background-color: #fff;
                    border: 1px solid #ddd;
                }

                .pagination > li.active > a,
                .pagination > li.active > a:hover,
                .pagination > li.active > a:focus {
                    z-index: 2;
                    color: #fff;
                    cursor: default;
                    background-color: #337ab7;
                    border-color: #337ab7;
                }

                .pagination > li > a:hover,
                .pagination > li > a:focus {
                    color: #23527c;
                    background-color: #eee;
                    border-color: #ddd;
                }

        .image-wrapper {
            text-align: center;
        }

            .image-wrapper img {
                max-width: 100%;
                height: auto;
                display: block;
                margin: 0 auto;
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
    <div class="box2_list">
        <ul>
            <asp:Repeater ID="RepeaterNews" runat="server">
                <ItemTemplate>
                    <li>
                        <div class="list01">
                            <ul>
                                <li>
                                    <div>
                                        <p class="image-wrapper">
                                            <img src='<%# Eval("ImagePath") %>' alt='<%# Eval("AltText") %>' onerror="this.onerror=null;this.src='Front_Assets/images/default2.jpg';" border="0" />
                                            <%# (bool)Eval("IsTop") ? $"<img src='{ResolveUrl("~/Front_Assets/images/new_top01.png")}' class='top-news-tag' alt='置頂新聞' />" : "" %>
                                        </p>
                                    </div>
                                </li>
                                <li>
                                    <span><%# ((DateTime)Eval("PublishDate")).ToString("yyyy-MM-dd") %></span>
                                    <br />
                                    <a href="NewsDetail.aspx?id=<%# Eval("Id") %>"><%# Eval("Title") %></a>
                                </li>
                                <br />
                                <%--優化 看更多--%>
                                <li><%# Eval("Title") %></li>
                            </ul>
                        </div>
                    </li>

                </ItemTemplate>

            </asp:Repeater>


        </ul>

        <div style="text-align: center;">
            <asp:Literal ID="ltlPager" runat="server"></asp:Literal>
        </div>


    </div>
</asp:Content>
