using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Steamworks;

public class SteamAvatar
{
	
	#region Variables

	static Dictionary<CSteamID, Texture2D> _Cache = new Dictionary<CSteamID, Texture2D> ();

	static ulong _AvatarTaskCount = 0;
	static Dictionary<ulong, Callback<AvatarImageLoaded_t>> _AvatarTaskList = new Dictionary<ulong, Callback<AvatarImageLoaded_t>>();

	#endregion



	#region Functions

	public static void GetUserAvatar (CSteamID steamID, System.Action<Texture2D> callback)
	{
		if (_Cache.ContainsKey (steamID))
		{
			callback (_Cache[steamID]);
			return;
		}

		int userAvatar = SteamFriends.GetLargeFriendAvatar (steamID);
		uint imageWidth;
		uint imageHeight;

		bool restartAvatarLoad = true;
		bool success = SteamUtils.GetImageSize (userAvatar, out imageWidth, out imageHeight);

		if (success && imageWidth > 0 && imageHeight > 0)
		{
			byte[] data = new byte[imageWidth * imageHeight * 4];
			var returnTex = new Texture2D ((int)imageWidth, (int)imageHeight, TextureFormat.RGBA32, false, false);

			success = SteamUtils.GetImageRGBA (userAvatar, data, (int)(imageWidth * imageHeight * 4));
			if (success)
			{
				restartAvatarLoad = false;
				returnTex.LoadRawTextureData (data);
				returnTex.Apply ();

				//NOTE: texture loads upside down, so we flip it to normal...
				var result = FlipTexture (returnTex);

				_Cache.Add (steamID, result);

				callback (result);

				Texture2D.DestroyImmediate (returnTex);
			}
		}

		if (restartAvatarLoad)
		{
			ulong key = _AvatarTaskCount;
			_AvatarTaskCount++;

			var task = new Callback<AvatarImageLoaded_t> (delegate(AvatarImageLoaded_t param) 
			{
				GetUserAvatar (steamID, callback);
				_AvatarTaskList.Remove (key);
			});

			_AvatarTaskList.Add (key, task);
		}
	}

	static Texture2D FlipTexture(Texture2D original)
	{
		Texture2D flipped = new Texture2D(original.width, original.height);
	     
		int xN = original.width;
		int yN = original.height;
	     
		for(int i=0;i<xN;i++)
		{
			for(int j=0;j<yN;j++)
			{
				flipped.SetPixel(i, yN-j-1, original.GetPixel(i,j));
			}
		}

		flipped.Apply();
	     
		return flipped;
	}
	#endregion

}

