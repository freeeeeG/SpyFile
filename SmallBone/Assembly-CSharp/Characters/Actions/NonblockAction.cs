using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200094F RID: 2383
	public class NonblockAction : Action
	{
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06003345 RID: 13125 RVA: 0x00098504 File Offset: 0x00096704
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

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x00098515 File Offset: 0x00096715
		public Motion motion
		{
			get
			{
				return this._motion;
			}
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003347 RID: 13127 RVA: 0x0009851D File Offset: 0x0009671D
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._motion);
			}
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00098547 File Offset: 0x00096747
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

		// Token: 0x06003349 RID: 13129 RVA: 0x00098566 File Offset: 0x00096766
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._motion.Initialize(this);
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x0009857B File Offset: 0x0009677B
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoActionNonBlock(this.motion);
			return true;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x0009859C File Offset: 0x0009679C
		public override bool Process()
		{
			base.Process();
			return false;
		}

		// Token: 0x040029BB RID: 10683
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _motion;
	}
}
