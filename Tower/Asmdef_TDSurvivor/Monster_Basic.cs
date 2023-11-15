using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000BA RID: 186
public class Monster_Basic : AMonsterBase
{
	// Token: 0x06000438 RID: 1080 RVA: 0x00010993 File Offset: 0x0000EB93
	protected void Start()
	{
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00010995 File Offset: 0x0000EB95
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x000109A0 File Offset: 0x0000EBA0
	protected override void SpawnProc()
	{
		if (this.monsterData.GetMonsterSize() == eMonsterSize.SMALL || this.monsterData.GetMonsterSize() == eMonsterSize.MEDIUM)
		{
			this.animator.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
		}
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00010A00 File Offset: 0x0000EC00
	protected override void DespawnProc()
	{
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00010A02 File Offset: 0x0000EC02
	protected override void HitProc(int damage)
	{
		this.animator.SetTrigger("Hit");
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00010A14 File Offset: 0x0000EC14
	protected override IEnumerator DeathProc(int damage, bool playAnimation = true)
	{
		if (playAnimation)
		{
			this.animator.SetTrigger("Die");
			yield return new WaitForSeconds(this.deadAnimationLength);
		}
		yield break;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00010A2A File Offset: 0x0000EC2A
	protected override void UpdateProc(float deltaTime)
	{
	}
}
