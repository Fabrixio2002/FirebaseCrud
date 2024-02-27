namespace StarBank.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
    }


    private void btn_administrar_Clicked(object sender, EventArgs e)
    {

    }

    private void btn_pagar_Clicked(object sender, EventArgs e)
    {

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