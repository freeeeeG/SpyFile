using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class KAnimLayering
{
	// Token: 0x06001796 RID: 6038 RVA: 0x000796D2 File Offset: 0x000778D2
	public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
	{
		this.controller = controller;
		this.layer = layer;
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x000796F0 File Offset: 0x000778F0
	public void SetLayer(Grid.SceneLayer layer)
	{
		this.layer = layer;
		if (this.foregroundController != null)
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.transform.SetLocalPosition(position);
		}
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x0007975C File Offset: 0x0007795C
	public void SetIsForeground(bool is_foreground)
	{
		this.isForeground = is_foreground;
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x00079765 File Offset: 0x00077965
	public bool GetIsForeground()
	{
		return this.isForeground;
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x0007976D File Offset: 0x0007796D
	public KAnimLink GetLink()
	{
		return this.link;
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x00079778 File Offset: 0x00077978
	private static bool IsAnimLayered(KAnimFile[] anims)
	{
		foreach (KAnimFile kanimFile in anims)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x000797E0 File Offset: 0x000779E0
	private void HideSymbolsInternal()
	{
		foreach (KAnimFile kanimFile in this.controller.AnimFiles)
		{
			if (!(kanimFile == null))
			{
				KAnimFileData data = kanimFile.GetData();
				if (data.build != null)
				{
					KAnim.Build.Symbol[] symbols = data.build.symbols;
					for (int j = 0; j < symbols.Length; j++)
					{
						if ((symbols[j].flags & 8) != 0 != this.isForeground && !(symbols[j].hash == KAnimLayering.UI))
						{
							this.controller.SetSymbolVisiblity(symbols[j].hash, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x0007988C File Offset: 0x00077A8C
	public void HideSymbols()
	{
		if (EntityPrefabs.Instance == null)
		{
			return;
		}
		if (this.isForeground)
		{
			return;
		}
		KAnimFile[] animFiles = this.controller.AnimFiles;
		bool flag = KAnimLayering.IsAnimLayered(animFiles);
		if (flag && this.layer != Grid.SceneLayer.NoLayer)
		{
			bool flag2 = this.foregroundController == null;
			if (flag2)
			{
				GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, this.controller.gameObject, null);
				gameObject.name = this.controller.name + "_fg";
				this.foregroundController = gameObject.GetComponent<KAnimControllerBase>();
				this.link = new KAnimLink(this.controller, this.foregroundController);
			}
			this.foregroundController.AnimFiles = animFiles;
			this.foregroundController.GetLayering().SetIsForeground(true);
			this.foregroundController.initialAnim = this.controller.initialAnim;
			this.Dirty();
			KAnimSynchronizer synchronizer = this.controller.GetSynchronizer();
			if (flag2)
			{
				synchronizer.Add(this.foregroundController);
			}
			else
			{
				this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
			}
			synchronizer.Sync(this.foregroundController);
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(this.layer) - this.controller.gameObject.transform.GetPosition().z - 0.1f);
			this.foregroundController.gameObject.transform.SetLocalPosition(position);
			this.foregroundController.gameObject.SetActive(true);
		}
		else if (!flag && this.foregroundController != null)
		{
			this.controller.GetSynchronizer().Remove(this.foregroundController);
			this.foregroundController.gameObject.DeleteObject();
			this.link = null;
		}
		if (this.foregroundController != null)
		{
			this.HideSymbolsInternal();
			KAnimLayering layering = this.foregroundController.GetLayering();
			if (layering != null)
			{
				layering.HideSymbolsInternal();
			}
		}
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x00079A8F File Offset: 0x00077C8F
	public void RefreshForegroundBatchGroup()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(this.foregroundController.AnimFiles);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x00079ABC File Offset: 0x00077CBC
	public void Dirty()
	{
		if (this.foregroundController == null)
		{
			return;
		}
		this.foregroundController.Offset = this.controller.Offset;
		this.foregroundController.Pivot = this.controller.Pivot;
		this.foregroundController.Rotation = this.controller.Rotation;
		this.foregroundController.FlipX = this.controller.FlipX;
		this.foregroundController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04000D10 RID: 3344
	private bool isForeground;

	// Token: 0x04000D11 RID: 3345
	private KAnimControllerBase controller;

	// Token: 0x04000D12 RID: 3346
	private KAnimControllerBase foregroundController;

	// Token: 0x04000D13 RID: 3347
	private KAnimLink link;

	// Token: 0x04000D14 RID: 3348
	private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04000D15 RID: 3349
	public static readonly KAnimHashedString UI = new KAnimHashedString("ui");
}
