using System;
using Characters;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Level.BlackMarket
{
	// Token: 0x02000626 RID: 1574
	public class CollectorReroll : InteractiveObject
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06001F92 RID: 8082 RVA: 0x00060020 File Offset: 0x0005E220
		// (remove) Token: 0x06001F93 RID: 8083 RVA: 0x00060058 File Offset: 0x0005E258
		public event Action onInteracted;

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x0006008D File Offset: 0x0005E28D
		private bool canRefreshFree
		{
			get
			{
				return Settings.instance.marketSettings.collectorFreeRefreshCount >= 1 && this._freeRefreshCount < Settings.instance.marketSettings.collectorFreeRefreshCount;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000600BA File Offset: 0x0005E2BA
		private int cost
		{
			get
			{
				if (!this.canRefreshFree)
				{
					return this._costs[Math.Min(this._refreshCount, this._costs.Length - 1)];
				}
				return 0;
			}
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000600E2 File Offset: 0x0005E2E2
		private void OnEnable()
		{
			this._costs = Singleton<Service>.Instance.levelManager.currentChapter.collectorRefreshCosts;
			this._animator.Play(CollectorReroll._idleHash);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x00060110 File Offset: 0x0005E310
		public override void InteractWith(Character character)
		{
			GlobalSettings marketSettings = Settings.instance.marketSettings;
			if (this.canRefreshFree)
			{
				this._freeRefreshCount++;
			}
			else
			{
				if (!GameData.Currency.gold.Consume(this.cost))
				{
					PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactionFailedSound, base.transform.position);
					return;
				}
				this._refreshCount++;
			}
			this._animator.Play(CollectorReroll._interactHash, 0, 0f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			Action action = this.onInteracted;
			if (action != null)
			{
				action();
			}
			UnityEvent onReroll = this._onReroll;
			if (onReroll == null)
			{
				return;
			}
			onReroll.Invoke();
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000601D4 File Offset: 0x0005E3D4
		private void Update()
		{
			string arg = GameData.Currency.gold.Has(this.cost) ? "#FFDE37" : "#FF0000";
			this._text.text = string.Format("{0}(<color={1}>{2}</color>)", Localization.GetLocalizedString("label/interaction/refresh"), arg, this.cost);
		}

		// Token: 0x04001ABD RID: 6845
		private static readonly int _idleHash = Animator.StringToHash("Idle");

		// Token: 0x04001ABE RID: 6846
		private static readonly int _interactHash = Animator.StringToHash("Interact");

		// Token: 0x04001AC0 RID: 6848
		private int[] _costs;

		// Token: 0x04001AC1 RID: 6849
		private int _refreshCount;

		// Token: 0x04001AC2 RID: 6850
		private int _freeRefreshCount;

		// Token: 0x04001AC3 RID: 6851
		private const string _goldColor = "#FFDE37";

		// Token: 0x04001AC4 RID: 6852
		private const string _notEnoughGoldColor = "#FF0000";

		// Token: 0x04001AC5 RID: 6853
		[SerializeField]
		private SoundInfo _interactionFailedSound;

		// Token: 0x04001AC6 RID: 6854
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001AC7 RID: 6855
		[SerializeField]
		private TMP_Text _text;

		// Token: 0x04001AC8 RID: 6856
		[SerializeField]
		private UnityEvent _onReroll;
	}
}
