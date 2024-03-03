using Firebase.Database;
using Microsoft.Maui.Storage;
using StarBank.Models;

namespace StarBank.Views;

public partial class DashboardPage : ContentPage
{
    ConexionFirebase conexionFirebase = new ConexionFirebase();
    private String Usuario;
    private String Apellido;
    private String Cuenta;
    private String dinero;
    private String ID;
    public DashboardPage(String userString, String name,String apellido,String cuenta,String saldo)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        Usuario = name;
        Apellido = apellido;
        Cuenta = cuenta;
        dinero = saldo;
        ID = userString;
        txtBienvenida.Text = "Bienvenido " +Usuario+""+Apellido;
        txtCuenta.Text = cuenta;
        saldoActual.Text= "L."+saldo;

    }


    private void btn_administrar_Clicked(object sender, EventArgs e)
    {

    }

    private async void btn_pagar_Clicked(object sender, EventArgs e)
    {
         await conexionFirebase.ActualizarDatoUsuario(ID, "Saldo", "700");
       

    }

    private void btn_transferencias_Clicked(object sender, EventArgs e)
    {

    }

    private void btn_control_Clicked(object sender, EventArgs e)
    {

    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        ConexionFirebase conexionFirebase = new ConexionFirebase();
        await conexionFirebase.CerrarSesion();

    }



}