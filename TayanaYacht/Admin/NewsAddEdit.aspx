<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewsAddEdit.aspx.cs" Inherits="TayanaYacht.Admin.NewsAddEdit" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HyperLink ID="HyperLinkBack" runat="server" NavigateUrl="~/Admin/NewsList.aspx">回列表</asp:HyperLink>
    <%--新增/編輯區塊--%>
    <h2>
        <asp:Label ID="LabelContentTitle" runat="server" Text="管理新聞"></asp:Label></h2>
    <%--檢視模式--%>
    <asp:Panel ID="PanelViewMode" runat="server" Visible="false">
        <asp:Label Text="檢視新聞資訊" runat="server" />
        <table>
            <tr>
                <td>*標題：</td>
                <td>
                    <asp:Label ID="LabelTitle" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>是否置頂：</td>
                <td>
                    <asp:CheckBox ID="chkIsTop" runat="server" Enabled="false" /></td>
            </tr>
            <tr>
                <td>是否顯示：</td>
                <td>
                    <asp:CheckBox ID="chkIsVisible" runat="server" Checked="true" Enabled="false" /></td>
            </tr>
            <tr>
                <td>發佈日期：</td>
                <td>
                    <asp:Label ID="LabelPublishDate" runat="server"></asp:Label>
            </tr>
            <tr>
                <td>內文：</td>
                <td>
                    <asp:Literal ID="LiteralContent" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>建立日期：</td>
                <td>
                    <asp:Label ID="LabelCreatedDate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>建立者：</td>
                <td>
                    <asp:Label ID="LabelCreatedBy" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>最後修改日期：</td>
                <td>
                    <asp:Label ID="LabelModifiedDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>最後修改者：</td>
                <td>
                    <asp:Label ID="LabelModifiedBy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="ButtonEdit" runat="server" Text="編輯" OnClick="ButtonEdit_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%--編輯模式--%>
    <asp:Panel ID="PanelEditMode" runat="server" Visible="false">
        <asp:Label ID="LabelEditMode" runat="server" />
        <table>
            <tr>
                <td>*標題：</td>
                <td>
                    <asp:TextBox ID="txtTitle" runat="server" Width="400" /></td>
            </tr>
            <tr>
                <td>是否置頂：</td>
                <td>
                    <asp:CheckBox ID="CheckBoxIsTopEdit" runat="server" /></td>
            </tr>
            <tr>
                <td>是否顯示：</td>
                <td>
                    <asp:CheckBox ID="CheckBoxIsVisibleEdit" runat="server" Checked="true" /></td>
            </tr>
            <tr>
                <td>發佈日期：</td>
                <td>
                    <asp:TextBox ID="txtPublishDate" runat="server" TextMode="Date" /></td>
            </tr>
            <tr>
                <td>內文：</td>
                <td>
                    <CKEditor:CKEditorControl ID="CKEditorControlContent" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
                </td>
            </tr>
            <tr>
                <td>建立日期：</td>
                <td>
                    <asp:Label ID="LabelCreatedDateAdd" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>建立者：</td>
                <td>
                    <asp:Label ID="LabelCreatedByAdd" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>最後修改日期：</td>
                <td>
                    <asp:Label ID="LabelModifiedDateAdd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>最後修改者：</td>
                <td>
                    <asp:Label ID="LabelModifiedByAdd" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="ButtonSave" runat="server" Text="儲存" OnClick="btnSave_Click" />
                    <asp:Button ID="ButtonCancel" runat="server" Text="取消" OnClick="ButtonCancel_Click" />
                    <asp:Label ID="LabelSaveMessage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%--優化: 圖片上傳預覽 alt iscover 設置--%>
    <%--圖片編輯上傳區--%>
    <asp:Panel ID="PanelImage" runat="server">
        <h3>圖片列表</h3>
        <asp:Label ID="LabelImageGridView" runat="server" Text="Label"></asp:Label>

        <asp:GridView ID="GridViewImage" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="GridViewImage_RowEditing" OnRowCancelingEdit="GridViewImage_RowCancelingEdit" OnRowUpdating="GridViewImage_RowUpdating" OnRowDeleting="GridViewImage_RowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="序號">
                    <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="圖片">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImagePath") %>' AlternateText='<%# Eval("AltText") %>' Height="100px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="說明文字(Alt)" SortExpression="AltText">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Text='<%# Bind("AltText") %>' ID="TextBoxAlt"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("AltText") %>' ID="Label1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="是否為封面" SortExpression="IsCover">
                    <EditItemTemplate>
                        <asp:CheckBox runat="server" Checked='<%# Bind("IsCover") %>' ID="CheckBoxIsCover"></asp:CheckBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" Checked='<%# Bind("IsCover") %>' Enabled="false" ID="CheckBox1"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="True"></asp:CommandField>
                <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
            </Columns>
        </asp:GridView>
        <h3>圖片上傳區</h3>
        <asp:FileUpload ID="FileUploadImage" runat="server" AllowMultiple="true" />
        <asp:Button ID="ButtonAddImage" runat="server" Text="上傳" OnClick="ButtonAddImage_Click" />
        <asp:Label ID="LabelImageMessage" runat="server"></asp:Label>
    </asp:Panel>


    <%--檔案編輯上傳區--%>
    <asp:Panel ID="PanelFile" runat="server">
        <h3>檔案列表</h3>
        <asp:Label ID="LabelFileGridView" runat="server"></asp:Label>
        <asp:GridView ID="GridViewFile" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="GridViewFile_RowEditing" OnRowCancelingEdit="GridViewFile_RowCancelingEdit" OnRowUpdating="GridViewFile_RowUpdating" OnRowDeleting="GridViewFile_RowDeleting">
            <Columns>
                <asp:TemplateField HeaderText="序號">
                    <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="檔案">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLinkFile" runat="server" Text='<%# Eval("DisplayName") %>' NavigateUrl='<%#Eval("FilePath") %>' Target="_blank" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Text='<%# Bind("DisplayName") %>' ID="TextBoxName"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
               
                <asp:CommandField ShowEditButton="True"></asp:CommandField>
                <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
            </Columns>
        </asp:GridView>
        <h3>檔案上傳區</h3>
        <asp:FileUpload ID="FileUploadFile" runat="server" AllowMultiple="true" />
        <asp:Button ID="ButtonAddFile" runat="server" Text="上傳" OnClick="ButtonAddFile_Click" />
        <asp:Label ID="LabelFileMessage" runat="server"></asp:Label>
    </asp:Panel>

</asp:Content>
