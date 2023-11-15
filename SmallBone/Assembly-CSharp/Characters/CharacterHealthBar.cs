using System;
using System.Collections;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006BE RID: 1726
	public class CharacterHealthBar : MonoBehaviour
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x00019108 File Offset: 0x00017308
		// (set) Token: 0x060022AE RID: 8878 RVA: 0x00033467 File Offset: 0x00031667
		public bool visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x00068244 File Offset: 0x00066444
		private void Awake()
		{
			this._defaultHealthScale = this._healthBar.localScale;
			if (this._canHealBar != null)
			{
				this._defaultCanHealScale = this._canHealBar.localScale;
				this._defaultGrayHealthScale = this._grayHealthBar.localScale;
			}
			this._defaultShieldScale = this._shieldBar.localScale;
			this._defaultDamageLerpScale = this._damageLerpBar.localScale;
			this._defaultHealLerpScale = this._healLerpBar.localScale;
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000682C8 File Offset: 0x000664C8
		private void OnEnable()
		{
			this._healthScale.x = 0f;
			this._grayHealthScale.x = 0f;
			this._canHealScale.x = 0f;
			this._shieldScale.x = 0f;
			this._damageLerpScale.x = 0f;
			this._healLerpScale.x = 0f;
			base.StartCoroutine(this.CLerp());
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00068344 File Offset: 0x00066544
		public void Initialize(Character character)
		{
			this._character = character;
			this._health = this._character.health;
			if (!this._alwaysVisible)
			{
				this._container.gameObject.SetActive(false);
				this._remainLifetime = 3f;
				this._health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
				return;
			}
			this._container.gameObject.SetActive(true);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000683B6 File Offset: 0x000665B6
		private void OnDestroy()
		{
			if (this._health != null)
			{
				this._health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000683E0 File Offset: 0x000665E0
		public void SetWidth(float width)
		{
			RectTransform healthBar = this._healthBar;
			RectTransform shieldBar = this._shieldBar;
			RectTransform damageLerpBar = this._damageLerpBar;
			RectTransform healLerpBar = this._healLerpBar;
			Vector2 pivot = new Vector2(0.5f, 0.5f);
			healLerpBar.pivot = pivot;
			healthBar.pivot = (shieldBar.pivot = (damageLerpBar.pivot = pivot));
			if (this._grayHealthBar != null)
			{
				this._grayHealthBar.pivot = new Vector2(0.5f, 0.5f);
				this._canHealBar.pivot = new Vector2(0.5f, 0.5f);
			}
			this._container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			RectTransform healthBar2 = this._healthBar;
			RectTransform shieldBar2 = this._shieldBar;
			RectTransform damageLerpBar2 = this._damageLerpBar;
			RectTransform healLerpBar2 = this._healLerpBar;
			pivot = new Vector2(0f, 0.5f);
			healLerpBar2.pivot = pivot;
			healthBar2.pivot = (shieldBar2.pivot = (damageLerpBar2.pivot = pivot));
			if (this._grayHealthBar != null)
			{
				this._grayHealthBar.pivot = new Vector2(0f, 0.5f);
				this._canHealBar.pivot = new Vector2(0f, 0.5f);
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x00068508 File Offset: 0x00066708
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			this._container.gameObject.SetActive(true);
			this._remainLifetime = 3f;
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x00068528 File Offset: 0x00066728
		private void Update()
		{
			if (this._character == null && this._actionWhenCharacterNull == CharacterHealthBar.ActionWhenCharacterNull.Deactivate)
			{
				this._container.gameObject.SetActive(false);
				return;
			}
			double num;
			double num2;
			double num3;
			double num4;
			double num5;
			if (this._character == null || this._character.health == null || this._character.health.dead)
			{
				num = 0.0;
				num2 = ((this._health == null) ? 0.0 : this._health.maximumHealth);
				num3 = 0.0;
				num4 = 0.0;
				num5 = 0.0;
			}
			else
			{
				num = this._health.currentHealth;
				num2 = this._health.maximumHealth;
				num3 = this._health.grayHealth.maximum;
				num4 = this._health.grayHealth.canHeal;
				num5 = this._health.shield.amount;
			}
			if (num2 == 0.0)
			{
				this._percent = 0f;
				this._percentWithGrayHealth = 0f;
				this._percentWithCanHeal = 0f;
				this._percentWithShield = 0f;
			}
			else if (num3 > 0.0)
			{
				double num6 = num + num3;
				double num7 = num + num4;
				if (num6 + num5 <= num2)
				{
					this._percent = (this._roundUp ? this._health.displayPercent : ((float)this._health.percent));
					this._percentWithGrayHealth = (float)(num6 / num2);
					this._percentWithCanHeal = (float)(num7 / num2);
					this._percentWithShield = (float)((num6 + num5) / num2);
				}
				else
				{
					this._percent = (float)(num / (num6 + num5));
					this._percentWithGrayHealth = (float)(num6 / (num6 + num5));
					this._percentWithCanHeal = (float)(num7 / (num6 + num5));
					this._percentWithShield = 1f;
				}
			}
			else
			{
				this._percentWithGrayHealth = 0f;
				this._percentWithCanHeal = 0f;
				if (num + num5 <= num2)
				{
					this._percent = (this._roundUp ? this._health.displayPercent : ((float)this._health.percent));
					if (this._roundUp)
					{
						this._percentWithShield = Mathf.Round((float)(num + num5)) / Mathf.Round((float)num2);
					}
					else
					{
						this._percentWithShield = (float)((num + num5) / num2);
					}
				}
				else
				{
					this._percent = (float)(num / (num + num5));
					this._percentWithShield = 1f;
				}
			}
			this._healLerpScale.x = this._percentWithShield;
			this._healLerpBar.localScale = Vector3.Scale(this._healLerpScale, this._defaultHealLerpScale);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000687D0 File Offset: 0x000669D0
		private IEnumerator CLerp()
		{
			for (;;)
			{
				if (this._percentWithShield < this._damageLerpScale.x)
				{
					this._damageLerpScale.x = Mathf.Lerp(this._damageLerpScale.x, this._percentWithShield, 0.1f);
				}
				else
				{
					this._damageLerpScale.x = this._shieldScale.x;
				}
				if (this._percentWithShield < this._shieldScale.x)
				{
					this._shieldScale.x = this._percentWithShield;
				}
				else
				{
					this._shieldScale.x = Mathf.Lerp(this._shieldScale.x, this._percentWithShield, 0.1f);
				}
				this._healthScale.x = this._shieldScale.x - (this._percentWithShield - this._percent);
				this._grayHealthScale.x = this._shieldScale.x - (this._percentWithShield - this._percentWithGrayHealth);
				this._canHealScale.x = this._shieldScale.x - (this._percentWithShield - this._percentWithCanHeal);
				if (this._healthScale.x < 0f)
				{
					this._healthScale.x = 0f;
				}
				if (this._canHealScale.x < 0f)
				{
					this._canHealScale.x = 0f;
				}
				if (this._grayHealthScale.x < 0f)
				{
					this._grayHealthScale.x = 0f;
				}
				this._damageLerpBar.localScale = Vector3.Scale(this._damageLerpScale, this._defaultDamageLerpScale);
				this._healthBar.localScale = Vector3.Scale(this._healthScale, this._defaultHealthScale);
				if (this._canHealBar != null)
				{
					this._grayHealthBar.localScale = Vector3.Scale(this._grayHealthScale, this._defaultGrayHealthScale);
					this._canHealBar.localScale = Vector3.Scale(this._canHealScale, this._defaultCanHealScale);
				}
				this._shieldBar.localScale = Vector3.Scale(this._shieldScale, this._defaultShieldScale);
				this._remainLifetime -= Time.deltaTime;
				if (!this._alwaysVisible && this._remainLifetime <= 0f)
				{
					this._damageLerpScale.x = this._shieldScale.x;
					this._shieldScale.x = this._percentWithShield;
					this._container.gameObject.SetActive(false);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04001D77 RID: 7543
		[SerializeField]
		protected RectTransform _container;

		// Token: 0x04001D78 RID: 7544
		[SerializeField]
		protected bool _alwaysVisible;

		// Token: 0x04001D79 RID: 7545
		[SerializeField]
		protected bool _roundUp;

		// Token: 0x04001D7A RID: 7546
		[SerializeField]
		private CharacterHealthBar.ActionWhenCharacterNull _actionWhenCharacterNull;

		// Token: 0x04001D7B RID: 7547
		[SerializeField]
		protected RectTransform _healthBar;

		// Token: 0x04001D7C RID: 7548
		[SerializeField]
		protected RectTransform _grayHealthBar;

		// Token: 0x04001D7D RID: 7549
		[SerializeField]
		protected RectTransform _canHealBar;

		// Token: 0x04001D7E RID: 7550
		[SerializeField]
		protected RectTransform _shieldBar;

		// Token: 0x04001D7F RID: 7551
		[SerializeField]
		protected RectTransform _damageLerpBar;

		// Token: 0x04001D80 RID: 7552
		[SerializeField]
		protected RectTransform _healLerpBar;

		// Token: 0x04001D81 RID: 7553
		protected Character _character;

		// Token: 0x04001D82 RID: 7554
		protected CharacterHealth _health;

		// Token: 0x04001D83 RID: 7555
		protected float _percent;

		// Token: 0x04001D84 RID: 7556
		protected float _percentWithGrayHealth;

		// Token: 0x04001D85 RID: 7557
		protected float _percentWithCanHeal;

		// Token: 0x04001D86 RID: 7558
		protected float _percentWithShield;

		// Token: 0x04001D87 RID: 7559
		protected Vector3 _defaultHealthScale;

		// Token: 0x04001D88 RID: 7560
		protected Vector3 _defaultGrayHealthScale;

		// Token: 0x04001D89 RID: 7561
		protected Vector3 _defaultCanHealScale;

		// Token: 0x04001D8A RID: 7562
		protected Vector3 _defaultShieldScale;

		// Token: 0x04001D8B RID: 7563
		protected Vector3 _defaultDamageLerpScale;

		// Token: 0x04001D8C RID: 7564
		protected Vector3 _defaultHealLerpScale;

		// Token: 0x04001D8D RID: 7565
		protected Vector3 _healthScale = Vector3.one;

		// Token: 0x04001D8E RID: 7566
		protected Vector3 _grayHealthScale = Vector3.one;

		// Token: 0x04001D8F RID: 7567
		protected Vector3 _canHealScale = Vector3.one;

		// Token: 0x04001D90 RID: 7568
		protected Vector3 _shieldScale = Vector3.one;

		// Token: 0x04001D91 RID: 7569
		protected Vector3 _damageLerpScale = Vector3.one;

		// Token: 0x04001D92 RID: 7570
		protected Vector3 _healLerpScale = Vector3.one;

		// Token: 0x04001D93 RID: 7571
		private const float _lifeTime = 3f;

		// Token: 0x04001D94 RID: 7572
		protected float _remainLifetime;

		// Token: 0x020006BF RID: 1727
		public enum ActionWhenCharacterNull
		{
			// Token: 0x04001D96 RID: 7574
			Deactivate,
			// Token: 0x04001D97 RID: 7575
			ShowZero
		}
	}
}
