<%@ Page Title="" Language="C#" MasterPageFile="~/admin/index.Master" AutoEventWireup="true" CodeBehind="orderstatus.aspx.cs" Inherits="smartshop.admin.orderstatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%= lblMsg.ClientID %>");
                if (lblMsg) {
                    lblMsg.style.display = "none";
                }
            }, seconds * 1000);
        };
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pcoded-inner-content pt-0">
        <div class="align-align-self-end">
            <!-- Ensure lblMsg is visible for the message to show -->
            <asp:Label ID="lblMsg" runat="server" CssClass="align-align-self-end text-white alert" Visible="true"></asp:Label>
        </div>

        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">
                        <div class="container">
                            <div class="row">
                                <!-- Category Form Card -->
                                <div class="col-sm-6 col-md-8 col-lg-8">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="sub-title">Order List</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table-responsive">
                                                <asp:Repeater ID="rorderstatus" runat="server" OnItemCommand="rorderstatus_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered text-white">
                                                            <thead>
                                                                <tr>
                                                                    <th>Order No.</th>
                                                                    <th>Order Date</th>
                                                                    <th>Status</th>
                                                                    <th>Product Name</th>
                                                                    <th>Total Price</th>
                                                                    <th>Payment Mode</th>
                                                                    <th>Edit</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("orderno") %></td>
                                                            <td><%# Eval("orderdate", "{0:MM/dd/yyyy}") %></td>
                                                            <td>
                                                                <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>'
                                                                    CssClass='<%# Eval("status").ToString() == "Delivered" ? "badge badge-success" : "badge badge-warning" %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td><%# Eval("name") %></td>
                                                            <td><%# Eval("totalprice", "{0:C}") %></td>
                                                            <td><%# Eval("paymentmode") %></td>
                                                            <td>
                                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("orderdetailsid") %>' CssClass="btn btn-sm btn-warning" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Category List Card -->
                                <div class="col-sm-6 col-mb-4 col-lg-4">
                                    <div class="card">
                                        <asp:Panel ID="pupdateorderstatus" runat="server" Visible="false">
                                            <div class="card-header">
                                                <h4 class="sub-title">Update Status</h4>
                                            </div>
                                            <div class="card-body">
                                                <div class="form-group">
                                                    <label>Order Status</label>
                                                    <asp:DropDownList ID="ddlorderstatus" runat="server" CssClass="form-control text-white">
                                                        <asp:ListItem Value="0">Select Status</asp:ListItem>
                                                        <asp:ListItem>Pending</asp:ListItem>
                                                        <asp:ListItem>Dispatched</asp:ListItem>
                                                        <asp:ListItem>Delivered</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlorderstatus" runat="server" ForeColor="Red" ControlToValidate="ddlorderstatus"
                                                        ErrorMessage="Order Status is required" SetFocusOnError="true" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                                    <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                </div>
                                                <div class="form-group">
                                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
