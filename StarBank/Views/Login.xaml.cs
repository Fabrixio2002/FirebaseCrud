namespace StarBank.Views;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Microsoft.Maui.ApplicationModel.Communication;

public partial class Login : ContentPage
{
    ConexionFirebase conexionFirebase = new ConexionFirebase();
    string email;
    public Login()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
                                                        // Obtener el último correo electrónico utilizado para iniciar sesión desde las preferencias
        string lastLoggedInUser = Preferences.Get("LastLoggedInUser", string.Empty);

        // Mostrar el último correo electrónico en el campo de texto
        txt_email.Text = lastLoggedInUser;
    }

    private async void btn_iniciar_Clicked(object sender, EventArgs e)
    {
         email = txt_email.Text;
        string password = txt_password.Text;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
           await DisplayAlert("Error", "Por favor, ingresa tu correo electrónico y contraseña.", "Aceptar");
        }
        else 
        {
            var credenciales = await conexionFirebase.InicioSesion(email, password);
            txt_email.Text = "";
            txt_password.Text = "";
            Preferences.Set("LastLoggedInUser", email);

        }
    }

    private void btn_registrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Registrar());//Para cambiar de Pantalla


    }


    //restablecer contra
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

        String gmail = txt_email.Text;
        if(gmail==null)
        {
            await DisplayAlert("StarBank", "Ingrese un Correo Valido", "OK");
        }
        else
        {
            await conexionFirebase.ContraseñaNueva(gmail);
            await DisplayAlert("Restablecer Contraseña", "", "ok");
        }
     
        

    }
}