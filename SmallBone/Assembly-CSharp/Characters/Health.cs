using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006FD RID: 1789
	public class Health : MonoBehaviour
	{
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06002409 RID: 9225 RVA: 0x0006CA1C File Offset: 0x0006AC1C
		// (remove) Token: 0x0600240A RID: 9226 RVA: 0x0006CA54 File Offset: 0x0006AC54
		public event TookDamageDelegate onTookDamage;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x0600240B RID: 9227 RVA: 0x0006CA8C File Offset: 0x0006AC8C
		// (remove) Token: 0x0600240C RID: 9228 RVA: 0x0006CAC4 File Offset: 0x0006ACC4
		public event Action onDie;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x0600240D RID: 9229 RVA: 0x0006CAFC File Offset: 0x0006ACFC
		// (remove) Token: 0x0600240E RID: 9230 RVA: 0x0006CB34 File Offset: 0x0006AD34
		public event Action onDied;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x0600240F RID: 9231 RVA: 0x0006CB6C File Offset: 0x0006AD6C
		// (remove) Token: 0x06002410 RID: 9232 RVA: 0x0006CBA4 File Offset: 0x0006ADA4
		public event Action onDiedTryCatch;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06002411 RID: 9233 RVA: 0x0006CBDC File Offset: 0x0006ADDC
		// (remove) Token: 0x06002412 RID: 9234 RVA: 0x0006CC14 File Offset: 0x0006AE14
		public event Action onChanged;

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06002413 RID: 9235 RVA: 0x0006CC4C File Offset: 0x0006AE4C
		// (remove) Token: 0x06002414 RID: 9236 RVA: 0x0006CC84 File Offset: 0x0006AE84
		public event Action onConsumeShield;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06002415 RID: 9237 RVA: 0x0006CCBC File Offset: 0x0006AEBC
		// (remove) Token: 0x06002416 RID: 9238 RVA: 0x0006CCF4 File Offset: 0x0006AEF4
		public event Health.HealedDelegate onHealed;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06002417 RID: 9239 RVA: 0x0006CD2C File Offset: 0x0006AF2C
		// (remove) Token: 0x06002418 RID: 9240 RVA: 0x0006CD64 File Offset: 0x0006AF64
		public event Health.HealByGiver onHealByGiver;

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x0006CD99 File Offset: 0x0006AF99
		public PriorityList<TakeDamageDelegate> onTakeDamage
		{
			get
			{
				return this._onTakeDamage;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x0006CDA1 File Offset: 0x0006AFA1
		// (set) Token: 0x0600241B RID: 9243 RVA: 0x0006CDA9 File Offset: 0x0006AFA9
		public Character owner { get; set; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x0006CDB2 File Offset: 0x0006AFB2
		// (set) Token: 0x0600241D RID: 9245 RVA: 0x0006CDBA File Offset: 0x0006AFBA
		public Shield shield { get; private set; } = new Shield();

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0006CDC3 File Offset: 0x0006AFC3
		// (set) Token: 0x0600241F RID: 9247 RVA: 0x0006CDCB File Offset: 0x0006AFCB
		public GrayHealth grayHealth { get; private set; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0006CDD4 File Offset: 0x0006AFD4
		// (set) Token: 0x06002421 RID: 9249 RVA: 0x0006CDDC File Offset: 0x0006AFDC
		public double currentHealth { get; private set; }

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x0006CDE5 File Offset: 0x0006AFE5
		// (set) Token: 0x06002423 RID: 9251 RVA: 0x0006CDED File Offset: 0x0006AFED
		public double maximumHealth { get; private set; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x0006CDF6 File Offset: 0x0006AFF6
		// (set) Token: 0x06002425 RID: 9253 RVA: 0x0006CDFE File Offset: 0x0006AFFE
		public double percent { get; private set; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x0006CE07 File Offset: 0x0006B007
		// (set) Token: 0x06002427 RID: 9255 RVA: 0x0006CE0F File Offset: 0x0006B00F
		public float displayPercent { get; private set; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x0006CE18 File Offset: 0x0006B018
		// (set) Token: 0x06002429 RID: 9257 RVA: 0x0006CE20 File Offset: 0x0006B020
		public bool dead { get; private set; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x0006CE29 File Offset: 0x0006B029
		// (set) Token: 0x0600242B RID: 9259 RVA: 0x0006CE31 File Offset: 0x0006B031
		public double lastTakenDamage { get; private set; }

		// Token: 0x0600242C RID: 9260 RVA: 0x0006CE3A File Offset: 0x0006B03A
		private void Awake()
		{
			this._collider = base.GetComponentInChildren<Collider2D>();
			this.grayHealth = new GrayHealth(this);
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0006CE54 File Offset: 0x0006B054
		public void SetHealth(double current, double maximum)
		{
			this.currentHealth = ((current < maximum) ? current : maximum);
			this.maximumHealth = maximum;
			this.UpdateHealth();
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0006CE71 File Offset: 0x0006B071
		public void SetCurrentHealth(double health)
		{
			this.currentHealth = health;
			this.UpdateHealth();
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0006CE80 File Offset: 0x0006B080
		public void SetMaximumHealth(double health)
		{
			this.maximumHealth = health;
			if (this.currentHealth > this.maximumHealth)
			{
				this.currentHealth = health;
			}
			this.UpdateHealth();
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0006CEA4 File Offset: 0x0006B0A4
		public bool TakeDamage(ref Damage damage)
		{
			double num;
			return this.TakeDamage(ref damage, out num);
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0006CEBC File Offset: 0x0006B0BC
		public bool TakeDamage(ref Damage damage, out double dealtDamage)
		{
			damage.Evaluate(this.immuneToCritical);
			Damage damage2 = damage;
			if (this._onTakeDamage.Invoke(ref damage))
			{
				damage.@base = 0.0;
				dealtDamage = 0.0;
				return true;
			}
			if (damage.@null)
			{
				damage.@base = 0.0;
				dealtDamage = 0.0;
			}
			double amount = damage.amount;
			double num = this.shield.Consume(amount);
			if (amount != num)
			{
				Action action = this.onConsumeShield;
				if (action != null)
				{
					action();
				}
			}
			double num2 = amount - num;
			double num3 = this.TakeHealth(num);
			TookDamageDelegate tookDamageDelegate = this.onTookDamage;
			if (tookDamageDelegate != null)
			{
				tookDamageDelegate(damage2, damage, num3 + num2);
			}
			dealtDamage = num3 + num2;
			return false;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0006CF80 File Offset: 0x0006B180
		public double TakeHealth(double amount)
		{
			if (this.dead)
			{
				return 0.0;
			}
			this.currentHealth -= amount;
			this.lastTakenDamage = amount;
			if (this.currentHealth <= 0.0)
			{
				double currentHealth = this.currentHealth;
				this.currentHealth = 0.0;
				try
				{
					Action action = this.onDie;
					if (action != null)
					{
						action();
					}
					if (this.currentHealth <= 0.0)
					{
						this.dead = true;
						Action action2 = this.onDied;
						if (action2 != null)
						{
							action2();
						}
						Action action3 = this.onDiedTryCatch;
						if (action3 != null)
						{
							action3();
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("Eror while running onDie or onDied of " + base.name + " : " + ex.Message);
					this.currentHealth = 0.0;
					this.dead = true;
					Action action4 = this.onDiedTryCatch;
					if (action4 != null)
					{
						action4();
					}
				}
				this.UpdateHealth();
				return amount + currentHealth;
			}
			this.UpdateHealth();
			return amount;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0006D098 File Offset: 0x0006B298
		public double PercentHeal(float percent)
		{
			return this.Heal(this.maximumHealth * (double)percent, true);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0006D0AC File Offset: 0x0006B2AC
		public double FixedAmountHeal(double amount, bool notify = true)
		{
			double num = 0.0;
			this.currentHealth += amount;
			if (this.currentHealth > this.maximumHealth)
			{
				num = this.currentHealth - this.maximumHealth;
				amount -= num;
				this.currentHealth = this.maximumHealth;
			}
			this.UpdateHealth();
			if (notify)
			{
				if (this._collider == null)
				{
					Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, base.transform.position);
				}
				else
				{
					Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, MMMaths.RandomPointWithinBounds(this._collider.bounds));
				}
				Health.HealedDelegate healedDelegate = this.onHealed;
				if (healedDelegate != null)
				{
					healedDelegate(amount, num);
				}
			}
			return num;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0006D16B File Offset: 0x0006B36B
		public double Heal(Health.HealInfo info)
		{
			Health.HealByGiver healByGiver = this.onHealByGiver;
			if (healByGiver != null)
			{
				healByGiver(ref info);
			}
			return this.Heal(ref info.amount, info.notify);
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0006D193 File Offset: 0x0006B393
		public double Heal(double amount, bool notify = true)
		{
			return this.Heal(ref amount, notify);
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0006D1A0 File Offset: 0x0006B3A0
		public double Heal(ref double amount, bool notify = true)
		{
			amount *= this.owner.stat.GetFinal(Stat.Kind.TakingHealAmount);
			double num = 0.0;
			this.currentHealth += amount;
			if (this.currentHealth > this.maximumHealth)
			{
				num = this.currentHealth - this.maximumHealth;
				amount -= num;
				this.currentHealth = this.maximumHealth;
			}
			this.UpdateHealth();
			if (notify)
			{
				if (this._collider == null)
				{
					Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, base.transform.position);
				}
				else
				{
					Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(amount, MMMaths.RandomPointWithinBounds(this._collider.bounds));
				}
				Health.HealedDelegate healedDelegate = this.onHealed;
				if (healedDelegate != null)
				{
					healedDelegate(amount, num);
				}
			}
			return num;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0006D280 File Offset: 0x0006B480
		public void GrayHeal()
		{
			double num = this.grayHealth.canHeal;
			double num2 = 0.0;
			this.currentHealth += num;
			if (this.currentHealth > this.maximumHealth)
			{
				num2 = this.currentHealth - this.maximumHealth;
				num -= num2;
				this.currentHealth = this.maximumHealth;
			}
			this.grayHealth.maximum = 0.0;
			this.UpdateHealth();
			if (this._collider == null)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(num, base.transform.position);
			}
			else
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnHeal(num, MMMaths.RandomPointWithinBounds(this._collider.bounds));
			}
			Health.HealedDelegate healedDelegate = this.onHealed;
			if (healedDelegate == null)
			{
				return;
			}
			healedDelegate(num, num2);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0006D359 File Offset: 0x0006B559
		public void ResetToMaximumHealth()
		{
			this.SetCurrentHealth(this.maximumHealth);
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0006D367 File Offset: 0x0006B567
		public void Revive()
		{
			this.Revive(this.maximumHealth);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0006D375 File Offset: 0x0006B575
		public void Revive(double health)
		{
			this.dead = false;
			this.SetCurrentHealth(health);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0006D388 File Offset: 0x0006B588
		public void TryKill()
		{
			this.currentHealth = 0.0;
			try
			{
				Action action = this.onDie;
				if (action != null)
				{
					action();
				}
				if (this.currentHealth <= 0.0)
				{
					this.dead = true;
					Action action2 = this.onDied;
					if (action2 != null)
					{
						action2();
					}
					Action action3 = this.onDiedTryCatch;
					if (action3 != null)
					{
						action3();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Eror while running onDie or onDied of " + base.name + " : " + ex.Message);
				this.currentHealth = 0.0;
				this.dead = true;
				Action action4 = this.onDiedTryCatch;
				if (action4 != null)
				{
					action4();
				}
			}
			this.UpdateHealth();
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0006D454 File Offset: 0x0006B654
		public void Kill()
		{
			if (this.dead)
			{
				return;
			}
			this.currentHealth = 0.0;
			this.dead = true;
			this.UpdateHealth();
			try
			{
				Action action = this.onDie;
				if (action != null)
				{
					action();
				}
				Action action2 = this.onDied;
				if (action2 != null)
				{
					action2();
				}
				Action action3 = this.onDiedTryCatch;
				if (action3 != null)
				{
					action3();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Eror while running onDie or onDied of " + base.name + " : " + ex.Message);
				Action action4 = this.onDiedTryCatch;
				if (action4 != null)
				{
					action4();
				}
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0006D500 File Offset: 0x0006B700
		private void UpdateHealth()
		{
			this.percent = this.currentHealth / this.maximumHealth;
			this.displayPercent = Mathf.Round((float)this.currentHealth) / (float)Mathf.RoundToInt((float)this.maximumHealth);
			Action action = this.onChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0006D550 File Offset: 0x0006B750
		private void OnDestroy()
		{
			this.onTookDamage = null;
			this.onDie = null;
			this.onDied = null;
			this.onDiedTryCatch = null;
			this.onChanged = null;
			this.onConsumeShield = null;
			this.onHealed = null;
			this._onTakeDamage.Clear();
		}

		// Token: 0x04001EE0 RID: 7904
		private readonly TakeDamageEvent _onTakeDamage = new TakeDamageEvent();

		// Token: 0x04001EE1 RID: 7905
		public bool immuneToCritical;

		// Token: 0x04001EE2 RID: 7906
		private Collider2D _collider;

		// Token: 0x020006FE RID: 1790
		public enum HealthGiverType
		{
			// Token: 0x04001EED RID: 7917
			None,
			// Token: 0x04001EEE RID: 7918
			Potion,
			// Token: 0x04001EEF RID: 7919
			AdventurerPotion,
			// Token: 0x04001EF0 RID: 7920
			System
		}

		// Token: 0x020006FF RID: 1791
		public struct HealInfo
		{
			// Token: 0x06002441 RID: 9281 RVA: 0x0006D5AC File Offset: 0x0006B7AC
			public HealInfo(Health.HealthGiverType healthGiverType, double amount, bool notify = true)
			{
				this.healthGiver = healthGiverType;
				this.amount = amount;
				this.notify = notify;
			}

			// Token: 0x04001EF1 RID: 7921
			public Health.HealthGiverType healthGiver;

			// Token: 0x04001EF2 RID: 7922
			public double amount;

			// Token: 0x04001EF3 RID: 7923
			public bool notify;
		}

		// Token: 0x02000700 RID: 1792
		// (Invoke) Token: 0x06002443 RID: 9283
		public delegate void HealedDelegate(double healed, double overHealed);

		// Token: 0x02000701 RID: 1793
		// (Invoke) Token: 0x06002447 RID: 9287
		public delegate void HealByGiver(ref Health.HealInfo healInfo);
	}
}
