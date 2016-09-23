<%@ Page Title="" Language="C#" MasterPageFile="~/Portal/PaginaMaestra.Master" AutoEventWireup="true" CodeBehind="Modulos.aspx.cs" Inherits="PortalTrabajadores.Portal.Modulos" %>
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
                        <th colspan="2">Activación de modulos Objetivos/Competencias</th>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblNit" runat="server" Text="Digite Nit de la empresa:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:TextBox ID="txtNit" runat="server" MaxLength="20" CssClass="MarcaAgua"
                                ToolTip="Usuario" placeholder="Nit"
                                onkeypress="return ValidaSoloNumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="CeldaTablaDatos">
                            <asp:Label ID="lblEmpresa" runat="server" Text="Seleccione la empresa:" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:DropDownList ID="ddlEmpresa" runat="server">
                                <asp:ListItem Value="ST">Sumitemp</asp:ListItem>
                                <asp:ListItem Value="SB">Sumitemp Bucaramanga</asp:ListItem>
                                <asp:ListItem Value="SS">Sumiservis</asp:ListItem>
                                <asp:ListItem Value="AE">Aliados Estrategicos</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnBuscar" runat="server" 
                                Text="Buscar Nit" OnClick="BtnBuscar_Click" />
                        </td>
                        <td class="BotonTablaDatos">
                            <asp:Button ID="BtnRestablecer" runat="server" 
                                Text="Buscar nuevamente" OnClick="BtnRestablecer_Click"
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel2" runat="server" visible ="false">
                <table id="TablaDatos">
                    <tr>
                        <th colspan="2">Seleccione el Proyecto</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="DropListProyecto" runat="server" Height="22px" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                         <td>
                            <asp:Button ID="BtnProyectos" runat="server" 
                                Text="Seleccionar" OnClick="BtnProyectos_Click"/>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Container_UpdatePanel3" runat="server" visible ="false">
                <br />
                <asp:GridView ID="gvModulosActivos" runat="server" 
                    AutoGenerateColumns="False" OnRowDataBound="gvModulosActivos_RowDataBound"
                    OnRowCommand="gvModulosActivos_RowCommand">
                    <AlternatingRowStyle CssClass="ColorOscuro" />
                    <Columns>
                        <asp:BoundField DataField="Nombre_Modulo" HeaderText="Modulos" />
                        <asp:BoundField DataField="ModActivo" HeaderText="Estado" Visible="false"/>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnON" runat="server" 
                                    ImageUrl="~/Img/on.png" CommandArgument='<%#Eval("idMatriz") + ";" + Eval("idModulos")%>' 
                                    CommandName="On" />
                                <asp:ImageButton ID="btnOFF" runat="server" 
                                    ImageUrl="~/Img/off.png" CommandArgument='<%#Eval("idMatriz") + ";" + Eval("idModulos")%>' 
                                    CommandName="Off" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="BtnGuardar" runat="server" Text="Guardar"/>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnProyectos" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="BtnGuardar" EventName="Click" />
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
