using System;
using Pathfinding.RVO;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F1 RID: 241
	[RequireComponent(typeof(RVOController))]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_examples_1_1_manual_r_v_o_agent.php")]
	public class ManualRVOAgent : MonoBehaviour
	{
		// Token: 0x06000A0A RID: 2570 RVA: 0x000420D4 File Offset: 0x000402D4
		private void Awake()
		{
			this.rvo = base.GetComponent<RVOController>();
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000420E4 File Offset: 0x000402E4
		private void Update()
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			Vector3 vector = new Vector3(axis, 0f, axis2) * this.speed;
			this.rvo.velocity = vector;
			base.transform.position += vector * Time.deltaTime;
		}

		// Token: 0x04000632 RID: 1586
		private RVOController rvo;

		// Token: 0x04000633 RID: 1587
		public float speed = 1f;
	}
}
