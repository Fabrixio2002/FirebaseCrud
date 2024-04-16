
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


        //<CREAR USUARIO 
        public async Task<UserCredential> CrearUsuario(string Email, string Password)
        {
            var cliente = ConectarFirebase();
            var userCredential = await cliente.CreateUserWithEmailAndPasswordAsync(Email, Password);
            return userCredential;
        }


        //MANJEMOS EL INCIO DE SESION 
        public async Task<UserCredential> InicioSesion(string Email, string Password)
        {
            try
            {
                //HACEMOS CONECXION 
                var cliente = ConectarFirebase();
                //USAMOS EL INCIO DE SESION CON EMAIL Y CONTRASEÑA
                var userCredential = await cliente.SignInWithEmailAndPasswordAsync(Email, Password);
                //OBTENEMOS EL ID DEL USUARIO QUE SE CONECTO

                //oBTENEMOS EL USUARIO Y LO CONVERTIMOS EN UNA CADENA DE TEXTO 
                var userId = await BuscarCorreoEnBaseDeDatos(Email);
                String userString = userId.ToString();


                //CAMBIAMOS DE PAGINA Y LE MANDAMOS PARAMETROS DE LOS DATOS QUE NECESITAMOS
               // await Application.Current.MainPage.Navigation.PushAsync(new Views.DashboardPage(userId.ToString()));


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
                await Application.Current.MainPage.Navigation.PopToRootAsync();
              //  await Application.Current.MainPage.Navigation.PushAsync(new Views.Login());// Redirigir a la página de inicio de sesión después de cerrar sesión

            }
            catch (FirebaseAuthException ex)
            {
                // Manejar la excepción si ocurre algún error durante el cierre de sesión
                await Application.Current.MainPage.DisplayAlert("Error", "Se produjo un error al cerrar sesión.", "Aceptar");
            }
        }


        //Usamos un metodo de firebase para recuperar contraseña mediante el correo electronico
        public async Task ContraseñaNueva(String correo)
        {
            var cliente = ConectarFirebase();
            await cliente.ResetEmailPasswordAsync(correo);

        }


        //Estamos buscando en nuestra base de datos nuestro correo y obtenemos nuestro id poara manejar los datos.
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

            return "no encontrado";
        }


        //Estamos buscando en nuestra base de datos nuestro correo y obtenemos nuestro id poara manejar los datos.
        public async Task<string> BuscarIDEnBaseDeDatos(string cuenta)
        {
            var usuarios = await client.Child("Usuarios").OnceAsync<Dictionary<string, string>>();
            string userId = ""; // Inicializa userId con un valor predeterminado
            foreach (var usuario in usuarios)
            {
                if (usuario.Object["N_Cuenta"] == cuenta)
                {
                    userId = usuario.Key;
                    return userId;
                }
            }

            return "no encontrado";
        }

        public async Task<string> BuscarTarjeta(string numeroTarjeta)
        {
            var tarjetas = await client.Child("Tarjetas").OnceAsync<Dictionary<string, string>>();
            string userId = ""; // Inicializa userId con un valor predeterminado
            foreach (var tarjeta in tarjetas)
            {
                if (tarjeta.Object["N_Tarjeta"] == numeroTarjeta)
                {
                    userId = tarjeta.Key;
                    return userId;
                }
            }

            return "No encontrado";
        }

        public async Task<string> BuscarFactura(string NumeroFactura)
        {
            var tarjetas = await client.Child("Facturas").OnceAsync<Dictionary<string, string>>();
            string userId = ""; // Inicializa userId con un valor predeterminado
            foreach (var tarjeta in tarjetas)
            {
                if (tarjeta.Object["N_Factrua"] == NumeroFactura)
                {
                    userId = tarjeta.Key;
                    return userId;
                }
            }

            return "No encontrado";
        }






        //Metodo para obtener datos de la base de datos
        public async Task<string> ObtenerDatosID<T>(string idUsuario,String nombreDato,String Nodo)
        {
            try
            {
                var snapshot = await client.Child(Nodo).Child(idUsuario).OnceSingleAsync<T>();
                var propiedad = typeof(T).GetProperty(nombreDato);
                if (snapshot != null)
                {
                    var valorPropiedad = propiedad.GetValue(snapshot); // Obtener el valor de la propiedad específica
                    if (valorPropiedad != null)
                    {
                        return valorPropiedad.ToString(); // Convertir el objeto al tipo string
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de conexión o datos no encontrados
                Console.WriteLine($"Error: {ex.Message}");
                return "Error al recuperar nombre de usuario";
            }
        }

        public async Task Prueba(string id, string nombreDato, string nuevoValor)
        {
            try
            {
                // Construir la ruta completa al nodo que contiene el dato que deseas actualizar
                var rutaNodo = $"Usuarios/{id}/{nombreDato}";

                // Actualizar el dato utilizando PutAsync
                await client.Child(rutaNodo).PutAsync(nuevoValor);
                Console.WriteLine($"Dato {nombreDato} actualizado correctamente para el usuario con ID {id}");
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine($"Error al actualizar el dato: {ex.Message}");
            }
        }



        //Actualizamos algun campo de la base de datos
        public async Task  ActualizarDatoUsuario(string idUsuario, string nombreDato, string nuevoValor)
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

        public async Task ActualizarDatoTarjeta(string id, string nombreDato, string nuevoValor)
        {
            try
            {
                // Construir la ruta completa al nodo que contiene el dato que deseas actualizar
                var rutaNodo = $"Tarjetas/{id}/{nombreDato}";

                // Actualizar el dato utilizando PutAsync
                await client.Child(rutaNodo).PutAsync(nuevoValor);
                Console.WriteLine($"Dato {nombreDato} actualizado correctamente para el usuario con ID {id}");
            }
            catch (Exception ex)
            {
                // Manejar errores
                Console.WriteLine($"Error al actualizar el dato: {ex.Message}");
            }
        }



        public async Task ActuDato(string idUsuario, string nombreDato, string nuevoValor)
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




    


     



        //Actualizamos algun campo de la base de datos
        public async Task ActuEstado(string idUsuario, string nombreDato, string nuevoValor)
        {
            try
            {
                // Construir la ruta completa al nodo que contiene el dato que deseas actualizar
                var rutaNodo = $"Facturas/{idUsuario}/{nombreDato}";

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



