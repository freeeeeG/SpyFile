using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000639 RID: 1593
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicMemory")]
public class LogicMemory : KMonoBehaviour
{
	// Token: 0x06002945 RID: 10565 RVA: 0x000DEEA8 File Offset: 0x000DD0A8
	protected override void OnSpawn()
	{
		if (LogicMemory.infoStatusItem == null)
		{
			LogicMemory.infoStatusItem = new StatusItem("StoredValue", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicMemory.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicMemory.ResolveInfoStatusItemString);
		}
		base.Subscribe<LogicMemory>(-801688580, LogicMemory.OnLogicValueChangedDelegate);
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x000DEF0C File Offset: 0x000DD10C
	public void OnLogicValueChanged(object data)
	{
		if (this.ports == null || base.gameObject == null || this == null)
		{
			return;
		}
		if (((LogicValueChanged)data).portID != LogicMemory.READ_PORT_ID)
		{
			int inputValue = this.ports.GetInputValue(LogicMemory.SET_PORT_ID);
			int inputValue2 = this.ports.GetInputValue(LogicMemory.RESET_PORT_ID);
			int num = this.value;
			if (LogicCircuitNetwork.IsBitActive(0, inputValue2))
			{
				num = 0;
			}
			else if (LogicCircuitNetwork.IsBitActive(0, inputValue))
			{
				num = 1;
			}
			if (num != this.value)
			{
				this.value = num;
				this.ports.SendSignal(LogicMemory.READ_PORT_ID, this.value);
				KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.Play(LogicCircuitNetwork.IsBitActive(0, this.value) ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
		}
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x000DF000 File Offset: 0x000DD200
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		int outputValue = ((LogicMemory)data).ports.GetOutputValue(LogicMemory.READ_PORT_ID);
		return string.Format(BUILDINGS.PREFABS.LOGICMEMORY.STATUS_ITEM_VALUE, outputValue);
	}

	// Token: 0x04001837 RID: 6199
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04001838 RID: 6200
	[Serialize]
	private int value;

	// Token: 0x04001839 RID: 6201
	private static StatusItem infoStatusItem;

	// Token: 0x0400183A RID: 6202
	public static readonly HashedString READ_PORT_ID = new HashedString("LogicMemoryRead");

	// Token: 0x0400183B RID: 6203
	public static readonly HashedString SET_PORT_ID = new HashedString("LogicMemorySet");

	// Token: 0x0400183C RID: 6204
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicMemoryReset");

	// Token: 0x0400183D RID: 6205
	private static readonly EventSystem.IntraObjectHandler<LogicMemory> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicMemory>(delegate(LogicMemory component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
