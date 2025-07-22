<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Company.aspx.cs" Inherits="TayanaYacht.Company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
    <style>
        .pit ul {
            display: flex;
            flex-wrap: wrap;
            gap: 20px; /* 圖片之間的間距 */
            justify-content: center; /* 讓圖片置中 */
            padding: 0;
            list-style: none;
            margin: 0 auto;
            max-width: 1000px; /* 可視需要調整 */
        }

            .pit ul li {
                flex: 0 1 calc(33.333% - 20px); /* 每排最多3張圖片，可自動縮放 */
                box-sizing: border-box;
                text-align: center;
            }

                .pit ul li img {
                    max-width: 100%;
                    height: auto;
                    border: 1px solid #ccc;
                    border-radius: 4px;
                }
    </style>

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
