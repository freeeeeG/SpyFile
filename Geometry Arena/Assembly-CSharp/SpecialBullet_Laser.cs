using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class SpecialBullet_Laser : Bullet
{
	// Token: 0x06000879 RID: 2169 RVA: 0x000310B6 File Offset: 0x0002F2B6
	protected new void Update()
	{
		this.EndLife(false);
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x000310BF File Offset: 0x0002F2BF
	protected override void UpdateProps()
	{
		base.UpdateProps();
		this.TotalDamage *= (double)this.damageMulti;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x000310DC File Offset: 0x0002F2DC
	public void LaserInit(Color cl, float laserLength, Vector2 direction, BasicUnit source, bool ifColli)
	{
		this.laserCollider2D.enabled = ifColli;
		this.hitCount = 0;
		float[] facs = Player_10_Lasergun.SkillLaser.facs;
		this.lifeTimeMax = Time.deltaTime;
		this.lifeTimeLeft = this.lifeTimeMax;
		this.source = source;
		Factor playerFactorTotal = source.playerFactorTotal;
		this.shapeType = EnumShapeType.TRIANGLE;
		this.length = laserLength;
		this.basicSize = playerFactorTotal.bulletSize / 10f;
		this.basicDamage = source.Fac_ActualBulletDamage;
		this.rangeTotal = laserLength;
		this.rangeLeft = laserLength;
		this.widthMulti = 1f;
		this.damageMulti = 1f;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			SkillModule skillModule_CurrentJobWithEffectID = SkillModule.GetSkillModule_CurrentJobWithEffectID(1);
			this.widthMulti *= skillModule_CurrentJobWithEffectID.facs[0];
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			SkillModule skillModule_CurrentJobWithEffectID2 = SkillModule.GetSkillModule_CurrentJobWithEffectID(2);
			this.widthMulti *= skillModule_CurrentJobWithEffectID2.facs[0];
		}
		if (Player_10_Lasergun.instLasergun.ifOverload)
		{
			if (TempData.inst.GetBool_SkillModuleOpenFlag(7))
			{
				SkillModule skillModule_CurrentJobWithEffectID3 = SkillModule.GetSkillModule_CurrentJobWithEffectID(7);
				this.widthMulti *= skillModule_CurrentJobWithEffectID3.facs[0];
				this.damageMulti *= skillModule_CurrentJobWithEffectID3.facs[1];
			}
			else
			{
				this.widthMulti *= facs[5];
				this.damageMulti *= facs[6];
			}
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(5))
		{
			SkillModule skillModule_CurrentJobWithEffectID4 = SkillModule.GetSkillModule_CurrentJobWithEffectID(5);
			int splitUpgradeCount = Battle.inst.GetSplitUpgradeCount();
			this.widthMulti *= Mathf.Pow(skillModule_CurrentJobWithEffectID4.facs[1], (float)splitUpgradeCount);
			this.damageMulti *= Mathf.Pow(skillModule_CurrentJobWithEffectID4.facs[2], (float)splitUpgradeCount);
		}
		this.FixedUpdate_Scale();
		this.basicSpeed = 0f;
		base.UpdateDirection(direction);
		this.repulse = source.Fac_ActualRepulse;
		if (MyTool.DecimalToBool(playerFactorTotal.critChc))
		{
			this.basicDamage *= (double)playerFactorTotal.critDmg;
			this.ifCrit = true;
		}
		this.mainColor = cl;
		this.mainColor = this.mainColor.SetValue(1f).SetSaturation(0.54f);
		this.spr_Body.color = this.mainColor.SetAlpha(0.6f);
		this.spr_Body_Outline.color = this.mainColor.SetAlpha(0.3f);
		SpriteRenderer[] sprs = new SpriteRenderer[]
		{
			this.spr_Body,
			this.spr_Body_Outline
		};
		this.bloom.InitMat(sprs, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
		this.SetType();
		this.AfterInit();
		this.SpecialInit();
		this.inited = true;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00031394 File Offset: 0x0002F594
	protected override void FixedUpdate_Scale()
	{
		float scaleWithSizeShape = MyTool.GetScaleWithSizeShape(this.shapeType, this.basicSize);
		base.transform.localScale = new Vector2(this.length, scaleWithSizeShape * this.widthMulti);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x000313D8 File Offset: 0x0002F5D8
	public override void EndLife(bool ifNormal)
	{
		if (this.laserCollider2D.enabled)
		{
			this.laserCollider2D.enabled = false;
			if (TempData.inst.GetBool_SkillModuleOpenFlag(6) && this.source != null)
			{
				float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(6).facs;
				if (this.hitCount <= 0)
				{
					this.source.GetHurt((double)facs[2], this.source, Vector2.zero, false, this.source.transform.position, true);
				}
			}
		}
		base.EndLife(ifNormal);
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00031466 File Offset: 0x0002F666
	protected override void Special_OnHitEnemy(Enemy enemy)
	{
		this.hitCount++;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(6) && this.source != null)
		{
			this.source.LifeAdd(1.0, true);
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x000314A6 File Offset: 0x0002F6A6
	protected override void SetType()
	{
		this.bulletType = EnumBulletType.LASER;
	}

	// Token: 0x040006FE RID: 1790
	[SerializeField]
	private SpriteRenderer spr_Body_Outline;

	// Token: 0x040006FF RID: 1791
	[SerializeField]
	private float length;

	// Token: 0x04000700 RID: 1792
	[SerializeField]
	private float lifeTimeLeft = 0.5f;

	// Token: 0x04000701 RID: 1793
	[SerializeField]
	[CustomLabel("存在时间-实际存在时间")]
	private float lifeTimeMax = 0.5f;

	// Token: 0x04000702 RID: 1794
	[SerializeField]
	private Collider2D laserCollider2D;

	// Token: 0x04000703 RID: 1795
	[SerializeField]
	private int hitCount;

	// Token: 0x04000704 RID: 1796
	[SerializeField]
	private float widthMulti = 1f;

	// Token: 0x04000705 RID: 1797
	[SerializeField]
	private float damageMulti = 1f;
}
