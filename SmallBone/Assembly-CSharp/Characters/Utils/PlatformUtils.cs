using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Utils
{
	// Token: 0x0200074C RID: 1868
	public static class PlatformUtils
	{
		// Token: 0x060025FE RID: 9726 RVA: 0x000729A0 File Offset: 0x00070BA0
		public static Collider2D GetClosestPlatform(Vector2 origin, Vector2 direction, NonAllocCaster caster, LayerMask layerMask, float distance = 100f)
		{
			caster.contactFilter.SetLayerMask(layerMask);
			caster.RayCast(origin, direction, distance);
			ReadonlyBoundedList<RaycastHit2D> results = caster.results;
			if (results.Count < 0)
			{
				return null;
			}
			int index = 0;
			float num = results[0].distance;
			for (int i = 1; i < results.Count; i++)
			{
				float distance2 = results[i].distance;
				if (distance2 < num)
				{
					num = distance2;
					index = i;
				}
			}
			return results[index].collider;
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x00072A2C File Offset: 0x00070C2C
		public static Vector2 GetProjectionPointToPlatform(Vector2 origin, Vector2 direction, NonAllocCaster caster, LayerMask layerMask, float distance = 100f)
		{
			caster.contactFilter.SetLayerMask(layerMask);
			caster.RayCast(origin, direction, distance);
			ReadonlyBoundedList<RaycastHit2D> results = caster.results;
			if (results.Count <= 0)
			{
				return origin;
			}
			int index = 0;
			float num = results[0].distance;
			for (int i = 1; i < results.Count; i++)
			{
				float distance2 = results[i].distance;
				if (distance2 < num)
				{
					num = distance2;
					index = i;
				}
			}
			return results[index].point;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x00072AB8 File Offset: 0x00070CB8
		public static bool Teleport(Transform traget, Bounds bounds, Vector2 destination, float maxRetryDistance)
		{
			return PlatformUtils.Teleport(traget, bounds, destination, (MMMaths.Vector3ToVector2(traget.position) - destination).normalized, maxRetryDistance);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x00072AE8 File Offset: 0x00070CE8
		public static bool Teleport(Transform traget, Bounds bounds, Vector2 destination, Vector2 direction, float maxRetryDistance)
		{
			int num = 0;
			while ((float)num <= maxRetryDistance)
			{
				if (PlatformUtils.Teleport(traget, bounds, destination + direction * (float)num))
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x00072B20 File Offset: 0x00070D20
		public static bool Teleport(Transform traget, Bounds bounds, Vector2 destination)
		{
			bounds.center = new Vector2(destination.x, destination.y + (bounds.center.y - bounds.min.y));
			NonAllocOverlapper.shared.contactFilter.SetLayerMask(Layers.terrainMask);
			if (NonAllocOverlapper.shared.OverlapBox(bounds.center, bounds.size, 0f).results.Count == 0)
			{
				traget.position = destination;
				return true;
			}
			return false;
		}
	}
}
