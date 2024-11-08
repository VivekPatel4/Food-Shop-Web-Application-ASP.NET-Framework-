<%@ Page Title="" Language="C#" MasterPageFile="~/user/index.Master" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="smartshop.user.contact" %>

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
                <h2>Send Your Query</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">

                        <div>
                            <asp:TextBox ID="txtname" runat="server" CssClass="form-control" placeholder="Your Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvname" runat="server" ErrorMessage="Name is Required" ControlToValidate="txtname"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" placeholder="Your Email" TextMode="Email"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvemail" runat="server" ErrorMessage="Email is Required" ControlToValidate="txtemail"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:TextBox ID="txtsubject" runat="server" CssClass="form-control" placeholder="subject"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvsubject" runat="server" ErrorMessage="subject is Required" ControlToValidate="txtsubject"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:TextBox ID="txtmessage" runat="server" CssClass="form-control" placeholder="Enter your query/Feedback"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvmessage" runat="server" ErrorMessage="Message is Required" ControlToValidate="txtmessage"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                        </div>
                       
                        <div class="btn_box">
                            <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="btn btn-warning rounded-pill pl-4 pr-4 text-white"
                              OnClick="btnsubmit_Click"  />
                        </div>

                    </div>
                </div>
                <div class="col-md-6">
                    <div class="map_container ">
                        <div id="googleMap"></div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
