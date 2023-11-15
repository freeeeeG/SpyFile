using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000DD RID: 221
	[ExecuteInEditMode]
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Simulator")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_r_v_o_1_1_r_v_o_simulator.php")]
	public class RVOSimulator : VersionedMonoBehaviour
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0003DBBA File Offset: 0x0003BDBA
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x0003DBC1 File Offset: 0x0003BDC1
		public static RVOSimulator active { get; private set; }

		// Token: 0x0600096C RID: 2412 RVA: 0x0003DBC9 File Offset: 0x0003BDC9
		public Simulator GetSimulator()
		{
			if (this.simulator == null)
			{
				this.Awake();
			}
			return this.simulator;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0003DBDF File Offset: 0x0003BDDF
		private void OnEnable()
		{
			RVOSimulator.active = this;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0003DBE8 File Offset: 0x0003BDE8
		protected override void Awake()
		{
			base.Awake();
			RVOSimulator.active = this;
			if (this.simulator == null && Application.isPlaying)
			{
				int workers = AstarPath.CalculateThreadCount(this.workerThreads);
				this.simulator = new Simulator(workers, this.doubleBuffering, this.movementPlane);
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0003DC34 File Offset: 0x0003BE34
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.desiredSimulationFPS < 1)
			{
				this.desiredSimulationFPS = 1;
			}
			Simulator simulator = this.GetSimulator();
			simulator.DesiredDeltaTime = 1f / (float)this.desiredSimulationFPS;
			simulator.symmetryBreakingBias = this.symmetryBreakingBias;
			simulator.Update();
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0003DC83 File Offset: 0x0003BE83
		private void OnDestroy()
		{
			RVOSimulator.active = null;
			if (this.simulator != null)
			{
				this.simulator.OnDestroy();
			}
		}

		// Token: 0x04000581 RID: 1409
		[Tooltip("Desired FPS for rvo simulation. It is usually not necessary to run a crowd simulation at a very high fps.\nUsually 10-30 fps is enough, but can be increased for better quality.\nThe rvo simulation will never run at a higher fps than the game")]
		public int desiredSimulationFPS = 20;

		// Token: 0x04000582 RID: 1410
		[Tooltip("Number of RVO worker threads. If set to None, no multithreading will be used.")]
		public ThreadCount workerThreads = ThreadCount.Two;

		// Token: 0x04000583 RID: 1411
		[Tooltip("Calculate local avoidance in between frames.\nThis can increase jitter in the agents' movement so use it only if you really need the performance boost. It will also reduce the responsiveness of the agents to the commands you send to them.")]
		public bool doubleBuffering;

		// Token: 0x04000584 RID: 1412
		[Tooltip("Bias agents to pass each other on the right side.\nIf the desired velocity of an agent puts it on a collision course with another agent or an obstacle its desired velocity will be rotated this number of radians (1 radian is approximately 57°) to the right. This helps to break up symmetries and makes it possible to resolve some situations much faster.\n\nWhen many agents have the same goal this can however have the side effect that the group clustered around the target point may as a whole start to spin around the target point.")]
		[Range(0f, 0.2f)]
		public float symmetryBreakingBias = 0.1f;

		// Token: 0x04000585 RID: 1413
		[Tooltip("Determines if the XY (2D) or XZ (3D) plane is used for movement")]
		public MovementPlane movementPlane;

		// Token: 0x04000586 RID: 1414
		public bool drawObstacles;

		// Token: 0x04000587 RID: 1415
		private Simulator simulator;
	}
}
