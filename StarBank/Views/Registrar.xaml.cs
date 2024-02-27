using Firebase.Auth;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Net.Mail;
using System.Net;
namespace StarBank.Views;
using Firebase.Database;
using Firebase.Database.Query;
using StarBank.Models;
using System.Net;
using System.Net.Mail;


public partial class Registrar : ContentPage
{

    public Registrar()
    {

        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);//ELIMINA EL TOOLBAR

    }



    private void irLogin(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Login());//Para cambiar de Pantalla

    }

    //Boton para registrar.
    private async void btn_registrarcuenta_Clicked(object sender, EventArgs e)
    {
        //conexion con firebase igual mente todos los campos paera hacer validaciones 
        ConexionFirebase conexionFirebase = new ConexionFirebase();
        string correo = txt_emailR.Text;
        string contrasenia = txt_passwordR.Text;
        String nombre = txt_nombre.Text;
        String apellido = txt_apellidos.Text;
        String dni = txt_identidad.Text;
        String telefono = txt_telefono.Text;
        String contra2 = txt_passwordRC.Text;
        //VALIDACIONES DE CAMPOS VACIOS :O


      

        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenia) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(telefono) || string.IsNullOrWhiteSpace(contra2))
        {
            await DisplayAlert("Error", "Por favor,Completa todo el Formulario.", "Aceptar");
        }
        else
        {
            if (contrasenia.Length < 6)
            {
                await DisplayAlert("StarBank", "¡La contraseña debe tener al menos 6 caracteres!", "OK");
                return; // Salimos del método si la contraseña es demasiado corta
            }
            //Condicion para verificar el confirmar contraseña.
            if (contrasenia == contra2 || contrasenia.Length > 6)
            {
                // Lógica adicional aquí si la contraseña cumple con los requisitos
                string codigoGenerado = GenerarCodigoAleatorio();
                await EnviarCorreoVerificacion(correo, codigoGenerado);
                string result = string.Empty;
                bool codigoCorrecto = false;

                // Resto del código...




                //usamos un cilco while para que no se cierre la ventana ermegente
                //al mete mal el codigo,si no que seguira abierta pero manda una alerta al usuario... :D
                while (!codigoCorrecto)
                {
                    result = await DisplayPromptAsync("CODIGO DE VERIFICACION", "Por favor, ingrese el codigo que se envio a su correo" +
                        ":", "Aceptar", "Cancelar", keyboard: Keyboard.Numeric);

                    //Manejamos si son iguales
                    if (result == codigoGenerado)
                    {
                        codigoCorrecto = true;
                    }
                    else
                    {
                        await DisplayAlert("StarBank", "¡ERROR EN EL CODIGO DE VERIFICACION!", "OK");
                    }


                    // enviamos una alerta hacemos uso de la conexion para llamar al metodo de crear usuario
                    // generamos numero de cue nta actualmente sigue en prueba.
                    await DisplayAlert("StarBank", "¡USUARIO CREADO CON EXITO!", "OK");
                    //Creacion  de usuario en el apartado de autenticacion
                    //funciones de incio y cierre de sesion 
                    var credenciales = await conexionFirebase.CrearUsuario(correo, contrasenia);
                    string numeroDeCuenta = NumeroDeCuentaGenerator.GenerarNumeroDeCuenta();

                    //Registro en base de datos en firebase.
                    RegistrarUsuarios(nombre, apellido, dni, telefono, correo, numeroDeCuenta);

                    limpiar();
                    await Navigation.PushAsync(new Views.Login());//Para cambiar de Pantalla
                }

            }
            else
            {
                await DisplayAlert("StarBank", "¡ERROR contraseña muy corta o no coincide !", "OK");

            }
        }

    }

    //Metodo para enviar el correo de verificacion
    //actualmente se hace desde un correo personal... :(
    public async Task EnviarCorreoVerificacion(String Email, String cod)
    {

        var fromAddress = new MailAddress("fabriciojosuegarciapena@gmail.com", "STARBANK");
        var toAddress = new MailAddress(Email, "Usuario");
        const string fromPassword = "oliswwttazemnlyn";
        const string subject = "Codigo de Verificacion";
        string body = "Este es el Numero de verificacion: " + cod;

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com", // Cambia esto según tu proveedor de correo
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            smtp.Send(message);
        }
    }

    //Clase para hacer el codigo de verificacion es de manera ramdon asi que siemppre sera diferenete
    //................ :D
    static string GenerarCodigoAleatorio()
    {
        // Creamos una instancia de la clase Random
        Random random = new Random();

        // Creamos una cadena para almacenar el código aleatorio
        string codigo = "";

        // Generamos el código aleatorio
        for (int i = 0; i < 4; i++)
        {
            // Generamos un dígito aleatorio entre 0 y 9
            int digitoAleatorio = random.Next(0, 10);

            // Agregamos el dígito aleatorio a la cadena del código
            codigo += digitoAleatorio.ToString();
        }

        // Retornamos el código aleatorio generado
        return codigo;
    }

    //Metodo para limpiar los campos del formulario ;)
    public void limpiar(){
        txt_emailR.Text = string.Empty;
        txt_passwordR.Text = string.Empty; ;
        txt_nombre.Text = string.Empty; ;
        txt_apellidos.Text = string.Empty; ;
        txt_identidad.Text = string.Empty; ;
        txt_telefono.Text = string.Empty; ;
        txt_passwordRC.Text = string.Empty; ;
    }


    public static class NumeroDeCuentaGenerator
    {
        private static int contador = 1;
        private static object lockObject = new object();

        public static string GenerarNumeroDeCuenta()
        {
            int numeroDeCuenta = 0;

            lock (lockObject)
            {
                numeroDeCuenta = contador;
                contador++;
            }

            if (numeroDeCuenta > 9999)
            {
                throw new InvalidOperationException("Se ha alcanzado el máximo de números de cuenta.");
            }

            return numeroDeCuenta.ToString("00000");
        }
    }

    //METODOS CRUD///

  
    //METODO PARA AGREGAR
    public static void RegistrarUsuarios(String name, String apellido,String identidad,String telefono, String correo,String Cuenta)
    {
        //Estamos diciendoa nuestra base de datos que añadiremos un usuario

        //Nos pide el nelace de nuestra base de datos :D
        FirebaseClient user = new FirebaseClient("https://proyectostarbank-default-rtdb.firebaseio.com/");
        var usuario = user.Child("Usuarios").OnceAsync<Usuarios>();

    
         user.Child("Usuarios").PostAsync(new Usuarios { Nombre =name, Apellidos = apellido,DNI=identidad,Telefono=telefono,Correo=correo,N_Cuenta=Cuenta });
        


    }




}





