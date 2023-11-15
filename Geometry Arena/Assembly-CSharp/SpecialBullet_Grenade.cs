using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class SpecialBullet_Grenade : Bullet
{
	// Token: 0x06000870 RID: 2160 RVA: 0x00030DEF File Offset: 0x0002EFEF
	protected override void UpdateProps()
	{
		base.UpdateProps();
		this.TotalSize *= this.superGrenadeSize;
		this.TotalSpeed *= this.superGrenadeSpeed;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00030E20 File Offset: 0x0002F020
	public override void EndLife(bool ifNormal)
	{
		if (Player.inst != null && ifNormal)
		{
			BasicUnit unit = Player.inst.unit;
			SpecialEffects.NewExplosionWave(base.transform.position, 4f * base.TransScale, this.mainColor, (double)this.blastDamageMulti * unit.playerFactorTotal.bulletDmg * (double)unit.playerFactorTotal.fireSpd, 0f, 0f, unit.shapeType, false, true, this.ifCrit);
		}
		if (Battle.inst.specialEffect[26] >= 1 && Player.inst != null)
		{
			SpecialEffects.ShootBulletOnce_Mine().transform.position = base.transform.position;
		}
		BattleManager.inst.listGrenades.Remove(this);
		base.EndLife(ifNormal);
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00030EF8 File Offset: 0x0002F0F8
	protected override void SpecialInit()
	{
		BattleManager.inst.listGrenades.Add(this);
		List<Enemy> listEnemies = BattleManager.inst.listEnemies;
		if (listEnemies.Count > 0)
		{
			base.UpdateDirection(listEnemies.GetRandom<Enemy>().transform.position - base.transform.position);
		}
		this.basicSize *= 9f;
		this.blastDamageMulti = 1f;
		if (Battle.inst.specialEffect[81] >= 1)
		{
			this.upgrade_BounceWall = 99999;
			this.upgrade_BounceEnemy = 0;
		}
		this.superGrenadeSize = 1f;
		this.superGrenadeSpeed = 1f;
		this.spr_Body.sprite = this.spr;
		this.spriteRendererCircle.color = this.mainColor;
		SpriteRenderer[] sprs = new SpriteRenderer[]
		{
			this.spr_Body,
			this.spriteRendererCircle
		};
		this.bloom.InitMat(sprs, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
		if (Battle.inst.specialEffect[21] >= 1)
		{
			this.rangeTotal *= 3f;
			this.rangeLeft *= 3f;
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0003102F File Offset: 0x0002F22F
	protected override void SetType()
	{
		this.bulletType = EnumBulletType.GRENADE;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00031038 File Offset: 0x0002F238
	protected override void OnBounceCommon()
	{
		base.OnBounceCommon();
		this.GrenadeBounce();
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00031046 File Offset: 0x0002F246
	protected override void OnHitWall()
	{
		base.OnHitWall();
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0003104E File Offset: 0x0002F24E
	private void GrenadeBounce()
	{
		if (Battle.inst.specialEffect[22] >= 1)
		{
			this.superGrenadeSize = 2f;
			this.superGrenadeSpeed = 2f;
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00031076 File Offset: 0x0002F276
	public void ForceExplode(float dmg)
	{
		this.blastDamageMulti *= dmg;
		this.EndLife(true);
	}

	// Token: 0x040006F9 RID: 1785
	[SerializeField]
	private Sprite spr;

	// Token: 0x040006FA RID: 1786
	[SerializeField]
	private float superGrenadeSize = 1f;

	// Token: 0x040006FB RID: 1787
	[SerializeField]
	private float superGrenadeSpeed = 1f;

	// Token: 0x040006FC RID: 1788
	[SerializeField]
	private float blastDamageMulti = 1f;

	// Token: 0x040006FD RID: 1789
	[SerializeField]
	private SpriteRenderer spriteRendererCircle;
}
