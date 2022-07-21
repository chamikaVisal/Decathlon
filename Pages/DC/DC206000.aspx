<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="DC206000.aspx.cs" Inherits="Page_DC206000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PX.Objects.DC.CmpeInventoryMaint"
        PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule ControlSize="M" LabelsWidth="M" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
            <px:PXSelector runat="server" ID="PXSelector1" DataField="PartID" AutoRefresh="true" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="CstPXSelector1" DataField="WarehouseID" AutoRefresh="true" CommitChanges="true"></px:PXSelector>
            <px:PXSelector runat="server" ID="CstPXSelector4" DataField="LocationID" AutoRefresh="true" CommitChanges="true"></px:PXSelector>
            <px:PXLayoutRule ControlSize="M" LabelsWidth="M" ID="PXLayoutRule2" runat="server" StartColumn="True"></px:PXLayoutRule>
        </Template>
    </px:PXFormView>
   
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="Locations">
                <Template>
                    <px:PXGrid SyncPosition="True" SkinID="Details" Width="100%" runat="server" ID="CstPXGrid5" OnRowDataBound ="INStatusRecords_RowDataBound">
                        <Levels>
                            <px:PXGridLevel DataMember="INStatusRecords">
                                <Columns>
                                     <px:PXGridColumn DataField="InventoryStatusID" Width="280"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="PartID" Width="280"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="WarehouseID" Width="70"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="LocationID" Width="280"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="Quantity" DataType="Decimal"></px:PXGridColumn>
                                </Columns>
                              
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Enabled="True"></AutoSize>
                        <Mode InitNewRow="True"></Mode>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
    </px:PXTab>
</asp:Content>
