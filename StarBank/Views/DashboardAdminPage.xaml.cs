namespace StarBank.Views;

public partial class DashboardAdminPage : ContentPage
{
    public DashboardAdminPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR

    }

    private void btn_gestionarEventos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.EventoPage());//Para cambiar de Pantalla

    }

    private void btn_historial_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AdministraBase());//Para cambiar de Pantalla
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new Views.Login());
    }
}