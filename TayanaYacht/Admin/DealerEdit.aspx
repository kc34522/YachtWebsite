<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DealerEdit.aspx.cs" Inherits="TayanaYacht.Admin.DealerEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HyperLink ID="HyperLinkLastPage" runat="server" NavigateUrl="~/Admin/DealerList.aspx">回經銷商列表</asp:HyperLink>
    <!-- 檢視模式 -->
    <asp:Panel ID="PanelViewMode" runat="server">
        <h3>經銷商資料檢視</h3>
        <table>
            <tr>
                <td><strong>國家：</strong></td>
                <td>
                    <asp:Label ID="lblCountry" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>地區：</strong></td>
                <td>
                    <asp:Label ID="lblRegion" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>經銷商名稱：</strong></td>
                <td>
                    <asp:Label ID="lblName" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>聯絡人：</strong></td>
                <td>
                    <asp:Label ID="lblContact" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>地址：</strong></td>
                <td>
                    <asp:Label ID="lblAddress" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>電話：</strong></td>
                <td>
                    <asp:Label ID="lblTel" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>傳真：</strong></td>
                <td>
                    <asp:Label ID="lblFax" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>手機：</strong></td>
                <td>
                    <asp:Label ID="lblCell" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>Email：</strong></td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>網站：</strong></td>
                <td>
                    <asp:Label ID="lblWebsite" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>圖片路徑：</strong></td>
                <td>
                    <asp:Image ID="ImageView" runat="server" />
                </td>
            </tr>

            <tr>
                <td><strong>顯示前台：</strong></td>
                <td>
                    <asp:CheckBox ID="CheckBoxIsActive" runat="server" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td><strong>建立日期：</strong></td>
                <td>
                    <asp:Label ID="lblCreatedDate" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>建立者：</strong></td>
                <td>
                    <asp:Label ID="lblCreatedBy" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>更新時間：</strong></td>
                <td>
                    <asp:Label ID="lblUpdatedAt" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>更新者：</strong></td>
                <td>
                    <asp:Label ID="lblUpdatedBy" runat="server" /></td>
            </tr>
        </table>
        <asp:Button ID="ButtonEdit" runat="server" Text="編輯" OnClick="ButtonEdit_Click" />
    </asp:Panel>


    <!-- 編輯模式 -->
    <asp:Label ID="LabelEditMode" runat="server" Text="" Visible="false"></asp:Label>
    <asp:Panel ID="PanelEditMode" runat="server">
        <h3>
            <asp:Label ID="LabelTitle" runat="server" Text=""></asp:Label></h3>
        <table>
            <tr>
                <td><strong>國家：</strong></td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
                    <%--待做功能--%>
                    <%--<asp:Button ID="ButtonAddCountry" runat="server" Text="新增國家" />--%>
            </tr>
            <tr>
                <td><strong>地區：</strong></td>
                <td>
                    <asp:DropDownList ID="ddlRegion" runat="server"></asp:DropDownList>
                    <%--待做功能--%>
                    <%--<asp:Button ID="ButtonAddRegion" runat="server" Text="新增地區" />--%>
            </tr>
            <tr>
                <td><strong>*名稱：</strong></td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" /></td>
                <td>
                    <asp:Label ID="LabelName" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td><strong>聯絡人：</strong></td>
                <td>
                    <asp:TextBox ID="txtContact" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>地址：</strong></td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" /></td>
            </tr>
            <tr>
                <td><strong>電話：</strong></td>
                <td>
                    <asp:TextBox ID="txtTel" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>傳真：</strong></td>
                <td>
                    <asp:TextBox ID="txtFax" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>手機：</strong></td>
                <td>
                    <asp:TextBox ID="txtCell" runat="server" /></td>
            </tr>
            <tr>
                <td><strong>Email：</strong></td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" /></td>
            </tr>
            <tr>
                <td><strong>網站：</strong></td>
                <td>
                    <asp:TextBox ID="txtWebsite" runat="server" TextMode="Url" /></td>
            </tr>
            <tr>
                <td><strong>圖片路徑：</strong></td>
                <td>
                    <asp:Image ID="ImageView2" runat="server" />
                    <asp:FileUpload ID="FileUploadImage" runat="server" />
                    <asp:Label ID="LabelImage" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td><strong>顯示前台：</strong></td>
                <td>
                    <asp:CheckBox ID="chkIsActive" runat="server" /></td>
            </tr>
        </table>
        <asp:Button ID="ButtonSave" runat="server" Text="儲存" OnClick="ButtonSave_Click" />
        <asp:Button ID="ButtonCancel" runat="server" Text="取消" OnClick="ButtonCancel_Click" />
    </asp:Panel>

</asp:Content>
