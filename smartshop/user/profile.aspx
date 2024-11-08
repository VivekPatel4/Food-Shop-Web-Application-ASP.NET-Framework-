<%@ Page Title="" Language="C#" MasterPageFile="~/user/index.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="smartshop.user.profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <h2>User Information</h2>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="card-accordion-title mb-4">
                                <div class="d-flex justify-content-start">
                                    <div class="image-container">
                                        <asp:Image ID="imgProfile" runat="server" CssClass="img-thumbnail" Width="150" Height="150"
                                            ImageUrl='<%= Session["imageurl"] != null ? Session["imageurl"].ToString() : "default.jpg" %>' />
                                        <div class="middle pt-2">
                                            <a href="reg.aspx?id=<%Response.Write(Session["userid"]); %>" class="btn btn-warning">
                                                <i class="fa fa-pencil"></i>Edit Details
                                            </a>
                                        </div>
                                    </div>

                                    <div class="userData ml-3">
                                        <h2 class="d-block" style="font-size: 1.5rem; font-weight: bold;">
                                            <a href="javascript:void(0);"><%Response.Write(Session["name"]); %></a>
                                        </h2>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0)">
                                                <asp:Label ID="lblusername" runat="server" ToolTip="Unique Username">
                                                    @<%Response.Write(Session["username"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0)">
                                                <asp:Label ID="lblemail" runat="server" ToolTip="User Email">
                                                    @<%Response.Write(Session["email"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0)">
                                                <asp:Label ID="lblCreatedData" runat="server" ToolTip="Account Created on">
                                                    @<%Response.Write(Session["createddate"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
                                        <li class="nav-item">
                                            <a class="nav-link active text-info" id="basicInfo-tab" data-toggle="tab" href="#basicInfo" role="tab"
                                                aria-controls="basicInfo" aria-selected="true"><i class="fa fa-id-badge mr-2"></i>Basic Info</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link text-info" id="connectedServices-tab" data-toggle="tab" href="#connectedServices" role="tab"
                                                aria-controls="connectedServices" aria-selected="false"><i class="fa fa-clock-o mr-2"></i>Purchased History</a>
                                        </li>
                                    </ul>

                                    <div class="tab-content ml-1" id="myTabContent">
                                        <div class="tab-pane fade show active" id="basicInfo" role="tabpanel" aria-labelledby="basicInfo-tab">
                                            <asp:Repeater ID="ruserprofile" runat="server">
                                                <ItemTemplate>
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Name</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("name") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Username</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("username") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Mobile No.</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("mobile") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Email </label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("email") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Post code </label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("postcode") %>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Address</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("address") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>

                                        <div class="tab-pane fade" id="connectedServices" role="tabpanel" aria-labelledby="connectedServices-tab">
                                            <asp:Repeater ID="rparchasehistory" runat="server" OnItemDataBound="rparchasehistory_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="container">
                                                        <div class="row pt-1 pb-1" style="background-color: lightgray">
                                                            <div class="col-4">
                                                                <span class="badge badge-pill badge-dark text-white">
                                                                    <%# Eval("srno") %>
                                                                </span>
                                                                Payment Mode:<%# Eval("paymentmode").ToString()=="cod"?"Cash On Delivery": Eval("paymentmode").ToString().ToUpper() %>
                                                            </div>
                                                            <div class="col-6">
                                                                <%# string.IsNullOrEmpty(Eval("cardno").ToString())?"":"Card No:" + Eval("cardno") %>
                                                            </div>
                                                            <div class="col-2" style="text-align: end">
                                                                <a href="invoice.aspx?id=<%# Eval("paymentid") %>" class="btn btn-info">
                                                                    <i class="fa fa-download mr-2"></i>Invoice</a>
                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hdnpaymentid" runat="server" Value='<%# Eval("paymentid") %>' />

                                                        <asp:Repeater ID="rorder" runat="server">
                                                            <HeaderTemplate>
                                                                <table class="table data-table-export table-responsive-sm table-bordered table-hover">
                                                                    <thead class="bg-dark text-white">
                                                                        <tr>
                                                                            <th>Product Name</th>
                                                                            <th>Unit Price</th>
                                                                            <th>Qty</th>
                                                                            <th>Total Price</th>
                                                                            <th>OrderId</th>
                                                                            <th>Status</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblname" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblprice" runat="server" Text='<%# string.IsNullOrEmpty(Eval("price").ToString())?"":"₹" + Eval("price") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblquantity" runat="server" Text='<%# Eval("quantity") %>'></asp:Label>
                                                                    </td>
                                                                    <td>₹<asp:Label ID="lbltotalprice" runat="server" Text='<%# Eval("totalprice") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblorderno" runat="server" Text='<%# Eval("orderno") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>'
                                                                            CssClass='<%# (Eval("status").ToString().ToLower() == "delivered") ? "badge badge-success" : "badge badge-warning" %>'></asp:Label>
                                                                    </td>

                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </tbody>
                                                                </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>

                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
