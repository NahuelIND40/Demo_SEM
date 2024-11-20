#region
using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
#endregion

public class TelegramNotification : BaseNetLogic
{
    private TelegramBot _telegramBot;  // Instancia de la clase TelegramBot

    // Constructor sin parámetros
    public TelegramNotification()
    {
        // Aquí se configura tu token de bot y chatId de Telegram
        string botToken = "7531456685:AAGMeU57zyRwxqo1YgRw3dEU-uVC48Am9_Y";  // Reemplaza con tu token de bot
        string chatId = "5651875803";      // Reemplaza con tu chat ID

        // Instancia el bot de Telegram
        _telegramBot = new TelegramBot(botToken, chatId);
    }

    // Este método se ejecuta cuando la lógica del sistema se inicia
    public override async void Start()
    {
        // Lógica cuando la lógica se inicia, por ejemplo, enviar un mensaje de inicio
        Console.WriteLine("Lógica de RuntimeNetLogic1 iniciada.");
        await SendTelegramMessage("El sistema ha comenzado.");
    }

    // Este método se ejecuta cuando la lógica del sistema se detiene
    public override async void Stop()
    {
        // Lógica cuando la lógica se detiene, por ejemplo, enviar un mensaje de detención
        Console.WriteLine("Lógica de RuntimeNetLogic1 detenida.");
        await SendTelegramMessage("El sistema se ha detenido.");
    }

    // Método de ejemplo, se ejecuta cuando se llama
    [ExportMethod]
    public async void Method1()
    {
        // Lógica cuando se ejecuta este método
        Console.WriteLine("Método 1 ejecutado.");
        await SendTelegramMessage("La aplicación comenzó su ejecución. Para visualizarla, acceda al browser: https://127.0.0.2:8443/ .");
        await SendTelegramMessage("El tanque ha llegado a su capacidad Máxima. Revisar la descarga del mismo .");
    }

    // Método para enviar un mensaje a Telegram
    private async Task SendTelegramMessage(string message)
    {
        try
        {
            // Llamada a TelegramBot para enviar un mensaje
            await _telegramBot.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            // Manejo de errores
            Console.WriteLine($"Error al enviar el mensaje: {ex.Message}");
        }
    }

    // Clase interna TelegramBot que maneja la lógica de enviar mensajes a Telegram
    public class TelegramBot
    {
        private readonly string _botToken;
        private readonly string _chatId;

        // Constructor que recibe el token del bot y el ID del chat
        public TelegramBot(string botToken, string chatId)
        {
            _botToken = botToken;
            _chatId = chatId;
        }

        // Método asíncrono para enviar un mensaje a Telegram
        public async Task SendMessageAsync(string message)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    chat_id = _chatId,
                    text = message
                }), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                string responseContent = await response.Content.ReadAsStringAsync();

                // Log de la respuesta de la API de Telegram (puedes cambiarlo por una librería de logs si es necesario)
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mensaje enviado exitosamente.");
                }
                else
                {
                    Console.WriteLine($"Error al enviar el mensaje. Código: {response.StatusCode}, Respuesta: {responseContent}");
                }
            }
        }
    }
}
