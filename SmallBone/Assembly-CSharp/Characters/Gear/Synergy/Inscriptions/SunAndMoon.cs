using System;
using System.Collections.Generic;
using Characters.Gear.Items;
using Characters.Operations;
using Characters.Player;
using GameResources;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008B7 RID: 2231
	public sealed class SunAndMoon : InscriptionInstance
	{
		// Token: 0x06002F83 RID: 12163 RVA: 0x00002191 File Offset: 0x00000391
		private void Awake()
		{
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x0008E610 File Offset: 0x0008C810
		protected override void Initialize()
		{
			this._targetItemInfo = new List<ValueTuple<string, int>>(2);
			ItemReference itemReference;
			if (!GearResource.instance.TryGetItemReferenceByGuid(this._swordOfSun.AssetGUID, out itemReference))
			{
				return;
			}
			ItemReference itemReference2;
			if (!GearResource.instance.TryGetItemReferenceByGuid(this._ringOfMoon.AssetGUID, out itemReference2))
			{
				return;
			}
			ItemReference itemReference3;
			if (!GearResource.instance.TryGetItemReferenceByGuid(this._shardOfDarkness.AssetGUID, out itemReference3))
			{
				return;
			}
			this._compositions = new SunAndMoon.Composition[]
			{
				new SunAndMoon.Composition
				{
					first = itemReference.name,
					second = itemReference.name,
					result = this._superSolorItem,
					onChanged = new Action(this.OnUpgradeToSuperSolar)
				},
				new SunAndMoon.Composition
				{
					first = itemReference2.name,
					second = itemReference2.name,
					result = this._superLunarItem,
					onChanged = new Action(this.OnUpgradeToSuperLunar)
				},
				new SunAndMoon.Composition
				{
					first = itemReference3.name,
					second = itemReference3.name,
					result = this._unknownDarkness,
					onChanged = new Action(this.OnUpgradeToUnknownDarkness)
				},
				new SunAndMoon.Composition
				{
					first = itemReference.name,
					second = itemReference2.name,
					result = this._brightDawn,
					onChanged = new Action(this.OnUpgradeToBrightDawn)
				},
				new SunAndMoon.Composition
				{
					first = itemReference2.name,
					second = itemReference.name,
					result = this._brightDawn,
					onChanged = new Action(this.OnUpgradeToBrightDawn)
				},
				new SunAndMoon.Composition
				{
					first = itemReference.name,
					second = itemReference3.name,
					result = this._solarEclipse,
					onChanged = new Action(this.OnUpgradeToSolarEclipse)
				},
				new SunAndMoon.Composition
				{
					first = itemReference3.name,
					second = itemReference.name,
					result = this._solarEclipse,
					onChanged = new Action(this.OnUpgradeToSolarEclipse)
				},
				new SunAndMoon.Composition
				{
					first = itemReference2.name,
					second = itemReference3.name,
					result = this._lunarEclipse,
					onChanged = new Action(this.OnUpgradeToLunarEclipse)
				},
				new SunAndMoon.Composition
				{
					first = itemReference3.name,
					second = itemReference2.name,
					result = this._lunarEclipse,
					onChanged = new Action(this.OnUpgradeToLunarEclipse)
				}
			};
			if (this._onChanged != null)
			{
				this._onChanged.Initialize();
			}
			if (this._onChangedToSuperSolor != null)
			{
				this._onChangedToSuperSolor.Initialize();
			}
			if (this._onChangedToSuperLunar != null)
			{
				this._onChangedToSuperLunar.Initialize();
			}
			if (this._onChangedToSolarEclipse != null)
			{
				this._onChangedToSolarEclipse.Initialize();
			}
			if (this._onChangedToLunarEclipse != null)
			{
				this._onChangedToLunarEclipse.Initialize();
			}
			if (this._onChangedToUnknownDarkness != null)
			{
				this._onChangedToUnknownDarkness.Initialize();
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x00002191 File Offset: 0x00000391
		public override void Attach()
		{
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x0008E948 File Offset: 0x0008CB48
		private void HandleOnChanged()
		{
			if (this._upgrading)
			{
				return;
			}
			this._upgrading = true;
			this._targetItemInfo.Clear();
			ItemInventory item = base.character.playerComponents.inventory.item;
			for (int i = 0; i < item.items.Count; i++)
			{
				Item item2 = item.items[i];
				if (!(item2 == null) && (item2.keyword1 == Inscription.Key.SunAndMoon || item2.keyword2 == Inscription.Key.SunAndMoon))
				{
					this._targetItemInfo.Add(new ValueTuple<string, int>(item2.name, i));
				}
			}
			if (this._targetItemInfo.Count < 2)
			{
				return;
			}
			for (int j = 0; j < this._compositions.Length; j++)
			{
				SunAndMoon.Composition composition = this._compositions[j];
				string item3 = this._targetItemInfo[0].Item1;
				if (composition.first.Equals(item3))
				{
					item3 = this._targetItemInfo[1].Item1;
					if (composition.second.Equals(item3))
					{
						int item4 = this._targetItemInfo[0].Item2;
						int item5 = this._targetItemInfo[1].Item2;
						this.MergeItem(item4, item5, composition.result);
						Action onChanged = composition.onChanged;
						if (onChanged == null)
						{
							return;
						}
						onChanged();
						return;
					}
				}
			}
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x0008EAA0 File Offset: 0x0008CCA0
		private void MergeItem(int index1, int index2, AssetReference changeTo)
		{
			ItemInventory item = base.character.playerComponents.inventory.item;
			Item item2 = item.items[index1];
			item.Remove(index2);
			ItemReference itemReference;
			if (!GearResource.instance.TryGetItemReferenceByGuid(changeTo.AssetGUID, out itemReference))
			{
				return;
			}
			ItemRequest itemRequest = itemReference.LoadAsync();
			itemRequest.WaitForCompletion();
			Item item3 = Singleton<Service>.Instance.levelManager.DropItem(itemRequest, Vector3.zero);
			item2.ChangeOnInventory(item3);
			item.Trim();
			this._onChanged.gameObject.SetActive(true);
			this._onChanged.Run(base.character);
			this._upgrading = false;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x0008EB48 File Offset: 0x0008CD48
		private void OnUpgradeToBrightDawn()
		{
			this._onChangedToBrightDawn.gameObject.SetActive(true);
			this._onChangedToBrightDawn.Run(base.character);
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0008EB6C File Offset: 0x0008CD6C
		private void OnUpgradeToSuperSolar()
		{
			this._onChangedToSuperSolor.gameObject.SetActive(true);
			this._onChangedToSuperSolor.Run(base.character);
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x0008EB90 File Offset: 0x0008CD90
		private void OnUpgradeToSuperLunar()
		{
			this._onChangedToSuperLunar.gameObject.SetActive(true);
			this._onChangedToSuperLunar.Run(base.character);
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x0008EBB4 File Offset: 0x0008CDB4
		private void OnUpgradeToSolarEclipse()
		{
			this._onChangedToSolarEclipse.gameObject.SetActive(true);
			this._onChangedToSolarEclipse.Run(base.character);
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x0008EBD8 File Offset: 0x0008CDD8
		private void OnUpgradeToLunarEclipse()
		{
			this._onChangedToLunarEclipse.gameObject.SetActive(true);
			this._onChangedToLunarEclipse.Run(base.character);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x0008EBFC File Offset: 0x0008CDFC
		private void OnUpgradeToUnknownDarkness()
		{
			this._onChangedToUnknownDarkness.gameObject.SetActive(true);
			this._onChangedToUnknownDarkness.Run(base.character);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x0008EC20 File Offset: 0x0008CE20
		public override void Detach()
		{
			base.character.playerComponents.inventory.item.onChanged -= this.HandleOnChanged;
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x0008EC48 File Offset: 0x0008CE48
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			if (this.keyword.isMaxStep)
			{
				this.HandleOnChanged();
			}
		}

		// Token: 0x0400272E RID: 10030
		[Header("2세트 효과")]
		[Space]
		[SerializeField]
		private AssetReference _swordOfSun;

		// Token: 0x0400272F RID: 10031
		[SerializeField]
		private AssetReference _ringOfMoon;

		// Token: 0x04002730 RID: 10032
		[SerializeField]
		private AssetReference _shardOfDarkness;

		// Token: 0x04002731 RID: 10033
		[Space]
		[SerializeField]
		private AssetReference _brightDawn;

		// Token: 0x04002732 RID: 10034
		[SerializeField]
		private AssetReference _superSolorItem;

		// Token: 0x04002733 RID: 10035
		[SerializeField]
		private AssetReference _superLunarItem;

		// Token: 0x04002734 RID: 10036
		[Space]
		[SerializeField]
		private AssetReference _solarEclipse;

		// Token: 0x04002735 RID: 10037
		[SerializeField]
		private AssetReference _lunarEclipse;

		// Token: 0x04002736 RID: 10038
		[SerializeField]
		private AssetReference _unknownDarkness;

		// Token: 0x04002737 RID: 10039
		[Subcomponent(typeof(OperationInfos))]
		[Space]
		[SerializeField]
		private OperationInfos _onChanged;

		// Token: 0x04002738 RID: 10040
		[Header("아이템 별 오퍼레이션")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onChangedToBrightDawn;

		// Token: 0x04002739 RID: 10041
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onChangedToSuperSolor;

		// Token: 0x0400273A RID: 10042
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onChangedToSuperLunar;

		// Token: 0x0400273B RID: 10043
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onChangedToSolarEclipse;

		// Token: 0x0400273C RID: 10044
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onChangedToLunarEclipse;

		// Token: 0x0400273D RID: 10045
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onChangedToUnknownDarkness;

		// Token: 0x0400273E RID: 10046
		private SunAndMoon.Composition[] _compositions;

		// Token: 0x0400273F RID: 10047
		private List<ValueTuple<string, int>> _targetItemInfo;

		// Token: 0x04002740 RID: 10048
		private bool _upgrading;

		// Token: 0x020008B8 RID: 2232
		private class Composition
		{
			// Token: 0x04002741 RID: 10049
			public string first;

			// Token: 0x04002742 RID: 10050
			public string second;

			// Token: 0x04002743 RID: 10051
			public AssetReference result;

			// Token: 0x04002744 RID: 10052
			public Action onChanged;
		}
	}
}
