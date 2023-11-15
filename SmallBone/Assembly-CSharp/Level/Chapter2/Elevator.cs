using System;
using System.Collections;
using Characters;
using FX;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Chapter2
{
	// Token: 0x02000630 RID: 1584
	public class Elevator : InteractiveObject
	{
		// Token: 0x06001FC7 RID: 8135 RVA: 0x000608B0 File Offset: 0x0005EAB0
		private void OnEnable()
		{
			this.Intro();
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000608B8 File Offset: 0x0005EAB8
		public void Intro()
		{
			this._behind.Play(this._introHash);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._elevatorUp, base.transform.position);
			base.StartCoroutine(this.WaitForIntro());
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000608F4 File Offset: 0x0005EAF4
		private IEnumerator WaitForIntro()
		{
			yield return Chronometer.global.WaitForSeconds(this._introLength);
			this._front.gameObject.SetActive(true);
			this._front.Play(this._frontIdleHash);
			this._behind.Play(this._backIdleHash);
			this._interactable = true;
			yield break;
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x00060903 File Offset: 0x0005EB03
		private IEnumerator WalkToIn()
		{
			this._block.SetActive(false);
			yield return this.MoveTo(this._destination.position);
			this.Close();
			yield break;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x00060914 File Offset: 0x0005EB14
		public void Close()
		{
			this._front.Play(this._frontCloseHash);
			this._behind.Play(this._backCloseHash);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._elevatorDown, base.transform.position);
			base.StartCoroutine(this.WaitForClose());
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0006096C File Offset: 0x0005EB6C
		private IEnumerator WaitForClose()
		{
			yield return Chronometer.global.WaitForSeconds(this._closeLength);
			this._front.Play(this._frontOutroHash);
			this._behind.gameObject.SetActive(false);
			Scene<GameBase>.instance.cameraController.StartTrack(this._cameraPoint);
			base.StartCoroutine(this.MovePlayer());
			yield return Chronometer.global.WaitForSeconds(this._outroLength);
			yield break;
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0006097C File Offset: 0x0005EB7C
		private void LoadNextMap()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._interactSound, base.transform.position);
			Singleton<Service>.Instance.levelManager.LoadNextMap(NodeIndex.Node1);
			Character player = Singleton<Service>.Instance.levelManager.player;
			Scene<GameBase>.instance.cameraController.StartTrack(player.transform);
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000609DA File Offset: 0x0005EBDA
		private IEnumerator MovePlayer()
		{
			yield return Chronometer.global.WaitForSeconds(1f);
			Singleton<Service>.Instance.levelManager.player.transform.position = this._downDestination.position;
			yield break;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000609E9 File Offset: 0x0005EBE9
		public override void InteractWith(Character character)
		{
			if (!this._interactable)
			{
				return;
			}
			if (this._used)
			{
				return;
			}
			this._used = true;
			base.StartCoroutine(this.WalkToIn());
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x00060A11 File Offset: 0x0005EC11
		private IEnumerator MoveTo(Vector3 destination)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			for (;;)
			{
				float num = destination.x - player.transform.position.x;
				if (Mathf.Abs(num) < 0.1f)
				{
					break;
				}
				Vector2 move = (num > 0f) ? Vector2.right : Vector2.left;
				player.movement.move = move;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x00060A20 File Offset: 0x0005EC20
		public void Run()
		{
			base.StartCoroutine(this.CMove());
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00060A2F File Offset: 0x0005EC2F
		private IEnumerator CMove()
		{
			while (!this._interactable)
			{
				yield return null;
			}
			base.StartCoroutine(this.WalkToIn());
			yield break;
		}

		// Token: 0x04001AF9 RID: 6905
		private readonly int _introHash = Animator.StringToHash("Back_Intro");

		// Token: 0x04001AFA RID: 6906
		private readonly int _frontIdleHash = Animator.StringToHash("Front_Idle");

		// Token: 0x04001AFB RID: 6907
		private readonly int _backIdleHash = Animator.StringToHash("Back_Idle");

		// Token: 0x04001AFC RID: 6908
		private readonly int _frontCloseHash = Animator.StringToHash("Front_Close");

		// Token: 0x04001AFD RID: 6909
		private readonly int _backCloseHash = Animator.StringToHash("Back_Close");

		// Token: 0x04001AFE RID: 6910
		private readonly int _frontOutroHash = Animator.StringToHash("Front_Outro");

		// Token: 0x04001AFF RID: 6911
		[SerializeField]
		private float _introLength;

		// Token: 0x04001B00 RID: 6912
		[SerializeField]
		private float _closeLength;

		// Token: 0x04001B01 RID: 6913
		[SerializeField]
		private float _outroLength;

		// Token: 0x04001B02 RID: 6914
		[SerializeField]
		private Animator _front;

		// Token: 0x04001B03 RID: 6915
		[SerializeField]
		private Animator _behind;

		// Token: 0x04001B04 RID: 6916
		[SerializeField]
		private Transform _destination;

		// Token: 0x04001B05 RID: 6917
		[SerializeField]
		private Transform _downDestination;

		// Token: 0x04001B06 RID: 6918
		[SerializeField]
		private Transform _cameraPoint;

		// Token: 0x04001B07 RID: 6919
		[SerializeField]
		private GameObject _block;

		// Token: 0x04001B08 RID: 6920
		[SerializeField]
		private SoundInfo _elevatorUp;

		// Token: 0x04001B09 RID: 6921
		[SerializeField]
		private SoundInfo _elevatorDown;

		// Token: 0x04001B0A RID: 6922
		private new bool _interactable;

		// Token: 0x04001B0B RID: 6923
		private bool _used;
	}
}
