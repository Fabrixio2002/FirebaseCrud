namespace StarBank.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
    }

    private void btn_iniciar_Clicked(object sender, EventArgs e)
    {
        string email = txt_email.Text;
        string password = txt_password.Text;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            DisplayAlert("Error", "Por favor, ingresa tu correo electr�nico y contrase�a.", "Aceptar");
        }
        else
        {
            // Aqu� colocas la l�gica para iniciar sesi�n
            // Por ejemplo, puedes llamar a un m�todo que maneje la autenticaci�n
            // this.IniciarSesion(email, password);

            //Navigation.PushAsync(new DashboardPage());//Para cambiar de Pantalla
        }
    }

    private void btn_registrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Registrar());//Para cambiar de Pantalla

    }
}