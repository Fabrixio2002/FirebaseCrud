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
        Navigation.PushAsync(new Views.AddEventoPage());//Para cambiar de Pantalla

    }

    private void btn_historial_Clicked(object sender, EventArgs e)
    {

    }
}