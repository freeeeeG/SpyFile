using System;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008BD RID: 2237
public class HighScoresMessage : Serialisable
{
	// Token: 0x06002B87 RID: 11143 RVA: 0x000CB82C File Offset: 0x000C9C2C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_Machine, this.BitsRequiredForMachineID);
		bool flag = this.DLC != -1;
		writer.Write(flag);
		if (flag)
		{
			writer.Write((uint)this.DLC, 4);
		}
		uint count = (uint)this.HighScores.Scores.Count;
		writer.Write(count, 8);
		int num = 0;
		while ((long)num < (long)((ulong)count))
		{
			writer.Write((uint)this.HighScores.Scores[num].iLevelID, 8);
			int iHighScore = this.HighScores.Scores[num].iHighScore;
			int iSurvivalModeTime = this.HighScores.Scores[num].iSurvivalModeTime;
			bool flag2 = iHighScore >= 0;
			writer.Write(flag2);
			writer.Write((uint)((!flag2) ? (-(uint)iHighScore) : iHighScore), this.BitsRequiredForHighScore);
			writer.Write((uint)iSurvivalModeTime, this.BitsRequiredForTime);
			num++;
		}
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x000CB928 File Offset: 0x000C9D28
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_Machine = (User.MachineID)reader.ReadUInt32(this.BitsRequiredForMachineID);
		bool flag = reader.ReadBit();
		if (flag)
		{
			this.DLC = (int)reader.ReadUInt32(4);
		}
		else
		{
			this.DLC = -1;
		}
		uint num = reader.ReadUInt32(8);
		this.HighScores.Scores.Clear();
		int num2 = 0;
		while ((long)num2 < (long)((ulong)num))
		{
			int iLevelID = (int)reader.ReadUInt32(8);
			bool flag2 = reader.ReadBit();
			int num3 = (int)reader.ReadUInt32(this.BitsRequiredForHighScore);
			int iSurvivalModeTime = (int)reader.ReadUInt32(this.BitsRequiredForTime);
			if (!flag2)
			{
				num3 = -num3;
			}
			this.HighScores.Scores.Add(new GameProgress.HighScores.Score
			{
				iLevelID = iLevelID,
				iHighScore = num3,
				iSurvivalModeTime = iSurvivalModeTime
			});
			num2++;
		}
		return true;
	}

	// Token: 0x040022AF RID: 8879
	public GameProgress.HighScores HighScores = new GameProgress.HighScores();

	// Token: 0x040022B0 RID: 8880
	public int DLC = -1;

	// Token: 0x040022B1 RID: 8881
	public User.MachineID m_Machine;

	// Token: 0x040022B2 RID: 8882
	private const int BitsRequiredForLevelCount = 8;

	// Token: 0x040022B3 RID: 8883
	private readonly int BitsRequiredForHighScore = GameUtils.GetRequiredBitCount(65535);

	// Token: 0x040022B4 RID: 8884
	private readonly int BitsRequiredForTime = GameUtils.GetRequiredBitCount(5999);

	// Token: 0x040022B5 RID: 8885
	private readonly int BitsRequiredForMachineID = GameUtils.GetRequiredBitCount(4);
}
