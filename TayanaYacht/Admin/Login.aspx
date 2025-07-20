<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TayanaYacht.Admin.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Tayana | 後台管理系統</title>
    <link href="<%= ResolveUrl("~/assets/vendor/fontawesome-free/css/all.min.css") %>" rel="stylesheet" type="text/css" />
    <link
        href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i"
        rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="<%= ResolveUrl("~/assets/css/sb-admin-2.min.css") %>" rel="stylesheet" />

</head>
<body class="bg-gradient-primary">
    <form id="form1" runat="server">
        <div class="container">
            <!-- Outer Row -->
            <div class="row justify-content-center min-vh-100 d-flex align-items-center">

                <div class="col-xl-10 col-lg-12 col-md-9">

                    <div class="card o-hidden border-0 shadow-lg my-5">
                        <div class="card-body p-0">
                            <!-- Nested Row within Card Body -->
                            <div class="row">
                                <div class="col-lg-6 d-none d-lg-block bg-login-image">
                                    <div class="p-5">
                                        <img src="<%= ResolveUrl("~\\assets\\img\\yacht-svgrepo-com.svg") %>" alt="Rocket Image" />
                                    </div>
                                </div>
                                <div class="col-lg-6 align-self-center">
                                    <div class="p-5">
                                        <div class="text-center">
                                            <h1 class="h4 text-gray-900 mb-4">Welcome Back!</h1>
                                        </div>
                                        <div class="user">
                                            <div class="form-group">
                                                <asp:TextBox ID="TextBoxUserName" runat="server" placeholder="帳號" CssClass="form-control form-control-user"></asp:TextBox>
                                                <%--<input type="email" class="form-control form-control-user"
                                                    id="exampleInputEmail" aria-describedby="emailHelp"
                                                    placeholder="請輸入帳號..." />--%>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" placeholder="密碼" CssClass="form-control form-control-user"></asp:TextBox>
                                            </div>
                                            <%--<div class="form-group">
                                    <div class="custom-control custom-checkbox small">
                                        <input type="checkbox" class="custom-control-input" id="customCheck">
                                        <label class="custom-control-label" for="customCheck">Remember
                                            Me</label>
                                    </div>
                                </div>--%>
                                            <asp:Button ID="ButtonLogin" runat="server" Text="登入" OnClick="ButtonLogin_Click" CssClass="btn btn-primary btn-user btn-block" />
                                            <br />
                                            <asp:Label ID="LabelLoginMessage" runat="server" Visible="false"></asp:Label>


                                            <%--  <a href="index.html" class="btn btn-google btn-user btn-block">
                                    <i class="fab fa-google fa-fw"></i> Login with Google
                                </a>
                                <a href="index.html" class="btn btn-facebook btn-user btn-block">
                                    <i class="fab fa-facebook-f fa-fw"></i> Login with Facebook
                                </a>--%>
                                        </div>

                                        <%-- <div class="text-center">
                                <a class="small" href="forgot-password.html">Forgot Password?</a>
                            </div>
                            <div class="text-center">
                                <a class="small" href="register.html">Create an Account!</a>
                            </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </form>
    <!-- Bootstrap core JavaScript-->
    <script src="<%= ResolveUrl("~/assets/vendor/jquery/jquery.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/assets/vendor/bootstrap/js/bootstrap.bundle.min.js") %>"></script>

    <!-- Core plugin JavaScript-->
    <script src="<%= ResolveUrl("~/assets/vendor/jquery-easing/jquery.easing.min.js") %>"></script>

    <!-- Custom scripts for all pages-->
    <script src="<%= ResolveUrl("~/assets/js/sb-admin-2.min.js") %>"></script>
</html>
