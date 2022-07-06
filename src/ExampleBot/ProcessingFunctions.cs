using System;
using DSharpPlus;
using System.Threading.Tasks;
using System.Linq;


namespace ExampleBot
{
    public static class ProcessingFunctions
    {
        public static async Task DiscordClient_GuildDownloadCompleted(DiscordClient sender, DSharpPlus.EventArgs.GuildDownloadCompletedEventArgs e)
        {
            // if you have allowed your bot correctly - you should see an ulong here
            Console.WriteLine($"Connected guilds : {string.Join(",", e.Guilds.Keys)}");
        }

        public static async Task DiscordClient_MessageCreated(DiscordClient sender, DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            // output the data we got from discord to console log
            Console.WriteLine($"{e.Author.Username}: {e.Message.Content}");

            // example matching pattern
            if (e.Message.Content.StartsWith("!ping")
                // check if bot was mentioned
                && e.MentionedUsers.Any(c => c.Id == sender.CurrentUser.Id)
            )
            {
                // responde with a message
                await e.Channel.SendMessageAsync("PONG");
            }

        }

    }
}
