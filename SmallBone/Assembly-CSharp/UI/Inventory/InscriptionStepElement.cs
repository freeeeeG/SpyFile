using System;
using System.Collections.Generic;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
	// Token: 0x02000430 RID: 1072
	public sealed class InscriptionStepElement : MonoBehaviour, ILayoutElement
	{
		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x0003ECA6 File Offset: 0x0003CEA6
		public float minWidth
		{
			get
			{
				return this._description.minWidth;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0003ECB3 File Offset: 0x0003CEB3
		public float preferredWidth
		{
			get
			{
				return this._description.preferredWidth;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001471 RID: 5233 RVA: 0x0003ECC0 File Offset: 0x0003CEC0
		public float flexibleWidth
		{
			get
			{
				return this._description.flexibleWidth;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0003ECCD File Offset: 0x0003CECD
		public float minHeight
		{
			get
			{
				return this._description.minHeight;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0003ECDC File Offset: 0x0003CEDC
		public float preferredHeight
		{
			get
			{
				float num = math.clamp(this._description.preferredHeight, this._minHeight, this._maxHeight);
				Vector2 sizeDelta = this._description.rectTransform.sizeDelta;
				sizeDelta.y = num;
				this._description.rectTransform.sizeDelta = sizeDelta;
				return num;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0003ED31 File Offset: 0x0003CF31
		public float flexibleHeight
		{
			get
			{
				return this._description.flexibleHeight;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0003ED3E File Offset: 0x0003CF3E
		public int layoutPriority
		{
			get
			{
				return this._description.layoutPriority;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0003ED4B File Offset: 0x0003CF4B
		public float descriptionPreferredHeight
		{
			get
			{
				return this._description.preferredHeight;
			}
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0003ED58 File Offset: 0x0003CF58
		public void CalculateLayoutInputHorizontal()
		{
			this._description.CalculateLayoutInputHorizontal();
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0003ED65 File Offset: 0x0003CF65
		public void CalculateLayoutInputVertical()
		{
			this._description.CalculateLayoutInputVertical();
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0003ED74 File Offset: 0x0003CF74
		public void Set(Inscription.Key key, IList<int> steps, int stepIndex, bool activated)
		{
			base.gameObject.SetActive(true);
			Inscription inscription = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions[key];
			Color color;
			ColorUtility.TryParseHtmlString(activated ? InscriptionStepElement.activatedColor : InscriptionStepElement.inactivatedColor, out color);
			this._arrow.sprite = (activated ? this._arrowActivated : this._arrowDeactivated);
			this._step.text = steps[stepIndex].ToString();
			this._step.color = color;
			if (this._description != null)
			{
				this._description.text = inscription.GetDescription(stepIndex);
				this._description.color = color;
			}
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0003EE3E File Offset: 0x0003D03E
		public void ClampHeight(float minHeight, float maxHeight)
		{
			this._minHeight = minHeight;
			this._maxHeight = maxHeight;
		}

		// Token: 0x04001163 RID: 4451
		public static readonly string activatedColor = "#755754";

		// Token: 0x04001164 RID: 4452
		public static readonly string inactivatedColor = "#B2977B";

		// Token: 0x04001165 RID: 4453
		[SerializeField]
		private TMP_Text _step;

		// Token: 0x04001166 RID: 4454
		[SerializeField]
		private Image _arrow;

		// Token: 0x04001167 RID: 4455
		[SerializeField]
		private Sprite _arrowActivated;

		// Token: 0x04001168 RID: 4456
		[SerializeField]
		private Sprite _arrowDeactivated;

		// Token: 0x04001169 RID: 4457
		[Space]
		[SerializeField]
		private TextMeshProUGUI _description;

		// Token: 0x0400116A RID: 4458
		private float _minHeight = 30f;

		// Token: 0x0400116B RID: 4459
		private float _maxHeight = 90f;
	}
}
