<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="YachtEdit.aspx.cs" Inherits="TayanaYacht.Admin.YachtEdit" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* 為 RadioButtonList 的選項之間增加間距 */
        .radio-list-spacing label {
            margin-right: 1.5rem; /* 調整此數值以改變間距 */
        }

        .image-grid {
            display: grid;
            /* 自動填充，每個項目最小 220px，最大佔滿剩餘空間 */
            grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
            gap: 1rem; /* 項目之間的間距 */
    </style>
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
        // 監聽分頁切換事件，儲存當前分頁的 href 到 HiddenField
        document.querySelectorAll('#yachtTab .nav-link').forEach(function (tab) {
            tab.addEventListener('click', function (e) {
                var activeTabId = e.currentTarget.getAttribute('href'); // 獲取 href，例如 "#dimensions"
                document.getElementById('<%= ActiveTabField.ClientID %>').value = activeTabId; // 儲存到 HiddenField
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="ActiveTabField" runat="server" Value="#main-info" />

    <div class="card shadow mb-4">
        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h4 class="m-0 font-weight-bold text-primary">
                <asp:Label ID="lblCardTitle" runat="server" Text="管理船型資料"></asp:Label>
            </h4>
            <%-- 將「返回列表」按鈕保留在全域的頁首，因為它是一個全域操作 --%>
            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Admin/YachtsList.aspx" CssClass="btn btn-secondary btn-sm">
                <i class="fas fa-arrow-left"></i> 返回列表
            </asp:HyperLink>
        </div>
        <div class="card-body">
            <ul class="nav nav-tabs" id="yachtTab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="mainInfoTab" runat="server" data-toggle="tab" href="#mainInfo" role="tab" aria-controls="mainInfo" aria-selected="true">主要資訊</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="dimensionsTab" runat="server" data-toggle="tab" href="#dimensions" role="tab" aria-controls="dimensions" aria-selected="false">尺寸規格</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="galleryTab" runat="server" data-toggle="tab" href="#gallery" role="tab" aria-controls="gallery" aria-selected="false">圖庫管理</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="downloadsTab" runat="server" data-toggle="tab" href="#downloads" role="tab" aria-controls="downloads" aria-selected="false">檔案下載</a>
                </li>
            </ul>

            <div class="tab-content" id="yachtTabContent">
                <div class="tab-pane fade show active" role="tabpanel" id="mainInfo" runat="server" clientidmode="Static">
                    <div class="p-3 border border-top-0">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="<%=txtModelText.ClientID %>">*型號系列 (例: Tayana)</label><asp:TextBox ID="txtModelText" runat="server" CssClass="form-control" /><asp:Label ID="LabelModelText" runat="server" CssClass="text-danger small mt-1"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="<%=txtModelNumber.ClientID%>">*型號數字 (例: 48)</label><asp:TextBox ID="txtModelNumber" runat="server" CssClass="form-control" /><asp:Label ID="LabelModelNumber" runat="server" CssClass="text-danger small mt-1"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <%-- 第一個表單項目：是否顯示 --%>
                        <div class="form-group">
                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" id="CheckBoxIsActive" class="custom-control-input" runat="server" checked="checked" />
                                <label class="custom-control-label" for="ContentPlaceHolder1_CheckBoxIsActive">是否顯示於前台</label>
                            </div>
                        </div>

                        <%-- 第二個表單項目：類型選擇 --%>
                        <div class="form-group">
                            <label class="d-block">類型：</label>
                            <%-- 為 RadioButtonList 加上一個主標籤 --%>
                            <%-- 【已修改】為 RadioButtonList 設定最簡潔的輸出屬性 --%>
                            <asp:RadioButtonList ID="rblBuildDesign" runat="server"
                                RepeatDirection="Horizontal"
                                RepeatLayout="Flow"
                                CssClass="radio-list-spacing">
                                <asp:ListItem Value="None" Selected="True">無</asp:ListItem>
                                <asp:ListItem Value="NewBuilding">全新建造</asp:ListItem>
                                <asp:ListItem Value="NewDesign">全新設計</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <hr />
                        <div class="form-group">
                            <label>OVERVIEW 內容</label><CKEditor:CKEditorControl ID="CKEditorOverviewContent" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
                        </div>
                        <div class="form-group">
                            <label>SPECIFICATION 內容</label><CKEditor:CKEditorControl ID="CKEditorSpecification" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
                        </div>
                    </div>
                    <div id="mainInfoFooter" class="card-footer text-right">
                        <asp:Label ID="LabelInfoMessage" runat="server" CssClass="text-success mr-3"></asp:Label>
                        <asp:Button ID="ButtonSave" runat="server" Text="儲存主要資訊" OnClick="ButtonSave_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="ButtonCancel" runat="server" Text="取消變更" OnClick="ButtonCancel_Click" CausesValidation="false" CssClass="btn btn-warning" />
                    </div>
                </div>

                <div class="tab-pane fade" role="tabpanel" id="dimensions" runat="server" clientidmode="Static">
                    <div class="p-3 border border-top-0">
                        <div class="row">
                            <div class="col-md-8">
                                <h5>尺寸內容規格</h5>
                                <asp:Label ID="LabelGridViewDimMessage" runat="server" CssClass="text-info d-block mb-2"></asp:Label>
                                <div class="table-responsive">
                                    <asp:GridView ID="GridViewDimension" runat="server" AutoGenerateColumns="false" ShowFooter="true" DataKeyNames="SpecID" OnRowCommand="GridViewDimension_RowCommand" OnRowEditing="GridViewDimension_RowEditing" OnRowCancelingEdit="GridViewDimension_RowCancelingEdit" OnRowUpdating="GridViewDimension_RowUpdating" OnRowDeleting="GridViewDimension_RowDeleting" CssClass="table table-bordered" Width="100%">
                                        <HeaderStyle CssClass="thead-dark" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="規格名稱">
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" Text='<%# Bind("SpecLabel") %>' ID="TextBoxSpecLabelEdit" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Bind("SpecLabel") %>' ID="Label1"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TextBoxSpecLabel" runat="server" placeholder="新規格名稱" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="規格內容">
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" Text='<%# Bind("SpecValue") %>' ID="TextBoxSpecValueEdit" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Bind("SpecValue") %>' ID="Label2"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TextBoxSpecValue" runat="server" placeholder="新規格內容" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="操作">
                                                <FooterStyle CssClass="text-nowrap text-center" />
                                                <HeaderStyle CssClass="text-nowrap text-center" />
                                                <ItemTemplate>
                                                    <div style="white-space: nowrap;">
                                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary mr-1" ToolTip="編輯"><i class="fa fa-pencil-alt"></i></asp:LinkButton><asp:LinkButton ID="LinkButtonDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="刪除" OnClientClick="return confirm('您確定要刪除此規格嗎？');"><i class="fa fa-trash"></i></asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="white-space: nowrap;">
                                                        <asp:LinkButton ID="LinkButtonSave" runat="server" CommandName="Update" CssClass="btn btn-sm btn-success mr-1" ToolTip="更新"><i class="fa fa-save"></i></asp:LinkButton><asp:LinkButton ID="LinkButtonCancel" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-secondary" ToolTip="取消" CausesValidation="false"><i class="fa fa-times"></i></asp:LinkButton>
                                                    </div>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAdd" runat="server" Text="新增" CommandName="AddSpec" CssClass="btn btn-success btn-sm" />
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <h5>尺寸圖</h5>
                                <asp:Label ID="LabelDimImageMessage" runat="server" CssClass="text-info d-block mb-2"></asp:Label>
                                <%-- 檢視現有圖片 --%>
                                <asp:Panel ID="PanelViewDimensionImage" runat="server" Visible="false">
                                    <asp:Image ID="ImageDimensionImage" runat="server" CssClass="img-thumbnail mb-2" />
                                    <asp:LinkButton ID="LinkButtonDeleteDimensionImage" runat="server" OnClick="LinkButtonDeleteDimensionImage_Click" CssClass="btn btn-danger btn-sm btn-block" OnClientClick="return confirm('您確定要刪除尺寸圖嗎？');"><i class="fa fa-trash"></i> 刪除圖片</asp:LinkButton>
                                </asp:Panel>
                                <%-- 上傳新圖片 --%>
                                <asp:Panel ID="PanelUploadDimensionImage" runat="server" Visible="false">
                                    <div class="form-group mt-2">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUploadDimensionImage" runat="server" CssClass="custom-file-input" /><label class="custom-file-label" for="<%=FileUploadDimensionImage.ClientID %>">選擇檔案...</label>
                                        </div>
                                        <asp:Button ID="ButtonDimensionUpload" runat="server" Text="上傳" OnClick="ButtonDimensionUpload_Click" CssClass="btn btn-info btn-block mt-2" />
                                        <asp:Label ID="LabelDimUploadMessage" runat="server" CssClass="small text-danger"></asp:Label>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="tab-pane fade" role="tabpanel" id="gallery" runat="server" clientidmode="Static">
                    <div class="p-3 border border-top-0">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="card shadow-sm mb-3">
                                    <div class="card-header">
                                        <h6 class="m-0 font-weight-bold">Layout & Deck 圖片</h6>
                                    </div>
                                    <div class="card-body">
                                        <asp:Label ID="LabelGridViewLayout" runat="server"></asp:Label>
                                        <asp:GridView ID="GridViewLayout" runat="server" AutoGenerateColumns="False" DataKeyNames="ImageID" OnRowDeleting="GridViewLayout_RowDeleting" CssClass="table table-bordered table-sm" Width="100%">
                                            <HeaderStyle CssClass="thead-dark" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="圖片">
                                                    <HeaderStyle CssClass="text-nowrap text-center" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="ImageLayout" runat="server" ImageUrl='<%# Bind("ImagePath") %>' CssClass="img-thumbnail" Style="max-width: 80px; height: auto;" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CreateDate" HeaderText="建立時間" DataFormatString="{0:yyyy-MM-dd}" HeaderStyle-CssClass="text-nowrap text-center" ItemStyle-CssClass="text-nowrap text-center align-middle" />
                                                <asp:TemplateField HeaderText="刪除">
                                                    <HeaderStyle CssClass="text-nowrap text-center" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('確定刪除?');"><i class="fa fa-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="card-footer">
                                        <label>上傳新圖片 (可多選)</label><div class="custom-file">
                                            <asp:FileUpload ID="FileUploadLayout" runat="server" AllowMultiple="true" CssClass="custom-file-input" /><label class="custom-file-label" for="<%= FileUploadLayout.ClientID %>">選擇檔案...</label>
                                        </div>
                                        <asp:Button ID="ButtonLayoutUpload" runat="server" Text="上傳" OnClick="ButtonLayoutUpload_Click" CssClass="btn btn-info btn-block mt-2" /><asp:Label ID="LabelLayoutMessage" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card shadow-sm mb-3">
                                    <div class="card-header">
                                        <h6 class="m-0 font-weight-bold">輪播圖</h6>
                                    </div>
                                    <div class="card-body">
                                        <asp:Label ID="LabelGridViewCarousel" runat="server" CssClass="d-block mb-3"></asp:Label>
                                        <div class="image-grid">

                                            <asp:Repeater ID="RepeaterCarouselImages" runat="server" OnItemCommand="RepeaterCarouselImages_ItemCommand">
                                                <ItemTemplate>
                                                    <div class="text-center border p-2 rounded">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImagePath") %>' Width="200px" CssClass="img-thumbnail" /><br />
                                                        <div class="mt-2">
                                                            <div class="custom-control custom-radio custom-control-inline">
                                                                <input type="radio"
                                                                    name="CarouselGroup"
                                                                    class="custom-control-input"
                                                                    id='radio_<%# Eval("ImageID") %>'
                                                                    value='<%# Eval("ImageID") %>'
                                                                    <%# Convert.ToBoolean(Eval("IsHomepageCarousel")) ? "checked" : "" %>
                                                                    onchange="__doPostBack('SetHomepageCarousel', '<%# Eval("ImageID") %>')" />
                                                                <label class="custom-control-label" for='radio_<%# Eval("ImageID") %>'>設為首頁輪播圖</label>
                                                            </div>


                                                            <asp:HiddenField ID="HiddenFieldImageID" runat="server" Value='<%# Eval("ImageID") %>' />
                                                            <asp:LinkButton ID="LinkButtonDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ImageID") %>' OnClientClick="return confirm('確定要刪除嗎?');" CssClass="btn btn-link btn-sm text-danger"><i class="fa fa-trash"></i> 刪除</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <label>上傳新輪播圖 (可多選)</label><div class="custom-file">
                                            <asp:FileUpload ID="FileUploadCarouselImages" runat="server" AllowMultiple="true" CssClass="custom-file-input" /><label class="custom-file-label" for="<%= FileUploadCarouselImages.ClientID %>">選擇檔案...</label>
                                        </div>
                                        <asp:Button ID="ButtonCarouselImages" runat="server" Text="上傳" OnClick="ButtonCarouselImages_Click" CssClass="btn btn-info btn-block mt-2" /><asp:Label ID="LabelCarouselMessage" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="tab-pane fade" role="tabpanel" id="downloads" runat="server" clientidmode="Static">
                    <div class="p-3 border border-top-0">
                        <div class="row">
                            <div class="col-md-8">
                                <h5>已上傳的檔案</h5>
                                <asp:Label ID="LabelDownloadGridViewMessage" runat="server"></asp:Label>
                                <asp:GridView ID="GridViewFile" runat="server" AutoGenerateColumns="False" DataKeyNames="DownloadID" OnRowEditing="GridViewFile_RowEditing" OnRowCancelingEdit="GridViewFile_RowCancelingEdit" OnRowUpdating="GridViewFile_RowUpdating" OnRowDeleting="GridViewFile_RowDeleting" CssClass="table table-bordered table-sm" Width="100%">
                                    <HeaderStyle CssClass="thead-dark" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="檔案名稱">
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" Text='<%# Bind("DisplayName") %>' ID="TextBoxDisplayName" CssClass="form-control form-control-sm"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Bind("FilePath") %>' Text='<%# Bind("DisplayName") %>' Target="_blank" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="功能">
                                            <HeaderStyle CssClass="text-nowrap text-center" />
                                            <ItemTemplate>
                                                <div style="white-space: nowrap;">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary mr-1" ToolTip="編輯"><i class="fa fa-pencil-alt"></i></asp:LinkButton><asp:LinkButton ID="LinkButton4" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('確定刪除?');" ToolTip="刪除"><i class="fa fa-trash"></i></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div style="white-space: nowrap;">
                                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandName="Update" CssClass="btn btn-sm btn-success mr-1" ToolTip="更新"><i class="fa fa-save"></i></asp:LinkButton><asp:LinkButton ID="LinkButton6" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-secondary" ToolTip="取消" CausesValidation="false"><i class="fa fa-times"></i></asp:LinkButton>
                                                </div>
                                            </EditItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="col-md-4">
                                <h5>上傳新檔案</h5>
                                <div class="form-group">
                                    <label>檔案上傳 (可多選, 限 PDF)</label><div class="custom-file">
                                        <asp:FileUpload ID="FileUploadDownload" runat="server" AllowMultiple="true" CssClass="custom-file-input" /><label class="custom-file-label" for="<%= FileUploadDownload.ClientID %>">選擇檔案...</label>
                                    </div>
                                    <asp:Button ID="ButtonFileUpload" runat="server" Text="上傳" OnClick="ButtonFileUpload_Click" CssClass="btn btn-info btn-block mt-2" /><asp:Label ID="LabelDownloadMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
