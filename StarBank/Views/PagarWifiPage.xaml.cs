using StarBank.Models;
using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;
using System.Net;
using System.Net.Mail;
using MauiPopup;
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
            //await DisplayAlert("ERROR", "Factura Pagado", "Aceptar");

        }
        else
        {
            if (SaldoActual < totalF)
            {
                await PopupAction.DisplayPopup(new PopUp.FondosIns());
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
                String nom = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Nombre", "Facturas");
                String NFac = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "N_Factrua", "Facturas");
                String Fecha = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Fecha", "Facturas");
                String Consumo = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Consumo", "Facturas");
                String Mora = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "Mora", "Facturas");
                String Montoconsumo = await conexionFirebase.ObtenerDatosID<Servicios>(IdFac, "MontoConsumo", "Facturas");
                string correo = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Correo", "Usuarios");

                await EnviarCorreoVerificacion(correo, NFac, nom, Fecha, Consumo, Mora, Montoconsumo, Total);

                await conexionFirebase.RegistrarTransacciones(Factura, "SERVICIO", fechaComoString, CunetaO, "ENEE", NombreO + "" + ApellidoO, "EMPRESA DE ENERGIA ELECTRICA");
                await PopupAction.DisplayPopup(new PopUp.Servicios());

            }
        }

        }


    public async Task EnviarCorreoVerificacion(string Email, string codigoFactura, string nombreCliente, string fechaVencimiento, string consumo, string montoMora, string montoConsumo, string totalPagar)
    {
        var fromAddress = new MailAddress("fabriciojosuegarciapena@gmail.com", "STARBANK");
        var toAddress = new MailAddress(Email, "Estimado/a Cliente");
        const string fromPassword = "oliswwttazemnlyn";
        const string subject = "Detalles de la Factura y Verificación";

        string body = "<html><head><style>" +
                      "body { font-family: Arial, sans-serif; }" +
                      "h2 { color: #007bff; }" +
                      "ul { list-style-type: none; padding: 0; }" +
                      "li { margin-bottom: 8px; }" +
                      ".total { font-weight: bold; }" +
                      ".signature { font-style: italic; font-size: 0.9em; color: #888; }" +
                      "</style></head><body>" +
                      "<h2>Estimado/a Cliente,</h2>" +
                       "<h3>Factura De WIFI/CABLE,</h3>" +
                      "<p>Adjunto encontrará los detalles de su factura junto con un código de verificación para garantizar la seguridad de la transacción.</p>" +
                      "<hr />" + // Línea divisiva
                      "<ul>" +
                      "<li><strong>Código de Factura:</strong> " + codigoFactura + "</li>" +
                      "<li><strong>Nombre:</strong> " + nombreCliente + "</li>" +
                      "<li><strong>Fecha de Vencimiento:</strong> " + fechaVencimiento + "</li>" +
                      "<li><strong>Consumo:</strong> " + consumo + "</li>" +
                      "<li><strong>Monto por Mora:</strong> $" + montoMora + "</li>" +
                      "<li><strong>Monto por Consumo:</strong> $" + montoConsumo + "</li>" +
                      "<li class='total'><strong>Total a Pagar:</strong> " + totalPagar + ".Lps</li>" +
                      "</ul>" +
                      "<hr />" + // Otra línea divisiva
                      "<p>Por favor, utilice este código al realizar el pago para verificar su transacción.</p>" +
                      "<p>Gracias por confiar en STARBANK.</p>" +
                      "<p class='signature'>Saludos cordiales,<br/>El equipo de STARBANK</p>" +
                      "</body></html>";

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true // Habilita el formato HTML en el cuerpo del correo
        })
        {
            await smtp.SendMailAsync(message);
        }
    }




}