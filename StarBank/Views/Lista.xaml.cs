using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;


namespace StarBank.Views
{
    public partial class Lista : ContentPage
    {
        private readonly FirebaseClient firebaseClient;
        public ObservableCollection<Productos> Productos { get; set; }

        public Lista()
        {
            InitializeComponent();
            firebaseClient = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");
            Productos = new ObservableCollection<Productos>();
            listViewProductos.ItemsSource = Productos;
            MostrarProductos();
        }

        private async void MostrarProductos()
        {
            try
            {
                var productosRef = firebaseClient.Child("productos");
                var productos = await productosRef.OnceAsync<Productos>();

                Productos.Clear(); // Limpiar la lista antes de agregar los nuevos productos

                foreach (var producto in productos)
                {
                    Productos.Add(producto.Object);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar los productos: {ex.Message}", "OK");
            }
        }

        private async void OnProductoSeleccionado(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var productoSeleccionado = e.SelectedItem as Productos;

            bool eliminar = await DisplayAlert("Eliminar producto", $"¿Estás seguro de eliminar '{productoSeleccionado.Nombre}'?", "Sí", "Cancelar");

            if (eliminar)
            {
                await EliminarProducto(productoSeleccionado);
            }

            // Desmarcar la selección
            ((ListView)sender).SelectedItem = null;
        }

        private async Task EliminarProducto(Productos producto)
        {
            try
            {
                var productosRef = firebaseClient.Child("productos");

                // Buscar el producto en Firebase utilizando su nombre
                var productosAEliminar = (await productosRef
                    .OrderBy("Nombre")
                    .EqualTo(producto.Nombre)
                    .OnceAsync<Productos>()).FirstOrDefault();

                if (productosAEliminar != null)
                {
                    // Eliminar el producto de Firebase
                    await productosRef.Child(productosAEliminar.Key).DeleteAsync();

                    // Actualizar la lista en la interfaz de usuario
                    Productos.Remove(producto);
                }
                else
                {
                    await DisplayAlert("Error", "El producto no pudo ser encontrado.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo eliminar el producto: {ex.Message}", "OK");
            }
        }


    }
}
