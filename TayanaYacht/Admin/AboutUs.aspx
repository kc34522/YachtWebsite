<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="TayanaYacht.Admin.AboutUs" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
    <%-- 標題已移至下方的 Card Header 中，此處留空 --%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h4 class="m-0 font-weight-bold text-primary">About Us 頁面編輯</h4>
        </div>
        <div class="card-body">
            
            <%-- 使用 Bootstrap Alert 來顯示儲存結果訊息 --%>
            <div id="MessageContainer" runat="server" class="alert alert-success" role="alert" visible="false">
                <asp:Literal ID="LiteralMessage" runat="server"></asp:Literal>
            </div>

            <%-- CKEditor 編輯器 --%>
            <div class="form-group">
                <CKEditor:CKEditorControl ID="CKEditorControl1" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
            </div>

            <%-- 最後更新資訊 --%>
            <div class="form-group text-right text-muted">
                 <asp:Label ID="LabelLastUpdatedInfo" runat="server"></asp:Label>
            </div>

        </div>
        <div class="card-footer text-right">
            <%-- 將儲存按鈕改為 LinkButton 以支援圖示 --%>
            <asp:LinkButton ID="ButtonSave" runat="server" Text="<i class='fas fa-save'></i> 儲存" OnClick="ButtonSave_Click" CssClass="btn btn-primary"/>
        </div>
    </div>
</asp:Content>