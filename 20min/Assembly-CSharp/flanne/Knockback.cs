using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C2 RID: 194
	public class Knockback : MonoBehaviour
	{
		// Token: 0x0600063D RID: 1597 RVA: 0x0001CA51 File Offset: 0x0001AC51
		private void Awake()
		{
			this.myMove = base.GetComponent<MoveComponent2D>();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001CA60 File Offset: 0x0001AC60
		private void OnCollisionEnter2D(Collision2D other)
		{
			MoveComponent2D component = other.gameObject.GetComponent<MoveComponent2D>();
			if (component == null || (component.knockbackImmune && !this.ignoreKBImmune) || other.gameObject.tag.Contains("Passive"))
			{
				return;
			}
			if (this.myMove != null)
			{
				component.vector = this.knockbackForce * this.myMove.vectorLastFrame.normalized;
				return;
			}
			Vector2 vector = other.transform.position - base.transform.position;
			component.vector = this.knockbackForce * vector.normalized;
		}

		// Token: 0x04000401 RID: 1025
		public float knockbackForce;

		// Token: 0x04000402 RID: 1026
		[SerializeField]
		private bool ignoreKBImmune;

		// Token: 0x04000403 RID: 1027
		private MoveComponent2D myMove;
	}
}
