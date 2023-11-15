using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
[SelectionBase]
public class Obj_WaterBarrel : MonoBehaviour, IDynamicPlacementTarget
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00013F14 File Offset: 0x00012114
	public bool IsAnimPlaying
	{
		get
		{
			return this.isAnimPlaying;
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00013F1C File Offset: 0x0001211C
	private void Start()
	{
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00013F1E File Offset: 0x0001211E
	private void Update()
	{
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00013F20 File Offset: 0x00012120
	private void OnMouseEnter()
	{
		if (this.attachedTower != null)
		{
			return;
		}
		EventMgr.SendEvent<IDynamicPlacementTarget>(eGameEvents.RegisterDynamicPlacementObject, this);
		EventMgr.SendEvent<Renderer, OutlineController.eOutlineType>(eGameEvents.RequestAddOutline, this.renderer_Barrel, OutlineController.eOutlineType.BASIC);
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00013F56 File Offset: 0x00012156
	private void OnMouseExit()
	{
		if (this.attachedTower != null)
		{
			return;
		}
		EventMgr.SendEvent<IDynamicPlacementTarget>(eGameEvents.UnregisterDynamicPlacementObject, this);
		EventMgr.SendEvent<Renderer>(eGameEvents.RequestRemoveOutline, this.renderer_Barrel);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00013F8B File Offset: 0x0001218B
	public Transform GetPlacementTransform()
	{
		return this.node_PlacementPosition;
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00013F94 File Offset: 0x00012194
	public void PlaceTower(ABaseTower tower)
	{
		if (this.attachedTower != null)
		{
			Debug.LogError("試圖在已經有砲塔的IDynamicPlacementTarget上放置物件!!");
			return;
		}
		EventMgr.SendEvent<IDynamicPlacementTarget>(eGameEvents.UnregisterDynamicPlacementObject, this);
		EventMgr.SendEvent<Renderer>(eGameEvents.RequestRemoveOutline, this.renderer_Barrel);
		this.attachedTower = tower;
		this.animator.SetTrigger("Trigger");
	}

	// Token: 0x040004BE RID: 1214
	[SerializeField]
	private Animator animator;

	// Token: 0x040004BF RID: 1215
	[SerializeField]
	private Renderer renderer_Barrel;

	// Token: 0x040004C0 RID: 1216
	[SerializeField]
	private Transform node_PlacementPosition;

	// Token: 0x040004C1 RID: 1217
	[SerializeField]
	private ABaseTower attachedTower;

	// Token: 0x040004C2 RID: 1218
	private bool isAnimPlaying;
}
