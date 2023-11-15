using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008EB RID: 2283
[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour, IWiltCause
{
	// Token: 0x060041F9 RID: 16889 RVA: 0x00170FE4 File Offset: 0x0016F1E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
		int cell = Grid.PosToCell(base.gameObject);
		GameObject x = Grid.Objects[cell, 5];
		if (x != null && x != base.gameObject)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
		else
		{
			this.SetOccupyGridSpace(true);
		}
		base.Subscribe<TreeBud>(1272413801, TreeBud.OnHarvestDelegate);
	}

	// Token: 0x060041FA RID: 16890 RVA: 0x00171053 File Offset: 0x0016F253
	private void OnHarvest(object data)
	{
		if (this.buddingTrunk.Get() != null)
		{
			this.buddingTrunk.Get().TryRollNewSeed();
		}
	}

	// Token: 0x060041FB RID: 16891 RVA: 0x00171078 File Offset: 0x0016F278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.buddingTrunk != null && this.buddingTrunk.Get() != null)
		{
			this.SubscribeToTrunk();
			this.UpdateAnimationSet();
			return;
		}
		global::Debug.LogWarning("TreeBud loaded with missing trunk reference. Destroying...");
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x001710C8 File Offset: 0x0016F2C8
	protected override void OnCleanUp()
	{
		this.UnsubscribeToTrunk();
		this.SetOccupyGridSpace(false);
		base.OnCleanUp();
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x001710E0 File Offset: 0x0016F2E0
	private void SetOccupyGridSpace(bool active)
	{
		int cell = Grid.PosToCell(base.gameObject);
		if (active)
		{
			GameObject gameObject = Grid.Objects[cell, 5];
			if (gameObject != null && gameObject != base.gameObject)
			{
				global::Debug.LogWarningFormat(base.gameObject, "TreeBud.SetOccupyGridSpace already occupied by {0}", new object[]
				{
					gameObject
				});
			}
			Grid.Objects[cell, 5] = base.gameObject;
			return;
		}
		if (Grid.Objects[cell, 5] == base.gameObject)
		{
			Grid.Objects[cell, 5] = null;
		}
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x00171174 File Offset: 0x0016F374
	private void SubscribeToTrunk()
	{
		if (this.trunkWiltHandle != -1 || this.trunkWiltRecoverHandle != -1)
		{
			return;
		}
		global::Debug.Assert(this.buddingTrunk != null, "buddingTrunk null");
		BuddingTrunk buddingTrunk = this.buddingTrunk.Get();
		global::Debug.Assert(buddingTrunk != null, "tree_trunk null");
		this.trunkWiltHandle = buddingTrunk.Subscribe(-724860998, new Action<object>(this.OnTrunkWilt));
		this.trunkWiltRecoverHandle = buddingTrunk.Subscribe(712767498, new Action<object>(this.OnTrunkRecover));
		base.Trigger(912965142, !buddingTrunk.GetComponent<WiltCondition>().IsWilting());
		ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
		PlantablePlot receptacle = buddingTrunk.GetComponent<ReceptacleMonitor>().GetReceptacle();
		component.SetReceptacle(receptacle);
		Vector3 position = base.gameObject.transform.position;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.1f * (float)this.trunkPosition;
		base.gameObject.transform.SetPosition(position);
		base.GetComponent<BudUprootedMonitor>().SetParentObject(buddingTrunk.GetComponent<KPrefabID>());
	}

	// Token: 0x060041FF RID: 16895 RVA: 0x00171284 File Offset: 0x0016F484
	private void UnsubscribeToTrunk()
	{
		if (this.buddingTrunk == null)
		{
			global::Debug.LogWarning("buddingTrunk null", base.gameObject);
			return;
		}
		BuddingTrunk buddingTrunk = this.buddingTrunk.Get();
		if (buddingTrunk == null)
		{
			global::Debug.LogWarning("tree_trunk null", base.gameObject);
			return;
		}
		buddingTrunk.Unsubscribe(this.trunkWiltHandle);
		buddingTrunk.Unsubscribe(this.trunkWiltRecoverHandle);
		buddingTrunk.OnBranchRemoved(this.trunkPosition, this);
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x001712F5 File Offset: 0x0016F4F5
	public void SetTrunkPosition(BuddingTrunk budding_trunk, int idx)
	{
		this.buddingTrunk = new Ref<BuddingTrunk>(budding_trunk);
		this.trunkPosition = idx;
		this.SubscribeToTrunk();
		this.UpdateAnimationSet();
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x00171316 File Offset: 0x0016F516
	private void OnTrunkWilt(object data = null)
	{
		base.Trigger(912965142, false);
	}

	// Token: 0x06004202 RID: 16898 RVA: 0x00171329 File Offset: 0x0016F529
	private void OnTrunkRecover(object data = null)
	{
		base.Trigger(912965142, true);
	}

	// Token: 0x06004203 RID: 16899 RVA: 0x0017133C File Offset: 0x0016F53C
	private void UpdateAnimationSet()
	{
		this.crop.anims = TreeBud.animSets[this.trunkPosition];
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Offset = TreeBud.animOffset[this.trunkPosition];
		component.Play(this.crop.anims.grow, KAnim.PlayMode.Paused, 1f, 0f);
		this.crop.RefreshPositionPercent();
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06004204 RID: 16900 RVA: 0x001713AC File Offset: 0x0016F5AC
	public string WiltStateString
	{
		get
		{
			return "    • " + DUPLICANTS.STATS.TRUNKHEALTH.NAME;
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06004205 RID: 16901 RVA: 0x001713C2 File Offset: 0x0016F5C2
	public WiltCondition.Condition[] Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.UnhealthyRoot
			};
		}
	}

	// Token: 0x04002B17 RID: 11031
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04002B18 RID: 11032
	[MyCmpReq]
	private StandardCropPlant crop;

	// Token: 0x04002B19 RID: 11033
	[Serialize]
	public Ref<BuddingTrunk> buddingTrunk;

	// Token: 0x04002B1A RID: 11034
	[Serialize]
	private int trunkPosition;

	// Token: 0x04002B1B RID: 11035
	[Serialize]
	public int growingPos;

	// Token: 0x04002B1C RID: 11036
	private int trunkWiltHandle = -1;

	// Token: 0x04002B1D RID: 11037
	private int trunkWiltRecoverHandle = -1;

	// Token: 0x04002B1E RID: 11038
	private static StandardCropPlant.AnimSet[] animSets = new StandardCropPlant.AnimSet[]
	{
		new StandardCropPlant.AnimSet
		{
			grow = "branch_a_grow",
			grow_pst = "branch_a_grow_pst",
			idle_full = "branch_a_idle_full",
			wilt_base = "branch_a_wilt",
			harvest = "branch_a_harvest"
		},
		new StandardCropPlant.AnimSet
		{
			grow = "branch_b_grow",
			grow_pst = "branch_b_grow_pst",
			idle_full = "branch_b_idle_full",
			wilt_base = "branch_b_wilt",
			harvest = "branch_b_harvest"
		},
		new StandardCropPlant.AnimSet
		{
			grow = "branch_c_grow",
			grow_pst = "branch_c_grow_pst",
			idle_full = "branch_c_idle_full",
			wilt_base = "branch_c_wilt",
			harvest = "branch_c_harvest"
		},
		new StandardCropPlant.AnimSet
		{
			grow = "branch_d_grow",
			grow_pst = "branch_d_grow_pst",
			idle_full = "branch_d_idle_full",
			wilt_base = "branch_d_wilt",
			harvest = "branch_d_harvest"
		},
		new StandardCropPlant.AnimSet
		{
			grow = "branch_e_grow",
			grow_pst = "branch_e_grow_pst",
			idle_full = "branch_e_idle_full",
			wilt_base = "branch_e_wilt",
			harvest = "branch_e_harvest"
		},
		new StandardCropPlant.AnimSet
		{
			grow = "branch_f_grow",
			grow_pst = "branch_f_grow_pst",
			idle_full = "branch_f_idle_full",
			wilt_base = "branch_f_wilt",
			harvest = "branch_f_harvest"
		},
		new StandardCropPlant.AnimSet
		{
			grow = "branch_g_grow",
			grow_pst = "branch_g_grow_pst",
			idle_full = "branch_g_idle_full",
			wilt_base = "branch_g_wilt",
			harvest = "branch_g_harvest"
		}
	};

	// Token: 0x04002B1F RID: 11039
	private static Vector3[] animOffset = new Vector3[]
	{
		new Vector3(1f, 0f, 0f),
		new Vector3(1f, -1f, 0f),
		new Vector3(1f, -2f, 0f),
		new Vector3(0f, -2f, 0f),
		new Vector3(-1f, -2f, 0f),
		new Vector3(-1f, -1f, 0f),
		new Vector3(-1f, 0f, 0f)
	};

	// Token: 0x04002B20 RID: 11040
	private static readonly EventSystem.IntraObjectHandler<TreeBud> OnHarvestDelegate = new EventSystem.IntraObjectHandler<TreeBud>(delegate(TreeBud component, object data)
	{
		component.OnHarvest(data);
	});
}
