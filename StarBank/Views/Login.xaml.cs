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
            DisplayAlert("Error", "Por favor, ingresa tu correo electrónico y contraseña.", "Aceptar");
        }
        else
        {
            // Aquí colocas la lógica para iniciar sesión
            // Por ejemplo, puedes llamar a un método que maneje la autenticación
            // this.IniciarSesion(email, password);

            //Navigation.PushAsync(new DashboardPage());//Para cambiar de Pantalla
        }
    }

    private void btn_registrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Registrar());//Para cambiar de Pantalla

    }
}