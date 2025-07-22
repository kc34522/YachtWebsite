<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TayanaYacht.Contact" %>

<%@ Register Assembly="Recaptcha.Web" Namespace="Recaptcha.Web.UI.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <link href="<%= ResolveUrl("~/Front_Assets/css/homestyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="bannerMask" runat="server">
    <img src="<%= ResolveUrl("~/Front_Assets/images/contact.jpg") %>" alt="&quot;&quot;" width="967" height="371" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bannerSmall" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="banner" runat="server">
    <div class="banner">
        <ul>
            <li>
                <img src="<%= ResolveUrl("~/Front_Assets/images/newbanner.jpg") %>" alt="Tayana Yachts" /></li>
        </ul>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <!--表單-->


    <div class="from01">
        <p>
            Please Enter your contact information<span class="span01">*Required</span>
        </p>
        <br />
        <table>
            <tr>
                <td class="from01td01">
                    <label for="TextBoxName">Name :</label>
                </td>
                <td><span>*</span><asp:TextBox ID="TextBoxName" runat="server" ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="LabelName" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="from01td01"><label for="TextBoxEmail">Email :</label></td>
                <td><span>*</span><asp:TextBox ID="TextBoxEmail" runat="server" TextMode="Email" ClientIDMode="Static"></asp:TextBox>
                    <asp:Label ID="LabelEmail" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="from01td01"><label for="TextBoxPhone">Phone :</label></td>
                <td><span>*</span><asp:TextBox ID="TextBoxPhone" runat="server" ClientIDMode="Static" TextMode="Phone"></asp:TextBox>
                    <asp:Label ID="LabelPhone" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="from01td01"><label for="DropDownListCountry">Country :</label></td>
                <td><span>*</span>
                    <asp:DropDownList ID="DropDownListCountry" runat="server" ClientIDMode="Static"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2"><span>*</span><label for="DropDownListBrochure">Brochure of interest  *Which Brochure would you like to view?</label></td>
            </tr>
            <tr>
                <td class="from01td01">&nbsp;</td>
                <td>
                    <asp:DropDownList ID="DropDownListBrochure" runat="server" ClientIDMode="Static"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="from01td01"><label for="TextBoxComments">Comments :</label></td>
                <td>
                    <asp:TextBox ID="TextBoxComments" runat="server" Columns="60" Rows="5" TextMode="MultiLine" ClientIDMode="Static" MaxLength="500"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="from01td01">&nbsp;</td>
                <td class="f_right">
                    <cc1:RecaptchaWidget ID="RecaptchaWidget1" runat="server" />
                    <asp:Label ID="LabelRecaptcha" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>

            <tr>
                <td class="from01td01">&nbsp;</td>
                <td class="f_right">
                    <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="submit" Width="59" Height="25" ImageUrl="~/Front_Assets/images/buttom03.gif" OnClick="ImageButton1_Click" />
                    <br />
                    <asp:Label ID="LabelSubmit" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <!--表單-->
    <div class="box1">
        <span class="span02">Contact with us</span><br />
        Thanks for your enjoying our web site as an introduction to the Tayana world and our range of yachts.
As all the designs in our range are semi-custom built, we are glad to offer a personal service to all our potential customers. 
If you have any questions about our yachts or would like to take your interest a stage further, please feel free to contact us.
    </div>

    <div class="list03">
        <p>
            <span>TAYANA HEAD OFFICE</span><br />
            NO.60 Haichien Rd. Chungmen Village Linyuan Kaohsiung Hsien 832 Taiwan R.O.C<br />
            tel. +886(7)641 2422<br />
            fax. +886(7)642 3193<br />
            info@tayanaworld.com<br />
        </p>
    </div>


    <div class="list03">
        <p>
            <span>SALES DEPT.</span><br />
            +886(7)641 2422  ATTEN. Mr.Basil Lin<br />
            <br />
        </p>
    </div>

    <div class="box4">
        <h4>Location</h4>
        <p>
            <iframe width="695" height="518" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3685.9791343917445!2d120.36291278285873!3d22.504966026785095!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3471e297f2453fc9%3A0xb4400b87cbf2aa95!2zODMy6auY6ZuE5biC5p6X5ZyS5Y2A5rW35aKY6LevNjDomZ8!5e0!3m2!1szh-TW!2stw!4v1752922425334!5m2!1szh-TW!2stw" width="600" height="450" style="border: 0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
        </p>

    </div>
</asp:Content>

