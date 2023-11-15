using System;
using Characters;
using UnityEngine;

namespace Level.Objects.DecorationCharacter
{
	// Token: 0x02000577 RID: 1399
	public class DecorationCharacter : MonoBehaviour
	{
		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001B75 RID: 7029 RVA: 0x000554F9 File Offset: 0x000536F9
		public float speed
		{
			get
			{
				return this._speed;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x00055501 File Offset: 0x00053701
		public DecorationCharacterAnimationController animationController
		{
			get
			{
				return this._animationController;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001B77 RID: 7031 RVA: 0x00055509 File Offset: 0x00053709
		public float deltaTime
		{
			get
			{
				return this.chronometer.animation.DeltaTime();
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x0005551B File Offset: 0x0005371B
		// (set) Token: 0x06001B79 RID: 7033 RVA: 0x00055524 File Offset: 0x00053724
		public Character.LookingDirection lookingDirection
		{
			get
			{
				return this._lookingDirection;
			}
			set
			{
				this.desiringLookingDirection = value;
				this._lookingDirection = value;
				if (this._lookingDirection == Character.LookingDirection.Right)
				{
					this._animationController.parameter.flipX = false;
					this.attachWithFlip.transform.localScale = Vector3.one;
					return;
				}
				this._animationController.parameter.flipX = true;
				this.attachWithFlip.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x000555A3 File Offset: 0x000537A3
		// (set) Token: 0x06001B7B RID: 7035 RVA: 0x000555AB File Offset: 0x000537AB
		public Character.LookingDirection desiringLookingDirection { get; private set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x000555B4 File Offset: 0x000537B4
		public BoxCollider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001B7D RID: 7037 RVA: 0x000555BC File Offset: 0x000537BC
		// (set) Token: 0x06001B7E RID: 7038 RVA: 0x000555C4 File Offset: 0x000537C4
		public GameObject attachWithFlip { get; private set; }

		// Token: 0x06001B7F RID: 7039 RVA: 0x000555CD File Offset: 0x000537CD
		public void SetRenderSortingOrder(int order)
		{
			this._renderer.sortingOrder = order;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000555DC File Offset: 0x000537DC
		private void Awake()
		{
			this._animationController.Initialize(this.chronometer);
			if (this._attach == null)
			{
				this._attach = new GameObject("_attach");
				this._attach.transform.SetParent(base.transform, false);
			}
			if (this.attachWithFlip == null)
			{
				this.attachWithFlip = new GameObject("attachWithFlip");
				this.attachWithFlip.transform.SetParent(this._attach.transform, false);
			}
		}

		// Token: 0x0400179F RID: 6047
		public readonly CharacterChronometer chronometer = new CharacterChronometer();

		// Token: 0x040017A0 RID: 6048
		[SerializeField]
		[GetComponent]
		private DecorationCharacterAnimationController _animationController;

		// Token: 0x040017A1 RID: 6049
		[SerializeField]
		private BoxCollider2D _collider;

		// Token: 0x040017A2 RID: 6050
		[SerializeField]
		private GameObject _attach;

		// Token: 0x040017A3 RID: 6051
		[SerializeField]
		private float _speed = 5f;

		// Token: 0x040017A4 RID: 6052
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x040017A5 RID: 6053
		private Character.LookingDirection _lookingDirection;
	}
}
