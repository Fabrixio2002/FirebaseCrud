using Microsoft.Maui.Graphics.Text;
using StarBank.Models;

namespace StarBank.Views;

public partial class AggTarjetaPage : ContentPage
{
    private String ID;
    private String Cuenta;
    private String nombre;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public AggTarjetaPage(String Id)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        ID=Id;
        txt_cvv.TextChanged += Txt_cvv_TextChanged;//FORMARO DEL CVV

        txt_mm_aa.TextChanged += Txt_mm_aa_TextChanged;//FORMATO MM/AA
         
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new Views.TransferPage(ID));
    }


    private async void btn_savetarjeta_Clicked(object sender, EventArgs e)
    {
        // Verificar si algún campo está vacío
        if (string.IsNullOrWhiteSpace(txt_numTarjeta.Text) ||
            string.IsNullOrWhiteSpace(txt_mm_aa.Text) ||
            string.IsNullOrWhiteSpace(txt_cvv.Text) ||
            string.IsNullOrWhiteSpace(txt_nameTajeta.Text) ||
            string.IsNullOrWhiteSpace(txt_ciudad.Text) ||
            string.IsNullOrWhiteSpace(txt_direccion.Text) ||
            string.IsNullOrWhiteSpace(txt_montoAgg.Text))

        {
            // Mostrar alerta indicando campos vacíos
          await  DisplayAlert("Campos Vacíos", "Por favor, complete todos los campos.", "Aceptar");
        }
        else
        {
            DateTime fechaActual = DateTime.Now;
            string fechaComoString = fechaActual.ToString("dd/MM/yyyy HH:mm:ss"); // Formato personalizado de fecha y hora
            var nombreUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "Nombre", "Usuarios");
            var Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "N_Cuenta", "Usuarios");
            var Saldo = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "Saldo", "Usuarios");
            String monto = txt_montoAgg.Text;
            String Tarjeta=txt_numTarjeta.Text;
            String MTarjeta = txt_nameTajeta.Text;
            int Saldoactual = int.Parse(Saldo);
            int Deposito = int.Parse(monto);
            int  nuevosaldo = Saldoactual + Deposito;

            var IDTarjeta = await conexionFirebase.BuscarTarjeta(Tarjeta);
            var saldoString = await conexionFirebase.ObtenerDatosID<Tarjetas>(IDTarjeta, "Saldo", "Tarjetas");
            int saldoInt = Convert.ToInt32(saldoString);


            if (saldoInt < Deposito)
            {
                await DisplayAlert("ERROR" , "Fondos INSUFICIENTES", "Aceptar");

            }
            else
            {

                int NuevoS = saldoInt - Deposito;
               await conexionFirebase.ActualizarDatoTarjeta(IDTarjeta, "Saldo", NuevoS.ToString());
                await DisplayAlert("Listo", "TRANFERENCIA REALIZADA", "Aceptar");
                await conexionFirebase.RegistrarTransacciones(monto, "Tranferencia", fechaComoString, Tarjeta, Cuenta, txt_nameTajeta.Text, nombreUsuario);
                await conexionFirebase.ActualizarDatoUsuario(ID, "Saldo", nuevosaldo.ToString());

            }



        }


    }



    //FORMATO DEL CVV
    private void Txt_cvv_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Obtener el texto actual del Entry
        var text = ((Entry)sender).Text;

        // Quitar cualquier carácter que no sea dígito
        var digitsOnly = new string(text.Where(char.IsDigit).ToArray());

        // Si la longitud del texto es mayor que 3 (o 4), cortarlo
        if (digitsOnly.Length > 3)
        {
            digitsOnly = digitsOnly.Substring(0, 3);
        }

            // Asignar el texto limpio de vuelta al Entry
            ((Entry)sender).Text = digitsOnly;
    }

  

    //FORMATO DEL MM/AA
    private void Txt_mm_aa_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Obtener el texto actual del Entry
        var text = ((Entry)sender).Text;

        // Quitar cualquier carácter que no sea dígito
        var digitsOnly = new string(text.Where(char.IsDigit).ToArray());

        // Si la longitud del texto es mayor que 5, cortarlo
        if (digitsOnly.Length > 5)
        {
            digitsOnly = digitsOnly.Substring(0, 5);
        }

        // Insertar "/" después de los primeros 2 caracteres si aún no está presente
        if (digitsOnly.Length >= 2 && !digitsOnly.Contains("/"))
        {
            digitsOnly = digitsOnly.Insert(2, "/");
        }

            // Asignar el texto limpio de vuelta al Entry
            ((Entry)sender).Text = digitsOnly;
    }




  

}