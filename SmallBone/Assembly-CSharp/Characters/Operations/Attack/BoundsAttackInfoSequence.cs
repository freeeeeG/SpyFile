using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F6D RID: 3949
	public sealed class BoundsAttackInfoSequence : MonoBehaviour
	{
		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x000E3631 File Offset: 0x000E1831
		public float timeToTrigger
		{
			get
			{
				return this._timeToTrigger;
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x000E3639 File Offset: 0x000E1839
		public BoundsAttackInfo attackInfo
		{
			get
			{
				return this._attackInfo;
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x000E3641 File Offset: 0x000E1841
		public override string ToString()
		{
			return string.Format("{0:0.##}s({1:0.##}f), {2}", this._timeToTrigger, this._timeToTrigger * 60f, this._attackInfo.GetAutoName());
		}

		// Token: 0x04003C52 RID: 15442
		[FrameTime]
		[SerializeField]
		private float _timeToTrigger;

		// Token: 0x04003C53 RID: 15443
		[SerializeField]
		[Subcomponent(typeof(BoundsAttackInfo))]
		private BoundsAttackInfo _attackInfo;

		// Token: 0x02000F6E RID: 3950
		[Serializable]
		internal sealed class Subcomponents : SubcomponentArray<BoundsAttackInfoSequence>
		{
			// Token: 0x17000F49 RID: 3913
			// (get) Token: 0x06004CA7 RID: 19623 RVA: 0x000E3674 File Offset: 0x000E1874
			// (set) Token: 0x06004CA8 RID: 19624 RVA: 0x000E367C File Offset: 0x000E187C
			internal bool noDelay { get; private set; }

			// Token: 0x06004CA9 RID: 19625 RVA: 0x000E3688 File Offset: 0x000E1888
			internal void Initialize()
			{
				Array.Sort<BoundsAttackInfoSequence>(base.components, (BoundsAttackInfoSequence x, BoundsAttackInfoSequence y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
				this.noDelay = true;
				foreach (BoundsAttackInfoSequence boundsAttackInfoSequence in base.components)
				{
					if (boundsAttackInfoSequence._timeToTrigger > 0f)
					{
						this.noDelay = false;
					}
					boundsAttackInfoSequence.attackInfo.Initialize();
				}
			}

			// Token: 0x06004CAA RID: 19626 RVA: 0x000E36FC File Offset: 0x000E18FC
			internal void StopAllOperationsToOwner()
			{
				for (int i = 0; i < this._components.Length; i++)
				{
					this._components[i].attackInfo.operationsToOwner.StopAll();
				}
			}
		}
	}
}
