<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master"
    AutoEventWireup="true" ValidateRequest="false" CodeFile="DC201000.aspx.cs" Inherits="Page_DC201000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PX.Objects.DC.CmpePartMaint"
        PrimaryView="Parts">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Parts" Width="100%" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule LabelsWidth="S" ControlSize="M" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
            <px:PXMaskEdit runat="server" ID="CstPXMaskEdit2" DataField="PartCD" CommitChanges="true"></px:PXMaskEdit>
            <px:PXTextEdit runat="server" ID="CstPXTextEdit1" DataField="Description"></px:PXTextEdit>
            <px:PXLayoutRule runat="server" ID="PXLayoutRule2" StartColumn="True" LabelsWidth="S" ControlSize="M" />
            <px:PXDropDown runat="server" ID="CstPXDropDown3" DataField="ItemType" CommitChanges="true"></px:PXDropDown>
            <px:PXDropDown runat="server" ID="CstPXTextEdit3" DataField="Type"></px:PXDropDown>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit1" DataField="Price" CommitChanges="true"></px:PXNumberEdit>
        </Template>

        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXFormView>
</asp:Content>
