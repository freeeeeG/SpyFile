using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090C RID: 2316
public class DiseaseVisualization : ScriptableObject
{
	// Token: 0x06004329 RID: 17193 RVA: 0x00178254 File Offset: 0x00176454
	public DiseaseVisualization.Info GetInfo(HashedString id)
	{
		foreach (DiseaseVisualization.Info info in this.info)
		{
			if (id == info.name)
			{
				return info;
			}
		}
		return default(DiseaseVisualization.Info);
	}

	// Token: 0x04002BC5 RID: 11205
	public Sprite overlaySprite;

	// Token: 0x04002BC6 RID: 11206
	public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();

	// Token: 0x0200175D RID: 5981
	[Serializable]
	public struct Info
	{
		// Token: 0x06008E2A RID: 36394 RVA: 0x0031EC4A File Offset: 0x0031CE4A
		public Info(string name)
		{
			this.name = name;
			this.overlayColourName = "germFoodPoisoning";
		}

		// Token: 0x04006E82 RID: 28290
		public string name;

		// Token: 0x04006E83 RID: 28291
		public string overlayColourName;
	}
}
