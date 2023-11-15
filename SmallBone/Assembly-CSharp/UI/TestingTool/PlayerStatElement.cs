using System;
using Characters;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.TestingTool
{
	// Token: 0x02000412 RID: 1042
	public sealed class PlayerStatElement : MonoBehaviour
	{
		// Token: 0x060013C7 RID: 5063 RVA: 0x0003C268 File Offset: 0x0003A468
		public void Set(Stat.Kind kind)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._kind = kind;
			this._stat = new Stat.Values(new Stat.Value[]
			{
				new Stat.Value(Stat.Category.Percent, kind, 1.0),
				new Stat.Value(Stat.Category.PercentPoint, kind, 0.0),
				new Stat.Value(Stat.Category.Constant, kind, 0.0)
			});
			this._name.text = kind.name;
			this._percent.text = "1";
			this._percentPoint.text = "0";
			this._constant.text = "0";
			Stat.Kind.ValueForm valueForm = kind.valueForm;
			if (valueForm != Stat.Kind.ValueForm.Percent)
			{
				if (valueForm == Stat.Kind.ValueForm.Product)
				{
					this._constant.interactable = false;
				}
			}
			else
			{
				this._constant.interactable = false;
			}
			this.UpdateFinal();
			player.stat.AttachValues(this._stat);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0003C368 File Offset: 0x0003A568
		private void UpdateFinal()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			string text = "";
			switch (this._kind.valueForm)
			{
			case Stat.Kind.ValueForm.Constant:
				text = string.Format("{0}", player.stat.GetFinal(this._kind));
				break;
			case Stat.Kind.ValueForm.Percent:
				text = string.Format("{0:0}", player.stat.GetFinal(this._kind) * 100.0);
				break;
			case Stat.Kind.ValueForm.Product:
				text = string.Format("x{0:0.00}", player.stat.GetFinal(this._kind));
				break;
			}
			this._final.text = text;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0003C428 File Offset: 0x0003A628
		public void Apply()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			double value;
			if (!double.TryParse(this._percent.text, out value))
			{
				this._percent.text = string.Format("{0:0}", this._stat.values[0].value);
				this._percentPoint.text = string.Format("{0:0}", this._stat.values[1].value);
				this._constant.text = string.Format("{0:0}", this._stat.values[2].value);
				return;
			}
			double value2;
			if (!double.TryParse(this._percentPoint.text, out value2))
			{
				this._percent.text = string.Format("{0:0}", this._stat.values[0].value);
				this._percentPoint.text = string.Format("{0:0}", this._stat.values[1].value);
				this._constant.text = string.Format("{0:0}", this._stat.values[2].value);
				return;
			}
			double value3;
			if (!double.TryParse(this._constant.text, out value3))
			{
				this._percent.text = string.Format("{0:0}", this._stat.values[0].value);
				this._percentPoint.text = string.Format("{0:0}", this._stat.values[1].value);
				this._constant.text = string.Format("{0:0}", this._stat.values[2].value);
				return;
			}
			this._stat.values[0].value = value;
			this._stat.values[1].value = value2;
			this._stat.values[2].value = value3;
			player.stat.Update();
			this.UpdateFinal();
		}

		// Token: 0x040010C4 RID: 4292
		[SerializeField]
		private TMP_Text _name;

		// Token: 0x040010C5 RID: 4293
		[SerializeField]
		private TMP_InputField _percent;

		// Token: 0x040010C6 RID: 4294
		[SerializeField]
		private TMP_InputField _percentPoint;

		// Token: 0x040010C7 RID: 4295
		[SerializeField]
		private TMP_InputField _constant;

		// Token: 0x040010C8 RID: 4296
		[SerializeField]
		private TMP_Text _final;

		// Token: 0x040010C9 RID: 4297
		private Stat.Kind _kind;

		// Token: 0x040010CA RID: 4298
		private Stat.Values _stat;
	}
}
