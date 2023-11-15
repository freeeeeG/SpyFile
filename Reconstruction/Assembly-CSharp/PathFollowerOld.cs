using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class PathFollowerOld : ReusableObject, IGameBehavior
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x00011170 File Offset: 0x0000F370
	// (set) Token: 0x06000655 RID: 1621 RVA: 0x00011178 File Offset: 0x0000F378
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

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000656 RID: 1622 RVA: 0x00011181 File Offset: 0x0000F381
	// (set) Token: 0x06000657 RID: 1623 RVA: 0x00011189 File Offset: 0x0000F389
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

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000658 RID: 1624 RVA: 0x00011192 File Offset: 0x0000F392
	// (set) Token: 0x06000659 RID: 1625 RVA: 0x0001119A File Offset: 0x0000F39A
	public GameTile SpawnPoint { get; set; }

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x0600065A RID: 1626 RVA: 0x000111A3 File Offset: 0x0000F3A3
	// (set) Token: 0x0600065B RID: 1627 RVA: 0x000111AB File Offset: 0x0000F3AB
	public virtual float Speed
	{
		get
		{
			return this.speed;
		}
		set
		{
			this.speed = value;
		}
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x000111B4 File Offset: 0x0000F3B4
	public virtual bool GameUpdate()
	{
		this.progress += Time.deltaTime * this.progressFactor;
		while (this.progress >= 1f)
		{
			if (this.tileTo == null)
			{
				return true;
			}
			this.progress = 0f;
		}
		if (this.DirectionChange == DirectionChange.None)
		{
			base.transform.localPosition = Vector3.LerpUnclamped(this.positionFrom, this.positionTo, this.progress);
		}
		else
		{
			float z = Mathf.LerpUnclamped(this.directionAngleFrom, this.directionAngleTo, this.progress);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}
		return true;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00011264 File Offset: 0x0000F464
	protected virtual void PrepareOutro()
	{
		this.positionTo = this.tileFrom.transform.position;
		this.DirectionChange = DirectionChange.None;
		this.directionAngleTo = this.Direction.GetAngle();
		this.model.localPosition = new Vector3(this.pathOffset, 0f);
		base.transform.localRotation = this.Direction.GetRotation();
		this.adjust = 2f;
		this.progressFactor = this.adjust * this.Speed;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000112F0 File Offset: 0x0000F4F0
	private void PrepareForward()
	{
		base.transform.localRotation = this.Direction.GetRotation();
		this.directionAngleTo = this.Direction.GetAngle();
		this.model.localPosition = new Vector3(this.pathOffset, 0f);
		this.adjust = 1f;
		this.progressFactor = this.adjust * this.Speed;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00011360 File Offset: 0x0000F560
	private void PrepareTurnRight()
	{
		this.directionAngleTo = this.directionAngleFrom - 90f;
		this.model.localPosition = new Vector3(this.pathOffset - 0.5f, 0f);
		base.transform.localPosition = this.positionFrom + this.Direction.GetHalfVector();
		this.adjust = 1f / (1.5707964f * (0.5f - this.pathOffset));
		this.progressFactor = this.adjust * this.Speed;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000113F4 File Offset: 0x0000F5F4
	private void PrepareTurnLeft()
	{
		this.directionAngleTo = this.directionAngleFrom + 90f;
		this.model.localPosition = new Vector3(this.pathOffset + 0.5f, 0f);
		base.transform.localPosition = this.positionFrom + this.Direction.GetHalfVector();
		this.adjust = 1f / (1.5707964f * (0.5f + this.pathOffset));
		this.progressFactor = this.adjust * this.Speed;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00011488 File Offset: 0x0000F688
	private void PrepareTurnAround()
	{
		this.directionAngleTo = this.directionAngleFrom + ((this.pathOffset < 0f) ? 180f : -180f);
		this.model.localPosition = new Vector3(this.pathOffset, 0f);
		base.transform.localPosition = this.positionFrom;
		this.adjust = 1f / (3.1415927f * Mathf.Max(Mathf.Abs(this.pathOffset), 0.2f));
		this.progressFactor = this.adjust * this.Speed;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00011521 File Offset: 0x0000F721
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.model.localPosition = Vector3.zero;
	}

	// Token: 0x040002C1 RID: 705
	private Direction direction;

	// Token: 0x040002C2 RID: 706
	private DirectionChange directionChange;

	// Token: 0x040002C4 RID: 708
	[SerializeField]
	protected Transform model;

	// Token: 0x040002C5 RID: 709
	[HideInInspector]
	public GameTile tileFrom;

	// Token: 0x040002C6 RID: 710
	[HideInInspector]
	public GameTile tileTo;

	// Token: 0x040002C7 RID: 711
	protected Vector3 positionFrom;

	// Token: 0x040002C8 RID: 712
	protected Vector3 positionTo;

	// Token: 0x040002C9 RID: 713
	protected float progress;

	// Token: 0x040002CA RID: 714
	protected float progressFactor;

	// Token: 0x040002CB RID: 715
	protected float adjust;

	// Token: 0x040002CC RID: 716
	protected float directionAngleFrom;

	// Token: 0x040002CD RID: 717
	protected float directionAngleTo;

	// Token: 0x040002CE RID: 718
	protected float pathOffset;

	// Token: 0x040002CF RID: 719
	protected float speed = 0.8f;
}
