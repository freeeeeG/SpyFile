using System;
using System.Text;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F20 RID: 3872
	public class SpawnEnemyLineText : CharacterOperation
	{
		// Token: 0x06004B89 RID: 19337 RVA: 0x000DE540 File Offset: 0x000DC740
		public override void Run(Character owner)
		{
			if (!this._force && !this._lineText.finished)
			{
				return;
			}
			string[] localizedStringArray = this.GetLocalizedStringArray(owner);
			if (localizedStringArray.Length == 0)
			{
				return;
			}
			string text = localizedStringArray.Random<string>();
			Vector2 v = (this._spawnPosition == null) ? this.GetDefaultPosition(owner.collider.bounds) : this._spawnPosition.position;
			this._lineText.transform.position = v;
			this._lineText.Display(text, this._duration);
		}

		// Token: 0x06004B8A RID: 19338 RVA: 0x000D0EDE File Offset: 0x000CF0DE
		private Vector2 GetDefaultPosition(Bounds bounds)
		{
			return new Vector2(bounds.center.x, bounds.max.y + 0.5f);
		}

		// Token: 0x06004B8B RID: 19339 RVA: 0x000DE5D4 File Offset: 0x000DC7D4
		private string[] GetLocalizedStringArray(Character owner)
		{
			if (this._localizedStrings == null)
			{
				StringBuilder stringBuilder = new StringBuilder(this._textKeyPrefix);
				stringBuilder.Append(owner.key.ToString());
				stringBuilder.Append("/");
				stringBuilder.Append(this._textKey);
				this._localizedStrings = Localization.GetLocalizedStringArray(stringBuilder.ToString());
			}
			return this._localizedStrings;
		}

		// Token: 0x04003AC4 RID: 15044
		private readonly string _textKeyPrefix = "enemy/line/";

		// Token: 0x04003AC5 RID: 15045
		[SerializeField]
		private string _textKey;

		// Token: 0x04003AC6 RID: 15046
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003AC7 RID: 15047
		[SerializeField]
		private float _duration = 1.5f;

		// Token: 0x04003AC8 RID: 15048
		[SerializeField]
		private float _coolTime = 8f;

		// Token: 0x04003AC9 RID: 15049
		[SerializeField]
		private bool _force;

		// Token: 0x04003ACA RID: 15050
		[SerializeField]
		private LineText _lineText;

		// Token: 0x04003ACB RID: 15051
		private string[] _localizedStrings;
	}
}
