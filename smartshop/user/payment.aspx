<%@ Page Title="" Language="C#" MasterPageFile="~/user/index.Master" AutoEventWireup="true" CodeBehind="payment.aspx.cs" Inherits="smartshop.user.payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rounded {
            border-radius: 1rem;
        }

        .nav-pills .nav-link {
            color: #555;
        }

            .nav-pills .nav-link.active {
                color: white;
            }

        .bold {
            font-weight: bold;
        }

        .card {
            padding: 40px 50px;
            border-radius: 20px;
            border: none;
            box-shadow: 1px 5px 10px 1px rgba(0, 0, 0, 0.2);
        }
    </style>

    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%= lblmsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };

        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

    <script type="text/javascript">
        function disablebackbutton() {
            window.history.forward();
        }

        disablebackbutton();
        window.onload = disablebackbutton;
        window.onpageshow = function (evt) {
            if (evt.persisted) disablebackbutton();
        };
        window.onunload = function () {
            void (0);
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section" style="background-image: url('../images/payment-bg.png'); width: 100%; height: 100%; background-repeat: no-repeat; background-size: auto; background-attachment: fixed; background-position: left;">
        <div class="container py-5">
            <div class="align-self-end">
                <asp:Label ID="lblmsg" runat="server" Visible="false"></asp:Label>
            </div>

            <div class="row mb-4">
                <div class="col-lg-8 mx-auto text-center">
                    <h1 class="display-6">Order Payment</h1>
                </div>
            </div>
            <div class="row pb-5">
                <div class="col-lg-6 mx-auto">
                    <div class="card">
                        <div class="card-header">
                            <div class="bg-white shadow-sm pt-4 pl-2 pr-2 pb-2">
                                <!-- Payment method tabs -->
                                <ul role="tablist" class="nav bg-light nav-pills rounded nav-fill mb-3">
                                    <li class="nav-item">
                                        <a data-toggle="pill" href="#credit-card" class="nav-link active">
                                            <i class="fas fa-credit-card mr-2"></i>Credit Card
                                        </a>
                                    </li>
                                    <li class="nav-item">
                                        <a data-toggle="pill" href="#cashondelivery" class="nav-link">
                                            <i class="fa fa-truck mr-2"></i>Cash on Delivery
                                        </a>
                                    </li>
                                </ul>
                            </div>

                            <!-- Payment method content -->
                            <div class="tab-content">
                                <!-- Credit card info -->
                                <div id="credit-card" class="tab-pane fade show active pt-3">
                                    <div role="form">
                                        <!-- Card Owner -->
                                        <div class="form-group">
                                            <label for="txtname">
                                                <h6>Card Owner</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvname" runat="server" ErrorMessage="Name is required" ControlToValidate="txtname"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revname" runat="server" ErrorMessage="Name must be characters only"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[a-zA-Z\s]+$"
                                                ControlToValidate="txtname" ValidationGroup="card">
                                            </asp:RegularExpressionValidator>
                                            <asp:TextBox ID="txtname" runat="server" CssClass="form-control" placeholder="Card Owner Name"></asp:TextBox>
                                        </div>

                                        <!-- Card Number -->
                                        <div class="form-group">
                                            <label for="txtcardno">
                                                <h6>Card Number</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvcardno" runat="server" ErrorMessage="Card Number is required" ControlToValidate="txtcardno"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                            </asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Card Number must be 16 digits"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{16}"
                                                ControlToValidate="txtcardno" ValidationGroup="card">
                                            </asp:RegularExpressionValidator>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtcardno" runat="server" CssClass="form-control" placeholder="Valid Card Number" TextMode="Number"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text text-muted">
                                                        <i class="fa fa-cc-visa mx-1"></i>
                                                        <i class="fa fa-cc-mastercard mx-1"></i>
                                                        <i class="fa fa-cc-amex mx-1"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Expiration Date and CVV -->
                                        <div class="row">
                                            <div class="col-sm-8">
                                                <div class="form-group">
                                                    <label>
                                                        <span class="hidden-xs">
                                                            <h6>Expiration Date</h6>
                                                        </span>
                                                    </label>
                                                    <asp:RequiredFieldValidator ID="rfvexpmonth" runat="server" ErrorMessage="Expiry month is required" ControlToValidate="txtexpmonth"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Expiry month must be of 2 digits"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{2}"
                                                        ControlToValidate="txtexpmonth" ValidationGroup="card">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvexpyear" runat="server" ErrorMessage="Expiry year is required" ControlToValidate="txtexpyear"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Expiry year must be of 4 digits"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{4}"
                                                        ControlToValidate="txtexpyear" ValidationGroup="card">
                                                    </asp:RegularExpressionValidator>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtexpmonth" runat="server" CssClass="form-control" placeholder="MM" TextMode="Number"></asp:TextBox>
                                                        <asp:TextBox ID="txtexpyear" runat="server" CssClass="form-control" placeholder="YYYY" TextMode="Number"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group mb-4">
                                                    <label data-toggel="tooltip" title="Three digit CV code on the back of your card">
                                                        <h6>CVV<i class="fa fa-question-circle d-inline"></i></h6>
                                                    </label>
                                                    <asp:RequiredFieldValidator ID="rfvcvv" runat="server" ErrorMessage="CVV no. is required" ControlToValidate="txtcvv"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="CVV No. must be of 3 digits"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{3}"
                                                        ControlToValidate="txtcvv" ValidationGroup="card">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtcvv" runat="server" CssClass="form-control" placeholder="CVV" TextMode="Number"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtaddress">
                                                <h6>Delivery Address</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvaddress" runat="server" ErrorMessage="Address is required" ControlToValidate="txtaddress"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtaddress" runat="server" CssClass="form-control" placeholder="Delivery Address" TextMode="MultiLine" ValidationGroup="card"></asp:TextBox>

                                        </div>
                                        <!-- Credit card form submit button -->
                                        <asp:LinkButton ID="lbcardsubmit" runat="server" CssClass="subscribe btn btn-info btn-block shadow" ValidationGroup="card"
                                            OnClick="lbcardsubmit_Click">
                                                Confirm Payment</asp:LinkButton>
                                    </div>
                                </div>

                                <!-- Cash on delivery option -->
                                <div id="cashondelivery" class="tab-pane fade pt-3">
                                    <div role="form">
                                        <!-- Full Name for COD -->

                                        <!-- Phone Number -->
                                        <div class="form-group">
                                            <label for="txtcodaddress">
                                                <h6>Delivery Address</h6>
                                            </label>
                                            <asp:TextBox ID="txtcodaddress" runat="server" CssClass="form-control" placeholder="Delivery Address" TextMode="MultiLine" ValidationGroup="card"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvcodaddress" runat="server" ErrorMessage="Address is required" ControlToValidate="txtcodaddress"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="cod" Font-Names="Segoe Script">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <!-- Submit button for COD -->
                                        <asp:LinkButton ID="lbcodsubmit" runat="server" CssClass="btn btn-info" ValidationGroup="cod"
                                            OnClick="lbcodsubmit_Click"> <i class="fa fa-cart-arrow-down mr-2"></i>
                                        Confirm Order</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Order Total Label -->
                        <div class="card-footer">
                            <b class="badge badge-success badge-pill shadow-sm">Order Total:₹ <% Response.Write(Session["grandtotalprice"]); %></b>
                            <div class="pt-1">
                                <asp:ValidationSummary ID="validationsummary1" runat="server" ForeColor="Red" ValidationGroup="card"
                                    HeaderText="Fix the following errors" Font-Names="Segoe Script"></asp:ValidationSummary>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
