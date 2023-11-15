using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000EA RID: 234
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(SingleNodeBlocker))]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_turn_based_door.php")]
	public class TurnBasedDoor : MonoBehaviour
	{
		// Token: 0x060009DF RID: 2527 RVA: 0x00041056 File Offset: 0x0003F256
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
			this.blocker = base.GetComponent<SingleNodeBlocker>();
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00041070 File Offset: 0x0003F270
		private void Start()
		{
			this.blocker.BlockAtCurrentPosition();
			this.animator.CrossFade("close", 0.2f);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00041092 File Offset: 0x0003F292
		public void Close()
		{
			base.StartCoroutine(this.WaitAndClose());
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x000410A1 File Offset: 0x0003F2A1
		private IEnumerator WaitAndClose()
		{
			List<SingleNodeBlocker> selector = new List<SingleNodeBlocker>
			{
				this.blocker
			};
			GraphNode node = AstarPath.active.GetNearest(base.transform.position).node;
			if (this.blocker.manager.NodeContainsAnyExcept(node, selector))
			{
				this.animator.CrossFade("blocked", 0.2f);
			}
			while (this.blocker.manager.NodeContainsAnyExcept(node, selector))
			{
				yield return null;
			}
			this.open = false;
			this.animator.CrossFade("close", 0.2f);
			this.blocker.BlockAtCurrentPosition();
			yield break;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000410B0 File Offset: 0x0003F2B0
		public void Open()
		{
			base.StopAllCoroutines();
			this.animator.CrossFade("open", 0.2f);
			this.open = true;
			this.blocker.Unblock();
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000410DF File Offset: 0x0003F2DF
		public void Toggle()
		{
			if (this.open)
			{
				this.Close();
				return;
			}
			this.Open();
		}

		// Token: 0x04000603 RID: 1539
		private Animator animator;

		// Token: 0x04000604 RID: 1540
		private SingleNodeBlocker blocker;

		// Token: 0x04000605 RID: 1541
		private bool open;
	}
}
