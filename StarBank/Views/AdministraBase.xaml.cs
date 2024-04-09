namespace StarBank.Views;

public partial class AdministraBase : ContentPage
{
    public AdministraBase()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false); // Elimina la barra de navegación

        // Crea un objeto WebView
        var webView = new WebView
        {
            Source = "https://console.firebase.google.com/u/0/project/proyectostarbank/overview", // Establece la URL que deseas cargar
            VerticalOptions = LayoutOptions.FillAndExpand, // Opcional: expande verticalmente
            HorizontalOptions = LayoutOptions.FillAndExpand // Opcional: expande horizontalmente
        };

        // Agrega el WebView al StackLayout existente o a otro contenedor
        Content = new StackLayout
        {
            Children = { webView }
        };
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        // Navega a la nueva página cuando se toca la imagen
        await Navigation.PushAsync(new DashboardAdminPage());
    }
}