namespace StarBank.Views;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Maui.ApplicationModel.Communication;

public partial class Login : ContentPage
{
    string email;
    public Login()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR

    }

    private async void btn_iniciar_Clicked(object sender, EventArgs e)
    {
        ConexionFirebase conexionFirebase = new ConexionFirebase();
         email = txt_email.Text;
        string password = txt_password.Text;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
           await DisplayAlert("Error", "Por favor, ingresa tu correo electrónico y contraseña.", "Aceptar");
        }
        else 
        {
            var credenciales = await conexionFirebase.InicioSesion(email, password);


        }
    }

    private void btn_registrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Registrar());//Para cambiar de Pantalla

    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        String gmail = txt_email.Text;
        ConexionFirebase conexionFirebase = new ConexionFirebase();

         await conexionFirebase.ContraseñaNueva(gmail);
        await DisplayAlert("Restablecer Contraseña", "", "");

    }
}