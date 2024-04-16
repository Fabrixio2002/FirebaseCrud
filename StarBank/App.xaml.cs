namespace StarBank
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.Producto());//PANTALLA QUE INICIA AL EJECUTAR

        }
    }
}
