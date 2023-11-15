using System;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
public class EightDirectionController
{
	// Token: 0x1700020A RID: 522
	// (get) Token: 0x0600263E RID: 9790 RVA: 0x000CFDCE File Offset: 0x000CDFCE
	// (set) Token: 0x0600263F RID: 9791 RVA: 0x000CFDD6 File Offset: 0x000CDFD6
	public KBatchedAnimController controller { get; private set; }

	// Token: 0x06002640 RID: 9792 RVA: 0x000CFDDF File Offset: 0x000CDFDF
	public EightDirectionController(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBank)
	{
		this.Initialize(buildingController, targetSymbol, defaultAnim, frontBank, Grid.SceneLayer.NoLayer);
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000CFDF4 File Offset: 0x000CDFF4
	private void Initialize(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBack, Grid.SceneLayer userSpecifiedRenderLayer)
	{
		string name = buildingController.name + ".eight_direction";
		this.gameObject = new GameObject(name);
		this.gameObject.SetActive(false);
		this.gameObject.transform.parent = buildingController.transform;
		this.gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.defaultAnim = defaultAnim;
		this.controller = this.gameObject.AddOrGet<KBatchedAnimController>();
		this.controller.AnimFiles = new KAnimFile[]
		{
			buildingController.AnimFiles[0]
		};
		this.controller.initialAnim = defaultAnim;
		this.controller.isMovable = true;
		this.controller.sceneLayer = Grid.SceneLayer.NoLayer;
		if (EightDirectionController.Offset.UserSpecified == frontBack)
		{
			this.controller.sceneLayer = userSpecifiedRenderLayer;
		}
		buildingController.SetSymbolVisiblity(targetSymbol, false);
		bool flag;
		Vector3 position = buildingController.GetSymbolTransform(new HashedString(targetSymbol), out flag).GetColumn(3);
		switch (frontBack)
		{
		case EightDirectionController.Offset.Infront:
			position.z = buildingController.transform.GetPosition().z - 0.1f;
			break;
		case EightDirectionController.Offset.Behind:
			position.z = buildingController.transform.GetPosition().z + 0.1f;
			break;
		case EightDirectionController.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(userSpecifiedRenderLayer);
			break;
		}
		this.gameObject.transform.SetPosition(position);
		this.gameObject.SetActive(true);
		this.link = new KAnimLink(buildingController, this.controller);
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000CFF7C File Offset: 0x000CE17C
	public void SetPositionPercent(float percent_full)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.SetPositionPercent(percent_full);
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000CFF99 File Offset: 0x000CE199
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.controller != null)
		{
			this.controller.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x000CFFBB File Offset: 0x000CE1BB
	public void SetRotation(float rot)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.Rotation = rot;
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000CFFD8 File Offset: 0x000CE1D8
	public void PlayAnim(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		this.controller.Play(anim, mode, 1f, 0f);
	}

	// Token: 0x040015E0 RID: 5600
	public GameObject gameObject;

	// Token: 0x040015E1 RID: 5601
	private string defaultAnim;

	// Token: 0x040015E2 RID: 5602
	private KAnimLink link;

	// Token: 0x0200129C RID: 4764
	public enum Offset
	{
		// Token: 0x0400602A RID: 24618
		Infront,
		// Token: 0x0400602B RID: 24619
		Behind,
		// Token: 0x0400602C RID: 24620
		UserSpecified
	}
}
