using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000141 RID: 321
	public class Vacuum : MonoBehaviour
	{
		// Token: 0x0600085F RID: 2143 RVA: 0x000233E9 File Offset: 0x000215E9
		private void OnEnable()
		{
			this._inRangeMoveComponents = new List<MoveComponent2D>();
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000233F8 File Offset: 0x000215F8
		private void Update()
		{
			for (int i = 0; i < this._inRangeMoveComponents.Count; i++)
			{
				if (!this._inRangeMoveComponents[i].knockbackImmune && this._inRangeMoveComponents[i].gameObject.activeSelf)
				{
					Vector2 b = (base.transform.position - this._inRangeMoveComponents[i].transform.position).normalized * this.vacuumStrength * Time.deltaTime;
					this._inRangeMoveComponents[i].vector += b;
				}
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000234B4 File Offset: 0x000216B4
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				MoveComponent2D component = other.gameObject.GetComponent<MoveComponent2D>();
				if (component != null)
				{
					this._inRangeMoveComponents.Add(component);
				}
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x000234FC File Offset: 0x000216FC
		private void OnCollisionExit2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				MoveComponent2D component = other.gameObject.GetComponent<MoveComponent2D>();
				if (component != null)
				{
					this._inRangeMoveComponents.Remove(component);
				}
			}
		}

		// Token: 0x0400062F RID: 1583
		[SerializeField]
		private string hitTag;

		// Token: 0x04000630 RID: 1584
		[SerializeField]
		private float vacuumStrength;

		// Token: 0x04000631 RID: 1585
		private List<MoveComponent2D> _inRangeMoveComponents;
	}
}
