using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000244 RID: 580
	public class AttachVisionDamage : MonoBehaviour
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0002E3CF File Offset: 0x0002C5CF
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0002E3D7 File Offset: 0x0002C5D7
		public PersistentHarm visionDamage { get; private set; }

		// Token: 0x06000CAF RID: 3247 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
		private void Start()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerVision");
			PersistentHarm componentInChildren = gameObject.GetComponentInChildren<PersistentHarm>();
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.visionDamagePrefab.gameObject);
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.SetActive(true);
			this.visionDamage = gameObject2.GetComponent<PersistentHarm>();
			if (componentInChildren != null)
			{
				this.visionDamage.damageAmount = componentInChildren.damageAmount;
				this.visionDamage.tickRate = componentInChildren.tickRate;
			}
		}

		// Token: 0x040008E4 RID: 2276
		[SerializeField]
		private PersistentHarm visionDamagePrefab;
	}
}
