using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000009 RID: 9
	public interface IAstarAI
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000FA RID: 250
		// (set) Token: 0x060000FB RID: 251
		float radius { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FC RID: 252
		// (set) Token: 0x060000FD RID: 253
		float height { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FE RID: 254
		Vector3 position { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FF RID: 255
		// (set) Token: 0x06000100 RID: 256
		Quaternion rotation { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000101 RID: 257
		// (set) Token: 0x06000102 RID: 258
		float maxSpeed { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000103 RID: 259
		Vector3 velocity { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000104 RID: 260
		Vector3 desiredVelocity { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000105 RID: 261
		float remainingDistance { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000106 RID: 262
		bool reachedDestination { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000107 RID: 263
		bool reachedEndOfPath { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000108 RID: 264
		// (set) Token: 0x06000109 RID: 265
		Vector3 destination { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600010A RID: 266
		// (set) Token: 0x0600010B RID: 267
		bool canSearch { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600010C RID: 268
		// (set) Token: 0x0600010D RID: 269
		bool canMove { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600010E RID: 270
		bool hasPath { get; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600010F RID: 271
		bool pathPending { get; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000110 RID: 272
		// (set) Token: 0x06000111 RID: 273
		bool isStopped { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000112 RID: 274
		Vector3 steeringTarget { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000113 RID: 275
		// (set) Token: 0x06000114 RID: 276
		Action onSearchPath { get; set; }

		// Token: 0x06000115 RID: 277
		void GetRemainingPath(List<Vector3> buffer, out bool stale);

		// Token: 0x06000116 RID: 278
		void SearchPath();

		// Token: 0x06000117 RID: 279
		void SetPath(Path path, bool updateDestinationFromPath = true);

		// Token: 0x06000118 RID: 280
		void Teleport(Vector3 newPosition, bool clearPath = true);

		// Token: 0x06000119 RID: 281
		void Move(Vector3 deltaPosition);

		// Token: 0x0600011A RID: 282
		void MovementUpdate(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation);

		// Token: 0x0600011B RID: 283
		void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation);
	}
}
