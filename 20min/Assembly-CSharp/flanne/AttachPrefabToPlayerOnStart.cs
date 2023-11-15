using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000060 RID: 96
	public class AttachPrefabToPlayerOnStart : MonoBehaviour
	{
		// Token: 0x06000460 RID: 1120 RVA: 0x00016A9C File Offset: 0x00014C9C
		private void Start()
		{
			PlayerController instance = PlayerController.Instance;
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab);
			gameObject.transform.SetParent(instance.transform);
			gameObject.transform.localPosition = this.posOffset;
			Orbital component = gameObject.GetComponent<Orbital>();
			if (component != null)
			{
				component.center = instance.transform;
				Orbital[] componentsInChildren = instance.GetComponentsInChildren<Orbital>();
				List<Orbital> list = new List<Orbital>();
				foreach (Orbital orbital in componentsInChildren)
				{
					if (orbital.tag == component.tag)
					{
						list.Add(orbital);
					}
				}
				Vector2 v = this.posOffset;
				for (int j = 0; j < list.Count; j++)
				{
					int num = j * (360 / list.Count);
					list[j].transform.localPosition = v.Rotate((float)num);
					if (!component.dontRotate)
					{
						list[j].transform.rotation = Quaternion.Euler(0f, 0f, (float)num);
					}
				}
			}
		}

		// Token: 0x04000255 RID: 597
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04000256 RID: 598
		[SerializeField]
		private Vector3 posOffset;
	}
}
