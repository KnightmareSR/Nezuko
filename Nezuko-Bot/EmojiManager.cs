using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public class EmojiManager : ModuleBase<SocketCommandContext>
{
	// Example command to add emoji to the server
	[Command("steal")]
	public async Task AddEmojiAsync(string emojiName, string imageUrl)
	{
		var guild = Context.Guild; 

		// Check if the user has permission to manage emojis
		if (Context.User is not SocketGuildUser user || !user.GuildPermissions.ManageEmojisAndStickers)
		{
			await ReplyAsync("You don't have permission to manage emojis in this server.");
			return;
		}

		try
		{
			// Download image from the URL
			var httpClient = new HttpClient();
			var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

			using (var stream = new MemoryStream(imageBytes))
			{
				// Create the emoji
				// Create the image object
				var discordImage = new Discord.Image(stream); 

				// Create the emoji
				var emote = await guild.CreateEmoteAsync(emojiName, discordImage); 

				// Reply with success
				await ReplyAsync($"Emoji {emojiName} has been created successfully!");
			}
		}
		catch (Exception ex)
		{
			// Error handling
			await ReplyAsync($"Failed to create emoji: {ex.Message}");
		}
	}
}
