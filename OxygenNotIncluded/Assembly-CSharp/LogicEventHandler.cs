using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200084E RID: 2126
internal class LogicEventHandler : ILogicEventReceiver, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06003E01 RID: 15873 RVA: 0x0015891A File Offset: 0x00156B1A
	public LogicEventHandler(int cell, Action<int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x0015893F File Offset: 0x00156B3F
	public void ReceiveLogicEvent(int value)
	{
		this.TriggerAudio(value);
		this.value = value;
		this.onValueChanged(value);
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06003E03 RID: 15875 RVA: 0x0015895B File Offset: 0x00156B5B
	public int Value
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x00158963 File Offset: 0x00156B63
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x0015896B File Offset: 0x00156B6B
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00158973 File Offset: 0x00156B73
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06003E07 RID: 15879 RVA: 0x00158985 File Offset: 0x00156B85
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06003E08 RID: 15880 RVA: 0x00158997 File Offset: 0x00156B97
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x001589A0 File Offset: 0x00156BA0
	private void TriggerAudio(int new_value)
	{
		LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (networkForCell != null && new_value != this.value && instance != null && !instance.IsPaused)
		{
			if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
			{
				return;
			}
			string name = "Logic_Building_Toggle";
			if (!CameraController.Instance.IsAudibleSound(Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
			{
				return;
			}
			LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
			Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = LogicCircuitNetwork.logicSoundRegister;
			int id = networkForCell.id;
			if (!logicSoundRegister.ContainsKey(id))
			{
				logicSoundRegister.Add(id, logicSoundPair);
			}
			else
			{
				logicSoundPair.playedIndex = logicSoundRegister[id].playedIndex;
				logicSoundPair.lastPlayed = logicSoundRegister[id].lastPlayed;
			}
			if (logicSoundPair.playedIndex < 2)
			{
				logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
			}
			else
			{
				logicSoundRegister[id].playedIndex = 0;
				logicSoundRegister[id].lastPlayed = Time.time;
			}
			float num = (Time.time - logicSoundPair.lastPlayed) / 3f;
			EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), Grid.CellToPos(this.cell), 1f);
			instance2.setParameterByName("logic_volumeModifer", num, false);
			instance2.setParameterByName("wireCount", (float)(networkForCell.WireCount % 24), false);
			instance2.setParameterByName("enabled", (float)new_value, false);
			KFMOD.EndOneShot(instance2);
		}
	}

	// Token: 0x06003E0A RID: 15882 RVA: 0x00158B50 File Offset: 0x00156D50
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x0400284C RID: 10316
	private int cell;

	// Token: 0x0400284D RID: 10317
	private int value;

	// Token: 0x0400284E RID: 10318
	private Action<int> onValueChanged;

	// Token: 0x0400284F RID: 10319
	private Action<int, bool> onConnectionChanged;

	// Token: 0x04002850 RID: 10320
	private LogicPortSpriteType spriteType;
}
