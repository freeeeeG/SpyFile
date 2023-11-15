using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

// Token: 0x02000435 RID: 1077
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StateMachineController")]
public class StateMachineController : KMonoBehaviour, ISaveLoadableDetails, IStateMachineControllerHack
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060016A8 RID: 5800 RVA: 0x00075C06 File Offset: 0x00073E06
	public StateMachineController.CmpDef cmpdef
	{
		get
		{
			return this.defHandle.Get<StateMachineController.CmpDef>();
		}
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x00075C13 File Offset: 0x00073E13
	public IEnumerator<StateMachine.Instance> GetEnumerator()
	{
		return this.stateMachines.GetEnumerator();
	}

	// Token: 0x060016AA RID: 5802 RVA: 0x00075C25 File Offset: 0x00073E25
	public void AddStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!this.stateMachines.Contains(state_machine))
		{
			this.stateMachines.Add(state_machine);
			MyAttributes.OnAwake(state_machine, this);
		}
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x00075C48 File Offset: 0x00073E48
	public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetStateMachine().saveHistory && !state_machine.GetStateMachine().debugSettings.saveHistory)
		{
			this.stateMachines.Remove(state_machine);
		}
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x00075C76 File Offset: 0x00073E76
	public bool HasStateMachineInstance(StateMachine.Instance state_machine)
	{
		return this.stateMachines.Contains(state_machine);
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x00075C84 File Offset: 0x00073E84
	public void AddDef(StateMachine.BaseDef def)
	{
		this.cmpdef.defs.Add(def);
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x00075C97 File Offset: 0x00073E97
	public LoggerFSSSS GetLog()
	{
		return this.log;
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x00075C9F File Offset: 0x00073E9F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log.SetName(base.name);
		base.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
		base.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x00075CDC File Offset: 0x00073EDC
	private void OnTargetDestroyed(object data)
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x00075D20 File Offset: 0x00073F20
	protected override void OnLoadLevel()
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.FreeResources();
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x00075D60 File Offset: 0x00073F60
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x00075DA8 File Offset: 0x00073FA8
	public void CreateSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			baseDef.CreateSMI(this);
		}
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x00075E10 File Offset: 0x00074010
	public void StartSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(baseDef.GetStateMachineType()).GetStateMachineInstanceType());
			if (smi != null && !smi.IsRunning())
			{
				smi.StartSM();
			}
		}
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x00075E9C File Offset: 0x0007409C
	public void Serialize(BinaryWriter writer)
	{
		this.serializer.Serialize(this.stateMachines, writer);
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x00075EB0 File Offset: 0x000740B0
	public void Deserialize(IReader reader)
	{
		this.serializer.Deserialize(reader);
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x00075EBE File Offset: 0x000740BE
	public bool Restore(StateMachine.Instance smi)
	{
		return this.serializer.Restore(smi);
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x00075ECC File Offset: 0x000740CC
	public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
	{
		if (!this.defHandle.IsValid())
		{
			return default(DefType);
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				return defType;
			}
		}
		return default(DefType);
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x00075F58 File Offset: 0x00074158
	public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
	{
		List<DefType> list = new List<DefType>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				list.Add(defType);
			}
		}
		return list;
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x00075FD8 File Offset: 0x000741D8
	public StateMachine.Instance GetSMI(Type type)
	{
		for (int i = 0; i < this.stateMachines.Count; i++)
		{
			StateMachine.Instance instance = this.stateMachines[i];
			if (type.IsAssignableFrom(instance.GetType()))
			{
				return instance;
			}
		}
		return null;
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x00076019 File Offset: 0x00074219
	public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		return this.GetSMI(typeof(StateMachineInstanceType)) as StateMachineInstanceType;
	}

	// Token: 0x060016BC RID: 5820 RVA: 0x00076038 File Offset: 0x00074238
	public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		List<StateMachineInstanceType> list = new List<StateMachineInstanceType>();
		foreach (StateMachine.Instance instance in this.stateMachines)
		{
			StateMachineInstanceType stateMachineInstanceType = instance as StateMachineInstanceType;
			if (stateMachineInstanceType != null)
			{
				list.Add(stateMachineInstanceType);
			}
		}
		return list;
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x000760A4 File Offset: 0x000742A4
	public List<IGameObjectEffectDescriptor> GetDescriptors()
	{
		List<IGameObjectEffectDescriptor> list = new List<IGameObjectEffectDescriptor>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (baseDef is IGameObjectEffectDescriptor)
			{
				list.Add(baseDef as IGameObjectEffectDescriptor);
			}
		}
		return list;
	}

	// Token: 0x04000CA2 RID: 3234
	public DefHandle defHandle;

	// Token: 0x04000CA3 RID: 3235
	private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();

	// Token: 0x04000CA4 RID: 3236
	private LoggerFSSSS log = new LoggerFSSSS("StateMachineController", 35);

	// Token: 0x04000CA5 RID: 3237
	private StateMachineSerializer serializer = new StateMachineSerializer();

	// Token: 0x04000CA6 RID: 3238
	private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>(delegate(StateMachineController component, object data)
	{
		component.OnTargetDestroyed(data);
	});

	// Token: 0x020010C4 RID: 4292
	public class CmpDef
	{
		// Token: 0x04005A03 RID: 23043
		public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
	}
}
