using System;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F21 RID: 3873
	public class SpawnLineText : CharacterOperation
	{
		// Token: 0x06004B8D RID: 19341 RVA: 0x000DE66C File Offset: 0x000DC86C
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
			string[] localizedStringArray = Localization.GetLocalizedStringArray(this._textKey);
			if (localizedStringArray.Length == 0)
			{
				return;
			}
			string text = localizedStringArray.Random<string>();
			Vector2 v = (this._spawnPosition == null) ? this.GetDefaultPosition(owner.collider.bounds) : this._spawnPosition.position;
			this._lineText.transform.position = v;
			this._lineText.Display(text, this._duration);
		}

		// Token: 0x06004B8E RID: 19342 RVA: 0x000D0EDE File Offset: 0x000CF0DE
		private Vector2 GetDefaultPosition(Bounds bounds)
		{
			return new Vector2(bounds.center.x, bounds.max.y + 0.5f);
		}

		// Token: 0x04003ACC RID: 15052
		[SerializeField]
		private string _textKey;

		// Token: 0x04003ACD RID: 15053
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003ACE RID: 15054
		[SerializeField]
		private float _duration = 2f;

		// Token: 0x04003ACF RID: 15055
		[SerializeField]
		private float _coolTime = 8f;

		// Token: 0x04003AD0 RID: 15056
		[SerializeField]
		private bool _force;

		// Token: 0x04003AD1 RID: 15057
		private LineText _lineText;

		// Token: 0x04003AD2 RID: 15058
		private Character _owner;
	}
}
