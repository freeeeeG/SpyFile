using System;
using System.Collections.Generic;
using Data;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x02000585 RID: 1413
	public sealed class AnotherWitchSelector : MonoBehaviour
	{
		// Token: 0x06001BBA RID: 7098 RVA: 0x0005645C File Offset: 0x0005465C
		private void Awake()
		{
			this._originalInteractive.SetActive(false);
			AnotherWitchSelector.WitchsByLevel[] witchByHardmodeLevel = this._witchByHardmodeLevel;
			for (int i = 0; i < witchByHardmodeLevel.Length; i++)
			{
				witchByHardmodeLevel[i].Hide();
			}
			int clearedLevel = GameData.HardmodeProgress.clearedLevel;
			if (clearedLevel < 0)
			{
				this._originalWitchAnimator.Play(this._blackOutHash);
				return;
			}
			DarktechManager.DarktechByLevel darktechByLevel = Singleton<DarktechManager>.Instance.darkTechByLevels[clearedLevel];
			if (!Singleton<DarktechManager>.Instance.IsActivated(darktechByLevel.types[0]))
			{
				this._originalWitchAnimator.Play(this._originalIdleNoWitchHash);
				this._witchByHardmodeLevel[clearedLevel].GetCutsceneWitch().SetActive(true);
				return;
			}
			if (MMMaths.Chance(1f / ((float)clearedLevel + 0.5f)))
			{
				this._originalInteractive.SetActive(true);
				this._originalWitchAnimator.Play(this._originalIdleHash);
				return;
			}
			this._originalWitchAnimator.Play(this._originalIdleNoWitchHash);
			List<AnotherWitchSelector.WitchsByLevel> list = new List<AnotherWitchSelector.WitchsByLevel>();
			for (int j = 0; j < clearedLevel; j++)
			{
				list.Add(this._witchByHardmodeLevel[j]);
			}
			list.Random<AnotherWitchSelector.WitchsByLevel>().GetRandomWitch().SetActive(true);
		}

		// Token: 0x040017D7 RID: 6103
		private readonly int _originalIdleHash = Animator.StringToHash("Original_Idle");

		// Token: 0x040017D8 RID: 6104
		private readonly int _originalIdleNoWitchHash = Animator.StringToHash("Original_Idle_NoWitch");

		// Token: 0x040017D9 RID: 6105
		private readonly int _blackOutHash = Animator.StringToHash("BlackOut_Loop");

		// Token: 0x040017DA RID: 6106
		[SerializeField]
		private Animator _originalWitchAnimator;

		// Token: 0x040017DB RID: 6107
		[SerializeField]
		private GameObject _originalInteractive;

		// Token: 0x040017DC RID: 6108
		[SerializeField]
		private AnotherWitchSelector.WitchsByLevel[] _witchByHardmodeLevel;

		// Token: 0x02000586 RID: 1414
		[Serializable]
		private class WitchsByLevel
		{
			// Token: 0x06001BBC RID: 7100 RVA: 0x000565AD File Offset: 0x000547AD
			public GameObject GetCutsceneWitch()
			{
				return this._witchs[0];
			}

			// Token: 0x06001BBD RID: 7101 RVA: 0x000565B7 File Offset: 0x000547B7
			public GameObject GetRandomWitch()
			{
				return this._witchs.Random<GameObject>();
			}

			// Token: 0x06001BBE RID: 7102 RVA: 0x000565C4 File Offset: 0x000547C4
			public void Hide()
			{
				GameObject[] witchs = this._witchs;
				for (int i = 0; i < witchs.Length; i++)
				{
					witchs[i].SetActive(false);
				}
			}

			// Token: 0x040017DD RID: 6109
			[SerializeField]
			private GameObject[] _witchs;
		}
	}
}
