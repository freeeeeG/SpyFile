using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x02000065 RID: 101
	public class PreventDamage : MonoBehaviour
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00016EEB File Offset: 0x000150EB
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x00016EF3 File Offset: 0x000150F3
		public bool isActive { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00016EFC File Offset: 0x000150FC
		public float finalCooldown
		{
			get
			{
				return this.cooldownRate.ModifyInverse(this.cooldownTime);
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00016F0F File Offset: 0x0001510F
		private void OnPreventDamage()
		{
			if (this._waitCooldownCoroutine == null)
			{
				this._waitCooldownCoroutine = this.InvincibilityCooldownCR();
				base.StartCoroutine(this._waitCooldownCoroutine);
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00016F34 File Offset: 0x00015134
		private void Start()
		{
			this.cooldownRate = new StatMod();
			this._waitCooldownCoroutine = null;
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.playerHealth = componentInParent.playerHealth;
			this.playerHealth.onDamagePrevented.AddListener(new UnityAction(this.OnPreventDamage));
			this.ApplyInvincibility();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00016F88 File Offset: 0x00015188
		private void OnDestroy()
		{
			this.playerHealth.onDamagePrevented.RemoveListener(new UnityAction(this.OnPreventDamage));
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00016FA6 File Offset: 0x000151A6
		public void ReduceCDTimer(float reduceAmount)
		{
			if (!this.isActive)
			{
				this._timer += reduceAmount;
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00016FBE File Offset: 0x000151BE
		private void ApplyInvincibility()
		{
			this.playerHealth.isProtected = true;
			this.isActive = true;
			this.OnCooldownDone.Invoke();
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00016FDE File Offset: 0x000151DE
		private IEnumerator InvincibilityCooldownCR()
		{
			this.OnDamagePrevented.Invoke();
			this.playerHealth.isInvincible.Flip();
			yield return new WaitForSeconds(0.5f);
			this.playerHealth.isInvincible.UnFlip();
			this.isActive = false;
			this._timer = 0f;
			while (this._timer < this.finalCooldown)
			{
				this._timer += Time.deltaTime;
				yield return null;
			}
			this.ApplyInvincibility();
			this._waitCooldownCoroutine = null;
			yield break;
		}

		// Token: 0x04000271 RID: 625
		public float cooldownTime;

		// Token: 0x04000272 RID: 626
		public StatMod cooldownRate;

		// Token: 0x04000273 RID: 627
		public UnityEvent OnDamagePrevented;

		// Token: 0x04000274 RID: 628
		public UnityEvent OnCooldownDone;

		// Token: 0x04000275 RID: 629
		private PlayerHealth playerHealth;

		// Token: 0x04000276 RID: 630
		private IEnumerator _waitCooldownCoroutine;

		// Token: 0x04000277 RID: 631
		private float _timer;
	}
}
