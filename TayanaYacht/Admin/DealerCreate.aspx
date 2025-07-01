<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DealerCreate.aspx.cs" Inherits="TayanaYacht.Admin.DealerCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <%--新增經銷商--%>
 <div style="max-width: 600px; margin: auto;">
     <h4 class="mb-3">新增經銷商</h4>

     <asp:DropDownList ID="ddlCountry" runat="server" />
     新增國家選項:
     <asp:TextBox ID="TextBoxAddCountry" runat="server"></asp:TextBox>
     <asp:Button ID="ButtonAddCountry" runat="server" Text="新增" />
     <br />
     <asp:DropDownList ID="ddlRegion" runat="server" />
     新增地區選項:
     <asp:TextBox ID="TextBoxAddRegion" runat="server"></asp:TextBox>
     <asp:Button ID="ButtonAddRegion" runat="server" Text="新增" />
     <br />

     經銷商名稱: <asp:TextBox ID="txtName" runat="server" Placeholder="經銷商名稱" CssClass="form-control" />
     <br />
     聯絡人: <asp:TextBox ID="TextBoxContact" runat="server" Placeholder="聯絡人" CssClass="form-control" />
     <br />
     <asp:TextBox ID="txtAddress" runat="server" Placeholder="地址" CssClass="form-control" />
     <br />
     <asp:TextBox ID="txtTel" runat="server" Placeholder="電話" CssClass="form-control" />
     <br />
     <asp:TextBox ID="TextBoxFax" runat="server" Placeholder="傳真" CssClass="form-control" />
     <br />
     <asp:TextBox ID="TextBoxCell" runat="server" Placeholder="手機" CssClass="form-control" />
     <br />
     <asp:TextBox ID="txtEmail" runat="server" Placeholder="Email" CssClass="form-control" />
     <br />
     <asp:TextBox ID="txtWebsite" runat="server" Placeholder="網站" CssClass="form-control" />
     <br />

     <asp:FileUpload ID="fuImage" runat="server" />
     <br />
     <asp:Image ID="imgPreview" runat="server" Width="150" Visible="false" />
     <br />

     <asp:Button ID="btnSave" runat="server" Text="儲存" />
 </div>
</asp:Content>
