using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200071B RID: 1819
	public class Shield
	{
		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060024D6 RID: 9430 RVA: 0x0006EAFC File Offset: 0x0006CCFC
		// (remove) Token: 0x060024D7 RID: 9431 RVA: 0x0006EB34 File Offset: 0x0006CD34
		public event Action<Shield.Instance> onAdd;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x060024D8 RID: 9432 RVA: 0x0006EB6C File Offset: 0x0006CD6C
		// (remove) Token: 0x060024D9 RID: 9433 RVA: 0x0006EBA4 File Offset: 0x0006CDA4
		public event Action<Shield.Instance> onRemove;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x060024DA RID: 9434 RVA: 0x0006EBDC File Offset: 0x0006CDDC
		// (remove) Token: 0x060024DB RID: 9435 RVA: 0x0006EC14 File Offset: 0x0006CE14
		public event Action<Shield.Instance> onUpdate;

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x0006EC49 File Offset: 0x0006CE49
		public double amount
		{
			get
			{
				return this._shields.Sum((Shield.Instance shield) => shield.amount);
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x0006EC75 File Offset: 0x0006CE75
		public bool hasAny
		{
			get
			{
				return this._shields.Count > 0;
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x0006EC88 File Offset: 0x0006CE88
		public Shield.Instance Add(object key, float amount, Action onBroke = null)
		{
			Shield.Instance instance = new Shield.Instance(key, amount, onBroke);
			this._shields.Add(instance);
			Action<Shield.Instance> action = this.onAdd;
			if (action != null)
			{
				action(instance);
			}
			return instance;
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0006ECC0 File Offset: 0x0006CEC0
		public void AddOrUpdate(object key, float amount, Action onBroke = null)
		{
			int i = 0;
			while (i < this._shields.Count)
			{
				Shield.Instance instance = this._shields[i];
				if (instance.key.Equals(key))
				{
					instance.amount = (double)amount;
					Action<Shield.Instance> action = this.onUpdate;
					if (action == null)
					{
						return;
					}
					action(instance);
					return;
				}
				else
				{
					i++;
				}
			}
			this._shields.Add(new Shield.Instance(key, amount, onBroke));
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x0006ED2C File Offset: 0x0006CF2C
		public bool Remove(object key)
		{
			for (int i = 0; i < this._shields.Count; i++)
			{
				Shield.Instance instance = this._shields[i];
				if (instance.key.Equals(key))
				{
					this._shields.RemoveAt(i);
					Action<Shield.Instance> action = this.onRemove;
					if (action != null)
					{
						action(instance);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x0006ED8C File Offset: 0x0006CF8C
		public void Clear()
		{
			for (int i = 0; i < this._shields.Count; i++)
			{
				Shield.Instance obj = this._shields[i];
				Action<Shield.Instance> action = this.onRemove;
				if (action != null)
				{
					action(obj);
				}
			}
			this._shields.Clear();
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0006EDDC File Offset: 0x0006CFDC
		public double Consume(double damage)
		{
			for (int i = 0; i < this._shields.Count; i++)
			{
				damage = this._shields[i].Consume(damage);
			}
			for (int j = this._shields.Count - 1; j >= 0; j--)
			{
				Shield.Instance instance = this._shields[j];
				if (instance.amount > 0.0)
				{
					break;
				}
				Debug.Log("Shield was not destroyed even though the shield amount is 0. So it's manually broken.");
				instance.Break();
			}
			return damage;
		}

		// Token: 0x04001F4B RID: 8011
		private readonly List<Shield.Instance> _shields = new List<Shield.Instance>();

		// Token: 0x0200071C RID: 1820
		public class Instance
		{
			// Token: 0x170007BB RID: 1979
			// (get) Token: 0x060024E4 RID: 9444 RVA: 0x0006EE6D File Offset: 0x0006D06D
			// (set) Token: 0x060024E5 RID: 9445 RVA: 0x0006EE75 File Offset: 0x0006D075
			public double originalAmount { get; set; }

			// Token: 0x170007BC RID: 1980
			// (get) Token: 0x060024E6 RID: 9446 RVA: 0x0006EE7E File Offset: 0x0006D07E
			// (set) Token: 0x060024E7 RID: 9447 RVA: 0x0006EE86 File Offset: 0x0006D086
			public double amount { get; set; }

			// Token: 0x060024E8 RID: 9448 RVA: 0x0006EE8F File Offset: 0x0006D08F
			public Instance(object key, float amount, Action onBroke)
			{
				this.key = key;
				this.originalAmount = (double)amount;
				this.amount = (double)amount;
				this._onBroke = onBroke;
			}

			// Token: 0x060024E9 RID: 9449 RVA: 0x0006EEB5 File Offset: 0x0006D0B5
			public void Break()
			{
				this.amount = 0.0;
				Action onBroke = this._onBroke;
				if (onBroke == null)
				{
					return;
				}
				onBroke();
			}

			// Token: 0x060024EA RID: 9450 RVA: 0x0006EED8 File Offset: 0x0006D0D8
			public double Consume(double damage)
			{
				if (damage < this.amount)
				{
					this.amount -= damage;
					return 0.0;
				}
				damage -= this.amount;
				this.amount = 0.0;
				Action onBroke = this._onBroke;
				if (onBroke != null)
				{
					onBroke();
				}
				return damage;
			}

			// Token: 0x04001F4C RID: 8012
			public readonly object key;

			// Token: 0x04001F4D RID: 8013
			private Action _onBroke;
		}
	}
}
