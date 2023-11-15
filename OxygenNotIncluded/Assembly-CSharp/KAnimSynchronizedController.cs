using System;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class KAnimSynchronizedController
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060017AC RID: 6060 RVA: 0x00079E7C File Offset: 0x0007807C
	// (set) Token: 0x060017AD RID: 6061 RVA: 0x00079E84 File Offset: 0x00078084
	public string Postfix
	{
		get
		{
			return this.postfix;
		}
		set
		{
			this.postfix = value;
		}
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x00079E90 File Offset: 0x00078090
	public KAnimSynchronizedController(KAnimControllerBase controller, Grid.SceneLayer layer, string postfix)
	{
		this.controller = controller;
		this.Postfix = postfix;
		GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, controller.gameObject, null);
		gameObject.name = controller.name + postfix;
		this.synchronizedController = gameObject.GetComponent<KAnimControllerBase>();
		this.synchronizedController.AnimFiles = controller.AnimFiles;
		gameObject.SetActive(true);
		this.synchronizedController.initialAnim = controller.initialAnim + postfix;
		this.synchronizedController.defaultAnim = this.synchronizedController.initialAnim;
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(layer) - 0.1f);
		gameObject.transform.SetLocalPosition(position);
		this.link = new KAnimLink(controller, this.synchronizedController);
		this.Dirty();
		KAnimSynchronizer synchronizer = controller.GetSynchronizer();
		synchronizer.Add(this);
		synchronizer.SyncController(this);
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x00079F80 File Offset: 0x00078180
	public void Enable(bool enable)
	{
		this.synchronizedController.enabled = enable;
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x00079F8E File Offset: 0x0007818E
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.synchronizedController.enabled && this.synchronizedController.HasAnimation(anim_name))
		{
			this.synchronizedController.Play(anim_name, mode, speed, time_offset);
		}
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x00079FBC File Offset: 0x000781BC
	public void Dirty()
	{
		if (this.synchronizedController == null)
		{
			return;
		}
		this.synchronizedController.Offset = this.controller.Offset;
		this.synchronizedController.Pivot = this.controller.Pivot;
		this.synchronizedController.Rotation = this.controller.Rotation;
		this.synchronizedController.FlipX = this.controller.FlipX;
		this.synchronizedController.FlipY = this.controller.FlipY;
	}

	// Token: 0x04000D1E RID: 3358
	private KAnimControllerBase controller;

	// Token: 0x04000D1F RID: 3359
	public KAnimControllerBase synchronizedController;

	// Token: 0x04000D20 RID: 3360
	private KAnimLink link;

	// Token: 0x04000D21 RID: 3361
	private string postfix;
}
