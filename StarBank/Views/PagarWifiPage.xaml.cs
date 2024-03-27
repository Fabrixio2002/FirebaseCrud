using StarBank.Models;

namespace StarBank.Views;

public partial class PagarWifiPage : ContentPage
{
    private String id;
    private String Total;
    private String estado;
    private String IdFac;
    ConexionFirebase conexionFirebase = new ConexionFirebase();
    public PagarWifiPage(String ID)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        id = ID;

    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new ServiciosPage(id));
    }

    private async  void btn_buscar_Clicked(object sender, EventArgs e)
    {
        var Factura = txt_codeFactura.Text;
        IdFac = await conexionFirebase.BuscarFactura(Factura);
        String nom = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Nombre", "Facturas");
        String NFac = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "N_Factrua", "Facturas");
        String Fecha = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Fecha", "Facturas");
        String Consumo = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Consumo", "Facturas");
        String Mora = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Mora", "Facturas");
        String Montoconsumo = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "MontoConsumo", "Facturas");
        Total = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "TotalPagar", "Facturas");
        estado = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Estado", "Facturas");

        mora.Text = "Monto Por Mora: " + Mora;
        consumo.Text = "Consumo KW: " + Consumo;
        fecha.Text = "Fecha Vencimiento: " + Fecha;
        name.Text = "Nombre: " + nom;
        codeFact.Text = "Código Factura: " + NFac;
        Mconsumo.Text = "Monto Por Consumo: " + Montoconsumo;
        total.Text = "Total a Pagar: " + Total;

    }

    private async  void btn_pagarLuz_Clicked(object sender, EventArgs e)
    {
        var Factura = Total;
        int totalF = Convert.ToInt32(Factura);
        //conexionFirebase.Registrar("123456789", "EDILZON PALACIOS", "30/03/04", "25kw", "0", "5000", "5000","Pendiente");
        var saldoString = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Saldo", "Usuarios");
        int SaldoActual = Convert.ToInt32(saldoString);
        if (estado == "Pagado")
        {
            await DisplayAlert("ERROR", "Factura Pagado", "Aceptar");

        }
        else
        {
            if (SaldoActual < totalF)
            {
                await DisplayAlert("ERROR", "FONDOS INSUFICIENTES", "Aceptar");
                return;
            }
            else
            {
                int SaldoOrigen = SaldoActual - totalF;
                await conexionFirebase.ActualizarDatoUsuario(id, "Saldo", SaldoOrigen.ToString());

                DateTime fechaActual = DateTime.Now.Date;
                string fechaComoString = fechaActual.ToString("dd/MM/yyyy"); // Formato personalizado de fecha y hora
                string CunetaO = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "N_Cuenta", "Usuarios");
                string NombreO = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Nombre", "Usuarios");
                string ApellidoO = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Apellidos", "Usuarios");


                await conexionFirebase.RegistrarTransacciones(Factura, "SERVICIO", fechaComoString, CunetaO, "ENEE", NombreO + "" + ApellidoO, "EMPRESA DE ENERGIA ELECTRICA");
                await DisplayAlert("Listo", "Factura de Energia Electrica Pagada con exito", "Aceptar");

            }
        }

        }
}