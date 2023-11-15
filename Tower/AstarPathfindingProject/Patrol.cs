using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000005 RID: 5
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_patrol.php")]
	public class Patrol : VersionedMonoBehaviour
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00003472 File Offset: 0x00001672
		protected override void Awake()
		{
			base.Awake();
			this.agent = base.GetComponent<IAstarAI>();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003488 File Offset: 0x00001688
		private void Update()
		{
			if (this.targets.Length == 0)
			{
				return;
			}
			bool flag = false;
			if (this.agent.reachedEndOfPath && !this.agent.pathPending && float.IsPositiveInfinity(this.switchTime))
			{
				this.switchTime = Time.time + this.delay;
			}
			if (Time.time >= this.switchTime)
			{
				this.index++;
				flag = true;
				this.switchTime = float.PositiveInfinity;
			}
			this.index %= this.targets.Length;
			this.agent.destination = this.targets[this.index].position;
			if (flag)
			{
				this.agent.SearchPath();
			}
		}

		// Token: 0x0400003F RID: 63
		public Transform[] targets;

		// Token: 0x04000040 RID: 64
		public float delay;

		// Token: 0x04000041 RID: 65
		private int index;

		// Token: 0x04000042 RID: 66
		private IAstarAI agent;

		// Token: 0x04000043 RID: 67
		private float switchTime = float.PositiveInfinity;
	}
}
