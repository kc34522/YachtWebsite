<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightMenu.ascx.cs" Inherits="TayanaYacht.UserControls.RightMenu" %>

<div id="crumb">
    <asp:Repeater ID="RepeaterRightMenu" runat="server" OnItemDataBound="RepeaterRightMenu_ItemDataBound">
        <ItemTemplate>
        <%-- 🔍 顯示目前第幾個項目、總共幾筆資料（測試用） --%>
        <%--<div style="color:red;">
            Index: <%# Container.ItemIndex %>,
            Total: <%# ((Repeater)Container.NamingContainer).Items.Count %>
        </div>--%>
            <asp:Literal ID="ltlMenuItem" runat="server"></asp:Literal>
            <asp:Literal ID="ltlArrow" runat="server"></asp:Literal>

            <%--<a href="<%# Eval("Url") %>"><%# Eval("Text") %></a>--%> 
            <%--Container.ItemIndex 是目前項目的索引。
            Container.NamingContainer 會回傳目前 Repeater 的實體，轉型成 (Repeater) 就可以用 .Items.Count。--%>
           <%-- <%# Container.ItemIndex <  ((Repeater)Container.NamingContainer).Items.Count - 1 ? "&gt;&gt;": ""	%>--%>
            
        </ItemTemplate>
    </asp:Repeater>

</div>

