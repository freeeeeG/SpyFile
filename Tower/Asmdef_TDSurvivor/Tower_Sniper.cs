using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class Tower_Sniper : ABaseTower
{
	// Token: 0x0600076D RID: 1901 RVA: 0x0001BF30 File Offset: 0x0001A130
	private void Start()
	{
		this.shootTimer = 0f;
		this.lineRenderer.startWidth = 0f;
		this.lineRenderer.endWidth = 0f;
		this.lineRenderer.enabled = false;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0001BF6C File Offset: 0x0001A16C
	private void Update()
	{
		if (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		if (this.shootTimer <= 0f)
		{
			if (this.currentTarget == null || !this.currentTarget.IsAttackable())
			{
				this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
			}
			if (this.currentTarget != null && this.currentTarget.IsAttackable())
			{
				this.shootTimer = this.settingData.GetShootInterval(1f);
				base.Shoot();
				this.timeAfterShoot = 0f;
				this.spin_Cog.rotationsPerSecond = this.cogSpinSpeed_Fast;
			}
			else
			{
				this.shootTimer = 0.05f;
				this.spin_Cog.rotationsPerSecond = Vector3.Lerp(this.spin_Cog.rotationsPerSecond, this.cogSpinSpeed_Normal, Time.unscaledDeltaTime);
			}
		}
		else
		{
			if (this.timeAfterShoot > 0.5f && (this.currentTarget == null || !this.currentTarget.IsAttackable()))
			{
				this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
			}
			this.shootTimer -= Time.deltaTime;
			this.spin_Cog.rotationsPerSecond = Vector3.Lerp(this.spin_Cog.rotationsPerSecond, this.cogSpinSpeed_Normal, Time.unscaledDeltaTime);
		}
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.node_HeadAimingRotation.forward = Vector3.Lerp(this.node_HeadAimingRotation.forward, this.currentTarget.HeadWorldPosition - base.ShootWorldPosition, Time.unscaledDeltaTime * 10f);
			this.headModelForward = this.currentTarget.HeadWorldPosition - this.node_CannonHeadModel.position;
			this.headModelForward.y = 0f;
			this.node_CannonHeadModel.forward = this.headModelForward;
		}
		this.timeAfterShoot += Time.deltaTime;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0001C1AF File Offset: 0x0001A3AF
	protected override void ShootProc()
	{
		base.StartCoroutine(this.CR_ShootEffect());
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1009", -1f, -1f, -1f);
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0001C1ED File Offset: 0x0001A3ED
	private IEnumerator CR_ShootEffect()
	{
		this.lineRenderer.startWidth = 0f;
		this.lineRenderer.endWidth = 0f;
		this.lineRenderer.enabled = true;
		this.UpdateLinePosition();
		float time = 0f;
		float duration = this.shootEffectDuration;
		this.bulletIndex++;
		base.BulletHitCallback(this.currentTarget, this.shootIndex, this.bulletIndex);
		int damage = this.settingData.GetDamage(1f);
		this.currentTarget.Hit(damage, eDamageType.FIRE, default(Vector3));
		this.particle_HitEffect.transform.position = this.currentTarget.HeadWorldPosition;
		this.particle_HitEffect.Play();
		while (time <= duration)
		{
			float num = this.curve_ShootEffectSize.Evaluate(time / duration);
			this.lineRenderer.startWidth = num * this.lineRendererMaxWidth;
			this.lineRenderer.endWidth = num * this.lineRendererMaxWidth;
			this.UpdateLinePosition();
			yield return null;
			time += Time.deltaTime;
		}
		this.lineRenderer.startWidth = 0f;
		this.lineRenderer.endWidth = 0f;
		this.lineRenderer.enabled = false;
		yield break;
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0001C1FC File Offset: 0x0001A3FC
	private void UpdateLinePosition()
	{
		if (this.currentTarget != null)
		{
			this.lineRenderer.SetPosition(0, Vector3.zero);
			this.lineRenderer.SetPosition(1, this.lineRenderer.transform.InverseTransformPoint(this.currentTarget.HeadWorldPosition));
			this.lastHitPosition = this.currentTarget.HeadWorldPosition;
			return;
		}
		this.lineRenderer.SetPosition(0, Vector3.zero);
		this.lineRenderer.SetPosition(1, this.lineRenderer.transform.InverseTransformPoint(this.lastHitPosition));
	}

	// Token: 0x04000600 RID: 1536
	private Vector3 headModelForward;

	// Token: 0x04000601 RID: 1537
	[SerializeField]
	private Transform node_HeadAimingRotation;

	// Token: 0x04000602 RID: 1538
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x04000603 RID: 1539
	[SerializeField]
	private ParticleSystem particle_HitEffect;

	// Token: 0x04000604 RID: 1540
	[SerializeField]
	private float shootEffectDuration = 1f;

	// Token: 0x04000605 RID: 1541
	[SerializeField]
	private float lineRendererMaxWidth = 1f;

	// Token: 0x04000606 RID: 1542
	[SerializeField]
	private AnimationCurve curve_ShootEffectSize;

	// Token: 0x04000607 RID: 1543
	[SerializeField]
	private Spin spin_Cog;

	// Token: 0x04000608 RID: 1544
	[SerializeField]
	private Vector3 cogSpinSpeed_Normal;

	// Token: 0x04000609 RID: 1545
	[SerializeField]
	private Vector3 cogSpinSpeed_Fast;

	// Token: 0x0400060A RID: 1546
	private float timeAfterShoot;

	// Token: 0x0400060B RID: 1547
	private Vector3 lastHitPosition = Vector3.zero;
}
