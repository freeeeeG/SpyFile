using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000D RID: 13
public abstract class AObj_RandomPlacement : MonoBehaviour
{
	// Token: 0x06000012 RID: 18 RVA: 0x0000216F File Offset: 0x0000036F
	private void Awake()
	{
		this.list_PlacedObjects = new List<GameObject>();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000217C File Offset: 0x0000037C
	private void Start()
	{
		if (this.doAutoPlacementOnStart)
		{
			this.TriggerRandomPlacement();
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x0000218C File Offset: 0x0000038C
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.RequestStartRandomPlacement, new Action(this.OnRequestStartRandomPlacement));
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000021A6 File Offset: 0x000003A6
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.RequestStartRandomPlacement, new Action(this.OnRequestStartRandomPlacement));
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000021C0 File Offset: 0x000003C0
	private void OnRequestStartRandomPlacement()
	{
		if (this.enableType != AObj_RandomPlacement.eEnableType.Always)
		{
			if (this.enableType == AObj_RandomPlacement.eEnableType.CorruptedOnly)
			{
				if (!GameDataManager.instance.IntermediateData.isCorrupted)
				{
					return;
				}
			}
			else if (this.enableType == AObj_RandomPlacement.eEnableType.NormalOnly && GameDataManager.instance.IntermediateData.isCorrupted)
			{
				return;
			}
		}
		this.TriggerRandomPlacement();
	}

	// Token: 0x06000017 RID: 23
	public abstract void TriggerRandomPlacement();

	// Token: 0x04000016 RID: 22
	[SerializeField]
	protected List<RandomPlacementData> list_RandomPlacementData;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	[Header("是否專屬於腐化場景")]
	protected AObj_RandomPlacement.eEnableType enableType;

	// Token: 0x04000018 RID: 24
	[SerializeField]
	[Header("是否遊戲開始就自動放置")]
	protected bool doAutoPlacementOnStart;

	// Token: 0x04000019 RID: 25
	protected List<GameObject> list_PlacedObjects;

	// Token: 0x020001D1 RID: 465
	public enum eEnableType
	{
		// Token: 0x0400099B RID: 2459
		[InspectorName("永遠啟用")]
		Always,
		// Token: 0x0400099C RID: 2460
		[InspectorName("正常關限定")]
		NormalOnly,
		// Token: 0x0400099D RID: 2461
		[InspectorName("腐化關限定")]
		CorruptedOnly
	}
}
