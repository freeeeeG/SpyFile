using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x0200019E RID: 414
	public class AddShooterAction : Action
	{
		// Token: 0x060009D9 RID: 2521 RVA: 0x00027334 File Offset: 0x00025534
		public override void Activate(GameObject target)
		{
			Gun componentInChildren = target.GetComponentInChildren<Gun>();
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab);
			gameObject.transform.SetParent(componentInChildren.transform);
			gameObject.transform.localPosition = Vector2.zero;
			Shooter componentInChildren2 = gameObject.GetComponentInChildren<Shooter>();
			componentInChildren.AddShooter(componentInChildren2);
		}

		// Token: 0x040006FD RID: 1789
		[SerializeField]
		private GameObject prefab;
	}
}
