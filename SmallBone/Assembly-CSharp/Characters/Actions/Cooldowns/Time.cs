using System;
using System.Collections;
using UnityEngine;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x0200096A RID: 2410
	public class Time : Basic
	{
		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060033F8 RID: 13304 RVA: 0x00099FC0 File Offset: 0x000981C0
		public override float remainPercent
		{
			get
			{
				if (this._remainStreaks > 0)
				{
					return 0f;
				}
				if (base.stacks != this._maxStacks)
				{
					return this.remainTime / this._cooldownTime;
				}
				return 0f;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060033F9 RID: 13305 RVA: 0x00099FF2 File Offset: 0x000981F2
		public float remainStreakPercent
		{
			get
			{
				return this._remainStreaksTime / this._streakTimeout;
			}
		}

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060033FA RID: 13306 RVA: 0x0009A001 File Offset: 0x00098201
		public float cooldownTime
		{
			get
			{
				return this._cooldownTime;
			}
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0009A009 File Offset: 0x00098209
		internal override void Initialize(Character character)
		{
			base.Initialize(character);
			this._updateReference.Stop();
			this._updateReference = CoroutineProxy.instance.StartCoroutineWithReference(this.CUpdate());
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x0009A033 File Offset: 0x00098233
		protected override void Awake()
		{
			base.Awake();
			this.remainTime = this._cooldownTime;
			if (this._continual)
			{
				base.GetComponentInParent<Action>().onEnd += delegate()
				{
					this._remainStreaksTime = 0f;
				};
			}
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x0009A066 File Offset: 0x00098266
		private void OnDestroy()
		{
			this._updateReference.Stop();
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x0009A073 File Offset: 0x00098273
		private IEnumerator CUpdate()
		{
			for (;;)
			{
				yield return null;
				if (!(this._character == null) && base.stacks != this._maxStacks && this._remainStreaks <= 0)
				{
					this.remainTime -= this._character.chronometer.master.deltaTime;
					if (this.remainTime <= 0f)
					{
						this.remainTime = this._cooldownTime;
						int stacks = base.stacks;
						base.stacks = stacks + 1;
					}
				}
			}
			yield break;
		}

		// Token: 0x04002A13 RID: 10771
		[NonSerialized]
		public float remainTime;

		// Token: 0x04002A14 RID: 10772
		[SerializeField]
		protected float _cooldownTime;

		// Token: 0x04002A15 RID: 10773
		[SerializeField]
		private bool _continual;

		// Token: 0x04002A16 RID: 10774
		private CoroutineReference _updateReference;
	}
}
