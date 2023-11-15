using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008B7 RID: 2231
public class GameProgressDataNetworkMessage : Serialisable
{
	// Token: 0x06002B6D RID: 11117 RVA: 0x000CB18C File Offset: 0x000C958C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.ProgressData.LastLevelEntered, 8);
		writer.Write(this.ProgressData.NewGamePlusEnabled);
		writer.Write(this.ProgressData.NewGamePlusDialogShown);
		for (int i = 0; i < 2; i++)
		{
			writer.Write(this.MetaDialogsShownStatus[i]);
		}
		uint bits = (uint)this.ProgressData.Levels.Length;
		writer.Write(bits, 8);
		for (int j = 0; j < this.ProgressData.Levels.Length; j++)
		{
			GameProgress.GameProgressData.LevelProgress levelProgress = this.ProgressData.Levels[j];
			writer.Write((uint)levelProgress.LevelId, 8);
			writer.Write(levelProgress.Completed);
			writer.Write(levelProgress.Purchased);
			writer.Write(levelProgress.Revealed);
			writer.Write(levelProgress.ObjectivesCompleted);
			writer.Write(levelProgress.NGPEnabled);
			int num = levelProgress.HighScore;
			if (num == -2147483648)
			{
				num = 65535;
			}
			bool flag = num >= 0;
			writer.Write(flag);
			writer.Write((uint)((!flag) ? (-(uint)num) : num), this.BitsRequiredForHighScore);
			writer.Write((uint)levelProgress.ScoreStars, this.BitsRequiredForStars);
			writer.Write((uint)levelProgress.SurvivalModeTime, this.BitsRequiredForTime);
		}
		uint bits2 = (uint)this.ProgressData.Switches.Length;
		writer.Write(bits2, 8);
		for (int k = 0; k < this.ProgressData.Switches.Length; k++)
		{
			GameProgress.GameProgressData.SwitchState switchState = this.ProgressData.Switches[k];
			writer.Write((uint)switchState.SwitchId, 8);
			writer.Write(switchState.Activated);
		}
		uint bits3 = (uint)this.ProgressData.Teleportals.Length;
		writer.Write(bits3, this.BitsRequiredForTeleportals);
		for (int l = 0; l < this.ProgressData.Teleportals.Length; l++)
		{
			GameProgress.GameProgressData.TeleportalState teleportalState = this.ProgressData.Teleportals[l];
			writer.Write((uint)teleportalState.World, this.BitsRequiredForTeleportals);
		}
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x000CB3B0 File Offset: 0x000C97B0
	public bool Deserialise(BitStreamReader reader)
	{
		this.ProgressData.LastLevelEntered = (int)reader.ReadUInt32(8);
		if (this.ProgressData.LastLevelEntered == 255)
		{
			this.ProgressData.LastLevelEntered = -1;
		}
		this.ProgressData.NewGamePlusEnabled = reader.ReadBit();
		this.ProgressData.NewGamePlusDialogShown = reader.ReadBit();
		for (int i = 0; i < 2; i++)
		{
			this.MetaDialogsShownStatus[i] = reader.ReadBit();
		}
		uint num = reader.ReadUInt32(8);
		this.ProgressData.Levels = new GameProgress.GameProgressData.LevelProgress[num];
		int num2 = 0;
		while ((long)num2 < (long)((ulong)num))
		{
			GameProgress.GameProgressData.LevelProgress levelProgress = new GameProgress.GameProgressData.LevelProgress();
			levelProgress.LevelId = (int)reader.ReadUInt32(8);
			levelProgress.Completed = reader.ReadBit();
			levelProgress.Purchased = reader.ReadBit();
			levelProgress.Revealed = reader.ReadBit();
			levelProgress.ObjectivesCompleted = reader.ReadBit();
			levelProgress.NGPEnabled = reader.ReadBit();
			bool flag = reader.ReadBit();
			levelProgress.HighScore = (int)reader.ReadUInt32(this.BitsRequiredForHighScore);
			if (!flag)
			{
				levelProgress.HighScore = -levelProgress.HighScore;
			}
			levelProgress.ScoreStars = (int)reader.ReadUInt32(this.BitsRequiredForStars);
			levelProgress.SurvivalModeTime = (int)reader.ReadUInt32(this.BitsRequiredForTime);
			this.ProgressData.Levels[num2] = levelProgress;
			num2++;
		}
		uint num3 = reader.ReadUInt32(8);
		this.ProgressData.Switches = new GameProgress.GameProgressData.SwitchState[num3];
		int num4 = 0;
		while ((long)num4 < (long)((ulong)num3))
		{
			GameProgress.GameProgressData.SwitchState switchState = new GameProgress.GameProgressData.SwitchState();
			switchState.SwitchId = (int)reader.ReadUInt32(8);
			switchState.Activated = reader.ReadBit();
			this.ProgressData.Switches[num4] = switchState;
			num4++;
		}
		uint num5 = reader.ReadUInt32(this.BitsRequiredForTeleportals);
		this.ProgressData.Teleportals = new GameProgress.GameProgressData.TeleportalState[num5];
		int num6 = 0;
		while ((long)num6 < (long)((ulong)num5))
		{
			GameProgress.GameProgressData.TeleportalState teleportalState = new GameProgress.GameProgressData.TeleportalState();
			teleportalState.World = (SceneDirectoryData.World)reader.ReadUInt32(this.BitsRequiredForTeleportals);
			this.ProgressData.Teleportals[num6] = teleportalState;
			num6++;
		}
		return true;
	}

	// Token: 0x0400229D RID: 8861
	public bool[] MetaDialogsShownStatus = new bool[2];

	// Token: 0x0400229E RID: 8862
	public GameProgress.GameProgressData ProgressData = new GameProgress.GameProgressData();

	// Token: 0x0400229F RID: 8863
	private const int BitsRequiredForLevelCount = 8;

	// Token: 0x040022A0 RID: 8864
	private int BitsRequiredForHighScore = GameUtils.GetRequiredBitCount(65535);

	// Token: 0x040022A1 RID: 8865
	private int BitsRequiredForStars = GameUtils.GetRequiredBitCount(4);

	// Token: 0x040022A2 RID: 8866
	private int BitsRequiredForTime = GameUtils.GetRequiredBitCount(5999);

	// Token: 0x040022A3 RID: 8867
	private const int BitsRequiredForSwitches = 8;

	// Token: 0x040022A4 RID: 8868
	private int BitsRequiredForTeleportals = GameUtils.GetRequiredBitCount(26);
}
