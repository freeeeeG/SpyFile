using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x0200084F RID: 2127
public class LogicCircuitNetwork : UtilityNetwork
{
	// Token: 0x06003E0B RID: 15883 RVA: 0x00158B6C File Offset: 0x00156D6C
	public override void AddItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			LogicWire.BitDepth maxBitDepth = logicWire.MaxBitDepth;
			List<LogicWire> list = this.wireGroups[(int)maxBitDepth];
			if (list == null)
			{
				list = new List<LogicWire>();
				this.wireGroups[(int)maxBitDepth] = list;
			}
			list.Add(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = (ILogicEventReceiver)item;
			this.receivers.Add(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Add(item3);
		}
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x00158BEC File Offset: 0x00156DEC
	public override void RemoveItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			this.wireGroups[(int)logicWire.MaxBitDepth].Remove(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = item as ILogicEventReceiver;
			this.receivers.Remove(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Remove(item3);
		}
	}

	// Token: 0x06003E0D RID: 15885 RVA: 0x00158C56 File Offset: 0x00156E56
	public override void ConnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			((ILogicEventReceiver)item).OnLogicNetworkConnectionChanged(true);
			return;
		}
		if (item is ILogicEventSender)
		{
			((ILogicEventSender)item).OnLogicNetworkConnectionChanged(true);
		}
	}

	// Token: 0x06003E0E RID: 15886 RVA: 0x00158C81 File Offset: 0x00156E81
	public override void DisconnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver logicEventReceiver = item as ILogicEventReceiver;
			logicEventReceiver.ReceiveLogicEvent(0);
			logicEventReceiver.OnLogicNetworkConnectionChanged(false);
			return;
		}
		if (item is ILogicEventSender)
		{
			(item as ILogicEventSender).OnLogicNetworkConnectionChanged(false);
		}
	}

	// Token: 0x06003E0F RID: 15887 RVA: 0x00158CB4 File Offset: 0x00156EB4
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		this.resetting = true;
		this.previousValue = -1;
		this.outputValue = 0;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list = this.wireGroups[i];
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					LogicWire logicWire = list[j];
					if (logicWire != null)
					{
						int num = Grid.PosToCell(logicWire.transform.GetPosition());
						UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
						utilityNetworkGridNode.networkIdx = -1;
						grid[num] = utilityNetworkGridNode;
					}
				}
				list.Clear();
			}
		}
		this.senders.Clear();
		this.receivers.Clear();
		this.resetting = false;
		this.RemoveOverloadedNotification();
	}

	// Token: 0x06003E10 RID: 15888 RVA: 0x00158D68 File Offset: 0x00156F68
	public void UpdateLogicValue()
	{
		if (this.resetting)
		{
			return;
		}
		this.previousValue = this.outputValue;
		this.outputValue = 0;
		foreach (ILogicEventSender logicEventSender in this.senders)
		{
			logicEventSender.LogicTick();
		}
		foreach (ILogicEventSender logicEventSender2 in this.senders)
		{
			int logicValue = logicEventSender2.GetLogicValue();
			this.outputValue |= logicValue;
		}
	}

	// Token: 0x06003E11 RID: 15889 RVA: 0x00158E24 File Offset: 0x00157024
	public int GetBitsUsed()
	{
		int result;
		if (this.outputValue > 1)
		{
			result = 4;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x00158E43 File Offset: 0x00157043
	public bool IsBitActive(int bit)
	{
		return (this.OutputValue & 1 << bit) > 0;
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x00158E55 File Offset: 0x00157055
	public static bool IsBitActive(int bit, int value)
	{
		return (value & 1 << bit) > 0;
	}

	// Token: 0x06003E14 RID: 15892 RVA: 0x00158E62 File Offset: 0x00157062
	public static int GetBitValue(int bit, int value)
	{
		return value & 1 << bit;
	}

	// Token: 0x06003E15 RID: 15893 RVA: 0x00158E6C File Offset: 0x0015706C
	public void SendLogicEvents(bool force_send, int id)
	{
		if (this.resetting)
		{
			return;
		}
		if (this.outputValue != this.previousValue || force_send)
		{
			foreach (ILogicEventReceiver logicEventReceiver in this.receivers)
			{
				logicEventReceiver.ReceiveLogicEvent(this.outputValue);
			}
			if (!force_send)
			{
				this.TriggerAudio((this.previousValue >= 0) ? this.previousValue : 0, id);
			}
		}
	}

	// Token: 0x06003E16 RID: 15894 RVA: 0x00158EFC File Offset: 0x001570FC
	private void TriggerAudio(int old_value, int id)
	{
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (old_value != this.outputValue && instance != null && !instance.IsPaused)
		{
			int num = 0;
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			List<LogicWire> list = new List<LogicWire>();
			for (int i = 0; i < 2; i++)
			{
				List<LogicWire> list2 = this.wireGroups[i];
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						num++;
						if (visibleArea.Min <= list2[j].transform.GetPosition() && list2[j].transform.GetPosition() <= visibleArea.Max)
						{
							list.Add(list2[j]);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				int index = Mathf.CeilToInt((float)(list.Count / 2));
				if (list[index] != null)
				{
					Vector3 position = list[index].transform.GetPosition();
					position.z = 0f;
					string name = "Logic_Circuit_Toggle";
					LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
					if (!LogicCircuitNetwork.logicSoundRegister.ContainsKey(id))
					{
						LogicCircuitNetwork.logicSoundRegister.Add(id, logicSoundPair);
					}
					else
					{
						logicSoundPair.playedIndex = LogicCircuitNetwork.logicSoundRegister[id].playedIndex;
						logicSoundPair.lastPlayed = LogicCircuitNetwork.logicSoundRegister[id].lastPlayed;
					}
					if (logicSoundPair.playedIndex < 2)
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
					}
					else
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = 0;
						LogicCircuitNetwork.logicSoundRegister[id].lastPlayed = Time.time;
					}
					float value = (Time.time - logicSoundPair.lastPlayed) / 3f;
					EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), position, 1f);
					instance2.setParameterByName("logic_volumeModifer", value, false);
					instance2.setParameterByName("wireCount", (float)(num % 24), false);
					instance2.setParameterByName("enabled", (float)this.outputValue, false);
					KFMOD.EndOneShot(instance2);
				}
			}
		}
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x00159134 File Offset: 0x00157334
	public void UpdateOverloadTime(float dt, int bits_used)
	{
		bool flag = false;
		List<LogicWire> list = null;
		List<LogicUtilityNetworkLink> list2 = null;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list3 = this.wireGroups[i];
			List<LogicUtilityNetworkLink> list4 = this.relevantBridges[i];
			float num = (float)LogicWire.GetBitDepthAsInt((LogicWire.BitDepth)i);
			if ((float)bits_used > num && ((list4 != null && list4.Count > 0) || (list3 != null && list3.Count > 0)))
			{
				flag = true;
				list = list3;
				list2 = list4;
				break;
			}
		}
		if (list != null)
		{
			list.RemoveAll((LogicWire x) => x == null);
		}
		if (list2 != null)
		{
			list2.RemoveAll((LogicUtilityNetworkLink x) => x == null);
		}
		if (flag)
		{
			this.timeOverloaded += dt;
			if (this.timeOverloaded > 6f)
			{
				this.timeOverloaded = 0f;
				if (this.targetOverloadedWire == null)
				{
					if (list2 != null && list2.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, list2.Count);
						this.targetOverloadedWire = list2[index].gameObject;
					}
					else if (list != null && list.Count > 0)
					{
						int index2 = UnityEngine.Random.Range(0, list.Count);
						this.targetOverloadedWire = list[index2].gameObject;
					}
				}
				if (this.targetOverloadedWire != null)
				{
					this.targetOverloadedWire.Trigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = BUILDINGS.DAMAGESOURCES.LOGIC_CIRCUIT_OVERLOADED,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LOGIC_CIRCUIT_OVERLOADED,
						takeDamageEffect = SpawnFXHashes.BuildingLogicOverload,
						fullDamageEffectName = "logic_ribbon_damage_kanim",
						statusItemID = Db.Get().BuildingStatusItems.LogicOverloaded.Id
					});
				}
				if (this.overloadedNotification == null)
				{
					this.timeOverloadNotificationDisplayed = 0f;
					this.overloadedNotification = new Notification(MISC.NOTIFICATIONS.LOGIC_CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, null, null, true, 0f, null, null, this.targetOverloadedWire.transform, true, false, false);
					Game.Instance.FindOrAdd<Notifier>().Add(this.overloadedNotification, "");
					return;
				}
			}
		}
		else
		{
			this.timeOverloaded = Mathf.Max(0f, this.timeOverloaded - dt * 0.95f);
			this.timeOverloadNotificationDisplayed += dt;
			if (this.timeOverloadNotificationDisplayed > 5f)
			{
				this.RemoveOverloadedNotification();
			}
		}
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x001593AF File Offset: 0x001575AF
	private void RemoveOverloadedNotification()
	{
		if (this.overloadedNotification != null)
		{
			Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
			this.overloadedNotification = null;
		}
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x001593D8 File Offset: 0x001575D8
	public void UpdateRelevantBridges(List<LogicUtilityNetworkLink>[] bridgeGroups)
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		for (int i = 0; i < bridgeGroups.Length; i++)
		{
			if (this.relevantBridges[i] != null)
			{
				this.relevantBridges[i].Clear();
			}
			for (int j = 0; j < bridgeGroups[i].Count; j++)
			{
				if (logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_one) == this || logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_two) == this)
				{
					if (this.relevantBridges[i] == null)
					{
						this.relevantBridges[i] = new List<LogicUtilityNetworkLink>();
					}
					this.relevantBridges[i].Add(bridgeGroups[i][j]);
				}
			}
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06003E1A RID: 15898 RVA: 0x00159489 File Offset: 0x00157689
	public int OutputValue
	{
		get
		{
			return this.outputValue;
		}
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06003E1B RID: 15899 RVA: 0x00159494 File Offset: 0x00157694
	public int WireCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (this.wireGroups[i] != null)
				{
					num += this.wireGroups[i].Count;
				}
			}
			return num;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06003E1C RID: 15900 RVA: 0x001594CA File Offset: 0x001576CA
	public ReadOnlyCollection<ILogicEventSender> Senders
	{
		get
		{
			return this.senders.AsReadOnly();
		}
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06003E1D RID: 15901 RVA: 0x001594D7 File Offset: 0x001576D7
	public ReadOnlyCollection<ILogicEventReceiver> Receivers
	{
		get
		{
			return this.receivers.AsReadOnly();
		}
	}

	// Token: 0x04002851 RID: 10321
	private List<LogicWire>[] wireGroups = new List<LogicWire>[2];

	// Token: 0x04002852 RID: 10322
	private List<LogicUtilityNetworkLink>[] relevantBridges = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04002853 RID: 10323
	private List<ILogicEventReceiver> receivers = new List<ILogicEventReceiver>();

	// Token: 0x04002854 RID: 10324
	private List<ILogicEventSender> senders = new List<ILogicEventSender>();

	// Token: 0x04002855 RID: 10325
	private int previousValue = -1;

	// Token: 0x04002856 RID: 10326
	private int outputValue;

	// Token: 0x04002857 RID: 10327
	private bool resetting;

	// Token: 0x04002858 RID: 10328
	public static float logicSoundLastPlayedTime = 0f;

	// Token: 0x04002859 RID: 10329
	private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;

	// Token: 0x0400285A RID: 10330
	private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;

	// Token: 0x0400285B RID: 10331
	private GameObject targetOverloadedWire;

	// Token: 0x0400285C RID: 10332
	private float timeOverloaded;

	// Token: 0x0400285D RID: 10333
	private float timeOverloadNotificationDisplayed;

	// Token: 0x0400285E RID: 10334
	private Notification overloadedNotification;

	// Token: 0x0400285F RID: 10335
	public static Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = new Dictionary<int, LogicCircuitNetwork.LogicSoundPair>();

	// Token: 0x02001620 RID: 5664
	public class LogicSoundPair
	{
		// Token: 0x04006ABE RID: 27326
		public int playedIndex;

		// Token: 0x04006ABF RID: 27327
		public float lastPlayed;
	}
}
