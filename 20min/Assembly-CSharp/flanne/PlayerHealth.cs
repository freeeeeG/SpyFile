using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000F9 RID: 249
	public class PlayerHealth : MonoBehaviour
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001F86D File Offset: 0x0001DA6D
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001F875 File Offset: 0x0001DA75
		public int baseMaxHP
		{
			get
			{
				return this._baseMaxHP;
			}
			set
			{
				this._baseMaxHP = value;
				this.maxHP = this._baseMaxHP;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001F88A File Offset: 0x0001DA8A
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001F894 File Offset: 0x0001DA94
		public int maxHP
		{
			get
			{
				return this._maxHP;
			}
			set
			{
				int num = Mathf.Clamp(value, 0, 20);
				this.shp = Mathf.Clamp(this.shp, 0, 20 - num);
				int num2 = num - this._maxHP;
				this._maxHP = num;
				if (num2 > 0)
				{
					this.hp += num2;
				}
				else
				{
					this.hp = Mathf.Clamp(this.hp, 1, this.maxHP);
				}
				this.onMaxHPChangedTo.Invoke(this._maxHP);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001F90E File Offset: 0x0001DB0E
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001F916 File Offset: 0x0001DB16
		public int hp
		{
			get
			{
				return this._hp;
			}
			set
			{
				this._hp = Mathf.Clamp(value, 0, this.maxHP);
				this.onHealthChangedTo.Invoke(this._hp);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001F93C File Offset: 0x0001DB3C
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001F944 File Offset: 0x0001DB44
		public int shp
		{
			get
			{
				return this._shp;
			}
			set
			{
				if (value < this._shp && this._shp != 0)
				{
					this.onLostSHP.Invoke();
				}
				else if (value > this._shp && this._shp < 20 - this.maxHP)
				{
					this.onGainedSHP.Invoke();
				}
				this._shp = Mathf.Clamp(value, 0, 20 - this.maxHP);
				this._shp = Mathf.Clamp(value, 0, this.maxSHP);
				this.onSoulHPChangedTo.Invoke(this._shp);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001F9D0 File Offset: 0x0001DBD0
		private void Start()
		{
			this.isInvincible = new BoolToggle(false);
			this.dodger = new DodgeRoller(this.player);
			this.vulnerableTags = new List<string>
			{
				"Enemy",
				"HarmfulToPlayer",
				"EProjectile"
			};
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001FA26 File Offset: 0x0001DC26
		private void OnTriggerEnter2D(Collider2D other)
		{
			this.CheckCollision(other.gameObject);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001FA34 File Offset: 0x0001DC34
		private void OnCollisionEnter2D(Collision2D other)
		{
			this.CheckCollision(other.gameObject);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001FA42 File Offset: 0x0001DC42
		public void Heal(int amount)
		{
			if (amount < 0)
			{
				Debug.LogWarning("Player should not be healed a negative amount");
			}
			this.hp += amount;
			this.onHealed.Invoke();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001FA6B File Offset: 0x0001DC6B
		public void AutoKill()
		{
			this.hp = 0;
			this.onDeath.Invoke();
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001FA7F File Offset: 0x0001DC7F
		public void RemoveVulnerability(string tag)
		{
			this.vulnerableTags.Remove(tag);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001FA90 File Offset: 0x0001DC90
		private void CheckCollision(GameObject other)
		{
			string tag = other.tag;
			if (!this.CheckIfVulnerable(tag))
			{
				return;
			}
			if (this.isInvincible.value)
			{
				return;
			}
			if (this.isProtected)
			{
				this.isProtected = false;
				this.onDamagePrevented.Invoke();
				return;
			}
			if (this.dodger.Roll())
			{
				this.onDodged.Invoke();
				return;
			}
			AIComponent component = other.gameObject.GetComponent<AIComponent>();
			if (component == null)
			{
				this.TakeDamage(1);
				return;
			}
			this.TakeDamage(component.damageToPlayer);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001FB1C File Offset: 0x0001DD1C
		private void TakeDamage(int damage)
		{
			for (int i = 0; i < damage; i++)
			{
				if (this.shp == 0)
				{
					int num = this.hp;
					this.hp = num - 1;
				}
				else
				{
					int num = this.shp;
					this.shp = num - 1;
				}
			}
			base.StartCoroutine(this.InvicibilityCR(1f));
			this.onHurt.Invoke();
			if (this.hp == 0)
			{
				this.onDeath.Invoke();
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001FB90 File Offset: 0x0001DD90
		private bool CheckIfVulnerable(string tag)
		{
			bool result = false;
			foreach (string value in this.vulnerableTags)
			{
				if (tag.Contains(value))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001FBEC File Offset: 0x0001DDEC
		private IEnumerator InvicibilityCR(float duration)
		{
			this.isInvincible.Flip();
			yield return new WaitForSeconds(duration);
			this.isInvincible.UnFlip();
			yield break;
		}

		// Token: 0x04000501 RID: 1281
		[SerializeField]
		private PlayerController player;

		// Token: 0x04000502 RID: 1282
		public UnityIntEvent onHealthChangedTo;

		// Token: 0x04000503 RID: 1283
		public UnityIntEvent onMaxHPChangedTo;

		// Token: 0x04000504 RID: 1284
		public UnityIntEvent onSoulHPChangedTo;

		// Token: 0x04000505 RID: 1285
		public UnityEvent onDamagePrevented;

		// Token: 0x04000506 RID: 1286
		public UnityEvent onDodged;

		// Token: 0x04000507 RID: 1287
		public UnityEvent onHealed;

		// Token: 0x04000508 RID: 1288
		public UnityEvent onGainedSHP;

		// Token: 0x04000509 RID: 1289
		public UnityEvent onLostSHP;

		// Token: 0x0400050A RID: 1290
		public UnityEvent onHurt;

		// Token: 0x0400050B RID: 1291
		public UnityEvent onDeath;

		// Token: 0x0400050C RID: 1292
		public List<string> vulnerableTags;

		// Token: 0x0400050D RID: 1293
		public int maxSHP = 3;

		// Token: 0x0400050E RID: 1294
		private int _baseMaxHP;

		// Token: 0x0400050F RID: 1295
		private int _maxHP;

		// Token: 0x04000510 RID: 1296
		private int _hp;

		// Token: 0x04000511 RID: 1297
		private int _shp;

		// Token: 0x04000512 RID: 1298
		public BoolToggle isInvincible;

		// Token: 0x04000513 RID: 1299
		public bool isProtected;

		// Token: 0x04000514 RID: 1300
		private DodgeRoller dodger;
	}
}
