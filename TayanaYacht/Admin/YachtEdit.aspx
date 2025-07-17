<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="YachtEdit.aspx.cs" Inherits="TayanaYacht.Admin.YachtEdit" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
    新增/編輯船型： 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="ButtonBack2" runat="server" Text="返回列表" PostBackUrl="~/Admin/YachtsList.aspx" />

    <%--卡片一：核心資訊--%>
    <label for="<%=txtModelText.ClientID %>">型號系列:</label>
    <asp:TextBox ID="txtModelText" runat="server"></asp:TextBox>
    <asp:Label ID="LabelModelText" runat="server"></asp:Label>
    <br />
    <br />

    <%-- 型號數字 --%>
    <label for="<%=txtModelNumber.ClientID%>">型號數字:</label>
    <asp:TextBox ID="txtModelNumber" runat="server"></asp:TextBox>
    <asp:Label ID="LabelModelNumber" runat="server"></asp:Label>
    <br />
    <br />

    <asp:CheckBox ID="CheckBoxIsActive" runat="server" Text="是否顯示於前台" Checked="true"/>
    <br />
    <br />

    <%-- 最新建造 --%>
    <%-- 對於 CheckBox，可以把文字直接寫在 Text 屬性裡，也可以用 Label --%>
    <asp:CheckBox ID="chkIsNewBuilding" runat="server" Text="全新建造(New Building)" />
    <br />
    <br />

    <asp:CheckBox ID="chkIsNewDesign" runat="server" Text="全新設計(New Design)" />
    <br />
    <br />
    <hr />

    <%-- Overview & Dimensions 區塊 --%>
    <label for="<%=CKEditorOverviewContent.ClientID%>">簡介內容:</label>
    <CKEditor:CKEditorControl ID="CKEditorOverviewContent" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
    <br />

    <%-- SPECIFICATION 區塊 --%>
    <label for="<%=CKEditorSpecification.ClientID%>">簡介內容:</label>
    <CKEditor:CKEditorControl ID="CKEditorSpecification" runat="server" BasePath="/Scripts/ckeditor/"></CKEditor:CKEditorControl>
    <br />
    <asp:Button ID="ButtonCancel" runat="server" Text="取消" OnClick="ButtonCancel_Click"/>
    <asp:Button ID="ButtonSave" runat="server" Text="儲存" OnClick="ButtonSave_Click" />
    <asp:Label ID="LabelInfoMessage" runat="server"></asp:Label>

    <%--卡片二：尺寸規格 (Dimension Specs)--%>

    <label>尺寸內容:</label><asp:Label ID="LabelGridViewDimMessage" runat="server" ></asp:Label>
    <asp:GridView ID="GridViewDimension" runat="server" AutoGenerateColumns="false" ShowFooter="true" DataKeyNames="SpecID" OnRowCommand="GridViewDimension_RowCommand" OnRowEditing="GridViewDimension_RowEditing" OnRowCancelingEdit="GridViewDimension_RowCancelingEdit" OnRowUpdating="GridViewDimension_RowUpdating" OnRowDeleting="GridViewDimension_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="序號">
                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                <FooterTemplate></FooterTemplate>
                <%-- 頁腳的序號欄是空的 --%>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="規格名稱" SortExpression="SpecLabel">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("SpecLabel") %>' ID="TextBoxSpecLabelEdit"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("SpecLabel") %>' ID="Label1"></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="TextBoxSpecLabel" runat="server" placeholder="請輸入新規格名稱"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="規格內容" SortExpression="SpecValue">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("SpecValue") %>' ID="TextBoxSpecValueEdit"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("SpecValue") %>' ID="Label2"></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="TextBoxSpecValue" runat="server" placeholder="請輸入新規格內容"></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="Edit">編輯</asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonDelete" runat="server" CommandName="Delete">刪除</asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButtonSave" runat="server" CommandName="Update">更新</asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonCancel" runat="server" CommandName="Cancel">取消</asp:LinkButton>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:Button ID="ButtonAdd" runat="server" Text="新增" CommandName="AddSpec"/>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
        <%--<EmptyDataTemplate>
            <p>目前沒有規格資料，請使用下方頁腳新增資料。</p>
        </EmptyDataTemplate>--%>
    </asp:GridView>
    
    <br />

    <%--卡片三：尺寸圖 (Dimension Image)--%>

    尺寸圖:
    <asp:Label ID="LabelDimImageMessage" runat="server"></asp:Label>
    <asp:Panel ID="PanelViewDimensionImage" runat="server" Visible="false">
        <asp:Image ID="ImageDimensionImage" runat="server" />
        <asp:LinkButton ID="LinkButtonDeleteDimensionImage" runat="server" OnClick="LinkButtonDeleteDimensionImage_Click">刪除圖片</asp:LinkButton>
    </asp:Panel>
    <asp:Panel ID="PanelUploadDimensionImage" runat="server" Visible="false">
        <label for="<%=FileUploadDimensionImage.ClientID%>">尺寸圖上傳(一張):</label>
        <asp:FileUpload ID="FileUploadDimensionImage" runat="server" AllowMultiple="false" />
        <asp:Button ID="ButtonDimensionUpload" runat="server" Text="上傳" OnClick="ButtonDimensionUpload_Click"/>
        <asp:Label ID="LabelDimUploadMessage" runat="server"></asp:Label>
    </asp:Panel>

    <br />

<%--卡片四：檔案下載 (Downloads)--%>

    <label for="<%=FileUploadDownload.ClientID%>">檔案上傳:</label>
    <asp:FileUpload ID="FileUploadDownload" runat="server" AllowMultiple="true" />
    <asp:Button ID="ButtonFileUpload" runat="server" Text="上傳" OnClick="ButtonFileUpload_Click"/>
    <asp:Label ID="LabelDownloadMessage" runat="server"></asp:Label>
    已上傳的檔案:
    <asp:Label ID="LabelDownloadGridViewMessage" runat="server"></asp:Label>
    <asp:GridView ID="GridViewFile" runat="server" AutoGenerateColumns="False" DataKeyNames="DownloadID" OnRowEditing="GridViewFile_RowEditing" OnRowCancelingEdit="GridViewFile_RowCancelingEdit" OnRowUpdating="GridViewFile_RowUpdating" OnRowDeleting="GridViewFile_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="序號">
                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="檔案名稱" SortExpression="DisplayName">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("DisplayName") %>' ID="TextBoxDisplayName"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Bind("FilePath") %>' Text='<%# Bind("DisplayName") %>' Target="_blank" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedByUserID" HeaderText="建立者" SortExpression="CreatedByUserID"></asp:BoundField>
            <asp:BoundField DataField="CreateDate" HeaderText="建立時間" SortExpression="CreateDate"></asp:BoundField>
            <asp:CommandField ShowEditButton="True"></asp:CommandField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>

    <%-- 關鍵：加上這個樣板 --%>
    <%--<EmptyDataTemplate>
        <div style="text-align: center; padding: 20px; border: 1px dashed #ccc; color: #999;">
            --- 目前尚無任何上傳檔案 ---
        </div>
    </EmptyDataTemplate>--%>


    <%--卡片五：Layout & Deck 圖片--%>

    <label for="<%=FileUploadLayout.ClientID%>">Layout & deck 圖片上傳:</label>
    <asp:FileUpload ID="FileUploadLayout" runat="server" AllowMultiple="true" />
    <asp:Button ID="ButtonLayoutUpload" runat="server" Text="上傳" OnClick="ButtonLayoutUpload_Click"/>
    <asp:Label ID="LabelLayoutMessage" runat="server"></asp:Label>
    <br />
    <asp:Label ID="LabelGridViewLayout" runat="server"></asp:Label>
    <asp:GridView ID="GridViewLayout" runat="server" AutoGenerateColumns="False" DataKeyNames="ImageID" OnRowDeleting="GridViewLayout_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="圖片" SortExpression="ImagePath">
                <ItemTemplate>
                    <asp:Image ID="ImageLayout" runat="server" ImageUrl='<%# Bind("ImagePath") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedByUserID" HeaderText="建立者" SortExpression="CreatedByUserID"></asp:BoundField>
            <asp:BoundField DataField="CreateDate" HeaderText="建立時間" SortExpression="CreateDate"></asp:BoundField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>


    <%--卡片六：輪播圖 (Carousel Images)--%>
    <label for="<%=FileUploadCarouselImages.ClientID%>">輪播圖上傳:</label>
    <asp:FileUpload ID="FileUploadCarouselImages" runat="server" AllowMultiple="true" />
    <asp:Button ID="ButtonCarouselImages" runat="server" Text="上傳" OnClick="ButtonCarouselImages_Click"/>
    <asp:Label ID="LabelCarouselMessage" runat="server"></asp:Label>
    <br />
    <asp:Label ID="LabelGridViewCarousel" runat="server" ></asp:Label>
    <asp:Repeater ID="RepeaterCarouselImages" runat="server" OnItemCommand="RepeaterCarouselImages_ItemCommand" >

        <ItemTemplate>
            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ImagePath") %>' Width="200px" /><br />
            <asp:RadioButton ID="RadioButtonSelectHomepage" runat="server" GroupName="CarouselGroup" Text="設為首頁輪播圖" Checked='<%# Convert.ToBoolean(Eval("IsHomepageCarousel")) %>' AutoPostBack="true" OnCheckedChanged="RadioButtonSelectHomepage_CheckedChanged"/>
            <%-- HiddenField 為了讓批次儲存時能找到對應的ID --%>
            <asp:HiddenField ID="HiddenFieldImageID" runat="server" Value='<%# Eval("ImageID") %>' />
            <asp:LinkButton ID="LinkButtonDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ImageID") %>' OnClientClick="return confirm('確定要刪除嗎?');" Text="刪除"/>
        </ItemTemplate>
    </asp:Repeater>   



</asp:Content>
