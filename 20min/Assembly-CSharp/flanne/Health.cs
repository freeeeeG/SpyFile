using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000BD RID: 189
	public class Health : MonoBehaviour
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001C3E6 File Offset: 0x0001A5E6
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x0001C3F0 File Offset: 0x0001A5F0
		public int maxHP
		{
			get
			{
				return this._maxHP;
			}
			set
			{
				int num = value - this._maxHP;
				this._maxHP = value;
				this.onMaxHealthChange.Invoke(this._maxHP);
				if (num > 0)
				{
					this.HP += num;
					this.onHealthChange.Invoke(this.HP);
					return;
				}
				this.HP = Mathf.Clamp(this.HP, 0, this.maxHP);
				this.onHealthChange.Invoke(this.HP);
				if (this._maxHP <= 0)
				{
					this.onDeath.Invoke();
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x0001C47F File Offset: 0x0001A67F
		public bool isDead
		{
			get
			{
				return this.HP <= 0;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0001C48D File Offset: 0x0001A68D
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x0001C495 File Offset: 0x0001A695
		public int HP { get; private set; }

		// Token: 0x0600061F RID: 1567 RVA: 0x0001C49E File Offset: 0x0001A69E
		private void Awake()
		{
			this.isInvincible = new BoolToggle(false);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001C4AC File Offset: 0x0001A6AC
		private void OnEnable()
		{
			this.HP = this.maxHP;
			this.onMaxHealthChange.Invoke(this.maxHP);
			this.onHealthChange.Invoke(this.HP);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001C4DC File Offset: 0x0001A6DC
		public void TakeDamage(DamageType damageType, int damage, float finalMultiplier = 1f)
		{
			if (this.HP <= 0)
			{
				return;
			}
			if (damage < 0)
			{
				Debug.LogWarning("Cannot take negative damage.");
				return;
			}
			if (this.isInvincible.value)
			{
				this.onDamagePrevented.Invoke();
				return;
			}
			int num = damage.NotifyModifiers(string.Format("Health.Tweak{0}Damage", damageType.ToString()), this);
			num = num.NotifyModifiers(Health.TweakDamageEvent, this);
			num = Mathf.FloorToInt((float)num * finalMultiplier);
			bool hp = this.HP != 0;
			this.HP = Mathf.Clamp(this.HP - num, 0, this.maxHP);
			if (hp && this.HP == 0)
			{
				this.onDeath.Invoke();
				this.PostNotification(string.Format("Health.{0}DamageKill", damageType.ToString()), null);
				this.PostNotification(Health.DeathEvent, null);
			}
			this.PostNotification(Health.TookDamageEvent, num);
			this.onHealthChange.Invoke(this.HP);
			this.onHurt.Invoke(num);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		public void HPChange(int change)
		{
			if (this.HP <= 0)
			{
				return;
			}
			if (this.isInvincible.value && change < 0)
			{
				this.onDamagePrevented.Invoke();
				return;
			}
			int num = change.NotifyModifiers(Health.TweakDamageEvent, this);
			bool hp = this.HP != 0;
			this.HP = Mathf.Clamp(this.HP + num, 0, this.maxHP);
			this.onHealthChange.Invoke(this.HP);
			if (change < 0)
			{
				this.onHurt.Invoke(num);
			}
			else if (change > 0)
			{
				this.onHeal.Invoke(num);
			}
			if (hp && this.HP == 0)
			{
				this.onDeath.Invoke();
				this.PostNotification(Health.DeathEvent, null);
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001C695 File Offset: 0x0001A895
		public void AutoKill(bool notify = true)
		{
			if (this.HP <= 0)
			{
				return;
			}
			this.HP = 0;
			this.onDeath.Invoke();
			if (notify)
			{
				this.PostNotification(Health.DeathEvent, null);
			}
		}

		// Token: 0x040003EC RID: 1004
		public static string DeathEvent = "Health.DeathEvent";

		// Token: 0x040003ED RID: 1005
		public static string TweakDamageEvent = "Health.TweakDamageEvent";

		// Token: 0x040003EE RID: 1006
		public static string TookDamageEvent = "Health.TookDamageEvent";

		// Token: 0x040003EF RID: 1007
		[SerializeField]
		private int _maxHP;

		// Token: 0x040003F0 RID: 1008
		public BoolToggle isInvincible;

		// Token: 0x040003F1 RID: 1009
		public UnityIntEvent onHealthChange;

		// Token: 0x040003F2 RID: 1010
		public UnityIntEvent onMaxHealthChange;

		// Token: 0x040003F3 RID: 1011
		public UnityIntEvent onHurt;

		// Token: 0x040003F4 RID: 1012
		public UnityIntEvent onHeal;

		// Token: 0x040003F5 RID: 1013
		public UnityEvent onDeath;

		// Token: 0x040003F6 RID: 1014
		public UnityEvent onDamagePrevented;
	}
}
