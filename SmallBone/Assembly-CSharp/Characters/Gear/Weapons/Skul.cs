using System;
using Characters.Actions;
using CutScenes;
using Data;
using Level;
using Services;
using Singletons;
using SkulStories;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x0200082B RID: 2091
	public class Skul : MonoBehaviour
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002B38 RID: 11064 RVA: 0x00084F47 File Offset: 0x00083147
		public Characters.Actions.Action spawn
		{
			get
			{
				return this._spawn;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002B39 RID: 11065 RVA: 0x00084F4F File Offset: 0x0008314F
		public Characters.Actions.Action downButNotOut
		{
			get
			{
				return this._downButNotOut;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002B3A RID: 11066 RVA: 0x00084F57 File Offset: 0x00083157
		public Characters.Actions.Action getSkul
		{
			get
			{
				return this._getSkul;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x00084F5F File Offset: 0x0008315F
		public Characters.Actions.Action getScroll
		{
			get
			{
				return this._getScroll;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002B3C RID: 11068 RVA: 0x00084F67 File Offset: 0x00083167
		public Characters.Actions.Action endPose
		{
			get
			{
				return this._endPose;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002B3D RID: 11069 RVA: 0x00084F6F File Offset: 0x0008316F
		public Characters.Actions.Action introIdle
		{
			get
			{
				return this._introIdle;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002B3E RID: 11070 RVA: 0x00084F77 File Offset: 0x00083177
		public Characters.Actions.Action introWalk
		{
			get
			{
				return this._introWalk;
			}
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x00084F7F File Offset: 0x0008317F
		private void Start()
		{
			this.StartSkulSpawnAction();
			this.RemoveUsedSkulActions();
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x00084F90 File Offset: 0x00083190
		private void StartSkulSpawnAction()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			if (levelManager.currentChapter.type != Chapter.Type.Castle && levelManager.currentChapter.type != Chapter.Type.HardmodeCastle)
			{
				levelManager.skulSpawnAnimaionEnable = true;
				return;
			}
			if (levelManager.skulSpawnAnimaionEnable)
			{
				this._spawn.TryStart();
			}
			levelManager.skulSpawnAnimaionEnable = true;
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x00084FE8 File Offset: 0x000831E8
		private void RemoveUsedSkulActions()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			if (this._getSkul != null && GameData.Progress.skulstory.GetData(SkulStories.Key.prologue))
			{
				levelManager.player.actions.Remove(this._getSkul);
				UnityEngine.Object.Destroy(this._getSkul.gameObject);
				this._getSkul = null;
			}
			if (this._getScroll != null && GameData.Generic.tutorial.isPlayed())
			{
				levelManager.player.actions.Remove(this._getScroll);
				UnityEngine.Object.Destroy(this._getScroll.gameObject);
				this._getScroll = null;
			}
			if (GameData.Progress.cutscene.GetData(CutScenes.Key.anotherWitch_Save))
			{
				levelManager.player.actions.Remove(this._endPose);
				levelManager.player.actions.Remove(this._introIdle);
				levelManager.player.actions.Remove(this._introWalk);
				if (this._endPose != null)
				{
					UnityEngine.Object.Destroy(this._endPose.gameObject);
					this._endPose = null;
				}
				if (this._introIdle != null)
				{
					UnityEngine.Object.Destroy(this._introIdle.gameObject);
					this._introIdle = null;
				}
				if (this._introWalk != null)
				{
					UnityEngine.Object.Destroy(this._introWalk.gameObject);
					this._introWalk = null;
				}
			}
		}

		// Token: 0x040024C5 RID: 9413
		[SerializeField]
		private Characters.Actions.Action _spawn;

		// Token: 0x040024C6 RID: 9414
		[SerializeField]
		private Characters.Actions.Action _downButNotOut;

		// Token: 0x040024C7 RID: 9415
		[SerializeField]
		private Characters.Actions.Action _getSkul;

		// Token: 0x040024C8 RID: 9416
		[SerializeField]
		private Characters.Actions.Action _getScroll;

		// Token: 0x040024C9 RID: 9417
		[SerializeField]
		private Characters.Actions.Action _endPose;

		// Token: 0x040024CA RID: 9418
		[SerializeField]
		private Characters.Actions.Action _introIdle;

		// Token: 0x040024CB RID: 9419
		[SerializeField]
		private Characters.Actions.Action _introWalk;
	}
}
