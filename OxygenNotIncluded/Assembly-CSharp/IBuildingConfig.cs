using System;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
public abstract class IBuildingConfig
{
	// Token: 0x06002448 RID: 9288
	public abstract BuildingDef CreateBuildingDef();

	// Token: 0x06002449 RID: 9289 RVA: 0x000C5FE1 File Offset: 0x000C41E1
	public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x0600244A RID: 9290
	public abstract void DoPostConfigureComplete(GameObject go);

	// Token: 0x0600244B RID: 9291 RVA: 0x000C5FE3 File Offset: 0x000C41E3
	public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x000C5FE5 File Offset: 0x000C41E5
	public virtual void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600244D RID: 9293 RVA: 0x000C5FE7 File Offset: 0x000C41E7
	public virtual void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x0600244E RID: 9294 RVA: 0x000C5FE9 File Offset: 0x000C41E9
	public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600244F RID: 9295 RVA: 0x000C5FF0 File Offset: 0x000C41F0
	public virtual bool ForbidFromLoading()
	{
		return false;
	}
}
