using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000786 RID: 1926
public class ClientTutorialIconController : ClientIconTutorialBase
{
	// Token: 0x06002525 RID: 9509 RVA: 0x000AF548 File Offset: 0x000AD948
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_tutorialController = (TutorialIconController)synchronisedObject;
		this.m_semanticIconLookup = GameUtils.RequireManager<SemanticIconLookup>();
		this.m_playerSwitchingManager = GameUtils.RequireManager<PlayerSwitchingManager>();
		this.m_clientFlowController = (this.m_iClientFlowController as ClientCampaignFlowController);
		this.m_tutorialController.m_dialogueAnimator.SetInteger(ClientTutorialIconController.m_animParam_OrderNumber, (int)this.m_stage);
		this.m_plates = this.GetPlates();
		this.m_lettuceNode = new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(this.m_tutorialController.m_lettuce)
		};
		this.m_tomatoNode = new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(this.m_tutorialController.m_tomato)
		};
		this.m_cucumberNode = new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(this.m_tutorialController.m_cucumber)
		};
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000AF613 File Offset: 0x000ADA13
	private void OnNewPlate(GameObject _plate)
	{
		this.m_plates = this.GetPlates();
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000AF624 File Offset: 0x000ADA24
	private void OnStackAdded(IClientAttachment _iHoldable)
	{
		if (this.m_plateStack != null)
		{
			this.m_plateStack.UnregisterOnPlateAdded(new GenericVoid<GameObject>(this.OnNewPlate));
		}
		this.m_plateStack = _iHoldable.AccessGameObject().RequestComponent<ClientPlateStackBase>();
		if (this.m_plateStack != null)
		{
			this.m_plateStack.RegisterOnPlateAdded(new GenericVoid<GameObject>(this.OnNewPlate));
		}
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000AF694 File Offset: 0x000ADA94
	private void OnEngagementChanged(EngagementSlot _s, GamepadUser _b, GamepadUser _a)
	{
		for (int i = 0; i < this.m_icons.Length; i++)
		{
			if (this.m_icons[i].Icon != null)
			{
				UnityEngine.Object.Destroy(this.m_icons[i].Icon.gameObject);
			}
		}
		this.CreateIcons(out this.m_icons, out this.m_animatorFlags);
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000AF6FC File Offset: 0x000ADAFC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		GameUtils.RequireManager<PlayerManager>().EngagementChangeCallback -= this.OnEngagementChanged;
		if (this.m_plateStack != null)
		{
			this.m_plateStack.UnregisterOnPlateAdded(new GenericVoid<GameObject>(this.OnNewPlate));
		}
		if (this.m_plateReturnAttach != null)
		{
			this.m_plateReturnAttach.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnStackAdded));
		}
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000AF778 File Offset: 0x000ADB78
	protected void CreateIcons(out ClientIconTutorialBase.IconData[] o_iconsData, out int[] o_animatorFlags)
	{
		this.m_usedFollowTargets.Clear();
		o_iconsData = new ClientIconTutorialBase.IconData[13];
		o_animatorFlags = new int[13];
		Sprite icon = this.m_semanticIconLookup.GetIcon(SemanticIconLookup.Semantic.Pickup, PlayerInputLookup.Player.One, ControllerIconLookup.IconContext.Bordered);
		o_iconsData[0] = new ClientIconTutorialBase.IconData(this.m_tutorialController, icon, this.m_tutorialController.m_lettuceCrate, new ClientIconTutorialBase.ActiveQuery(this.IconActive_LettuceCrate));
		o_animatorFlags[0] = ClientTutorialIconController.m_iCollectLettuce;
		o_iconsData[1] = new ClientIconTutorialBase.IconData(this.m_tutorialController, icon, this.m_tutorialController.m_tomatoCrate, new ClientIconTutorialBase.ActiveQuery(this.IconActive_TomatoCrate));
		o_animatorFlags[1] = ClientTutorialIconController.m_iCollectTomato;
		o_iconsData[2] = new ClientIconTutorialBase.IconData(this.m_tutorialController, icon, this.m_tutorialController.m_cucumberCrate, new ClientIconTutorialBase.ActiveQuery(this.IconActive_CucumberCrate));
		o_animatorFlags[2] = ClientTutorialIconController.m_iCollectCucumber;
		o_iconsData[3] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_lettuce.m_iconSprite, this.m_tutorialController.m_choppingboards[0], new ClientIconTutorialBase.ActiveQuery(this.IconActive_LettuceChoppingBoard));
		o_animatorFlags[3] = ClientTutorialIconController.m_iPlaceLettuce;
		o_iconsData[4] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_tomato.m_iconSprite, this.m_tutorialController.m_choppingboards[1], new ClientIconTutorialBase.ActiveQuery(this.IconActive_TomatoChoppingBoard));
		o_animatorFlags[4] = ClientTutorialIconController.m_iPlaceTomato;
		o_iconsData[5] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_cucumber.m_iconSprite, this.m_tutorialController.m_choppingboards[2], new ClientIconTutorialBase.ActiveQuery(this.IconActive_CucumberChoppingBoard));
		o_animatorFlags[5] = ClientTutorialIconController.m_iPlaceCucumber;
		Sprite icon2 = this.m_semanticIconLookup.GetIcon(SemanticIconLookup.Semantic.Chop, PlayerInputLookup.Player.One, ControllerIconLookup.IconContext.Bordered);
		o_iconsData[6] = new ClientIconTutorialBase.IconData(this.m_tutorialController, icon2, this.m_tutorialController.m_choppingboards[0], new ClientIconTutorialBase.ActiveQuery(this.IconActive_ChopTheLettuce));
		o_animatorFlags[6] = ClientTutorialIconController.m_iChopLettuce;
		o_iconsData[7] = new ClientIconTutorialBase.IconData(this.m_tutorialController, icon2, this.m_tutorialController.m_choppingboards[1], new ClientIconTutorialBase.ActiveQuery(this.IconActive_ChopTheTomato));
		o_animatorFlags[7] = ClientTutorialIconController.m_iChopTomato;
		o_iconsData[8] = new ClientIconTutorialBase.IconData(this.m_tutorialController, icon2, this.m_tutorialController.m_choppingboards[2], new ClientIconTutorialBase.ActiveQuery(this.IconActive_ChopTheCucumber));
		o_animatorFlags[8] = ClientTutorialIconController.m_iChopCucumber;
		o_iconsData[9] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_lettuce.m_iconSprite, null, new ClientIconTutorialBase.ActiveQuery(this.IconActive_PlateUpLettuce));
		o_animatorFlags[9] = ClientTutorialIconController.m_iPlateLettuce;
		o_iconsData[10] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_tomato.m_iconSprite, null, new ClientIconTutorialBase.ActiveQuery(this.IconActive_PlateUpTomato));
		o_animatorFlags[10] = ClientTutorialIconController.m_iPlateTomato;
		o_iconsData[11] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_cucumber.m_iconSprite, null, new ClientIconTutorialBase.ActiveQuery(this.IconActive_PlateUpCucumber));
		o_animatorFlags[11] = ClientTutorialIconController.m_iPlateCucumber;
		if (this.m_stage == TutorialIconController.TutorialStage.Lettuce)
		{
			o_iconsData[12] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_lettuceSaladIcon, this.m_tutorialController.m_plateStation, new ClientIconTutorialBase.ActiveQuery(this.IconActive_ServeSalad));
		}
		else if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomato)
		{
			o_iconsData[12] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_tomatoSaladIcon, this.m_tutorialController.m_plateStation, new ClientIconTutorialBase.ActiveQuery(this.IconActive_ServeSalad));
		}
		else if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber)
		{
			o_iconsData[12] = new ClientIconTutorialBase.IconData(this.m_tutorialController, this.m_tutorialController.m_cucumberSaladIcon, this.m_tutorialController.m_plateStation, new ClientIconTutorialBase.ActiveQuery(this.IconActive_ServeSalad));
		}
		o_animatorFlags[12] = ClientTutorialIconController.m_iService;
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x000AFB1C File Offset: 0x000ADF1C
	protected override void OnStartTutorial()
	{
		this.CreateIcons(out this.m_icons, out this.m_animatorFlags);
		this.m_clientFlowController.RegisterOnSuccessfulDeliveryCallback(new VoidGeneric<RecipeList.Entry>(this.OnSuccessfulOrder));
		GameUtils.RequireManager<PlayerManager>().EngagementChangeCallback += this.OnEngagementChanged;
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000AFB68 File Offset: 0x000ADF68
	protected override void OnTutorialUpdate()
	{
		base.OnTutorialUpdate();
		if (!this.m_tutorialController.m_dialogueAnimator.enabled)
		{
			this.m_tutorialController.m_dialogueAnimator.enabled = true;
		}
		this.m_usedFollowTargets.Clear();
		for (int i = 0; i < this.m_animatorFlags.Length; i++)
		{
			if (i < this.m_icons.Length)
			{
				this.m_tutorialController.m_dialogueAnimator.SetBool(this.m_animatorFlags[i], this.m_icons[i].Icon.gameObject.activeInHierarchy);
				this.m_icons[i].Icon.transform.SetAsLastSibling();
			}
		}
		if (this.m_switchTutorial == null && this.DetectSwapTutorialStart())
		{
			this.m_hasTriggeredSwapTutorial = true;
			ClientTutorialPopupController clientTutorialPopupController = base.gameObject.RequireComponent<ClientTutorialPopupController>();
			this.m_switchTutorial = clientTutorialPopupController.ShowTutorial(this.m_tutorialController.m_swapTutorialUI, new Generic<IEnumerator, GameObject>(this.SwapTutorialDismissRoutine));
		}
		if (this.m_switchTutorial != null && !this.m_switchTutorial.MoveNext())
		{
			ClientTutorialPopupController clientTutorialPopupController2 = base.gameObject.RequireComponent<ClientTutorialPopupController>();
			clientTutorialPopupController2.Shutdown();
			this.m_switchTutorial = null;
			this.m_playerSwitchingManager.ForceSwitchToNext(PlayerInputLookup.Player.One);
		}
		if (this.m_plateReturnAttach == null)
		{
			this.m_plateReturnAttach = this.m_tutorialController.m_plateReturn.gameObject.RequestComponent<ClientAttachStation>();
			if (this.m_plateReturnAttach != null)
			{
				this.m_plateReturnAttach.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnStackAdded));
				if (this.m_plateStack == null)
				{
					GameObject gameObject = this.m_plateReturnAttach.InspectItem();
					if (gameObject != null)
					{
						this.OnStackAdded(gameObject.RequestInterface<IClientAttachment>());
					}
				}
			}
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000AFD30 File Offset: 0x000AE130
	protected override void OnStopTutorial()
	{
		this.m_animatorFlags = new int[0];
		this.m_switchTutorial = null;
		if (this.m_switchTutorialUIObject != null)
		{
			UnityEngine.Object.Destroy(this.m_switchTutorialUIObject);
		}
		GameUtils.RequireManager<PlayerManager>().EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000AFD84 File Offset: 0x000AE184
	protected void OnSuccessfulOrder(RecipeList.Entry _node)
	{
		if (this.m_stage == TutorialIconController.TutorialStage.Lettuce)
		{
			this.m_stage = TutorialIconController.TutorialStage.LettuceTomato;
			if (this.m_icons != null && this.m_icons.Length > 12 && this.m_icons[12].Icon != null)
			{
				this.m_icons[12].Icon.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_icons.Length; i++)
			{
				UnityEngine.Object.Destroy(this.m_icons[i].Icon.gameObject);
			}
			this.CreateIcons(out this.m_icons, out this.m_animatorFlags);
		}
		else if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomato)
		{
			this.m_stage = TutorialIconController.TutorialStage.LettuceTomatoCucumber;
			if (this.m_icons != null && this.m_icons.Length > 12 && this.m_icons[12].Icon != null)
			{
				this.m_icons[12].Icon.gameObject.SetActive(false);
			}
			for (int j = 0; j < this.m_icons.Length; j++)
			{
				UnityEngine.Object.Destroy(this.m_icons[j].Icon.gameObject);
			}
			this.CreateIcons(out this.m_icons, out this.m_animatorFlags);
		}
		else if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber)
		{
			this.m_clientFlowController.UnregisterOnSuccessfulDeliveryCallback(new VoidGeneric<RecipeList.Entry>(this.OnSuccessfulOrder));
			this.m_tutorialController.m_dialogueAnimator.SetBool(ClientTutorialIconController.m_iCompletedTutorialRecipes, true);
			base.CompleteTutorial();
		}
		this.m_tutorialController.m_dialogueAnimator.SetInteger(ClientTutorialIconController.m_animParam_OrderNumber, (int)this.m_stage);
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000AFF38 File Offset: 0x000AE338
	private bool DetectSwapTutorialStart()
	{
		if (ClientUserSystem.m_Users.Count == 1 && !this.m_hasTriggeredSwapTutorial)
		{
			PlayerControls controls = this.m_playerSwitchingManager.SelectedAvatar(PlayerInputLookup.Player.One);
			ClientWorkableItem clientWorkableItem;
			if (this.IsChopping(controls, out clientWorkableItem) && clientWorkableItem != null && clientWorkableItem.GetProgress() > 0.1f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000AFF9C File Offset: 0x000AE39C
	private bool IsChopping(PlayerControls _controls, out ClientWorkableItem o_workable)
	{
		if (_controls != null)
		{
			ClientInteractable currentlyInteracting = _controls.GetCurrentlyInteracting();
			if (currentlyInteracting && currentlyInteracting.GetComponent<Workstation>())
			{
				ClientAttachStation clientAttachStation = currentlyInteracting.gameObject.RequireComponent<ClientAttachStation>();
				GameObject gameObject = clientAttachStation.InspectItem();
				if (gameObject != null)
				{
					o_workable = gameObject.RequestComponent<ClientWorkableItem>();
					return true;
				}
			}
		}
		o_workable = null;
		return false;
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000B0004 File Offset: 0x000AE404
	private IEnumerator SwapTutorialDismissRoutine(GameObject _ui)
	{
		ILogicalButton swapButton = PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.PlayerSwitch, PlayerInputLookup.Player.One);
		while (!swapButton.JustPressed())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000B0018 File Offset: 0x000AE418
	protected bool InRound()
	{
		return this.m_clientFlowController.InRound;
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000B0025 File Offset: 0x000AE425
	protected bool IconActive_LettuceCrate(ref Transform _follower, HoverIconUIController _controller)
	{
		return this.InRound() && !this.HaveGottenIngredient(this.m_tutorialController.m_lettuce) && !this.PotentialSaladContains(this.m_lettuceNode);
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x000B0060 File Offset: 0x000AE460
	protected bool IconActive_TomatoCrate(ref Transform _follower, HoverIconUIController _controller)
	{
		return this.m_stage != TutorialIconController.TutorialStage.Lettuce && this.InRound() && (!this.HaveGottenIngredient(this.m_tutorialController.m_tomato) && !this.PotentialSaladContains(this.m_tomatoNode));
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000B00B0 File Offset: 0x000AE4B0
	protected bool IconActive_CucumberCrate(ref Transform _follower, HoverIconUIController _controller)
	{
		return this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber && this.InRound() && (!this.HaveGottenIngredient(this.m_tutorialController.m_cucumber) && !this.PotentialSaladContains(this.m_cucumberNode));
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000B0104 File Offset: 0x000AE504
	protected bool IconActive_LettuceChoppingBoard(ref Transform _follower, HoverIconUIController _controller)
	{
		_follower = this.GetEmptyAttachPoint(this.m_tutorialController.m_choppingboards[0], this.m_tutorialController.m_choppingboards);
		bool flag = this.InRound() && this.IngredientToPlaceOnBoard(this.m_tutorialController.m_lettuce);
		if (flag)
		{
			this.m_usedFollowTargets.Add(_follower);
		}
		return flag;
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000B0168 File Offset: 0x000AE568
	protected bool IconActive_TomatoChoppingBoard(ref Transform _follower, HoverIconUIController _controller)
	{
		if (this.m_stage != TutorialIconController.TutorialStage.LettuceTomato && this.m_stage != TutorialIconController.TutorialStage.LettuceTomatoCucumber)
		{
			return false;
		}
		_follower = this.GetEmptyAttachPoint(this.m_tutorialController.m_choppingboards[0], this.m_tutorialController.m_choppingboards);
		bool flag = this.InRound() && this.IngredientToPlaceOnBoard(this.m_tutorialController.m_tomato);
		if (flag)
		{
			this.m_usedFollowTargets.Add(_follower);
		}
		return flag;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000B01E4 File Offset: 0x000AE5E4
	protected bool IconActive_CucumberChoppingBoard(ref Transform _follower, HoverIconUIController _controller)
	{
		if (this.m_stage != TutorialIconController.TutorialStage.LettuceTomatoCucumber)
		{
			return false;
		}
		_follower = this.GetEmptyAttachPoint(this.m_tutorialController.m_choppingboards[0], this.m_tutorialController.m_choppingboards);
		bool flag = this.InRound() && this.IngredientToPlaceOnBoard(this.m_tutorialController.m_cucumber);
		if (flag)
		{
			this.m_usedFollowTargets.Add(_follower);
		}
		return flag;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000B0253 File Offset: 0x000AE653
	protected bool IconActive_ChopTheLettuce(ref Transform _follower, HoverIconUIController _controller)
	{
		return this.InRound() && this.ChopTheWhatever(this.m_tutorialController.m_lettuce, ref _follower);
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000B0275 File Offset: 0x000AE675
	protected bool IconActive_ChopTheTomato(ref Transform _follower, HoverIconUIController _controller)
	{
		return (this.m_stage == TutorialIconController.TutorialStage.LettuceTomato || this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber) && this.InRound() && this.ChopTheWhatever(this.m_tutorialController.m_tomato, ref _follower);
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000B02B1 File Offset: 0x000AE6B1
	protected bool IconActive_ChopTheCucumber(ref Transform _follower, HoverIconUIController _controller)
	{
		return this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber && this.InRound() && this.ChopTheWhatever(this.m_tutorialController.m_cucumber, ref _follower);
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000B02E4 File Offset: 0x000AE6E4
	protected bool IconActive_PlateUpLettuce(ref Transform _follower, HoverIconUIController _controller)
	{
		if (this.m_stage == TutorialIconController.TutorialStage.Lettuce)
		{
			return this.InRound() && this.PlateUpIngredientIcon(this.m_tutorialController.m_lettuce, new AssembledDefinitionNode[0], ref _follower, _controller, 0f);
		}
		if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomato)
		{
			return this.InRound() && this.PlateUpIngredientIcon(this.m_tutorialController.m_lettuce, new AssembledDefinitionNode[]
			{
				new IngredientAssembledNode(this.m_tutorialController.m_tomato)
			}, ref _follower, _controller, -30f);
		}
		return this.InRound() && this.PlateUpIngredientIcon(this.m_tutorialController.m_lettuce, new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(this.m_tutorialController.m_tomato),
			new IngredientAssembledNode(this.m_tutorialController.m_cucumber)
		}, ref _follower, _controller, 0f);
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000B03C8 File Offset: 0x000AE7C8
	protected bool IconActive_PlateUpTomato(ref Transform _follower, HoverIconUIController _controller)
	{
		if (this.m_stage == TutorialIconController.TutorialStage.LettuceTomato)
		{
			return this.InRound() && this.PlateUpIngredientIcon(this.m_tutorialController.m_tomato, new AssembledDefinitionNode[]
			{
				new IngredientAssembledNode(this.m_tutorialController.m_lettuce)
			}, ref _follower, _controller, 30f);
		}
		return this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber && this.InRound() && this.PlateUpIngredientIcon(this.m_tutorialController.m_tomato, new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(this.m_tutorialController.m_lettuce),
			new IngredientAssembledNode(this.m_tutorialController.m_cucumber)
		}, ref _follower, _controller, 45f);
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000B0484 File Offset: 0x000AE884
	protected bool IconActive_PlateUpCucumber(ref Transform _follower, HoverIconUIController _controller)
	{
		return this.m_stage == TutorialIconController.TutorialStage.LettuceTomatoCucumber && this.InRound() && this.PlateUpIngredientIcon(this.m_tutorialController.m_cucumber, new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(this.m_tutorialController.m_lettuce),
			new IngredientAssembledNode(this.m_tutorialController.m_tomato)
		}, ref _follower, _controller, -45f);
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000B04F4 File Offset: 0x000AE8F4
	protected bool PlateUpIngredientIcon(IngredientOrderNode _ingredient, AssembledDefinitionNode[] _otherIngredients, ref Transform _follower, HoverIconUIController _controller, float _optionalRotation)
	{
		if (!this.InRound())
		{
			return false;
		}
		RectTransformExtension rectTransformExtension = _controller.gameObject.RequireComponent<RectTransformExtension>();
		rectTransformExtension.rotation = Vector3.zero;
		if (!this.PotentialSaladContains(new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(_ingredient)
		}) && GameUtils.GetIngredients(_ingredient).Length > 0)
		{
			GameObject[] array = this.FindPotentialSalad(_otherIngredients);
			if (array.Length > 0)
			{
				rectTransformExtension.rotation = new Vector3(0f, 0f, _optionalRotation);
				_follower = array[0].transform;
				return true;
			}
			GameObject[] array2 = this.FindEmptyPlates();
			if (!array2.IsEmpty<GameObject>())
			{
				rectTransformExtension.rotation = new Vector3(0f, 0f, _optionalRotation);
				_follower = array2[0].transform;
				return true;
			}
			if (this.m_plates.Length > 0)
			{
				_follower = this.m_plates[0].transform;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000B05D8 File Offset: 0x000AE9D8
	protected bool IconActive_ServeSalad(ref Transform _follower, HoverIconUIController _controller)
	{
		TutorialIconController.TutorialStage stage = this.m_stage;
		if (stage == TutorialIconController.TutorialStage.Lettuce)
		{
			IngredientAssembledNode ingredientAssembledNode = new IngredientAssembledNode(this.m_tutorialController.m_lettuce);
			return this.InRound() && !this.FindPotentialSalad(this.m_lettuceNode).IsEmpty<GameObject>();
		}
		if (stage == TutorialIconController.TutorialStage.LettuceTomato)
		{
			IngredientAssembledNode ingredientAssembledNode2 = new IngredientAssembledNode(this.m_tutorialController.m_lettuce);
			IngredientAssembledNode ingredientAssembledNode3 = new IngredientAssembledNode(this.m_tutorialController.m_tomato);
			return this.InRound() && !this.FindPotentialSalad(new AssembledDefinitionNode[]
			{
				ingredientAssembledNode2,
				ingredientAssembledNode3
			}).IsEmpty<GameObject>();
		}
		if (stage != TutorialIconController.TutorialStage.LettuceTomatoCucumber)
		{
			return false;
		}
		IngredientAssembledNode ingredientAssembledNode4 = new IngredientAssembledNode(this.m_tutorialController.m_lettuce);
		IngredientAssembledNode ingredientAssembledNode5 = new IngredientAssembledNode(this.m_tutorialController.m_tomato);
		IngredientAssembledNode ingredientAssembledNode6 = new IngredientAssembledNode(this.m_tutorialController.m_cucumber);
		return this.InRound() && !this.FindPotentialSalad(new AssembledDefinitionNode[]
		{
			ingredientAssembledNode4,
			ingredientAssembledNode5,
			ingredientAssembledNode6
		}).IsEmpty<GameObject>();
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000B06F0 File Offset: 0x000AEAF0
	protected bool ChopTheWhatever(IngredientOrderNode _ingredient, ref Transform _follower)
	{
		if (!this.InRound())
		{
			return false;
		}
		if (this.PotentialSaladContains(new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(_ingredient)
		}) || GameUtils.GetIngredients(this.m_tutorialController.m_lettuce).Length != 0)
		{
			return false;
		}
		int i = 0;
		while (i < this.m_tutorialController.m_choppingboards.Length)
		{
			Transform transform = this.FindIngredientOnStation(_ingredient, this.m_tutorialController.m_choppingboards[i]);
			if (transform != null)
			{
				ClientWorkstation clientWorkstation = transform.gameObject.RequestComponentUpwardsRecursive<ClientWorkstation>();
				if (clientWorkstation != null && clientWorkstation.IsBeingUsed())
				{
					return false;
				}
				_follower = transform;
				return true;
			}
			else
			{
				i++;
			}
		}
		return false;
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000B07A8 File Offset: 0x000AEBA8
	protected Transform FindIngredientOnStation(IngredientOrderNode _ingredient, Transform _station)
	{
		GameObject[] preIngredients = GameUtils.GetPreIngredients(_ingredient);
		ClientAttachStation clientAttachStation = _station.gameObject.RequireComponent<ClientAttachStation>();
		GameObject gameObject = clientAttachStation.InspectItem();
		if (gameObject != null && preIngredients.Contains(gameObject))
		{
			return clientAttachStation.GetComponent<AttachStation>().GetAttachPoint(gameObject);
		}
		return null;
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000B07F8 File Offset: 0x000AEBF8
	protected Transform GetEmptyAttachPoint(Transform _default, Transform[] _stations)
	{
		ClientAttachStation clientAttachStation = _default.gameObject.RequireComponent<ClientAttachStation>();
		Transform attachPoint = clientAttachStation.GetComponent<AttachStation>().GetAttachPoint(clientAttachStation.InspectItem());
		if (!clientAttachStation.HasItem() && !this.m_usedFollowTargets.Contains(attachPoint))
		{
			return attachPoint;
		}
		for (int i = 0; i < _stations.Length; i++)
		{
			ClientAttachStation clientAttachStation2 = _stations[i].gameObject.RequireComponent<ClientAttachStation>();
			attachPoint = clientAttachStation2.GetComponent<AttachStation>().GetAttachPoint(clientAttachStation2.InspectItem());
			if (!clientAttachStation2.HasItem() && !this.m_usedFollowTargets.Contains(attachPoint))
			{
				return attachPoint;
			}
		}
		return attachPoint;
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000B0894 File Offset: 0x000AEC94
	protected virtual bool IngredientToPlaceOnBoard(IngredientOrderNode _ingredient)
	{
		return GameUtils.GetPreIngredients(_ingredient).Length > 0 && !this.PotentialSaladContains(new AssembledDefinitionNode[]
		{
			new IngredientAssembledNode(_ingredient)
		}) && GameUtils.GetIngredients(_ingredient).Length == 0 && !this.OnChoppingBoard(GameUtils.GetPreIngredients(_ingredient));
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000B08E8 File Offset: 0x000AECE8
	protected bool OnChoppingBoard(GameObject[] _objects)
	{
		for (int i = 0; i < this.m_tutorialController.m_choppingboards.Length; i++)
		{
			ClientAttachStation clientAttachStation = this.m_tutorialController.m_choppingboards[i].gameObject.RequireComponent<ClientAttachStation>();
			GameObject gameObject = clientAttachStation.InspectItem();
			if (gameObject && _objects.Contains(gameObject))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000B094C File Offset: 0x000AED4C
	protected bool HaveGottenIngredient(IngredientOrderNode _ingredient)
	{
		return GameUtils.GetPreIngredients(_ingredient).Length != 0 || GameUtils.GetIngredients(_ingredient).Length != 0;
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x000B096C File Offset: 0x000AED6C
	protected bool PotentialSaladContains(AssembledDefinitionNode[] _ingredients)
	{
		GameObject[] array = this.FindPotentialSalad(_ingredients);
		return !array.IsEmpty<GameObject>();
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x000B098C File Offset: 0x000AED8C
	public GameObject[] FindPotentialSalad(AssembledDefinitionNode[] _ingredients)
	{
		if (this.m_plates != null)
		{
			this.m_plates = this.m_plates.AllRemoved_Predicate((GameObject x) => x == null || !x.activeInHierarchy);
		}
		if (this.m_plates == null || this.m_plates.IsEmpty<GameObject>())
		{
			this.m_plates = this.GetPlates();
		}
		return GameUtils.FindContainersWithSubset("Plate", _ingredients, this.m_plates);
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x000B0A0C File Offset: 0x000AEE0C
	public GameObject[] FindEmptyPlates()
	{
		if (this.m_plates != null)
		{
			this.m_plates = this.m_plates.AllRemoved_Predicate((GameObject x) => x == null || !x.activeInHierarchy);
		}
		if (this.m_plates == null || this.m_plates.IsEmpty<GameObject>())
		{
			this.m_plates = this.GetPlates();
		}
		return GameUtils.FindEmptyContainers("Plate", this.m_plates);
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000B0A89 File Offset: 0x000AEE89
	protected GameObject[] GetPlates()
	{
		return GameObject.FindGameObjectsWithTag("Plate");
	}

	// Token: 0x04001CB5 RID: 7349
	private TutorialIconController m_tutorialController;

	// Token: 0x04001CB6 RID: 7350
	private AssembledDefinitionNode[] m_lettuceNode;

	// Token: 0x04001CB7 RID: 7351
	private AssembledDefinitionNode[] m_tomatoNode;

	// Token: 0x04001CB8 RID: 7352
	private AssembledDefinitionNode[] m_cucumberNode;

	// Token: 0x04001CB9 RID: 7353
	private static readonly int m_iCollectLettuce = Animator.StringToHash("CollectLettuce");

	// Token: 0x04001CBA RID: 7354
	private static readonly int m_iCollectTomato = Animator.StringToHash("CollectTomato");

	// Token: 0x04001CBB RID: 7355
	private static readonly int m_iCollectCucumber = Animator.StringToHash("CollectCucumber");

	// Token: 0x04001CBC RID: 7356
	private static readonly int m_iPlaceLettuce = Animator.StringToHash("PlaceLettuce");

	// Token: 0x04001CBD RID: 7357
	private static readonly int m_iPlaceTomato = Animator.StringToHash("PlaceTomato");

	// Token: 0x04001CBE RID: 7358
	private static readonly int m_iPlaceCucumber = Animator.StringToHash("PlaceCucumber");

	// Token: 0x04001CBF RID: 7359
	private static readonly int m_iChopLettuce = Animator.StringToHash("ChopLettuce");

	// Token: 0x04001CC0 RID: 7360
	private static readonly int m_iChopTomato = Animator.StringToHash("ChopTomato");

	// Token: 0x04001CC1 RID: 7361
	private static readonly int m_iChopCucumber = Animator.StringToHash("ChopCucumber");

	// Token: 0x04001CC2 RID: 7362
	private static readonly int m_iPlateLettuce = Animator.StringToHash("PlateLettuce");

	// Token: 0x04001CC3 RID: 7363
	private static readonly int m_iPlateTomato = Animator.StringToHash("PlateTomato");

	// Token: 0x04001CC4 RID: 7364
	private static readonly int m_iPlateCucumber = Animator.StringToHash("PlateCucumber");

	// Token: 0x04001CC5 RID: 7365
	private static readonly int m_iService = Animator.StringToHash("Service!");

	// Token: 0x04001CC6 RID: 7366
	private static readonly int m_iCompletedTutorialRecipes = Animator.StringToHash("CompletedTutorialRecipes");

	// Token: 0x04001CC7 RID: 7367
	protected SemanticIconLookup m_semanticIconLookup;

	// Token: 0x04001CC8 RID: 7368
	private int[] m_animatorFlags = new int[0];

	// Token: 0x04001CC9 RID: 7369
	private IEnumerator m_switchTutorial;

	// Token: 0x04001CCA RID: 7370
	private GameObject m_switchTutorialUIObject;

	// Token: 0x04001CCB RID: 7371
	private bool m_hasTriggeredSwapTutorial;

	// Token: 0x04001CCC RID: 7372
	protected GameObject[] m_plates;

	// Token: 0x04001CCD RID: 7373
	private List<Transform> m_usedFollowTargets = new List<Transform>();

	// Token: 0x04001CCE RID: 7374
	private PlayerSwitchingManager m_playerSwitchingManager;

	// Token: 0x04001CCF RID: 7375
	private ClientCampaignFlowController m_clientFlowController;

	// Token: 0x04001CD0 RID: 7376
	private TutorialIconController.TutorialStage m_stage;

	// Token: 0x04001CD1 RID: 7377
	private static readonly int m_animParam_OrderNumber = Animator.StringToHash("OrderNumber");

	// Token: 0x04001CD2 RID: 7378
	private ClientPlateStackBase m_plateStack;

	// Token: 0x04001CD3 RID: 7379
	private ClientAttachStation m_plateReturnAttach;
}
