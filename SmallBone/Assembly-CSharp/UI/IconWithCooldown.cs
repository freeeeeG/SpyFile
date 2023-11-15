using System;
using System.Collections;
using Characters.Cooldowns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003A9 RID: 937
	public class IconWithCooldown : MonoBehaviour
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0003318D File Offset: 0x0003138D
		public Image icon
		{
			get
			{
				return this._icon;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x00033195 File Offset: 0x00031395
		// (set) Token: 0x06001144 RID: 4420 RVA: 0x000331A0 File Offset: 0x000313A0
		public CooldownSerializer cooldown
		{
			get
			{
				return this._cooldown;
			}
			set
			{
				if (this._effect != null && this._cooldown != value)
				{
					if (this._cooldown != null)
					{
						this._cooldown.onReady -= this.SpawnEffect;
					}
					if (value != null)
					{
						value.onReady += this.SpawnEffect;
					}
				}
				this._cooldown = value;
			}
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00033200 File Offset: 0x00031400
		private void Awake()
		{
			this._effect.gameObject.SetActive(true);
			this._effectLength = this._effect.GetCurrentAnimatorStateInfo(0).length;
			this._effect.gameObject.SetActive(false);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x0003324C File Offset: 0x0003144C
		protected virtual void Update()
		{
			if (this.cooldown == null)
			{
				return;
			}
			this._cooldownMask.fillAmount = this.cooldown.remainPercent;
			if (this.cooldown.type == CooldownSerializer.Type.Time)
			{
				if (this._remainStreaks != null)
				{
					if (this.cooldown.streak.remains > 0)
					{
						this._remainStreaks.text = this.cooldown.streak.remains.ToString();
						this._streakMask.fillAmount = this.cooldown.time.streak.remainPercent;
						return;
					}
					if (this.cooldown.stacks > 1)
					{
						this._remainStreaks.text = this.cooldown.stacks.ToString();
						this._streakMask.fillAmount = 0f;
						return;
					}
					this._remainStreaks.text = string.Empty;
					this._streakMask.fillAmount = 0f;
					return;
				}
			}
			else
			{
				this._remainStreaks.text = string.Empty;
				this._streakMask.fillAmount = 0f;
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0003336F File Offset: 0x0003156F
		private void OnDisable()
		{
			this._effect.gameObject.SetActive(false);
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00033382 File Offset: 0x00031582
		private void SpawnEffect()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this._cPlayEffectReference.Stop();
			this._cPlayEffectReference = this.StartCoroutineWithReference(this.CPlayEffect());
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x000333AA File Offset: 0x000315AA
		private IEnumerator CPlayEffect()
		{
			this._effect.gameObject.SetActive(true);
			this._effect.Play(0, 0, 0f);
			yield return Chronometer.global.WaitForSeconds(this._effectLength);
			this._effect.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000E36 RID: 3638
		[SerializeField]
		private Image _icon;

		// Token: 0x04000E37 RID: 3639
		[SerializeField]
		private Image _cooldownMask;

		// Token: 0x04000E38 RID: 3640
		[SerializeField]
		private Image _streakMask;

		// Token: 0x04000E39 RID: 3641
		[SerializeField]
		private TMP_Text _remainStreaks;

		// Token: 0x04000E3A RID: 3642
		[SerializeField]
		private Animator _effect;

		// Token: 0x04000E3B RID: 3643
		private float _effectLength;

		// Token: 0x04000E3C RID: 3644
		private CooldownSerializer _cooldown;

		// Token: 0x04000E3D RID: 3645
		private CoroutineReference _cPlayEffectReference;
	}
}
