
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Storage;
using MimeKit;
using StarBank.Models;



namespace StarBank
{

    internal class ConexionFirebase
    {

        FirebaseClient client = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");

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
                new Firebase.Auth.Providers.EmailProvider(),
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
                //HACEMOS CONECXION 
                var cliente = ConectarFirebase();
                //USAMOS EL INCIO DE SESION CON EMAIL Y CONTRASEÑA
                var userCredential = await cliente.SignInWithEmailAndPasswordAsync(Email, Password);
                //OBTENEMOS EL ID DEL USUARIO QUE SE CONECTO

                var userId = await BuscarCorreoEnBaseDeDatos(Email);
                String userString = userId.ToString();

                var nombreUsuario = await GetNombreUsuarioPorId(userString);
                await Application.Current.MainPage.Navigation.PushAsync(new Views.DashboardPage(nombreUsuario.ToString()));


                return userCredential;

            }
            catch (FirebaseAuthException ex)
            {

                await Application.Current.MainPage.DisplayAlert("Error", "Correo electrónico o contraseña incorrectos.", "Aceptar");

                return null;
            }
        }


        //cerrar sesion x(
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


        public async Task<string> BuscarCorreoEnBaseDeDatos(string correo)
        {
            var usuarios = await client.Child("Usuarios").OnceAsync<Dictionary<string, string>>();
            string userId = ""; // Inicializa userId con un valor predeterminado
            foreach (var usuario in usuarios)
            {
                if (usuario.Object["Correo"] == correo)
                {
                    userId = usuario.Key;
                    return userId;
                }
            }

            return "xd";
        }




        public async Task<string> GetNombreUsuarioPorId(string idUsuario)
        {
            try
            {
                var snapshot = await client.Child("Usuarios").Child(idUsuario).OnceSingleAsync<Usuarios>();
                if (snapshot != null)
                {
                    return snapshot.Nombre;
                }
                else
                {
                    return "Nombre de usuario no encontrado";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de conexión o datos no encontrados
                Console.WriteLine($"Error: {ex.Message}");
                return "Error al recuperar nombre de usuario";
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
    }



