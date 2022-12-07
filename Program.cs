using System;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace Discord_LolyBot
{
    internal class Program
    {
        private DiscordSocketClient? _client;
        private Command? _command;

        public static Task Main() => new Program().DiscordBotAsync();

        public async Task DiscordBotAsync()
        {
            Console.WriteLine("Bot is starting...");
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.Ready += DiscordBotReady;
            _client.SlashCommandExecuted += SlashCommandExecuted;

            string? token = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"))?.Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public Task DiscordBotReady()
        {
            if(_client == null)
            {
                Console.WriteLine("Client is null!");
                return Task.CompletedTask;
            }
            else
            {
                Console.WriteLine("Bot is ready!");
            }
            
            Console.WriteLine($"\nNombre de Serveurs : { _client.Guilds.Count }");
            foreach(SocketGuild guild in _client.Guilds)
            {
                Console.WriteLine($" -  Guild : { guild.Name}");
                _command = new Command(_client, guild);
            }

            return Task.CompletedTask;
        }

        private async Task SlashCommandExecuted(SocketSlashCommand command)
        {
            switch(command.Data.Name)
            {                
                case "help":
                    await Command.HelpCommandExecute(command);
                    break;
                    
                case "aoc":
                    await Command.aocCommandExecute(command);
                    break;
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}