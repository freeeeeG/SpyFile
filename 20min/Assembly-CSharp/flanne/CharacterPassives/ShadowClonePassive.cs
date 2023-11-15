using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.CharacterPassives
{
	// Token: 0x0200025F RID: 607
	public class ShadowClonePassive : SkillPassive
	{
		// Token: 0x06000D24 RID: 3364 RVA: 0x0002FFAC File Offset: 0x0002E1AC
		protected override void Init()
		{
			this.player = base.transform.root.GetComponent<PlayerController>();
			this.spriteTrail.mLeadingSprite = this.player.playerSprite;
			this.shadowClone.SetActive(false);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0002FFE6 File Offset: 0x0002E1E6
		protected override void PerformSkill()
		{
			base.StartCoroutine(this.DashCR(this.player));
			this.SummonShadowClone();
			UnityEvent unityEvent = this.onUse;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00030014 File Offset: 0x0002E214
		private void SummonShadowClone()
		{
			if (this.shadowClone == null)
			{
				return;
			}
			this.shadowClone.transform.position = this.player.transform.position;
			this.shadowClone.SetActive(true);
			this.shadowCloneLifetime.Refresh();
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00030067 File Offset: 0x0002E267
		private IEnumerator DashCR(PlayerController player)
		{
			player.disableAction.Flip();
			player.playerHealth.isInvincible.Flip();
			float originalMoveSpeed = player.movementSpeed;
			player.movementSpeed *= this.dashSpeedMulti;
			SpriteTrail spriteTrail = this.spriteTrail;
			if (spriteTrail != null)
			{
				spriteTrail.SetEnabled(true);
			}
			yield return new WaitForSeconds(this.dashDuration);
			player.movementSpeed = originalMoveSpeed;
			SpriteTrail spriteTrail2 = this.spriteTrail;
			if (spriteTrail2 != null)
			{
				spriteTrail2.SetEnabled(false);
			}
			player.disableAction.UnFlip();
			UnityEvent unityEvent = this.onEnd;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			yield return new WaitForSeconds(0.3f);
			player.playerHealth.isInvincible.UnFlip();
			yield break;
		}

		// Token: 0x04000983 RID: 2435
		[SerializeField]
		private float dashSpeedMulti;

		// Token: 0x04000984 RID: 2436
		[SerializeField]
		private float dashDuration;

		// Token: 0x04000985 RID: 2437
		[SerializeField]
		private GameObject shadowClone;

		// Token: 0x04000986 RID: 2438
		[SerializeField]
		private TimeToLive shadowCloneLifetime;

		// Token: 0x04000987 RID: 2439
		[SerializeField]
		private SpriteTrail spriteTrail;

		// Token: 0x04000988 RID: 2440
		public UnityEvent onUse;

		// Token: 0x04000989 RID: 2441
		public UnityEvent onEnd;

		// Token: 0x0400098A RID: 2442
		private PlayerController player;
	}
}
