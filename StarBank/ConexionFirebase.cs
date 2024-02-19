using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using FirebaseAdmin.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using Plugin.FirebaseAuth;



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
                Providers = new FirebaseAuthProvider[]
            {
                // Add and configure individual providers
                new GoogleProvider().AddScopes("email"),
                new EmailProvider()
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
