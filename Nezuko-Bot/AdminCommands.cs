using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


	public class AdminCommands : ModuleBase<SocketCommandContext>
	{
		
		[Command("Kick")]
		private async Task KickMember(IGuildUser user, [Remainder]string reason) 
		{
			try
			{
				await user.KickAsync(reason);
				await ReplyAsync($"{user.Username} has been kicked for: {reason}");

			}
			catch (Exception e)
			{

				await ReplyAsync($"Failed to kick {user.Username}: {e.Message}");
			}
		} 
	}
