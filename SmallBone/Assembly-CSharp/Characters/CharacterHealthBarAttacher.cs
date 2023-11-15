using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006C1 RID: 1729
	public class CharacterHealthBarAttacher : MonoBehaviour
	{
		// Token: 0x060022BE RID: 8894 RVA: 0x00068B00 File Offset: 0x00066D00
		private void Start()
		{
			if (this._character != null)
			{
				if (this._instanitated != null)
				{
					UnityEngine.Object.Destroy(this._instanitated.gameObject);
				}
				this._instanitated = UnityEngine.Object.Instantiate<CharacterHealthBar>(this._healthBar, this._parent ?? base.transform);
				this._instanitated.transform.position = base.transform.position + MMMaths.Vector2ToVector3(this._offset);
				this._instanitated.Initialize(this._character);
				this._instanitated.SetWidth(this._character.collider.size.x * 32f);
			}
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00068BBF File Offset: 0x00066DBF
		private void OnDrawGizmosSelected()
		{
			if (!Application.isPlaying)
			{
				Gizmos.DrawIcon(base.transform.position + MMMaths.Vector2ToVector3(this._offset), "healthbar");
			}
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x00068BF0 File Offset: 0x00066DF0
		public void SetHealthBar(CharacterHealthBar newHealthBar)
		{
			if (this._instanitated != null)
			{
				UnityEngine.Object.Destroy(this._instanitated.gameObject);
			}
			this._instanitated = UnityEngine.Object.Instantiate<CharacterHealthBar>(newHealthBar, this._parent ?? base.transform);
			this._instanitated.transform.position = base.transform.position + MMMaths.Vector2ToVector3(this._offset);
			this._instanitated.Initialize(this._character);
			this._instanitated.SetWidth(this._character.collider.size.x * 32f);
		}

		// Token: 0x04001D9B RID: 7579
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001D9C RID: 7580
		[SerializeField]
		private CharacterHealthBar _healthBar;

		// Token: 0x04001D9D RID: 7581
		[SerializeField]
		private Transform _parent;

		// Token: 0x04001D9E RID: 7582
		[SerializeField]
		private Vector2 _offset;

		// Token: 0x04001D9F RID: 7583
		private CharacterHealthBar _instanitated;
	}
}
