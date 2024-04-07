using StarBank.Models;

namespace StarBank.Views;

public partial class verPerfilPage : ContentPage
{
    private string ID;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public verPerfilPage(String id)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        ID = id;
        InicializarDatos(ID);
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
       await Navigation.PushAsync(new DashboardPage(ID));
    }

    private void btn_editar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditarPerfilPage(ID));//Para cambiar de Pantalla

    }


    private async void InicializarDatos(string userString)
    {
        var nombreUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Nombre", "Usuarios");
        var ApellidoUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Apellidos", "Usuarios");
        var Dni = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "DNI", "Usuarios");
        var Cel = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Telefono", "Usuarios");
        var email = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Correo", "Usuarios");

        name.Text = "Nombre: " + nombreUsuario;
        apellido.Text = "Apellido: " + ApellidoUsuario;
        dni.Text = "N° Identificación: "+ Dni;
        telefono.Text = "Telefono: " + Cel;
        correo.Text = "Correo: " + email;

    }
}