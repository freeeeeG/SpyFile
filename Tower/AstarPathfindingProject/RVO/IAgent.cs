using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020000D4 RID: 212
	public interface IAgent
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060008E1 RID: 2273
		// (set) Token: 0x060008E2 RID: 2274
		Vector2 Position { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060008E3 RID: 2275
		// (set) Token: 0x060008E4 RID: 2276
		float ElevationCoordinate { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060008E5 RID: 2277
		Vector2 CalculatedTargetPoint { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060008E6 RID: 2278
		float CalculatedSpeed { get; }

		// Token: 0x060008E7 RID: 2279
		void SetTarget(Vector2 targetPoint, float desiredSpeed, float maxSpeed);

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060008E8 RID: 2280
		// (set) Token: 0x060008E9 RID: 2281
		bool Locked { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060008EA RID: 2282
		// (set) Token: 0x060008EB RID: 2283
		float Radius { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060008EC RID: 2284
		// (set) Token: 0x060008ED RID: 2285
		float Height { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060008EE RID: 2286
		// (set) Token: 0x060008EF RID: 2287
		float AgentTimeHorizon { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060008F0 RID: 2288
		// (set) Token: 0x060008F1 RID: 2289
		float ObstacleTimeHorizon { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060008F2 RID: 2290
		// (set) Token: 0x060008F3 RID: 2291
		int MaxNeighbours { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060008F4 RID: 2292
		int NeighbourCount { get; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060008F5 RID: 2293
		// (set) Token: 0x060008F6 RID: 2294
		RVOLayer Layer { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060008F7 RID: 2295
		// (set) Token: 0x060008F8 RID: 2296
		RVOLayer CollidesWith { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060008F9 RID: 2297
		// (set) Token: 0x060008FA RID: 2298
		bool DebugDraw { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060008FB RID: 2299
		[Obsolete]
		List<ObstacleVertex> NeighbourObstacles { get; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060008FC RID: 2300
		// (set) Token: 0x060008FD RID: 2301
		float Priority { get; set; }

		// Token: 0x17000137 RID: 311
		// (set) Token: 0x060008FE RID: 2302
		Action PreCalculationCallback { set; }

		// Token: 0x060008FF RID: 2303
		void SetCollisionNormal(Vector2 normal);

		// Token: 0x06000900 RID: 2304
		void ForceSetVelocity(Vector2 velocity);
	}
}
