using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;

namespace StarBank.Views;


public partial class VerTransMes : ContentPage
{
    private List<Transacciones> eventos;
    private string ID;

    public VerTransMes(String id)
    {
        InitializeComponent();
        BindingContext = this; // Esto establece el contexto de enlace en esta misma p�gina (VerTransMes)
        ID = id;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

        try
        {
            // Construye la consulta para filtrar por t�tulo espec�fico
            var query = await firebaseClient.Child("Transferencia").OnceAsync<Transacciones>();

            // Convierte los datos de Firebase en una lista de Eventos
            eventos = query.Select(item => new Transacciones
            {
                CuentaO = item.Object.CuentaO,
                Tipo = item.Object.Tipo,

            }).ToList();

            // Asigna los datos al ItemsSource del CollectionView solo si hay datos disponibles
            if (eventos != null && eventos.Any())
            {
                Eventos.ItemsSource = eventos;
                // Muestra un mensaje de alerta en caso de �xito
                await DisplayAlert("�xito", "Datos de Firebase recuperados correctamente", "Aceptar");
            }
            else
            {
                // Muestra un mensaje de alerta si no se encontraron datos
                await DisplayAlert("Alerta", "No se encontraron datos de Firebase", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            // Muestra un mensaje de alerta si ocurre alg�n error al recuperar los datos de Firebase
            await DisplayAlert("Error", $"Error al recuperar datos de Firebase: {ex.Message}", "Aceptar");
            Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
        }
    }

    private async void TipoTransaccionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        // Obtener el tipo de transacci�n seleccionado
        var selectedTipo = TipoTransaccionPicker.SelectedItem as string;
        if (selectedTipo == null)
            return;

        // Obtener las transacciones correspondientes al tipo seleccionado
        switch (selectedTipo)
        {

            case "Transferencia":

                var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

                try
                {
                    // Construye la consulta para filtrar por t�tulo espec�fico
                    var query = await firebaseClient
                        .Child("Transferencia")
                        .OrderBy("Tipo")
                        .EqualTo("Transferencia")
                        .OnceAsync<Transacciones>();

                    // Filtrar los resultados por un segundo campo (por ejemplo, CuentaO)
                    var resultados = query
                        .Where(item => item.Object.CuentaO == "00001") // Ajusta seg�n el campo y el valor que desees validar
                        .Select(item => new Transacciones
                        {
                            CuentaO = item.Object.CuentaO,
                            Tipo=item.Object.Tipo,
                            // Agrega otros campos que desees recuperar
                        })
                        .ToList();

                    // Asigna los datos al ItemsSource del CollectionView solo si hay datos disponibles
                    if (resultados.Any())
                    {
                        Eventos.ItemsSource = resultados;

                        // Muestra un mensaje de alerta en caso de �xito
                        await DisplayAlert("�xito", "Datos de Firebase recuperados correctamente", "Aceptar");
                    }
                    else
                    {
                        // Muestra un mensaje de alerta si no se encontraron datos
                        await DisplayAlert("Alerta", "No se encontraron datos de Firebase", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    // Muestra un mensaje de alerta si ocurre alg�n error al recuperar los datos de Firebase
                    await DisplayAlert("Error", $"Error al recuperar datos de Firebase: {ex.Message}", "Aceptar");
                    Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
                }

                break;
            case "Evento":
                // L�gica para obtener transacciones de tipo evento
                // Actualiza el origen de datos (eventos) seg�n corresponda
                break;
            case "Servicio":
                // L�gica para obtener transacciones de tipo servicio
                // Actualiza el origen de datos (eventos) seg�n corresponda
                break;
            default:
                break;
        }
    }


    



}
