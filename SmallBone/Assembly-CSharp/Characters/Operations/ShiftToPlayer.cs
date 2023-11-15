using System;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E06 RID: 3590
	public class ShiftToPlayer : CharacterOperation
	{
		// Token: 0x060047BF RID: 18367 RVA: 0x000D0C14 File Offset: 0x000CEE14
		public override void Run(Character owner)
		{
			Component player = Singleton<Service>.Instance.levelManager.player;
			Collider2D platform = this.GetPlatform();
			float x = player.transform.position.x + this._offsetX;
			float y = platform.bounds.max.y + this._offsetY;
			this._object.position = new Vector2(x, y);
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x000D0C80 File Offset: 0x000CEE80
		private Collider2D GetPlatform()
		{
			if (this._lastStandingPlatform)
			{
				return null;
			}
			ShiftToPlayer.caster.contactFilter.SetLayerMask(Layers.groundMask);
			NonAllocCaster nonAllocCaster = ShiftToPlayer.caster.BoxCast(Singleton<Service>.Instance.levelManager.player.transform.position, Singleton<Service>.Instance.levelManager.player.collider.bounds.size, 0f, Vector2.down, 100f);
			if (nonAllocCaster.results.Count == 0)
			{
				return null;
			}
			return nonAllocCaster.results[0].collider;
		}

		// Token: 0x040036D8 RID: 14040
		[SerializeField]
		private Transform _object;

		// Token: 0x040036D9 RID: 14041
		[SerializeField]
		private float _offsetY;

		// Token: 0x040036DA RID: 14042
		[SerializeField]
		private float _offsetX;

		// Token: 0x040036DB RID: 14043
		[SerializeField]
		private bool _fromPlatform;

		// Token: 0x040036DC RID: 14044
		[SerializeField]
		private bool _lastStandingPlatform;

		// Token: 0x040036DD RID: 14045
		private static NonAllocCaster caster = new NonAllocCaster(1);
	}
}
