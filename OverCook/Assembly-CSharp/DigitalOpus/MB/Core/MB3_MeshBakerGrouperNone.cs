using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200005E RID: 94
	[Serializable]
	public class MB3_MeshBakerGrouperNone : MB3_MeshBakerGrouperCore
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0001B1F7 File Offset: 0x000195F7
		public MB3_MeshBakerGrouperNone(GrouperData d)
		{
			this.d = d;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0001B208 File Offset: 0x00019608
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Debug.Log("Filtering into groups none");
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			List<Renderer> list = new List<Renderer>();
			for (int i = 0; i < selection.Count; i++)
			{
				if (selection[i] != null)
				{
					list.Add(selection[i].GetComponent<Renderer>());
				}
			}
			dictionary.Add("MeshBaker", list);
			return dictionary;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0001B273 File Offset: 0x00019673
		public override void DrawGizmos(Bounds sourceObjectBounds)
		{
		}
	}
}
