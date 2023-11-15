using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200094D RID: 2381
	public class MultipleAction : Action
	{
		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x00098350 File Offset: 0x00096550
		public override Motion[] motions
		{
			get
			{
				return this._motions.components;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x00098360 File Offset: 0x00096560
		public override bool canUse
		{
			get
			{
				if (!base.cooldown.canUse || this._owner.stunedOrFreezed)
				{
					return false;
				}
				for (int i = 0; i < this._motions.components.Length; i++)
				{
					if (base.PassAllConstraints(this._motions.components[i]))
					{
						return true;
					}
				}
				return true;
			}
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000983BC File Offset: 0x000965BC
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			for (int i = 0; i < this.motions.Length; i++)
			{
				this.motions[i].Initialize(this);
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000983F4 File Offset: 0x000965F4
		public override bool TryStart()
		{
			if (!base.cooldown.canUse || this._owner.stunedOrFreezed)
			{
				return false;
			}
			for (int i = 0; i < this._motions.components.Length; i++)
			{
				if (base.PassAllConstraints(this._motions.components[i]) && base.ConsumeCooldownIfNeeded())
				{
					base.DoAction(this._motions.components[i]);
					return true;
				}
			}
			return false;
		}

		// Token: 0x040029B9 RID: 10681
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion.Subcomponents _motions;
	}
}
