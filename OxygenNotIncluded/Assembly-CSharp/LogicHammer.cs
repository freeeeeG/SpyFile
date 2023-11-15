using System;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x02000636 RID: 1590
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHammer : Switch
{
	// Token: 0x06002901 RID: 10497 RVA: 0x000DDFC4 File Offset: 0x000DC1C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.switchedOn = false;
		this.UpdateVisualState(false, false);
		this.rotatable = base.GetComponent<Rotatable>();
		CellOffset rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.target_offset);
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.resonator_cell = Grid.OffsetCell(cell, rotatedCellOffset);
		base.Subscribe<LogicHammer>(-801688580, LogicHammer.OnLogicValueChangedDelegate);
		base.Subscribe<LogicHammer>(-592767678, LogicHammer.OnOperationalChangedDelegate);
		base.OnToggle += this.OnSwitchToggled;
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x000DE064 File Offset: 0x000DC264
	private void OnSwitchToggled(bool toggled_on)
	{
		bool connected = false;
		if (this.operational.IsOperational && toggled_on)
		{
			connected = this.TriggerAudio();
			this.operational.SetActive(true, false);
		}
		else
		{
			this.operational.SetActive(false, false);
		}
		this.UpdateVisualState(connected, false);
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x000DE0AD File Offset: 0x000DC2AD
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.SetState(LogicCircuitNetwork.IsBitActive(0, this.logic_value));
			return;
		}
		this.UpdateVisualState(false, false);
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x000DE0D8 File Offset: 0x000DC2D8
	private bool TriggerAudio()
	{
		if (this.wasOn || !this.switchedOn)
		{
			return false;
		}
		string text = null;
		if (!Grid.IsValidCell(this.resonator_cell))
		{
			return false;
		}
		float num = float.NaN;
		GameObject gameObject = Grid.Objects[this.resonator_cell, 1];
		if (gameObject == null)
		{
			gameObject = Grid.Objects[this.resonator_cell, 30];
			if (gameObject == null)
			{
				gameObject = Grid.Objects[this.resonator_cell, 26];
				if (gameObject != null)
				{
					Wire component = gameObject.GetComponent<Wire>();
					if (component != null)
					{
						ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)Game.Instance.electricalConduitSystem.GetNetworkForCell(component.GetNetworkCell());
						if (electricalUtilityNetwork != null)
						{
							num = (float)electricalUtilityNetwork.allWires.Count;
						}
					}
				}
				else
				{
					gameObject = Grid.Objects[this.resonator_cell, 31];
					if (gameObject != null)
					{
						if (gameObject.GetComponent<LogicWire>() != null)
						{
							LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.resonator_cell);
							if (networkForCell != null)
							{
								num = (float)networkForCell.WireCount;
							}
						}
					}
					else
					{
						gameObject = Grid.Objects[this.resonator_cell, 12];
						if (gameObject != null)
						{
							Conduit component2 = gameObject.GetComponent<Conduit>();
							FlowUtilityNetwork flowUtilityNetwork = (FlowUtilityNetwork)Conduit.GetNetworkManager(ConduitType.Gas).GetNetworkForCell(component2.GetNetworkCell());
							if (flowUtilityNetwork != null)
							{
								num = (float)flowUtilityNetwork.conduitCount;
							}
						}
						else
						{
							gameObject = Grid.Objects[this.resonator_cell, 16];
							if (gameObject != null)
							{
								Conduit component3 = gameObject.GetComponent<Conduit>();
								FlowUtilityNetwork flowUtilityNetwork2 = (FlowUtilityNetwork)Conduit.GetNetworkManager(ConduitType.Liquid).GetNetworkForCell(component3.GetNetworkCell());
								if (flowUtilityNetwork2 != null)
								{
									num = (float)flowUtilityNetwork2.conduitCount;
								}
							}
							else
							{
								gameObject = Grid.Objects[this.resonator_cell, 20];
								gameObject != null;
							}
						}
					}
				}
			}
		}
		if (gameObject != null)
		{
			Building component4 = gameObject.GetComponent<BuildingComplete>();
			if (component4 != null)
			{
				text = component4.Def.PrefabID;
			}
		}
		if (text != null)
		{
			string text2 = StringFormatter.Combine(LogicHammer.SOUND_EVENT_PREFIX, text);
			text2 = GlobalAssets.GetSound(text2, true);
			if (text2 == null)
			{
				text2 = GlobalAssets.GetSound(LogicHammer.DEFAULT_NO_SOUND_EVENT, false);
			}
			Vector3 position = base.transform.position;
			position.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(text2, position, 1f);
			if (!float.IsNaN(num))
			{
				instance.setParameterByName(LogicHammer.PARAMETER_NAME, num, false);
			}
			KFMOD.EndOneShot(instance);
			return true;
		}
		return false;
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x000DE360 File Offset: 0x000DC560
	private void UpdateVisualState(bool connected, bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				if (connected)
				{
					this.animController.Play(LogicHammer.ON_HIT_ANIMS, KAnim.PlayMode.Once);
					return;
				}
				this.animController.Play(LogicHammer.ON_MISS_ANIMS, KAnim.PlayMode.Once);
				return;
			}
			else
			{
				this.animController.Play(LogicHammer.OFF_ANIMS, KAnim.PlayMode.Once);
			}
		}
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x000DE3D0 File Offset: 0x000DC5D0
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LogicHammer.PORT_ID)
		{
			this.SetState(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
			this.logic_value = logicValueChanged.newValue;
		}
	}

	// Token: 0x0400180D RID: 6157
	protected KBatchedAnimController animController;

	// Token: 0x0400180E RID: 6158
	private static readonly EventSystem.IntraObjectHandler<LogicHammer> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicHammer>(delegate(LogicHammer component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400180F RID: 6159
	private static readonly EventSystem.IntraObjectHandler<LogicHammer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicHammer>(delegate(LogicHammer component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001810 RID: 6160
	public static readonly HashedString PORT_ID = new HashedString("LogicHammerInput");

	// Token: 0x04001811 RID: 6161
	private static string PARAMETER_NAME = "hammerObjectCount";

	// Token: 0x04001812 RID: 6162
	private static string SOUND_EVENT_PREFIX = "Hammer_strike_";

	// Token: 0x04001813 RID: 6163
	private static string DEFAULT_NO_SOUND_EVENT = "Hammer_strike_default";

	// Token: 0x04001814 RID: 6164
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04001815 RID: 6165
	private int resonator_cell;

	// Token: 0x04001816 RID: 6166
	private CellOffset target_offset = new CellOffset(-1, 0);

	// Token: 0x04001817 RID: 6167
	private Rotatable rotatable;

	// Token: 0x04001818 RID: 6168
	private int logic_value;

	// Token: 0x04001819 RID: 6169
	private bool wasOn;

	// Token: 0x0400181A RID: 6170
	protected static readonly HashedString[] ON_HIT_ANIMS = new HashedString[]
	{
		"on_hit"
	};

	// Token: 0x0400181B RID: 6171
	protected static readonly HashedString[] ON_MISS_ANIMS = new HashedString[]
	{
		"on_miss"
	};

	// Token: 0x0400181C RID: 6172
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};
}
