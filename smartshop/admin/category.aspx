<%@ Page Title="" Language="C#" MasterPageFile="~/admin/index.Master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="smartshop.admin.category" %>

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
                    document.getElementById('<%=imgCategory.ClientID %>').src = e.target.result;
                    document.getElementById('<%=imgCategory.ClientID %>').style.display = 'block';
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
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
                                <!-- Category Form Card -->
                                <div class="col-sm-6 mb-3">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="sub-title">Category</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="form-group">
                                                <label for="txtname">Category Name</label>
                                                <asp:TextBox ID="txtname" runat="server" CssClass="form-control text-white" placeholder="Enter Category Name" required></asp:TextBox>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                            </div>
                                            <div class="form-group">
                                                <label for="fuCategoryImage">Category Image</label>
                                                <asp:FileUpload ID="fuCategoryImage" runat="server" CssClass="form-control" onchange="ImagePreview(this);" />
                                                <asp:HiddenField ID="hdnImagePath" runat="server" />
                                            </div>
                                            <div class="form-check form-group">
                                                <asp:CheckBox ID="cbIsactive" runat="server" CssClass="form-check-input" />
                                                <label class="form-check-label" for="cbIsactive">Is Active</label>
                                            </div>
                                            <div class="form-group">
                                                <asp:Button ID="btnAddorUpdate" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddorUpdate_Click" />
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnClear_Click" />
                                            </div>
                                            <div class="form-group">
                                                <asp:Image ID="imgCategory" runat="server" CssClass="img-thumbnail" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Category List Card -->
                                <div class="col-sm-6 mb-3">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="sub-title">Category List</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table-responsive">
                                                <asp:Repeater ID="rcategory" runat="server" OnItemCommand="rcategory_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered">
                                                            <thead>
                                                                <tr>
                                                                    <th>Name</th>
                                                                    <th>Image</th>
                                                                    <th>IsActive</th>
                                                                    <th>CreatedDate</th>
                                                                    <th>Action</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("name") %></td>
                                                            <td>
                                                                <asp:Image
                                                                    ID="imgCategory"
                                                                    runat="server"
                                                                    ImageUrl='<%# GetImageUrl(Eval("imageurl").ToString()) %>'
                                                                    AlternateText="Category Image"
                                                                    Width="90"
                                                                    Height="90"
                                                                    Style="background-color: transparent; object-fit: cover;" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblisactive" runat="server"
                                                                    CssClass='<%# Convert.ToBoolean(Eval("isactive")) ? "badge badge-success" : "badge badge-danger" %>'
                                                                    Text='<%# Convert.ToBoolean(Eval("isactive")) ? "Active" : "Inactive" %>' />
                                                            </td>
                                                            <td><%# Eval("createddate", "{0:MM/dd/yyyy}") %></td>
                                                            <td>
                                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("categoryid") %>' CssClass="btn btn-sm btn-warning" CausesValidation="false" />
                                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("categoryid") %>' CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this category?');" />
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
