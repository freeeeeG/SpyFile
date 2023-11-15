using System;
using System.Collections.ObjectModel;
using GameResources;
using PhysicsUtils;
using UnityEngine;

namespace FX
{
	// Token: 0x02000237 RID: 567
	public class FootShadowRenderer
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x0001EE8D File Offset: 0x0001D08D
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x0001EE95 File Offset: 0x0001D095
		public SpriteRenderer spriteRenderer { get; private set; }

		// Token: 0x06000B27 RID: 2855 RVA: 0x0001EEA0 File Offset: 0x0001D0A0
		public FootShadowRenderer(int accuracy, Transform transform)
		{
			this._lineSequenceCaster = new LineSequenceNonAllocCaster(1, accuracy * 2 + 1)
			{
				caster = new RayCaster
				{
					direction = Vector2.down,
					distance = 5f
				}
			};
			this._lineSequenceCaster.caster.contactFilter.SetLayerMask(Layers.groundMask);
			this.spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(FootShadowRenderer.Assets.footShadow, transform);
			this._size = this.spriteRenderer.size;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0001EF24 File Offset: 0x0001D124
		public void SetBounds(Bounds bounds)
		{
			this._lineSequenceCaster.start = bounds.min;
			this._lineSequenceCaster.end.x = bounds.max.x;
			this._lineSequenceCaster.end.y = bounds.min.y;
			this._size.x = bounds.size.x;
			this.spriteRenderer.size = this._size;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0001EFA8 File Offset: 0x0001D1A8
		public void Update()
		{
			this._lineSequenceCaster.Cast();
			ReadOnlyCollection<NonAllocCaster> nonAllocCasters = this._lineSequenceCaster.nonAllocCasters;
			int num = -1;
			for (int i = 0; i < nonAllocCasters.Count; i++)
			{
				if (nonAllocCasters[i].results.Count != 0 && (num == -1 || nonAllocCasters[num].results[0].distance > nonAllocCasters[i].results[0].distance))
				{
					num = i;
				}
			}
			if (num == -1)
			{
				this.spriteRenderer.gameObject.SetActive(false);
				return;
			}
			RaycastHit2D raycastHit2D = nonAllocCasters[num].results[0];
			float distance = raycastHit2D.distance;
			Vector3 position = this.spriteRenderer.transform.position;
			position.y = raycastHit2D.point.y;
			float d = (5f - distance) / 5f;
			this.spriteRenderer.gameObject.SetActive(true);
			this.spriteRenderer.transform.position = position;
			this.spriteRenderer.transform.localScale = Vector3.one * d;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0001F0DB File Offset: 0x0001D2DB
		public void DrawDebugLine()
		{
			this._lineSequenceCaster.DrawDebugLine();
		}

		// Token: 0x04000955 RID: 2389
		private const float _maxDistance = 5f;

		// Token: 0x04000956 RID: 2390
		private LineSequenceNonAllocCaster _lineSequenceCaster;

		// Token: 0x04000957 RID: 2391
		private Vector2 _size;

		// Token: 0x02000238 RID: 568
		private class Assets
		{
			// Token: 0x04000959 RID: 2393
			internal static readonly SpriteRenderer footShadow = CommonResource.instance.footShadow;
		}
	}
}
