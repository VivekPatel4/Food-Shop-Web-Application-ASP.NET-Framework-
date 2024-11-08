<%@ Page Title="" Language="C#" MasterPageFile="~/admin/index.Master" AutoEventWireup="true" CodeBehind="contacts.aspx.cs" Inherits="smartshop.admin.contacts" %>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="pcoded-inner-content pt-0">
        <div class="align-align-self-end">
            <asp:Label ID="lblMsg" runat="server" CssClass="align-align-self-end text-white" Visible="false"></asp:Label>
        </div>

        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">
                        <div class="container">
                            <div class="row">

                                <div class="col-sm-15 mb-3">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="sub-title">Contact List</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table-responsive">
                                                <asp:Repeater ID="rcontacts" runat="server" OnItemCommand="rcontacts_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered">
                                                            <thead>
                                                                <tr>
                                                                    <th>SrNo.</th>
                                                                    <th>User Name</th>
                                                                    <th>Email</th>
                                                                    <th>Subject</th>
                                                                    <th>Message</th>
                                                                    <th>Contact Date</th>
                                                                    <th>Delete</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("srno") %></td>
                                                            <td><%# Eval("name") %></td>
                                                            <td><%# Eval("email") %></td>
                                                            <td><%# Eval("subject") %></td>
                                                            <td><%# Eval("message") %></td>
                                                            <td><%# Eval("createddate", "{0:MM/dd/yyyy}") %></td>
                                                            <td>

                                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("contactid") %>' CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this Record?');" />
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
