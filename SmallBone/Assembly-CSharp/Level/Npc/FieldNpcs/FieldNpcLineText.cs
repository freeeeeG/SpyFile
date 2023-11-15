using System;
using GameResources;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x020005CA RID: 1482
	public class FieldNpcLineText : MonoBehaviour
	{
		// Token: 0x06001D87 RID: 7559 RVA: 0x0005A1EF File Offset: 0x000583EF
		private void Start()
		{
			if (this._lineText == null)
			{
				this._lineText = base.GetComponentInChildren<LineText>();
			}
			this.ResetTime();
			this._fieldNpc.onCageDestroyed += delegate()
			{
				if (string.IsNullOrEmpty(this._overrideCageDestroyedLineKey))
				{
					this.ShowLineText(this._fieldNpc.cageDestroyedLine);
					return;
				}
				this.ShowLineText(Localization.GetLocalizedString(this._overrideCageDestroyedLineKey));
			};
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x0005A228 File Offset: 0x00058428
		private void ShowLineText(string text)
		{
			this._lineText.Display(text, this._duration);
			this.ResetTime();
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x0005A242 File Offset: 0x00058442
		private void ResetTime()
		{
			this._cooltime = UnityEngine.Random.Range(this._coolTimeRange.x, this._coolTimeRange.y);
			this._elapsed = 0f;
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x0005A270 File Offset: 0x00058470
		private void Update()
		{
			if (!this._fieldNpc.release)
			{
				return;
			}
			this._elapsed += Chronometer.global.deltaTime;
			if (this._elapsed > this._cooltime)
			{
				this._canRun = true;
			}
			else
			{
				this._canRun = false;
			}
			if (this._canRun)
			{
				if (string.IsNullOrEmpty(this._overrideNormalLineKey))
				{
					this.ShowLineText(this._fieldNpc.normalLine);
					return;
				}
				this.ShowLineText(Localization.GetLocalizedStringArray(this._overrideNormalLineKey).Random<string>());
			}
		}

		// Token: 0x04001901 RID: 6401
		[SerializeField]
		private FieldNpc _fieldNpc;

		// Token: 0x04001902 RID: 6402
		[SerializeField]
		[Range(0f, 100f)]
		private float _duration;

		// Token: 0x04001903 RID: 6403
		[SerializeField]
		[MinMaxSlider(0f, 100f)]
		private Vector2 _coolTimeRange;

		// Token: 0x04001904 RID: 6404
		[SerializeField]
		private LineText _lineText;

		// Token: 0x04001905 RID: 6405
		private float _cooltime;

		// Token: 0x04001906 RID: 6406
		private float _elapsed;

		// Token: 0x04001907 RID: 6407
		private bool _canRun;

		// Token: 0x04001908 RID: 6408
		[SerializeField]
		private string _overrideCageDestroyedLineKey;

		// Token: 0x04001909 RID: 6409
		[SerializeField]
		private string _overrideNormalLineKey;
	}
}
