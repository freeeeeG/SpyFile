using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
	// Token: 0x02000391 RID: 913
	public class CurrencyDisplay : MonoBehaviour
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x00031652 File Offset: 0x0002F852
		private void Awake()
		{
			this._balanceCache = GameData.Currency.currencies[this._type].balance;
			this._text.text = this._balanceCache.ToString();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00031685 File Offset: 0x0002F885
		private void OnEnable()
		{
			this._animator.enabled = false;
			this._animator.enabled = true;
			this._animator.Play(CurrencyDisplay._idleAnimationHash);
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x000316B0 File Offset: 0x0002F8B0
		private void Update()
		{
			int balance = GameData.Currency.currencies[this._type].balance;
			if (this._balanceCache == balance)
			{
				return;
			}
			this._balanceCache = balance;
			this._text.text = balance.ToString();
			if (this._animator != null)
			{
				this._animator.Play(CurrencyDisplay._earnAnimationHash);
			}
			if (this._effect != null)
			{
				this._effect.Play();
			}
		}

		// Token: 0x04000DB4 RID: 3508
		private static readonly int _earnAnimationHash = Animator.StringToHash("Earn");

		// Token: 0x04000DB5 RID: 3509
		private static readonly int _idleAnimationHash = Animator.StringToHash("Idle");

		// Token: 0x04000DB6 RID: 3510
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04000DB7 RID: 3511
		[SerializeField]
		private TextMeshProUGUI _text;

		// Token: 0x04000DB8 RID: 3512
		[Header("Effects")]
		[SerializeField]
		private CurrencyEffect _effect;

		// Token: 0x04000DB9 RID: 3513
		[SerializeField]
		private Animator _animator;

		// Token: 0x04000DBA RID: 3514
		private int _balanceCache;
	}
}
