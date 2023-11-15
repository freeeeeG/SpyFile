using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020004EE RID: 1262
[AddComponentMenu("KMonoBehaviour/scripts/Prioritizable")]
public class Prioritizable : KMonoBehaviour
{
	// Token: 0x06001D8C RID: 7564 RVA: 0x0009CFCB File Offset: 0x0009B1CB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Prioritizable>(-905833192, Prioritizable.OnCopySettingsDelegate);
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x0009CFE4 File Offset: 0x0009B1E4
	private void OnCopySettings(object data)
	{
		Prioritizable component = ((GameObject)data).GetComponent<Prioritizable>();
		if (component != null)
		{
			this.SetMasterPriority(component.GetMasterPriority());
		}
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x0009D014 File Offset: 0x0009B214
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.masterPriority != -2147483648)
		{
			this.masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
			this.masterPriority = int.MinValue;
		}
		PrioritySetting prioritySetting;
		if (SaveLoader.Instance.GameInfo.IsVersionExactly(7, 2) && Prioritizable.conversions.TryGetValue(this.masterPrioritySetting, out prioritySetting))
		{
			this.masterPrioritySetting = prioritySetting;
		}
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x0009D078 File Offset: 0x0009B278
	protected override void OnSpawn()
	{
		if (this.onPriorityChanged != null)
		{
			this.onPriorityChanged(this.masterPrioritySetting);
		}
		this.RefreshHighPriorityNotification();
		this.RefreshTopPriorityOnWorld();
		Vector3 position = base.transform.GetPosition();
		Extents extents = new Extents((int)position.x, (int)position.y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, extents, GameScenePartitioner.Instance.prioritizableObjects, null);
		Components.Prioritizables.Add(this);
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0009D0FB File Offset: 0x0009B2FB
	public PrioritySetting GetMasterPriority()
	{
		return this.masterPrioritySetting;
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x0009D104 File Offset: 0x0009B304
	public void SetMasterPriority(PrioritySetting priority)
	{
		if (!priority.Equals(this.masterPrioritySetting))
		{
			this.masterPrioritySetting = priority;
			if (this.onPriorityChanged != null)
			{
				this.onPriorityChanged(this.masterPrioritySetting);
			}
			this.RefreshTopPriorityOnWorld();
			this.RefreshHighPriorityNotification();
		}
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x0009D157 File Offset: 0x0009B357
	private void RefreshTopPriorityOnWorld()
	{
		this.SetTopPriorityOnWorld(this.IsTopPriority());
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x0009D168 File Offset: 0x0009B368
	private void SetTopPriorityOnWorld(bool state)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (Game.Instance == null || myWorld == null)
		{
			return;
		}
		if (state)
		{
			myWorld.AddTopPriorityPrioritizable(this);
			return;
		}
		myWorld.RemoveTopPriorityPrioritizable(this);
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x0009D1AA File Offset: 0x0009B3AA
	public void AddRef()
	{
		this.refCount++;
		this.RefreshTopPriorityOnWorld();
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x0009D1C6 File Offset: 0x0009B3C6
	public void RemoveRef()
	{
		this.refCount--;
		if (this.IsTopPriority() || this.refCount == 0)
		{
			this.SetTopPriorityOnWorld(false);
		}
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x0009D1F3 File Offset: 0x0009B3F3
	public bool IsPrioritizable()
	{
		return this.refCount > 0;
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x0009D1FE File Offset: 0x0009B3FE
	public bool IsTopPriority()
	{
		return this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x0009D218 File Offset: 0x0009B418
	protected override void OnCleanUp()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null)
		{
			myWorld.RemoveTopPriorityPrioritizable(this);
		}
		else
		{
			global::Debug.LogWarning("World has been destroyed before prioritizable " + base.name);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.RemoveTopPriorityPrioritizable(this);
			}
		}
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		Components.Prioritizables.Remove(this);
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x0009D2BC File Offset: 0x0009B4BC
	public static void AddRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.AddRef();
		}
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x0009D2E0 File Offset: 0x0009B4E0
	public static void RemoveRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.RemoveRef();
		}
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x0009D304 File Offset: 0x0009B504
	private void RefreshHighPriorityNotification()
	{
		bool flag = this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
		if (flag && this.highPriorityStatusItem == Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmergencyPriority, null);
			return;
		}
		if (!flag && this.highPriorityStatusItem != Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.highPriorityStatusItem, false);
		}
	}

	// Token: 0x04001063 RID: 4195
	[SerializeField]
	[Serialize]
	private int masterPriority = int.MinValue;

	// Token: 0x04001064 RID: 4196
	[SerializeField]
	[Serialize]
	private PrioritySetting masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x04001065 RID: 4197
	public Action<PrioritySetting> onPriorityChanged;

	// Token: 0x04001066 RID: 4198
	public bool showIcon = true;

	// Token: 0x04001067 RID: 4199
	public Vector2 iconOffset;

	// Token: 0x04001068 RID: 4200
	public float iconScale = 1f;

	// Token: 0x04001069 RID: 4201
	[SerializeField]
	private int refCount;

	// Token: 0x0400106A RID: 4202
	private static readonly EventSystem.IntraObjectHandler<Prioritizable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Prioritizable>(delegate(Prioritizable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400106B RID: 4203
	private static Dictionary<PrioritySetting, PrioritySetting> conversions = new Dictionary<PrioritySetting, PrioritySetting>
	{
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		}
	};

	// Token: 0x0400106C RID: 4204
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x0400106D RID: 4205
	private Guid highPriorityStatusItem;
}
