using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200095C RID: 2396
	public class SimpleAction : Action
	{
		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x0600339A RID: 13210 RVA: 0x00099167 File Offset: 0x00097367
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

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600339B RID: 13211 RVA: 0x00099178 File Offset: 0x00097378
		public Motion motion
		{
			get
			{
				return this._motion;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x0600339C RID: 13212 RVA: 0x00099180 File Offset: 0x00097380
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._motion);
			}
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x000991AA File Offset: 0x000973AA
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
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x000991E0 File Offset: 0x000973E0
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this._motion.Initialize(this);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x000991F5 File Offset: 0x000973F5
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._motion);
			return true;
		}

		// Token: 0x040029E1 RID: 10721
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion _motion;
	}
}
