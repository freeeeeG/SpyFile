using System;
using System.Linq;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005DF RID: 1503
	public class FieldNpcSelector : MonoBehaviour
	{
		// Token: 0x06001E11 RID: 7697 RVA: 0x0005B810 File Offset: 0x00059A10
		private void Awake()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			System.Random random = new System.Random((int)(GameData.Save.instance.randomSeed + 699075432 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
			this._cage = base.GetComponentInParent<Cage>();
			FieldNpc fieldNpc = null;
			FieldNpcSelector.Npcs.Property[] values = this._npcs.values;
			double num = random.NextDouble() * (double)values.Sum((FieldNpcSelector.Npcs.Property a) => a.weight);
			int i = 0;
			while (i < values.Length)
			{
				FieldNpcSelector.Npcs.Property property = values[i];
				num -= (double)property.weight;
				if (num <= 0.0 && (property.npc == null || !property.npc.encountered))
				{
					fieldNpc = property.npc;
					if (property.bigCage)
					{
						this._cage.OverrideProp(this._bigProp, this._bigPropBehind, this._bigBehindWreck);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (fieldNpc == null || !Map.TestingTool.fieldNPC)
			{
				this._cage.Destroy();
				this._cage.gameObject.SetActive(false);
			}
			else
			{
				fieldNpc = UnityEngine.Object.Instantiate<FieldNpc>(fieldNpc, base.transform);
				fieldNpc.transform.position = base.transform.position;
				fieldNpc.SetCage(this._cage);
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x04001962 RID: 6498
		private const int _randomSeed = 699075432;

		// Token: 0x04001963 RID: 6499
		[SerializeField]
		private FieldNpcSelector.Npcs _npcs;

		// Token: 0x04001964 RID: 6500
		[SerializeField]
		private Prop _bigProp;

		// Token: 0x04001965 RID: 6501
		[SerializeField]
		private SpriteRenderer _bigPropBehind;

		// Token: 0x04001966 RID: 6502
		[SerializeField]
		private Sprite _bigBehindWreck;

		// Token: 0x04001967 RID: 6503
		private Cage _cage;

		// Token: 0x020005E0 RID: 1504
		[Serializable]
		private class Npcs : ReorderableArray<FieldNpcSelector.Npcs.Property>
		{
			// Token: 0x020005E1 RID: 1505
			[Serializable]
			internal class Property
			{
				// Token: 0x1700063A RID: 1594
				// (get) Token: 0x06001E14 RID: 7700 RVA: 0x0005B993 File Offset: 0x00059B93
				public bool bigCage
				{
					get
					{
						return this._bigCage;
					}
				}

				// Token: 0x1700063B RID: 1595
				// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0005B99B File Offset: 0x00059B9B
				public float weight
				{
					get
					{
						return this._weight;
					}
				}

				// Token: 0x1700063C RID: 1596
				// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0005B9A3 File Offset: 0x00059BA3
				public FieldNpc npc
				{
					get
					{
						return this._npc;
					}
				}

				// Token: 0x04001968 RID: 6504
				[SerializeField]
				private bool _bigCage;

				// Token: 0x04001969 RID: 6505
				[SerializeField]
				private float _weight;

				// Token: 0x0400196A RID: 6506
				[SerializeField]
				private FieldNpc _npc;
			}
		}
	}
}
