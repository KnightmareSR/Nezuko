using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot 
{ 
public class Commands : ModuleBase<SocketCommandContext>
{
	


	[Command("Info")]
	public async Task Test(SocketUser user) 
	{
			var avi = user.GetAvatarUrl();
			var created = user.CreatedAt;
			var activity = user.Status;
			var userName = user.Username;
			var embed = new EmbedBuilder();
			

			embed.WithThumbnailUrl(avi);
			embed.WithTitle(userName);
			embed.WithDescription($"{user.Mention} Profile info ");
			embed.AddField($" Date Created {created} ", false);
			embed.AddField($"Status: {activity} ", " Status", false);
			embed.WithColor(Color.Blue);

			var sent = await Context.Channel.SendMessageAsync("", false, embed.Build());




	}


	}

}
