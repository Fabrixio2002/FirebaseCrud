using StarBank.Models;

namespace StarBank.Views;

public partial class ServiciosPage : ContentPage
{
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    private String id;
	public ServiciosPage(String ID)
	{
        id= ID;
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        InicializarBienvenidaAsync(id);
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new Views.DashboardPage(id));
    }

    private void btn_luz_Clicked(object sender, EventArgs e)
    {
       Navigation.PushAsync(new PagarLuzPage(id));//Para cambiar de Pantalla

    }

    private void btn_agua_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PagarAguaPage(id));//Para cambiar de Pantalla
    }

    private void btn_internet_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PagarWifiPage(id));//Para cambiar de Pantalla

    }

    private void btn_eventos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VerEventosPage());//Para cambiar de Pantalla

    }

    private async void InicializarBienvenidaAsync(string userString)
    {
        var Saldo = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Saldo", "Usuarios");
        var Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "N_Cuenta", "Usuarios");

        cuenta.Text = Cuenta;
        saldo.Text = "L." + Saldo;
    }
}