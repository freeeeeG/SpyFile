using System;
using System.Collections.Generic;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000004 RID: 4
	[RequireComponent(typeof(HighlighterTrigger))]
	public class MousePointTriggerEvent : MonoBehaviour
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002228 File Offset: 0x00000428
		private void Start()
		{
			this.highlighterTrigger = base.GetComponent<HighlighterTrigger>();
			this.myColliders = new List<Collider>();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002244 File Offset: 0x00000444
		private void Update()
		{
			bool flag = false;
			Ray ray = this.myCamera.ScreenPointToRay(Input.mousePosition);
			if (this.myColliders.Count == 0)
			{
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				this.myColliders.AddRange(componentsInChildren);
			}
			using (List<Collider>.Enumerator enumerator = this.myColliders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RaycastHit raycastHit;
					if (enumerator.Current.Raycast(ray, out raycastHit, 1000f))
					{
						flag = true;
					}
				}
			}
			this.highlighterTrigger.ChangeTriggeringState(flag);
			if (flag && Input.GetMouseButtonDown(0))
			{
				this.highlighterTrigger.TriggerHit();
			}
		}

		// Token: 0x04000003 RID: 3
		private List<Collider> myColliders;

		// Token: 0x04000004 RID: 4
		private HighlighterTrigger highlighterTrigger;

		// Token: 0x04000005 RID: 5
		public Camera myCamera;
	}
}
