<%@ Page Title="" Language="C#" MasterPageFile="~/user/index.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="smartshop.user.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%# lblMsg.ClientID %>");
                if (lblMsg) {
                    lblMsg.style.display = "none";
                }
            }, seconds * 1000);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>
                <h2>Login</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <img id="userLogin" src="../images/login.png" alt="" class="img-thumbnail" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form_container">
                        <asp:RequiredFieldValidator ID="rfvusername" runat="server" ErrorMessage="Username is Required" ControlToValidate="txtusername"
                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" placeholder="Enter UserName"></asp:TextBox>
                    </div>
                    <div class="form_container">
                        <asp:RequiredFieldValidator ID="rfvpassword" runat="server" ErrorMessage="Password is Required" ControlToValidate="txtpassword"
                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        <asp:TextBox ID="txtpassword" runat="server" CssClass="form-control" placeholder="Enter Password"></asp:TextBox>
                    </div>
                    <div class="btn_box">
                        <asp:Button ID="btnlogin" runat="server" Text="Login" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white" 
                            OnClick="btnlogin_Click"/>
                        <span class="pl-3 text-info">New User? <a href="reg.aspx" class="badge badge-info">Register Here...</a></span>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
