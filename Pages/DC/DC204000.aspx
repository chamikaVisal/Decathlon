<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="DC204000.aspx.cs" Inherits="Page_DC204000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PX.Objects.DC.CmpeDIReceiptMaint"
        PrimaryView="StatusDetails">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="StatusDetails">
                <Columns>
                   <px:PXGridColumn Type="CheckBox" AllowCheckAll="True" TextAlign="Center" DataField="Selected" Width="60"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PartID" Width="140"/>
                    <px:PXGridColumn DataField="LocationID" Width="140" />
                    <px:PXGridColumn DataField="WarehouseID" Width="140" />
                    <px:PXGridColumn DataField="Quantity" Width="70"/>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
    </px:PXGrid>
</asp:Content>