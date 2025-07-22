<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TayanaYacht.Admin.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%--待優化: 搜尋功能加上「選擇搜尋欄位」的下拉選單--%>

<%-- Content2: 建議將此處的標題移除，避免與卡片標題重複 --%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">聯絡表單列表</h6>
        </div>
        <div class="card-body">
            <asp:Panel runat="server" DefaultButton="ButtonSearch">
                <div class="row">
                    <div class="col-md-6">
                        <div class="input-group">
                            <asp:TextBox ID="TextBoxSearch" runat="server" TextMode="Search" placeholder="請輸入關鍵字..." CssClass="form-control"></asp:TextBox>
                            <div class="input-group-append">
                                <asp:LinkButton ID="ButtonSearch" runat="server" OnClick="ButtonSearch_Click" CssClass="btn btn-primary">
                                    <i class='fas fa-search'></i> 搜尋
                                </asp:LinkButton>
                                <asp:Button ID="ButtonClearSearch" runat="server" Text="清除" OnClick="ButtonClearSearch_Click" CssClass="btn btn-outline-secondary" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Label ID="LabelSearchResult" runat="server" Visible="false" CssClass="text-info mt-2 d-block"></asp:Label>
            <asp:Label ID="LabelContactListMessage" runat="server" Visible="false" CssClass="text-danger mt-2 d-block"></asp:Label>

            <div class="table-responsive mt-4">
                <asp:GridView ID="GridViewContact" runat="server"
                    AutoGenerateColumns="False"
                    OnRowDeleting="GridViewConctact_RowDeleting"
                    AllowPaging="true" PageSize="10"
                    OnPageIndexChanging="GridViewContact_PageIndexChanging"
                    DataKeyNames="Id" OnRowDataBound="GridViewContact_RowDataBound"
                    CssClass="table table-bordered table-hover" Width="100%">

                    <HeaderStyle CssClass="thead-dark" />

                    <Columns>
                        <asp:TemplateField HeaderText="序號">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemTemplate>
                                <asp:Label ID="LabelRowNumber" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="text-nowrap align-middle" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="姓名" SortExpression="Name">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Email" HeaderText="信箱" SortExpression="Email">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Phone" HeaderText="電話" SortExpression="Phone">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CountryName" HeaderText="國家" SortExpression="CountryName">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ModelName" HeaderText="宣傳單:船型" SortExpression="ModelName">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="回覆內容" SortExpression="Comments">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <%-- 【已修改】設定固定寬度，並允許內容自動換行 --%>
                                <div style="width: 400px; word-wrap: break-word;">
                                    <%# Eval("Comments") %>
                                </div>
                            </ItemTemplate>
                            <ItemStyle CssClass="align-middle" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="CreatedTime" HeaderText="表單送出時間" SortExpression="CreatedTime" DataFormatString="{0:yyyy-MM-dd HH:mm}">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="操作">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="Delete"
                                    CssClass="btn btn-sm btn-danger"
                                    ToolTip="刪除"
                                    OnClientClick="return confirm('確定要刪除這筆資料嗎？');">
                                    <i class='fas fa-trash'></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:TemplateField>
                    </Columns>

                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
