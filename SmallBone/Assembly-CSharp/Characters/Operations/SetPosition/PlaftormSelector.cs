using System;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED5 RID: 3797
	public static class PlaftormSelector
	{
		// Token: 0x06004A93 RID: 19091 RVA: 0x000D99F0 File Offset: 0x000D7BF0
		private static Collider2D GetPlatform()
		{
			PlaftormSelector.caster.contactFilter.SetLayerMask(Layers.groundMask);
			NonAllocCaster nonAllocCaster = PlaftormSelector.caster.BoxCast(Singleton<Service>.Instance.levelManager.player.transform.position, Singleton<Service>.Instance.levelManager.player.collider.bounds.size, 0f, Vector2.down, 100f);
			if (nonAllocCaster.results.Count == 0)
			{
				return null;
			}
			return nonAllocCaster.results[0].collider;
		}

		// Token: 0x040039B2 RID: 14770
		private static NonAllocCaster caster = new NonAllocCaster(1);
	}
}
