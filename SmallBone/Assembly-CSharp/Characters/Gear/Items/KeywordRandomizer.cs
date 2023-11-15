using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Synergy.Inscriptions;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Items
{
	// Token: 0x020008FE RID: 2302
	public class KeywordRandomizer : MonoBehaviour
	{
		// Token: 0x0600311D RID: 12573 RVA: 0x00092FD0 File Offset: 0x000911D0
		private void Awake()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 716722307 + currentChapter.type * (Chapter.Type)16 + currentChapter.stageIndex));
			this.UpdateKeword();
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x00093020 File Offset: 0x00091220
		private void UpdateKeword()
		{
			List<Inscription.Key> list = Inscription.keys.ToList<Inscription.Key>();
			list.Remove(Inscription.Key.None);
			list.Remove(Inscription.Key.SunAndMoon);
			list.Remove(Inscription.Key.Masterpiece);
			list.Remove(Inscription.Key.Omen);
			list.Remove(Inscription.Key.Sin);
			this._item.keyword1 = list.Random(this._random);
			if (this._type == KeywordRandomizer.Type.Normal)
			{
				list.Remove(this._item.keyword1);
				this._item.keyword2 = list.Random(this._random);
				return;
			}
			if (this._type == KeywordRandomizer.Type.Clone)
			{
				this._item.keyword2 = this._item.keyword1;
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000930CC File Offset: 0x000912CC
		public void UpdateKeword(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.UpdateKeword();
			}
		}

		// Token: 0x04002866 RID: 10342
		private const int _randomSeed = 716722307;

		// Token: 0x04002867 RID: 10343
		[SerializeField]
		private Item _item;

		// Token: 0x04002868 RID: 10344
		[SerializeField]
		private KeywordRandomizer.Type _type;

		// Token: 0x04002869 RID: 10345
		private System.Random _random;

		// Token: 0x020008FF RID: 2303
		private enum Type
		{
			// Token: 0x0400286B RID: 10347
			Normal,
			// Token: 0x0400286C RID: 10348
			Clone
		}
	}
}
