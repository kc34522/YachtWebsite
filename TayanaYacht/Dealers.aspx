<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dealers.aspx.cs" Inherits="TayanaYacht.Dealers" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
    <style>
        .list02 ul {
            display: flex;
            flex - wrap: nowrap;
            align - items: flex - start;
        }

        .list02li {
            flex: 0 0 auto;
            margin - right: 20px;
        }

        .list02 ul li: last - child {
            flex: 1;
            word -break: break-word;
            overflow - wrap: break-word;
            min - width: 0; /* 避免超出容器 */
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="bannerMask" runat="server">
    <img src="<%= ResolveUrl("~/Front_Assets/images/DEALERS.jpg") %>" alt="&quot;&quot;" width="967" height="371" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bannerSmall" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="banner" runat="server">
    <div class="banner">
        <ul>
            <li>
                <img src="<%= ResolveUrl("~/Front_Assets/images/newbanner.jpg") %>" alt="Tayana Yachts" /></li>
        </ul>
    </div>
</asp:Content>

<%--有時間改為ckeditor--%>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box2_list">
        <ul>
            <asp:Repeater ID="RepeaterDealerList" runat="server" OnItemDataBound="RepeaterDealerList_ItemDataBound">
                <ItemTemplate>
                    <li>
                        <div class="list02">
                            <ul>
                                <li class="list02li">
                                    <div>
                                        <p>
                                            <img src='<%# Eval("ImagePath") %>' alt="DealerPicture" onerror="this.onerror=null;this.src='Front_Assets/images/default2.jpg';" />
                                            <%--加上 onerror 事件的方式，在圖片載入失敗（找不到圖檔或欄位是空的）時顯示預設圖片，這是初學者最簡單實用的做法--%>
                                        </p>
                                    </div>
                                </li>
                                <li><span><%# Eval("RegionName") %></span><br />
                                    <%# Eval("DealerName") %><br />
                                    <asp:PlaceHolder ID="PlaceHolderContact" runat="server">Contact：<%# Eval("Contact") %>
                                        <br />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolderAddress" runat="server">Address：<%# Eval("Address") %><br />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolderTel" runat="server">TEL：<%# Eval("Tel") %><br />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolderFax" runat="server">FAX：<%# Eval("Fax") %><br />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolderCell" runat="server">CELL：<%# Eval("Cell") %><br />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolderEmail" runat="server">E-mail：<%# Eval("Email") %><br />
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="PlaceHolderWebsite" runat="server"><a href='<%# Eval("Website") %>' target="_blank"><%# Eval("Website") %></a>
                                    </asp:PlaceHolder>
                                </li>
                            </ul>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        <%--待做: 分頁--%>
    </div>
</asp:Content>
