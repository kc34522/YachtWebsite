<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="TayanaYacht.Admin.NewsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<%--優化: 搜尋功能 分頁--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HyperLink ID="HyperLinkAdd" runat="server" NavigateUrl="~/Admin/NewsAddEdit.aspx">新增新聞</asp:HyperLink>
    <asp:Label ID="LabelGridViewMessage" runat="server" ></asp:Label>
    <asp:GridView ID="GridViewNews" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="GridViewNews_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="序號">
                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="標題" SortExpression="Title"></asp:BoundField>
            <asp:CheckBoxField DataField="IsTop" HeaderText="是否置頂" SortExpression="IsTop"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsVisible" HeaderText="是否顯示" SortExpression="IsVisible"></asp:CheckBoxField>
            <asp:BoundField DataField="PublishDate" HeaderText="上架日期" SortExpression="PublishDate" DataFormatString="{0:yyyy-MM-dd}"></asp:BoundField> 
            <asp:TemplateField HeaderText="功能">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLinkEdit" runat="server" NavigateUrl='<%# ResolveUrl("~/Admin/NewsAddEdit.aspx?Id=")+Eval("Id") %>'>管理</asp:HyperLink>
                    <%--<asp:Button ID="ButtonDelete" runat="server" Text="刪除"/>--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>
</asp:Content>
