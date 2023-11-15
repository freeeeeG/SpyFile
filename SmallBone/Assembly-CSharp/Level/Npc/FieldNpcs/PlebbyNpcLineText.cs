using System;
using GameResources;
using UnityEngine;

namespace Level.Npc.FieldNpcs
{
	// Token: 0x02000602 RID: 1538
	public class PlebbyNpcLineText : MonoBehaviour
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x0005DA73 File Offset: 0x0005BC73
		private string _AcageDestroyedLine
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/A/line/resqued", this._fieldNpc.type));
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x0005DA94 File Offset: 0x0005BC94
		private string _BcageDestroyedLine
		{
			get
			{
				return Localization.GetLocalizedString(string.Format("npc/{0}/B/line/resqued", this._fieldNpc.type));
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0005DAB8 File Offset: 0x0005BCB8
		private void Start()
		{
			this._normalLineLenght = Localization.GetLocalizedStringArray(string.Format("npc/{0}/A/line", this._fieldNpc.type)).Length;
			this.ResetTime();
			this._fieldNpc.onCageDestroyed += delegate()
			{
				this.ShowLineText(this._ALineText, this._AcageDestroyedLine);
				this.ShowLineText(this._BLineText, this._BcageDestroyedLine);
			};
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0005DB09 File Offset: 0x0005BD09
		private void ShowLineText(LineText lineText, string text)
		{
			lineText.Display(text, this._duration);
			this.ResetTime();
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0005DB1E File Offset: 0x0005BD1E
		private void ResetTime()
		{
			this._cooltime = UnityEngine.Random.Range(this._coolTimeRange.x, this._coolTimeRange.y);
			this._elapsed = 0f;
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0005DB4C File Offset: 0x0005BD4C
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
				int num = UnityEngine.Random.Range(0, this._normalLineLenght);
				this.ShowLineText(this._ALineText, Localization.GetLocalizedString(string.Format("npc/{0}/A/line/{1}", this._fieldNpc.type, num)));
				this.ShowLineText(this._BLineText, Localization.GetLocalizedString(string.Format("npc/{0}/B/line/{1}", this._fieldNpc.type, num)));
			}
		}

		// Token: 0x04001A0C RID: 6668
		[SerializeField]
		private FieldNpc _fieldNpc;

		// Token: 0x04001A0D RID: 6669
		[SerializeField]
		[Range(0f, 100f)]
		private float _duration;

		// Token: 0x04001A0E RID: 6670
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2 _coolTimeRange;

		// Token: 0x04001A0F RID: 6671
		[SerializeField]
		private LineText _ALineText;

		// Token: 0x04001A10 RID: 6672
		[SerializeField]
		private LineText _BLineText;

		// Token: 0x04001A11 RID: 6673
		private float _cooltime;

		// Token: 0x04001A12 RID: 6674
		private float _elapsed;

		// Token: 0x04001A13 RID: 6675
		private bool _canRun;

		// Token: 0x04001A14 RID: 6676
		private int _normalLineLenght;
	}
}
