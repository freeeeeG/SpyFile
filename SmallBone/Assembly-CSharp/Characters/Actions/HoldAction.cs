using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000940 RID: 2368
	public class HoldAction : Action
	{
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x060032DA RID: 13018 RVA: 0x0009729D File Offset: 0x0009549D
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

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x000972AE File Offset: 0x000954AE
		public Motion motion
		{
			get
			{
				return this._motion;
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x060032DC RID: 13020 RVA: 0x000972B6 File Offset: 0x000954B6
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._motion);
			}
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000972E0 File Offset: 0x000954E0
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
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x000972FF File Offset: 0x000954FF
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._motion.Initialize(this);
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x00097314 File Offset: 0x00095514
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._motion);
			return true;
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00097335 File Offset: 0x00095535
		public override bool TryEnd()
		{
			base.owner.CancelAction();
			return false;
		}

		// Token: 0x04002979 RID: 10617
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion _motion;
	}
}
