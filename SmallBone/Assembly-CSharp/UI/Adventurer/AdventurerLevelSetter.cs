using System;
using System.Collections;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

namespace UI.Adventurer
{
	// Token: 0x0200045B RID: 1115
	public class AdventurerLevelSetter : MonoBehaviour
	{
		// Token: 0x06001533 RID: 5427 RVA: 0x00042BD4 File Offset: 0x00040DD4
		private void OnEnable()
		{
			base.StartCoroutine(this.CAnimateLevel());
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x00042BE3 File Offset: 0x00040DE3
		private IEnumerator CAnimateLevel()
		{
			int currentLevel = 1;
			Vector2Int adventurerLevel = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.adventurerLevel;
			int targetLevel = UnityEngine.Random.Range(adventurerLevel.x, adventurerLevel.y);
			float duration = (float)targetLevel * 0.01f;
			float time = 0f;
			while (time < duration)
			{
				time += Chronometer.global.deltaTime;
				this._levelText.text = Mathf.Lerp((float)currentLevel, (float)targetLevel, time).ToString("0");
				yield return null;
			}
			this._levelText.text = targetLevel.ToString();
			yield break;
		}

		// Token: 0x04001288 RID: 4744
		[SerializeField]
		private TMP_Text _levelText;

		// Token: 0x04001289 RID: 4745
		private float _delay;
	}
}
