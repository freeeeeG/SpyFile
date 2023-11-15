using System;
using System.Collections;
using Characters.Movements;
using GameResources;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007F4 RID: 2036
	public class PlayerCameraController : MonoBehaviour
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x0600295F RID: 10591 RVA: 0x0007E5E3 File Offset: 0x0007C7E3
		public Vector3 trackPosition
		{
			get
			{
				return this._trackPosition;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x0007E5EB File Offset: 0x0007C7EB
		public float trackSpeed
		{
			get
			{
				return 7f;
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0007E5F4 File Offset: 0x0007C7F4
		private void Awake()
		{
			this._collisionState = this._character.movement.controller.collisionState;
			RayCaster rayCaster = new RayCaster
			{
				direction = Vector2.down,
				distance = 2.75f
			};
			rayCaster.contactFilter.SetLayerMask(393216);
			this._lineCaster.caster = rayCaster;
			this._character.health.onDied += this.OnDie;
			this._deathCamera.targetTexture = CommonResource.instance.deathCamRenderTexture;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x0007E68A File Offset: 0x0007C88A
		private void OnDestroy()
		{
			this._deathCamera.targetTexture = null;
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x0007E698 File Offset: 0x0007C898
		private void OnDie()
		{
			CoroutineProxy.instance.StartCoroutine(this.CRenderDeathCamera());
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x0007E6AB File Offset: 0x0007C8AB
		private IEnumerator CRenderDeathCamera()
		{
			yield return new WaitForSecondsRealtime(1.6f);
			this.RenderDeathCamera();
			yield break;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0007E6BA File Offset: 0x0007C8BA
		public void RenderDeathCamera()
		{
			this._deathCamera.Render();
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x0007E6C8 File Offset: 0x0007C8C8
		private void Update()
		{
			Bounds bounds = this._character.collider.bounds;
			this._lineCaster.start = new Vector2(bounds.min.x, bounds.min.y);
			this._lineCaster.end = new Vector2(bounds.max.x, bounds.min.y);
			this._lineCaster.Cast();
			this._trackPosition = base.transform.position;
			this._trackPosition.y = this._trackPosition.y + 1f;
			RaycastHit2D? raycastHit2D = null;
			ReadonlyBoundedList<RaycastHit2D> results = this._lineCaster.nonAllocCasters[0].results;
			ReadonlyBoundedList<RaycastHit2D> results2 = this._lineCaster.nonAllocCasters[1].results;
			RaycastHit2D raycastHit2D2 = results[0];
			RaycastHit2D raycastHit2D3 = results2[0];
			bool flag = results.Count > 0;
			bool flag2 = results2.Count > 0;
			if (flag && flag2)
			{
				raycastHit2D = new RaycastHit2D?((raycastHit2D2.distance < raycastHit2D3.distance) ? raycastHit2D2 : raycastHit2D3);
			}
			else if (flag && !flag2)
			{
				raycastHit2D = new RaycastHit2D?(raycastHit2D2);
			}
			else if (!flag && flag2)
			{
				raycastHit2D = new RaycastHit2D?(raycastHit2D3);
			}
			if (raycastHit2D == null)
			{
				return;
			}
			this._ground = raycastHit2D.Value.collider;
			if (this._ground.bounds.size.x > 6f)
			{
				this._trackPosition.y = this._ground.bounds.max.y + 2.5f;
			}
		}

		// Token: 0x04002393 RID: 9107
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04002394 RID: 9108
		[SerializeField]
		private Camera _deathCamera;

		// Token: 0x04002395 RID: 9109
		private readonly LineSequenceNonAllocCaster _lineCaster = new LineSequenceNonAllocCaster(1, 2);

		// Token: 0x04002396 RID: 9110
		private Collider2D _ground;

		// Token: 0x04002397 RID: 9111
		private CharacterController2D.CollisionState _collisionState;

		// Token: 0x04002398 RID: 9112
		private Vector3 _trackPosition;
	}
}
