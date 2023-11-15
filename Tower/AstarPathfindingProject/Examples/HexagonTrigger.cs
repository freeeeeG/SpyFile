using System;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding.Examples
{
	// Token: 0x020000E9 RID: 233
	[RequireComponent(typeof(Animator))]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_hexagon_trigger.php")]
	public class HexagonTrigger : MonoBehaviour
	{
		// Token: 0x060009DB RID: 2523 RVA: 0x00040F91 File Offset: 0x0003F191
		private void Awake()
		{
			this.anim = base.GetComponent<Animator>();
			this.button.interactable = false;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00040FAC File Offset: 0x0003F1AC
		private void OnTriggerEnter(Collider coll)
		{
			TurnBasedAI componentInParent = coll.GetComponentInParent<TurnBasedAI>();
			GraphNode node = AstarPath.active.GetNearest(base.transform.position).node;
			if (componentInParent != null && componentInParent.targetNode == node)
			{
				this.button.interactable = true;
				this.visible = true;
				this.anim.CrossFade("show", 0.1f);
			}
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00041015 File Offset: 0x0003F215
		private void OnTriggerExit(Collider coll)
		{
			if (coll.GetComponentInParent<TurnBasedAI>() != null && this.visible)
			{
				this.button.interactable = false;
				this.anim.CrossFade("hide", 0.1f);
			}
		}

		// Token: 0x04000600 RID: 1536
		public Button button;

		// Token: 0x04000601 RID: 1537
		private Animator anim;

		// Token: 0x04000602 RID: 1538
		private bool visible;
	}
}
