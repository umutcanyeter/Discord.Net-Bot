using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Required libraries
using Discord;
using Discord.Commands;


namespace chhe_BOT
{
    class MyBot
    {
        Random rnd;
        DiscordClient discord;
        CommandService commands;
        public MyBot()
        {
            rnd = new Random();
            String[] pic = new string[]
            {
                "Files/14.jpg",
                "Files/9.jpg",
                "Files/3.jpg"
            };
            discord = new DiscordClient(x =>
            {
                x.AppName = "Bot Name";
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });
            commands = discord.GetService<CommandService>();
            //Reorganize delete message command
            registerpurgecommand();
            //Reorganize send photo commad
            sendphotocommand();
            //Commands
            commands.CreateCommand("hi")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Hello " + e.User.Mention);
                });
            //Reorganize delete messages command
            commands.CreateCommand("say").Parameter("message", ParameterType.Multiple)
                .Do(async (e) =>
                {
                    await DoAnnouncement(e);
                });
            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MzI5OTQ2Mjk2MjI2MDIxMzc4.DDfTxQ.O86HWE_L4KS6e2ixFBqJHiibvC0", TokenType.Bot);
            });
            //Delete messages command
            private void registerpurgecommand()
            {
                var commands = discord.GetService<CommandService>();
                commands.CreateCommand("delete")
                    .Do(async (e) =>
                    {
                        Message[] messagesToDelete;
                        messagesToDelete = await e.Channel.DownloadMessages(2);

                        await e.Channel.DeleteMessages(messagesToDelete);
                    });
            }
            private async Task DoAnnouncement(CommandEventArgs e)
            {
                var channel = e.Server.FindChannels(e.Args[0], ChannelType.Text).FirstOrDefault();
                var message = ConstructMessage(e, channel != null);
                if (channel != null)
                {
                    await channel.SendMessage(message.ToString());
                }
                else
                {
                    await e.Channel.SendMessage(message.ToString());
                }
            }
            private object ConstructMessage(CommandEventArgs e, bool firstArgsChannel)
            {
                string message = "";

                var name = e.User.Nickname != null ? e.User.Nickname : e.User.Name;

                var startIndex = firstArgsChannel ? 1 : 0;

                for (int i = startIndex; i < e.Args.Length; i++)
                {
                    message += e.Args[i].ToString() + " ";
                }

                var result = name + " *" + "**" + message + "**" + "*" + "dedi.";//output, you can modify here what you want

                return result;
            }
            //Send photo command
            private void sendphotocommand()
            {
                commands.CreateCommand("pic")
                    .Do(async (e) =>
                    {
                        int randomPicIndex = rnd.Next(pic.Length);
                        string PicToPost = pic[randomPicIndex];
                        await e.Channel.SendFile(PicToPost);
                    });
            }
            //Log Messages
            private void Log(object sender, LogMessageEventArgs e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
