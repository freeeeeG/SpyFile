using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class LaserBullet : LineBullet
{
	// Token: 0x06000D22 RID: 3362 RVA: 0x00021D7E File Offset: 0x0001FF7E
	public void SetTravelIncrease(bool increase, float increaseValue)
	{
		this.travelIncrease = increase;
		this.increaseValue = increaseValue;
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x00021D8E File Offset: 0x0001FF8E
	public override void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
	{
		base.Initialize(turret, target, pos);
		this.distanceCounter = 0f;
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x00021DA4 File Offset: 0x0001FFA4
	public override bool GameUpdate()
	{
		this.IncreaseDmgWhileTravel();
		return base.GameUpdate();
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x00021DB4 File Offset: 0x0001FFB4
	private void IncreaseDmgWhileTravel()
	{
		if (!this.travelIncrease)
		{
			return;
		}
		if (Vector2.Distance(base.transform.position, this.turretParent.transform.position) - this.distanceCounter >= 1f)
		{
			base.AttackIntensify += this.increaseValue;
			this.distanceCounter += 1f;
		}
	}

	// Token: 0x0400065C RID: 1628
	private float distanceCounter;

	// Token: 0x0400065D RID: 1629
	private bool travelIncrease;

	// Token: 0x0400065E RID: 1630
	private float increaseValue = 0.4f;
}
