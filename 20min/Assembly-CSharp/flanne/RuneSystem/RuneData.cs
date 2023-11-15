using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000156 RID: 342
	[CreateAssetMenu(fileName = "RuneData", menuName = "RuneData")]
	public class RuneData : ScriptableObject
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00024D8C File Offset: 0x00022F8C
		public string nameString
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.nameStringID.key);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00024D9E File Offset: 0x00022F9E
		public virtual string description
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.descriptionStringID.key);
			}
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00024DB0 File Offset: 0x00022FB0
		public void Apply(PlayerController player)
		{
			Object.Instantiate<GameObject>(this.runePrefab.gameObject).GetComponent<Rune>().Attach(player, this.level);
		}

		// Token: 0x04000684 RID: 1668
		public Sprite icon;

		// Token: 0x04000685 RID: 1669
		public LocalizedString nameStringID;

		// Token: 0x04000686 RID: 1670
		public LocalizedString descriptionStringID;

		// Token: 0x04000687 RID: 1671
		public int costPerLevel;

		// Token: 0x04000688 RID: 1672
		public Rune runePrefab;

		// Token: 0x04000689 RID: 1673
		[NonSerialized]
		public int level;
	}
}
