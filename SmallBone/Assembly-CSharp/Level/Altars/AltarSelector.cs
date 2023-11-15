using System;
using System.Linq;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars
{
	// Token: 0x0200060A RID: 1546
	public class AltarSelector : MonoBehaviour
	{
		// Token: 0x06001EFF RID: 7935 RVA: 0x0005DFB4 File Offset: 0x0005C1B4
		private void Awake()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + 898776742 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			Prop prop = null;
			AltarSelector.Altars.Property[] values = this._altars.values;
			double num = random.NextDouble() * (double)values.Sum((AltarSelector.Altars.Property a) => a.weight);
			for (int i = 0; i < values.Length; i++)
			{
				num -= (double)values[i].weight;
				if (num <= 0.0)
				{
					prop = values[i].altar;
					break;
				}
			}
			if (prop != null)
			{
				prop = UnityEngine.Object.Instantiate<Prop>(prop, base.transform.parent);
				prop.transform.position = base.transform.position;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001A2F RID: 6703
		private const int _randomSeed = 898776742;

		// Token: 0x04001A30 RID: 6704
		[SerializeField]
		private AltarSelector.Altars _altars;

		// Token: 0x0200060B RID: 1547
		[Serializable]
		private class Altars : ReorderableArray<AltarSelector.Altars.Property>
		{
			// Token: 0x0200060C RID: 1548
			[Serializable]
			internal class Property
			{
				// Token: 0x1700067C RID: 1660
				// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0005E0BF File Offset: 0x0005C2BF
				public float weight
				{
					get
					{
						return this._weight;
					}
				}

				// Token: 0x1700067D RID: 1661
				// (get) Token: 0x06001F03 RID: 7939 RVA: 0x0005E0C7 File Offset: 0x0005C2C7
				public Prop altar
				{
					get
					{
						return this._altar;
					}
				}

				// Token: 0x04001A31 RID: 6705
				[SerializeField]
				private float _weight;

				// Token: 0x04001A32 RID: 6706
				[SerializeField]
				private Prop _altar;
			}
		}
	}
}
