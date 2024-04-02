using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;
using System.Globalization;

namespace StarBank.Views;


public partial class VerTransMes : ContentPage
{
    private List<Transacciones> eventos;
    private string ID;
    ConexionFirebase conexionFirebase = new ConexionFirebase();

    public VerTransMes(String id)
    {
        InitializeComponent();
        BindingContext = this; // Esto establece el contexto de enlace en esta misma página (VerTransMes)
        ID = id;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

        try
        {
            string Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "N_Cuenta", "Usuarios");
            // Construye la consulta para filtrar por título específico
            // Construye la consulta para filtrar por tipo, número de cuenta y mes
            var query = await firebaseClient
                .Child("Transferencia")
                .OrderBy("CuentaO")
                .EqualTo(Cuenta)
                .OnceAsync<Transacciones>();

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
                // Muestra un mensaje de alerta en caso de éxito
               // await DisplayAlert("Éxito", "Datos de Firebase recuperados correctamente", "Aceptar");
            }
            else
            {
                // Muestra un mensaje de alerta si no se encontraron datos
                await DisplayAlert("Alerta", "No se encontraron datos de Firebase", "Aceptar");
            }
        }
        catch (Exception ex)
        {
            // Muestra un mensaje de alerta si ocurre algún error al recuperar los datos de Firebase
            await DisplayAlert("Error", $"Error al recuperar datos de Firebase: {ex.Message}", "Aceptar");
            Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
        }
    }

    private async void TipoTransaccionPicker_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        // Obtener el tipo de transacción seleccionado
        var selectedTipo = TipoTransaccionPicker.SelectedItem as string;
        if (selectedTipo == null)
            return;

        // Obtener las transacciones correspondientes al tipo seleccionado
        switch (selectedTipo)
        {

            case "Transferencia":
                try
                {
                    // Obtener el número de cuenta del usuario
                    string Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "N_Cuenta", "Usuarios");

                    // Inicializar el cliente de Firebase
                    var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

                    // Obtener el índice del mes seleccionado
                    var selectedIndex = MesPicker.SelectedIndex;

                    // Construye la consulta para filtrar por tipo, número de cuenta y mes
                    var query = await firebaseClient
                        .Child("Transferencia")
                        .OrderBy("Tipo")
                        .EqualTo("Transferencia")
                        .OnceAsync<Transacciones>();

                    // Filtrar los resultados por tipo, número de cuenta y mes
                    var resultados = query
                        .Where(item =>
                            !string.IsNullOrEmpty(item.Object.Fecha) && // Verifica que la cadena de fecha no esté vacía
                            item.Object.CuentaO == Cuenta && // Ajusta según el número de cuenta que desees validar
                            item.Object.Tipo == "Transferencia" && // Ajusta según el tipo que desees validar
                            DateTime.TryParseExact(item.Object.Fecha, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) && // Intenta analizar la cadena de fecha como DateTime
                            fecha.Month == selectedIndex + 1) // Filtrar por el mes que desees (agregamos 1 al índice para coincidir con el sistema de numeración de meses)
                        .Select(item => new Transacciones
                        {
                            CuentaO = item.Object.CuentaO,
                            Tipo = item.Object.Tipo,
                            NombreRecibe=item.Object.NombreRecibe
                            // Agrega otros campos que desees recuperar
                        })
                        .ToList();

                    // Asigna los datos al ItemsSource del CollectionView solo si hay datos disponibles
                    if (resultados.Any())
                    {
                        Eventos.ItemsSource = resultados;

                        // Muestra un mensaje de alerta en caso de éxito
                        await DisplayAlert("Éxito", "Datos de Firebase recuperados correctamente", "Aceptar");
                    }
                    else
                    {
                        // Muestra un mensaje de alerta si no se encontraron datos
                        await DisplayAlert("Alerta", "Seleccione un Mes", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    // Muestra un mensaje de alerta si ocurre algún error al recuperar los datos de Firebase
                    await DisplayAlert("Error", $"Error al recuperar datos", "Aceptar");
                }


                break;
            case "Evento":
                try
                {
                    // Obtener el número de cuenta del usuario
                    string Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "N_Cuenta", "Usuarios");

                    // Inicializar el cliente de Firebase
                    var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

                    // Obtener el índice del mes seleccionado
                    var selectedIndex = MesPicker.SelectedIndex;

                    // Construye la consulta para filtrar por tipo, número de cuenta y mes
                    var query = await firebaseClient
                        .Child("Transferencia")
                        .OrderBy("Tipo")
                        .EqualTo("EVENTO")
                        .OnceAsync<Transacciones>();

                    // Filtrar los resultados por tipo, número de cuenta y mes
                    var resultados = query
                        .Where(item =>
                            !string.IsNullOrEmpty(item.Object.Fecha) && // Verifica que la cadena de fecha no esté vacía
                            item.Object.CuentaO == Cuenta && // Ajusta según el número de cuenta que desees validar
                            item.Object.Tipo == "EVENTO" && // Ajusta según el tipo que desees validar
                            DateTime.TryParseExact(item.Object.Fecha, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) && // Intenta analizar la cadena de fecha como DateTime
                            fecha.Month == selectedIndex + 1) // Filtrar por el mes que desees (agregamos 1 al índice para coincidir con el sistema de numeración de meses)
                        .Select(item => new Transacciones
                        {
                            CuentaO = item.Object.CuentaO,
                            Tipo = item.Object.Tipo,
                            NombreRecibe = item.Object.NombreRecibe
                            // Agrega otros campos que desees recuperar
                        })
                        .ToList();

                    // Asigna los datos al ItemsSource del CollectionView solo si hay datos disponibles
                    if (resultados.Any())
                    {
                        Eventos.ItemsSource = resultados;

                        // Muestra un mensaje de alerta en caso de éxito
                        await DisplayAlert("Éxito", "Datos de Firebase recuperados correctamente", "Aceptar");
                    }
                    else
                    {
                        // Muestra un mensaje de alerta si no se encontraron datos
                        await DisplayAlert("Alerta", "No se encontraron datos de Firebase para los criterios especificados" + Cuenta, "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    // Muestra un mensaje de alerta si ocurre algún error al recuperar los datos de Firebase
                    await DisplayAlert("Error", $"Error al recuperar datos de Firebase: {ex.Message}", "Aceptar");
                    Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
                }

                break;
            case "Servicio":

                try
                {
                    // Obtener el número de cuenta del usuario
                    string Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "N_Cuenta", "Usuarios");

                    // Inicializar el cliente de Firebase
                    var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

                    // Obtener el índice del mes seleccionado
                    var selectedIndex = MesPicker.SelectedIndex;

                    // Construye la consulta para filtrar por tipo, número de cuenta y mes
                    var query = await firebaseClient
                        .Child("Transferencia")
                        .OrderBy("Tipo")
                        .EqualTo("SERVICIO")
                        .OnceAsync<Transacciones>();

                    // Filtrar los resultados por tipo, número de cuenta y mes
                    var resultados = query
                        .Where(item =>
                            !string.IsNullOrEmpty(item.Object.Fecha) && // Verifica que la cadena de fecha no esté vacía
                            item.Object.CuentaO == Cuenta && // Ajusta según el número de cuenta que desees validar
                            item.Object.Tipo == "SERVICIO" && // Ajusta según el tipo que desees validar
                            DateTime.TryParseExact(item.Object.Fecha, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) && // Intenta analizar la cadena de fecha como DateTime
                            fecha.Month == selectedIndex + 1) // Filtrar por el mes que desees (agregamos 1 al índice para coincidir con el sistema de numeración de meses)
                        .Select(item => new Transacciones
                        {
                            CuentaO = item.Object.CuentaO,
                            Tipo = item.Object.Tipo,
                            NombreRecibe = item.Object.NombreRecibe
                            // Agrega otros campos que desees recuperar
                        })
                        .ToList();

                    // Asigna los datos al ItemsSource del CollectionView solo si hay datos disponibles
                    if (resultados.Any())
                    {
                        Eventos.ItemsSource = resultados;

                        // Muestra un mensaje de alerta en caso de éxito
                        await DisplayAlert("Éxito", "Datos de Firebase recuperados correctamente", "Aceptar");
                    }
                    else
                    {
                        // Muestra un mensaje de alerta si no se encontraron datos
                        await DisplayAlert("Alerta", "No se encontraron datos de Firebase para los criterios especificados" + Cuenta, "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    // Muestra un mensaje de alerta si ocurre algún error al recuperar los datos de Firebase
                    await DisplayAlert("Error", $"Error al recuperar datos de Firebase: {ex.Message}", "Aceptar");
                    Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
                }



                break;
            default:
                break;
        }
    }

    private async void MesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Obtener el tipo de transacción seleccionado
        var selectedTipo = TipoTransaccionPicker.SelectedItem as string;
        if (selectedTipo == null)
            return;

        // Obtener el número de cuenta del usuario
        string Cuenta = await conexionFirebase.ObtenerDatosID<Usuarios>(ID, "N_Cuenta", "Usuarios");

        // Inicializar el cliente de Firebase
        var firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

        // Obtener el índice del mes seleccionado
        var selectedIndex = MesPicker.SelectedIndex;

        try
        {
            // Construye la consulta para filtrar por tipo, número de cuenta y mes
            var query = await firebaseClient
                .Child("Transferencia")
                .OrderBy("Tipo")
                .EqualTo("Transferencia")
                .OnceAsync<Transacciones>();

            // Filtrar los resultados por tipo, número de cuenta y mes
            var resultados = query
                .Where(item =>
                    !string.IsNullOrEmpty(item.Object.Fecha) && // Verifica que la cadena de fecha no esté vacía
                    item.Object.CuentaO == Cuenta && // Ajusta según el número de cuenta que desees validar
                    item.Object.Tipo == "Transferencia" && // Ajusta según el tipo que desees validar
                    DateTime.TryParseExact(item.Object.Fecha, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) && // Intenta analizar la cadena de fecha como DateTime
                    fecha.Month == selectedIndex + 1) // Filtrar por el mes que desees (agregamos 1 al índice para coincidir con el sistema de numeración de meses)
                .Select(item => new Transacciones
                {
                    CuentaO = item.Object.CuentaO,
                    Tipo = item.Object.Tipo,
                    NombreRecibe = item.Object.NombreRecibe
                    // Agrega otros campos que desees recuperar
                })
                .ToList();

            // Asigna los datos al ItemsSource del CollectionView solo si hay datos disponibles
            if (resultados.Any())
            {
                Eventos.ItemsSource = resultados;

                // Muestra un mensaje de alerta en caso de éxito
                await DisplayAlert("Éxito", "Datos de Firebase recuperados correctamente", "Aceptar");
            }
            else
            {
                // Muestra un mensaje de alerta si no se encontraron datos
                await DisplayAlert("Alerta", "No se encontraron datos de Firebase para los criterios especificados" + Cuenta, "Aceptar");
            }
        }
        catch (Exception ex)
        {
            // Muestra un mensaje de alerta si ocurre algún error al recuperar los datos de Firebase
            await DisplayAlert("Error", $"Error al recuperar datos de Firebase: {ex.Message}", "Aceptar");
            Console.WriteLine($"Error al recuperar datos de Firebase: {ex.Message}");
        }
    }

}
