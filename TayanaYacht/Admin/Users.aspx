<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="TayanaYacht.Admin.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
    使用者管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="LabelGridView" runat="server" ></asp:Label>
    <asp:GridView ID="GridViewUser" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="GridViewUser_RowCommand">

        <Columns>
            <asp:BoundField DataField="DisplayName" HeaderText="用戶名稱" SortExpression="DisplayName"></asp:BoundField>
            <asp:BoundField DataField="UserName" HeaderText="帳號" SortExpression="UserName"></asp:BoundField>
            <asp:BoundField DataField="Role" HeaderText="身份" SortExpression="Role"></asp:BoundField>
            <asp:CheckBoxField DataField="IsActive" HeaderText="權限" SortExpression="IsActive"></asp:CheckBoxField>
            <asp:BoundField DataField="LastLoginAt" HeaderText="最後登入時間" SortExpression="LastLoginAt"></asp:BoundField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <%-- 編輯按鈕：使用 LinkButton 看起來更像連結，比較清爽 --%>
                    <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="EditUser" CommandArgument='<%# Eval("Id") %>'>編輯</asp:LinkButton>

                    &nbsp; <%-- 加上一個空格 --%>

                    <asp:Button ID="ButtonDelete" runat="server" Text="刪除" CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('您確定要刪除這個用戶嗎？');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Panel ID="PanelForm" runat="server">
        <asp:Label ID="LabelTitle" runat="server"></asp:Label>
        <asp:Label ID="LabelPanelMessage" runat="server"></asp:Label>
        <br />
        <asp:HiddenField ID="HiddenFieldUserId" runat="server" />
        <table>
            <tr>
                <td>帳號:</td>
                <td>
                    <asp:TextBox ID="TextBoxUserName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>密碼:</td>
                <td>
                    <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <br />
                    <small>新增時為必填，編輯時留空表示不變更密碼</small>
                </td>
            </tr>
             <tr>
                <td>確認密碼：</td>
                <td>
                    <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>顯示名稱：</td>
                <td>
                    <asp:TextBox ID="TextBoxDisplayName" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Email：</td>
                <td>
                    <asp:TextBox ID="TextBoxEmail" runat="server" TextMode="Email" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>角色：</td>
                <td>
                    <asp:DropDownList ID="DropDownListRole" runat="server">
                        <asp:ListItem Value="Admin">一般管理員</asp:ListItem>
                        <asp:ListItem Value="SuperAdmin">超級管理員</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>啟用狀態：</td>
                <td>
                    <asp:CheckBox ID="CheckBoxIsActive" runat="server" Checked="true" Text="已啟用" />
                </td>
            </tr>
             <tr>
                <td colspan="2">
                     <asp:Button ID="ButtonSave" runat="server" Text="新增" OnClick="ButtonSave_Click" />
                     &nbsp;
                     <asp:Button ID="ButtonCancel" runat="server" Text="清除" OnClick="ButtonCancel_Click" />
                </td>
            </tr>
            <tr id="infoRow" runat="server" visible="false">
                <td colspan="2">
                    <hr/>
                    <small>創建時間：<asp:Literal ID="LiteralCreatedAt" runat="server"></asp:Literal></small><br/>
                    <small>最後登入：<asp:Literal ID="LiteralLastLoginAt" runat="server"></asp:Literal></small>
                </td>
            </tr>
        </table>



    </asp:Panel>

</asp:Content>
