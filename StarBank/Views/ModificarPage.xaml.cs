using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using StarBank.Models;

namespace StarBank.Views;

public partial class ModificarPage : ContentPage
{
    public String ID;

    private string imagenUri;
    public ModificarPage(Dictionary<string, object> parameters)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR
        if (parameters.ContainsKey("EventoSeleccionado") && parameters.ContainsKey("ImagenUrl"))
        {
            var eventoSeleccionado = (Eventos)parameters["EventoSeleccionado"];
            var imagenUrl = (string)parameters["ImagenUrl"];

            // Utilizar los datos del evento seleccionado para inicializar los campos en la página de modificación
            // Por ejemplo:
            txt_titulo.Text = eventoSeleccionado.Titulo;
            txt_descripcion.Text = eventoSeleccionado.Descripcion;
            txt_direccion.Text = eventoSeleccionado.Direccion;
            txt_precio.Text = eventoSeleccionado.Precio;
            foto.Source = eventoSeleccionado.ImagenUrl;
            ID = eventoSeleccionado.Id;
            // Inicializa otros campos según sea necesario
        }

    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new EventoPage());
    }

    private async void btn_foto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Seleccionar imagen"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                foto.Source = ImageSource.FromStream(() => stream);

                // Guarda la URI de la imagen seleccionada
                imagenUri = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
        }
    }

    private async void btn_agregar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txt_titulo.Text) ||
       string.IsNullOrWhiteSpace(txt_descripcion.Text) ||
       string.IsNullOrWhiteSpace(txt_direccion.Text) ||
       string.IsNullOrWhiteSpace(txt_precio.Text))
        {
            // Mostrar alerta indicando campos vacíos
            await DisplayAlert("Campos Vacíos", "Por favor, complete todos los campos.", "Aceptar");
        }
        else
        {
            try
            {
                string fechaSeleccionadaString = fecha.Date.ToString("dd/MM/yyyy"); // Convierte la fecha seleccionada a una cadena en formato "dd/MM/yyyy"

                // Crea un objeto con los datos actualizados del evento
                var eventoModificado = new Eventos
                {
                    Id = ID, // Utiliza el ID del evento existente
                    Titulo = txt_titulo.Text,
                    Descripcion = txt_descripcion.Text,
                    Direccion = txt_direccion.Text,
                    Precio = txt_precio.Text,
                    Fecha = fechaSeleccionadaString,
                    ImagenUrl = imagenUri // La URL de la imagen puede haber cambiado si se seleccionó una nueva
                };

                // Actualiza el evento en la base de datos de Firebase
                var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");
                await firebaseClient
                    .Child("Eventos") // Nombre del nodo en la base de datos
                    .Child(ID) // ID del evento que se desea modificar
                    .PutAsync(eventoModificado); // Actualiza los datos del evento con los nuevos datos

                await DisplayAlert("Listo", "Evento modificado correctamente.", "Aceptar");

                // Vuelve a la página anterior
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al modificar el evento: {ex.Message}", "Aceptar");
            }
        }
    }

    private async Task<string> SubirImagenFirebaseStorage(string imagenUri)
    {
        try
        {
            // Crea una instancia del cliente de Firebase Storage
            var firebaseStorage = new FirebaseStorage("proyectostarbank.appspot.com");

            // Obtiene el nombre de archivo de la URI de la imagen
            var fileName = Path.GetFileName(imagenUri);

            // Abre el archivo de imagen como flujo de datos
            using (var stream = File.OpenRead(imagenUri))
            {
                // Sube la imagen a Firebase Storage
                var imageUrl = await firebaseStorage
                    .Child("images") // Especifica la carpeta en la que deseas guardar la imagen
                    .Child(fileName) // Especifica el nombre de la imagen en Firebase Storage
                    .PutAsync(stream);

                // Obtiene la URL de descarga de la imagen subida
                return imageUrl;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al subir la imagen a Firebase Storage: {ex.Message}");
            return null;
        }

    }
}