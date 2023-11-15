using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010AA RID: 4266
	public sealed class SpiritBottle : MonoBehaviour
	{
		// Token: 0x060052AB RID: 21163 RVA: 0x000F7FED File Offset: 0x000F61ED
		private void Start()
		{
			this._character.health.onDied += this.OnDied;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x000F800C File Offset: 0x000F620C
		private void OnDied()
		{
			this._character.collider.enabled = false;
			this._character.gameObject.SetActive(true);
			this._spriteRenderer.enabled = true;
			SimpleAction onDied = this._onDied;
			if (onDied != null)
			{
				onDied.TryStart();
			}
			this._character.health.onDied -= this.OnDied;
			base.StartCoroutine(this.<OnDied>g__CDestroy|4_0());
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x000F8082 File Offset: 0x000F6282
		[CompilerGenerated]
		private IEnumerator <OnDied>g__CDestroy|4_0()
		{
			while (this._onDied.running)
			{
				yield return null;
			}
			UnityEngine.Object.Destroy(this._character.gameObject);
			yield break;
		}

		// Token: 0x0400425F RID: 16991
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04004260 RID: 16992
		[SerializeField]
		private Character _character;

		// Token: 0x04004261 RID: 16993
		[Subcomponent(true, typeof(SimpleAction))]
		[SerializeField]
		private SimpleAction _onDied;
	}
}
