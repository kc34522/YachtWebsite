<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YachtSubMenu.ascx.cs" Inherits="TayanaYacht.UserControls.YachtSubMenu" %>

<!--次選單-->
<div class="menu_y">
    <ul>
        <li class="menu_y00">YACHTS</li>
        <li>
            <a class="menu_yli01" href='<%# "Yachts_OverView.aspx?id=" + YachtId %>'>Overview</a>
        </li>
        <li>
            <a class="menu_yli02" href='<%# "Yachts_Layout.aspx?id=" + YachtId %>'>Layout &amp; deck plan</a>
        </li>
        <li>
            <a class="menu_yli03" href='<%# "Yachts_Specification.aspx?id=" + YachtId %>'>Specification</a>
        </li>
        <li></li>
    </ul>
</div>
<!--次選單-->
