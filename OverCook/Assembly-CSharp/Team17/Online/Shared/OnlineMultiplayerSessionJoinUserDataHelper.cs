using System;
using System.Collections.Generic;
using BitStream;

namespace Team17.Online.Shared
{
	// Token: 0x02000975 RID: 2421
	public static class OnlineMultiplayerSessionJoinUserDataHelper
	{
		// Token: 0x06002F3F RID: 12095 RVA: 0x000DCB40 File Offset: 0x000DAF40
		public static bool Serialize(List<OnlineMultiplayerSessionJoinLocalUserData> inputData, BitStreamWriter writer)
		{
			try
			{
				writer.Write((uint)inputData.Count, BitHelper.CalculateRequirement(OnlineMultiplayerConfig.MaxPlayers));
				for (int i = 0; i < inputData.Count; i++)
				{
					writer.Write(inputData[i].GameDataSize, BitHelper.CalculateRequirement(OnlineMultiplayerConfig.MaxTransportMessageSize));
				}
				for (int j = 0; j < inputData.Count; j++)
				{
					byte[] gameData = inputData[j].GameData;
					uint gameDataSize = inputData[j].GameDataSize;
					for (uint num = 0U; num < gameDataSize; num += 1U)
					{
						writer.Write(gameData[(int)((UIntPtr)num)], 8);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
			}
			return false;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000DCC0C File Offset: 0x000DB00C
		public static bool Deserialize(BitStreamReader reader, List<OnlineMultiplayerSessionJoinRemoteUserData> output)
		{
			try
			{
				if (output != null && reader != null)
				{
					output.Clear();
					uint num = reader.ReadUInt32(BitHelper.CalculateRequirement(OnlineMultiplayerConfig.MaxPlayers));
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						uint num3 = reader.ReadUInt32(BitHelper.CalculateRequirement(OnlineMultiplayerConfig.MaxTransportMessageSize));
						output.Add(new OnlineMultiplayerSessionJoinRemoteUserData
						{
							GameData = new byte[num3],
							GameDataSize = num3
						});
					}
					for (uint num4 = 0U; num4 < num; num4 += 1U)
					{
						byte[] gameData = output[(int)num4].GameData;
						uint gameDataSize = output[(int)num4].GameDataSize;
						int num5 = 0;
						while ((long)num5 < (long)((ulong)gameDataSize))
						{
							gameData[num5] = reader.ReadByte(8);
							num5++;
						}
					}
					return true;
				}
			}
			catch (Exception ex)
			{
				output.Clear();
			}
			return false;
		}
	}
}
