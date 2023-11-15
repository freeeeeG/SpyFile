using System;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020007CB RID: 1995
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GameClock")]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
	// Token: 0x060037B5 RID: 14261 RVA: 0x0012EF06 File Offset: 0x0012D106
	public static void DestroyInstance()
	{
		GameClock.Instance = null;
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x0012EF0E File Offset: 0x0012D10E
	protected override void OnPrefabInit()
	{
		GameClock.Instance = this;
		this.timeSinceStartOfCycle = 50f;
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x0012EF24 File Offset: 0x0012D124
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.time != 0f)
		{
			this.cycle = (int)(this.time / 600f);
			this.timeSinceStartOfCycle = Mathf.Max(this.time - (float)this.cycle * 600f, 0f);
			this.time = 0f;
		}
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x0012EF80 File Offset: 0x0012D180
	public void Sim33ms(float dt)
	{
		this.AddTime(dt);
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x0012EF89 File Offset: 0x0012D189
	public void Render1000ms(float dt)
	{
		this.timePlayed += dt;
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x0012EF99 File Offset: 0x0012D199
	private void LateUpdate()
	{
		this.frame++;
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x0012EFAC File Offset: 0x0012D1AC
	private void AddTime(float dt)
	{
		this.timeSinceStartOfCycle += dt;
		bool flag = false;
		while (this.timeSinceStartOfCycle >= 600f)
		{
			this.cycle++;
			this.timeSinceStartOfCycle -= 600f;
			base.Trigger(631075836, null);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.Trigger(631075836, null);
			}
			flag = true;
		}
		if (!this.isNight && this.IsNighttime())
		{
			this.isNight = true;
			base.Trigger(-722330267, null);
		}
		if (this.isNight && !this.IsNighttime())
		{
			this.isNight = false;
		}
		if (flag && SaveGame.Instance.AutoSaveCycleInterval > 0 && this.cycle % SaveGame.Instance.AutoSaveCycleInterval == 0)
		{
			this.DoAutoSave(this.cycle);
		}
		int num = Mathf.FloorToInt(this.timeSinceStartOfCycle - dt / 25f);
		int num2 = Mathf.FloorToInt(this.timeSinceStartOfCycle / 25f);
		if (num != num2)
		{
			base.Trigger(-1215042067, num2);
		}
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x0012F0F0 File Offset: 0x0012D2F0
	public float GetTimeSinceStartOfReport()
	{
		if (this.IsNighttime())
		{
			return 525f - this.GetTimeSinceStartOfCycle();
		}
		return this.GetTimeSinceStartOfCycle() + 75f;
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x0012F113 File Offset: 0x0012D313
	public float GetTimeSinceStartOfCycle()
	{
		return this.timeSinceStartOfCycle;
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x0012F11B File Offset: 0x0012D31B
	public float GetCurrentCycleAsPercentage()
	{
		return this.timeSinceStartOfCycle / 600f;
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x0012F129 File Offset: 0x0012D329
	public float GetTime()
	{
		return this.timeSinceStartOfCycle + (float)this.cycle * 600f;
	}

	// Token: 0x060037C0 RID: 14272 RVA: 0x0012F13F File Offset: 0x0012D33F
	public float GetTimeInCycles()
	{
		return (float)this.cycle + this.GetCurrentCycleAsPercentage();
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x0012F14F File Offset: 0x0012D34F
	public int GetFrame()
	{
		return this.frame;
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x0012F157 File Offset: 0x0012D357
	public int GetCycle()
	{
		return this.cycle;
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x0012F15F File Offset: 0x0012D35F
	public bool IsNighttime()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875f;
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x0012F175 File Offset: 0x0012D375
	public float GetDaytimeDurationInPercentage()
	{
		return 0.875f;
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x0012F17C File Offset: 0x0012D37C
	public void SetTime(float new_time)
	{
		float dt = Mathf.Max(new_time - this.GetTime(), 0f);
		this.AddTime(dt);
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x0012F1A3 File Offset: 0x0012D3A3
	public float GetTimePlayedInSeconds()
	{
		return this.timePlayed;
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x0012F1AC File Offset: 0x0012D3AC
	private void DoAutoSave(int day)
	{
		if (GenericGameSettings.instance.disableAutosave)
		{
			return;
		}
		day++;
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, GameClock.NewCycleKey, day);
		OniMetrics.SendEvent(OniMetrics.Event.EndOfCycle, "DoAutoSave");
		string text = SaveLoader.GetActiveSaveFilePath();
		if (text == null)
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		int num = text.LastIndexOf("\\");
		if (num > 0)
		{
			int num2 = text.IndexOf(" Cycle ", num);
			if (num2 > 0)
			{
				text = text.Substring(0, num2);
			}
		}
		text = Path.ChangeExtension(text, null);
		text = text + " Cycle " + day.ToString();
		text = SaveScreen.GetValidSaveFilename(text);
		text = Path.Combine(SaveLoader.GetActiveAutoSavePath(), Path.GetFileName(text));
		string text2 = text;
		int num3 = 1;
		while (File.Exists(text))
		{
			text = text2.Replace(".sav", "");
			text = SaveScreen.GetValidSaveFilename(text2 + " (" + num3.ToString() + ")");
			num3++;
		}
		Game.Instance.StartDelayedSave(text, true, false);
	}

	// Token: 0x04002294 RID: 8852
	public static GameClock Instance;

	// Token: 0x04002295 RID: 8853
	[Serialize]
	private int frame;

	// Token: 0x04002296 RID: 8854
	[Serialize]
	private float time;

	// Token: 0x04002297 RID: 8855
	[Serialize]
	private float timeSinceStartOfCycle;

	// Token: 0x04002298 RID: 8856
	[Serialize]
	private int cycle;

	// Token: 0x04002299 RID: 8857
	[Serialize]
	private float timePlayed;

	// Token: 0x0400229A RID: 8858
	[Serialize]
	private bool isNight;

	// Token: 0x0400229B RID: 8859
	public static readonly string NewCycleKey = "NewCycle";
}
