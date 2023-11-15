using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
[AddComponentMenu("KMonoBehaviour/scripts/Operational")]
public class Operational : KMonoBehaviour
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06001D0F RID: 7439 RVA: 0x0009A8D3 File Offset: 0x00098AD3
	// (set) Token: 0x06001D10 RID: 7440 RVA: 0x0009A8DB File Offset: 0x00098ADB
	public bool IsFunctional { get; private set; }

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06001D11 RID: 7441 RVA: 0x0009A8E4 File Offset: 0x00098AE4
	// (set) Token: 0x06001D12 RID: 7442 RVA: 0x0009A8EC File Offset: 0x00098AEC
	public bool IsOperational { get; private set; }

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06001D13 RID: 7443 RVA: 0x0009A8F5 File Offset: 0x00098AF5
	// (set) Token: 0x06001D14 RID: 7444 RVA: 0x0009A8FD File Offset: 0x00098AFD
	public bool IsActive { get; private set; }

	// Token: 0x06001D15 RID: 7445 RVA: 0x0009A906 File Offset: 0x00098B06
	[OnSerializing]
	private void OnSerializing()
	{
		this.AddTimeData(this.IsActive);
		this.activeStartTime = GameClock.Instance.GetTime();
		this.inactiveStartTime = GameClock.Instance.GetTime();
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x0009A934 File Offset: 0x00098B34
	protected override void OnPrefabInit()
	{
		this.UpdateFunctional();
		this.UpdateOperational();
		base.Subscribe<Operational>(-1661515756, Operational.OnNewBuildingDelegate);
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0009A970 File Offset: 0x00098B70
	public void OnNewBuilding(object data)
	{
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.creationTime > 0f)
		{
			this.inactiveStartTime = component.creationTime;
			this.activeStartTime = component.creationTime;
		}
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x0009A9A9 File Offset: 0x00098BA9
	public bool IsOperationalType(Operational.Flag.Type type)
	{
		if (type == Operational.Flag.Type.Functional)
		{
			return this.IsFunctional;
		}
		return this.IsOperational;
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0009A9BC File Offset: 0x00098BBC
	public void SetFlag(Operational.Flag flag, bool value)
	{
		bool flag2 = false;
		if (this.Flags.TryGetValue(flag, out flag2))
		{
			if (flag2 != value)
			{
				this.Flags[flag] = value;
				base.Trigger(187661686, flag);
			}
		}
		else
		{
			this.Flags[flag] = value;
			base.Trigger(187661686, flag);
		}
		if (flag.FlagType == Operational.Flag.Type.Functional && value != this.IsFunctional)
		{
			this.UpdateFunctional();
		}
		if (value != this.IsOperational)
		{
			this.UpdateOperational();
		}
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x0009AA3C File Offset: 0x00098C3C
	public bool GetFlag(Operational.Flag flag)
	{
		bool result = false;
		this.Flags.TryGetValue(flag, out result);
		return result;
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x0009AA5C File Offset: 0x00098C5C
	private void UpdateFunctional()
	{
		bool isFunctional = true;
		foreach (KeyValuePair<Operational.Flag, bool> keyValuePair in this.Flags)
		{
			if (keyValuePair.Key.FlagType == Operational.Flag.Type.Functional && !keyValuePair.Value)
			{
				isFunctional = false;
				break;
			}
		}
		this.IsFunctional = isFunctional;
		base.Trigger(-1852328367, this.IsFunctional);
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x0009AAE4 File Offset: 0x00098CE4
	private void UpdateOperational()
	{
		Dictionary<Operational.Flag, bool>.Enumerator enumerator = this.Flags.GetEnumerator();
		bool flag = true;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Operational.Flag, bool> keyValuePair = enumerator.Current;
			if (!keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag != this.IsOperational)
		{
			this.IsOperational = flag;
			if (!this.IsOperational)
			{
				this.SetActive(false, false);
			}
			if (this.IsOperational)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Operational, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);
			}
			base.Trigger(-592767678, this.IsOperational);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0009AB95 File Offset: 0x00098D95
	public void SetActive(bool value, bool force_ignore = false)
	{
		if (this.IsActive != value)
		{
			this.AddTimeData(value);
			base.Trigger(824508782, this);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x0009ABC8 File Offset: 0x00098DC8
	private void AddTimeData(bool value)
	{
		float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
		float time = GameClock.Instance.GetTime();
		float num2 = time - num;
		if (this.IsActive)
		{
			this.activeTime += num2;
		}
		else
		{
			this.inactiveTime += num2;
		}
		this.IsActive = value;
		if (this.IsActive)
		{
			this.activeStartTime = time;
			return;
		}
		this.inactiveStartTime = time;
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x0009AC40 File Offset: 0x00098E40
	public void OnNewDay(object data)
	{
		this.AddTimeData(this.IsActive);
		this.uptimeData.Add(this.activeTime / 600f);
		while (this.uptimeData.Count > this.MAX_DATA_POINTS)
		{
			this.uptimeData.RemoveAt(0);
		}
		this.activeTime = 0f;
		this.inactiveTime = 0f;
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x0009ACA8 File Offset: 0x00098EA8
	public float GetCurrentCycleUptime()
	{
		if (this.IsActive)
		{
			float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
			float num2 = GameClock.Instance.GetTime() - num;
			return (this.activeTime + num2) / GameClock.Instance.GetTimeSinceStartOfCycle();
		}
		return this.activeTime / GameClock.Instance.GetTimeSinceStartOfCycle();
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x0009AD06 File Offset: 0x00098F06
	public float GetLastCycleUptime()
	{
		if (this.uptimeData.Count > 0)
		{
			return this.uptimeData[this.uptimeData.Count - 1];
		}
		return 0f;
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x0009AD34 File Offset: 0x00098F34
	public float GetUptimeOverCycles(int num_cycles)
	{
		if (this.uptimeData.Count > 0)
		{
			int num = Mathf.Min(this.uptimeData.Count, num_cycles);
			float num2 = 0f;
			for (int i = num - 1; i >= 0; i--)
			{
				num2 += this.uptimeData[i];
			}
			return num2 / (float)num;
		}
		return 0f;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x0009AD8E File Offset: 0x00098F8E
	public bool MeetsRequirements(Operational.State stateRequirement)
	{
		switch (stateRequirement)
		{
		case Operational.State.Operational:
			return this.IsOperational;
		case Operational.State.Functional:
			return this.IsFunctional;
		case Operational.State.Active:
			return this.IsActive;
		}
		return true;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x0009ADBE File Offset: 0x00098FBE
	public static GameHashes GetEventForState(Operational.State state)
	{
		if (state == Operational.State.Operational)
		{
			return GameHashes.OperationalChanged;
		}
		if (state == Operational.State.Functional)
		{
			return GameHashes.FunctionalChanged;
		}
		return GameHashes.ActiveChanged;
	}

	// Token: 0x04001023 RID: 4131
	[Serialize]
	public float inactiveStartTime;

	// Token: 0x04001024 RID: 4132
	[Serialize]
	public float activeStartTime;

	// Token: 0x04001025 RID: 4133
	[Serialize]
	private List<float> uptimeData = new List<float>();

	// Token: 0x04001026 RID: 4134
	[Serialize]
	private float activeTime;

	// Token: 0x04001027 RID: 4135
	[Serialize]
	private float inactiveTime;

	// Token: 0x04001028 RID: 4136
	private int MAX_DATA_POINTS = 5;

	// Token: 0x04001029 RID: 4137
	public Dictionary<Operational.Flag, bool> Flags = new Dictionary<Operational.Flag, bool>();

	// Token: 0x0400102A RID: 4138
	private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate = new EventSystem.IntraObjectHandler<Operational>(delegate(Operational component, object data)
	{
		component.OnNewBuilding(data);
	});

	// Token: 0x02001188 RID: 4488
	public enum State
	{
		// Token: 0x04005C99 RID: 23705
		Operational,
		// Token: 0x04005C9A RID: 23706
		Functional,
		// Token: 0x04005C9B RID: 23707
		Active,
		// Token: 0x04005C9C RID: 23708
		None
	}

	// Token: 0x02001189 RID: 4489
	public class Flag
	{
		// Token: 0x060079E4 RID: 31204 RVA: 0x002DA6ED File Offset: 0x002D88ED
		public Flag(string name, Operational.Flag.Type type)
		{
			this.Name = name;
			this.FlagType = type;
		}

		// Token: 0x060079E5 RID: 31205 RVA: 0x002DA703 File Offset: 0x002D8903
		public static Operational.Flag.Type GetFlagType(Operational.State operationalState)
		{
			switch (operationalState)
			{
			case Operational.State.Operational:
			case Operational.State.Active:
				return Operational.Flag.Type.Requirement;
			case Operational.State.Functional:
				return Operational.Flag.Type.Functional;
			}
			throw new InvalidOperationException("Can not convert NONE state to an Operational Flag Type");
		}

		// Token: 0x04005C9D RID: 23709
		public string Name;

		// Token: 0x04005C9E RID: 23710
		public Operational.Flag.Type FlagType;

		// Token: 0x0200209A RID: 8346
		public enum Type
		{
			// Token: 0x040091A9 RID: 37289
			Requirement,
			// Token: 0x040091AA RID: 37290
			Functional
		}
	}
}
