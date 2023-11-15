using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000EC RID: 236
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_example_mover.php")]
	public class ExampleMover : MonoBehaviour
	{
		// Token: 0x060009F0 RID: 2544 RVA: 0x000413A3 File Offset: 0x0003F5A3
		private void Awake()
		{
			this.agent = base.GetComponent<RVOExampleAgent>();
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x000413B1 File Offset: 0x0003F5B1
		private void Start()
		{
			this.agent.SetTarget(this.target.position);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x000413C9 File Offset: 0x0003F5C9
		private void LateUpdate()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				this.agent.SetTarget(this.target.position);
			}
		}

		// Token: 0x0400060D RID: 1549
		private RVOExampleAgent agent;

		// Token: 0x0400060E RID: 1550
		public Transform target;
	}
}
