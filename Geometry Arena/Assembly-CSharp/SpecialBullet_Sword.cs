using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class SpecialBullet_Sword : Bullet
{
	// Token: 0x06000328 RID: 808 RVA: 0x000135B0 File Offset: 0x000117B0
	public void InitSword(Player_9_SwordMaster player, float angleZ)
	{
		this.swordMaster = player;
		this.source = player.unit;
		this.angleZ = angleZ;
		this.Common_UpdateSprite();
		if (this.swordState == SpecialBullet_Sword.EnumSwordState.INHAND)
		{
			this.InHand_UpdateLocalTransform();
		}
		this.box2D.enabled = false;
		this.shapeType = EnumShapeType.CIRCLE;
		this.inited = true;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00013608 File Offset: 0x00011808
	private void InHand_UpdateLocalTransform()
	{
		if (this.swordMaster == null || this.swordMaster.unit == null)
		{
			return;
		}
		if (this.swordState != SpecialBullet_Sword.EnumSwordState.INHAND)
		{
			Debug.LogError("Error_SwordState!=INHAND !");
			return;
		}
		base.transform.parent = null;
		base.transform.localRotation = Quaternion.Euler(0f, 0f, this.angleZ + this.source.transform.eulerAngles.z);
		base.transform.localPosition = this.GetVector2_InHandPosition_World();
	}

	// Token: 0x0600032A RID: 810 RVA: 0x000136A4 File Offset: 0x000118A4
	public Vector2 GetVector2_InHandPosition_Local()
	{
		this.radiusFix = this.swordMaster.setting_SwordRadiusFix + this.swordMaster.transform.localScale.x / 2f;
		float num = this.radiusFix + this.sword_Length / 2f;
		float f = this.angleZ / 180f * 3.1415927f;
		float x = num * Mathf.Cos(f);
		float y = num * Mathf.Sin(f);
		return new Vector2(x, y);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0001371C File Offset: 0x0001191C
	public Vector2 GetVector2_InHandPosition_World()
	{
		return this.GetVector2_InHandPosition_Local().Rotate(this.source.transform.eulerAngles.z) + this.source.transform.position;
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00013758 File Offset: 0x00011958
	private void Common_UpdateSprite()
	{
		if (this.swordMaster == null || this.swordMaster.unit == null)
		{
			return;
		}
		bool flag = TempData.inst.currentSceneType == EnumSceneType.MAINMENU;
		BasicUnit unit = Player.inst.unit;
		this.spr_Sword.color = unit.mainColor;
		this.bloom.InitMat(this.spr_Sword, ResourceLibrary.Inst.GlowIntensity_Unit, false);
		Factor factor = unit.playerFactorTotal;
		if (flag)
		{
			factor = TempData.inst.playerPreview.TotalFactor;
		}
		float num = Mathf.Sqrt(factor.bulletRng);
		this.sword_Length = num * this.GetFloat_LengthMulti();
		this.basicSize = 1f;
		float num2 = Mathf.Sqrt(this.basicSize);
		this.sword_Width = num2 * this.GetFloat_WidthMulti() * this.widthFac;
		float num3 = this.sword_Length / this.sword_Width * 3f / this.lengthSizeFlag;
		this.spr_Sword.size = new Vector2(num3, this.sword_Width);
		this.spr_Sword.transform.localScale = new Vector2(this.sword_Length / num3, 1f);
		base.transform.localScale = new Vector2(1f, 1f);
		this.box2D.size = new Vector2(this.sword_Length, this.GetFloat_ColliderWidth());
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000138C4 File Offset: 0x00011AC4
	private float GetFloat_ColliderWidth()
	{
		if (this.swordState != SpecialBullet_Sword.EnumSwordState.INHAND)
		{
			return Mathf.Clamp(this.fly_RotateSpeed * Time.fixedDeltaTime / 180f * 3.1415927f * this.sword_Length / 2f, this.sword_Width * 3f, this.sword_Length + this.sword_Width);
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			return Mathf.Clamp(SkillModule.GetSkillModule_CurrentJobWithEffectID(0).facs[1] * this.source.playerFactorTotal.bulletSpd * Time.fixedDeltaTime / 180f * 3.1415927f * this.sword_Length, this.sword_Width * 3f, this.sword_Length * 2f + this.sword_Width);
		}
		return this.sword_Width * 3f;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x00013994 File Offset: 0x00011B94
	private float GetFloat_LengthMulti()
	{
		float num = 1f;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs;
			int splitUpgradeCount = Battle.inst.GetSplitUpgradeCount();
			num *= Mathf.Pow(facs[2], (float)splitUpgradeCount);
		}
		return num;
	}

	// Token: 0x0600032F RID: 815 RVA: 0x000139DC File Offset: 0x00011BDC
	private float GetFloat_WidthMulti()
	{
		float num = 1f;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(1))
		{
			num *= SkillModule.GetSkillModule_CurrentJobWithEffectID(1).facs[1];
		}
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs;
			int splitUpgradeCount = Battle.inst.GetSplitUpgradeCount();
			num *= Mathf.Pow(facs[1], (float)splitUpgradeCount);
		}
		return num;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00013A40 File Offset: 0x00011C40
	private float GetFloat_DamageMulti()
	{
		float num = 1f;
		if (TempData.inst.GetBool_SkillModuleOpenFlag(4))
		{
			float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(4).facs;
			int splitUpgradeCount = Battle.inst.GetSplitUpgradeCount();
			num *= Mathf.Pow(facs[3], (float)splitUpgradeCount);
		}
		return num;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00013A85 File Offset: 0x00011C85
	protected override void SetType()
	{
		this.bulletType = EnumBulletType.SWORD;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00013A8E File Offset: 0x00011C8E
	protected override void FixedUpdate_Scale()
	{
		this.Common_UpdateSprite();
		if (this.swordState == SpecialBullet_Sword.EnumSwordState.INHAND)
		{
			this.InHand_UpdateLocalTransform();
		}
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00013AA4 File Offset: 0x00011CA4
	protected override void FixedUpdate_SpecialMove()
	{
		if (this.swordMaster == null || this.source == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		this.FixedUpdate_Collider();
		this.FixedUpdate_Flying();
		this.AntiBug_WallDetect();
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00013AE0 File Offset: 0x00011CE0
	protected override void FixedUpdate_Collider()
	{
		if (this.box2D.enabled)
		{
			this.box2D.enabled = false;
		}
		Factor playerFactorTotal = this.source.playerFactorTotal;
		this.basicDamage = playerFactorTotal.bulletDmg * (double)playerFactorTotal.fireSpd * (double)this.colliTimeMax;
		if (MyTool.DecimalToBool(playerFactorTotal.critChc))
		{
			this.basicDamage *= (double)playerFactorTotal.critDmg;
		}
		this.basicDamage *= (double)playerFactorTotal.bulletSize;
		this.basicDamage *= (double)this.GetFloat_DamageMulti();
		this.box2D.enabled = true;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00013B84 File Offset: 0x00011D84
	private void FixedUpdate_Flying()
	{
		switch (this.swordState)
		{
		case SpecialBullet_Sword.EnumSwordState.INHAND:
			return;
		case SpecialBullet_Sword.EnumSwordState.FLYINGGOING:
			if (TempData.inst.GetBool_SkillModuleOpenFlag(2))
			{
				this.FlyCommon_UpdateSpeed();
				base.transform.position += Time.fixedDeltaTime * this.fly_Direction * this.fly_Speed;
			}
			else if (this.fly_Speed != 0f)
			{
				this.FlyCommon_UpdateSpeed();
				Vector2 vector = this.fly_Target - base.transform.position;
				this.fly_Direction = vector.normalized;
				base.transform.position += Time.fixedDeltaTime * this.fly_Direction * this.fly_Speed;
				if (vector.magnitude <= this.fly_Speed * Time.fixedDeltaTime * 0.5f)
				{
					base.transform.position = this.fly_Target;
					this.fly_Speed = 0f;
				}
			}
			break;
		case SpecialBullet_Sword.EnumSwordState.FLYINGBACKING:
		{
			this.fly_Target = this.GetVector2_InHandPosition_World();
			Vector2 vector2 = this.fly_Target - base.transform.position;
			this.fly_Direction = vector2.normalized;
			base.transform.position += this.fly_Direction * (Time.fixedDeltaTime * this.fly_Speed + this.GetFloat_ArcLengthInRotate() * 1.5f);
			if (vector2.magnitude <= this.fly_Speed * Time.fixedDeltaTime * 0.5f + this.GetFloat_ArcLengthInRotate() * 1.5f)
			{
				this.swordState = SpecialBullet_Sword.EnumSwordState.INHAND;
			}
			break;
		}
		}
		if (this.swordState != SpecialBullet_Sword.EnumSwordState.INHAND)
		{
			base.transform.Rotate(0f, 0f, Time.fixedDeltaTime * this.fly_RotateSpeed);
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00013D84 File Offset: 0x00011F84
	public void FlyDefault_FlyGoingToTarget(Vector2 targetPosition)
	{
		this.fly_Target = targetPosition;
		this.swordState = SpecialBullet_Sword.EnumSwordState.FLYINGGOING;
		this.FlyCommon_UpdateSpeed();
		this.fly_Direction = (this.fly_Target - base.transform.position).normalized;
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00013DD0 File Offset: 0x00011FD0
	private void FlyCommon_UpdateSpeed()
	{
		int num = 0;
		Skill skill = Skill.SkillCurrent(ref num);
		this.fly_Speed = this.source.playerFactorTotal.bulletSpd;
		this.fly_RotateSpeed = this.fly_Speed * skill.facs[10];
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00013E13 File Offset: 0x00012013
	public void FlyCommon_FlyBackingToHand()
	{
		this.swordState = SpecialBullet_Sword.EnumSwordState.FLYINGBACKING;
		this.fly_Speed = this.source.playerFactorTotal.bulletSpd * 5f;
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00013E38 File Offset: 0x00012038
	public override float GetFloat_HitBackForce()
	{
		Factor playerFactorTotal = this.source.playerFactorTotal;
		return playerFactorTotal.bulletSpd * playerFactorTotal.bulletSize * playerFactorTotal.repulse * 50f * 0.4f;
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00013E74 File Offset: 0x00012074
	public override Vector2 GetVector2_HitBackDirection(Transform transTarget)
	{
		Vector2 result = Vector2.zero;
		if (this.swordState != SpecialBullet_Sword.EnumSwordState.INHAND || TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			result = (transTarget.position - this.source.transform.position).Rotate(90f).normalized;
		}
		else
		{
			result = (transTarget.position - this.source.transform.position).normalized;
		}
		return result;
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00013EFC File Offset: 0x000120FC
	private float GetFloat_ArcLengthInRotate()
	{
		if (TempData.inst.GetBool_SkillModuleOpenFlag(0))
		{
			return SkillModule.GetSkillModule_CurrentJobWithEffectID(0).facs[1] * this.source.playerFactorTotal.bulletSpd * Time.fixedDeltaTime / 180f * 3.1415927f * this.sword_Length / 2f;
		}
		return 0f;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00013F5C File Offset: 0x0001215C
	public override void AntiBug_WallDetect()
	{
		if (TempData.inst.currentSceneType != EnumSceneType.BATTLE)
		{
			return;
		}
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			return;
		}
		Vector2 vector = base.transform.position;
		float x = vector.x;
		float y = vector.y;
		float num = Mathf.Abs(x);
		float num2 = Mathf.Abs(y);
		float num3 = 22f * SceneObj.inst.SceneSize - this.sword_Length / 2f;
		if (num > num3 || num2 > num3)
		{
			if (Mathf.Abs(x) - num3 > 0f && x * this.fly_Direction.x > 0f)
			{
				Vector2 inNormal = new Vector2(1f, 0f);
				this.fly_Direction = Vector2.Reflect(this.fly_Direction, inNormal);
			}
			if (Mathf.Abs(y) - num3 > 0f && y * this.fly_Direction.y > 0f)
			{
				Vector2 inNormal2 = new Vector2(0f, 1f);
				this.fly_Direction = Vector2.Reflect(this.fly_Direction, inNormal2);
			}
			return;
		}
	}

	// Token: 0x040002DC RID: 732
	public SpecialBullet_Sword.EnumSwordState swordState;

	// Token: 0x040002DD RID: 733
	[SerializeField]
	private SpriteRenderer spr_Sword;

	// Token: 0x040002DE RID: 734
	[SerializeField]
	private float sword_Length;

	// Token: 0x040002DF RID: 735
	[SerializeField]
	private float sword_Width;

	// Token: 0x040002E0 RID: 736
	[SerializeField]
	private float radiusFix;

	// Token: 0x040002E1 RID: 737
	[SerializeField]
	private float angleZ;

	// Token: 0x040002E2 RID: 738
	[SerializeField]
	private float widthFac = 0.6f;

	// Token: 0x040002E3 RID: 739
	[SerializeField]
	private float lengthSizeFlag = 3f;

	// Token: 0x040002E4 RID: 740
	[SerializeField]
	private Player_9_SwordMaster swordMaster;

	// Token: 0x040002E5 RID: 741
	[Header("Collision")]
	[SerializeField]
	private BoxCollider2D box2D;

	// Token: 0x040002E6 RID: 742
	[SerializeField]
	private float colliTimeMax = 0.1f;

	// Token: 0x040002E7 RID: 743
	[Header("Flying")]
	[SerializeField]
	private Vector2 fly_Target = Vector2.zero;

	// Token: 0x040002E8 RID: 744
	[SerializeField]
	private Vector2 fly_Direction = Vector2.zero;

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	private float fly_Speed;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	private float fly_RotateSpeed = 720f;

	// Token: 0x0200014E RID: 334
	public enum EnumSwordState
	{
		// Token: 0x040009BA RID: 2490
		INHAND,
		// Token: 0x040009BB RID: 2491
		FLYINGGOING,
		// Token: 0x040009BC RID: 2492
		FLYINGBACKING
	}
}
