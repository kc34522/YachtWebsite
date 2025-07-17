<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="YachtsList.aspx.cs" Inherits="TayanaYacht.Admin.YachtsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HyperLink ID="HyperLinkAdd" runat="server" NavigateUrl="~/Admin/YachtEdit.aspx">新增船型</asp:HyperLink>
    <asp:Label ID="LabelGridViewYacht" runat="server"></asp:Label>
    <asp:GridView ID="GridViewYachtList" runat="server" AutoGenerateColumns="False" DataKeyNames="YachtID" OnRowDeleting="GridViewYachtList_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="序號">
                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ModelName" HeaderText="船型" SortExpression="ModelName"></asp:BoundField>
            <asp:CheckBoxField DataField="IsActive" HeaderText="顯示前台" SortExpression="IsActive"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsNewBuilding" HeaderText="全新建造(New Building)" SortExpression="IsNewBuilding"></asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsNewDesign" HeaderText="全新設計(New Design)" SortExpression="IsNewDesign"></asp:CheckBoxField>
            <asp:TemplateField HeaderText="功能">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLinkEdit" runat="server" NavigateUrl='<%# ResolveUrl("~/Admin/YachtEdit.aspx?Id=")+Eval("YachtID") %>'>管理</asp:HyperLink>
                    <%--<asp:Button ID="ButtonDelete" runat="server" Text="刪除"/>--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>
</asp:Content>
