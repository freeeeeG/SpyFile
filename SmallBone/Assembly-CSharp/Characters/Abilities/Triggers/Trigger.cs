using System;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B66 RID: 2918
	public abstract class Trigger : ITrigger
	{
		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06003A63 RID: 14947 RVA: 0x000ACA20 File Offset: 0x000AAC20
		// (remove) Token: 0x06003A64 RID: 14948 RVA: 0x000ACA39 File Offset: 0x000AAC39
		public event Action onTriggered
		{
			add
			{
				this._onTriggered = (Action)Delegate.Combine(this._onTriggered, value);
			}
			remove
			{
				this._onTriggered = (Action)Delegate.Remove(this._onTriggered, value);
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06003A65 RID: 14949 RVA: 0x000ACA52 File Offset: 0x000AAC52
		public float cooldownTime
		{
			get
			{
				return this._cooldownTime;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x000ACA5A File Offset: 0x000AAC5A
		public float remainCooldownTime
		{
			get
			{
				return this._remainCooldownTime;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06003A67 RID: 14951 RVA: 0x000ACA62 File Offset: 0x000AAC62
		protected bool canBeTriggered
		{
			get
			{
				return this._remainCooldownTime <= 0f && MMMaths.PercentChance(this._possibility);
			}
		}

		// Token: 0x06003A68 RID: 14952
		public abstract void Attach(Character character);

		// Token: 0x06003A69 RID: 14953
		public abstract void Detach();

		// Token: 0x06003A6A RID: 14954 RVA: 0x000ACA7E File Offset: 0x000AAC7E
		public virtual void UpdateTime(float deltaTime)
		{
			this._remainCooldownTime -= deltaTime;
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x000ACA8E File Offset: 0x000AAC8E
		protected void Invoke()
		{
			if (this.canBeTriggered)
			{
				this._remainCooldownTime = this._cooldownTime;
				Action onTriggered = this._onTriggered;
				if (onTriggered == null)
				{
					return;
				}
				onTriggered();
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x00002191 File Offset: 0x00000391
		public virtual void Refresh()
		{
		}

		// Token: 0x04002E6F RID: 11887
		protected Action _onTriggered;

		// Token: 0x04002E70 RID: 11888
		[Range(0f, 100f)]
		[SerializeField]
		protected int _possibility = 100;

		// Token: 0x04002E71 RID: 11889
		[SerializeField]
		protected float _cooldownTime;

		// Token: 0x04002E72 RID: 11890
		protected float _remainCooldownTime;
	}
}
