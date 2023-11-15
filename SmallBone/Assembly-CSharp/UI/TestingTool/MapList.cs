using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using GameResources;
using InControl;
using Level;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x02000407 RID: 1031
	public class MapList : MonoBehaviour
	{
		// Token: 0x06001377 RID: 4983 RVA: 0x0003ACB4 File Offset: 0x00038EB4
		private void Awake()
		{
			this._currentChapterType = Singleton<Service>.Instance.levelManager.currentChapter.type;
			this.LoadMaps(this._currentChapterType);
			this._enemy.onValueChanged.AddListener(delegate(bool value)
			{
				Map.TestingTool.safeZone = !value;
			});
			this._fieldNPC.onValueChanged.AddListener(delegate(bool value)
			{
				Map.TestingTool.fieldNPC = value;
			});
			this._darkEnemy.onValueChanged.AddListener(delegate(bool value)
			{
				Map.TestingTool.darkenemy = value;
			});
			this._tutorial.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Tutorial);
			});
			this._castle.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Castle);
			});
			this._chapter1.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Chapter1);
			});
			this._chapter2.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Chapter2);
			});
			this._chapter3.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Chapter3);
			});
			this._chapter4.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Chapter4);
			});
			this._chapter5.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.Chapter5);
			});
			this._hardmodeCastle.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.HardmodeCastle);
			});
			this._hardChapter1.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.HardmodeChapter1);
			});
			this._hardChapter2.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.HardmodeChapter2);
			});
			this._hardChapter3.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.HardmodeChapter3);
			});
			this._hardChapter4.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.HardmodeChapter4);
			});
			this._hardChapter5.onClick.AddListener(delegate
			{
				this.FilterMapList(Chapter.Type.HardmodeChapter5);
			});
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0003AEE0 File Offset: 0x000390E0
		private void LoadMaps(Chapter.Type chapterType)
		{
			MapList.<>c__DisplayClass24_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.chapterType = chapterType;
			if (this._chpaterLoaded[CS$<>8__locals1.chapterType])
			{
				return;
			}
			this._chpaterLoaded[CS$<>8__locals1.chapterType] = true;
			CS$<>8__locals1.stringBuilder = new StringBuilder();
			CS$<>8__locals1.substringIndex = "Assets/Level/".Length;
			CS$<>8__locals1.prefabExtensionLength = ".prefab".Length;
			AssetReference assetReference = LevelResource.instance.chapters[(int)CS$<>8__locals1.chapterType];
			bool flag = false;
			Chapter chapter;
			if (assetReference.IsValid())
			{
				chapter = (Chapter)assetReference.Asset;
			}
			else
			{
				flag = true;
				chapter = assetReference.LoadAssetAsync<Chapter>().WaitForCompletion();
			}
			foreach (AssetReference assetReference2 in chapter.stages)
			{
				bool flag2 = false;
				IStageInfo stageInfo;
				if (assetReference2.IsValid())
				{
					stageInfo = (IStageInfo)assetReference2.Asset;
				}
				else
				{
					flag2 = true;
					stageInfo = assetReference2.LoadAssetAsync<IStageInfo>().WaitForCompletion();
				}
				foreach (MapReference map in stageInfo.maps)
				{
					this.<LoadMaps>g__AddToMapList|24_0(map, ref CS$<>8__locals1);
				}
				StageInfo stageInfo2 = stageInfo as StageInfo;
				if (stageInfo2 != null)
				{
					this.<LoadMaps>g__AddToMapList|24_0(stageInfo2.entry.reference, ref CS$<>8__locals1);
					this.<LoadMaps>g__AddToMapList|24_0(stageInfo2.terminal.reference, ref CS$<>8__locals1);
				}
				if (flag2)
				{
					Addressables.Release<IStageInfo>(stageInfo);
				}
			}
			if (flag)
			{
				Addressables.Release<Chapter>(chapter);
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0003B05C File Offset: 0x0003925C
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (!activeDevice.LeftBumper.WasPressed && !activeDevice.LeftTrigger.WasPressed)
			{
				if (activeDevice.RightBumper.WasPressed || activeDevice.RightTrigger.WasPressed)
				{
					if (this._currentChapterType == EnumValues<Chapter.Type>.Values.Last<Chapter.Type>())
					{
						this.FilterMapList(EnumValues<Chapter.Type>.Values.First<Chapter.Type>());
						return;
					}
					this.FilterMapList(this._currentChapterType + 1);
				}
				return;
			}
			if (this._currentChapterType == Chapter.Type.Castle)
			{
				this.FilterMapList(EnumValues<Chapter.Type>.Values.Last<Chapter.Type>());
				return;
			}
			this.FilterMapList(this._currentChapterType - 1);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0003B100 File Offset: 0x00039300
		private void FilterMapList(Chapter.Type chapter)
		{
			this.LoadMaps(chapter);
			this._currentChapterType = chapter;
			this._currentChapterFilterText.text = chapter.ToString();
			foreach (MapListElement mapListElement in this._mapListElements)
			{
				mapListElement.gameObject.SetActive(mapListElement.chapter == chapter);
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0003B220 File Offset: 0x00039420
		[CompilerGenerated]
		private void <LoadMaps>g__AddToMapList|24_0(MapReference map, ref MapList.<>c__DisplayClass24_0 A_2)
		{
			if (map.IsNullOrEmpty())
			{
				return;
			}
			MapListElement mapListElement = UnityEngine.Object.Instantiate<MapListElement>(this._mapListElementPrefab, this._gridContainer);
			A_2.stringBuilder.Clear();
			A_2.stringBuilder.Append(map.path);
			int num = map.path.IndexOf('/', A_2.substringIndex) + 1;
			mapListElement.Set(A_2.chapterType, A_2.stringBuilder.ToString(num, A_2.stringBuilder.Length - num - A_2.prefabExtensionLength), map);
			this._mapListElements.Add(mapListElement);
		}

		// Token: 0x04001055 RID: 4181
		[SerializeField]
		private MapListElement _mapListElementPrefab;

		// Token: 0x04001056 RID: 4182
		[SerializeField]
		private Button _back;

		// Token: 0x04001057 RID: 4183
		[SerializeField]
		private TMP_Text _currentChapterFilterText;

		// Token: 0x04001058 RID: 4184
		[SerializeField]
		[Header("옵션")]
		private Toggle _enemy;

		// Token: 0x04001059 RID: 4185
		[SerializeField]
		private Toggle _fieldNPC;

		// Token: 0x0400105A RID: 4186
		[SerializeField]
		private Toggle _darkEnemy;

		// Token: 0x0400105B RID: 4187
		[Header("노말 모드")]
		[SerializeField]
		private Button _tutorial;

		// Token: 0x0400105C RID: 4188
		[SerializeField]
		private Button _castle;

		// Token: 0x0400105D RID: 4189
		[SerializeField]
		private Button _chapter1;

		// Token: 0x0400105E RID: 4190
		[SerializeField]
		private Button _chapter2;

		// Token: 0x0400105F RID: 4191
		[SerializeField]
		private Button _chapter3;

		// Token: 0x04001060 RID: 4192
		[SerializeField]
		private Button _chapter4;

		// Token: 0x04001061 RID: 4193
		[SerializeField]
		private Button _chapter5;

		// Token: 0x04001062 RID: 4194
		[SerializeField]
		[Header("하드 모드")]
		private Button _hardmodeCastle;

		// Token: 0x04001063 RID: 4195
		[SerializeField]
		private Button _hardChapter1;

		// Token: 0x04001064 RID: 4196
		[SerializeField]
		private Button _hardChapter2;

		// Token: 0x04001065 RID: 4197
		[SerializeField]
		private Button _hardChapter3;

		// Token: 0x04001066 RID: 4198
		[SerializeField]
		private Button _hardChapter4;

		// Token: 0x04001067 RID: 4199
		[SerializeField]
		private Button _hardChapter5;

		// Token: 0x04001068 RID: 4200
		[SerializeField]
		private Transform _gridContainer;

		// Token: 0x04001069 RID: 4201
		private readonly List<MapListElement> _mapListElements = new List<MapListElement>();

		// Token: 0x0400106A RID: 4202
		private Chapter.Type _currentChapterType;

		// Token: 0x0400106B RID: 4203
		private EnumArray<Chapter.Type, bool> _chpaterLoaded = new EnumArray<Chapter.Type, bool>();
	}
}
