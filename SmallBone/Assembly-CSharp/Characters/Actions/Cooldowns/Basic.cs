using System;
using System.Collections;
using UnityEngine;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x02000962 RID: 2402
	public abstract class Basic : Cooldown
	{
		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x00099B8B File Offset: 0x00097D8B
		public override bool canUse
		{
			get
			{
				return base.stacks > 0 || this._remainStreaks > 0;
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x00099BA1 File Offset: 0x00097DA1
		public float remainStreaks
		{
			get
			{
				return (float)this._remainStreaks;
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x00099BAC File Offset: 0x00097DAC
		internal override bool Consume()
		{
			if (this._remainStreaks > 0)
			{
				this._remainStreaks--;
				return true;
			}
			if (base.stacks > 0)
			{
				int stacks = base.stacks;
				base.stacks = stacks - 1;
				if (this._streak != null)
				{
					base.StopCoroutine(this._streak);
				}
				this._streak = base.StartCoroutine(this.CStreak());
				return true;
			}
			return false;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x00099C14 File Offset: 0x00097E14
		protected override void Awake()
		{
			base.Awake();
			this._stacks = this._maxStacks;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x00099C28 File Offset: 0x00097E28
		private void OnDisable()
		{
			this._remainStreaks = 0;
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x00099C31 File Offset: 0x00097E31
		private IEnumerator CStreak()
		{
			this._remainStreaks = this._streakCount;
			this._remainStreaksTime = this._streakTimeout;
			Chronometer chronometer = this._character.chronometer.master;
			while (this._remainStreaksTime > 0f)
			{
				yield return null;
				this._remainStreaksTime -= chronometer.deltaTime;
			}
			this._remainStreaks = 0;
			this._streak = null;
			yield break;
		}

		// Token: 0x040029FB RID: 10747
		[SerializeField]
		protected int _maxStacks = 1;

		// Token: 0x040029FC RID: 10748
		[SerializeField]
		protected int _streakCount;

		// Token: 0x040029FD RID: 10749
		[SerializeField]
		protected float _streakTimeout;

		// Token: 0x040029FE RID: 10750
		protected int _remainStreaks;

		// Token: 0x040029FF RID: 10751
		protected float _remainStreaksTime;

		// Token: 0x04002A00 RID: 10752
		private Coroutine _streak;
	}
}
