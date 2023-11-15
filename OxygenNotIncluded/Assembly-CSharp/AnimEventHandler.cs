using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06001928 RID: 6440 RVA: 0x00083EA8 File Offset: 0x000820A8
	// (remove) Token: 0x06001929 RID: 6441 RVA: 0x00083EE0 File Offset: 0x000820E0
	private event AnimEventHandler.SetPos onWorkTargetSet;

	// Token: 0x0600192A RID: 6442 RVA: 0x00083F15 File Offset: 0x00082115
	public void SetDirty()
	{
		this.isDirty = 2;
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x00083F20 File Offset: 0x00082120
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (KBatchedAnimTracker kbatchedAnimTracker in base.GetComponentsInChildren<KBatchedAnimTracker>(true))
		{
			if (kbatchedAnimTracker.useTargetPoint)
			{
				this.onWorkTargetSet += kbatchedAnimTracker.SetTarget;
			}
		}
		this.baseOffset = this.animCollider.offset;
		this.instanceIndex = AnimEventHandler.InstanceSequence++;
		this.SetDirty();
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00083F91 File Offset: 0x00082191
	protected override void OnForcedCleanUp()
	{
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x00083FA0 File Offset: 0x000821A0
	public HashedString GetContext()
	{
		return this.context;
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x00083FA8 File Offset: 0x000821A8
	public void UpdateWorkTarget(Vector3 pos)
	{
		if (this.onWorkTargetSet != null)
		{
			this.onWorkTargetSet(pos);
		}
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x00083FBE File Offset: 0x000821BE
	public void SetContext(HashedString context)
	{
		this.context = context;
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x00083FC7 File Offset: 0x000821C7
	public void SetTargetPos(Vector3 target_pos)
	{
		this.targetPos = target_pos;
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x00083FD0 File Offset: 0x000821D0
	public Vector3 GetTargetPos()
	{
		return this.targetPos;
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x00083FD8 File Offset: 0x000821D8
	public void ClearContext()
	{
		this.context = default(HashedString);
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x00083FE8 File Offset: 0x000821E8
	public void LateUpdate()
	{
		int num = Time.frameCount % 3;
		int num2 = this.instanceIndex % 3;
		if (num != num2 && this.isDirty <= 0)
		{
			return;
		}
		this.UpdateOffset();
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x00084018 File Offset: 0x00082218
	public void UpdateOffset()
	{
		Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
		Vector3 vector = this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
		this.animCollider.offset = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - base.transform.GetPosition().x - vector.x, this.baseOffset.y + pivotSymbolPosition.y - base.transform.GetPosition().y + vector.y);
		this.isDirty = Mathf.Max(0, this.isDirty - 1);
	}

	// Token: 0x04000DE0 RID: 3552
	private const int UPDATE_FRAME_RATE = 3;

	// Token: 0x04000DE1 RID: 3553
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04000DE2 RID: 3554
	[MyCmpGet]
	private KBoxCollider2D animCollider;

	// Token: 0x04000DE3 RID: 3555
	[MyCmpGet]
	private Navigator navigator;

	// Token: 0x04000DE4 RID: 3556
	private Vector3 targetPos;

	// Token: 0x04000DE6 RID: 3558
	public Vector2 baseOffset;

	// Token: 0x04000DE7 RID: 3559
	public int isDirty;

	// Token: 0x04000DE8 RID: 3560
	private HashedString context;

	// Token: 0x04000DE9 RID: 3561
	private int instanceIndex;

	// Token: 0x04000DEA RID: 3562
	private static int InstanceSequence;

	// Token: 0x020010F5 RID: 4341
	// (Invoke) Token: 0x060077C3 RID: 30659
	private delegate void SetPos(Vector3 pos);
}
