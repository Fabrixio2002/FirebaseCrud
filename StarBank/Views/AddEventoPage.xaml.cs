using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;
using Firebase.Storage; 

namespace StarBank.Views;

public partial class AddEventoPage : ContentPage
{
    private string imagenUri;

    public AddEventoPage()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR

    }
    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navegar a la nueva página cuando se toca la imagen
        // await Navigation.PushAsync(new EventoPage());
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
            string.IsNullOrWhiteSpace(txt_precio.Text) ||
            string.IsNullOrWhiteSpace(imagenUri))
        {
            // Mostrar alerta indicando campos vacíos
            await DisplayAlert("Campos Vacíos", "Por favor, complete todos los campos y seleccione una imagen.", "Aceptar");
        }
        else
        {
            try
            {
                string fechaSeleccionadaString = fecha.Date.ToString("dd/MM/yyyy"); // Convierte la fecha seleccionada a una cadena en formato "dd/MM/yyyy"
                string imageUrl = await SubirImagenFirebaseStorage(imagenUri);

                // Crea un nuevo objeto con los datos del evento
                var nuevoEvento = new Eventos
                {
                    Titulo = txt_titulo.Text,
                    Descripcion = txt_descripcion.Text,
                    Direccion = txt_direccion.Text,
                    Precio = txt_precio.Text,
                    Fecha = fechaSeleccionadaString,
                    ImagenUrl = imageUrl // URL de descarga de la imagen
                };

                // Guarda el nuevo evento en tu base de datos de Firebase
                var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");
                var eventoAgregado = await firebaseClient
                    .Child("Eventos") // Nombre del nodo en la base de datos
                    .PostAsync(nuevoEvento);

                await DisplayAlert("Listo", "Evento guardado correctamente.", "Aceptar");

                // Limpia los campos de entrada después de guardar el evento
                txt_titulo.Text = string.Empty;
                txt_descripcion.Text = string.Empty;
                txt_direccion.Text = string.Empty;
                txt_precio.Text = string.Empty;
                fecha.Date = DateTime.Today;
                imagenUri = null; // Reinicia la URI de la imagen
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al guardar el evento: {ex.Message}", "Aceptar");
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