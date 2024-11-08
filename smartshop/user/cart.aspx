<%@ Page Title="" Language="C#" MasterPageFile="~/user/index.Master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="smartshop.user.cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblmsg" runat="server" Visible="false"></asp:Label>
                </div>
                <h2>Our Shopping Cart</h2>
            </div>
        </div>

        <div class="container">
            <asp:Repeater ID="rcartitem" runat="server" OnItemCommand="rcartitem_ItemCommand" OnItemDataBound="rcartitem_ItemDataBound">
                <HeaderTemplate>
                    <table class="table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Image</th>
                                <th>Unit Price</th>
                                <th>Quantity</th>
                                <th>Total Price</th>
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
                            <asp:Image
                                runat="server"
                                ImageUrl='<%# GetImageUrl(Eval("imageurl").ToString()) %>'
                                Width="60" />
                        </td>

                        <td>₹<asp:Label ID="lblprice" runat="server" Text='<%# Eval("price") %>'></asp:Label>
                            <asp:HiddenField ID="hdnproductid" runat="server" Value='<%# Eval("productid") %>' />
                            <asp:HiddenField ID="hdnquantity" runat="server" Value='<%# Eval("qty") %>' />
                            <asp:HiddenField ID="hdnprdquantity" runat="server" Value='<%# Eval("prdqty") %>' />
                        </td>

                        <td>
                            <div class="product__details__option">
                                <div class="quantity">
                                    <div class="pro-qty">

                                        <asp:TextBox
                                            ID="txtquantity"
                                            runat="server"
                                            TextMode="Number"
                                            Text='<%# Bind("qty") %>'
                                            CssClass="form-control">
                                        </asp:TextBox>


                                        <asp:RegularExpressionValidator
                                            ID="revquantity"
                                            runat="server"
                                            ErrorMessage="Please enter a valid quantity (1 or more)"
                                            ForeColor="Red"
                                            Font-Size="Small"
                                            ValidationExpression="^[1-9][0-9]*$"
                                            ControlToValidate="txtquantity"
                                            Display="Dynamic"
                                            SetFocusOnError="true"
                                            EnableClientScript="true">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                            </div>
                        </td>

                        <td>₹<asp:Label ID="lbltotalprice" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbdeletel" runat="server" Text="Remove" CommandName="remove"
                                CommandArgument='<%# Eval("productid") %>'
                                OnClientClick="return confirm('Do you want to remove this item from cart?');">
                                <i class="fa fa-close"></i></asp:LinkButton>
                        </td>

                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="3"></td>
                        <td class="pl-lg-5">
                            <b>Grand Total:-</b>
                        </td>
                        <td>₹<%Response.Write(Session["grandtotalprice"]); %></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="continue__btn">
                            <a href="menu.aspx" class="btn btn-info"><i class="fa fa-arrow-circle-left mr-2"></i>Continue Shopping</a>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbupdatecart" runat="server" CommandName="updatecart" CssClass="btn btn-warning">
                                <i class="fa fa-refresh mr-2"></i>Update Cart
                            </asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbchechout" runat="server" CommandName="checkout" CssClass="btn btn-success">
                               Chechout<i class="fa fa-arrow-circle-right ml-2"></i>
                            </asp:LinkButton>

                        </td>
                    </tr>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>


    </section>



</asp:Content>
