using System;
using DSharpPlus;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ExampleBot
{
    internal class Program
    {
        private static string BotName { get; set; }
        private static DiscordClient _discordClient;
        static async Task Main(string[] args)
        {

            // example of loading IConfiguration into console application. In more recent .net this is the main configuration object
            // in older dotnet this was web.config xml and ConfigurationManager

            IConfiguration configuration = new ConfigurationBuilder()
                // Always load this file - this would be loaded on a server / host
                .AddJsonFile("appsettings.json", optional: false)
                // optionally load the .development version hidden from git - you would put your local versions here
                .AddJsonFile("appsettings.development.json", optional: true)
                .Build();


            BotName = configuration.GetSection("BotName").Value;

            // there are 3 instantioations in c#
            // body
            // constructor
            // body and constructor

            // example of instantiation with only body params
            var config = new DiscordConfiguration()
            {
                // __ is habbit. A log of keys in config use : . However, in docker, this is not reliable, : are replaced with __ working cross platform 
                // Null coelesce from config. If it is not there, use enviroment variables (This is important for running in docker)
                // ?? coelesce  ?.value checks if null, if not gets value
                Token = configuration.GetSection("Discord:Token")?.Value ?? Environment.GetEnvironmentVariable("Discord__Client__Example"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,

            };
            // example of  instantiation through counstructor only 
            _discordClient = new DiscordClient(config);

            // event drivent methods. Discord client will raise this event and call the attached function - https://docs.microsoft.com/en-us/dotnet/api/system.eventhandler?view=netcore-3.1
            _discordClient.MessageCreated += ProcessingFunctions.DiscordClient_MessageCreated;
            // just an event to test its working . You would use this event to do things that require knowing the channesl (guilds) that are connected 
            _discordClient.GuildDownloadCompleted += ProcessingFunctions.DiscordClient_GuildDownloadCompleted;

            // connect to discord
            await _discordClient.ConnectAsync();

            // set bot name based off the configuration in json
            await _discordClient.UpdateCurrentUserAsync(BotName);

            //
            // Just some c# overview
            //

            // third type with both - Shows that constructor is set first, then body
            var example = new ExampleBoth(1, 2)
            {
                SetOnlyBody = 3,
                SetBoth = 4
            };

            // Constructor will be 1, body 3, Both 3
            // also -  string interpolation - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"Example 1 {JsonConvert.SerializeObject(example)}");
            // this example shows that SetBoth is being set in contructor
            var example2 = new ExampleBoth(1, 2)
            {
                SetOnlyBody = 3,
            };
            // Constructor will be 1, both 2, body 3
            Console.WriteLine($"Example 2 {JsonConvert.SerializeObject(example2)}");


            // keeps the task alive forever (Tasks run on background threads)
            // it locks this thread, but as everything is event driven they are done on different threads
            await Task.Delay(-1);
        }

      
    }
}
