using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000930 RID: 2352
	public class EnhanceableChainAction : Action
	{
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x00096401 File Offset: 0x00094601
		public override Motion[] motions
		{
			get
			{
				if (!this.enhanced)
				{
					return this._motions.components;
				}
				return this._enhancedMotions.components;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x000950CB File Offset: 0x000932CB
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.motions[0]);
			}
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x00096422 File Offset: 0x00094622
		protected override void Awake()
		{
			base.Awake();
			this.InitailizeMotionEvents(this._motions.components);
			this.InitailizeMotionEvents(this._enhancedMotions.components);
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x0009644C File Offset: 0x0009464C
		private void InitailizeMotionEvents(Motion[] motions)
		{
			bool flag = false;
			Action <>9__0;
			Action <>9__1;
			for (int i = 0; i < motions.Length; i++)
			{
				Motion motion = motions[i];
				if (motion.blockLook)
				{
					if (flag)
					{
						Motion motion2 = motion;
						Action value;
						if ((value = <>9__0) == null)
						{
							value = (<>9__0 = delegate()
							{
								this.owner.lookingDirection = this._lookingDirection;
							});
						}
						motion2.onStart += value;
					}
					Motion motion3 = motion;
					Action value2;
					if ((value2 = <>9__1) == null)
					{
						value2 = (<>9__1 = delegate()
						{
							this._lookingDirection = this.owner.lookingDirection;
						});
					}
					motion3.onStart += value2;
				}
				flag = motion.blockLook;
				if (i + 1 < motions.Length)
				{
					int cached = i + 1;
					motions[i].onEnd += delegate()
					{
						this.DoMotion(motions[cached]);
					};
				}
			}
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x00096538 File Offset: 0x00094738
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this.motions.Length; i++)
			{
				this._motions.components[i].Initialize(this);
			}
			for (int j = 0; j < this.motions.Length; j++)
			{
				this._enhancedMotions.components[j].Initialize(this);
			}
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000951E1 File Offset: 0x000933E1
		public override bool TryStart()
		{
			if (!base.gameObject.activeSelf || !this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this.motions[0]);
			return true;
		}

		// Token: 0x04002941 RID: 10561
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion.Subcomponents _motions;

		// Token: 0x04002942 RID: 10562
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion.Subcomponents _enhancedMotions;

		// Token: 0x04002943 RID: 10563
		private Character.LookingDirection _lookingDirection;

		// Token: 0x04002944 RID: 10564
		[NonSerialized]
		public bool enhanced;
	}
}
