using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F3 RID: 243
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_mine_bot_animation.php")]
	public class MineBotAnimation : VersionedMonoBehaviour
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x0004217B File Offset: 0x0004037B
		protected override void Awake()
		{
			base.Awake();
			this.ai = base.GetComponent<IAstarAI>();
			this.tr = base.GetComponent<Transform>();
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0004219C File Offset: 0x0004039C
		private void OnTargetReached()
		{
			if (this.endOfPathEffect != null && Vector3.Distance(this.tr.position, this.lastTarget) > 1f)
			{
				Object.Instantiate<GameObject>(this.endOfPathEffect, this.tr.position, this.tr.rotation);
				this.lastTarget = this.tr.position;
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00042208 File Offset: 0x00040408
		protected void Update()
		{
			if (this.ai.reachedEndOfPath)
			{
				if (!this.isAtDestination)
				{
					this.OnTargetReached();
				}
				this.isAtDestination = true;
			}
			else
			{
				this.isAtDestination = false;
			}
			Vector3 vector = this.tr.InverseTransformDirection(this.ai.velocity);
			vector.y = 0f;
			this.anim.SetFloat("NormalizedSpeed", vector.magnitude / this.anim.transform.lossyScale.x);
		}

		// Token: 0x04000638 RID: 1592
		public Animator anim;

		// Token: 0x04000639 RID: 1593
		public GameObject endOfPathEffect;

		// Token: 0x0400063A RID: 1594
		private bool isAtDestination;

		// Token: 0x0400063B RID: 1595
		private IAstarAI ai;

		// Token: 0x0400063C RID: 1596
		private Transform tr;

		// Token: 0x0400063D RID: 1597
		protected Vector3 lastTarget;
	}
}
