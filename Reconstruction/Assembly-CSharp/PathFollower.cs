using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FA RID: 250
public class PathFollower : ReusableObject, IGameBehavior
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000630 RID: 1584 RVA: 0x00010A53 File Offset: 0x0000EC53
	protected virtual bool IsPathfollower
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000631 RID: 1585 RVA: 0x00010A56 File Offset: 0x0000EC56
	// (set) Token: 0x06000632 RID: 1586 RVA: 0x00010A5E File Offset: 0x0000EC5E
	public Direction Direction
	{
		get
		{
			return this.direction;
		}
		set
		{
			this.direction = value;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x00010A67 File Offset: 0x0000EC67
	// (set) Token: 0x06000634 RID: 1588 RVA: 0x00010A6F File Offset: 0x0000EC6F
	public virtual DirectionChange DirectionChange
	{
		get
		{
			return this.directionChange;
		}
		set
		{
			this.directionChange = value;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x00010A78 File Offset: 0x0000EC78
	// (set) Token: 0x06000636 RID: 1590 RVA: 0x00010A80 File Offset: 0x0000EC80
	public Vector3 PositionFrom
	{
		get
		{
			return this.positionFrom;
		}
		set
		{
			this.positionFrom = value;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000637 RID: 1591 RVA: 0x00010A89 File Offset: 0x0000EC89
	// (set) Token: 0x06000638 RID: 1592 RVA: 0x00010A91 File Offset: 0x0000EC91
	public Vector3 PositionTo
	{
		get
		{
			return this.positionTo;
		}
		set
		{
			this.positionTo = value;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x00010A9A File Offset: 0x0000EC9A
	// (set) Token: 0x0600063A RID: 1594 RVA: 0x00010AA2 File Offset: 0x0000ECA2
	public float DirectionAngleFrom
	{
		get
		{
			return this.directionAngleFrom;
		}
		set
		{
			this.directionAngleFrom = value;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x0600063B RID: 1595 RVA: 0x00010AAB File Offset: 0x0000ECAB
	// (set) Token: 0x0600063C RID: 1596 RVA: 0x00010AB3 File Offset: 0x0000ECB3
	public float DirectionAngleTo
	{
		get
		{
			return this.directionAngleTo;
		}
		set
		{
			this.directionAngleTo = value;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x0600063D RID: 1597 RVA: 0x00010ABC File Offset: 0x0000ECBC
	public virtual float Speed
	{
		get
		{
			return this.speed;
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x0600063E RID: 1598 RVA: 0x00010AC4 File Offset: 0x0000ECC4
	// (set) Token: 0x0600063F RID: 1599 RVA: 0x00010ACC File Offset: 0x0000ECCC
	public float Adjust
	{
		get
		{
			return this.adjust;
		}
		set
		{
			this.adjust = value;
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x00010AD5 File Offset: 0x0000ECD5
	// (set) Token: 0x06000641 RID: 1601 RVA: 0x00010ADD File Offset: 0x0000ECDD
	public virtual float ProgressFactor
	{
		get
		{
			return this.progressFactor;
		}
		set
		{
			this.progressFactor = value;
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000642 RID: 1602 RVA: 0x00010AE6 File Offset: 0x0000ECE6
	// (set) Token: 0x06000643 RID: 1603 RVA: 0x00010AEE File Offset: 0x0000ECEE
	public float Progress
	{
		get
		{
			return this.progress;
		}
		set
		{
			this.progress = value;
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000644 RID: 1604 RVA: 0x00010AF7 File Offset: 0x0000ECF7
	// (set) Token: 0x06000645 RID: 1605 RVA: 0x00010AFF File Offset: 0x0000ECFF
	public List<PathPoint> PathPoints
	{
		get
		{
			return this.pathPoints;
		}
		set
		{
			this.pathPoints = value;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000646 RID: 1606 RVA: 0x00010B08 File Offset: 0x0000ED08
	// (set) Token: 0x06000647 RID: 1607 RVA: 0x00010B10 File Offset: 0x0000ED10
	public float PathOffset
	{
		get
		{
			return this.pathOffset;
		}
		set
		{
			this.pathOffset = value;
		}
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00010B19 File Offset: 0x0000ED19
	protected virtual void Awake()
	{
		this.model = base.transform.Find("Model");
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00010B34 File Offset: 0x0000ED34
	public virtual bool GameUpdate()
	{
		this.Progress += Time.deltaTime * this.ProgressFactor;
		while (this.Progress >= 1f)
		{
			if (this.PointIndex == this.PathPoints.Count - 1)
			{
				this.SpawnOn(0, null);
				return true;
			}
			this.Progress = 0f;
			this.PrepareNextState();
		}
		if (this.DirectionChange == DirectionChange.None)
		{
			base.transform.localPosition = Vector3.LerpUnclamped(this.positionFrom, this.positionTo, this.Progress);
		}
		else
		{
			float z = Mathf.LerpUnclamped(this.directionAngleFrom, this.directionAngleTo, this.Progress);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}
		return true;
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x00010BFC File Offset: 0x0000EDFC
	public virtual void SpawnOn(int pointIndex, List<PathPoint> path = null)
	{
		if (path != null)
		{
			this.pathPoints = path;
		}
		if (this.PathPoints.Count <= 1)
		{
			Debug.Log("没有路可走");
		}
		this.PointIndex = pointIndex;
		this.CurrentPoint = this.PathPoints[this.PointIndex];
		this.Progress = 0f;
		this.PrepareIntro();
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00010C5C File Offset: 0x0000EE5C
	protected virtual void PrepareIntro()
	{
		this.positionFrom = this.CurrentPoint.PathPos;
		base.transform.position = this.CurrentPoint.PathPos;
		if (this.PointIndex == this.PathPoints.Count - 1)
		{
			this.positionTo = this.CurrentPoint.PathPos;
			base.transform.localRotation = this.PathPoints[this.PointIndex - 2].PathDirection.GetRotation();
			this.Progress = 1f;
		}
		else
		{
			this.positionTo = this.CurrentPoint.ExitPoint;
			this.Direction = this.CurrentPoint.PathDirection;
			this.DirectionChange = DirectionChange.None;
			this.model.localPosition = new Vector3(this.PathOffset, 0f);
			this.directionAngleFrom = (this.directionAngleTo = this.Direction.GetAngle());
			base.transform.localRotation = this.CurrentPoint.PathDirection.GetRotation();
		}
		this.Adjust = 2f;
		this.ProgressFactor = this.Adjust * this.Speed;
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00010D98 File Offset: 0x0000EF98
	protected virtual void PrepareOutro()
	{
		this.positionFrom = this.positionTo;
		this.positionTo = this.CurrentPoint.PathPos;
		this.DirectionChange = DirectionChange.None;
		this.directionAngleTo = this.Direction.GetAngle();
		this.model.localPosition = new Vector3(this.PathOffset, 0f);
		base.transform.localRotation = this.Direction.GetRotation();
		this.Adjust = 2f;
		this.ProgressFactor = this.Adjust * this.Speed;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00010E30 File Offset: 0x0000F030
	protected virtual void PrepareNextState()
	{
		this.PointIndex++;
		this.CurrentPoint = this.PathPoints[this.PointIndex];
		if (this.PointIndex >= this.PathPoints.Count - 1)
		{
			this.PrepareOutro();
			return;
		}
		this.positionFrom = this.positionTo;
		this.positionTo = this.CurrentPoint.ExitPoint;
		this.DirectionChange = this.Direction.GetDirectionChangeTo(this.CurrentPoint.PathDirection);
		this.Direction = this.CurrentPoint.PathDirection;
		this.directionAngleFrom = this.directionAngleTo;
		switch (this.DirectionChange)
		{
		case DirectionChange.None:
			this.PrepareForward();
			return;
		case DirectionChange.TurnRight:
			this.PrepareTurnRight();
			return;
		case DirectionChange.TurnLeft:
			this.PrepareTurnLeft();
			return;
		case DirectionChange.TurnAround:
			this.PrepareTurnAround();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00010F14 File Offset: 0x0000F114
	private void PrepareForward()
	{
		base.transform.localRotation = this.Direction.GetRotation();
		this.directionAngleTo = this.Direction.GetAngle();
		this.model.localPosition = new Vector3(this.PathOffset, 0f);
		this.Adjust = 1f;
		this.ProgressFactor = this.Adjust * this.Speed;
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00010F84 File Offset: 0x0000F184
	private void PrepareTurnRight()
	{
		this.directionAngleTo = this.directionAngleFrom - 90f;
		this.model.localPosition = new Vector3(this.PathOffset - 0.5f, 0f);
		base.transform.localPosition = this.positionFrom + this.Direction.GetHalfVector();
		this.Adjust = 1f / (1.5707964f * (0.5f - this.PathOffset));
		this.ProgressFactor = this.Adjust * this.Speed;
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00011018 File Offset: 0x0000F218
	private void PrepareTurnLeft()
	{
		this.directionAngleTo = this.directionAngleFrom + 90f;
		this.model.localPosition = new Vector3(this.PathOffset + 0.5f, 0f);
		base.transform.localPosition = this.positionFrom + this.Direction.GetHalfVector();
		this.Adjust = 1f / (1.5707964f * (0.5f + this.PathOffset));
		this.ProgressFactor = this.Adjust * this.Speed;
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x000110AC File Offset: 0x0000F2AC
	private void PrepareTurnAround()
	{
		this.directionAngleTo = this.directionAngleFrom + ((this.PathOffset > 0f) ? 180f : -180f);
		this.model.localPosition = new Vector3(this.PathOffset, 0f);
		base.transform.localPosition = this.positionFrom;
		this.Adjust = 1f / (3.1415927f * Mathf.Max(Mathf.Abs(this.PathOffset), 0.2f));
		this.ProgressFactor = this.Adjust * this.Speed;
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00011145 File Offset: 0x0000F345
	public override void OnSpawn()
	{
		base.OnSpawn();
		this.model.localPosition = Vector3.zero;
	}

	// Token: 0x040002B2 RID: 690
	private List<PathPoint> pathPoints;

	// Token: 0x040002B3 RID: 691
	public int PointIndex;

	// Token: 0x040002B4 RID: 692
	private Direction direction;

	// Token: 0x040002B5 RID: 693
	private DirectionChange directionChange;

	// Token: 0x040002B6 RID: 694
	[HideInInspector]
	public Transform model;

	// Token: 0x040002B7 RID: 695
	protected PathPoint CurrentPoint;

	// Token: 0x040002B8 RID: 696
	protected Vector3 positionFrom;

	// Token: 0x040002B9 RID: 697
	protected Vector3 positionTo;

	// Token: 0x040002BA RID: 698
	private float progress;

	// Token: 0x040002BB RID: 699
	protected float directionAngleFrom;

	// Token: 0x040002BC RID: 700
	protected float directionAngleTo;

	// Token: 0x040002BD RID: 701
	private float pathOffset;

	// Token: 0x040002BE RID: 702
	protected float speed = 0.25f;

	// Token: 0x040002BF RID: 703
	private float adjust;

	// Token: 0x040002C0 RID: 704
	private float progressFactor;
}
