<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TayanaYacht.Admin.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
    Contact List
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--搜尋功能--%>
    <%--Panel 加 DefaultButton 指定搜尋按鈕按 Enter 時，就會正確觸發搜尋功能，而不會誤觸到「登出」或其他上面的按鈕。--%>
   <%-- <asp:Panel runat="server" DefaultButton="ButtonSearch">
        <asp:TextBox ID="TextBoxSearch" runat="server" TextMode="Search"></asp:TextBox>
        <asp:Button ID="ButtonSearch" runat="server" Text="搜尋" OnClick="ButtonSearch_Click" />
    </asp:Panel>
    <asp:Button ID="ButtonClearSearch" runat="server" Text="清除搜尋" OnClick="ButtonClearSearch_Click" />
    <br />
    <br />
    <asp:Label ID="LabelSearchResult" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
    <br />
    <br />--%>

    <%--回覆清單列表--%>
    <h4 class="mb-3">回覆清單列表</h4>
    <asp:Label ID="LabelContactListMessage" runat="server" Text="" Visible="false"></asp:Label>
    <br />
    <asp:GridView ID="GridViewContact" runat="server" AutoGenerateColumns="False" OnRowDeleting="GridViewConctact_RowDeleting">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" InsertVisible="False" SortExpression="Id" Visible="false"></asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="姓名" SortExpression="Name"></asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="信箱" SortExpression="Email"></asp:BoundField>
            <asp:BoundField DataField="Phone" HeaderText="電話" SortExpression="Phone"></asp:BoundField>
            <asp:BoundField DataField="CountryName" HeaderText="國家" SortExpression="CountryName"></asp:BoundField>
            <asp:BoundField DataField="Yacht" HeaderText="宣傳單:船型" SortExpression="Yacht"></asp:BoundField>
            <asp:BoundField DataField="Comments" HeaderText="回覆內容" SortExpression="Comments"></asp:BoundField>
            <asp:BoundField DataField="CreatedTime" HeaderText="回覆時間" SortExpression="CreatedTime"></asp:BoundField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>

    </asp:GridView>

</asp:Content>
