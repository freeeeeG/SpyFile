using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class SpecialBullet_Mine : Bullet
{
	// Token: 0x06000888 RID: 2184 RVA: 0x0003172C File Offset: 0x0002F92C
	protected override void UpdateProps()
	{
		this.TotalSize = this.basicSize * this.sizeMod_SuperMine;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00031744 File Offset: 0x0002F944
	public override void EndLife(bool ifNormal)
	{
		if (Player.inst != null && ifNormal)
		{
			BasicUnit unit = Player.inst.unit;
			SpecialEffects.NewExplosionWave(base.transform.position, 6f * base.TransScale, this.mainColor, (double)this.blastDamageMulti * unit.playerFactorTotal.bulletDmg * unit.playerFactorTotal.bulletDmg, 0f, 0f, unit.shapeType, false, true, this.ifCrit);
		}
		base.EndLife(ifNormal);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x000317D1 File Offset: 0x0002F9D1
	private void FixedUpdate()
	{
		this.FixedUpdate_SpecialMove();
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x000317DC File Offset: 0x0002F9DC
	protected override void FixedUpdate_SpecialMove()
	{
		int[] specialEffect = Battle.inst.specialEffect;
		if (BattleManager.inst.timeStage != EnumTimeStage.REST)
		{
			if (specialEffect[27] >= 1)
			{
				this.sizeMod_SuperMine += Time.fixedDeltaTime * 0.15f;
			}
			if (specialEffect[28] >= 1)
			{
				this.recycle_Time += Time.fixedDeltaTime;
				if (this.recycle_Time >= 9f)
				{
					Player.inst.transform.position = base.transform.position;
					Player.inst.unit.LifeAdd(1.0, true);
					BattleManager.inst.listMines.Remove(this);
					this.EndLife(true);
				}
			}
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00031896 File Offset: 0x0002FA96
	protected override void SetType()
	{
		this.bulletType = EnumBulletType.MINE;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x000318A0 File Offset: 0x0002FAA0
	protected override void SpecialInit()
	{
		BattleManager.inst.listMines.Add(this);
		base.UpdateDirection(Random.insideUnitCircle);
		this.basicSize = Player.inst.unit.lastSize;
		base.TransScale = Mathf.Sqrt(this.basicSize);
		this.blastDamageMulti = 1f;
		if (Battle.inst.specialEffect[24] >= 1)
		{
			this.basicSize *= 2f;
			this.blastDamageMulti *= 2f;
		}
		if (Battle.inst.specialEffect[61] >= 1)
		{
			this.blastDamageMulti *= 3f;
		}
		this.recycle_Time = 0f;
		this.sizeMod_SuperMine = 1f;
		this.basicSpeed = 0f;
		this.basicDamage = 0.0;
		this.spr_Body.sprite = this.spr;
		this.spriteRendererStar.color = this.mainColor;
		SpriteRenderer[] sprs = new SpriteRenderer[]
		{
			this.spr_Body,
			this.spriteRendererStar
		};
		this.bloom.InitMat(sprs, ResourceLibrary.Inst.GlowIntensity_Bullet, true);
		this.upgrade_BounceEnemy = 0;
		this.upgrade_BounceWall = 100;
		base.StartCoroutine(this.AntiBug_Position_ForFrames(3));
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000319EE File Offset: 0x0002FBEE
	private IEnumerator AntiBug_Position_ForFrames(int frames)
	{
		int num;
		for (int i = 0; i < frames; i = num + 1)
		{
			this.AntiBug_Position();
			yield return 0;
			num = i;
		}
		yield break;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00031A04 File Offset: 0x0002FC04
	private void AntiBug_Position()
	{
		float num = base.transform.position.x;
		float num2 = base.transform.position.y;
		bool flag = false;
		if (Mathf.Abs(num) > 22.2f)
		{
			flag = true;
			num = Mathf.Clamp(num, -22.2f, 22.2f);
		}
		if (Mathf.Abs(num2) > 22.2f)
		{
			flag = true;
			num2 = Mathf.Clamp(num2, -22.2f, 22.2f);
		}
		if (flag)
		{
			base.transform.position = new Vector2(num, num2);
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00031A8F File Offset: 0x0002FC8F
	public void ForceExplode(float dmg)
	{
		this.blastDamageMulti *= dmg;
		BattleManager.inst.listMines.Remove(this);
		this.EndLife(true);
	}

	// Token: 0x0400070A RID: 1802
	[SerializeField]
	private Sprite spr;

	// Token: 0x0400070B RID: 1803
	[SerializeField]
	private SpriteRenderer spriteRendererStar;

	// Token: 0x0400070C RID: 1804
	[SerializeField]
	private float blastDamageMulti = 1f;

	// Token: 0x0400070D RID: 1805
	[SerializeField]
	private float sizeMod_SuperMine = 1f;

	// Token: 0x0400070E RID: 1806
	[SerializeField]
	private float recycle_Time;
}
