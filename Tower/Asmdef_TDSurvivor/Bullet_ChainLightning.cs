using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class Bullet_ChainLightning : AProjectile
{
	// Token: 0x06000180 RID: 384 RVA: 0x0000690D File Offset: 0x00004B0D
	private void LateUpdate()
	{
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000690F File Offset: 0x00004B0F
	public void Setup(int damage, int targetCount, float jumpRange, Transform startTransform)
	{
		this.damage = damage;
		this.targetCount = targetCount;
		this.jumpRange = jumpRange;
		this.startTransform = startTransform;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000692E File Offset: 0x00004B2E
	protected override void SpawnProc()
	{
		this.lineRenderer.positionCount = 0;
		base.StartCoroutine(this.CR_ChainLightningProc(this.targetCount, this.targetMonster, this.jumpRange));
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000695B File Offset: 0x00004B5B
	protected override void DespawnProc()
	{
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000695D File Offset: 0x00004B5D
	protected override void DestroyProc()
	{
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000695F File Offset: 0x00004B5F
	private IEnumerator CR_ChainLightningProc(int targetCount, AMonsterBase firstTarget, float jumpRange)
	{
		List<AMonsterBase> list_Targets = new List<AMonsterBase>();
		list_Targets.Add(firstTarget);
		for (int j = 0; j < targetCount - 1; j++)
		{
			List<AMonsterBase> monstersInRange = Singleton<MonsterManager>.Instance.GetMonstersInRange(firstTarget.transform.position, jumpRange);
			for (int k = monstersInRange.Count - 1; k >= 0; k--)
			{
				if (list_Targets.Contains(monstersInRange[k]))
				{
					monstersInRange.RemoveAt(k);
				}
			}
			if (monstersInRange.Count == 0)
			{
				break;
			}
			AMonsterBase item = monstersInRange.RandomItem<AMonsterBase>();
			list_Targets.Add(item);
		}
		Debug.DrawLine(base.transform.position, list_Targets[0].HeadWorldPosition, Color.cyan, 2f);
		for (int l = 0; l < list_Targets.Count - 1; l++)
		{
			Debug.DrawLine(list_Targets[l].HeadWorldPosition, list_Targets[l + 1].HeadWorldPosition, Color.cyan, 2f);
		}
		Vector3 position = base.transform.position;
		Vector3 zero = Vector3.zero;
		int num;
		for (int i = 0; i < list_Targets.Count; i = num + 1)
		{
			Vector3 headWorldPosition = list_Targets[i].HeadWorldPosition;
			float timer = 0f;
			while (timer < this.jumpTargetInterval)
			{
				this.UpdateLineRenderer(list_Targets, i);
				timer += Time.deltaTime;
				yield return null;
			}
			list_Targets[i].Hit(this.damage, eDamageType.ELECTRIC, default(Vector3));
			this.particle_HitEffect.transform.position = list_Targets[i].HeadWorldPosition;
			this.particle_HitEffect.Emit(1);
			base.OnHit(list_Targets[i]);
			num = i;
		}
		float endingTimer = 0f;
		while (endingTimer < this.jumpTargetInterval)
		{
			this.UpdateLineRenderer(list_Targets, list_Targets.Count);
			endingTimer += Time.deltaTime;
			yield return null;
		}
		this.Despawn();
		yield break;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00006984 File Offset: 0x00004B84
	private void UpdateLineRenderer(List<AMonsterBase> list_Targets, int hitCount)
	{
		if (this.startTransform == null)
		{
			this.startTransform = base.transform;
		}
		this.lineRenderer.positionCount = hitCount + 1;
		this.lineRenderer.SetPosition(0, this.startTransform.position);
		for (int i = 0; i < hitCount; i++)
		{
			this.lineRenderer.SetPosition(i + 1, list_Targets[i].HeadWorldPosition);
		}
	}

	// Token: 0x0400010B RID: 267
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	private float jumpTargetInterval = 0.1f;

	// Token: 0x0400010D RID: 269
	[SerializeField]
	private ParticleSystem particle_HitEffect;

	// Token: 0x0400010E RID: 270
	private int damage;

	// Token: 0x0400010F RID: 271
	private int targetCount;

	// Token: 0x04000110 RID: 272
	private float jumpRange = 1f;

	// Token: 0x04000111 RID: 273
	private Transform startTransform;
}
