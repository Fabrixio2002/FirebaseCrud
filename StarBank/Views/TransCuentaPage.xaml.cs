using StarBank.Models;

namespace StarBank.Views;

public partial class TransCuentaPage : ContentPage
{
    private String identificador;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public TransCuentaPage(String id)
    {
        identificador = id;
        ObtenerCuenta(id);
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
    }

    //BOTON DE ATRAS
    private async void OnImageTapped(object sender, EventArgs e)
    {
        //Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new TransferPage(identificador));
    }

    private async void btn_transCuenta_Clicked(object sender, EventArgs e)
    {
        // Verificar si algún campo está vacío
        if (string.IsNullOrWhiteSpace(txt_Monto.Text) ||
            string.IsNullOrWhiteSpace(txt_cuentaOrigen.Text) ||
            string.IsNullOrWhiteSpace(txt_nameOrigen.Text) ||
            string.IsNullOrWhiteSpace(txt_cuentaDestino.Text) ||
            string.IsNullOrWhiteSpace(txt_nameDestino.Text) ||
            string.IsNullOrWhiteSpace(txt_descripcion.Text))
        {
            // Mostrar alerta indicando campos vacíos
           await DisplayAlert("Campos Vacíos", "Por favor, complete todos los campos.", "Aceptar");
        }
        else
        {
            var CuentaD = txt_cuentaDestino.Text;
            var CuentaO = txt_cuentaOrigen.Text; 
            int Monto = Convert.ToInt32(txt_Monto.Text);
            var MointoBD = Monto.ToString();
            String IDBenficiario = await conexionFirebase.BuscarIDEnBaseDeDatos(CuentaD);
            string NombreO = await conexionFirebase.ObtenerDatosID<Usuarios>(identificador, "Nombre", "Usuarios");
            string ApellidoO = await conexionFirebase.ObtenerDatosID<Usuarios>(identificador, "Apellidos", "Usuarios");
            string NombreD = await conexionFirebase.ObtenerDatosID<Usuarios>(IDBenficiario, "Nombre", "Usuarios");
            string ApellidoD = await conexionFirebase.ObtenerDatosID<Usuarios>(IDBenficiario, "Apellidos", "Usuarios");
            var saldoString = await conexionFirebase.ObtenerDatosID<Usuarios>(identificador, "Saldo", "Usuarios");
            var saldoString2 = await conexionFirebase.ObtenerDatosID<Usuarios>(IDBenficiario, "Saldo", "Usuarios");
           
            int SaldoActual2 = Convert.ToInt32(saldoString2);

            int SaldoActual = Convert.ToInt32(saldoString);

            if (SaldoActual<Monto) {
                await DisplayAlert("ERROR", "FONDOS INSUFICIENTES", "Aceptar");
                return;
            }
            else
            {
                int SaldoOrigen = SaldoActual - Monto;
                int SaldoBeneficiario = SaldoActual2 + Monto;
                await conexionFirebase.ActualizarDatoUsuario(identificador, "Saldo", SaldoOrigen.ToString());
                await conexionFirebase.ActualizarDatoUsuario(IDBenficiario, "Saldo", SaldoBeneficiario.ToString());
                DateTime fechaActual = DateTime.Now.Date;
                string fechaComoString = fechaActual.ToString("dd/MM/yyyy"); // Formato personalizado de fecha y hora

                await conexionFirebase.RegistrarTransacciones(MointoBD, "Transferencia", fechaComoString, CuentaO, CuentaD, NombreO+""+ApellidoO, NombreD+""+ApellidoD);

                await DisplayAlert("Listo", "Transferencia realizada correctamente.", "Aceptar");
                txt_cuentaDestino.Text = string.Empty;
                txt_nameDestino.Text = "Destinatario: --";
                txt_Monto.Text = string.Empty;
                txt_descripcion.Text = string.Empty;

            }




            // Todos los campos están llenos, continuar con la lógica de envío de transferencia
            // Aquí puedes agregar el código para realizar la transferencia
        }
    }

    private async void btn_verificar_Clicked(object sender, EventArgs e)
    {
        try
        {
            var CuentaEnvio = txt_cuentaDestino.Text;
            String IDBenficiario = await conexionFirebase.BuscarIDEnBaseDeDatos(CuentaEnvio);

            if (string.IsNullOrEmpty(IDBenficiario)) // Validación del ID del beneficiario
            {
                await DisplayAlert("ERROR", "CUENTA NO ENCONTRADA", "Aceptar");
                return;
            }

            string Nombre = await conexionFirebase.ObtenerDatosID<Usuarios>(IDBenficiario, "Nombre", "Usuarios");
            string Apellido = await conexionFirebase.ObtenerDatosID<Usuarios>(IDBenficiario, "Apellidos", "Usuarios");

            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Apellido)) // Verificar si el nombre o el apellido están vacíos
            {
                await DisplayAlert("ERROR", "Nombre No Encontra", "Aceptar");
                return;
            }

            txt_nameDestino.Text = Nombre + " " + Apellido;
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "Ocurrió un error: " + ex.Message, "Aceptar");
            return;
        }


    }

    private async void ObtenerCuenta(string userString)
    {
        string Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "N_Cuenta", "Usuarios");
        string Nombre = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Nombre", "Usuarios");
        string Apellido = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Apellidos", "Usuarios");

        txt_cuentaOrigen.Text = Cuenta;
        txt_nameOrigen.Text = Nombre+Apellido;

    }


}