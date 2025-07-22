<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CountryRegionManage.aspx.cs" Inherits="TayanaYacht.Admin.CountryRegionManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- 使用 Bootstrap Grid 建立左右兩欄佈局 --%>
    <div class="row">

        <div class="col-lg-6">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h4 class="m-0 font-weight-bold text-primary">國家管理</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label>新增國家</label>
                        <div class="input-group">
                            <asp:TextBox ID="TextBoxAddCountry" runat="server" CssClass="form-control" placeholder="請輸入國家名稱"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:Button ID="ButtonAddCountry" runat="server" Text="新增" OnClick="ButtonAddCountry_Click" CssClass="btn btn-success" />
                                <asp:Button ID="ButtonCancelCountry" runat="server" Text="清空" OnClick="ButtonCancelCountry_Click" CausesValidation="false" CssClass="btn btn-outline-secondary" />
                            </div>
                        </div>
                        <asp:Label ID="LabelAddCountry" runat="server" Text="" Visible="false" CssClass="text-danger mt-1"></asp:Label>
                    </div>

                    <hr />
                    <asp:Label ID="LabelCountryList" runat="server" Text="" Visible="false"></asp:Label>
                    <div class="table-responsive">
                        <asp:GridView ID="GridViewCountry" runat="server"
                            AutoGenerateColumns="False"
                            OnRowEditing="GridViewCountry_RowEditing"
                            OnRowCancelingEdit="GridViewCountry_RowCancelingEdit"
                            OnRowUpdating="GridViewCountry_RowUpdating"
                            OnRowDeleting="GridViewCountry_RowDeleting"
                            DataKeyNames="Id"
                            CssClass="table table-bordered" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:TemplateField HeaderText="序號">
                                    <HeaderStyle CssClass="text-nowrap text-center" Width="60px" />
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="國家名稱">
                                    <HeaderStyle CssClass="text-nowrap" />
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Bind("Name") %>' ID="TextBoxCountry" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="LabelCountry"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="align-middle" />
                                </asp:TemplateField>

                                <%-- 直接美化 CommandField --%>
                                <%-- 【已修改】將所有功能整合到此單一欄位 --%>
                                <asp:TemplateField HeaderText="功能">
                                    <HeaderStyle CssClass="text-nowrap text-center" />
                                    <ItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <%-- 一般模式下的「編輯」按鈕 --%>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary mr-2" ToolTip="編輯">
                <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                            <%-- 一般模式下的「刪除」按鈕 --%>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="刪除"
                                                OnClientClick="return confirm('您確定要刪除此項目嗎？');">
                <i class='fas fa-trash'></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <%-- 編輯模式下的「更新」按鈕 --%>
                                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-sm btn-success mr-2" ToolTip="更新">
                <i class="fas fa-save"></i>
                                            </asp:LinkButton>
                                            <%-- 編輯模式下的「取消」按鈕 --%>
                                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-secondary" ToolTip="取消" CausesValidation="false">
                <i class="fas fa-times"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" Width="120px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-6">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h4 class="m-0 font-weight-bold text-primary">地區管理</h4>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label>請選擇國家以管理其地區</label>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="custom-select"></asp:DropDownList>
                    </div>

                    <asp:Label ID="LabelRegionList" runat="server" Text="" Visible="false"></asp:Label>
                    <div class="table-responsive">
                        <asp:GridView ID="GridViewRegion" runat="server"
                            AutoGenerateColumns="False"
                            OnRowEditing="GridViewRegion_RowEditing"
                            OnRowCancelingEdit="GridViewRegion_RowCancelingEdit"
                            OnRowUpdating="GridViewRegion_RowUpdating"
                            OnRowDeleting="GridViewRegion_RowDeleting"
                            DataKeyNames="Id"
                            CssClass="table table-bordered" Width="100%">
                            <HeaderStyle CssClass="thead-dark" />
                            <Columns>
                                <asp:TemplateField HeaderText="序號">
                                    <HeaderStyle CssClass="text-nowrap text-center" Width="60px" />
                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="地區名稱">
                                    <HeaderStyle CssClass="text-nowrap" />
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# Bind("Name") %>' ID="TextBoxRegion" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="LabelRegion"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="align-middle" />
                                </asp:TemplateField>

                                <%-- 【已修改】將所有功能整合到此單一欄位 --%>
                                <asp:TemplateField HeaderText="功能">
                                    <HeaderStyle CssClass="text-nowrap text-center" />
                                    <ItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <%-- 一般模式下的「編輯」按鈕 --%>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-outline-primary mr-2" ToolTip="編輯">
                <i class="fas fa-pencil-alt"></i>
                                            </asp:LinkButton>
                                            <%-- 一般模式下的「刪除」按鈕 --%>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="刪除"
                                                OnClientClick="return confirm('您確定要刪除此項目嗎？');">
                <i class='fas fa-trash'></i>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div style="white-space: nowrap;">
                                            <%-- 編輯模式下的「更新」按鈕 --%>
                                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-sm btn-success mr-2" ToolTip="更新">
                <i class="fas fa-save"></i>
                                            </asp:LinkButton>
                                            <%-- 編輯模式下的「取消」按鈕 --%>
                                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-secondary" ToolTip="取消" CausesValidation="false">
                <i class="fas fa-times"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="align-middle" Width="120px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <asp:Panel ID="PanelAddRegion" runat="server" Visible="false" CssClass="mt-4">
                        <hr />
                        <div class="form-group">
                            <label>於選定國家下新增地區</label>
                            <div class="input-group">
                                <asp:TextBox ID="TextBoxAddRegion" runat="server" CssClass="form-control" placeholder="請輸入地區名稱"></asp:TextBox>
                                <div class="input-group-append">
                                    <asp:Button ID="ButtonAddRegion" runat="server" Text="新增" OnClick="ButtonAddRegion_Click" CssClass="btn btn-success" />
                                    <asp:Button ID="ButtonCancelRegion" runat="server" Text="清空" OnClick="ButtonCancelRegion_Click" CausesValidation="false" CssClass="btn btn-outline-secondary" />
                                </div>
                            </div>
                            <asp:Label ID="LabelAddRegion" runat="server" Text="" Visible="false" CssClass="text-danger mt-1"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
