using Firebase.Auth;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
namespace StarBank.Views;

public partial class Registrar : ContentPage
{

	public Registrar()
	{

		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR

    }



    private void irLogin(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Login());//Para cambiar de Pantalla

    }

    private async void btn_registrarcuenta_Clicked(object sender, EventArgs e)
    {
        // ConexionFirebase conexionFirebase = new ConexionFirebase();
        //  string correo = txt_emailR.Text;
        // string contrasenia = txt_passwordR.Text;

        //var credenciales = await conexionFirebase.CrearUsuario(correo, contrasenia);





    }





}