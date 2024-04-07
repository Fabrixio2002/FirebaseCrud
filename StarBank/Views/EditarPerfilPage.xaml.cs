using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;

namespace StarBank.Views;

public partial class EditarPerfilPage : ContentPage
{
    private string ID;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public EditarPerfilPage(String id)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        ID = id;
        InicializarDatos(ID);
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen

        await Navigation.PushAsync(new verPerfilPage(ID));
    }

    private async void btn_actu_Clicked(object sender, EventArgs e)
    {
       
        string nuevoTelefono = txt_telefono.Text;

        if (string.IsNullOrEmpty(txt_telefono.Text))
        {
            await DisplayAlert("Error", "Por favor completa todos los campos", "OK");
        }
        else
        {
            await conexionFirebase.Prueba(ID, "Telefono", nuevoTelefono);
            //  await conexionFirebase.Prueba(ID, "Apellidos", nuevoApellido);


        }
    }

 
    private async void InicializarDatos(string userString)
    {
        var nombreUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Nombre", "Usuarios");
        var ApellidoUsuario = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Apellidos", "Usuarios");
        var Cel = await conexionFirebase.ObtenerDatosID<Usuarios>(userString, "Telefono", "Usuarios");
   
        txt_telefono.Text = Cel;

    }


}