<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="ModificarProyecto.aspx.cs" Inherits="PortalTrabajadores.Portal.ModificarProyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Css para la fecha -->
    <link href="../CSS/CSSCallapsePanel.css" rel="stylesheet" type="text/css" />
    <!-- Js De Los campos de Textos -->
    <script src="../Js/funciones.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContainerTitulo" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTitulo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Container" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="Container_UpdatePanel1">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Módulo de Consulta Proyectos</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblTemp" runat="server" Text="Seleccione Temporal:" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="DropListTemp" runat="server">
                                <asp:ListItem Value="AE">Aliados</asp:ListItem>
                                <asp:ListItem Value="ST">Sumitemp</asp:ListItem>
                                <asp:ListItem Value="SB">Sumitemp Bucaramanga</asp:ListItem>
                                <asp:ListItem Value="SS">Sumiservis</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblCodProyecto" runat="server" Text="Digite Código Proyecto:" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="txtuser" runat="server" MaxLength="15" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Código" onkeypress="return soloLetrasYNum(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos"></td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar Proyecto" OnClick="BtnBuscar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible="false">
                <table id="TablaDatos2">
                    <tr>
                        <th colspan="2">Información del Proyecto</th>
                    </tr>

                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblCodigoProy" runat="server" Text="Código Proyecto:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtCodigoProy" runat="server" Enabled="false" MaxLength="10" /></td>
                    </tr>

                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblCodTemporal" runat="server" Text="Código Temporal:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtCodTemporal" runat="server" MaxLength="2" Enabled="false" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase" />
                        </td>
                    </tr>

                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblNomProyecto" runat="server" Text="Descripción Proyecto:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtNomProyecto" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase"/></td>
                    </tr>

                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblDesc2Proyecto" runat="server" Text="Descripción Proyecto 2:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtDesc2Proyecto" runat="server" MaxLength="100" onkeypress="return ValidaSoloLetras(event)" Style="text-transform: uppercase" /></td>
                    </tr>

                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblUseDescripcio2" runat="server" Text="Utilizar Descripción 2:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="DropMarcacionProy" runat="server">
                                <asp:ListItem Value="0">Inactivo</asp:ListItem>
                                <asp:ListItem Value="1">Activo</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblActivoProy" runat="server" Text="Proyecto Activo:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:DropDownList ID="DropActivoProy" runat="server">
                                <asp:ListItem Value="0">Inactivo</asp:ListItem>
                                <asp:ListItem Value="1">Activo</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblNitTercero" runat="server" Text="Nit del Tercero(Asociar):" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtNitTercero" runat="server" MaxLength="15" />
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                Enabled="True" TargetControlID="TxtNitTercero" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblDir" runat="server" Text="Dirección Proyecto:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtDir" runat="server" MaxLength="100" Style="text-transform: uppercase" /></td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="LblTelefono" runat="server" Text="Telefono Proyecto:" /></td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtTelefono" runat="server" MaxLength="10" />
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                Enabled="True" TargetControlID="TxtTelefono" FilterType="Numbers" />
                        </td>
                    </tr>
                    <tr class="ColorOscuro">
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="Lblcorreo" runat="server" Text="Correo Proyecto:" />
                        </td>
                        <td class="CeldaTablaDatos">
                            <asp:TextBox ID="TxtCorreo" runat="server" MaxLength="50" /><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtCorreo" ErrorMessage="*Ingrese un correo válido" ForeColor="#CC3300" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnEditar" runat="server" Text="Guardar Datos" ValidationGroup="form" OnClick="BtnEditar_Click" /></td>
                        <td class="BotonTablaDatos"></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
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
