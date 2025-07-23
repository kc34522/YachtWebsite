<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="NewsList.aspx.cs" Inherits="TayanaYacht.Admin.NewsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h4 class="m-0 font-weight-bold text-primary">新聞列表</h4>
            <asp:HyperLink ID="HyperLinkAdd" runat="server" NavigateUrl="~/Admin/NewsAddEdit.aspx" CssClass="btn btn-primary">
                <i class="fas fa-plus"></i> 新增新聞
            </asp:HyperLink>
        </div>
        <div class="card-body">
            <asp:Panel runat="server" DefaultButton="ButtonSearch">
                <div class="row">
                    <div class="col-md-6 mb-3">
                        <div class="input-group">
                            <asp:TextBox ID="TextBoxSearch" runat="server" TextMode="Search" placeholder="請輸入標題或內容關鍵字..." CssClass="form-control"></asp:TextBox>
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

            <asp:Label ID="LabelGridViewMessage" runat="server" CssClass="text-info d-block mb-2" ></asp:Label>

            <div class="table-responsive">
                <asp:GridView ID="GridViewNews" runat="server" 
                    AutoGenerateColumns="False" 
                    DataKeyNames="Id" 
                    OnRowDeleting="GridViewNews_RowDeleting"
                    AllowPaging="true" PageSize="10" OnPageIndexChanging="GridViewNews_PageIndexChanging"
                    OnRowDataBound="GridViewNews_RowDataBound"
                    CssClass="table table-bordered table-hover" Width="100%">
                    
                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:TemplateField HeaderText="序號">
                            <HeaderStyle CssClass="text-nowrap text-center" Width="60px" />
                            <ItemTemplate>
                                <asp:Label ID="LabelRowNumber" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="text-nowrap align-middle" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="Title" HeaderText="標題" SortExpression="Title">
                            <HeaderStyle CssClass="text-nowrap text-center" HorizontalAlign="Center" />
                            <ItemStyle CssClass="align-middle" />
                        </asp:BoundField>

                        <asp:CheckBoxField DataField="IsTop" HeaderText="是否置頂" SortExpression="IsTop">
                            <HeaderStyle CssClass="text-nowrap text-center" Width="120px" />
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:CheckBoxField>

                        <asp:CheckBoxField DataField="IsVisible" HeaderText="是否顯示" SortExpression="IsVisible">
                            <HeaderStyle CssClass="text-nowrap text-center" Width="120px" />
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:CheckBoxField>

                        <asp:BoundField DataField="PublishDate" HeaderText="上架日期" SortExpression="PublishDate" DataFormatString="{0:yyyy-MM-dd}">
                            <HeaderStyle CssClass="text-nowrap text-center" HorizontalAlign="Center" />
                            <ItemStyle CssClass="text-nowrap text-center align-middle" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="功能">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemTemplate>
                                <div style="white-space: nowrap;">
                                    <asp:HyperLink ID="HyperLinkEdit" runat="server" NavigateUrl='<%# "~/Admin/NewsAddEdit.aspx?Id="+Eval("Id") %>' CssClass="btn btn-sm btn-outline-primary mr-2" ToolTip="管理">
                                        <i class="fas fa-pencil-alt"></i>
                                    </asp:HyperLink>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="刪除" OnClientClick="return confirm('您確定要刪除此筆新聞嗎？');">
                                        <i class='fas fa-trash'></i>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" Width="120px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pagination-ys" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>