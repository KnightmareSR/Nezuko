using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Nezuko_Bot
{
	 public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private string token = String.Empty;


		public static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            // Initialize Discord client with MessageContent intent
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            // Initialize command service
            _commands = new CommandService();

            // Configure services and add dependencies
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            // Replace with environment variable for security
            token = await LoadTokenAsync("token.txt");
  

            // Attach event handlers
            _client.Log += LogAsync;
            _client.MessageReceived += HandleCommandAsync;

            // Register commands and start the bot
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            Console.WriteLine(token);

            // Keep the bot running
            await Task.Delay(-1);
        }

        //Load the token securley 
		private static async Task<string> LoadTokenAsync(string fileName)
		{
			string filePath = Path.Combine(AppContext.BaseDirectory, fileName);

			try
			{
				return await File.ReadAllTextAsync(filePath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error reading token file: {ex.Message}");
				return null;
			}
		}

		private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Ensure message is from a user and is not empty
            if (messageParam is not SocketUserMessage message) return;
            
            Console.WriteLine($"Received message: {message.Content}");

            // Set prefix position and check if message starts with prefix or bot mention
            int argPos = 0;
            if (!(message.HasCharPrefix('.', ref argPos) || 
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || 
                message.Author.IsBot)
                return;

            // Execute command
            var context = new SocketCommandContext(_client, message);
            Console.WriteLine("Executing command");

            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Error: {result.ErrorReason}");
            }
        }
    }
}