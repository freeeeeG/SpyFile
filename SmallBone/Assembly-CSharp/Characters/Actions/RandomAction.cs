using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000959 RID: 2393
	public class RandomAction : Action
	{
		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x0600338D RID: 13197 RVA: 0x00098F98 File Offset: 0x00097198
		public override Motion[] motions
		{
			get
			{
				return this._motions.components;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x00098FA5 File Offset: 0x000971A5
		public override bool canUse
		{
			get
			{
				return base.cooldown.canUse && !this._owner.stunedOrFreezed && base.PassAllConstraints(this._motions.components[this._indexToUse]);
			}
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x00098FDC File Offset: 0x000971DC
		public override void Initialize(Character owner)
		{
			base.Initialize(owner);
			this.RandomizeIndex();
			for (int i = 0; i < this.motions.Length; i++)
			{
				this.motions[i].Initialize(this);
			}
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x00099017 File Offset: 0x00097217
		private void RandomizeIndex()
		{
			this._indexToUse = this._motions.components.RandomIndex<Motion>();
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x0009902F File Offset: 0x0009722F
		public override bool TryStart()
		{
			if (!base.cooldown.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			base.DoAction(this._motions.components[this._indexToUse]);
			this.RandomizeIndex();
			return true;
		}

		// Token: 0x040029DC RID: 10716
		[Subcomponent(typeof(Motion))]
		[SerializeField]
		protected Motion.Subcomponents _motions;

		// Token: 0x040029DD RID: 10717
		private int _indexToUse;
	}
}
