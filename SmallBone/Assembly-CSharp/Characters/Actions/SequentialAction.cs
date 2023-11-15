using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200095B RID: 2395
	public class SequentialAction : Action
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06003394 RID: 13204 RVA: 0x0009906F File Offset: 0x0009726F
		public override Motion[] motions
		{
			get
			{
				return this._motions.components;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003395 RID: 13205 RVA: 0x0009907C File Offset: 0x0009727C
		public Motion currentMotion
		{
			get
			{
				return this._motions.components[this._motionIndex];
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x00099090 File Offset: 0x00097290
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this.currentMotion);
			}
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000990BC File Offset: 0x000972BC
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			if (this._shuffle)
			{
				this.motions.PseudoShuffle<Motion>();
			}
			for (int i = 0; i < this.motions.Length; i++)
			{
				this.motions[i].Initialize(this);
			}
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x00099104 File Offset: 0x00097304
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._motions.components[this._motionIndex]);
			this._motionIndex = (this._motionIndex + 1) % this._motions.components.Length;
			return true;
		}

		// Token: 0x040029DE RID: 10718
		[SerializeField]
		private bool _shuffle = true;

		// Token: 0x040029DF RID: 10719
		[SerializeField]
		[Subcomponent(typeof(Motion))]
		protected Motion.Subcomponents _motions;

		// Token: 0x040029E0 RID: 10720
		private int _motionIndex;
	}
}
