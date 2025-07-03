<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftMenu.ascx.cs" Inherits="TayanaYacht.LeftMenu" %>
<div class="left">
    <div class="left1">
        <p><span><%= TitleText %></span></p>
        <ul>
            <asp:Repeater ID="RepeaterLeftMenu" runat="server">
                <ItemTemplate>
                    <li><a href="<%# Eval("Url") %>"><%# Eval("Text") %></a></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>
