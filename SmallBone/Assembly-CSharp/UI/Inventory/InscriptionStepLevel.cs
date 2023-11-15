using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
	// Token: 0x02000431 RID: 1073
	public sealed class InscriptionStepLevel : MonoBehaviour
	{
		// Token: 0x0600147D RID: 5245 RVA: 0x0003EE84 File Offset: 0x0003D084
		public void Set(Inscription.Key key, bool fullactiveColorChange = false)
		{
			Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
			ReadOnlyCollection<int> steps = inscription.steps;
			int step = inscription.step;
			switch (inscription.steps.Count)
			{
			case 0:
			case 1:
				break;
			case 2:
				this._twoStep.Deactivate();
				this._threeStep.Deactivate();
				this._oneStep.Activate(steps, step, fullactiveColorChange);
				return;
			case 3:
				this._oneStep.Deactivate();
				this._threeStep.Deactivate();
				this._twoStep.Activate(steps, step, fullactiveColorChange);
				return;
			case 4:
				this._oneStep.Deactivate();
				this._twoStep.Deactivate();
				this._threeStep.Activate(steps, step, fullactiveColorChange);
				break;
			default:
				return;
			}
		}

		// Token: 0x0400116C RID: 4460
		[SerializeField]
		private InscriptionStepLevel.Steps _oneStep;

		// Token: 0x0400116D RID: 4461
		[SerializeField]
		private InscriptionStepLevel.Steps _twoStep;

		// Token: 0x0400116E RID: 4462
		[SerializeField]
		private InscriptionStepLevel.Steps _threeStep;

		// Token: 0x02000432 RID: 1074
		[Serializable]
		private class Steps
		{
			// Token: 0x0600147F RID: 5247 RVA: 0x0003EF5C File Offset: 0x0003D15C
			public void Activate(IList<int> steps, int stepIndex, bool fullactiveColorChange)
			{
				if (this._stepTexts.Length != steps.Count - 1)
				{
					Debug.LogError("각인의 개수가 잘못 전달되었습니다.");
					return;
				}
				this._parent.SetActive(true);
				for (int i = 0; i < this._stepTexts.Length; i++)
				{
					this._stepTexts[i].text = steps[i + 1].ToString();
					string htmlString = (stepIndex == i + 1) ? InscriptionStepLevel.Steps.activatedColor : InscriptionStepLevel.Steps.inactivatedColor;
					if (fullactiveColorChange && i == steps.Count - 2 && stepIndex == i + 1)
					{
						htmlString = "#fbd53e";
					}
					Color color;
					ColorUtility.TryParseHtmlString(htmlString, out color);
					this._stepTexts[i].color = color;
				}
			}

			// Token: 0x06001480 RID: 5248 RVA: 0x0003F007 File Offset: 0x0003D207
			public void Deactivate()
			{
				this._parent.SetActive(false);
			}

			// Token: 0x0400116F RID: 4463
			private const string fullActiveLevel = "#fbd53e";

			// Token: 0x04001170 RID: 4464
			public static readonly string activatedColor = "#E6D2C0";

			// Token: 0x04001171 RID: 4465
			public static readonly string inactivatedColor = "#9C8161";

			// Token: 0x04001172 RID: 4466
			[SerializeField]
			private GameObject _parent;

			// Token: 0x04001173 RID: 4467
			[SerializeField]
			private TMP_Text[] _stepTexts;

			// Token: 0x04001174 RID: 4468
			[SerializeField]
			private GameObject[] _arrowImages;
		}
	}
}
