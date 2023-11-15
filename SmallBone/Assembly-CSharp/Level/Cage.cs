using System;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x02000498 RID: 1176
	public class Cage : MonoBehaviour
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06001663 RID: 5731 RVA: 0x00046590 File Offset: 0x00044790
		// (remove) Token: 0x06001664 RID: 5732 RVA: 0x000465C8 File Offset: 0x000447C8
		public event Action onDestroyed;

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x000465FD File Offset: 0x000447FD
		public Collider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00046605 File Offset: 0x00044805
		private void Awake()
		{
			this._prop.onDestroy += this.Destroy;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00046620 File Offset: 0x00044820
		public void OverrideProp(Prop newProp, SpriteRenderer newBehind, Sprite behindWreck)
		{
			this._prop.onDestroy -= this.Destroy;
			this._prop.gameObject.SetActive(false);
			this._behind.gameObject.SetActive(false);
			this._prop = newProp;
			this._prop.onDestroy += this.Destroy;
			this._prop.gameObject.SetActive(true);
			this._behind = newBehind;
			this._behindWreckage = behindWreck;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000466A3 File Offset: 0x000448A3
		public void Activate()
		{
			this.collider.enabled = true;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000466B1 File Offset: 0x000448B1
		public void Deactivate()
		{
			this.collider.enabled = false;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000466BF File Offset: 0x000448BF
		public void Destroy()
		{
			this._collider.enabled = false;
			this._behind.sprite = this._behindWreckage;
			Action action = this.onDestroyed;
			if (action != null)
			{
				action();
			}
			this.Deactivate();
		}

		// Token: 0x040013A6 RID: 5030
		[SerializeField]
		private Target _target;

		// Token: 0x040013A7 RID: 5031
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040013A8 RID: 5032
		[SerializeField]
		private SpriteRenderer _behind;

		// Token: 0x040013A9 RID: 5033
		[SerializeField]
		private Sprite _behindWreckage;

		// Token: 0x040013AA RID: 5034
		[SerializeField]
		private Prop _prop;
	}
}
