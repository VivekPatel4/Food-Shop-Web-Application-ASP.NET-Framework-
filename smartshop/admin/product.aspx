<%@ Page Title="" Language="C#" MasterPageFile="~/admin/index.Master" AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="smartshop.admin.product" %>

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
                    document.getElementById('<%=imgProduct.ClientID %>').src = e.target.result;
                    document.getElementById('<%=imgProduct.ClientID %>').style.display = 'block';
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
                                <!-- Product Form Card -->
                                <div class="col-sm-6 mb-3">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="sub-title">Product</h4>
                                        </div>
                                        <div class="card-body">
                                            <!-- Product Name -->
                                            <div class="form-group">
                                                <label for="txtname">Product Name</label>
                                                <asp:TextBox ID="txtname" runat="server" CssClass="form-control text-white" placeholder="Enter Product Name"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name is required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtname"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                            </div>
                                            <!-- Product Description -->
                                            <div class="form-group">
                                                <label for="txtdescription">Product Description</label>
                                                <asp:TextBox ID="txtdescription" runat="server" CssClass="form-control text-white" placeholder="Enter Product Description" TextMode="MultiLine"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Description is required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtdescription"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- Product Price -->
                                            <div class="form-group">
                                                <label for="txtprice">Product Price(₹)</label>
                                                <asp:TextBox ID="txtprice" runat="server" CssClass="form-control text-white" placeholder="Enter Product Price"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Price is required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtprice"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Price must be decimal" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtprice" ValidationExpression="^\d{0,8}(\.\d{1,4})?$"></asp:RegularExpressionValidator>
                                            </div>
                                            <!-- Product Quantity -->
                                            <div class="form-group">
                                                <label for="txtqunatity">Product Quantity</label>
                                                <asp:TextBox ID="txtqunatity" runat="server" CssClass="form-control text-white" placeholder="Enter Product Quantity"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Quantity is required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtqunatity"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Quantity must be decimal" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtqunatity" ValidationExpression="^([1-9]\d*|0)$"></asp:RegularExpressionValidator>
                                            </div>
                                            <!-- Product Image -->
                                            <div class="form-group">
                                                <label for="fuproductImage">Product Image</label>
                                                <asp:FileUpload ID="fuproductImage" runat="server" CssClass="form-control" onchange="ImagePreview(this);" />
                                                <asp:HiddenField ID="hdnImagePath" runat="server" />
                                            </div>
                                            <!-- Product Category -->
                                            <div class="form-group">
                                                <label for="ddlcategories">Product Category</label>
                                                <asp:DropDownList ID="ddlcategories" runat="server" CssClass="form-control text-white" DataSourceID="SqlDataSource1" DataTextField="name" DataValueField="categoryid" AppendDataBoundItems="True">
                                                    <asp:ListItem Value="0">Select category</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Category is required" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlcategories" InitialValue="0"></asp:RequiredFieldValidator>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:foodieDBConnectionString %>" SelectCommand="SELECT [categoryid], [name] FROM [categories]"></asp:SqlDataSource>
                                            </div>
                                            <!-- Is Active Checkbox -->
                                            <div class="form-check form-group">
                                                <asp:CheckBox ID="cbIsactive" runat="server" CssClass="form-check-input text-white" />
                                                <label class="form-check-label" for="cbIsactive">Is Active</label>
                                            </div>
                                            <!-- Action Buttons -->
                                            <div class="form-group">
                                                <asp:Button ID="btnAddorUpdate" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddorUpdate_Click" />
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" CausesValidation="false" OnClick="btnClear_Click" />
                                            </div>
                                            <!-- Product Image Preview -->
                                            <div class="form-group">
                                                <asp:Image ID="imgProduct" runat="server" CssClass="img-thumbnail" Style="display: none;" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Product List Card -->
                                <div class="col-sm-6 mb-3">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="sub-title">Product List</h4>
                                        </div>
                                        <div class="card-body">
                                            <div class="table-responsive">
                                                <asp:Repeater ID="rproduct" runat="server" OnItemCommand="rproduct_ItemCommand">
                                                    <HeaderTemplate>
                                                        <table class="table table-bordered text-white">
                                                            <thead>
                                                                <tr>
                                                                    <th>Name</th>
                                                                    <th>Image</th>
                                                                    <th>Price(₹)</th>
                                                                    <th>Qty</th>
                                                                    <th>Category</th>
                                                                    <th>IsActive</th>
                                                                    <th>Description</th>
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
                                                                    ID="imgProductList"
                                                                    runat="server"
                                                                    ImageUrl='<%# GetImageUrl(Eval("imageurl").ToString()) %>'
                                                                    AlternateText="Product Image"
                                                                    Width="90"
                                                                    Height="90"
                                                                    Style="background-color: transparent; object-fit: cover;" />
                                                            </td>
                                                            <td><%# Eval("price") %></td>

                                                            <td>
                                                                <asp:Label ID="Label1" runat="server"
                                                                    CssClass='<%# Convert.ToInt32(Eval("quantity")) <= 5 ? "badge badge-danger" : "" %>'
                                                                    Text='<%# Eval("quantity").ToString() + (Convert.ToInt32(Eval("quantity")) <= 5 ? " - Item about to be out of stock!" : "") %>' />
                                                            </td>


                                                            <td><%# Eval("categoryname") %></td>
                                                            <td>
                                                                <asp:Label ID="lblisactive" runat="server"
                                                                    CssClass='<%# Convert.ToBoolean(Eval("isactive")) ? "badge badge-success" : "badge badge-danger" %>'
                                                                    Text='<%# Convert.ToBoolean(Eval("isactive")) ? "Active" : "Inactive" %>' />
                                                            </td>

                                                            <td><%# Eval("description") %></td>
                                                            <td><%# Eval("createddate", "{0:MM/dd/yyyy}") %></td>
                                                            <td>
                                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("productid") %>' CssClass="btn btn-sm btn-warning" CausesValidation="false"/>
                                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("productid") %>' CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this product?');" />
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
</asp:Content>
