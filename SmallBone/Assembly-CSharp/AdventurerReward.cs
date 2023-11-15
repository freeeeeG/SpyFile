using System;
using Characters;
using Level;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class AdventurerReward : MonoBehaviour
{
	// Token: 0x0600001A RID: 26 RVA: 0x00002BA0 File Offset: 0x00000DA0
	private void Start()
	{
		Character[] characters = this._characters;
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].health.onDiedTryCatch += this.CountAdventurerDead;
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002BDC File Offset: 0x00000DDC
	private void CountAdventurerDead()
	{
		int num = 0;
		foreach (Character character in this._characters)
		{
			if (character.health.dead)
			{
				num++;
			}
			else
			{
				this._lastDiedCharacter = character;
			}
		}
		if (num >= this._characters.Length)
		{
			this.DropReward();
			this.RemoveCountEvent();
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002C38 File Offset: 0x00000E38
	private void DropReward()
	{
		Potion potion = this._potionPossibilities.Get();
		if (potion != null)
		{
			Singleton<Service>.Instance.levelManager.DropPotion(potion, this._lastDiedCharacter.transform.position);
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002C7C File Offset: 0x00000E7C
	private void RemoveCountEvent()
	{
		Character[] characters = this._characters;
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].health.onDiedTryCatch -= this.CountAdventurerDead;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002CB7 File Offset: 0x00000EB7
	private void OnDestroy()
	{
		this.RemoveCountEvent();
	}

	// Token: 0x04000018 RID: 24
	[SerializeField]
	private Character[] _characters;

	// Token: 0x04000019 RID: 25
	[SerializeField]
	private PotionPossibilities _potionPossibilities;

	// Token: 0x0400001A RID: 26
	private Character _lastDiedCharacter;
}
