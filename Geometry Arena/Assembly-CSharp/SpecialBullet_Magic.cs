using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class SpecialBullet_Magic : Bullet
{
	// Token: 0x06000881 RID: 2177 RVA: 0x000314E4 File Offset: 0x0002F6E4
	protected override void UpdateProps()
	{
		base.UpdateProps();
		this.TotalDamage *= (double)this.charging[0];
		this.TotalSpeed *= this.charging[1];
		this.TotalSize *= this.charging[2];
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00031538 File Offset: 0x0002F738
	public override void AfterInit()
	{
		this.charging = new float[]
		{
			1f,
			1f,
			1f
		};
		this.radius = (Player.inst.transform.position - base.transform.position).magnitude;
		if (this.radius == 0f)
		{
			Debug.LogError("radius==0!");
		}
		this.deltaPosToPlayer = base.transform.position - Player.inst.transform.position;
		base.AfterInit();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x000315D0 File Offset: 0x0002F7D0
	protected override void FixedUpdate_Move()
	{
		if (Player.inst == null)
		{
			return;
		}
		this.deltaPosToPlayer += Time.fixedDeltaTime * this.Velocity;
		base.transform.position = Player.inst.transform.position + this.deltaPosToPlayer;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00031638 File Offset: 0x0002F838
	protected override void SpecialInFixedUpdate()
	{
		this.angleSpd = 360f * this.TotalSpeed / (6.2831855f * this.radius);
		base.UpdateDirection(this.direction.Rotate(this.angleSpd * Time.fixedDeltaTime));
		this.MagicCharging_InFixedUpdate();
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00031688 File Offset: 0x0002F888
	private void MagicCharging_InFixedUpdate()
	{
		if (!TempData.inst.GetBool_SkillModuleOpenFlag(2))
		{
			return;
		}
		float[] facs = SkillModule.GetSkillModule_CurrentJobWithEffectID(2).facs;
		for (int i = 0; i < 3; i++)
		{
			this.charging[i] += Time.fixedDeltaTime * facs[3 + i];
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x000316D6 File Offset: 0x0002F8D6
	protected override void Special_Liangzi()
	{
		this.deltaPosToPlayer = base.transform.position - Player.inst.transform.position;
	}

	// Token: 0x04000706 RID: 1798
	[SerializeField]
	private float radius;

	// Token: 0x04000707 RID: 1799
	[SerializeField]
	private float angleSpd;

	// Token: 0x04000708 RID: 1800
	[SerializeField]
	private Vector2 deltaPosToPlayer = Vector2.zero;

	// Token: 0x04000709 RID: 1801
	[SerializeField]
	private float[] charging = new float[]
	{
		1f,
		1f,
		1f
	};
}
