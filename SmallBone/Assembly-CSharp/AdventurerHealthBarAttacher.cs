using System;
using Characters;
using Scenes;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class AdventurerHealthBarAttacher : MonoBehaviour
{
	// Token: 0x06000015 RID: 21 RVA: 0x00002A9B File Offset: 0x00000C9B
	private void Start()
	{
		this._character.health.onDiedTryCatch += this.OnDied;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002AB9 File Offset: 0x00000CB9
	public void Show()
	{
		Scene<GameBase>.instance.uiManager.adventurerHealthBarUIController.Initialize(this._character, this._adventurerClass);
		Scene<GameBase>.instance.uiManager.adventurerHealthBarUIController.ShowHealthBarOf(this._adventurerClass);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002AF8 File Offset: 0x00000CF8
	private void OnDestroy()
	{
		if (Scene<GameBase>.instance.uiManager == null)
		{
			return;
		}
		Scene<GameBase>.instance.uiManager.adventurerHealthBarUIController.HideDeadUIOf(this._adventurerClass);
		Scene<GameBase>.instance.uiManager.adventurerHealthBarUIController.HideHealthBarOf(this._adventurerClass);
		this._character.health.onDiedTryCatch -= this.OnDied;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002B68 File Offset: 0x00000D68
	private void OnDied()
	{
		Scene<GameBase>.instance.uiManager.adventurerHealthBarUIController.ShowDeadUIOf(this._adventurerClass);
		this._character.health.onDiedTryCatch -= this.OnDied;
	}

	// Token: 0x04000016 RID: 22
	[SerializeField]
	private AdventurerHealthBarUIController.AdventurerClass _adventurerClass;

	// Token: 0x04000017 RID: 23
	[SerializeField]
	private Character _character;
}
