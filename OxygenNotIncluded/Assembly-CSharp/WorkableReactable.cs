using System;
using UnityEngine;

// Token: 0x02000412 RID: 1042
public class WorkableReactable : Reactable
{
	// Token: 0x06001606 RID: 5638 RVA: 0x00073EE8 File Offset: 0x000720E8
	public WorkableReactable(Workable workable, HashedString id, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable.gameObject, id, chore_type, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
	{
		this.workable = workable;
		this.allowedDirection = allowed_direction;
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x00073F2C File Offset: 0x0007212C
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.workable == null)
		{
			return false;
		}
		if (this.reactor != null)
		{
			return false;
		}
		Brain component = new_reactor.GetComponent<Brain>();
		if (component == null)
		{
			return false;
		}
		if (!component.IsRunning())
		{
			return false;
		}
		Navigator component2 = new_reactor.GetComponent<Navigator>();
		if (component2 == null)
		{
			return false;
		}
		if (!component2.IsMoving())
		{
			return false;
		}
		if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
		{
			return true;
		}
		Facing component3 = new_reactor.GetComponent<Facing>();
		if (component3 == null)
		{
			return false;
		}
		bool facing = component3.GetFacing();
		return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x00073FD1 File Offset: 0x000721D1
	protected override void InternalBegin()
	{
		this.worker = this.reactor.GetComponent<Worker>();
		this.worker.StartWork(new Worker.StartWorkInfo(this.workable));
	}

	// Token: 0x06001609 RID: 5641 RVA: 0x00073FFA File Offset: 0x000721FA
	public override void Update(float dt)
	{
		if (this.worker.workable == null)
		{
			base.End();
			return;
		}
		if (this.worker.Work(dt) != Worker.WorkResult.InProgress)
		{
			base.End();
		}
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x0007402B File Offset: 0x0007222B
	protected override void InternalEnd()
	{
		if (this.worker != null)
		{
			this.worker.StopWork();
		}
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x00074046 File Offset: 0x00072246
	protected override void InternalCleanup()
	{
	}

	// Token: 0x04000C51 RID: 3153
	protected Workable workable;

	// Token: 0x04000C52 RID: 3154
	private Worker worker;

	// Token: 0x04000C53 RID: 3155
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x02001093 RID: 4243
	public enum AllowedDirection
	{
		// Token: 0x04005999 RID: 22937
		Any,
		// Token: 0x0400599A RID: 22938
		Left,
		// Token: 0x0400599B RID: 22939
		Right
	}
}
