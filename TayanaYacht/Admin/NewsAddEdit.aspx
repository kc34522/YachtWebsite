<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewsAddEdit.aspx.cs" Inherits="TayanaYacht.Admin.NewsAddEdit" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- 【JS腳本】加入這段腳本以處理自訂檔案上傳的檔名顯示 --%>
    <script>
        // 新版本 JS，使用標準的 DOMContentLoaded 事件
        document.addEventListener("DOMContentLoaded", function () {
            // 尋找頁面上「所有」的 custom-file-input 元件
            var fileInputs = document.querySelectorAll('.custom-file-input');

            // 為每一個找到的元件都加上事件監聽
            fileInputs.forEach(function (input) {
                input.addEventListener('change', function (e) {
                    var files = e.target.files;
                    var label = input.nextElementSibling;
                    var labelText = '選擇檔案...'; // 預設文字

                    if (files.length > 1) {
                        // 如果選擇了多個檔案，顯示 "已選擇 X 個檔案"
                        labelText = '已選擇 ' + files.length + ' 個檔案';
                    } else if (files.length === 1) {
                        // 如果只選擇一個，顯示檔名
                        labelText = files[0].name;
                    }

                    if (label) {
                        label.innerText = labelText;
                    }
                });
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h4 class="m-0 font-weight-bold text-primary">
                <asp:Label ID="LabelContentTitle" runat="server"></asp:Label>
            </h4>
        </div>
        <div class="card-body">
            <div class="form-group">
                <label for="<%= txtTitle.ClientID %>">*標題</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="<%= txtPublishDate.ClientID %>">發佈日期</label>
                        <asp:TextBox ID="txtPublishDate" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-md-6 d-flex align-items-center">
                    <div class="form-group mb-0">
                        <div class="custom-control custom-checkbox mr-3">
                            <input type="checkbox" id="CheckBoxIsTopEdit" class="custom-control-input" runat="server" />
                            <label class="custom-control-label" for="ContentPlaceHolder1_CheckBoxIsTopEdit">是否置頂</label>
                        </div>
                        <div class="custom-control custom-checkbox">
                            <input type="checkbox" id="CheckBoxIsVisibleEdit" class="custom-control-input" runat="server" checked="checked" />
                            <label class="custom-control-label" for="ContentPlaceHolder1_CheckBoxIsVisibleEdit">是否顯示</label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group mt-3">
                <label>內文</label>
                <CKEditor:CKEditorControl ID="CKEditorControlContent" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
            </div>
            <hr />
            <div class="small text-muted">
                建立日期:
                <asp:Label ID="LabelCreatedDateAdd" runat="server"></asp:Label>
                by
                <asp:Label ID="LabelCreatedByAdd" runat="server"></asp:Label>
                <br />
                最後修改:
                <asp:Label ID="LabelModifiedDateAdd" runat="server"></asp:Label>
                by
                <asp:Label ID="LabelModifiedByAdd" runat="server"></asp:Label>
            </div>
        </div>
        <div class="card-footer text-right">
            <asp:Label ID="LabelSaveMessage" runat="server" CssClass="text-success mr-3"></asp:Label>
            <asp:LinkButton ID="ButtonSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-primary">
                <i class='fas fa-save'></i> 儲存
            </asp:LinkButton>
            <asp:HyperLink ID="HyperLinkBack" runat="server" NavigateUrl="~/Admin/NewsList.aspx" CssClass="btn btn-secondary">返回列表</asp:HyperLink>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-6">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">圖片管理</h6>
                </div>
                <div class="card-body">
                    <asp:Label ID="LabelImageGridView" runat="server"></asp:Label>
                    <div class="table-responsive">
                        <%-- 【樣式修正】加入 table-bordered 並補上 thead-dark --%>
                        <asp:GridView ID="GridViewImage" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="GridViewImage_RowEditing" OnRowCancelingEdit="GridViewImage_RowCancelingEdit" OnRowUpdating="GridViewImage_RowUpdating" OnRowDeleting="GridViewImage_RowDeleting" CssClass="table table-sm table-bordered" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:TemplateField HeaderText="圖片">
                                    <%-- 【已修改】將表頭置中 --%>
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <%-- 【已修改】限制圖片最大寬度為 80px --%>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImagePath") %>' AlternateText='<%# Eval("AltText") %>' CssClass="img-thumbnail" Style="max-width: 80px; height: auto;" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="說明文字(Alt)">
                                    <HeaderStyle CssClass="text-center" />
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Bind("AltText") %>' ID="TextBoxAlt" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("AltText") %>' ID="Label1"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="align-middle" />

                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="封面">
                                    <HeaderStyle CssClass="text-center" />
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# Bind("IsCover") %>' ID="CheckBoxIsCover"></asp:CheckBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" Checked='<%# Bind("IsCover") %>' Enabled="false" ID="CheckBox1"></asp:CheckBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="功能">
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <asp:LinkButton ID="btnEdit" CommandName="Edit" runat="server" CssClass="btn btn-sm btn-outline-primary mr-1" ToolTip="編輯"><i class="fa fa-pencil-alt"></i></asp:LinkButton><asp:LinkButton ID="btnDel" CommandName="Delete" runat="server" CssClass="btn btn-sm btn-danger" ToolTip="刪除" OnClientClick="return confirm('您確定要刪除此圖片嗎？');"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <asp:LinkButton ID="btnUpdate" CommandName="Update" runat="server" CssClass="btn btn-sm btn-success mr-1" ToolTip="更新"><i class="fa fa-save"></i></asp:LinkButton><asp:LinkButton ID="btnCancel" CommandName="Cancel" runat="server" CssClass="btn btn-sm btn-secondary" ToolTip="取消"><i class="fa fa-times"></i></asp:LinkButton>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer">
                    <div class="form-group mb-0">
                        <label>上傳新圖片 (可多選)</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadImage" runat="server" AllowMultiple="true" CssClass="custom-file-input" />
                            <label class="custom-file-label" for="<%= FileUploadImage.ClientID %>">選擇檔案...</label>
                        </div>
                        <asp:Button ID="ButtonAddImage" runat="server" Text="上傳" OnClick="ButtonAddImage_Click" CssClass="btn btn-info btn-block mt-2" />
                        <asp:Label ID="LabelImageMessage" runat="server" CssClass="small text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-6">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">檔案管理</h6>
                </div>
                <div class="card-body">
                    <asp:Label ID="LabelFileGridView" runat="server"></asp:Label>
                    <div class="table-responsive">
                        <%-- 【樣式修正】加入 table-bordered 並補上 thead-dark --%>
                        <asp:GridView ID="GridViewFile" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowEditing="GridViewFile_RowEditing" OnRowCancelingEdit="GridViewFile_RowCancelingEdit" OnRowUpdating="GridViewFile_RowUpdating" OnRowDeleting="GridViewFile_RowDeleting" CssClass="table table-sm table-bordered" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:TemplateField HeaderText="檔案">
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLinkFile" runat="server" Text='<%# Eval("DisplayName") %>' NavigateUrl='<%# Eval("FilePath") %>' Target="_blank" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="align-middle" />

                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Bind("DisplayName") %>' ID="TextBoxName" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="功能">
                                    <HeaderStyle CssClass="text-center" />
                                    <ItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <asp:LinkButton ID="LinkButton1" CommandName="Edit" runat="server" CssClass="btn btn-sm btn-outline-primary mr-1" ToolTip="編輯"><i class="fa fa-pencil-alt"></i></asp:LinkButton><asp:LinkButton ID="LinkButton2" CommandName="Delete" runat="server" CssClass="btn btn-sm btn-danger" ToolTip="刪除" OnClientClick="return confirm('您確定要刪除此檔案嗎？');"><i class="fa fa-trash"></i></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <asp:LinkButton ID="LinkButton3" CommandName="Update" runat="server" CssClass="btn btn-sm btn-success mr-1" ToolTip="更新"><i class="fa fa-save"></i></asp:LinkButton><asp:LinkButton ID="LinkButton4" CommandName="Cancel" runat="server" CssClass="btn btn-sm btn-secondary" ToolTip="取消"><i class="fa fa-times"></i></asp:LinkButton>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer">
                    <div class="form-group mb-0">
                        <label>上傳新檔案 (可多選)</label>
                        <div class="custom-file">
                            <asp:FileUpload ID="FileUploadFile" runat="server" AllowMultiple="true" CssClass="custom-file-input" />
                            <label class="custom-file-label" for="<%= FileUploadFile.ClientID %>">選擇檔案...</label>
                        </div>
                        <asp:Button ID="ButtonAddFile" runat="server" Text="上傳" OnClick="ButtonAddFile_Click" CssClass="btn btn-info btn-block mt-2" />
                        <asp:Label ID="LabelFileMessage" runat="server" CssClass="small text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
