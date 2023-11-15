using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E08 RID: 3592
	public class SpawnRawLineText : CharacterOperation
	{
		// Token: 0x060047C6 RID: 18374 RVA: 0x000D0E24 File Offset: 0x000CF024
		public override void Run(Character owner)
		{
			if (this._lineText == null)
			{
				this._lineText = owner.GetComponentInChildren<LineText>();
				if (this._lineText == null)
				{
					return;
				}
			}
			if (!this._force && !this._lineText.finished)
			{
				return;
			}
			if (this._text.Length <= 0)
			{
				return;
			}
			Vector2 v = (this._spawnPosition == null) ? this.GetDefaultPosition(owner.collider.bounds) : this._spawnPosition.position;
			this._lineText.transform.position = v;
			this._lineText.Display(this._text, this._duration);
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x000D0EDE File Offset: 0x000CF0DE
		private Vector2 GetDefaultPosition(Bounds bounds)
		{
			return new Vector2(bounds.center.x, bounds.max.y + 0.5f);
		}

		// Token: 0x040036E3 RID: 14051
		[SerializeField]
		private string _text;

		// Token: 0x040036E4 RID: 14052
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x040036E5 RID: 14053
		[SerializeField]
		private float _duration = 1.5f;

		// Token: 0x040036E6 RID: 14054
		[SerializeField]
		private float _coolTime = 8f;

		// Token: 0x040036E7 RID: 14055
		[SerializeField]
		private bool _force;

		// Token: 0x040036E8 RID: 14056
		private LineText _lineText;
	}
}
