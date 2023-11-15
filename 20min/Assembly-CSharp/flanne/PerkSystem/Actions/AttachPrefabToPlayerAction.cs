using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001A8 RID: 424
	public class AttachPrefabToPlayerAction : Action
	{
		// Token: 0x060009EF RID: 2543 RVA: 0x00027510 File Offset: 0x00025710
		public override void Activate(GameObject target)
		{
			for (int i = 0; i < this.amountToAttach; i++)
			{
				PowerupReference componentInChildren = this.prefab.GetComponentInChildren<PowerupReference>();
				if (componentInChildren == null)
				{
					this.AttachPrefab(this.prefab);
				}
				else
				{
					PowerupReference[] componentsInChildren = target.GetComponentsInChildren<PowerupReference>();
					bool flag = false;
					foreach (PowerupReference powerupReference in componentsInChildren)
					{
						if (powerupReference.powerup == componentInChildren.powerup)
						{
							this.AttachPrefab(powerupReference.gameObject);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.AttachPrefab(this.prefab);
					}
				}
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x000275AC File Offset: 0x000257AC
		private void AttachPrefab(GameObject prefab)
		{
			PlayerController instance = PlayerController.Instance;
			GameObject gameObject = Object.Instantiate<GameObject>(prefab);
			gameObject.transform.SetParent(instance.transform);
			gameObject.transform.localPosition = this.posOffset;
			Orbital component = gameObject.GetComponent<Orbital>();
			if (component != null)
			{
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

		// Token: 0x04000707 RID: 1799
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04000708 RID: 1800
		[SerializeField]
		private int amountToAttach = 1;

		// Token: 0x04000709 RID: 1801
		[SerializeField]
		private Vector3 posOffset;
	}
}
