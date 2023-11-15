using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010B RID: 267
[SelectionBase]
public abstract class ABasePanel : MonoBehaviour
{
	// Token: 0x060006C7 RID: 1735 RVA: 0x00018993 File Offset: 0x00016B93
	private void Reset()
	{
		if (this.list_Colliders == null || this.list_Colliders.Count == 0)
		{
			this.FetchCollider();
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x000189B0 File Offset: 0x00016BB0
	private void FetchCollider()
	{
		this.list_Colliders = new List<Collider>();
		foreach (Collider item in base.GetComponentsInChildren<Collider>())
		{
			this.list_Colliders.Add(item);
		}
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x000189ED File Offset: 0x00016BED
	public void Spawn(ABaseTower tower)
	{
		this.SpawnProc();
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x000189F5 File Offset: 0x00016BF5
	public virtual void SpawnProc()
	{
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x000189F7 File Offset: 0x00016BF7
	public void SetCannon(ABaseCannon cannon)
	{
		this.connectedCannon = cannon;
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x00018A00 File Offset: 0x00016C00
	public void Despawn()
	{
		this.DespawnProc();
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00018A08 File Offset: 0x00016C08
	protected virtual void DespawnProc()
	{
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00018A0A File Offset: 0x00016C0A
	public Transform GetCannonPlacementNode()
	{
		return this.node_CannonPlacementPosition;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00018A12 File Offset: 0x00016C12
	public List<Collider> GetColliders()
	{
		if (this.list_Colliders == null || this.list_Colliders.Count == 0)
		{
			Debug.LogError("底座沒有Collider可以回傳");
			return null;
		}
		return this.list_Colliders;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00018A3B File Offset: 0x00016C3B
	public int GetCost(float multiplier = 1f)
	{
		return this.settingData.GetBuildCost(multiplier);
	}

	// Token: 0x0400059C RID: 1436
	[SerializeField]
	[Header("設定資料")]
	protected PanelSettingData settingData;

	// Token: 0x0400059D RID: 1437
	[SerializeField]
	[Header("放置砲台的節點")]
	protected Transform node_CannonPlacementPosition;

	// Token: 0x0400059E RID: 1438
	[SerializeField]
	[Header("Collider")]
	protected List<Collider> list_Colliders;

	// Token: 0x0400059F RID: 1439
	[SerializeField]
	private bool isInitialized;

	// Token: 0x040005A0 RID: 1440
	protected ABaseCannon connectedCannon;
}
