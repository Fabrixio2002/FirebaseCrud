using Firebase.Database;
using StarBank.Models;

namespace StarBank.Views;

public partial class VerEventosPage : ContentPage
{
    private List<Eventos> eventos;
    private String id;

    public VerEventosPage(String ID)
	{
        id = ID;

        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

        try
        {
            // Recupera los datos de Firebase Realtime Database
            var eventosSnapshot = await firebaseClient.Child("Eventos").OnceAsync<Eventos>();

            // Convierte los datos de Firebase en una lista de Eventos
            eventos = eventosSnapshot.Select(item => new Eventos
            {
                Titulo = item.Object.Titulo,
                Descripcion = item.Object.Descripcion,
                ImagenUrl = item.Object.ImagenUrl,
                Fecha=item.Object.Fecha,
                Precio=item.Object.Precio,
                Direccion=item.Object.Direccion,
            }).ToList();

            // Asigna los datos al ItemsSource del CollectionView
            collectionView.ItemsSource = eventos;
        }
        catch (Exception ex)
        {
            // Maneja la excepción si ocurre algún error al recuperar los datos de Firebase
            Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
        }
    }
    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new ServiciosPage(id));
    }

    private async void btn_Ver_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new ComprarEventoPage());

    }
    private async  void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (collectionView.SelectedItem != null && collectionView.SelectedItem is Eventos selectedEvent)
        {
            // Pasar la descripción del elemento seleccionado a la nueva página
            await Navigation.PushAsync(new ComprarEventoPage(id,selectedEvent.Descripcion, selectedEvent.Titulo, selectedEvent.Direccion,
                selectedEvent.Fecha, selectedEvent.ImagenUrl,selectedEvent.Precio));

            // Deseleccionar el elemento después de realizar la navegación
            collectionView.SelectedItem = null;
        }

    }


}