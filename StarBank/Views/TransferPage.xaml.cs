using StarBank.Models;

namespace StarBank.Views;

public partial class TransferPage : ContentPage
{
    private String ID;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public TransferPage(String id)
    {
        ID = id;
        InicializarBienvenidaAsync(ID);
        
        InitializeComponent();
      
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR  
    }
  


    //BOTON DE ATRAS
    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new Views.DashboardPage(ID));
    }

    private void btn_transfercuenta_Clicked(object sender, EventArgs e)
    {
       // Navigation.PushAsync(new TransCuentaPage());//Para cambiar de Pantalla
    }

    private void btn_aggtarjeta_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AggTarjetaPage(ID));//Para cambiar de Pantalla
    }


    private async void InicializarBienvenidaAsync(string userString)
    {
        var Saldo = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Saldo", "Usuarios");
        string resultado = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "N_Cuenta", "Usuarios");
        N_Cuenta.Text = resultado;
        Saldo_Act.Text = Saldo;
    }
}