using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class ObjectMaterialControl : MonoBehaviour
{
	// Token: 0x06000459 RID: 1113 RVA: 0x00010DEC File Offset: 0x0000EFEC
	public void Initialize(Transform targetObject)
	{
		this.list_Renderers = new List<Renderer>();
		this.list_OriginalMaterials = new List<Material>();
		foreach (Renderer renderer in targetObject.GetComponentsInChildren<Renderer>())
		{
			if ((renderer is MeshRenderer || renderer is SkinnedMeshRenderer) && renderer.gameObject.layer != LayerMask.NameToLayer("FogOfWarMask") && !renderer.gameObject.name.Equals("Quad_DottedRing"))
			{
				this.list_Renderers.Add(renderer);
				this.list_OriginalMaterials.Add(renderer.sharedMaterial);
			}
		}
		this.material_PlacementMode = (Resources.Load("Materials/Mat_PlacementObject") as Material);
		this.material_Disabled = (Resources.Load("Materials/Mat_DisabledObject") as Material);
		if (this.material_PlacementMode == null || this.material_Disabled == null)
		{
			Debug.LogError("!! 沒有讀取到Resource資料夾的材質(material_PlacementMode / material_Disabled)");
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00010ED3 File Offset: 0x0000F0D3
	private void OnDestroy()
	{
		EventMgr.SendEvent<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, this.list_Renderers);
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00010EEC File Offset: 0x0000F0EC
	public void SwitchMaterial(ObjectMaterialControl.eMaterialType type)
	{
		if (type == this.curMaterialType)
		{
			return;
		}
		switch (type)
		{
		case ObjectMaterialControl.eMaterialType.ORIGINAL:
			EventMgr.SendEvent<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, this.list_Renderers);
			break;
		case ObjectMaterialControl.eMaterialType.PLACEMENT_MODE:
			EventMgr.SendEvent<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, this.list_Renderers);
			EventMgr.SendEvent<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestAddOutlineByList, this.list_Renderers, OutlineController.eOutlineType.BUILD_BLUE);
			this.lastOutlineType = OutlineController.eOutlineType.BUILD_BLUE;
			break;
		case ObjectMaterialControl.eMaterialType.DISABLED:
			EventMgr.SendEvent<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, this.list_Renderers);
			EventMgr.SendEvent<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestAddOutlineByList, this.list_Renderers, OutlineController.eOutlineType.BUILD_RED);
			this.lastOutlineType = OutlineController.eOutlineType.BUILD_RED;
			break;
		}
		this.curMaterialType = type;
	}

	// Token: 0x04000439 RID: 1081
	[SerializeField]
	[Header("控制的Renderer清單")]
	private List<Renderer> list_Renderers;

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	[Header("Renderer的原始材質")]
	private List<Material> list_OriginalMaterials;

	// Token: 0x0400043B RID: 1083
	[SerializeField]
	[Header("建造模式時要顯示的材質")]
	private Material material_PlacementMode;

	// Token: 0x0400043C RID: 1084
	[SerializeField]
	[Header("不可使用時要顯示的材質")]
	private Material material_Disabled;

	// Token: 0x0400043D RID: 1085
	private ObjectMaterialControl.eMaterialType curMaterialType;

	// Token: 0x0400043E RID: 1086
	private OutlineController.eOutlineType lastOutlineType;

	// Token: 0x0200022D RID: 557
	public enum eMaterialType
	{
		// Token: 0x04000AD4 RID: 2772
		ORIGINAL,
		// Token: 0x04000AD5 RID: 2773
		PLACEMENT_MODE,
		// Token: 0x04000AD6 RID: 2774
		DISABLED
	}
}
