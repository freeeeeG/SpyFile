using System;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BD3 RID: 3027
	public sealed class WeightedDarkAbility : MonoBehaviour
	{
		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x000B56B7 File Offset: 0x000B38B7
		public DarkAbility key
		{
			get
			{
				return this._abilityComponent;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003E57 RID: 15959 RVA: 0x000B56BF File Offset: 0x000B38BF
		public int value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x000B56C7 File Offset: 0x000B38C7
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this._label))
			{
				return this._label;
			}
			return base.ToString();
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x000B56E3 File Offset: 0x000B38E3
		public void Initialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x000B56F0 File Offset: 0x000B38F0
		public void ResetValue()
		{
			this._value = 0;
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x000B56F9 File Offset: 0x000B38F9
		public bool Available(Character owner)
		{
			return Singleton<HardmodeManager>.Instance.currentLevel >= this._appearanceLevel.x && Singleton<HardmodeManager>.Instance.currentLevel <= this._appearanceLevel.y && this.key.Available(owner);
		}

		// Token: 0x04003029 RID: 12329
		[SerializeField]
		private string _label;

		// Token: 0x0400302A RID: 12330
		[Range(0f, 100f)]
		[SerializeField]
		private int _value;

		// Token: 0x0400302B RID: 12331
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2Int _appearanceLevel;

		// Token: 0x0400302C RID: 12332
		[SerializeField]
		private DarkAbility _abilityComponent;

		// Token: 0x02000BD4 RID: 3028
		[Serializable]
		public class Subcomponents : SubcomponentArray<WeightedDarkAbility>
		{
		}
	}
}
