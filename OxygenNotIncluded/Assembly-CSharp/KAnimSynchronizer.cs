using System;
using System.Collections.Generic;

// Token: 0x0200044B RID: 1099
public class KAnimSynchronizer
{
	// Token: 0x060017B2 RID: 6066 RVA: 0x0007A046 File Offset: 0x00078246
	public KAnimSynchronizer(KAnimControllerBase master_controller)
	{
		this.masterController = master_controller;
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x0007A06B File Offset: 0x0007826B
	private void Clear(KAnimControllerBase controller)
	{
		controller.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x0007A088 File Offset: 0x00078288
	public void Add(KAnimControllerBase controller)
	{
		this.Targets.Add(controller);
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x0007A096 File Offset: 0x00078296
	public void Remove(KAnimControllerBase controller)
	{
		this.Clear(controller);
		this.Targets.Remove(controller);
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x0007A0AC File Offset: 0x000782AC
	private void Clear(KAnimSynchronizedController controller)
	{
		controller.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x0007A0C9 File Offset: 0x000782C9
	public void Add(KAnimSynchronizedController controller)
	{
		this.SyncedControllers.Add(controller);
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x0007A0D7 File Offset: 0x000782D7
	public void Remove(KAnimSynchronizedController controller)
	{
		this.Clear(controller);
		this.SyncedControllers.Remove(controller);
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x0007A0F0 File Offset: 0x000782F0
	public void Clear()
	{
		foreach (KAnimControllerBase kanimControllerBase in this.Targets)
		{
			if (!(kanimControllerBase == null) && kanimControllerBase.AnimFiles != null)
			{
				this.Clear(kanimControllerBase);
			}
		}
		this.Targets.Clear();
		foreach (KAnimSynchronizedController kanimSynchronizedController in this.SyncedControllers)
		{
			if (!(kanimSynchronizedController.synchronizedController == null) && kanimSynchronizedController.synchronizedController.AnimFiles != null)
			{
				this.Clear(kanimSynchronizedController);
			}
		}
		this.SyncedControllers.Clear();
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x0007A1C8 File Offset: 0x000783C8
	public void Sync(KAnimControllerBase controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		if (currentAnim != null && !string.IsNullOrEmpty(controller.defaultAnim) && !controller.HasAnimation(currentAnim.name))
		{
			controller.Play(controller.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(currentAnim.name, mode, playSpeed, elapsedTime);
		Facing component = controller.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.FlipX = this.masterController.FlipX;
		controller.FlipY = this.masterController.FlipY;
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x0007A2E8 File Offset: 0x000784E8
	public void SyncController(KAnimSynchronizedController controller)
	{
		if (this.masterController == null)
		{
			return;
		}
		if (controller == null)
		{
			return;
		}
		KAnim.Anim currentAnim = this.masterController.GetCurrentAnim();
		string s = (currentAnim != null) ? (currentAnim.name + controller.Postfix) : string.Empty;
		if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) && !controller.synchronizedController.HasAnimation(s))
		{
			controller.Play(controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (currentAnim == null)
		{
			return;
		}
		KAnim.PlayMode mode = this.masterController.GetMode();
		float playSpeed = this.masterController.GetPlaySpeed();
		float elapsedTime = this.masterController.GetElapsedTime();
		controller.Play(s, mode, playSpeed, elapsedTime);
		Facing component = controller.synchronizedController.GetComponent<Facing>();
		if (component != null)
		{
			float num = component.transform.GetPosition().x;
			num += (this.masterController.FlipX ? -0.5f : 0.5f);
			component.Face(num);
			return;
		}
		controller.synchronizedController.FlipX = this.masterController.FlipX;
		controller.synchronizedController.FlipY = this.masterController.FlipY;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x0007A430 File Offset: 0x00078630
	public void Sync()
	{
		for (int i = 0; i < this.Targets.Count; i++)
		{
			KAnimControllerBase controller = this.Targets[i];
			this.Sync(controller);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			KAnimSynchronizedController controller2 = this.SyncedControllers[j];
			this.SyncController(controller2);
		}
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x0007A494 File Offset: 0x00078694
	public void SyncTime()
	{
		float elapsedTime = this.masterController.GetElapsedTime();
		for (int i = 0; i < this.Targets.Count; i++)
		{
			this.Targets[i].SetElapsedTime(elapsedTime);
		}
		for (int j = 0; j < this.SyncedControllers.Count; j++)
		{
			this.SyncedControllers[j].synchronizedController.SetElapsedTime(elapsedTime);
		}
	}

	// Token: 0x04000D22 RID: 3362
	private KAnimControllerBase masterController;

	// Token: 0x04000D23 RID: 3363
	private List<KAnimControllerBase> Targets = new List<KAnimControllerBase>();

	// Token: 0x04000D24 RID: 3364
	private List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();
}
