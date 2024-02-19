namespace StarBank
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.Login());//PANTALLA QUE INICIA AL EJECUTAR

        }
    }
}
