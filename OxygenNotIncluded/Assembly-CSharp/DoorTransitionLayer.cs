using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class DoorTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x06001FB2 RID: 8114 RVA: 0x000A906B File Offset: 0x000A726B
	public DoorTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000A9080 File Offset: 0x000A7280
	private bool AreAllDoorsOpen()
	{
		foreach (INavDoor navDoor in this.doors)
		{
			if (navDoor != null && !navDoor.IsOpen())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000A90E0 File Offset: 0x000A72E0
	protected override bool IsOverrideComplete()
	{
		return base.IsOverrideComplete() && this.AreAllDoorsOpen();
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000A90F4 File Offset: 0x000A72F4
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.doors.Count > 0)
		{
			return;
		}
		int cell = Grid.PosToCell(navigator);
		int cell2 = Grid.OffsetCell(cell, transition.x, transition.y);
		this.AddDoor(cell2);
		if (navigator.CurrentNavType != NavType.Tube)
		{
			this.AddDoor(Grid.CellAbove(cell2));
		}
		for (int i = 0; i < transition.navGridTransition.voidOffsets.Length; i++)
		{
			int cell3 = Grid.OffsetCell(cell, transition.navGridTransition.voidOffsets[i]);
			this.AddDoor(cell3);
		}
		if (this.doors.Count == 0)
		{
			return;
		}
		if (!this.AreAllDoorsOpen())
		{
			base.BeginTransition(navigator, transition);
			transition.anim = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.start;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			navDoor.Open();
		}
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000A9218 File Offset: 0x000A7418
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (this.doors.Count == 0)
		{
			return;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			if (!navDoor.IsNullOrDestroyed())
			{
				navDoor.Close();
			}
		}
		this.doors.Clear();
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000A9294 File Offset: 0x000A7494
	private void AddDoor(int cell)
	{
		INavDoor door = this.GetDoor(cell);
		if (!door.IsNullOrDestroyed() && !this.doors.Contains(door))
		{
			this.doors.Add(door);
		}
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000A92CC File Offset: 0x000A74CC
	private INavDoor GetDoor(int cell)
	{
		if (!Grid.HasDoor[cell])
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			INavDoor navDoor = gameObject.GetComponent<INavDoor>();
			if (navDoor == null)
			{
				navDoor = gameObject.GetSMI<INavDoor>();
			}
			if (navDoor != null && navDoor.isSpawned)
			{
				return navDoor;
			}
		}
		return null;
	}

	// Token: 0x040011B7 RID: 4535
	private List<INavDoor> doors = new List<INavDoor>();
}
