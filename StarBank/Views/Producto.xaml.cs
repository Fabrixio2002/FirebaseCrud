using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;

namespace StarBank.Views;

public partial class Producto : ContentPage
{
    private readonly FirebaseClient firebaseClient;

    public Producto()
	{

		InitializeComponent();
        firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

    }


    private void btn_foto_Clicked(object sender, EventArgs e)
    {

    }

    private async void btn_guardarproducto_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txt_producto.Text) &&
            !string.IsNullOrWhiteSpace(txt_descripcion.Text) &&
            !string.IsNullOrWhiteSpace(txt_precio.Text))
        {
            // Crear el producto
            var producto = new Productos
            {
                Nombre = txt_producto.Text,
                Descripcion = txt_descripcion.Text,
                precio = txt_precio.Text // Guardar el precio como cadena
            };

            // Obtener referencia al nodo de productos
            var productosRef = firebaseClient.Child("productos");

            // Verificar si el nodo de productos existe
            var productosSnapshot = await productosRef.OnceAsync<Productos>();
            if (productosSnapshot == null || productosSnapshot.Count == 0)
            {
                // Si no existe, crear el nodo
                await firebaseClient.Child("productos").PostAsync(producto);
            }
            else
            {
                // Si el nodo existe, agregar el producto
                await productosRef.PostAsync(producto);
            }

            // Mostrar mensaje de éxito
            await DisplayAlert("Éxito", "Producto guardado correctamente.", "OK");

            // Limpiar campos
            LimpiarCampos();
        }
        else
        {
            await DisplayAlert("Campos incompletos", "Por favor, complete todos los campos antes de continuar.", "OK");
        }
    }


    public void LimpiarCampos()
    {
        txt_producto.Text = string.Empty;
        txt_descripcion.Text = string.Empty;
        txt_precio.Text = string.Empty;
    }

    private async void btn_verLista_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Lista());
    }
}