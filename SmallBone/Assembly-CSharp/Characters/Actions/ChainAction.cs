using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000920 RID: 2336
	public class ChainAction : Action
	{
		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x000950BE File Offset: 0x000932BE
		public override Motion[] motions
		{
			get
			{
				return this._motions.components;
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x000950CB File Offset: 0x000932CB
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.motions[0]);
			}
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000950F8 File Offset: 0x000932F8
		protected override void Awake()
		{
			base.Awake();
			bool flag = false;
			for (int i = 0; i < this._motions.components.Length; i++)
			{
				Motion motion = this._motions.components[i];
				if (motion.blockLook)
				{
					if (flag)
					{
						motion.onStart += delegate()
						{
							base.owner.lookingDirection = this._lookingDirection;
						};
					}
					motion.onStart += delegate()
					{
						this._lookingDirection = base.owner.lookingDirection;
					};
				}
				flag = motion.blockLook;
				if (i + 1 < this.motions.Length)
				{
					int cached = i + 1;
					this.motions[i].onEnd += delegate()
					{
						this.DoMotion(this.motions[cached]);
					};
				}
			}
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x000951AC File Offset: 0x000933AC
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this.motions.Length; i++)
			{
				this.motions[i].Initialize(this);
			}
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x000951E1 File Offset: 0x000933E1
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this.motions[0]);
			return true;
		}

		// Token: 0x04002908 RID: 10504
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion.Subcomponents _motions;

		// Token: 0x04002909 RID: 10505
		private Character.LookingDirection _lookingDirection;
	}
}
