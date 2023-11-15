using System;
using System.Collections;
using Characters.Gear;
using GameResources;
using Level;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TestingTool
{
	// Token: 0x02000401 RID: 1025
	public class GearListElement : MonoBehaviour
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x0003A8C5 File Offset: 0x00038AC5
		// (set) Token: 0x06001357 RID: 4951 RVA: 0x0003A8CD File Offset: 0x00038ACD
		public Gear.Type type { get; private set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x0003A8D6 File Offset: 0x00038AD6
		// (set) Token: 0x06001359 RID: 4953 RVA: 0x0003A8DE File Offset: 0x00038ADE
		public GearReference gearReference { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x0003A8E7 File Offset: 0x00038AE7
		public string text
		{
			get
			{
				return this._text.text;
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0003A8F4 File Offset: 0x00038AF4
		public void Set(GearReference gearReference)
		{
			this.gearReference = gearReference;
			this.type = gearReference.type;
			this._text.text = Localization.GetLocalizedString(gearReference.displayNameKey);
			if (string.IsNullOrWhiteSpace(this._text.text))
			{
				this._text.text = gearReference.name;
			}
			if (gearReference.obtainable)
			{
				this._text.color = GearListElement._rarityColorTable[gearReference.rarity];
			}
			else
			{
				this._text.color = Color.gray;
			}
			this._thumbnail.sprite = gearReference.thumbnail;
			this._button.onClick.AddListener(delegate
			{
				this.StartCoroutine(this.CDropGear(gearReference));
			});
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0003A9E6 File Offset: 0x00038BE6
		private IEnumerator CDropGear(GearReference gearReference)
		{
			GearRequest request = gearReference.LoadAsync();
			while (!request.isDone)
			{
				yield return null;
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			levelManager.DropGear(request, levelManager.player.transform.position);
			yield break;
		}

		// Token: 0x04001042 RID: 4162
		private static readonly EnumArray<Rarity, Color> _rarityColorTable = new EnumArray<Rarity, Color>(new Color[]
		{
			Color.black,
			Color.blue,
			Color.magenta,
			Color.red
		});

		// Token: 0x04001043 RID: 4163
		[SerializeField]
		private Button _button;

		// Token: 0x04001044 RID: 4164
		[SerializeField]
		private Image _thumbnail;

		// Token: 0x04001045 RID: 4165
		[SerializeField]
		private TMP_Text _text;
	}
}
