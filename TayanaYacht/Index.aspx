<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TayanaYacht.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Tayana | Tayana Yachts Official Website
    </title>
    <link rel="shortcut icon" href="<%= ResolveUrl("~/Front_Assets/images/favicon.ico") %>" />
    <link href="<%= ResolveUrl("~/Front_Assets/css/reset.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= ResolveUrl("~/Front_Assets/css/style.css") %>" rel="stylesheet" type="text/css" />


</head>
<body>

    <form id="form1" runat="server">

        <div class="contain">
            <div class="sub">
                <p>
                    <a href="Index.aspx">Home</a>
                </p>
            </div>
            <!--------------------------------選單開始---------------------------------------------------->
            <div id="logol">
                <a href="Index.aspx">
                    <img src="Front_Assets/images/logo001.gif" />
                    <%--<img src='<%# ResolveUrl("~/Front_Assets/images/logo001.gif") %>' alt="Tayana" />--%></a>
            </div>
            <div class="menu">
                <ul>
                    <li class="menuli01"><a href="/Yachts_OverView.aspx">Yachts</a></li>
                    <li class="menuli02"><a href="/NewsList.aspx">NEWS</a></li>
                    <li class="menuli03"><a href="/Company.aspx">COMPANY</a></li>
                    <li class="menuli04"><a href="/Dealers.aspx">DEALERS</a></li>
                    <li class="menuli05"><a href="/Contact.aspx">CONTACT</a></li>
                </ul>
            </div>
            <!--------------------------------選單開始結束---------------------------------------------------->
            <!--遮罩-->
            <div class="bannermasks">
                <img src="<%= ResolveUrl("~/Front_Assets/images/banner00_masks.png") %>" alt="&quot;&quot;" />
            </div>
            <!--遮罩結束-->
            <!--------------------------------換圖開始---------------------------------------------------->
            <div id="abgne-block-20110111">
                <div class="bd">
                    <div class="banner">
                        <ul>
                            <asp:Repeater ID="RepeaterBanner" runat="server">
                                <ItemTemplate>
                                    <li class="info"><a href="" target="_blank">
                                        <img src='<%# Eval("ImagePath") %>' width="966" height="424"/></a><!--文字開始--><div class="wordtitle">
                                            <%# Eval("ModelNameText") %> <span><%# Eval("ModelNameNumber") %></span><br />
                                            <p>
                                                SPECIFICATION SHEET
                                            </p>
                                        </div>
                                        <!--文字結束-->
                                        <!--新船型開始  54型才出現其於隱藏 -->
                                        <asp:PlaceHolder ID="PlaceHolderNewBuilding" runat="server" Visible='<%# Convert.ToBoolean(Eval("IsNewBuilding")) %>'>
                                            <div class="new">
                                                <img src='<%# ResolveUrl("~/Front_Assets/images/new01.png") %>' alt="new" />
                                            </div>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder ID="PlaceHolderNewDesign" runat="server" Visible='<%# Convert.ToBoolean(Eval("IsNewDesign")) %>'>
                                            <div class="new" visible='<%# Convert.ToBoolean(Eval("IsNewDesign")) %>'>
                                                <img src='<%# ResolveUrl("~/Front_Assets/images/new02.png") %>' alt="new" />
                                            </div>
                                        </asp:PlaceHolder>
                                        <!--新船型結束-->
                                        <%--                                        <input type="hidden" value="0" />--%>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>

                        <!--小圖開始-->
                        <div class="bannerimg title" >
                            <ul>
                                <asp:Repeater ID="RepeaterLittleBanner" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <div>
                                                <p class="bannerimg_p">
                                                    <img src="<%# Eval("ImagePath") %>" alt="TAYANA64SPECIFICATION SHEET" />
                                                </p>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </ul>



                        </div>
                        <!--小圖結束-->
                    </div>
                </div>
            </div>
            <!--------------------------------換圖結束---------------------------------------------------->
            <!--------------------------------最新消息---------------------------------------------------->
            <div class="news">
                <div class="newstitle">
                    <p class="newstitlep1">
                        <img src="<%= ResolveUrl("~/Front_Assets/images/news.gif") %>" alt="news" />
                    </p>
                    <p class="newstitlep2">
                        <a href="NewsList.aspx">More&gt;&gt;</a>
                    </p>
                </div>

                <ul>
                    <asp:Repeater ID="RepeaterNews" runat="server">
                        <ItemTemplate>
                            <li>
                                <div class="news01">
                                    <!--TOP標籤-->
                                    <div class="newstop" visible="<%# Convert.ToBoolean(Eval("IsTop")) %>">
                                        <img src="<%= ResolveUrl("~/Front_Assets/images/new_top01.png") %>" alt="&quot;&quot;" />
                                    </div>
                                    <!--TOP標籤結束-->
                                    <div class="news02p1">
                                        <p class="news02p1img">
                                            <img id="Repeater3_ctl01_Image1" src="<%# Eval("ImagePath") %>" border="0" />
                                        </p>
                                    </div>
                                    <p class="news02p2">
                                        <span><font color="#02a5b8"><%# ((DateTime)Eval("PublishDate")).ToString("yyyy-MM-dd") %></font></span>
                                        <span><a href='<%# "NewsDetail.aspx?id=" + Eval("Id") %>'><%# Eval("Title") %></a></span>
                                    </p>
                                    <%--                                    <input type="hidden" value="0" />--%>
                                </div>

                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>

            </div>
            <!--------------------------------最新消息結束---------------------------------------------------->
            <!--------------------------------落款開始---------------------------------------------------->
            <div class="footer">
                <div class="footerp00">
                    <a href="https://web.archive.org/web/20170716031852/http://www.tognews.com/" target="_blank">
                        <p>
                            <img src="<%= ResolveUrl("~/Front_Assets/images/tog.jpg") %>" alt="TOG" />
                    </a>
                    </p>
                
                    <p class="footerp001">
                        © 1973-2012 Tayana Yachts, Inc. All Rights Reserved
                    </p>
                </div>
                <div class="footer01">
                    <span>No. 60, Hai Chien Road, Chung Men Li, Lin Yuan District, Kaohsiung City, Taiwan,
                    R.O.C.</span><br />
                    <span>TEL：+886(7)641-2721</span> <span>FAX：+886(7)642-3193</span><span><a href="https://web.archive.org/web/20170716031852/mailto:tayangco@ms15.hinet.net">E-mail：tayangco@ms15.hinet.net</a>.</span>
                </div>
            </div>
            <!--------------------------------落款結束---------------------------------------------------->
        </div>

    </form>
    <script type="text/javascript" src="<%= ResolveUrl("~/Front_Assets/Scripts/jquery.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Front_Assets/Scripts/jquery.cycle.all.2.74.js") %>"></script>
    <script type="text/javascript">

        $(function () {
            // 先取得 #abgne-block-20110111 , 必要參數及輪播間隔
            var $block = $('#abgne-block-20110111'),
                timrt, speed = 4000;

            // 幫 #abgne-block-20110111 .title ul li 加上 hover() 事件
            var $li = $('.title ul li', $block).hover(function () {
                // 當滑鼠移上時加上 .over 樣式
                $(this).addClass('over').siblings('.over').removeClass('over');
            }, function () {
                // 當滑鼠移出時移除 .over 樣式
                $(this).removeClass('over');
            }).click(function () {
                // 當滑鼠點擊時, 顯示相對應的 div.info
                // 並加上 .on 樣式
                var $this = $(this);
                $this.add($('.bd li.info', $block).eq($this.index())).addClass('on').siblings('.on').removeClass('on');
            });

            // 幫 $block 加上 hover() 事件
            $block.hover(function () {
                // 當滑鼠移上時停止計時器
                clearTimeout(timer);
            }, function () {
                // 當滑鼠移出時啟動計時器
                timer = setTimeout(move, speed);
            });

            // 控制輪播
            function move() {
                var _index = $('.title ul li.on', $block).index();
                _index = (_index + 1) % $li.length;
                $li.eq(_index).click();

                timer = setTimeout(move, speed);
            }

            // 啟動計時器
            timer = setTimeout(move, speed);
        });


        //$(document).ready(function () {
        //    $('.slideshow').cycle({
        //        fx: 'fade' // choose your transition type, ex: fade, scrollUp, shuffle, etc...
        //    });
        //});
    </script>

</body>
</html>
