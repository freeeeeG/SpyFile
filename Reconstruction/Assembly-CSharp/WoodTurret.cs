using System;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class WoodTurret : ElementTurret
{
	// Token: 0x06000C9D RID: 3229 RVA: 0x00020BD4 File Offset: 0x0001EDD4
	public override bool GameUpdate()
	{
		if (this.Activated)
		{
			if (this.targetList.Count == 0)
			{
				if (this.isPlayingAudio)
				{
					this.turretAnim.SetBool("Attacking", false);
					this.ShootEffect.Stop();
					this.isPlayingAudio = false;
					this.audioSource.Stop();
				}
			}
			else if (!this.isPlayingAudio)
			{
				this.ShootEffect.Play();
				this.turretAnim.SetBool("Attacking", true);
				this.isPlayingAudio = true;
				this.PlayAudio(this.ShootClip, true);
			}
		}
		else if (this.isPlayingAudio)
		{
			this.turretAnim.SetBool("Attacking", false);
			this.ShootEffect.Stop();
			this.isPlayingAudio = false;
			this.audioSource.Stop();
		}
		return base.GameUpdate();
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00020CAC File Offset: 0x0001EEAC
	protected override void Shoot()
	{
		Bullet component = Singleton<ObjectPool>.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
		float d = Random.Range(-0.02f, 0.02f);
		component.transform.position = this.shootPoint.position + d * this.shootPoint.right;
		Vector2 vector = component.transform.position - base.transform.position;
		Vector2 value = this.shootPoint.position + vector.normalized * ((float)this.Strategy.FinalRange + 0.25f);
		component.Initialize(this, base.Target[0], new Vector2?(value));
	}

	// Token: 0x0400063A RID: 1594
	private bool isPlayingAudio;
}
