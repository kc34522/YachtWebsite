<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="YachtsList.aspx.cs" Inherits="TayanaYacht.Admin.YachtsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h4 class="m-0 font-weight-bold text-primary">產品列表</h4>
            <asp:HyperLink ID="HyperLinkAdd" runat="server" NavigateUrl="~/Admin/YachtEdit.aspx" CssClass="btn btn-primary">
                 <i class="fas fa-plus"></i> 新增產品
            </asp:HyperLink>
        </div>
        <div class="card-body">
            <asp:Label ID="LabelGridViewYacht" runat="server" CssClass="text-info d-block mb-2"></asp:Label>
            <div class="table-responsive">
                <%-- 【已移除】AllowPaging, PageSize, OnPageIndexChanging, AllowSorting, OnSorting, OnRowDataBound --%>
                <asp:GridView ID="GridViewYachtList" runat="server" 
                    AutoGenerateColumns="False" 
                    DataKeyNames="YachtID" 
                    OnRowDeleting="GridViewYachtList_RowDeleting"
                    CssClass="table table-bordered table-hover" Width="100%">

                    <HeaderStyle CssClass="thead-dark" />
                    <Columns>
                        <asp:TemplateField HeaderText="序號">
                            <HeaderStyle CssClass="text-nowrap text-center" Width="60px" />
                            <%-- 【已簡化】序號計算恢復為無分頁版本 --%>
                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="text-nowrap align-middle" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="ModelName" HeaderText="產品名稱" SortExpression="ModelName">
                             <HeaderStyle CssClass="text-nowrap text-center" HorizontalAlign="Left" />
                             <ItemStyle CssClass="text-nowrap align-middle text-center" />
                        </asp:BoundField>

                        <asp:CheckBoxField DataField="IsActive" HeaderText="前台顯示" SortExpression="IsActive">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:CheckBoxField>

                        <asp:CheckBoxField DataField="IsNewBuilding" HeaderText="全新建造" SortExpression="IsNewBuilding">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:CheckBoxField>
                        
                        <asp:CheckBoxField DataField="IsNewDesign" HeaderText="全新設計" SortExpression="IsNewDesign">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:CheckBoxField>
                        
                        <asp:TemplateField HeaderText="功能">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemTemplate>
                                <div style="white-space: nowrap;">
                                    <asp:HyperLink ID="HyperLinkEdit" runat="server" NavigateUrl='<%# "~/Admin/YachtEdit.aspx?Id="+Eval("YachtID") %>' CssClass="btn btn-sm btn-outline-primary mr-2" ToolTip="管理">
                                        <i class="fas fa-pencil-alt"></i>
                                    </asp:HyperLink>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="刪除" 
                                        OnClientClick="return confirm('您確定要刪除此船型嗎？');">
                                        <i class='fas fa-trash'></i>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" Width="120px" />
                        </asp:TemplateField>
                    </Columns>
                    <%-- 【已移除】PagerStyle 因為不再需要分頁 --%>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>