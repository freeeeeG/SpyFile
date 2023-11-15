using System;
using Characters.Abilities.Constraints;
using GameResources;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005AF RID: 1455
	public class NpcLineText : MonoBehaviour
	{
		// Token: 0x06001CD4 RID: 7380 RVA: 0x000589E2 File Offset: 0x00056BE2
		private void Start()
		{
			if (this._lineText == null)
			{
				this._lineText = base.GetComponentInChildren<LineText>();
			}
			this._cooltime = UnityEngine.Random.Range(this._preCoolTimeRange.x, this._preCoolTimeRange.y);
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00058A20 File Offset: 0x00056C20
		public void Run()
		{
			string[] localizedStringArray = Localization.GetLocalizedStringArray(this._commonTextKey);
			if (localizedStringArray.Length < 0)
			{
				this._elapsed = 0f;
				return;
			}
			string text = localizedStringArray.Random<string>();
			this._lineText.Display(text, this._duration);
			this._cooltime = UnityEngine.Random.Range(this._coolTimeRange.x, this._coolTimeRange.y);
			this._elapsed = 0f;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x00058A90 File Offset: 0x00056C90
		public void Run(string text)
		{
			this._lineText.Display(text, this._duration);
			this._cooltime = UnityEngine.Random.Range(this._coolTimeRange.x, this._coolTimeRange.y);
			this._elapsed = 0f;
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00058AD0 File Offset: 0x00056CD0
		private void Update()
		{
			if (this._constraints != null && !this._constraints.Pass())
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
				this.Run();
			}
		}

		// Token: 0x0400187C RID: 6268
		[SerializeField]
		private string _commonTextKey;

		// Token: 0x0400187D RID: 6269
		[Range(0f, 100f)]
		[SerializeField]
		private float _duration;

		// Token: 0x0400187E RID: 6270
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _preCoolTimeRange;

		// Token: 0x0400187F RID: 6271
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _coolTimeRange;

		// Token: 0x04001880 RID: 6272
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x04001881 RID: 6273
		[SerializeField]
		private LineText _lineText;

		// Token: 0x04001882 RID: 6274
		private float _cooltime;

		// Token: 0x04001883 RID: 6275
		private float _elapsed;

		// Token: 0x04001884 RID: 6276
		private bool _canRun;
	}
}
