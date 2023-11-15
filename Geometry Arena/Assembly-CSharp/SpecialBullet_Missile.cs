using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class SpecialBullet_Missile : Bullet
{
	// Token: 0x06000892 RID: 2194 RVA: 0x00031AD5 File Offset: 0x0002FCD5
	protected override void UpdateProps()
	{
		base.UpdateProps();
		this.TotalDamage *= (double)this.missleSplitDamage;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00031AF4 File Offset: 0x0002FCF4
	protected override void SpecialInFixedUpdate()
	{
		if (Battle.inst.specialEffect[75] >= 1)
		{
			Vector2 dirTarget = MyTool.MousePos() - base.transform.position;
			float maxAngle = this.rotateAngleSpeed * Time.fixedDeltaTime;
			base.UpdateDirection(this.direction.DirectionApproach(dirTarget, maxAngle));
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00031B4C File Offset: 0x0002FD4C
	protected override void SetType()
	{
		this.bulletType = EnumBulletType.MISSLE;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00031B58 File Offset: 0x0002FD58
	protected override void SpecialInit()
	{
		this.spr_Body.sprite = ResourceLibrary.Inst.Sprite_Star;
		if (!Battle.IfHasSpecialEffect(39))
		{
			this.rangeTotal *= 3f;
			this.rangeLeft *= 3f;
			this.basicDamage *= 3.0;
		}
		else
		{
			this.basicDamage *= 9.0;
		}
		this.upgrade_BounceWall = 0;
		this.upgrade_BounceEnemy = 0;
		if (Battle.IfHasSpecialEffect(33))
		{
			Vector2 dir = MyTool.MousePos() - base.transform.position;
			base.UpdateDirection(dir);
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00031C0E File Offset: 0x0002FE0E
	public override void Awake()
	{
		this.hasDie = false;
		this.missleSplitDamage = 1f;
		this.rotateAngleSpeed = Player.inst.unit.playerFactorTotal.bulletSpd * 12f;
		base.Awake();
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00031C48 File Offset: 0x0002FE48
	protected override void AfterSplit()
	{
		if (Battle.IfHasSpecialEffect(32))
		{
			this.missleSplitDamage += 1f;
		}
	}

	// Token: 0x0400070F RID: 1807
	[SerializeField]
	private float rotateAngleSpeed = 100f;

	// Token: 0x04000710 RID: 1808
	[SerializeField]
	private float missleSplitDamage = 1f;
}
