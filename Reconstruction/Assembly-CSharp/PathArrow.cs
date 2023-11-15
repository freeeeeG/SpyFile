using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class PathArrow : PathFollower
{
	// Token: 0x0600062C RID: 1580 RVA: 0x00010924 File Offset: 0x0000EB24
	public void PathArrowUpdate(float progress)
	{
		base.Progress = progress;
		if (base.Progress >= 1f - this.progressOffset)
		{
			if (this.PointIndex == base.PathPoints.Count - 1)
			{
				this.SpawnOn(0, null);
				return;
			}
			base.Progress = 0f;
			this.PrepareNextState();
		}
		if (this.DirectionChange == DirectionChange.None)
		{
			base.transform.localPosition = Vector3.LerpUnclamped(this.positionFrom, this.positionTo, base.Progress);
			return;
		}
		float z = Mathf.LerpUnclamped(this.directionAngleFrom, this.directionAngleTo, base.Progress);
		base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x000109D9 File Offset: 0x0000EBD9
	protected override void PrepareIntro()
	{
		base.PrepareIntro();
		base.PositionFrom = this.CurrentPoint.PathPos - base.Direction.GetHalfVector();
		this.progressOffset = 0f;
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00010A12 File Offset: 0x0000EC12
	protected override void PrepareOutro()
	{
		base.PrepareOutro();
		base.PositionTo = this.CurrentPoint.PathPos + base.Direction.GetHalfVector();
		this.progressOffset = 0.5f;
	}

	// Token: 0x040002B1 RID: 689
	private float progressOffset;
}
