using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020009A2 RID: 2466
[AddComponentMenu("KMonoBehaviour/scripts/LaunchConditionManager")]
public class LaunchConditionManager : KMonoBehaviour, ISim4000ms, ISim1000ms
{
	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x06004959 RID: 18777 RVA: 0x0019D6C3 File Offset: 0x0019B8C3
	// (set) Token: 0x0600495A RID: 18778 RVA: 0x0019D6CB File Offset: 0x0019B8CB
	public List<RocketModule> rocketModules { get; private set; }

	// Token: 0x0600495B RID: 18779 RVA: 0x0019D6D4 File Offset: 0x0019B8D4
	public void DEBUG_TraceModuleDestruction(string moduleName, string state, string stackTrace)
	{
		if (this.DEBUG_ModuleDestructions == null)
		{
			this.DEBUG_ModuleDestructions = new List<global::Tuple<string, string, string>>();
		}
		this.DEBUG_ModuleDestructions.Add(new global::Tuple<string, string, string>(moduleName, state, stackTrace));
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x0019D6FC File Offset: 0x0019B8FC
	[ContextMenu("Dump Module Destructions")]
	private void DEBUG_DumpModuleDestructions()
	{
		if (this.DEBUG_ModuleDestructions == null || this.DEBUG_ModuleDestructions.Count == 0)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Sorry, no logged module destructions. :("
			});
			return;
		}
		foreach (global::Tuple<string, string, string> tuple in this.DEBUG_ModuleDestructions)
		{
			DebugUtil.LogArgs(new object[]
			{
				tuple.first,
				">",
				tuple.second,
				"\n",
				tuple.third,
				"\nEND MODULE DUMP\n\n"
			});
		}
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x0019D7B0 File Offset: 0x0019B9B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketModules = new List<RocketModule>();
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x0019D7C3 File Offset: 0x0019B9C3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.launchable = base.GetComponent<ILaunchableRocket>();
		this.FindModules();
		base.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged = delegate(object data)
		{
			this.FindModules();
		};
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x0019D7F4 File Offset: 0x0019B9F4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x0019D7FC File Offset: 0x0019B9FC
	public void Sim1000ms(float dt)
	{
		Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
		if (spacecraftFromLaunchConditionManager == null)
		{
			return;
		}
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
		if (base.gameObject.GetComponent<LogicPorts>().GetInputValue(this.triggerPort) == 1 && spacecraftDestination != null && spacecraftDestination.id != -1)
		{
			this.Launch(spacecraftDestination);
		}
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x0019D864 File Offset: 0x0019BA64
	public void FindModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null && component.conditionManager == null)
			{
				component.conditionManager = this;
				component.RegisterWithConditionManager();
			}
		}
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x0019D8E0 File Offset: 0x0019BAE0
	public void RegisterRocketModule(RocketModule module)
	{
		if (!this.rocketModules.Contains(module))
		{
			this.rocketModules.Add(module);
		}
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x0019D8FC File Offset: 0x0019BAFC
	public void UnregisterRocketModule(RocketModule module)
	{
		this.rocketModules.Remove(module);
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x0019D90C File Offset: 0x0019BB0C
	public List<ProcessCondition> GetLaunchConditionList()
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			foreach (ProcessCondition item in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketPrep))
			{
				list.Add(item);
			}
			foreach (ProcessCondition item2 in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketStorage))
			{
				list.Add(item2);
			}
		}
		return list;
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x0019D9EC File Offset: 0x0019BBEC
	public void Launch(SpaceDestination destination)
	{
		if (destination == null)
		{
			global::Debug.LogError("Null destination passed to launch");
		}
		if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).state != Spacecraft.MissionState.Grounded)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode || (this.CheckReadyToLaunch() && this.CheckAbleToFly()))
		{
			this.launchable.LaunchableGameObject.Trigger(705820818, null);
			SpacecraftManager.instance.SetSpacecraftDestination(this, destination);
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).BeginMission(destination);
		}
	}

	// Token: 0x06004966 RID: 18790 RVA: 0x0019DA64 File Offset: 0x0019BC64
	public bool CheckReadyToLaunch()
	{
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketPrep).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketStorage).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x0019DB44 File Offset: 0x0019BD44
	public bool CheckAbleToFly()
	{
		foreach (RocketModule rocketModule in this.rocketModules)
		{
			using (List<ProcessCondition>.Enumerator enumerator2 = rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketFlight).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06004968 RID: 18792 RVA: 0x0019DBD8 File Offset: 0x0019BDD8
	private void ClearFlightStatuses()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		foreach (KeyValuePair<ProcessCondition, Guid> keyValuePair in this.conditionStatuses)
		{
			component.RemoveStatusItem(keyValuePair.Value, false);
		}
		this.conditionStatuses.Clear();
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x0019DC48 File Offset: 0x0019BE48
	public void Sim4000ms(float dt)
	{
		bool flag = this.CheckReadyToLaunch();
		LogicPorts component = base.gameObject.GetComponent<LogicPorts>();
		if (flag)
		{
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
			if (spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Grounded || spacecraftFromLaunchConditionManager.state == Spacecraft.MissionState.Launching)
			{
				component.SendSignal(this.statusPort, 1);
			}
			else
			{
				component.SendSignal(this.statusPort, 0);
			}
			KSelectable component2 = base.GetComponent<KSelectable>();
			using (List<RocketModule>.Enumerator enumerator = this.rocketModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketModule rocketModule = enumerator.Current;
					foreach (ProcessCondition processCondition in rocketModule.GetConditionSet(ProcessCondition.ProcessConditionType.RocketFlight))
					{
						if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
						{
							if (!this.conditionStatuses.ContainsKey(processCondition))
							{
								StatusItem statusItem = processCondition.GetStatusItem(ProcessCondition.Status.Failure);
								this.conditionStatuses[processCondition] = component2.AddStatusItem(statusItem, processCondition);
							}
						}
						else if (this.conditionStatuses.ContainsKey(processCondition))
						{
							component2.RemoveStatusItem(this.conditionStatuses[processCondition], false);
							this.conditionStatuses.Remove(processCondition);
						}
					}
				}
				return;
			}
		}
		this.ClearFlightStatuses();
		component.SendSignal(this.statusPort, 0);
	}

	// Token: 0x04003041 RID: 12353
	public HashedString triggerPort;

	// Token: 0x04003042 RID: 12354
	public HashedString statusPort;

	// Token: 0x04003044 RID: 12356
	private ILaunchableRocket launchable;

	// Token: 0x04003045 RID: 12357
	[Serialize]
	private List<global::Tuple<string, string, string>> DEBUG_ModuleDestructions;

	// Token: 0x04003046 RID: 12358
	private Dictionary<ProcessCondition, Guid> conditionStatuses = new Dictionary<ProcessCondition, Guid>();

	// Token: 0x02001821 RID: 6177
	public enum ConditionType
	{
		// Token: 0x0400711D RID: 28957
		Launch,
		// Token: 0x0400711E RID: 28958
		Flight
	}
}
