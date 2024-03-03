using Firebase.Database;
using Microsoft.Maui.Storage;
using StarBank.Models;

namespace StarBank.Views;

public partial class DashboardPage : ContentPage
{
    ConexionFirebase conexionFirebase = new ConexionFirebase();
    private String Usuario;
    private String ID;
    public  DashboardPage(String Nombre)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        Usuario = Nombre;
    }


    private void btn_administrar_Clicked(object sender, EventArgs e)
    {

    }

    private void btn_pagar_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("", ""+ Usuario, "xd");
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