
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

                var nombreUsuario = await GetNombreUsuarioPorId(userString, "Nombre");
                var apellidoUsuario = await GetNombreUsuarioPorId(userString, "Apellidos");
                var cuenta = await GetNombreUsuarioPorId(userString, "N_Cuenta");
                var saldo = await GetNombreUsuarioPorId(userString, "Saldo");


                await Application.Current.MainPage.Navigation.PushAsync(new Views.DashboardPage(userId.ToString(), nombreUsuario,apellidoUsuario,cuenta,saldo));


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




        public async Task<string> GetNombreUsuarioPorId(string idUsuario,String nombreDato)
        {
            try
            {
                var snapshot = await client.Child("Usuarios").Child(idUsuario).OnceSingleAsync<Usuarios>();
                var propiedad = typeof(Usuarios).GetProperty(nombreDato);
                if (snapshot != null)
                {
                    return propiedad.GetValue(snapshot).ToString(); // Convertir el objeto al tipo string
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

        public async Task ActualizarDatoUsuario(string idUsuario, string nombreDato, string nuevoValor)
        {
            try
            {
                // Construir la ruta completa al nodo que contiene el dato que deseas actualizar
                var rutaNodo = $"Usuarios/{idUsuario}/{nombreDato}";
               
                // Actualizar el dato utilizando PutAsync
                await client.Child(rutaNodo).PutAsync(nuevoValor);
                Console.WriteLine($"Dato {nombreDato} actualizado correctamente para el usuario con ID {idUsuario}");
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine($"Error al actualizar el dato: {ex.Message}");
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



