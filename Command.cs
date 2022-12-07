using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord;
using Newtonsoft.Json;
using Discord.WebSocket;

namespace Discord_LolyBot
{
    internal class Command
    {
        private static DiscordSocketClient? Client { get; set; }
        private static SocketGuild? Guild { get; set; }

        public Command(DiscordSocketClient _client, SocketGuild _guild)
        {
            Console.WriteLine("\nCréation des commandes...");
            Client = _client;
            Guild = _guild;
            GlobalCommandCreate(Client);
            Console.WriteLine("Création des commandes OK !\n");
        }

        private static Task GlobalCommandCreate(DiscordSocketClient client)
        {
            
            List<SlashCommandBuilder> commands = new List<SlashCommandBuilder>
            {
                new()
                {
                    Name = "help",
                    Description = "Shows all commands"
                },
                new()
                {
                    Name = "aoc",
                    Description = "Shows informations for Advent Of Code",
                    Options = new List<SlashCommandOptionBuilder>
                    {
                        new()
                        {
                            Name = "join",
                            Description = "Join the team",
                            Type = ApplicationCommandOptionType.SubCommand
                        },
                        new()
                        {
                            Name = "stats",
                            Description = "Shows the leaderboard",
                            Type = ApplicationCommandOptionType.SubCommand
                        }
                    }
                }
            };
            
            foreach(SlashCommandBuilder command in commands)
            {
                try
                {
                    Console.WriteLine($" -  Création de la commande { command.Name }...");
                    client.CreateGlobalApplicationCommandAsync(command.Build());
                }
                catch(ApplicationCommandException e)
                {
                    Console.WriteLine($" -  Création de la commande {command.Name} ERROR");
                    var json = JsonConvert.SerializeObject(e.Errors, Formatting.Indented);
                    Console.WriteLine(json);
                }
            }
            return Task.CompletedTask;
        }

        public static async Task HelpCommandExecute(SocketSlashCommand command)
        {
            EmbedBuilder? embed = new()
            {
                Title = "Help",
                Description = "HENTAI !",
                Timestamp = DateTime.Now,
                Color = Color.DarkTeal
            };
            Console.WriteLine($"Command Executed '{ command.Data.Name }' : {Client?.GetGuild(id: (ulong)command.GuildId).Name} -> {command.Channel.Name}");
            await command.RespondAsync(embed: embed.Build());
        }

        public static async Task aocCommandExecute(SocketSlashCommand command)
        {
            switch(command.Data.Options.First().Name)
            {
                case "join":
                    await command.RespondAsync("1095630-a9ede6a5");
                    break;
                    
                case "stats":
                    EmbedBuilder embed = new()
                    {
                        Title = "Leaderboard",
                        Description = AdventOfCode.GetStats().Result,
                        Timestamp = DateTime.Now,
                        Color = Color.DarkTeal
                    };
                    await command.RespondAsync(embed: embed.Build());
                    break;
            }
        }
    }
}
