<%@ Page Title="" Language="C#" MasterPageFile="~/user/index.Master" AutoEventWireup="true" CodeBehind="reg.aspx.cs" Inherits="smartshop.user.reg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
                if (lblMsg) {
                    lblMsg.style.display = "none";
                }
            }, seconds * 1000);
        };
    </script>

    <script>
        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%=imgUser.ClientID %>').src = e.target.result;
                    document.getElementById('<%=imgUser.ClientID %>').style.display = 'block';
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align_self_end">
                    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="text-gray"></asp:Label>
                </div>
                <asp:Label ID="lblHeaderMsg" runat="server" Text="<h2>User Registration</h2>"></asp:Label>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">

                        <div>
                            <asp:RequiredFieldValidator ID="rfvname" runat="server" ErrorMessage="Name is required" ControlToValidate="txtname"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revname" runat="server" ErrorMessage="Name must be characters only"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[a-zA-Z\s]+$"
                                ControlToValidate="txtname">
                            </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtname" runat="server" CssClass="form-control" placeholder="Enter Full Name"
                                ToolTip="Full Name">
                            </asp:TextBox>
                        </div>

                        <div>
                            <asp:RequiredFieldValidator ID="rfvusername" runat="server" ErrorMessage="Username is required" ControlToValidate="txtusername"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" placeholder="Enter Username"
                                ToolTip="Username">
                            </asp:TextBox>
                        </div>

                        <div>
                            <asp:RequiredFieldValidator ID="rfvemail" runat="server" ErrorMessage="Email is required" ControlToValidate="txtemail"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" placeholder="Enter Email"
                                ToolTip="Email" TextMode="Email">
                            </asp:TextBox>
                        </div>

                        <div>
                            <asp:RequiredFieldValidator ID="rfvmobile" runat="server" ErrorMessage="Mobile Number is required" ControlToValidate="txtmobile"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revmobile" runat="server" ErrorMessage="Mobile Number must have 10 digits"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[0-9]{10}$"
                                ControlToValidate="txtmobile">
                            </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtmobile" runat="server" CssClass="form-control" placeholder="Enter Mobile Number"
                                ToolTip="Mobile Number" TextMode="Number">
                            </asp:TextBox>
                        </div>

                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form_container">

                        <div>
                            <asp:RequiredFieldValidator ID="rfvaddress" runat="server" ErrorMessage="Address is required" ControlToValidate="txtaddress"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control" placeholder="Enter Address"
                                ToolTip="Address" TextMode="MultiLine">
                            </asp:TextBox>
                        </div>

                        <div>
                            <asp:RequiredFieldValidator ID="rfvpostcode" runat="server" ErrorMessage="Post/Zip Code is required" ControlToValidate="txtpostcode"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revpostcode" runat="server" ErrorMessage="Post/Zip Code must be of 6 digits"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[0-9]{6}$"
                                ControlToValidate="txtpostcode">
                            </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtpostcode" runat="server" CssClass="form-control" placeholder="Enter Post/Zip Code"
                                ToolTip="Post/Zip Code" TextMode="Number">
                            </asp:TextBox>
                        </div>

                        <div>
                            <asp:FileUpload ID="fuUserImage" runat="server" CssClass="form-control" ToolTip="User Image"
                                onchange="ImagePreview(this);" />
                        </div>

                        <div>
                            <asp:RequiredFieldValidator ID="rfvpassword" runat="server" ErrorMessage="Password is required" ControlToValidate="txtpassword"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtpassword" runat="server" CssClass="form-control" placeholder="Enter Password"
                                ToolTip="Password" TextMode="Password">
                            </asp:TextBox>
                        </div>

                    </div>
                    <div class="row pl-4">
                        <div class="btn_box">
                            <asp:Button ID="btnregister" runat="server" Text="Register" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white" OnClick="btnregister_Click" />

                            <asp:Label ID="lblAlredyUser" runat="server" Text="Already registered? <a href='login.aspx' class='badge badge-info'>Login here...</a>"></asp:Label>
                        </div>
                    </div>

                    <div class="row p-5">
                        <div style="align-items: center">
                            <asp:Image ID="imgUser" runat="server" CssClass="img-thumbnail" Style="display: none;" Width="200px" Height="200px" />
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </section>



</asp:Content>
