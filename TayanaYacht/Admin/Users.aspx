<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="TayanaYacht.Admin.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
    <%-- 標題已移至下方的 Card Header 中 --%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-7">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h4 class="m-0 font-weight-bold text-primary">使用者列表</h4>
                </div>
                <div class="card-body">
                    <asp:Label ID="LabelGridView" runat="server" CssClass="text-info d-block mb-2"></asp:Label>
                    <div class="table-responsive">
                        <asp:GridView ID="GridViewUser" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="GridViewUser_RowCommand" CssClass="table table-bordered table-hover" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:BoundField DataField="DisplayName" HeaderText="用戶名稱">
                                    <%-- 【表頭修正】加入 text-nowrap --%>
                                    <HeaderStyle CssClass="text-nowrap" />
                                    <ItemStyle CssClass="align-middle text-nowrap" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="帳號">
                                    <HeaderStyle CssClass="text-nowrap" />
                                    <ItemStyle CssClass="align-middle text-nowrap" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Role" HeaderText="身份">
                                    <HeaderStyle CssClass="text-nowrap" />
                                    <ItemStyle CssClass="align-middle text-nowrap" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="IsActive" HeaderText="權限">
                                    <HeaderStyle CssClass="text-center text-nowrap" />
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:CheckBoxField>
                                <asp:BoundField DataField="LastLoginAt" HeaderText="最後登入時間" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                                    <HeaderStyle CssClass="text-nowrap" />
                                    <ItemStyle CssClass="align-middle text-nowrap" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="操作">
                                    <HeaderStyle CssClass="text-center text-nowrap" />
                                    <ItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <asp:LinkButton ID="LinkButtonEdit" runat="server" CommandName="EditUser" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-primary mr-2" ToolTip="編輯"><i class="fas fa-pencil-alt"></i></asp:LinkButton>
                                            <asp:LinkButton ID="ButtonDelete" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('您確定要刪除這個用戶嗎？');" CssClass="btn btn-sm btn-danger" ToolTip="刪除"><i class="fas fa-trash"></i></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-5">
            <asp:Panel ID="PanelForm" runat="server">
                <div class="card shadow mb-4">
                    <div class="card-header py-3">
                        <h4 class="m-0 font-weight-bold text-primary">
                            <asp:Label ID="LabelTitle" runat="server">新增使用者</asp:Label>
                        </h4>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="LabelPanelMessage" runat="server" CssClass="alert alert-info d-block"></asp:Label>
                        <asp:HiddenField ID="HiddenFieldUserId" runat="server" />

                        <div class="form-group">
                            <label for="<%= TextBoxUserName.ClientID %>">*帳號</label><asp:TextBox ID="TextBoxUserName" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="<%= TextBoxPassword.ClientID %>">密碼</label><asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" CssClass="form-control" autocomplete="new-password"></asp:TextBox><small class="form-text text-muted">新增時為必填，編輯時留空表示不變更密碼。</small>
                        </div>
                        <div class="form-group">
                            <label for="<%= TextBoxConfirmPassword.ClientID %>">確認密碼</label><asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="<%= TextBoxDisplayName.ClientID %>">顯示名稱</label><asp:TextBox ID="TextBoxDisplayName" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="<%= TextBoxEmail.ClientID %>">Email</label><asp:TextBox ID="TextBoxEmail" runat="server" TextMode="Email" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="<%= DropDownListRole.ClientID %>">角色</label><asp:DropDownList ID="DropDownListRole" runat="server" CssClass="custom-select">
                                <asp:ListItem Value="Admin">一般管理員</asp:ListItem>
                                <asp:ListItem Value="SuperAdmin">超級管理員</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <div class="custom-control custom-checkbox">
    <input type="checkbox" id="CheckBoxIsActive" class="custom-control-input" runat="server" checked="checked" />
                                <label class="custom-control-label" for="ContentPlaceHolder1_CheckBoxIsActive">啟用此帳號</label>
</div>

                        </div>

                        <asp:Panel ID="infoRow" runat="server" Visible="false">
                            <hr />
                            <div class="small text-muted">
                                創建時間：<asp:Literal ID="LiteralCreatedAt" runat="server"></asp:Literal><br />
                                最後登入：<asp:Literal ID="LiteralLastLoginAt" runat="server"></asp:Literal>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="card-footer text-right">
                        <asp:Button ID="ButtonSave" runat="server" OnClick="ButtonSave_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="ButtonCancel" runat="server" Text="清除/重設" OnClick="ButtonCancel_Click" CausesValidation="false" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
