using Firebase.Database;
using Microsoft.Maui.Storage;
using StarBank.Models;

namespace StarBank.Views;

public partial class DashboardPage : ContentPage
{
    ConexionFirebase conexionFirebase = new ConexionFirebase();
 
    private string ID;

    public  DashboardPage(String userString)
    {
        ID = userString;
        InicializarBienvenidaAsync(userString);
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
      

    }


    private void btn_administrar_Clicked(object sender, EventArgs e)
    {

    }

    private  void btn_pagar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ServiciosPage(ID));//Para cambiar de Pantalla

    }

    private async void btn_transferencias_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new TransferPage(ID));
    }

    private async  void btn_control_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VerEventosPage());

    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        ConexionFirebase conexionFirebase = new ConexionFirebase();
        await conexionFirebase.CerrarSesion();

    }

    private async void InicializarBienvenidaAsync(string userString)
    {
        var nombreUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Nombre", "Usuarios");
        var ApellidoUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Apellidos", "Usuarios");
        var Saldo = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Saldo", "Usuarios");
        var cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "N_Cuenta", "Usuarios");
        
        txtCuenta.Text = cuenta;
        saldoActual.Text = "L." + Saldo;
        txtBienvenida.Text = "Bienvenido " + nombreUsuario + " " + ApellidoUsuario;
    }



}