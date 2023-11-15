using System;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding.RVO
{
	// Token: 0x020000DA RID: 218
	[AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_r_v_o_1_1_r_v_o_controller.php")]
	public class RVOController : VersionedMonoBehaviour
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0003CB65 File Offset: 0x0003AD65
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0003CB81 File Offset: 0x0003AD81
		public float radius
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.radius;
				}
				return this.radiusBackingField;
			}
			set
			{
				if (this.ai != null)
				{
					this.ai.radius = value;
				}
				this.radiusBackingField = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0003CB9E File Offset: 0x0003AD9E
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0003CBBA File Offset: 0x0003ADBA
		public float height
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.height;
				}
				return this.heightBackingField;
			}
			set
			{
				if (this.ai != null)
				{
					this.ai.height = value;
				}
				this.heightBackingField = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0003CBD7 File Offset: 0x0003ADD7
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x0003CBF9 File Offset: 0x0003ADF9
		public float center
		{
			get
			{
				if (this.ai != null)
				{
					return this.ai.height / 2f;
				}
				return this.centerBackingField;
			}
			set
			{
				this.centerBackingField = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x0003CC02 File Offset: 0x0003AE02
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x0003CC0A File Offset: 0x0003AE0A
		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public LayerMask mask
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0003CC0C File Offset: 0x0003AE0C
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0003CC0F File Offset: 0x0003AE0F
		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public bool enableRotation
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x0003CC11 File Offset: 0x0003AE11
		// (set) Token: 0x06000932 RID: 2354 RVA: 0x0003CC18 File Offset: 0x0003AE18
		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public float rotationSpeed
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0003CC1A File Offset: 0x0003AE1A
		// (set) Token: 0x06000934 RID: 2356 RVA: 0x0003CC21 File Offset: 0x0003AE21
		[Obsolete("This field is obsolete in version 4.0 and will not affect anything. Use the LegacyRVOController if you need the old behaviour")]
		public float maxSpeed
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0003CC23 File Offset: 0x0003AE23
		public MovementPlane movementPlane
		{
			get
			{
				if (this.simulator != null)
				{
					return this.simulator.movementPlane;
				}
				if (RVOSimulator.active)
				{
					return RVOSimulator.active.movementPlane;
				}
				return MovementPlane.XZ;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0003CC51 File Offset: 0x0003AE51
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0003CC59 File Offset: 0x0003AE59
		public IAgent rvoAgent { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0003CC62 File Offset: 0x0003AE62
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x0003CC6A File Offset: 0x0003AE6A
		public Simulator simulator { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600093A RID: 2362 RVA: 0x0003CC73 File Offset: 0x0003AE73
		// (set) Token: 0x0600093B RID: 2363 RVA: 0x0003CC95 File Offset: 0x0003AE95
		protected IAstarAI ai
		{
			get
			{
				if (this.aiBackingField as MonoBehaviour == null)
				{
					this.aiBackingField = null;
				}
				return this.aiBackingField;
			}
			set
			{
				this.aiBackingField = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0003CC9E File Offset: 0x0003AE9E
		public Vector3 position
		{
			get
			{
				return this.To3D(this.rvoAgent.Position, this.rvoAgent.ElevationCoordinate);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0003CCBC File Offset: 0x0003AEBC
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x0003CCEF File Offset: 0x0003AEEF
		public Vector3 velocity
		{
			get
			{
				float num = (Time.deltaTime > 0.0001f) ? Time.deltaTime : 0.02f;
				return this.CalculateMovementDelta(num) / num;
			}
			set
			{
				this.rvoAgent.ForceSetVelocity(this.To2D(value));
			}
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0003CD04 File Offset: 0x0003AF04
		public Vector3 CalculateMovementDelta(float deltaTime)
		{
			if (this.rvoAgent == null)
			{
				return Vector3.zero;
			}
			return this.To3D(Vector2.ClampMagnitude(this.rvoAgent.CalculatedTargetPoint - this.To2D((this.ai != null) ? this.ai.position : this.tr.position), this.rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0003CD72 File Offset: 0x0003AF72
		public Vector3 CalculateMovementDelta(Vector3 position, float deltaTime)
		{
			return this.To3D(Vector2.ClampMagnitude(this.rvoAgent.CalculatedTargetPoint - this.To2D(position), this.rvoAgent.CalculatedSpeed * deltaTime), 0f);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
		public void SetCollisionNormal(Vector3 normal)
		{
			this.rvoAgent.SetCollisionNormal(this.To2D(normal));
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0003CDBC File Offset: 0x0003AFBC
		[Obsolete("Set the 'velocity' property instead")]
		public void ForceSetVelocity(Vector3 velocity)
		{
			this.velocity = velocity;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0003CDC8 File Offset: 0x0003AFC8
		public Vector2 To2D(Vector3 p)
		{
			float num;
			return this.To2D(p, out num);
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0003CDDE File Offset: 0x0003AFDE
		public Vector2 To2D(Vector3 p, out float elevation)
		{
			if (this.movementPlane == MovementPlane.XY)
			{
				elevation = -p.z;
				return new Vector2(p.x, p.y);
			}
			elevation = p.y;
			return new Vector2(p.x, p.z);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0003CE1D File Offset: 0x0003B01D
		public Vector3 To3D(Vector2 p, float elevationCoordinate)
		{
			if (this.movementPlane == MovementPlane.XY)
			{
				return new Vector3(p.x, p.y, -elevationCoordinate);
			}
			return new Vector3(p.x, elevationCoordinate, p.y);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0003CE4E File Offset: 0x0003B04E
		private void OnDisable()
		{
			if (this.simulator == null)
			{
				return;
			}
			this.simulator.RemoveAgent(this.rvoAgent);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0003CE6C File Offset: 0x0003B06C
		private void OnEnable()
		{
			this.tr = base.transform;
			this.ai = base.GetComponent<IAstarAI>();
			AIBase aibase = this.ai as AIBase;
			if (aibase != null)
			{
				aibase.FindComponents();
			}
			if (RVOSimulator.active == null)
			{
				Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
				base.enabled = false;
				return;
			}
			this.simulator = RVOSimulator.active.GetSimulator();
			if (this.rvoAgent != null)
			{
				this.simulator.AddAgent(this.rvoAgent);
				return;
			}
			this.rvoAgent = this.simulator.AddAgent(Vector2.zero, 0f);
			this.rvoAgent.PreCalculationCallback = new Action(this.UpdateAgentProperties);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0003CF28 File Offset: 0x0003B128
		protected void UpdateAgentProperties()
		{
			Vector3 localScale = this.tr.localScale;
			this.rvoAgent.Radius = Mathf.Max(0.001f, this.radius * localScale.x);
			this.rvoAgent.AgentTimeHorizon = this.agentTimeHorizon;
			this.rvoAgent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
			this.rvoAgent.Locked = this.locked;
			this.rvoAgent.MaxNeighbours = this.maxNeighbours;
			this.rvoAgent.DebugDraw = this.debug;
			this.rvoAgent.Layer = this.layer;
			this.rvoAgent.CollidesWith = this.collidesWith;
			this.rvoAgent.Priority = this.priority;
			float num;
			this.rvoAgent.Position = this.To2D((this.ai != null) ? this.ai.position : this.tr.position, out num);
			if (this.movementPlane == MovementPlane.XZ)
			{
				this.rvoAgent.Height = this.height * localScale.y;
				this.rvoAgent.ElevationCoordinate = num + (this.center - 0.5f * this.height) * localScale.y;
				return;
			}
			this.rvoAgent.Height = 1f;
			this.rvoAgent.ElevationCoordinate = 0f;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0003D086 File Offset: 0x0003B286
		public void SetTarget(Vector3 pos, float speed, float maxSpeed)
		{
			if (this.simulator == null)
			{
				return;
			}
			this.rvoAgent.SetTarget(this.To2D(pos), speed, maxSpeed);
			if (this.lockWhenNotMoving)
			{
				this.locked = (speed < 0.001f);
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0003D0BC File Offset: 0x0003B2BC
		public void Move(Vector3 vel)
		{
			if (this.simulator == null)
			{
				return;
			}
			Vector2 b = this.To2D(vel);
			float magnitude = b.magnitude;
			this.rvoAgent.SetTarget(this.To2D((this.ai != null) ? this.ai.position : this.tr.position) + b, magnitude, magnitude);
			if (this.lockWhenNotMoving)
			{
				this.locked = (magnitude < 0.001f);
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0003D131 File Offset: 0x0003B331
		[Obsolete("Use transform.position instead, the RVOController can now handle that without any issues.")]
		public void Teleport(Vector3 pos)
		{
			this.tr.position = pos;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0003D140 File Offset: 0x0003B340
		private void OnDrawGizmos()
		{
			this.tr = base.transform;
			if (this.ai == null)
			{
				Color color = AIBase.ShapeGizmoColor * (this.locked ? 0.5f : 1f);
				Vector3 position = base.transform.position;
				Vector3 localScale = this.tr.localScale;
				if (this.movementPlane == MovementPlane.XY)
				{
					Draw.Gizmos.Cylinder(position, Vector3.forward, 0f, this.radius * localScale.x, color);
					return;
				}
				Draw.Gizmos.Cylinder(position + this.To3D(Vector2.zero, this.center - this.height * 0.5f) * localScale.y, this.To3D(Vector2.zero, 1f), this.height * localScale.y, this.radius * localScale.x, color);
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0003D22C File Offset: 0x0003B42C
		protected override int OnUpgradeSerializedData(int version, bool unityThread)
		{
			if (version <= 1)
			{
				if (!unityThread)
				{
					return -1;
				}
				if (base.transform.localScale.y != 0f)
				{
					this.centerBackingField /= Mathf.Abs(base.transform.localScale.y);
				}
				if (base.transform.localScale.y != 0f)
				{
					this.heightBackingField /= Mathf.Abs(base.transform.localScale.y);
				}
				if (base.transform.localScale.x != 0f)
				{
					this.radiusBackingField /= Mathf.Abs(base.transform.localScale.x);
				}
			}
			return 2;
		}

		// Token: 0x04000562 RID: 1378
		[SerializeField]
		[FormerlySerializedAs("radius")]
		internal float radiusBackingField = 0.5f;

		// Token: 0x04000563 RID: 1379
		[SerializeField]
		[FormerlySerializedAs("height")]
		private float heightBackingField = 2f;

		// Token: 0x04000564 RID: 1380
		[SerializeField]
		[FormerlySerializedAs("center")]
		private float centerBackingField = 1f;

		// Token: 0x04000565 RID: 1381
		[Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quality is not the best")]
		public bool locked;

		// Token: 0x04000566 RID: 1382
		[Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
		public bool lockWhenNotMoving;

		// Token: 0x04000567 RID: 1383
		[Tooltip("How far into the future to look for collisions with other agents (in seconds)")]
		public float agentTimeHorizon = 2f;

		// Token: 0x04000568 RID: 1384
		[Tooltip("How far into the future to look for collisions with obstacles (in seconds)")]
		public float obstacleTimeHorizon = 2f;

		// Token: 0x04000569 RID: 1385
		[Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
		public int maxNeighbours = 10;

		// Token: 0x0400056A RID: 1386
		public RVOLayer layer = RVOLayer.DefaultAgent;

		// Token: 0x0400056B RID: 1387
		[EnumFlag]
		public RVOLayer collidesWith = (RVOLayer)(-1);

		// Token: 0x0400056C RID: 1388
		[HideInInspector]
		[Obsolete]
		public float wallAvoidForce = 1f;

		// Token: 0x0400056D RID: 1389
		[HideInInspector]
		[Obsolete]
		public float wallAvoidFalloff = 1f;

		// Token: 0x0400056E RID: 1390
		[Tooltip("How strongly other agents will avoid this agent")]
		[Range(0f, 1f)]
		public float priority = 0.5f;

		// Token: 0x04000571 RID: 1393
		protected Transform tr;

		// Token: 0x04000572 RID: 1394
		[SerializeField]
		[FormerlySerializedAs("ai")]
		private IAstarAI aiBackingField;

		// Token: 0x04000573 RID: 1395
		public bool debug;
	}
}
