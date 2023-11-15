using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200010A RID: 266
[SelectionBase]
public abstract class ABaseCannon : MonoBehaviour
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x000187DA File Offset: 0x000169DA
	public Animator Animator
	{
		get
		{
			return this.animator;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060006B7 RID: 1719 RVA: 0x000187E2 File Offset: 0x000169E2
	public Vector3 ShootWorldPosition
	{
		get
		{
			return this.node_ShootPosition.position;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060006B8 RID: 1720 RVA: 0x000187EF File Offset: 0x000169EF
	public ABasePanel ConnectedPanel
	{
		get
		{
			return this.connectedPanel;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000187F7 File Offset: 0x000169F7
	public bool IsInitialized
	{
		get
		{
			return this.isInitialized;
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x000187FF File Offset: 0x000169FF
	protected void Awake()
	{
		if (this.settingData.GetBulletPrefab() == null)
		{
			Debug.LogError("沒有指定砲台" + base.gameObject.name + "的子彈Prefab!!", base.gameObject);
		}
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00018839 File Offset: 0x00016A39
	public void Spawn()
	{
		this.CannonSpawnProc();
		this.isInitialized = true;
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00018848 File Offset: 0x00016A48
	public void SetPanel(ABasePanel panel)
	{
		if (panel == null)
		{
			Debug.LogError("Panel is NULL");
		}
		base.transform.SetParent(panel.GetCannonPlacementNode());
		base.transform.localPosition = Vector3.zero;
		this.connectedPanel = panel;
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00018885 File Offset: 0x00016A85
	public void Despawn()
	{
		this.isInitialized = false;
		this.CannonDespawnProc();
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00018894 File Offset: 0x00016A94
	public void Move()
	{
		this.CannonMoveProc();
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0001889C File Offset: 0x00016A9C
	public void Shoot()
	{
		if (this.particle_ShootEffect != null)
		{
			this.particle_ShootEffect.Play();
		}
		this.ShootProc();
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x000188BD File Offset: 0x00016ABD
	public int GetCost(float multiplier = 1f)
	{
		return this.settingData.GetBuildCost(multiplier);
	}

	// Token: 0x060006C1 RID: 1729
	protected abstract void ShootProc();

	// Token: 0x060006C2 RID: 1730 RVA: 0x000188CC File Offset: 0x00016ACC
	protected virtual void CannonSpawnProc()
	{
		ABaseCannon.<CannonSpawnProc>d__26 <CannonSpawnProc>d__;
		<CannonSpawnProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonSpawnProc>d__.<>1__state = -1;
		<CannonSpawnProc>d__.<>t__builder.Start<ABaseCannon.<CannonSpawnProc>d__26>(ref <CannonSpawnProc>d__);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x000188FC File Offset: 0x00016AFC
	protected virtual void CannonUpgradeProc()
	{
		ABaseCannon.<CannonUpgradeProc>d__27 <CannonUpgradeProc>d__;
		<CannonUpgradeProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonUpgradeProc>d__.<>1__state = -1;
		<CannonUpgradeProc>d__.<>t__builder.Start<ABaseCannon.<CannonUpgradeProc>d__27>(ref <CannonUpgradeProc>d__);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x0001892C File Offset: 0x00016B2C
	protected virtual void CannonMoveProc()
	{
		ABaseCannon.<CannonMoveProc>d__28 <CannonMoveProc>d__;
		<CannonMoveProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonMoveProc>d__.<>1__state = -1;
		<CannonMoveProc>d__.<>t__builder.Start<ABaseCannon.<CannonMoveProc>d__28>(ref <CannonMoveProc>d__);
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0001895C File Offset: 0x00016B5C
	protected virtual void CannonDespawnProc()
	{
		ABaseCannon.<CannonDespawnProc>d__29 <CannonDespawnProc>d__;
		<CannonDespawnProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonDespawnProc>d__.<>1__state = -1;
		<CannonDespawnProc>d__.<>t__builder.Start<ABaseCannon.<CannonDespawnProc>d__29>(ref <CannonDespawnProc>d__);
	}

	// Token: 0x04000592 RID: 1426
	[SerializeField]
	[Header("設定資料")]
	protected CannonSettingData settingData;

	// Token: 0x04000593 RID: 1427
	[SerializeField]
	protected Animator animator;

	// Token: 0x04000594 RID: 1428
	[SerializeField]
	protected ParticleSystem particle_ShootEffect;

	// Token: 0x04000595 RID: 1429
	[SerializeField]
	[Header("砲台的旋轉節點")]
	protected Transform node_CannonHeadModel;

	// Token: 0x04000596 RID: 1430
	[SerializeField]
	[Header("發射點node")]
	protected Transform node_ShootPosition;

	// Token: 0x04000597 RID: 1431
	protected ABasePanel connectedPanel;

	// Token: 0x04000598 RID: 1432
	protected float shootTimer;

	// Token: 0x04000599 RID: 1433
	protected AMonsterBase currentTarget;

	// Token: 0x0400059A RID: 1434
	protected eTowerTargetPriority targetPriority;

	// Token: 0x0400059B RID: 1435
	protected bool isInitialized;
}
