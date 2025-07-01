<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="CountryRegionManage.aspx.cs" Inherits="TayanaYacht.Admin.CountryRegionManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderH1" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    國家管理:
    <br />

    新增國家:  <asp:TextBox ID="TextBoxAddCountry" runat="server"></asp:TextBox><asp:Button ID="ButtonAddCountry" runat="server" Text="新增" OnClick="ButtonAddCountry_Click" /><asp:Button ID="ButtonCancelCountry" runat="server" Text="清空" OnClick="ButtonCancelCountry_Click"/><asp:Label ID="LabelAddCountry" runat="server" Text="" Visible="false"></asp:Label><br />

    經銷商國家列表: <asp:Label ID="LabelCountryList" runat="server" Text="" Visible="false"></asp:Label>
    <asp:GridView ID="GridViewCountry" runat="server" AutoGenerateColumns="False" OnRowEditing="GridViewCountry_RowEditing" OnRowCancelingEdit="GridViewCountry_RowCancelingEdit" OnRowUpdating="GridViewCountry_RowUpdating" OnRowDeleting="GridViewCountry_RowDeleting" DataKeyNames="Id">
        <Columns>
            <asp:TemplateField HeaderText="序號">
                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="國家名稱" SortExpression="Name">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Name") %>' ID="TextBoxCountry"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="LabelCountry"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True"></asp:CommandField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>
    

    經銷商地區管理:
    <br />
    選擇國家:
    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
    <asp:Label ID="LabelRegionList" runat="server" Text="" Visible="false"></asp:Label>
    <asp:GridView ID="GridViewRegion" runat="server" AutoGenerateColumns="False" OnRowEditing="GridViewRegion_RowEditing" OnRowCancelingEdit="GridViewRegion_RowCancelingEdit" OnRowUpdating="GridViewRegion_RowUpdating" OnRowDeleting="GridViewRegion_RowDeleting" DataKeyNames="Id">
        <Columns>
            <asp:TemplateField HeaderText="序號">
                <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="地區名稱" SortExpression="Name">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("Name") %>' ID="TextBoxRegion"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="LabelRegion"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True"></asp:CommandField>
            <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
        </Columns>
    </asp:GridView>

    <asp:Panel ID="PanelAddRegion" runat="server" Visible="false">
        新增地區: <asp:TextBox ID="TextBoxAddRegion" runat="server"></asp:TextBox><asp:Button ID="ButtonAddRegion" runat="server" Text="新增" OnClick="ButtonAddRegion_Click"/><asp:Button ID="ButtonCancelRegion" runat="server" Text="清空" OnClick="ButtonCancelRegion_Click"/><asp:Label ID="LabelAddRegion" runat="server" Text="" Visible="false"></asp:Label>
    </asp:Panel>
    

</asp:Content>
