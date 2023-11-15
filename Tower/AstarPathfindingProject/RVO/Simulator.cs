using System;
using System.Collections.Generic;
using System.Threading;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000D7 RID: 215
	public class Simulator
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x0003BA0C File Offset: 0x00039C0C
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x0003BA14 File Offset: 0x00039C14
		public RVOQuadtree Quadtree { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x0003BA1D File Offset: 0x00039C1D
		public float DeltaTime
		{
			get
			{
				return this.deltaTime;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0003BA25 File Offset: 0x00039C25
		public bool Multithreading
		{
			get
			{
				return this.workers != null && this.workers.Length != 0;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0003BA3B File Offset: 0x00039C3B
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x0003BA43 File Offset: 0x00039C43
		public float DesiredDeltaTime
		{
			get
			{
				return this.desiredDeltaTime;
			}
			set
			{
				this.desiredDeltaTime = Math.Max(value, 0f);
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0003BA56 File Offset: 0x00039C56
		public List<Agent> GetAgents()
		{
			return this.agents;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0003BA5E File Offset: 0x00039C5E
		public List<ObstacleVertex> GetObstacles()
		{
			return this.obstacles;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0003BA68 File Offset: 0x00039C68
		public Simulator(int workers, bool doubleBuffering, MovementPlane movementPlane)
		{
			this.workers = new Simulator.Worker[workers];
			this.doubleBuffering = doubleBuffering;
			this.DesiredDeltaTime = 1f;
			this.movementPlane = movementPlane;
			this.Quadtree = new RVOQuadtree();
			for (int i = 0; i < workers; i++)
			{
				this.workers[i] = new Simulator.Worker(this);
			}
			this.agents = new List<Agent>();
			this.obstacles = new List<ObstacleVertex>();
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0003BB10 File Offset: 0x00039D10
		public void ClearAgents()
		{
			this.BlockUntilSimulationStepIsDone();
			for (int i = 0; i < this.agents.Count; i++)
			{
				this.agents[i].simulator = null;
			}
			this.agents.Clear();
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0003BB58 File Offset: 0x00039D58
		public void OnDestroy()
		{
			if (this.workers != null)
			{
				for (int i = 0; i < this.workers.Length; i++)
				{
					this.workers[i].Terminate();
				}
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0003BB90 File Offset: 0x00039D90
		public IAgent AddAgent(IAgent agent)
		{
			if (agent == null)
			{
				throw new ArgumentNullException("Agent must not be null");
			}
			Agent agent2 = agent as Agent;
			if (agent2 == null)
			{
				string str = "The agent must be of type Agent. Agent was of type ";
				Type type = agent.GetType();
				throw new ArgumentException(str + ((type != null) ? type.ToString() : null));
			}
			if (agent2.simulator != null && agent2.simulator == this)
			{
				throw new ArgumentException("The agent is already in the simulation");
			}
			if (agent2.simulator != null)
			{
				throw new ArgumentException("The agent is already added to another simulation");
			}
			agent2.simulator = this;
			this.BlockUntilSimulationStepIsDone();
			this.agents.Add(agent2);
			return agent;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0003BC20 File Offset: 0x00039E20
		[Obsolete("Use AddAgent(Vector2,float) instead")]
		public IAgent AddAgent(Vector3 position)
		{
			return this.AddAgent(new Vector2(position.x, position.z), position.y);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0003BC3F File Offset: 0x00039E3F
		public IAgent AddAgent(Vector2 position, float elevationCoordinate)
		{
			return this.AddAgent(new Agent(position, elevationCoordinate));
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0003BC50 File Offset: 0x00039E50
		public void RemoveAgent(IAgent agent)
		{
			if (agent == null)
			{
				throw new ArgumentNullException("Agent must not be null");
			}
			Agent agent2 = agent as Agent;
			if (agent2 == null)
			{
				string str = "The agent must be of type Agent. Agent was of type ";
				Type type = agent.GetType();
				throw new ArgumentException(str + ((type != null) ? type.ToString() : null));
			}
			if (agent2.simulator != this)
			{
				throw new ArgumentException("The agent is not added to this simulation");
			}
			this.BlockUntilSimulationStepIsDone();
			agent2.simulator = null;
			if (!this.agents.Remove(agent2))
			{
				throw new ArgumentException("Critical Bug! This should not happen. Please report this.");
			}
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0003BCD1 File Offset: 0x00039ED1
		public ObstacleVertex AddObstacle(ObstacleVertex v)
		{
			if (v == null)
			{
				throw new ArgumentNullException("Obstacle must not be null");
			}
			this.BlockUntilSimulationStepIsDone();
			this.obstacles.Add(v);
			this.UpdateObstacles();
			return v;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0003BCFA File Offset: 0x00039EFA
		public ObstacleVertex AddObstacle(Vector3[] vertices, float height, bool cycle = true)
		{
			return this.AddObstacle(vertices, height, Matrix4x4.identity, RVOLayer.DefaultObstacle, cycle);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0003BD0C File Offset: 0x00039F0C
		public ObstacleVertex AddObstacle(Vector3[] vertices, float height, Matrix4x4 matrix, RVOLayer layer = RVOLayer.DefaultObstacle, bool cycle = true)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("Vertices must not be null");
			}
			if (vertices.Length < 2)
			{
				throw new ArgumentException("Less than 2 vertices in an obstacle");
			}
			ObstacleVertex obstacleVertex = null;
			ObstacleVertex obstacleVertex2 = null;
			this.BlockUntilSimulationStepIsDone();
			for (int i = 0; i < vertices.Length; i++)
			{
				ObstacleVertex obstacleVertex3 = new ObstacleVertex
				{
					prev = obstacleVertex2,
					layer = layer,
					height = height
				};
				if (obstacleVertex == null)
				{
					obstacleVertex = obstacleVertex3;
				}
				else
				{
					obstacleVertex2.next = obstacleVertex3;
				}
				obstacleVertex2 = obstacleVertex3;
			}
			if (cycle)
			{
				obstacleVertex2.next = obstacleVertex;
				obstacleVertex.prev = obstacleVertex2;
			}
			this.UpdateObstacle(obstacleVertex, vertices, matrix);
			this.obstacles.Add(obstacleVertex);
			return obstacleVertex;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0003BDA4 File Offset: 0x00039FA4
		public ObstacleVertex AddObstacle(Vector3 a, Vector3 b, float height)
		{
			ObstacleVertex obstacleVertex = new ObstacleVertex();
			ObstacleVertex obstacleVertex2 = new ObstacleVertex();
			obstacleVertex.layer = RVOLayer.DefaultObstacle;
			obstacleVertex2.layer = RVOLayer.DefaultObstacle;
			obstacleVertex.prev = obstacleVertex2;
			obstacleVertex2.prev = obstacleVertex;
			obstacleVertex.next = obstacleVertex2;
			obstacleVertex2.next = obstacleVertex;
			obstacleVertex.position = a;
			obstacleVertex2.position = b;
			obstacleVertex.height = height;
			obstacleVertex2.height = height;
			obstacleVertex2.ignore = true;
			obstacleVertex.dir = new Vector2(b.x - a.x, b.z - a.z).normalized;
			obstacleVertex2.dir = -obstacleVertex.dir;
			this.BlockUntilSimulationStepIsDone();
			this.obstacles.Add(obstacleVertex);
			this.UpdateObstacles();
			return obstacleVertex;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0003BE64 File Offset: 0x0003A064
		public void UpdateObstacle(ObstacleVertex obstacle, Vector3[] vertices, Matrix4x4 matrix)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("Vertices must not be null");
			}
			if (obstacle == null)
			{
				throw new ArgumentNullException("Obstacle must not be null");
			}
			if (vertices.Length < 2)
			{
				throw new ArgumentException("Less than 2 vertices in an obstacle");
			}
			bool flag = matrix == Matrix4x4.identity;
			this.BlockUntilSimulationStepIsDone();
			int i = 0;
			ObstacleVertex obstacleVertex = obstacle;
			while (i < vertices.Length)
			{
				obstacleVertex.position = (flag ? vertices[i] : matrix.MultiplyPoint3x4(vertices[i]));
				obstacleVertex = obstacleVertex.next;
				i++;
				if (obstacleVertex == obstacle || obstacleVertex == null)
				{
					obstacleVertex = obstacle;
					do
					{
						if (obstacleVertex.next == null)
						{
							obstacleVertex.dir = Vector2.zero;
						}
						else
						{
							Vector3 vector = obstacleVertex.next.position - obstacleVertex.position;
							obstacleVertex.dir = new Vector2(vector.x, vector.z).normalized;
						}
						obstacleVertex = obstacleVertex.next;
					}
					while (obstacleVertex != obstacle && obstacleVertex != null);
					this.ScheduleCleanObstacles();
					this.UpdateObstacles();
					return;
				}
			}
			Debug.DrawLine(obstacleVertex.prev.position, obstacleVertex.position, Color.red);
			throw new ArgumentException("Obstacle has more vertices than supplied for updating (" + vertices.Length.ToString() + " supplied)");
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0003BF92 File Offset: 0x0003A192
		private void ScheduleCleanObstacles()
		{
			this.doCleanObstacles = true;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0003BF9B File Offset: 0x0003A19B
		private void CleanObstacles()
		{
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0003BF9D File Offset: 0x0003A19D
		public void RemoveObstacle(ObstacleVertex v)
		{
			if (v == null)
			{
				throw new ArgumentNullException("Vertex must not be null");
			}
			this.BlockUntilSimulationStepIsDone();
			this.obstacles.Remove(v);
			this.UpdateObstacles();
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0003BFC6 File Offset: 0x0003A1C6
		public void UpdateObstacles()
		{
			this.doUpdateObstacles = true;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0003BFD0 File Offset: 0x0003A1D0
		private void BuildQuadtree()
		{
			this.Quadtree.Clear();
			if (this.agents.Count > 0)
			{
				Rect bounds = Rect.MinMaxRect(this.agents[0].position.x, this.agents[0].position.y, this.agents[0].position.x, this.agents[0].position.y);
				for (int i = 1; i < this.agents.Count; i++)
				{
					Vector2 position = this.agents[i].position;
					bounds = Rect.MinMaxRect(Mathf.Min(bounds.xMin, position.x), Mathf.Min(bounds.yMin, position.y), Mathf.Max(bounds.xMax, position.x), Mathf.Max(bounds.yMax, position.y));
				}
				this.Quadtree.SetBounds(bounds);
				for (int j = 0; j < this.agents.Count; j++)
				{
					this.Quadtree.Insert(this.agents[j]);
				}
			}
			this.Quadtree.CalculateSpeeds();
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0003C114 File Offset: 0x0003A314
		private void BlockUntilSimulationStepIsDone()
		{
			if (this.Multithreading && this.doubleBuffering)
			{
				for (int i = 0; i < this.workers.Length; i++)
				{
					this.workers[i].WaitOne();
				}
			}
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0003C154 File Offset: 0x0003A354
		private void PreCalculation()
		{
			for (int i = 0; i < this.agents.Count; i++)
			{
				this.agents[i].PreCalculation();
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0003C188 File Offset: 0x0003A388
		private void CleanAndUpdateObstaclesIfNecessary()
		{
			if (this.doCleanObstacles)
			{
				this.CleanObstacles();
				this.doCleanObstacles = false;
				this.doUpdateObstacles = true;
			}
			if (this.doUpdateObstacles)
			{
				this.doUpdateObstacles = false;
			}
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0003C1B8 File Offset: 0x0003A3B8
		public void Update()
		{
			if (this.lastStep < 0f)
			{
				this.lastStep = Time.time;
				this.deltaTime = this.DesiredDeltaTime;
			}
			if (Time.time - this.lastStep >= this.DesiredDeltaTime)
			{
				this.deltaTime = Time.time - this.lastStep;
				this.lastStep = Time.time;
				this.deltaTime = Math.Max(this.deltaTime, 0.0005f);
				if (this.Multithreading)
				{
					if (this.doubleBuffering)
					{
						for (int i = 0; i < this.workers.Length; i++)
						{
							this.workers[i].WaitOne();
						}
						for (int j = 0; j < this.agents.Count; j++)
						{
							this.agents[j].PostCalculation();
						}
					}
					this.PreCalculation();
					this.CleanAndUpdateObstaclesIfNecessary();
					this.BuildQuadtree();
					for (int k = 0; k < this.workers.Length; k++)
					{
						this.workers[k].start = k * this.agents.Count / this.workers.Length;
						this.workers[k].end = (k + 1) * this.agents.Count / this.workers.Length;
					}
					for (int l = 0; l < this.workers.Length; l++)
					{
						this.workers[l].Execute(1);
					}
					for (int m = 0; m < this.workers.Length; m++)
					{
						this.workers[m].WaitOne();
					}
					for (int n = 0; n < this.workers.Length; n++)
					{
						this.workers[n].Execute(0);
					}
					if (!this.doubleBuffering)
					{
						for (int num = 0; num < this.workers.Length; num++)
						{
							this.workers[num].WaitOne();
						}
						for (int num2 = 0; num2 < this.agents.Count; num2++)
						{
							this.agents[num2].PostCalculation();
						}
						return;
					}
				}
				else
				{
					this.PreCalculation();
					this.CleanAndUpdateObstaclesIfNecessary();
					this.BuildQuadtree();
					for (int num3 = 0; num3 < this.agents.Count; num3++)
					{
						this.agents[num3].BufferSwitch();
					}
					for (int num4 = 0; num4 < this.agents.Count; num4++)
					{
						this.agents[num4].CalculateNeighbours();
						this.agents[num4].CalculateVelocity(this.coroutineWorkerContext);
					}
					for (int num5 = 0; num5 < this.agents.Count; num5++)
					{
						this.agents[num5].PostCalculation();
					}
				}
			}
		}

		// Token: 0x0400054E RID: 1358
		private readonly bool doubleBuffering = true;

		// Token: 0x0400054F RID: 1359
		private float desiredDeltaTime = 0.05f;

		// Token: 0x04000550 RID: 1360
		private readonly Simulator.Worker[] workers;

		// Token: 0x04000551 RID: 1361
		private List<Agent> agents;

		// Token: 0x04000552 RID: 1362
		public List<ObstacleVertex> obstacles;

		// Token: 0x04000554 RID: 1364
		private float deltaTime;

		// Token: 0x04000555 RID: 1365
		private float lastStep = -99999f;

		// Token: 0x04000556 RID: 1366
		private bool doUpdateObstacles;

		// Token: 0x04000557 RID: 1367
		private bool doCleanObstacles;

		// Token: 0x04000558 RID: 1368
		public float symmetryBreakingBias = 0.1f;

		// Token: 0x04000559 RID: 1369
		public readonly MovementPlane movementPlane;

		// Token: 0x0400055A RID: 1370
		private Simulator.WorkerContext coroutineWorkerContext = new Simulator.WorkerContext();

		// Token: 0x02000172 RID: 370
		internal class WorkerContext
		{
			// Token: 0x0400083C RID: 2108
			public Agent.VOBuffer vos = new Agent.VOBuffer(16);

			// Token: 0x0400083D RID: 2109
			public const int KeepCount = 3;

			// Token: 0x0400083E RID: 2110
			public Vector2[] bestPos = new Vector2[3];

			// Token: 0x0400083F RID: 2111
			public float[] bestSizes = new float[3];

			// Token: 0x04000840 RID: 2112
			public float[] bestScores = new float[4];

			// Token: 0x04000841 RID: 2113
			public Vector2[] samplePos = new Vector2[50];

			// Token: 0x04000842 RID: 2114
			public float[] sampleSize = new float[50];
		}

		// Token: 0x02000173 RID: 371
		private class Worker
		{
			// Token: 0x06000B9A RID: 2970 RVA: 0x00049A6C File Offset: 0x00047C6C
			public Worker(Simulator sim)
			{
				this.simulator = sim;
				new Thread(new ThreadStart(this.Run))
				{
					IsBackground = true,
					Name = "RVO Simulator Thread"
				}.Start();
			}

			// Token: 0x06000B9B RID: 2971 RVA: 0x00049AD1 File Offset: 0x00047CD1
			public void Execute(int task)
			{
				this.task = task;
				this.waitFlag.Reset();
				this.runFlag.Set();
			}

			// Token: 0x06000B9C RID: 2972 RVA: 0x00049AF0 File Offset: 0x00047CF0
			public void WaitOne()
			{
				if (!this.terminate)
				{
					this.waitFlag.Wait();
				}
			}

			// Token: 0x06000B9D RID: 2973 RVA: 0x00049B05 File Offset: 0x00047D05
			public void Terminate()
			{
				this.WaitOne();
				this.terminate = true;
				this.Execute(-1);
			}

			// Token: 0x06000B9E RID: 2974 RVA: 0x00049B1C File Offset: 0x00047D1C
			public void Run()
			{
				this.runFlag.Wait();
				this.runFlag.Reset();
				while (!this.terminate)
				{
					try
					{
						List<Agent> agents = this.simulator.GetAgents();
						if (this.task == 0)
						{
							for (int i = this.start; i < this.end; i++)
							{
								agents[i].CalculateNeighbours();
								agents[i].CalculateVelocity(this.context);
							}
						}
						else if (this.task == 1)
						{
							for (int j = this.start; j < this.end; j++)
							{
								agents[j].BufferSwitch();
							}
						}
						else
						{
							if (this.task != 2)
							{
								Debug.LogError("Invalid Task Number: " + this.task.ToString());
								throw new Exception("Invalid Task Number: " + this.task.ToString());
							}
							this.simulator.BuildQuadtree();
						}
					}
					catch (Exception message)
					{
						Debug.LogError(message);
					}
					this.waitFlag.Set();
					this.runFlag.Wait();
					this.runFlag.Reset();
				}
			}

			// Token: 0x04000843 RID: 2115
			public int start;

			// Token: 0x04000844 RID: 2116
			public int end;

			// Token: 0x04000845 RID: 2117
			private readonly ManualResetEventSlim runFlag = new ManualResetEventSlim(false);

			// Token: 0x04000846 RID: 2118
			private readonly ManualResetEventSlim waitFlag = new ManualResetEventSlim(true);

			// Token: 0x04000847 RID: 2119
			private readonly Simulator simulator;

			// Token: 0x04000848 RID: 2120
			private int task;

			// Token: 0x04000849 RID: 2121
			private bool terminate;

			// Token: 0x0400084A RID: 2122
			private Simulator.WorkerContext context = new Simulator.WorkerContext();
		}
	}
}
