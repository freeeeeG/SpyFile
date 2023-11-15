using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001B3 RID: 435
	public class IncrementHaloPieceAction : Action
	{
		// Token: 0x06000A08 RID: 2568 RVA: 0x00027880 File Offset: 0x00025A80
		public override void Activate(GameObject target)
		{
			ShanasHalo componentInChildren = target.GetComponentInChildren<ShanasHalo>();
			if (componentInChildren == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.haloManagerPrefab);
				gameObject.transform.SetParent(target.transform);
				gameObject.transform.localPosition = Vector3.zero;
				return;
			}
			componentInChildren.CollectPiece();
		}

		// Token: 0x04000715 RID: 1813
		[SerializeField]
		private GameObject haloManagerPrefab;
	}
}
