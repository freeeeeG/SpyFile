using System;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x0200046F RID: 1135
	[Serializable]
	public class DarkEnemyCandidate
	{
		// Token: 0x060015A8 RID: 5544 RVA: 0x0004400C File Offset: 0x0004220C
		public Character Get(Tier tier, System.Random random)
		{
			switch (tier)
			{
			case Tier.Low:
				if (this._lowTargets.Length == 0)
				{
					Debug.LogError("target count is 0");
				}
				return this._lowTargets.Random(random);
			case Tier.Middle:
				if (this._middleTargets.Length == 0)
				{
					Debug.LogError("target count is 0");
				}
				return this._middleTargets.Random(random);
			case Tier.High:
				if (this._highTargets.Length == 0)
				{
					Debug.LogError("target count is 0");
				}
				return this._highTargets.Random(random);
			default:
				return null;
			}
		}

		// Token: 0x040012EB RID: 4843
		[SerializeField]
		private Character[] _lowTargets;

		// Token: 0x040012EC RID: 4844
		[SerializeField]
		private Character[] _middleTargets;

		// Token: 0x040012ED RID: 4845
		[SerializeField]
		private Character[] _highTargets;
	}
}
