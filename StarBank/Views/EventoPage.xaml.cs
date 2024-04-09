namespace StarBank.Views;

using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;
public partial class EventoPage : ContentPage
{
    private List<Eventos> eventos;
    public EventoPage()
    {
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
                Id = item.Key, 
                Titulo = item.Object.Titulo,
                Descripcion = item.Object.Descripcion,
                ImagenUrl = item.Object.ImagenUrl,
                Fecha = item.Object.Fecha,
                Precio = item.Object.Precio,
                Direccion = item.Object.Direccion,
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
        await Navigation.PushAsync(new DashboardAdminPage());
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AddEventoPage());
    }

    private  async void btn_modificar_Clicked(object sender, EventArgs e)
    {
        if (collectionView.SelectedItem != null)
        {
            var eventoSeleccionado = (Eventos)collectionView.SelectedItem;

            // Crear un diccionario para almacenar los parámetros
            var parameters = new Dictionary<string, object>();
            parameters.Add("EventoSeleccionado", eventoSeleccionado); // Pasar el objeto completo del evento
            parameters.Add("ImagenUrl", eventoSeleccionado.ImagenUrl); // Pasar la URL de la imagen

            // Navegar a la página de modificación y pasar los parámetros
            await Navigation.PushAsync(new ModificarPage(parameters));
        }
        else
        {
            Console.WriteLine("Ningún evento seleccionado para modificar.");
        }
    }

    private async void btn_eliminar_Clicked(object sender, EventArgs e)
    {
        if (collectionView.SelectedItem != null) // Verifica si se ha seleccionado algún elemento
        {
            var eventoSeleccionado = (Eventos)collectionView.SelectedItem; // Obtiene el evento seleccionado

            var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

            try
            {
                // Elimina el evento seleccionado de la base de datos
                await firebaseClient.Child("Eventos").Child(eventoSeleccionado.Id).DeleteAsync();

                // Remueve el evento de la lista local
                eventos.Remove(eventoSeleccionado);

                // Actualiza el ItemsSource del CollectionView
                collectionView.ItemsSource = null;
                collectionView.ItemsSource = eventos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar evento de Firebase: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Ningún evento seleccionado para eliminar.");
        }


    }
    private async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }
    }