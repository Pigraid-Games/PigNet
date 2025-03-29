#region LICENSE
// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2025 Niclas Olofsson.
// All Rights Reserved.
#endregion

namespace PigNet.Net.EnumerationsTable;

public enum ItemDescriptorInternalType
{
	Invalid = 0,
	Default = 1,
	Molang = 2,
	ItemTag = 3,
	Deferred = 4,
	ComplexAlias = 5
}

public enum ItemReleaseInventoryTransactionActionType
{
	Release = 0,
	Use = 1
}

public enum ItemStackNetResult
{
    Success = 0,
    Error = 1,
    InvalidRequestActionType = 2,
    ActionRequestNotAllowed = 3,
    ScreenHandlerEndRequestFailed = 4,
    ItemRequestActionHandlerCommitFailed = 5,
    InvalidRequestCraftActionType = 6,
    InvalidCraftRequest = 7,
    InvalidCraftRequestScreen = 8,
    InvalidCraftResult = 9,
    InvalidCraftResultIndex = 10,
    InvalidCraftResultItem = 11,
    InvalidItemNetId = 12,
    MissingCreatedOutputContainer = 13,
    FailedToSetCreatedItemOutputSlot = 14,
    RequestAlreadyInProgress = 15,
    FailedToInitSparseContainer = 16,
    ResultTransferFailed = 17,
    ExpectedItemSlotNotFullyConsumed = 18,
    ExpectedAnywhereItemNotFullyConsumed = 19,
    ItemAlreadyConsumedFromSlot = 20,
    ConsumedTooMuchFromSlot = 21,
    MismatchSlotExpectedConsumedItem = 22,
    MismatchSlotExpectedConsumedItemNetIdVariant = 23,
    FailedToMatchExpectedSlotConsumedItem = 24,
    FailedToMatchExpectedAllowedAnywhereConsumedItem = 25,
    ConsumedItemOutOfAllowedSlotRange = 26,
    ConsumedItemNotAllowed = 27,
    PlayerNotInCreativeMode = 28,
    InvalidExperimentalRecipeRequest = 29,
    FailedToCraftCreative = 30,
    FailedToGetLevelRecipe = 31,
    FailedToFindRecipeByNetId = 32,
    MismatchedCraftingSize = 33,
    MissingInputSparseContainer = 34,
    MismatchedRecipeForInputGridItems = 35,
    EmptyCraftResults = 36,
    FailedToEnchant = 37,
    MissingInputItem = 38,
    InsufficientPlayerLevelToEnchant = 39,
    MissingMaterialItem = 40,
    MissingActor = 41,
    UnknownPrimaryEffect = 42,
    PrimaryEffectOutOfRange = 43,
    PrimaryEffectUnavailable = 44,
    SecondaryEffectOutOfRange = 45,
    SecondaryEffectUnavailable = 46,
    DstContainerEqualToCreatedOutputContainer = 47,
    DstContainerAndSlotEqualToSrcContainerAndSlot = 48,
    FailedToValidateSrcSlot = 49,
    FailedToValidateDstSlot = 50,
    InvalidAdjustedAmount = 51,
    InvalidItemSetType = 52,
    InvalidTransferAmount = 53,
    CannotSwapItem = 54,
    CannotPlaceItem = 55,
    UnhandledItemSetType = 56,
    InvalidRemovedAmount = 57,
    InvalidRegion = 58,
    CannotDropItem = 59,
    CannotDestroyItem = 60,
    InvalidSourceContainer = 61,
    ItemNotConsumed = 62,
    InvalidNumCrafts = 63,
    InvalidCraftResultStackSize = 64,
    CannotRemoveItem = 65,
    CannotConsumeItem = 66,
    ScreenStackError = 67
}

public enum ItemStackRequestActionType
{
	Take = 0,
	Place = 1,
	Swap = 2,
	Drop = 3,
	Destroy = 4,
	Consume = 5,
	Create = 6,
	PlaceInItemContainer_DEPRECATED = 7,
	TakeFromItemContainer_DEPRECATED = 8,
	ScreenLabTableCombine = 9,
	ScreenBeaconPayment = 10,
	ScreenHUDMineBlock = 11,
	CraftRecipe = 12,
	CraftRecipeAuto = 13,
	CraftCreative = 14,
	CraftRecipeOptional = 15,
	CraftRepairAndDisenchant = 16,
	CraftLoom = 17,
	CraftNonImplemented_DEPRECATEDASKTYLAING = 18,
	CraftResults_DEPRECATEDASKTYLAING = 19,
	ifdef = 20,
	TEST_INFRASTRUCTURE_ENABLED = 21,
	Test = 22,
	endif = 23
}

public enum ItemUseInventoryTransactionActionType
{
	Place = 0,
	Use = 1,
	Destroy = 2
}

public enum ItemUseInventoryTransactionPredictedResult
{
	Failure = 0,
	Success = 1
}

public enum ItemUseMethod
{
	Unknown = -1,
	EquipArmor = 0,
	Eat = 1,
	Attack = 2,
	Consume = 3,
	Throw = 4,
	Shoot = 5,
	Place = 6,
	FillBottle = 7,
	FillBucket = 8,
	PourBucket = 9,
	UseTool = 10,
	Interact = 11,
	Retrieved = 12,
	Dyed = 13,
	Traded = 14,
	BrushingCompleted = 15,
	OpenedVault = 16,
	Count = 17
}

public enum ItemUseOnActorInventoryTransactionActionType
{
	Interact = 0,
	Attack = 1,
	ItemInteract = 2
}