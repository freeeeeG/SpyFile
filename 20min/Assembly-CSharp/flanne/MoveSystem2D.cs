using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000092 RID: 146
	public class MoveSystem2D : MonoBehaviour
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x00019B79 File Offset: 0x00017D79
		private void Awake()
		{
			MoveSystem2D.moveComponents = new List<MoveComponent2D>();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00019B85 File Offset: 0x00017D85
		private void Start()
		{
			this.OP = ObjectPooler.SharedInstance;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00019B94 File Offset: 0x00017D94
		private void FixedUpdate()
		{
			for (int i = 0; i < MoveSystem2D.moveComponents.Count; i++)
			{
				if (MoveSystem2D.moveComponents[i].gameObject.activeSelf)
				{
					float num = MoveSystem2D.moveComponents[i].vector.magnitude;
					num *= Mathf.Exp(-MoveSystem2D.moveComponents[i].drag * Time.fixedDeltaTime);
					MoveSystem2D.moveComponents[i].vector = Mathf.Max(0f, num) * MoveSystem2D.moveComponents[i].vector.normalized;
					MoveSystem2D.moveComponents[i].Rb.MovePosition(MoveSystem2D.moveComponents[i].Rb.position + MoveSystem2D.moveComponents[i].vector * Time.fixedDeltaTime);
					if (MoveSystem2D.moveComponents[i].rotateTowardsMove)
					{
						Quaternion rotation = Quaternion.AngleAxis(Mathf.Atan2(MoveSystem2D.moveComponents[i].vector.y, MoveSystem2D.moveComponents[i].vector.x) * 57.29578f, Vector3.forward);
						MoveSystem2D.moveComponents[i].transform.rotation = rotation;
						if (MoveSystem2D.moveComponents[i].vector.x < 0f)
						{
							MoveSystem2D.moveComponents[i].transform.localScale = new Vector3(1f, -1f, 1f);
						}
						else
						{
							MoveSystem2D.moveComponents[i].transform.localScale = new Vector3(1f, 1f, 1f);
						}
					}
				}
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00019D6A File Offset: 0x00017F6A
		public static void Register(MoveComponent2D m)
		{
			MoveSystem2D.moveComponents.Add(m);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00019D77 File Offset: 0x00017F77
		public static void UnRegister(MoveComponent2D m)
		{
			MoveSystem2D.moveComponents.Remove(m);
		}

		// Token: 0x0400033E RID: 830
		public static List<MoveComponent2D> moveComponents;

		// Token: 0x0400033F RID: 831
		private ObjectPooler OP;
	}
}
