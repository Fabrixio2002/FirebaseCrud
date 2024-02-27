
using Firebase.Auth;
using Firebase.Auth.Providers;
using MimeKit;



namespace StarBank
{
    internal class ConexionFirebase
    {
        public static FirebaseAuthClient ConectarFirebase()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyCftYHrKkZgRw4WYt6ZBvQKjacGlh_MtJ8",
                AuthDomain = "proyectostarbank.web.app",
                Providers = new Firebase.Auth.Providers.FirebaseAuthProvider[]
            {
                // Add and configure individual providers
                new Firebase.Auth.Providers.GoogleProvider().AddScopes("email"),
                new Firebase.Auth.Providers.EmailProvider()
                // ...
            },

            };

            var client = new FirebaseAuthClient(config);

            return client;

        }



public async Task<UserCredential> CrearUsuario(string Email, string Password)
    {
        var cliente = ConectarFirebase();
        var userCredential = await cliente.CreateUserWithEmailAndPasswordAsync(Email, Password);
            return userCredential;
    }

     
        
        public async Task<UserCredential> InicioSesion(string Email, string Password)
        {
            try
    {
        var cliente = ConectarFirebase();
        var userCredential = await cliente.SignInWithEmailAndPasswordAsync(Email, Password);
        await Application.Current.MainPage.Navigation.PushAsync(new Views.DashboardPage());

                return userCredential;
    }
    catch (FirebaseAuthException ex)
    {
        // Mostrar un mensaje de alerta si las credenciales son incorrectas
        await Application.Current.MainPage.DisplayAlert("Error", "Correo electrónico o contraseña incorrectos.", "Aceptar");
        return null; // Retornar null o realizar otro manejo según tus necesidades
    }
        }

        public async Task CerrarSesion()
        {
            try
            {
                var cliente = ConectarFirebase();
                 cliente.SignOut();
                await Application.Current.MainPage.Navigation.PushAsync(new Views.Login()); // Redirigir a la página de inicio de sesión después de cerrar sesión
            }
            catch (FirebaseAuthException ex)
            {
                // Manejar la excepción si ocurre algún error durante el cierre de sesión
                await Application.Current.MainPage.DisplayAlert("Error", "Se produjo un error al cerrar sesión.", "Aceptar");
            }
        }


        public async Task ContraseñaNueva(String correo)
        {
            var cliente = ConectarFirebase();
            await cliente.ResetEmailPasswordAsync(correo);
            
        }








    }

}




      
    

    






internal class FirebaseConfig
    {
        private string v;

        public FirebaseConfig(string v)
        {
            this.v = v;
        }


    }



