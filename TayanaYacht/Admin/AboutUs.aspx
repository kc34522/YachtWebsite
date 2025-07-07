<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="TayanaYacht.Admin.AboutUs" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<%--優化: 加上最後修改時間和人--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <CKEditor:CKEditorControl ID="CKEditorControl1" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
    <asp:Button ID="ButtonSave" runat="server" Text="儲存" OnClick="ButtonSave_Click"/>
    <asp:Label ID="LabelLastUpdatedInfo" runat="server"></asp:Label>
    <asp:Literal ID="LiteralMessage" runat="server"></asp:Literal>
</asp:Content>
