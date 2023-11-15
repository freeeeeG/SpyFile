using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO.Sampled
{
	// Token: 0x020000DF RID: 223
	public class Agent : IAgent
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0003DE58 File Offset: 0x0003C058
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0003DE60 File Offset: 0x0003C060
		public Vector2 Position { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0003DE69 File Offset: 0x0003C069
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0003DE71 File Offset: 0x0003C071
		public float ElevationCoordinate { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0003DE7A File Offset: 0x0003C07A
		// (set) Token: 0x0600097E RID: 2430 RVA: 0x0003DE82 File Offset: 0x0003C082
		public Vector2 CalculatedTargetPoint { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0003DE8B File Offset: 0x0003C08B
		// (set) Token: 0x06000980 RID: 2432 RVA: 0x0003DE93 File Offset: 0x0003C093
		public float CalculatedSpeed { get; private set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x0003DE9C File Offset: 0x0003C09C
		// (set) Token: 0x06000982 RID: 2434 RVA: 0x0003DEA4 File Offset: 0x0003C0A4
		public bool Locked { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0003DEAD File Offset: 0x0003C0AD
		// (set) Token: 0x06000984 RID: 2436 RVA: 0x0003DEB5 File Offset: 0x0003C0B5
		public float Radius { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0003DEBE File Offset: 0x0003C0BE
		// (set) Token: 0x06000986 RID: 2438 RVA: 0x0003DEC6 File Offset: 0x0003C0C6
		public float Height { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x0003DECF File Offset: 0x0003C0CF
		// (set) Token: 0x06000988 RID: 2440 RVA: 0x0003DED7 File Offset: 0x0003C0D7
		public float AgentTimeHorizon { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0003DEE0 File Offset: 0x0003C0E0
		// (set) Token: 0x0600098A RID: 2442 RVA: 0x0003DEE8 File Offset: 0x0003C0E8
		public float ObstacleTimeHorizon { get; set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0003DEF1 File Offset: 0x0003C0F1
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0003DEF9 File Offset: 0x0003C0F9
		public int MaxNeighbours { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0003DF02 File Offset: 0x0003C102
		// (set) Token: 0x0600098E RID: 2446 RVA: 0x0003DF0A File Offset: 0x0003C10A
		public int NeighbourCount { get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600098F RID: 2447 RVA: 0x0003DF13 File Offset: 0x0003C113
		// (set) Token: 0x06000990 RID: 2448 RVA: 0x0003DF1B File Offset: 0x0003C11B
		public RVOLayer Layer { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0003DF24 File Offset: 0x0003C124
		// (set) Token: 0x06000992 RID: 2450 RVA: 0x0003DF2C File Offset: 0x0003C12C
		public RVOLayer CollidesWith { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0003DF35 File Offset: 0x0003C135
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x0003DF3D File Offset: 0x0003C13D
		public bool DebugDraw
		{
			get
			{
				return this.debugDraw;
			}
			set
			{
				this.debugDraw = (value && this.simulator != null && !this.simulator.Multithreading);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0003DF61 File Offset: 0x0003C161
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0003DF69 File Offset: 0x0003C169
		public float Priority { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0003DF72 File Offset: 0x0003C172
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0003DF7A File Offset: 0x0003C17A
		public Action PreCalculationCallback { private get; set; }

		// Token: 0x06000999 RID: 2457 RVA: 0x0003DF83 File Offset: 0x0003C183
		public void SetTarget(Vector2 targetPoint, float desiredSpeed, float maxSpeed)
		{
			maxSpeed = Math.Max(maxSpeed, 0f);
			desiredSpeed = Math.Min(Math.Max(desiredSpeed, 0f), maxSpeed);
			this.nextTargetPoint = targetPoint;
			this.nextDesiredSpeed = desiredSpeed;
			this.nextMaxSpeed = maxSpeed;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0003DFBA File Offset: 0x0003C1BA
		public void SetCollisionNormal(Vector2 normal)
		{
			this.collisionNormal = normal;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0003DFC4 File Offset: 0x0003C1C4
		public void ForceSetVelocity(Vector2 velocity)
		{
			this.nextTargetPoint = (this.CalculatedTargetPoint = this.position + velocity * 1000f);
			this.nextDesiredSpeed = (this.CalculatedSpeed = velocity.magnitude);
			this.manuallyControlled = true;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0003E013 File Offset: 0x0003C213
		public List<ObstacleVertex> NeighbourObstacles
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0003E018 File Offset: 0x0003C218
		public Agent(Vector2 pos, float elevationCoordinate)
		{
			this.AgentTimeHorizon = 2f;
			this.ObstacleTimeHorizon = 2f;
			this.Height = 5f;
			this.Radius = 5f;
			this.MaxNeighbours = 10;
			this.Locked = false;
			this.Position = pos;
			this.ElevationCoordinate = elevationCoordinate;
			this.Layer = RVOLayer.DefaultAgent;
			this.CollidesWith = (RVOLayer)(-1);
			this.Priority = 0.5f;
			this.CalculatedTargetPoint = pos;
			this.CalculatedSpeed = 0f;
			this.SetTarget(pos, 0f, 0f);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0003E0DC File Offset: 0x0003C2DC
		public void BufferSwitch()
		{
			this.radius = this.Radius;
			this.height = this.Height;
			this.maxSpeed = this.nextMaxSpeed;
			this.desiredSpeed = this.nextDesiredSpeed;
			this.agentTimeHorizon = this.AgentTimeHorizon;
			this.obstacleTimeHorizon = this.ObstacleTimeHorizon;
			this.maxNeighbours = this.MaxNeighbours;
			this.locked = (this.Locked && !this.manuallyControlled);
			this.position = this.Position;
			this.elevationCoordinate = this.ElevationCoordinate;
			this.collidesWith = this.CollidesWith;
			this.layer = this.Layer;
			if (this.locked)
			{
				this.desiredTargetPointInVelocitySpace = this.position;
				this.desiredVelocity = (this.currentVelocity = Vector2.zero);
				return;
			}
			this.desiredTargetPointInVelocitySpace = this.nextTargetPoint - this.position;
			this.currentVelocity = (this.CalculatedTargetPoint - this.position).normalized * this.CalculatedSpeed;
			this.desiredVelocity = this.desiredTargetPointInVelocitySpace.normalized * this.desiredSpeed;
			if (this.collisionNormal != Vector2.zero)
			{
				this.collisionNormal.Normalize();
				float num = Vector2.Dot(this.currentVelocity, this.collisionNormal);
				if (num < 0f)
				{
					this.currentVelocity -= this.collisionNormal * num;
				}
				this.collisionNormal = Vector2.zero;
			}
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0003E26C File Offset: 0x0003C46C
		public void PreCalculation()
		{
			if (this.PreCalculationCallback != null)
			{
				this.PreCalculationCallback();
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0003E284 File Offset: 0x0003C484
		public void PostCalculation()
		{
			if (!this.manuallyControlled)
			{
				this.CalculatedTargetPoint = this.calculatedTargetPoint;
				this.CalculatedSpeed = this.calculatedSpeed;
			}
			List<ObstacleVertex> list = this.obstaclesBuffered;
			this.obstaclesBuffered = this.obstacles;
			this.obstacles = list;
			this.manuallyControlled = false;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0003E2D4 File Offset: 0x0003C4D4
		public void CalculateNeighbours()
		{
			this.neighbours.Clear();
			this.neighbourDists.Clear();
			if (this.MaxNeighbours > 0 && !this.locked)
			{
				this.simulator.Quadtree.Query(this.position, this.maxSpeed, this.agentTimeHorizon, this.radius, this);
			}
			this.NeighbourCount = this.neighbours.Count;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003E342 File Offset: 0x0003C542
		private static float Sqr(float x)
		{
			return x * x;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0003E348 File Offset: 0x0003C548
		internal float InsertAgentNeighbour(Agent agent, float rangeSq)
		{
			if (this == agent || (agent.layer & this.collidesWith) == (RVOLayer)0)
			{
				return rangeSq;
			}
			float sqrMagnitude = (agent.position - this.position).sqrMagnitude;
			if (sqrMagnitude < rangeSq)
			{
				if (this.neighbours.Count < this.maxNeighbours)
				{
					this.neighbours.Add(null);
					this.neighbourDists.Add(float.PositiveInfinity);
				}
				int num = this.neighbours.Count - 1;
				if (sqrMagnitude < this.neighbourDists[num])
				{
					while (num != 0 && sqrMagnitude < this.neighbourDists[num - 1])
					{
						this.neighbours[num] = this.neighbours[num - 1];
						this.neighbourDists[num] = this.neighbourDists[num - 1];
						num--;
					}
					this.neighbours[num] = agent;
					this.neighbourDists[num] = sqrMagnitude;
				}
				if (this.neighbours.Count == this.maxNeighbours)
				{
					rangeSq = this.neighbourDists[this.neighbourDists.Count - 1];
				}
			}
			return rangeSq;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0003E46D File Offset: 0x0003C66D
		private static Vector3 FromXZ(Vector2 p)
		{
			return new Vector3(p.x, 0f, p.y);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0003E485 File Offset: 0x0003C685
		private static Vector2 ToXZ(Vector3 p)
		{
			return new Vector2(p.x, p.z);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0003E498 File Offset: 0x0003C698
		private Vector2 To2D(Vector3 p, out float elevation)
		{
			if (this.simulator.movementPlane == MovementPlane.XY)
			{
				elevation = -p.z;
				return new Vector2(p.x, p.y);
			}
			elevation = p.y;
			return new Vector2(p.x, p.z);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0003E4E8 File Offset: 0x0003C6E8
		private static void DrawVO(Vector2 circleCenter, float radius, Vector2 origin)
		{
			float num = Mathf.Atan2((origin - circleCenter).y, (origin - circleCenter).x);
			float num2 = radius / (origin - circleCenter).magnitude;
			float num3 = (num2 <= 1f) ? Mathf.Abs(Mathf.Acos(num2)) : 0f;
			Draw.Debug.CircleXZ(Agent.FromXZ(circleCenter), radius, Color.black, num - num3, num + num3);
			Vector2 vector = new Vector2(Mathf.Cos(num - num3), Mathf.Sin(num - num3)) * radius;
			Vector2 vector2 = new Vector2(Mathf.Cos(num + num3), Mathf.Sin(num + num3)) * radius;
			Vector2 p = -new Vector2(-vector.y, vector.x);
			Vector2 p2 = new Vector2(-vector2.y, vector2.x);
			vector += circleCenter;
			vector2 += circleCenter;
			Debug.DrawRay(Agent.FromXZ(vector), Agent.FromXZ(p).normalized * 100f, Color.black);
			Debug.DrawRay(Agent.FromXZ(vector2), Agent.FromXZ(p2).normalized * 100f, Color.black);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003E62C File Offset: 0x0003C82C
		internal void CalculateVelocity(Simulator.WorkerContext context)
		{
			if (this.manuallyControlled)
			{
				return;
			}
			if (this.locked)
			{
				this.calculatedSpeed = 0f;
				this.calculatedTargetPoint = this.position;
				return;
			}
			Agent.VOBuffer vos = context.vos;
			vos.Clear();
			this.GenerateObstacleVOs(vos);
			this.GenerateNeighbourAgentVOs(vos);
			if (!Agent.BiasDesiredVelocity(vos, ref this.desiredVelocity, ref this.desiredTargetPointInVelocitySpace, this.simulator.symmetryBreakingBias))
			{
				this.calculatedTargetPoint = this.desiredTargetPointInVelocitySpace + this.position;
				this.calculatedSpeed = this.desiredSpeed;
				if (this.DebugDraw)
				{
					Draw.Debug.CrossXZ(Agent.FromXZ(this.calculatedTargetPoint), Color.white, 1f);
				}
				return;
			}
			Vector2 vector = Vector2.zero;
			vector = this.GradientDescent(vos, this.currentVelocity, this.desiredVelocity);
			if (this.DebugDraw)
			{
				Draw.Debug.CrossXZ(Agent.FromXZ(vector + this.position), Color.white, 1f);
			}
			this.calculatedTargetPoint = this.position + vector;
			this.calculatedSpeed = Mathf.Min(vector.magnitude, this.maxSpeed);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0003E758 File Offset: 0x0003C958
		private static Color Rainbow(float v)
		{
			Color color = new Color(v, 0f, 0f);
			if (color.r > 1f)
			{
				color.g = color.r - 1f;
				color.r = 1f;
			}
			if (color.g > 1f)
			{
				color.b = color.g - 1f;
				color.g = 1f;
			}
			return color;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0003E7D0 File Offset: 0x0003C9D0
		private void GenerateObstacleVOs(Agent.VOBuffer vos)
		{
			float num = this.maxSpeed * this.obstacleTimeHorizon;
			for (int i = 0; i < this.simulator.obstacles.Count; i++)
			{
				ObstacleVertex obstacleVertex = this.simulator.obstacles[i];
				ObstacleVertex obstacleVertex2 = obstacleVertex;
				do
				{
					if (obstacleVertex2.ignore || (obstacleVertex2.layer & this.collidesWith) == (RVOLayer)0)
					{
						obstacleVertex2 = obstacleVertex2.next;
					}
					else
					{
						float a;
						Vector2 vector = this.To2D(obstacleVertex2.position, out a);
						float b;
						Vector2 vector2 = this.To2D(obstacleVertex2.next.position, out b);
						Vector2 normalized = (vector2 - vector).normalized;
						float num2 = Agent.VO.SignedDistanceFromLine(vector, normalized, this.position);
						if (num2 >= -0.01f && num2 < num)
						{
							float t = Vector2.Dot(this.position - vector, vector2 - vector) / (vector2 - vector).sqrMagnitude;
							float num3 = Mathf.Lerp(a, b, t);
							if ((Vector2.Lerp(vector, vector2, t) - this.position).sqrMagnitude < num * num && (this.simulator.movementPlane == MovementPlane.XY || (this.elevationCoordinate <= num3 + obstacleVertex2.height && this.elevationCoordinate + this.height >= num3)))
							{
								vos.Add(Agent.VO.SegmentObstacle(vector2 - this.position, vector - this.position, Vector2.zero, this.radius * 0.01f, 1f / this.ObstacleTimeHorizon, 1f / this.simulator.DeltaTime));
							}
						}
						obstacleVertex2 = obstacleVertex2.next;
					}
				}
				while (obstacleVertex2 != obstacleVertex && obstacleVertex2 != null && obstacleVertex2.next != null);
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0003E9A4 File Offset: 0x0003CBA4
		private void GenerateNeighbourAgentVOs(Agent.VOBuffer vos)
		{
			float num = 1f / this.agentTimeHorizon;
			Vector2 a = this.currentVelocity;
			for (int i = 0; i < this.neighbours.Count; i++)
			{
				Agent agent = this.neighbours[i];
				if (agent != this)
				{
					float num2 = Math.Min(this.elevationCoordinate + this.height, agent.elevationCoordinate + agent.height);
					float num3 = Math.Max(this.elevationCoordinate, agent.elevationCoordinate);
					if (num2 - num3 >= 0f)
					{
						float num4 = this.radius + agent.radius;
						Vector2 vector = agent.position - this.position;
						float num5;
						if (agent.locked || agent.manuallyControlled)
						{
							num5 = 1f;
						}
						else if (agent.Priority > 1E-05f || this.Priority > 1E-05f)
						{
							num5 = agent.Priority / (this.Priority + agent.Priority);
						}
						else
						{
							num5 = 0.5f;
						}
						Vector2 b = Vector2.Lerp(agent.currentVelocity, agent.desiredVelocity, 2f * num5 - 1f);
						Vector2 vector2 = Vector2.Lerp(a, b, num5);
						vos.Add(new Agent.VO(vector, vector2, num4, num, 1f / this.simulator.DeltaTime));
						if (this.DebugDraw)
						{
							Agent.DrawVO(this.position + vector * num + vector2, num4 * num, this.position + vector2);
						}
					}
				}
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0003EB30 File Offset: 0x0003CD30
		private Vector2 GradientDescent(Agent.VOBuffer vos, Vector2 sampleAround1, Vector2 sampleAround2)
		{
			float num;
			Vector2 vector = this.Trace(vos, sampleAround1, out num);
			if (this.DebugDraw)
			{
				Draw.Debug.CrossXZ(Agent.FromXZ(vector + this.position), Color.yellow, 0.5f);
			}
			float num2;
			Vector2 vector2 = this.Trace(vos, sampleAround2, out num2);
			if (this.DebugDraw)
			{
				Draw.Debug.CrossXZ(Agent.FromXZ(vector2 + this.position), Color.magenta, 0.5f);
			}
			if (num >= num2)
			{
				return vector2;
			}
			return vector;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0003EBB4 File Offset: 0x0003CDB4
		private static bool BiasDesiredVelocity(Agent.VOBuffer vos, ref Vector2 desiredVelocity, ref Vector2 targetPointInVelocitySpace, float maxBiasRadians)
		{
			float magnitude = desiredVelocity.magnitude;
			float num = 0f;
			for (int i = 0; i < vos.length; i++)
			{
				float b;
				vos.buffer[i].Gradient(desiredVelocity, out b);
				num = Mathf.Max(num, b);
			}
			bool result = num > 0f;
			if (magnitude < 0.001f)
			{
				return result;
			}
			float d = Mathf.Min(maxBiasRadians, num / magnitude);
			desiredVelocity += new Vector2(desiredVelocity.y, -desiredVelocity.x) * d;
			targetPointInVelocitySpace += new Vector2(targetPointInVelocitySpace.y, -targetPointInVelocitySpace.x) * d;
			return result;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0003EC78 File Offset: 0x0003CE78
		private Vector2 EvaluateGradient(Agent.VOBuffer vos, Vector2 p, out float value)
		{
			Vector2 vector = Vector2.zero;
			value = 0f;
			for (int i = 0; i < vos.length; i++)
			{
				float num;
				Vector2 vector2 = vos.buffer[i].ScaledGradient(p, out num);
				if (num > value)
				{
					value = num;
					vector = vector2;
				}
			}
			Vector2 a = this.desiredVelocity - p;
			float magnitude = a.magnitude;
			if (magnitude > 0.0001f)
			{
				vector += a * (0.1f / magnitude);
				value += magnitude * 0.1f;
			}
			float sqrMagnitude = p.sqrMagnitude;
			if (sqrMagnitude > this.desiredSpeed * this.desiredSpeed)
			{
				float num2 = Mathf.Sqrt(sqrMagnitude);
				if (num2 > this.maxSpeed)
				{
					value += 3f * (num2 - this.maxSpeed);
					vector -= 3f * (p / num2);
				}
				float num3 = 0.2f;
				value += num3 * (num2 - this.desiredSpeed);
				vector -= num3 * (p / num2);
			}
			return vector;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0003ED90 File Offset: 0x0003CF90
		private Vector2 Trace(Agent.VOBuffer vos, Vector2 p, out float score)
		{
			float num = Mathf.Max(this.radius, 0.2f * this.desiredSpeed);
			float num2 = float.PositiveInfinity;
			Vector2 result = p;
			for (int i = 0; i < 50; i++)
			{
				float num3 = 1f - (float)i / 50f;
				num3 = Agent.Sqr(num3) * num;
				float num4;
				Vector2 vector = this.EvaluateGradient(vos, p, out num4);
				if (num4 < num2)
				{
					num2 = num4;
					result = p;
				}
				vector.Normalize();
				vector *= num3;
				Vector2 a = p;
				p += vector;
				if (this.DebugDraw)
				{
					Debug.DrawLine(Agent.FromXZ(a + this.position), Agent.FromXZ(p + this.position), Agent.Rainbow((float)i * 0.1f) * new Color(1f, 1f, 1f, 1f));
				}
			}
			score = num2;
			return result;
		}

		// Token: 0x0400058B RID: 1419
		internal float radius;

		// Token: 0x0400058C RID: 1420
		internal float height;

		// Token: 0x0400058D RID: 1421
		internal float desiredSpeed;

		// Token: 0x0400058E RID: 1422
		internal float maxSpeed;

		// Token: 0x0400058F RID: 1423
		internal float agentTimeHorizon;

		// Token: 0x04000590 RID: 1424
		internal float obstacleTimeHorizon;

		// Token: 0x04000591 RID: 1425
		internal bool locked;

		// Token: 0x04000592 RID: 1426
		private RVOLayer layer;

		// Token: 0x04000593 RID: 1427
		private RVOLayer collidesWith;

		// Token: 0x04000594 RID: 1428
		private int maxNeighbours;

		// Token: 0x04000595 RID: 1429
		internal Vector2 position;

		// Token: 0x04000596 RID: 1430
		private float elevationCoordinate;

		// Token: 0x04000597 RID: 1431
		private Vector2 currentVelocity;

		// Token: 0x04000598 RID: 1432
		private Vector2 desiredTargetPointInVelocitySpace;

		// Token: 0x04000599 RID: 1433
		private Vector2 desiredVelocity;

		// Token: 0x0400059A RID: 1434
		private Vector2 nextTargetPoint;

		// Token: 0x0400059B RID: 1435
		private float nextDesiredSpeed;

		// Token: 0x0400059C RID: 1436
		private float nextMaxSpeed;

		// Token: 0x0400059D RID: 1437
		private Vector2 collisionNormal;

		// Token: 0x0400059E RID: 1438
		private bool manuallyControlled;

		// Token: 0x0400059F RID: 1439
		private bool debugDraw;

		// Token: 0x040005AF RID: 1455
		internal Agent next;

		// Token: 0x040005B0 RID: 1456
		private float calculatedSpeed;

		// Token: 0x040005B1 RID: 1457
		private Vector2 calculatedTargetPoint;

		// Token: 0x040005B2 RID: 1458
		internal Simulator simulator;

		// Token: 0x040005B3 RID: 1459
		private List<Agent> neighbours = new List<Agent>();

		// Token: 0x040005B4 RID: 1460
		private List<float> neighbourDists = new List<float>();

		// Token: 0x040005B5 RID: 1461
		private List<ObstacleVertex> obstaclesBuffered = new List<ObstacleVertex>();

		// Token: 0x040005B6 RID: 1462
		private List<ObstacleVertex> obstacles = new List<ObstacleVertex>();

		// Token: 0x040005B7 RID: 1463
		private const float DesiredVelocityWeight = 0.1f;

		// Token: 0x040005B8 RID: 1464
		private const float WallWeight = 5f;

		// Token: 0x02000179 RID: 377
		internal struct VO
		{
			// Token: 0x06000BA7 RID: 2983 RVA: 0x0004A0C0 File Offset: 0x000482C0
			public VO(Vector2 center, Vector2 offset, float radius, float inverseDt, float inverseDeltaTime)
			{
				this.weightFactor = 1f;
				this.weightBonus = 0f;
				this.circleCenter = center * inverseDt + offset;
				this.weightFactor = 4f * Mathf.Exp(-Agent.Sqr(center.sqrMagnitude / (radius * radius))) + 1f;
				if (center.magnitude < radius)
				{
					this.colliding = true;
					this.line1 = center.normalized * (center.magnitude - radius - 0.001f) * 0.3f * inverseDeltaTime;
					this.dir1 = new Vector2(this.line1.y, -this.line1.x).normalized;
					this.line1 += offset;
					this.cutoffDir = Vector2.zero;
					this.cutoffLine = Vector2.zero;
					this.dir2 = Vector2.zero;
					this.line2 = Vector2.zero;
					this.radius = 0f;
				}
				else
				{
					this.colliding = false;
					center *= inverseDt;
					radius *= inverseDt;
					Vector2 b = center + offset;
					float d = center.magnitude - radius + 0.001f;
					this.cutoffLine = center.normalized * d;
					this.cutoffDir = new Vector2(-this.cutoffLine.y, this.cutoffLine.x).normalized;
					this.cutoffLine += offset;
					float num = Mathf.Atan2(-center.y, -center.x);
					float num2 = Mathf.Abs(Mathf.Acos(radius / center.magnitude));
					this.radius = radius;
					this.line1 = new Vector2(Mathf.Cos(num + num2), Mathf.Sin(num + num2));
					this.dir1 = new Vector2(this.line1.y, -this.line1.x);
					this.line2 = new Vector2(Mathf.Cos(num - num2), Mathf.Sin(num - num2));
					this.dir2 = new Vector2(this.line2.y, -this.line2.x);
					this.line1 = this.line1 * radius + b;
					this.line2 = this.line2 * radius + b;
				}
				this.segmentStart = Vector2.zero;
				this.segmentEnd = Vector2.zero;
				this.segment = false;
			}

			// Token: 0x06000BA8 RID: 2984 RVA: 0x0004A358 File Offset: 0x00048558
			public static Agent.VO SegmentObstacle(Vector2 segmentStart, Vector2 segmentEnd, Vector2 offset, float radius, float inverseDt, float inverseDeltaTime)
			{
				Agent.VO vo = default(Agent.VO);
				vo.weightFactor = 1f;
				vo.weightBonus = Mathf.Max(radius, 1f) * 40f;
				Vector3 vector = VectorMath.ClosestPointOnSegment(segmentStart, segmentEnd, Vector2.zero);
				if (vector.magnitude <= radius)
				{
					vo.colliding = true;
					vo.line1 = vector.normalized * (vector.magnitude - radius) * 0.3f * inverseDeltaTime;
					vo.dir1 = new Vector2(vo.line1.y, -vo.line1.x).normalized;
					vo.line1 += offset;
					vo.cutoffDir = Vector2.zero;
					vo.cutoffLine = Vector2.zero;
					vo.dir2 = Vector2.zero;
					vo.line2 = Vector2.zero;
					vo.radius = 0f;
					vo.segmentStart = Vector2.zero;
					vo.segmentEnd = Vector2.zero;
					vo.segment = false;
				}
				else
				{
					vo.colliding = false;
					segmentStart *= inverseDt;
					segmentEnd *= inverseDt;
					radius *= inverseDt;
					Vector2 normalized = (segmentEnd - segmentStart).normalized;
					vo.cutoffDir = normalized;
					vo.cutoffLine = segmentStart + new Vector2(-normalized.y, normalized.x) * radius;
					vo.cutoffLine += offset;
					float sqrMagnitude = segmentStart.sqrMagnitude;
					Vector2 vector2 = -VectorMath.ComplexMultiply(segmentStart, new Vector2(radius, Mathf.Sqrt(Mathf.Max(0f, sqrMagnitude - radius * radius)))) / sqrMagnitude;
					float sqrMagnitude2 = segmentEnd.sqrMagnitude;
					Vector2 vector3 = -VectorMath.ComplexMultiply(segmentEnd, new Vector2(radius, -Mathf.Sqrt(Mathf.Max(0f, sqrMagnitude2 - radius * radius)))) / sqrMagnitude2;
					vo.line1 = segmentStart + vector2 * radius + offset;
					vo.line2 = segmentEnd + vector3 * radius + offset;
					vo.dir1 = new Vector2(vector2.y, -vector2.x);
					vo.dir2 = new Vector2(vector3.y, -vector3.x);
					vo.segmentStart = segmentStart;
					vo.segmentEnd = segmentEnd;
					vo.radius = radius;
					vo.segment = true;
				}
				return vo;
			}

			// Token: 0x06000BA9 RID: 2985 RVA: 0x0004A60D File Offset: 0x0004880D
			public static float SignedDistanceFromLine(Vector2 a, Vector2 dir, Vector2 p)
			{
				return (p.x - a.x) * dir.y - dir.x * (p.y - a.y);
			}

			// Token: 0x06000BAA RID: 2986 RVA: 0x0004A638 File Offset: 0x00048838
			public Vector2 ScaledGradient(Vector2 p, out float weight)
			{
				Vector2 vector = this.Gradient(p, out weight);
				if (weight > 0f)
				{
					vector *= 2f * this.weightFactor;
					weight *= 2f * this.weightFactor;
					weight += 1f + this.weightBonus;
				}
				return vector;
			}

			// Token: 0x06000BAB RID: 2987 RVA: 0x0004A690 File Offset: 0x00048890
			public Vector2 Gradient(Vector2 p, out float weight)
			{
				if (this.colliding)
				{
					float num = Agent.VO.SignedDistanceFromLine(this.line1, this.dir1, p);
					if (num >= 0f)
					{
						weight = num;
						return new Vector2(-this.dir1.y, this.dir1.x);
					}
					weight = 0f;
					return new Vector2(0f, 0f);
				}
				else
				{
					float num2 = Agent.VO.SignedDistanceFromLine(this.cutoffLine, this.cutoffDir, p);
					if (num2 <= 0f)
					{
						weight = 0f;
						return Vector2.zero;
					}
					float num3 = Agent.VO.SignedDistanceFromLine(this.line1, this.dir1, p);
					float num4 = Agent.VO.SignedDistanceFromLine(this.line2, this.dir2, p);
					if (num3 < 0f || num4 < 0f)
					{
						weight = 0f;
						return Vector2.zero;
					}
					Vector2 result;
					if (Vector2.Dot(p - this.line1, this.dir1) > 0f && Vector2.Dot(p - this.line2, this.dir2) < 0f)
					{
						if (!this.segment)
						{
							float num5;
							result = VectorMath.Normalize(p - this.circleCenter, out num5);
							weight = this.radius - num5;
							return result;
						}
						if (num2 < this.radius)
						{
							Vector2 b = VectorMath.ClosestPointOnSegment(this.segmentStart, this.segmentEnd, p);
							float num6;
							result = VectorMath.Normalize(p - b, out num6);
							weight = this.radius - num6;
							return result;
						}
					}
					if (this.segment && num2 < num3 && num2 < num4)
					{
						weight = num2;
						result = new Vector2(-this.cutoffDir.y, this.cutoffDir.x);
						return result;
					}
					if (num3 < num4)
					{
						weight = num3;
						result = new Vector2(-this.dir1.y, this.dir1.x);
					}
					else
					{
						weight = num4;
						result = new Vector2(-this.dir2.y, this.dir2.x);
					}
					return result;
				}
			}

			// Token: 0x0400085E RID: 2142
			private Vector2 line1;

			// Token: 0x0400085F RID: 2143
			private Vector2 line2;

			// Token: 0x04000860 RID: 2144
			private Vector2 dir1;

			// Token: 0x04000861 RID: 2145
			private Vector2 dir2;

			// Token: 0x04000862 RID: 2146
			private Vector2 cutoffLine;

			// Token: 0x04000863 RID: 2147
			private Vector2 cutoffDir;

			// Token: 0x04000864 RID: 2148
			private Vector2 circleCenter;

			// Token: 0x04000865 RID: 2149
			private bool colliding;

			// Token: 0x04000866 RID: 2150
			private float radius;

			// Token: 0x04000867 RID: 2151
			private float weightFactor;

			// Token: 0x04000868 RID: 2152
			private float weightBonus;

			// Token: 0x04000869 RID: 2153
			private Vector2 segmentStart;

			// Token: 0x0400086A RID: 2154
			private Vector2 segmentEnd;

			// Token: 0x0400086B RID: 2155
			private bool segment;
		}

		// Token: 0x0200017A RID: 378
		internal class VOBuffer
		{
			// Token: 0x06000BAC RID: 2988 RVA: 0x0004A8A0 File Offset: 0x00048AA0
			public void Clear()
			{
				this.length = 0;
			}

			// Token: 0x06000BAD RID: 2989 RVA: 0x0004A8A9 File Offset: 0x00048AA9
			public VOBuffer(int n)
			{
				this.buffer = new Agent.VO[n];
				this.length = 0;
			}

			// Token: 0x06000BAE RID: 2990 RVA: 0x0004A8C4 File Offset: 0x00048AC4
			public void Add(Agent.VO vo)
			{
				if (this.length >= this.buffer.Length)
				{
					Agent.VO[] array = new Agent.VO[this.buffer.Length * 2];
					this.buffer.CopyTo(array, 0);
					this.buffer = array;
				}
				Agent.VO[] array2 = this.buffer;
				int num = this.length;
				this.length = num + 1;
				array2[num] = vo;
			}

			// Token: 0x0400086C RID: 2156
			public Agent.VO[] buffer;

			// Token: 0x0400086D RID: 2157
			public int length;
		}
	}
}
