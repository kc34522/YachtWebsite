<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DealerList.aspx.cs" Inherits="TayanaYacht.Admin.DealerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card shadow mb-4">
        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
            <h4 class="m-0 font-weight-bold text-primary">經銷商列表</h4>
            <asp:HyperLink ID="ButtonAddDealer" runat="server" Text="<i class='fas fa-plus'></i> 新增代理商" NavigateUrl="~/Admin/DealerEdit.aspx" CssClass="btn btn-primary" />
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="<%= ddlCountry.ClientID %>">依國家篩選：</label>
                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="custom-select">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <asp:Label ID="LabelDealerListMessage" runat="server" Text="" Visible="false" CssClass="text-info d-block mt-2"></asp:Label>

            <div class="table-responsive mt-4">
                <asp:GridView ID="GridViewDealer" runat="server"
                    AutoGenerateColumns="False"
                    OnRowDeleting="GridViewDealer_RowDeleting"
                    DataKeyNames="Id"
                    CssClass="table table-bordered table-hover" Width="100%">

                    <HeaderStyle CssClass="thead-dark" />
                    <EmptyDataTemplate>
                        <div class="alert alert-info text-center">--- 查無符合的經銷商資料 ---</div>
                    </EmptyDataTemplate>

                    <Columns>
                        <asp:TemplateField HeaderText="序號">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="text-nowrap align-middle" Width="60px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CountryName" HeaderText="國家" SortExpression="CountryName">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RegionName" HeaderText="地區" SortExpression="RegionName">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Name" HeaderText="代理商名稱" SortExpression="Name">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <%-- 【新原則】恢復為不換行 --%>
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Tel" HeaderText="電話" SortExpression="Tel">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="text-nowrap align-middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Email" HeaderText="電子信箱" SortExpression="Email">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <%-- 【新原則】改回 text-truncate (不換行，超出部分...顯示) --%>
                            <ItemStyle CssClass="align-middle text-truncate" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Website" HeaderText="網站" SortExpression="Website">
                            <HeaderStyle CssClass="text-nowrap" HorizontalAlign="Left" />
                            <ItemStyle CssClass="align-middle text-truncate" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="前台顯示" SortExpression="IsActive">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox2" Checked='<%# Eval("IsActive") %>' Enabled="false" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="功能">
                            <HeaderStyle CssClass="text-nowrap text-center" />
                            <ItemTemplate>
                                <div style="white-space: nowrap;">
                                    <%-- 【已修正】恢復為水平並排按鈕 --%>
                                    <a href='DealerEdit.aspx?id=<%# Eval("Id") %>' class="btn btn-sm btn-outline-primary mr-2" title="編輯"><i class="fas fa-pencil-alt"></i></a>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" ToolTip="刪除" OnClientClick="return confirm('您確定要刪除這位代理商嗎？');">
                                    <i class='fas fa-trash'></i>
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" CssClass="align-middle" Width="120px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
