using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F79 RID: 3961
	public class CastAttackInfoSequence : MonoBehaviour
	{
		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06004CE1 RID: 19681 RVA: 0x000E42F9 File Offset: 0x000E24F9
		public float timeToTrigger
		{
			get
			{
				return this._timeToTrigger;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06004CE2 RID: 19682 RVA: 0x000E4301 File Offset: 0x000E2501
		public CastAttackInfo attackInfo
		{
			get
			{
				return this._attackInfo;
			}
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x000E4309 File Offset: 0x000E2509
		public override string ToString()
		{
			return string.Format("{0:0.##}s({1:0.##}f), {2}", this._timeToTrigger, this._timeToTrigger * 60f, this._attackInfo.GetAutoName());
		}

		// Token: 0x04003C80 RID: 15488
		[FrameTime]
		[SerializeField]
		private float _timeToTrigger;

		// Token: 0x04003C81 RID: 15489
		[Subcomponent(typeof(CastAttackInfo))]
		[SerializeField]
		private CastAttackInfo _attackInfo;

		// Token: 0x02000F7A RID: 3962
		[Serializable]
		public sealed class Subcomponents : SubcomponentArray<CastAttackInfoSequence>
		{
			// Token: 0x17000F55 RID: 3925
			// (get) Token: 0x06004CE5 RID: 19685 RVA: 0x000E433C File Offset: 0x000E253C
			// (set) Token: 0x06004CE6 RID: 19686 RVA: 0x000E4344 File Offset: 0x000E2544
			internal bool noDelay { get; private set; }

			// Token: 0x06004CE7 RID: 19687 RVA: 0x000E4350 File Offset: 0x000E2550
			internal void Initialize()
			{
				Array.Sort<CastAttackInfoSequence>(base.components, (CastAttackInfoSequence x, CastAttackInfoSequence y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
				this.noDelay = true;
				foreach (CastAttackInfoSequence castAttackInfoSequence in base.components)
				{
					if (castAttackInfoSequence._timeToTrigger > 0f)
					{
						this.noDelay = false;
					}
					castAttackInfoSequence.attackInfo.Initialize();
				}
			}

			// Token: 0x06004CE8 RID: 19688 RVA: 0x000E43C4 File Offset: 0x000E25C4
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
