using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000938 RID: 2360
	public class EnhanceableSimpleAction : Action
	{
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x000969C6 File Offset: 0x00094BC6
		private Motion currentMotion
		{
			get
			{
				if (!this.enhanced)
				{
					return this._motion;
				}
				return this._enhancedMotion;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000969DD File Offset: 0x00094BDD
		public override Motion[] motions
		{
			get
			{
				return new Motion[]
				{
					this._motion
				};
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000969EE File Offset: 0x00094BEE
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.currentMotion);
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x00096A18 File Offset: 0x00094C18
		protected override void Awake()
		{
			base.Awake();
			this._motion.onEnd += delegate()
			{
				Action onEnd = this._onEnd;
				if (onEnd == null)
				{
					return;
				}
				onEnd();
			};
			this._motion.onCancel += delegate()
			{
				Action onCancel = this._onCancel;
				if (onCancel == null)
				{
					return;
				}
				onCancel();
			};
			this._enhancedMotion.onEnd += delegate()
			{
				Action onEnd = this._onEnd;
				if (onEnd == null)
				{
					return;
				}
				onEnd();
			};
			this._enhancedMotion.onCancel += delegate()
			{
				Action onCancel = this._onCancel;
				if (onCancel == null)
				{
					return;
				}
				onCancel();
			};
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x00096A87 File Offset: 0x00094C87
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._motion.Initialize(this);
			this._enhancedMotion.Initialize(this);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00096AA8 File Offset: 0x00094CA8
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this.currentMotion);
			return true;
		}

		// Token: 0x04002959 RID: 10585
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _motion;

		// Token: 0x0400295A RID: 10586
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion _enhancedMotion;

		// Token: 0x0400295B RID: 10587
		[NonSerialized]
		public bool enhanced;
	}
}
