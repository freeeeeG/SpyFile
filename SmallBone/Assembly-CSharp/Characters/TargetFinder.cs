using System;
using System.Collections.Generic;
using System.Linq;
using PhysicsUtils;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000697 RID: 1687
	public static class TargetFinder
	{
		// Token: 0x060021AE RID: 8622 RVA: 0x000654DF File Offset: 0x000636DF
		public static bool BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direciton, float distance, LayerMask layerMask, ref RaycastHit2D hit)
		{
			TargetFinder._caster.contactFilter.SetLayerMask(layerMask);
			return TargetFinder.TryToGetClosestHit(TargetFinder._caster.BoxCast(origin, size, angle, direciton, distance).results, ref hit);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0006550E File Offset: 0x0006370E
		public static bool RayCast(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask, ref RaycastHit2D hit)
		{
			TargetFinder._caster.contactFilter.SetLayerMask(layerMask);
			return TargetFinder.TryToGetClosestHit(TargetFinder._caster.RayCast(origin, direction, distance).results, ref hit);
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0006553C File Offset: 0x0006373C
		public static void FindTargetInRange(Vector2 origin, float radius, LayerMask layerMask, List<Target> targets)
		{
			targets.Clear();
			TargetFinder._overalpper.contactFilter.SetLayerMask(layerMask);
			foreach (Collider2D collider2D in TargetFinder._overalpper.OverlapCircle(origin, radius).results)
			{
				Target component = collider2D.GetComponent<Target>();
				if (!(component == null))
				{
					targets.Add(component);
				}
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000655B8 File Offset: 0x000637B8
		public static Character GetRandomTarget(Collider2D findRange, LayerMask layerMask)
		{
			TargetFinder._overalpper.contactFilter.SetLayerMask(layerMask);
			ReadonlyBoundedList<Collider2D> results = TargetFinder._overalpper.OverlapCollider(findRange).results;
			if (results.Count <= 0)
			{
				return null;
			}
			if ((from collider in results
			where collider.GetComponent<Target>() != null
			select collider).Count<Collider2D>() <= 0)
			{
				return null;
			}
			Collider2D[] array = results.ToArray<Collider2D>();
			array.Shuffle<Collider2D>();
			Collider2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Target component = array2[i].GetComponent<Target>();
				if (!(component == null))
				{
					return component.character;
				}
			}
			return null;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00065658 File Offset: 0x00063858
		public static void FindCharacterInRange(Vector2 origin, float radius, LayerMask layerMask, List<Character> targets)
		{
			targets.Clear();
			TargetFinder._overalpper.contactFilter.SetLayerMask(layerMask);
			foreach (Collider2D collider2D in TargetFinder._overalpper.OverlapCircle(origin, radius).results)
			{
				Target component = collider2D.GetComponent<Target>();
				if (!(component == null) && !(component.character == null))
				{
					targets.Add(component.character);
				}
			}
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000656E8 File Offset: 0x000638E8
		public static Character FindClosestTarget(Collider2D range, Collider2D ownerCollider, LayerMask layerMask)
		{
			TargetFinder._overalpper.contactFilter.SetLayerMask(layerMask);
			return TargetFinder.FindClosestTarget(TargetFinder._overalpper, range, ownerCollider);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00065706 File Offset: 0x00063906
		public static Character FindClosestTarget(NonAllocOverlapper overlapper, Collider2D range, Collider2D ownerCollider)
		{
			return TargetFinder.GetClosestTarget(overlapper.OverlapCollider(range).GetComponents<Target>(true), ownerCollider);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0006571B File Offset: 0x0006391B
		public static Character FindClosestTarget(Vector2 point, float range, LayerMask layerMask)
		{
			TargetFinder._overalpper.contactFilter.SetLayerMask(layerMask);
			return TargetFinder.FindClosestTarget(TargetFinder._overalpper, point, range);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00065739 File Offset: 0x00063939
		public static Character FindClosestTarget(NonAllocOverlapper overlapper, Vector2 point, float range)
		{
			return TargetFinder.GetClosestTarget(overlapper.OverlapCircle(point, range).GetComponents<Target>(true), point);
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0006574F File Offset: 0x0006394F
		public static Character FindClosestTarget(NonAllocOverlapper overlapper, Vector2 point, Vector2 size, float angle = 0f)
		{
			return TargetFinder.GetClosestTarget(overlapper.OverlapBox(point, size, angle).GetComponents<Target>(true), point);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00065768 File Offset: 0x00063968
		private static Character GetClosestTarget(List<Target> results, Vector2 center)
		{
			if (results.Count == 0)
			{
				return null;
			}
			if (results.Count == 1)
			{
				return results[0].character;
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < results.Count; i++)
			{
				Collider2D collider = results[i].collider;
				if (results[i].character != null)
				{
					collider = results[i].character.collider;
				}
				float num2 = Vector2.Distance(center, collider.transform.position);
				if (num > num2)
				{
					index = i;
					num = num2;
				}
			}
			return results[index].character;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00065810 File Offset: 0x00063A10
		private static Character GetClosestTarget(List<Target> results, Collider2D ownerCollider)
		{
			if (results.Count == 0)
			{
				return null;
			}
			if (results.Count == 1)
			{
				return results[0].character;
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < results.Count; i++)
			{
				float distance = Physics2D.Distance(results[i].collider, ownerCollider).distance;
				if (num > distance)
				{
					index = i;
					num = distance;
				}
			}
			return results[index].character;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00065888 File Offset: 0x00063A88
		private static bool TryToGetClosestHit(ReadonlyBoundedList<RaycastHit2D> results, ref RaycastHit2D hit)
		{
			if (results.Count <= 0)
			{
				return false;
			}
			int index = 0;
			float num = results[0].distance;
			for (int i = 1; i < results.Count; i++)
			{
				float distance = results[i].distance;
				if (distance < num)
				{
					num = distance;
					index = i;
				}
			}
			hit = results[index];
			return true;
		}

		// Token: 0x04001CC3 RID: 7363
		private static NonAllocOverlapper _overalpper = new NonAllocOverlapper(32);

		// Token: 0x04001CC4 RID: 7364
		private static NonAllocCaster _caster = new NonAllocCaster(32);
	}
}
