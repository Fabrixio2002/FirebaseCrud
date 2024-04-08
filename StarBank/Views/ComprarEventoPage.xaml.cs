
using StarBank.Models;
using System.Net.Mail;
using System.Net;
using MauiPopup;


namespace StarBank.Views;

public partial class ComprarEventoPage : ContentPage
{
    public String des;
    public String FECHA;
    public String TITULO;
    public String IMAGEN;
    public String PRECIO;
    public String DIRECCION;
    public String id;
    public String url;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public ComprarEventoPage(String ID, string descripcion, String titulo, String direccion, String fecha, String imagen, String precio)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        des = descripcion;
        id = ID;
        FECHA = fecha;
        TITULO = titulo;
        IMAGEN = imagen;
        PRECIO = precio;
        DIRECCION = direccion;
        url = imagen;
        Nom.Text = "Descripcion: " + des;
        Title.Text = "EVENTO: " + TITULO;
        direc.Text = "Lugar: " + DIRECCION;
        prec.Text = "Precio: " + PRECIO + " Lps. ";
        fechita.Text = FECHA;

        // Crear una fuente de imagen URI con la URL
        ImageSource imagenSource = ImageSource.FromUri(new Uri(url));

        // Asignar la fuente de imagen al control Image
        foto.Source = imagenSource;


        // Suscribirse al evento TextChanged del Entry
        txt_cantidad.TextChanged += Txt_cantidad_TextChanged;

        // Calcular el total inicial
        CalcularTotal();
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new VerEventosPage(id));
    }

    private async void btn_comprar_Clicked(object sender, EventArgs e)
    {

        if (string.IsNullOrWhiteSpace(txt_cantidad.Text))
        {
            // Mostrar un mensaje de error o realizar alguna acción según tus necesidades
            await PopupAction.DisplayPopup(new PopUp.AlertError()); 
            return;
        }
        var saldoString = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Saldo", "Usuarios");
        int SaldoActual = Convert.ToInt32(saldoString);
        string cantidadTexto = txt_cantidad.Text;
        int cantidad = int.Parse(cantidadTexto);
        Double PRecio = double.Parse(PRECIO);
        Double tot = PRecio * cantidad;
        double SaldoOrigen = SaldoActual - tot;
     
        if (SaldoActual < tot)
        {
            await PopupAction.DisplayPopup(new PopUp.FondosIns());
            return;
        }
        else
        {
            String totalentradas = tot.ToString();
            await conexionFirebase.ActualizarDatoUsuario(id, "Saldo", SaldoOrigen.ToString());
            DateTime fechaActual = DateTime.Now.Date;
            string fechaComoString = fechaActual.ToString("dd/MM/yyyy"); // Formato personalizado de fecha y hora
            string CunetaO = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "N_Cuenta", "Usuarios");
            string NombreO = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Nombre", "Usuarios");
            string ApellidoO = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Apellidos", "Usuarios");
            string correo = await conexionFirebase.ObtenerDatosID<Usuarios>(id, "Correo", "Usuarios");
            await conexionFirebase.RegistrarTransacciones(totalentradas, "EVENTO", fechaComoString, CunetaO, "EVENTOS", NombreO + "" + ApellidoO, TITULO);
         
            await EnviarCorreoVerificacion(correo,TITULO, cantidadTexto,FECHA, PRECIO, totalentradas);
            await PopupAction.DisplayPopup(new PopUp.Servicios());

        }


    }

    private void Txt_cantidad_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Calcular el total cada vez que cambie el texto en el Entry
        CalcularTotal();
    }

    private void CalcularTotal()
    {
        string cantidadTexto = txt_cantidad.Text;
        if (!string.IsNullOrEmpty(cantidadTexto))
        {
            int  cantidad = int.Parse(cantidadTexto);
            Double PRecio = double.Parse(PRECIO);
            Double tot = PRecio * cantidad;
            String totalText = "0";
            totalText = tot.ToString();

            total.Text = "Total a pagar: " + totalText + " .Lps";
        }
        else
        {
            total.Text = "Ingrese una cantidad";
        }

      
    }



    public async Task EnviarCorreoVerificacion(string email, string tituloEvento, String totalEntradas, string fecha, string precioTicket,String total)
    {
        var fromAddress = new MailAddress("fabriciojosuegarciapena@gmail.com", "STARBANK");
        var toAddress = new MailAddress(email, "Estimado/a Cliente");
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
                      "<h3>Detalles del Evento y Verificación</h3>" +
                      "<p>Adjunto encontrará los detalles de su reserva junto con un código de verificación para garantizar la seguridad de la transacción.</p>" +
                      "<hr />" + // Línea divisiva
                      "<ul>" +
                      "<li><strong>Evento:</strong> " + tituloEvento + "</li>" +
                      "<li><strong>Total de Entradas:</strong> " + totalEntradas + "</li>" +
                      "<li><strong>Fecha:</strong> " + fecha + "</li>" +
                      "<li><strong>Precio por Entrada:</strong> $" + precioTicket + "</li>" +
                      "<li class='total'><strong>Total a Pagar:</strong> $" + (total) + ".00</li>" +
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