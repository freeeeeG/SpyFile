using System;
using System.Collections;
using Characters;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200066B RID: 1643
	public class MonsterSpawner : MonoBehaviour
	{
		// Token: 0x060020EE RID: 8430 RVA: 0x000635CD File Offset: 0x000617CD
		private void Awake()
		{
			this._prop.onDestroy += this.SpawnCharacter;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000635E8 File Offset: 0x000617E8
		private void SpawnCharacter()
		{
			this._destroyedBody.SetActive(true);
			Character character = (this._target == MonsterSpawner.Target.LooseSubject) ? this._looseSubject : this._strangeSubject;
			character.gameObject.SetActive(true);
			if (this._containInWave)
			{
				Map.Instance.waveContainer.Attach(character);
			}
			character.collider.enabled = true;
			character.StartCoroutineWithReference(this.CAttachCinematic(character));
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00063656 File Offset: 0x00061856
		private IEnumerator CAttachCinematic(Character character)
		{
			character.cinematic.Attach(this);
			yield return new WaitForSeconds(this._cinematicDuration.value);
			character.cinematic.Detach(this);
			yield break;
		}

		// Token: 0x04001C02 RID: 7170
		[SerializeField]
		private Prop _prop;

		// Token: 0x04001C03 RID: 7171
		[SerializeField]
		private GameObject _destroyedBody;

		// Token: 0x04001C04 RID: 7172
		[SerializeField]
		private Character _looseSubject;

		// Token: 0x04001C05 RID: 7173
		[SerializeField]
		private Character _strangeSubject;

		// Token: 0x04001C06 RID: 7174
		[SerializeField]
		private bool _containInWave = true;

		// Token: 0x04001C07 RID: 7175
		[SerializeField]
		private MonsterSpawner.Target _target;

		// Token: 0x04001C08 RID: 7176
		[SerializeField]
		private CustomFloat _cinematicDuration;

		// Token: 0x0200066C RID: 1644
		private enum Target
		{
			// Token: 0x04001C0A RID: 7178
			LooseSubject,
			// Token: 0x04001C0B RID: 7179
			StrangeSubject
		}
	}
}
