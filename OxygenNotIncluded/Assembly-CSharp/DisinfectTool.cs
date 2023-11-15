using System;
using UnityEngine;

// Token: 0x02000816 RID: 2070
public class DisinfectTool : DragTool
{
	// Token: 0x06003B9C RID: 15260 RVA: 0x0014B14B File Offset: 0x0014934B
	public static void DestroyInstance()
	{
		DisinfectTool.Instance = null;
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x0014B153 File Offset: 0x00149353
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisinfectTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
		this.viewMode = OverlayModes.Disease.ID;
	}

	// Token: 0x06003B9E RID: 15262 RVA: 0x0014B173 File Offset: 0x00149373
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0014B180 File Offset: 0x00149380
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				Disinfectable component = gameObject.GetComponent<Disinfectable>();
				if (component != null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					component.MarkForDisinfect(false);
				}
			}
		}
	}

	// Token: 0x04002742 RID: 10050
	public static DisinfectTool Instance;
}
