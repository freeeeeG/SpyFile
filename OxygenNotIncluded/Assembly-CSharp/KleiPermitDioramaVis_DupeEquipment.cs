using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B47 RID: 2887
public class KleiPermitDioramaVis_DupeEquipment : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06005907 RID: 22791 RVA: 0x00209986 File Offset: 0x00207B86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06005908 RID: 22792 RVA: 0x0020998E File Offset: 0x00207B8E
	public void ConfigureSetup()
	{
		this.uiMannequin.shouldShowOutfitWithDefaultItems = false;
	}

	// Token: 0x06005909 RID: 22793 RVA: 0x0020999C File Offset: 0x00207B9C
	public void ConfigureWith(PermitResource permit)
	{
		ClothingItemResource clothingItemResource = permit as ClothingItemResource;
		if (clothingItemResource != null)
		{
			this.uiMannequin.SetOutfit(clothingItemResource.outfitType, new ClothingItemResource[]
			{
				clothingItemResource
			});
			this.uiMannequin.ReactToClothingItemChange(clothingItemResource.Category);
		}
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(permit.Category);
	}

	// Token: 0x04003C35 RID: 15413
	[SerializeField]
	private UIMannequin uiMannequin;

	// Token: 0x04003C36 RID: 15414
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04003C37 RID: 15415
	[SerializeField]
	private Sprite clothingBG;

	// Token: 0x04003C38 RID: 15416
	[SerializeField]
	private Sprite atmosuitBG;
}
