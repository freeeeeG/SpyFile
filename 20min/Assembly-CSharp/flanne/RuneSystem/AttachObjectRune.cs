using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x02000146 RID: 326
	public class AttachObjectRune : Rune
	{
		// Token: 0x06000876 RID: 2166 RVA: 0x00023C5C File Offset: 0x00021E5C
		protected override void Init()
		{
			for (int i = 0; i < this.amountToAttach; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.prefab);
				gameObject.transform.SetParent(this.player.transform);
				gameObject.transform.localPosition = this.posOffset;
				Orbital component = gameObject.GetComponent<Orbital>();
				if (component != null)
				{
					Orbital[] componentsInChildren = this.player.GetComponentsInChildren<Orbital>();
					List<Orbital> list = new List<Orbital>();
					foreach (Orbital orbital in componentsInChildren)
					{
						if (orbital.tag == component.tag)
						{
							list.Add(orbital);
						}
					}
					Vector2 v = list[0].transform.localPosition;
					for (int k = 1; k < list.Count; k++)
					{
						list[k].transform.localPosition = v.Rotate((float)(k * (360 / list.Count)));
					}
				}
			}
		}

		// Token: 0x04000641 RID: 1601
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04000642 RID: 1602
		[SerializeField]
		private int amountToAttach = 1;

		// Token: 0x04000643 RID: 1603
		[SerializeField]
		private Vector3 posOffset;
	}
}
