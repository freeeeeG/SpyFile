using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities.Darks;
using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs
{
	// Token: 0x0200021E RID: 542
	public sealed class OpenDarkEnemyHealthBar : Event
	{
		// Token: 0x06000AB3 RID: 2739 RVA: 0x0001D198 File Offset: 0x0001B398
		private void Awake()
		{
			if (this._character == null)
			{
				this._character = base.GetComponentInParent<Character>();
			}
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0001D1B4 File Offset: 0x0001B3B4
		public override void Run()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001D1C3 File Offset: 0x0001B3C3
		private IEnumerator CRun()
		{
			while (!this._attcher.attached)
			{
				yield return null;
			}
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Open(this._character, this._attcher.displayName);
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.AddTarget(this._character, this._attcher.displayName);
			this._character.health.onDiedTryCatch += delegate()
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.RemoveTarget(this._character);
				if (Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.attached.Count == 0)
				{
					Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Close();
					return;
				}
				IDictionary<Character, string> attached = Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.attached;
				if (attached.Count > 0)
				{
					Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Set(attached.Random<KeyValuePair<Character, string>>().Key);
				}
			};
			yield break;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0001D1D2 File Offset: 0x0001B3D2
		private void OnDestroy()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.Close();
			Scene<GameBase>.instance.uiManager.headupDisplay.darkEnemyHealthBar.RemoveTarget(this._character);
		}

		// Token: 0x040008C0 RID: 2240
		[SerializeField]
		private Character _character;

		// Token: 0x040008C1 RID: 2241
		[SerializeField]
		private DarkAbilityAttacher _attcher;
	}
}
