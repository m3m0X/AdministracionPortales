<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="IngresoRetencion.aspx.cs" Inherits="PortalTrabajadores.Portal.IngresoRetencion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Cargue de Certificados de Ingresos y Retenciones</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">Archivo:</td>
                        <td class="BotonTablaDatos">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">Empresa:</td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="EmpresaSeleccionada" runat="server">
                                <asp:ListItem Value="SS">Sumiservis</asp:ListItem>
                                <asp:ListItem Value="ST">Sumitemp</asp:ListItem>
                                <asp:ListItem Value="AE">Aliados Estratégicos</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos" colspan="2">
                            <asp:Button ID="btnSubirArchivo" runat="server" Text="Cargar información" OnClick="btnSubirArchivo_Click" />  
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos" colspan="2">
                            <asp:Button ID="btnNuevoCargue" runat="server" Text="Realizar nuevo cargue" OnClick="btnNuevoCargue_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubirArchivo" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Errores" runat="server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:Label ID="LblMsj" runat="server" Text="LabelMsjError" Visible="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
