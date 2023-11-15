using System;
using Database;
using UnityEngine;

// Token: 0x02000B43 RID: 2883
public class KleiPermitDioramaVis_BuildingOnBackground : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x060058F6 RID: 22774 RVA: 0x002096A0 File Offset: 0x002078A0
	public void ConfigureSetup()
	{
		this.buildingKAnimPrefab.gameObject.SetActive(false);
		this.buildingKAnimArray = new KBatchedAnimController[9];
		for (int i = 0; i < this.buildingKAnimArray.Length; i++)
		{
			this.buildingKAnimArray[i] = (KBatchedAnimController)UnityEngine.Object.Instantiate(this.buildingKAnimPrefab, this.buildingKAnimPrefab.transform.parent, false);
		}
		Vector2 anchoredPosition = this.buildingKAnimPrefab.rectTransform().anchoredPosition;
		Vector2 a = 175f * Vector2.one;
		Vector2 a2 = anchoredPosition + a * new Vector2(-1f, 0f);
		int num = 0;
		for (int j = 0; j < 3; j++)
		{
			int k = 0;
			while (k < 3)
			{
				this.buildingKAnimArray[num].rectTransform().anchoredPosition = a2 + a * new Vector2((float)j, (float)k);
				this.buildingKAnimArray[num].gameObject.SetActive(true);
				k++;
				num++;
			}
		}
	}

	// Token: 0x060058F7 RID: 22775 RVA: 0x002097A4 File Offset: 0x002079A4
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x002097AC File Offset: 0x002079AC
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		BuildingDef value = KleiPermitVisUtil.GetBuildingDef(permit).Value;
		DebugUtil.DevAssert(value.WidthInCells == 1, "assert failed", null);
		DebugUtil.DevAssert(value.HeightInCells == 1, "assert failed", null);
		KBatchedAnimController[] array = this.buildingKAnimArray;
		for (int i = 0; i < array.Length; i++)
		{
			KleiPermitVisUtil.ConfigureToRenderBuilding(array[i], buildingPermit);
		}
	}

	// Token: 0x04003C2B RID: 15403
	[SerializeField]
	private KBatchedAnimController buildingKAnimPrefab;

	// Token: 0x04003C2C RID: 15404
	private KBatchedAnimController[] buildingKAnimArray;
}
