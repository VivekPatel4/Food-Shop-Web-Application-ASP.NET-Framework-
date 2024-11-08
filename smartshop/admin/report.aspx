<%@ Page Title="" Language="C#" MasterPageFile="~/admin/index.Master" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="smartshop.admin.report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="pcoded-inner-content pt-0">
        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="container">
                        <div class="row">
                            <div class="col-sm-12 col-md-10 offset-md-1">
                                <div class="card">
                                    <div class="card-header">
                                        <h4 class="sub-title">Selling Report Filter</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="txtfromdate">From Date</label>
                                                    <asp:TextBox ID="txtfromdate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvfromdate" runat="server" ErrorMessage="*" ControlToValidate="txtfromdate" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" />
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="txttodate">To Date</label>
                                                    <asp:TextBox ID="txttodate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtodate" runat="server" ErrorMessage="*" ControlToValidate="txttodate" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 text-center">
                                                <asp:Button ID="btnsearch" runat="server" Text="Search" CssClass="btn btn-primary mt-3" OnClick="btnsearch_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="card mt-4">
                                    <div class="card-header">
                                        <h4 class="sub-title">Selling Report</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            <asp:Repeater ID="rreport" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-bordered">
                                                        <thead>
                                                            <tr>
                                                                <th>SrNo.</th>
                                                                <th>Full Name</th>
                                                                <th>Email</th>
                                                                <th>Item Orders</th>
                                                                <th>Total Cost</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("srno") %></td>
                                                        <td><%# Eval("name") %></td>
                                                        <td><%# Eval("email") %></td>
                                                        <td><%# Eval("totalorders") %></td>
                                                        <td><%# Eval("totalprice") %></td>
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

                                <div class="row">
                                    <div class="col-md-12 text-right">
                                        <asp:Label ID="lbltotal" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
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
