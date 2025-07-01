<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DealerList.aspx.cs" Inherits="TayanaYacht.Admin.DealerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="ButtonAddDealer" runat="server" Text="新增代理商" PostBackUrl="~/Admin/DealerEdit.aspx" />
    <%--回覆清單列表--%>
    <h4 class="mb-3">經銷商列表</h4>
    <asp:Label ID="LabelDealerListMessage" runat="server" Text="" Visible="false"></asp:Label>
    <br />
    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
    </asp:DropDownList>
    <br />

    <asp:GridView ID="GridViewDealer" runat="server" AutoGenerateColumns="False">
        <%--GridView 無資料處理--%>
        <EmptyDataTemplate>
            <div style="padding: 10px;">--- 查無符合的經銷商資料 ---</div>
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="RegionName" HeaderText="地區" SortExpression="RegionName"></asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="代理商名稱" SortExpression="Name"></asp:BoundField>
            <asp:BoundField DataField="Tel" HeaderText="電話" SortExpression="Tel"></asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="電子信箱" SortExpression="Email"></asp:BoundField>
            <asp:BoundField DataField="Website" HeaderText="網站" SortExpression="Website"></asp:BoundField>
            <asp:TemplateField HeaderText="前台顯示" SortExpression="IsActive">
                <EditItemTemplate>
                    <asp:CheckBox ID="CheckBox1" checked='<%# Bind("IsActive") %>' Enabled="false" runat="server" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox2" checked='<%# Bind("IsActive") %>' Enabled="false" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="功能">
                <%--待學習--%>
                <ItemTemplate>
                    <a href='DealerEdit.aspx?id=<%# Eval("Id") %>'>編輯</a> |
                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" OnClientClick="return confirm('確認刪除？');">刪除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
