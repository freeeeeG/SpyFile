using System;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020006DD RID: 1757
public class LobbyServerMessage : Serialisable
{
	// Token: 0x0600213B RID: 8507 RVA: 0x0009EA1C File Offset: 0x0009CE1C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_type, 3);
		switch (this.m_type)
		{
		case LobbyServerMessage.LobbyMessageType.StateChange:
			writer.Write((uint)this.m_stateChange.m_state, 3);
			writer.Write(this.m_stateChange.m_bIsCoop);
			writer.Write((uint)this.m_stateChange.m_sessionVisibility, 2);
			writer.Write((uint)this.m_stateChange.m_connectionMode, 2);
			break;
		case LobbyServerMessage.LobbyMessageType.TimerUpdate:
			writer.Write(this.m_timerInfo.m_timerVal);
			break;
		case LobbyServerMessage.LobbyMessageType.ResetTimer:
			writer.Write(this.m_timerInfo.m_timerVal);
			break;
		case LobbyServerMessage.LobbyMessageType.SelectionUpdate:
			writer.Write((uint)this.m_selectionUpdate.m_theme, SceneDirectoryData.c_bitsPerTheme);
			writer.Write((uint)this.m_selectionUpdate.m_chefIndex, 2);
			break;
		case LobbyServerMessage.LobbyMessageType.FinalSelection:
			writer.Write((uint)this.m_selectionUpdate.m_theme, SceneDirectoryData.c_bitsPerTheme);
			writer.Write((uint)this.m_selectionUpdate.m_chefIndex, 2);
			break;
		case LobbyServerMessage.LobbyMessageType.CreateGameSession:
		{
			bool flag = this.m_dlcID != -1;
			writer.Write(flag);
			if (flag)
			{
				writer.Write((uint)this.m_dlcID, 4);
			}
			break;
		}
		}
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x0009EB64 File Offset: 0x0009CF64
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_type = (LobbyServerMessage.LobbyMessageType)reader.ReadUInt32(3);
		switch (this.m_type)
		{
		case LobbyServerMessage.LobbyMessageType.StateChange:
			this.m_stateChange = default(LobbyServerMessage.StateChange);
			this.m_stateChange.m_state = (LobbyFlowController.LobbyState)reader.ReadUInt32(3);
			this.m_stateChange.m_bIsCoop = reader.ReadBit();
			this.m_stateChange.m_sessionVisibility = (OnlineMultiplayerSessionVisibility)reader.ReadUInt32(2);
			this.m_stateChange.m_connectionMode = (OnlineMultiplayerConnectionMode)reader.ReadUInt32(2);
			break;
		case LobbyServerMessage.LobbyMessageType.TimerUpdate:
			this.m_timerInfo = default(LobbyServerMessage.TimerInfo);
			this.m_timerInfo.m_timerVal = reader.ReadFloat32();
			break;
		case LobbyServerMessage.LobbyMessageType.ResetTimer:
			this.m_timerInfo = default(LobbyServerMessage.TimerInfo);
			this.m_timerInfo.m_timerVal = reader.ReadFloat32();
			break;
		case LobbyServerMessage.LobbyMessageType.SelectionUpdate:
			this.m_selectionUpdate = default(LobbyServerMessage.SelectionUpdate);
			this.m_selectionUpdate.m_theme = (SceneDirectoryData.LevelTheme)reader.ReadUInt32(SceneDirectoryData.c_bitsPerTheme);
			this.m_selectionUpdate.m_chefIndex = (int)reader.ReadUInt32(2);
			break;
		case LobbyServerMessage.LobbyMessageType.FinalSelection:
			this.m_selectionUpdate = default(LobbyServerMessage.SelectionUpdate);
			this.m_selectionUpdate.m_theme = (SceneDirectoryData.LevelTheme)reader.ReadUInt32(SceneDirectoryData.c_bitsPerTheme);
			this.m_selectionUpdate.m_chefIndex = (int)reader.ReadUInt32(2);
			break;
		case LobbyServerMessage.LobbyMessageType.CreateGameSession:
		{
			bool flag = reader.ReadBit();
			if (flag)
			{
				this.m_dlcID = (int)reader.ReadUInt32(4);
			}
			else
			{
				this.m_dlcID = -1;
			}
			break;
		}
		default:
			return false;
		}
		return true;
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x0009ECF8 File Offset: 0x0009D0F8
	public bool ToSendReliable()
	{
		return this.m_type != LobbyServerMessage.LobbyMessageType.TimerUpdate;
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x0009ED08 File Offset: 0x0009D108
	public override string ToString()
	{
		string text = base.GetType() + "(" + this.m_type;
		switch (this.m_type)
		{
		case LobbyServerMessage.LobbyMessageType.StateChange:
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				", ",
				this.m_stateChange.m_state,
				", ",
				this.m_stateChange.m_bIsCoop,
				", ",
				this.m_stateChange.m_sessionVisibility,
				", ",
				this.m_stateChange.m_connectionMode
			});
			break;
		}
		case LobbyServerMessage.LobbyMessageType.TimerUpdate:
			text = text + ", " + this.m_timerInfo;
			break;
		case LobbyServerMessage.LobbyMessageType.ResetTimer:
			text = text + ", " + this.m_timerInfo;
			break;
		case LobbyServerMessage.LobbyMessageType.SelectionUpdate:
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				", SelectionUpdate(",
				this.m_selectionUpdate.m_theme,
				", ",
				this.m_selectionUpdate.m_chefIndex,
				")"
			});
			break;
		}
		}
		return text + ")";
	}

	// Token: 0x04001956 RID: 6486
	public const int kBitsPerLobbyMsgType = 3;

	// Token: 0x04001957 RID: 6487
	public LobbyServerMessage.LobbyMessageType m_type;

	// Token: 0x04001958 RID: 6488
	public LobbyServerMessage.StateChange m_stateChange;

	// Token: 0x04001959 RID: 6489
	public LobbyServerMessage.TimerInfo m_timerInfo;

	// Token: 0x0400195A RID: 6490
	public LobbyServerMessage.SelectionUpdate m_selectionUpdate;

	// Token: 0x0400195B RID: 6491
	public int m_dlcID = -1;

	// Token: 0x020006DE RID: 1758
	public enum LobbyMessageType
	{
		// Token: 0x0400195D RID: 6493
		StateChange,
		// Token: 0x0400195E RID: 6494
		TimerUpdate,
		// Token: 0x0400195F RID: 6495
		ResetTimer,
		// Token: 0x04001960 RID: 6496
		SelectionUpdate,
		// Token: 0x04001961 RID: 6497
		FinalSelection,
		// Token: 0x04001962 RID: 6498
		CreateGameSession
	}

	// Token: 0x020006DF RID: 1759
	public struct StateChange
	{
		// Token: 0x04001963 RID: 6499
		public const int kBitsPerLobbyState = 3;

		// Token: 0x04001964 RID: 6500
		public LobbyFlowController.LobbyState m_state;

		// Token: 0x04001965 RID: 6501
		public bool m_bIsCoop;

		// Token: 0x04001966 RID: 6502
		public const int kBitsPerVisiblity = 2;

		// Token: 0x04001967 RID: 6503
		public OnlineMultiplayerSessionVisibility m_sessionVisibility;

		// Token: 0x04001968 RID: 6504
		public const int kBitsPerConnectionMode = 2;

		// Token: 0x04001969 RID: 6505
		public OnlineMultiplayerConnectionMode m_connectionMode;
	}

	// Token: 0x020006E0 RID: 1760
	public struct TimerInfo
	{
		// Token: 0x0400196A RID: 6506
		public float m_timerVal;
	}

	// Token: 0x020006E1 RID: 1761
	public struct SelectionUpdate
	{
		// Token: 0x0400196B RID: 6507
		public SceneDirectoryData.LevelTheme m_theme;

		// Token: 0x0400196C RID: 6508
		public const int kBitsPerChefIndex = 2;

		// Token: 0x0400196D RID: 6509
		public int m_chefIndex;
	}
}
