<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DealerEdit.aspx.cs" Inherits="TayanaYacht.Admin.DealerEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        // DOM 載入後執行
        document.addEventListener("DOMContentLoaded", function () {
            // 監聽 custom file input 的變動
            var fileInput = document.querySelector('.custom-file-input');
            if (fileInput) {
                fileInput.addEventListener('change', function (e) {
                    var fileName = e.target.files[0].name;
                    var nextSibling = e.target.nextElementSibling;
                    nextSibling.innerText = fileName;
                });
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h4 class="m-0 font-weight-bold text-primary">
                <asp:Label ID="lblCardTitle" runat="server" Text="經銷商資料"></asp:Label>
            </h4>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label>國家</label>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="custom-select"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>地區</label>
                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="custom-select"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <hr />

            <div class="form-group">
                <label for="<%= txtName.ClientID %>">*經銷商名稱</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                <asp:Label ID="LabelName" runat="server" Text="" Visible="false" CssClass="text-danger small mt-1"></asp:Label>
            </div>
            <div class="form-group">
                <label for="<%= txtContact.ClientID %>">聯絡人</label>
                <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="<%= txtAddress.ClientID %>">地址</label>
                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
            </div>

            <%-- 【版面修改】將電話/傳真/手機改為三欄式佈局，提高資訊密度 --%>
            <div class="row">
                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="<%= txtTel.ClientID %>">電話</label>
                        <asp:TextBox ID="txtTel" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="<%= txtFax.ClientID %>">傳真</label>
                        <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="form-group">
                        <label for="<%= txtCell.ClientID %>">手機</label>
                        <asp:TextBox ID="txtCell" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>

            <%-- Email 和網站維持兩欄式佈局 --%>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="<%= txtEmail.ClientID %>">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="<%= txtWebsite.ClientID %>">網站</label>
                        <asp:TextBox ID="txtWebsite" runat="server" TextMode="Url" placeholder="https://..." CssClass="form-control" />
                    </div>
                </div>
            </div>

            <hr />

            <div class="form-group">
                <label>上傳/更換圖片</label>
                <asp:Image ID="ImageView2" runat="server" CssClass="img-thumbnail d-block mb-2" Width="250" />
                <div class="custom-file">
                    <asp:FileUpload ID="FileUploadImage" runat="server" CssClass="custom-file-input" />
                    <label class="custom-file-label" for="<%= FileUploadImage.ClientID %>">選擇檔案...</label>
                </div>
                <asp:Label ID="LabelImage" runat="server" Text="" CssClass="text-danger small mt-1"></asp:Label>
            </div>
            <div class="form-group">
                <div class="custom-control custom-checkbox">
                    <%-- 【已修正】改用標準 HTML input 標籤，確保 class 加在正確的位置 --%>
                    <input type="checkbox" id="chkIsActive" class="custom-control-input" runat="server" />
                    <label class="custom-control-label" for="ContentPlaceHolder1_chkIsActive">在前台顯示此經銷商</label>
                </div>
            </div>
        </div>
        <div class="card-footer text-right">
            <asp:LinkButton ID="ButtonSave" runat="server" Text="<i class='fas fa-save'></i> 儲存" OnClick="ButtonSave_Click" CssClass="btn btn-primary" />
            <asp:HyperLink ID="ButtonCancel" runat="server" NavigateUrl="~/Admin/DealerList.aspx" CssClass="btn btn-secondary">返回列表</asp:HyperLink>
        </div>
    </div>

    <%-- 暫時的除錯用 Label --%>
    <asp:Label ID="debugLabel" runat="server" ForeColor="Red" Font-Size="Large" />
</asp:Content>
