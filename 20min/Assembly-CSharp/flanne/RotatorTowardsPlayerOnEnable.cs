using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000106 RID: 262
	public class RotatorTowardsPlayerOnEnable : MonoBehaviour
	{
		// Token: 0x06000777 RID: 1911 RVA: 0x000206F0 File Offset: 0x0001E8F0
		private void OnEnable()
		{
			Vector2 vector = PlayerController.Instance.transform.position - base.transform.position;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}
