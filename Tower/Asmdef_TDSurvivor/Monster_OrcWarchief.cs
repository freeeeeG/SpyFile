using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000BC RID: 188
public class Monster_OrcWarchief : Monster_Basic
{
	// Token: 0x0600044A RID: 1098 RVA: 0x00010BFF File Offset: 0x0000EDFF
	protected override void SpawnProc()
	{
		base.SpawnProc();
		this.skillTimer = this.skillInterval;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00010C13 File Offset: 0x0000EE13
	protected override void UpdateProc(float deltaTime)
	{
		base.UpdateProc(deltaTime);
		if (!base.IsAttackable())
		{
			return;
		}
		this.skillTimer -= deltaTime;
		if (this.skillTimer <= 0f)
		{
			this.skillTimer = this.skillInterval;
			this.Skill_SpeedUpSurroundingMonster();
		}
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00010C52 File Offset: 0x0000EE52
	private void Skill_SpeedUpSurroundingMonster()
	{
		base.StartCoroutine(this.CR_Cast());
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00010C61 File Offset: 0x0000EE61
	private IEnumerator CR_Cast()
	{
		this.animator.SetTrigger("Cast");
		this.aiPath.canMove = false;
		yield return new WaitForSeconds(0.33f);
		if (!base.IsAttackable())
		{
			yield break;
		}
		this.particle_Roar.Play();
		SoundManager.PlaySound("Monster", "Monster_005_BattleRoar", -1f, -1f, -1f);
		foreach (AMonsterBase amonsterBase in Singleton<MonsterManager>.Instance.GetMonstersInRange(base.transform.position, this.skillRange))
		{
			if (!(amonsterBase == this))
			{
				amonsterBase.ApplySpeedModifier(this.skillSpeedModifier, this.skillSpeedModifierDuration, false);
			}
		}
		yield return new WaitForSeconds(1f);
		if (!base.IsAttackable())
		{
			yield break;
		}
		this.aiPath.canMove = true;
		yield break;
	}

	// Token: 0x04000432 RID: 1074
	[SerializeField]
	private float skillInterval = 10f;

	// Token: 0x04000433 RID: 1075
	[SerializeField]
	private float skillRange = 5f;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private float skillSpeedModifier = 2f;

	// Token: 0x04000435 RID: 1077
	[SerializeField]
	private ParticleSystem particle_Roar;

	// Token: 0x04000436 RID: 1078
	[SerializeField]
	private float skillSpeedModifierDuration = 1f;

	// Token: 0x04000437 RID: 1079
	[SerializeField]
	private float skillTimer;
}
