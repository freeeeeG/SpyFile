using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001123 RID: 4387
	public class ReviveScarecrowOnDie : MonoBehaviour
	{
		// Token: 0x06005550 RID: 21840 RVA: 0x000FEA78 File Offset: 0x000FCC78
		private void Start()
		{
			Character[] target = this._target;
			for (int i = 0; i < target.Length; i++)
			{
				Character character = target[i];
				character.health.onDied += delegate()
				{
					this.StartCoroutine(this.Revive(character));
				};
			}
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x000FEACC File Offset: 0x000FCCCC
		private IEnumerator Revive(Character target)
		{
			yield return Chronometer.global.WaitForSeconds(3f);
			Character spawned = UnityEngine.Object.Instantiate<Character>(this._origin, target.transform.position, Quaternion.identity, base.transform);
			ScareCrawAI componentInChildren = spawned.GetComponentInChildren<ScareCrawAI>();
			spawned.ForceToLookAt(target.lookingDirection);
			spawned.gameObject.SetActive(true);
			spawned.health.onDied += delegate()
			{
				this.StartCoroutine(this.Revive(spawned));
			};
			componentInChildren.Appear();
			UnityEngine.Object.Destroy(target.gameObject);
			yield break;
		}

		// Token: 0x0400445A RID: 17498
		[SerializeField]
		private Character[] _target;

		// Token: 0x0400445B RID: 17499
		[SerializeField]
		private Character _origin;
	}
}
