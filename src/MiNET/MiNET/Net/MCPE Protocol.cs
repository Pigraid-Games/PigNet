#region LICENSE

// The contents of this file are subject to the Common Public Attribution// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

#nullable enable
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using log4net;
using MiNET.Net.EnumerationsTable;
using MiNET.Net.Packets.Mcpe;
using MiNET.Net.Packets.RakNet;
using MiNET.Net.RakNet;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;

namespace MiNET.Net;

public class McpeProtocolInfo
{
	public const int ProtocolVersion = 786;
	public const string GameVersion = "1.21.70";
}

public interface IMcpeMessageHandler
{
	void Disconnect(string reason, bool sendDisconnect = true);

	void HandleMcpeLogin(McpeLogin message);
	void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message);
	void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message);
	void HandleMcpeText(McpeText message);
	void HandleMcpeMoveEntity(McpeMoveActor message);
	void HandleMcpeMovePlayer(McpeMovePlayer message);
	void HandleMcpeRiderJump(McpeRiderJump message);
	void HandleMcpeEntityEvent(McpeActorEvent message);
	void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
	void HandleMcpeMobEquipment(McpeMobEquipment message);
	void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
	void HandleMcpeInteract(McpeInteract message);
	void HandleMcpeBlockPickRequest(McpeBlockPickRequest message);
	void HandleMcpeTakeItemActor(McpeActorPickRequest message);
	void HandleMcpePlayerAction(McpePlayerAction message);
	void HandleMcpeSetActorData(McpeSetActorData message);
	void HandleMcpeSetActorMotion(McpeSetActorMotion message);
	void HandleMcpeAnimate(McpeAnimate message);
	void HandleMcpeRespawn(McpeRespawn message);
	void HandleMcpeContainerClose(McpeContainerClose message);
	void HandleMcpePlayerHotbar(McpePlayerHotbar message);
	void HandleMcpeInventoryContent(McpeInventoryContent message);
	void HandleMcpeInventorySlot(McpeInventorySlot message);
	void HandleMcpeBlockEntityData(McpeBlockActorData message);
	void HandleMcpePlayerInput(McpePlayerInput message);
	void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
	void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
	void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
	void HandleMcpeCommandRequest(McpeCommandRequest message);
	void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message);
	void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message);
	void HandleMcpePurchaseReceipt(McpePurchaseReceipt message);
	void HandleMcpePlayerSkin(McpePlayerSkin message);
	void HandleMcpeNpcRequest(McpeNpcRequest message);
	void HandleMcpePhotoTransfer(McpePhotoTransfer message);
	void HandleMcpeModalFormResponse(McpeModalFormResponse message);
	void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message);
	void HandleMcpeLabTable(McpeLabTable message);
	void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message);
	void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
	void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
	void HandleMcpeNetworkSettings(McpeNetworkSettings message);
	void HandleMcpePlayerAuthInput(McpePlayerAuthInput message);
	void HandleMcpeItemStackRequest(McpeItemStackRequest message);
	void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message);
	void HandleMcpePacketViolationWarning(McpeViolationWarning message);
	void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocks message);
	void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message);
	void HandleMcpeRequestAbility(McpeRequestAbility message);
	void HandleMcpeRequestNetworkSettings(McpeRequestNetworkSettings message);
	void HandleMcpeEmote(McpeEmotePacket message);
	void HandleMcpeEmoteList(McpeEmoteList message);
	void HandleMcpePermissionRequest(McpeRequestPermission message);
	void HandleMcpeSetInventoryOptions(McpeSetInventoryOptions message);
	void HandleMcpeAnvilDamage(McpeAnvilDamage message);
	void HandleMcpeServerboundLoadingScreen(McpeServerboundLoadingScreen message);
}

public interface IMcpeClientMessageHandler
{
	void HandleMcpePlayStatus(McpePlayStatus message);
	void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message);
	void HandleMcpeDisconnect(McpeDisconnect message);
	void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message);
	void HandleMcpeResourcePackStack(McpeResourcePackStack message);
	void HandleMcpeText(McpeText message);
	void HandleMcpeSetTime(McpeSetTime message);
	void HandleMcpeStartGame(McpeStartGame message);
	void HandleMcpeAddPlayer(McpeAddPlayer message);
	void HandleMcpeAddEntity(McpeAddActor message);
	void HandleMcpeRemoveEntity(McpeRemoveActor message);
	void HandleMcpeAddItemEntity(McpeAddItemActor message);
	void HandleMcpeTakeItemActor(McpeTakeItemActor message);
	void HandleMcpeMoveEntity(McpeMoveActor message);
	void HandleMcpeMovePlayer(McpeMovePlayer message);
	void HandleMcpeRiderJump(McpeRiderJump message);
	void HandleMcpeUpdateBlock(McpeUpdateBlock message);
	void HandleMcpeAddPainting(McpeAddPainting message);
	void HandleMcpeLevelEvent(McpeLevelEvent message);
	void HandleMcpeBlockEvent(McpeBlockEvent message);
	void HandleMcpeEntityEvent(McpeActorEvent message);
	void HandleMcpeMobEffect(McpeMobEffect message);
	void HandleMcpeUpdateAttributes(McpeUpdateAttributes message);
	void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
	void HandleMcpeMobEquipment(McpeMobEquipment message);
	void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
	void HandleMcpeInteract(McpeInteract message);
	void HandleMcpeHurtArmor(McpeHurtArmor message);
	void HandleMcpeSetActorData(McpeSetActorData message);
	void HandleMcpeSetActorMotion(McpeSetActorMotion message);
	void HandleMcpeSetActorLink(McpeSetActorLink message);
	void HandleMcpeSetHealth(McpeSetHealth message);
	void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message);
	void HandleMcpeAnimate(McpeAnimate message);
	void HandleMcpeRespawn(McpeRespawn message);
	void HandleMcpeContainerOpen(McpeContainerOpen message);
	void HandleMcpeContainerClose(McpeContainerClose message);
	void HandleMcpePlayerHotbar(McpePlayerHotbar message);
	void HandleMcpeInventoryContent(McpeInventoryContent message);
	void HandleMcpeInventorySlot(McpeInventorySlot message);
	void HandleMcpeContainerSetData(McpeContainerSetData message);
	void HandleMcpeCraftingData(McpeCraftingData message);
	void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message);
	void HandleMcpeBlockEntityData(McpeBlockActorData message);
	void HandleMcpeLevelChunk(McpeLevelChunk message);
	void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message);
	void HandleMcpeSetDifficulty(McpeSetDifficulty message);
	void HandleMcpeChangeDimension(McpeChangeDimension message);
	void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
	void HandleMcpePlayerList(McpePlayerList message);
	void HandleMcpeSimpleEvent(McpeSimpleEvent message);
	void HandleMcpeTelemetryEvent(McpeLegacyTelemetryEvent message);
	void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message);
	void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
	void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
	void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message);
	void HandleMcpeGameRulesChanged(McpeGameRulesChanged message);
	void HandleMcpeCamera(McpeCamera message);
	void HandleMcpeBossEvent(McpeBossEvent message);
	void HandleMcpeShowCredits(McpeShowCredits message);
	void HandleMcpeAvailableCommands(McpeAvailableCommands message);
	void HandleMcpeCommandOutput(McpeCommandOutput message);
	void HandleMcpeUpdateTrade(McpeUpdateTrade message);
	void HandleMcpeUpdateEquipment(McpeUpdateEquipment message);
	void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message);
	void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message);
	void HandleMcpeTransfer(McpeTransfer message);
	void HandleMcpePlaySound(McpePlaySound message);
	void HandleMcpeStopSound(McpeStopSound message);
	void HandleMcpeSetTitle(McpeSetTitle message);
	void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message);
	void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message);
	void HandleMcpeShowStoreOffer(McpeShowStoreOffer message);
	void HandleMcpePlayerSkin(McpePlayerSkin message);
	void HandleMcpeSubClientLogin(McpeSubClientLogin message);
	void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message);
	void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message);
	void HandleMcpeBookEdit(McpeBookEdit message);
	void HandleMcpeNpcRequest(McpeNpcRequest message);
	void HandleMcpeModalFormRequest(McpeModalFormRequest message);
	void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message);
	void HandleMcpeShowProfile(McpeShowProfile message);
	void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message);
	void HandleMcpeRemoveObjective(McpeRemoveObjective message);
	void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message);
	void HandleMcpeSetScore(McpeSetScore message);
	void HandleMcpeLabTable(McpeLabTable message);
	void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message);
	void HandleMcpeMoveEntityDelta(McpeMoveActorDelta message);
	void HandleMcpeSetScoreboardIdentity(McpeSetScoreboardIdentity message);
	void HandleMcpeUpdateSoftEnum(McpeUpdateSoftEnum message);
	void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message);
	void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message);
	void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message);
	void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message);
	void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
	void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message);
	void HandleMcpeLecternUpdate(McpeLecternUpdate message);
	void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
	void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message);
	void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message);
	void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataRequest message);
	void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataResponse message);
	void HandleMcpeUpdateBlockProperties(McpeUpdateBlockProperties message);
	void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message);
	void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message);
	void HandleMcpeNetworkSettings(McpeNetworkSettings message);
	void HandleMcpeCreativeContent(McpeCreativeContent message);
	void HandleMcpePlayerEnchantOptions(McpePlayerEnchantOptions message);
	void HandleMcpeItemStackResponse(McpeItemStackResponse message);
	void HandleMcpeItemComponent(McpeItemComponent message);
	void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocks message);
	void HandleMcpeSubChunkPacket(McpeSubChunk message);
	void HandleMcpeDimensionData(McpeDimensionData message);
	void HandleMcpeUpdateAbilities(McpeUpdateAbilities message);
	void HandleMcpeUpdateAdventureSettings(McpeUpdateAdventureSettings message);
	void HandleMcpeTrimData(McpeTrimData message);
	void HandleMcpeOpenSign(McpeOpenSign message);
	void HandleFtlCreatePlayer(FtlCreatePlayer message);
	void HandleMcpeEmote(McpeEmotePacket message);
	void HandleMcpeEmoteList(McpeEmoteList message);
	void HandleMcpePermissionRequest(McpeRequestPermission message);
	void HandleMcpePlayerFog(McpePlayerFog message);
	void HandleMcpeAnimateEntity(McpeAnimateEntity message);
	void HandleMcpeCloseForm(McpeClientboundCloseForm message);
}

public class McpeClientMessageDispatcher(IMcpeClientMessageHandler messageHandler)
{
	private readonly IMcpeClientMessageHandler _messageHandler = messageHandler;

	private static readonly Dictionary<Type, Action<IMcpeClientMessageHandler, Packet>> Handlers = new()
	{
		[typeof(McpePlayStatus)] = (h, p)
			=> h.HandleMcpePlayStatus((McpePlayStatus) p),
		[typeof(McpeServerToClientHandshake)] = (h, p)
			=> h.HandleMcpeServerToClientHandshake((McpeServerToClientHandshake) p),
		[typeof(McpeDisconnect)] = (h, p)
			=> h.HandleMcpeDisconnect((McpeDisconnect) p),
		[typeof(McpeResourcePacksInfo)] = (h, p)
			=> h.HandleMcpeResourcePacksInfo((McpeResourcePacksInfo) p),
		[typeof(McpeResourcePackStack)] = (h, p)
			=> h.HandleMcpeResourcePackStack((McpeResourcePackStack) p),
		[typeof(McpeText)] = (h, p)
			=> h.HandleMcpeText((McpeText) p),
		[typeof(McpeSetTime)] = (h, p)
			=> h.HandleMcpeSetTime((McpeSetTime) p),
		[typeof(McpeStartGame)] = (h, p)
			=> h.HandleMcpeStartGame((McpeStartGame) p),
		[typeof(McpeAddPlayer)] = (h, p)
			=> h.HandleMcpeAddPlayer((McpeAddPlayer) p),
		[typeof(McpeAddActor)] = (h, p)
			=> h.HandleMcpeAddEntity((McpeAddActor) p),
		[typeof(McpeRemoveActor)] = (h, p)
			=> h.HandleMcpeRemoveEntity((McpeRemoveActor) p),
		[typeof(McpeAddItemActor)] = (h, p)
			=> h.HandleMcpeAddItemEntity((McpeAddItemActor) p),
		[typeof(McpeTakeItemActor)] = (h, p)
			=> h.HandleMcpeTakeItemActor((McpeTakeItemActor) p),
		[typeof(McpeMoveActor)] = (h, p)
			=> h.HandleMcpeMoveEntity((McpeMoveActor) p),
		[typeof(McpeMovePlayer)] = (h, p)
			=> h.HandleMcpeMovePlayer((McpeMovePlayer) p),
		[typeof(McpeRiderJump)] = (h, p)
			=> h.HandleMcpeRiderJump((McpeRiderJump) p),
		[typeof(McpeUpdateBlock)] = (h, p)
			=> h.HandleMcpeUpdateBlock((McpeUpdateBlock) p),
		[typeof(McpeAddPainting)] = (h, p)
			=> h.HandleMcpeAddPainting((McpeAddPainting) p),
		[typeof(McpeLevelEvent)] = (h, p)
			=> h.HandleMcpeLevelEvent((McpeLevelEvent) p),
		[typeof(McpeBlockEvent)] = (h, p)
			=> h.HandleMcpeBlockEvent((McpeBlockEvent) p),
		[typeof(McpeActorEvent)] = (h, p)
			=> h.HandleMcpeEntityEvent((McpeActorEvent) p),
		[typeof(McpeMobEffect)] = (h, p)
			=> h.HandleMcpeMobEffect((McpeMobEffect) p),
		[typeof(McpeUpdateAttributes)] = (h, p)
			=> h.HandleMcpeUpdateAttributes((McpeUpdateAttributes) p),
		[typeof(McpeInventoryTransaction)] = (h, p)
			=> h.HandleMcpeInventoryTransaction((McpeInventoryTransaction) p),
		[typeof(McpeMobEquipment)] = (h, p)
			=> h.HandleMcpeMobEquipment((McpeMobEquipment) p),
		[typeof(McpeMobArmorEquipment)] = (h, p)
			=> h.HandleMcpeMobArmorEquipment((McpeMobArmorEquipment) p),
		[typeof(McpeInteract)] = (h, p)
			=> h.HandleMcpeInteract((McpeInteract) p),
		[typeof(McpeHurtArmor)] = (h, p)
			=> h.HandleMcpeHurtArmor((McpeHurtArmor) p),
		[typeof(McpeSetActorData)] = (h, p)
			=> h.HandleMcpeSetActorData((McpeSetActorData) p),
		[typeof(McpeSetActorMotion)] = (h, p)
			=> h.HandleMcpeSetActorMotion((McpeSetActorMotion) p),
		[typeof(McpeSetActorLink)] = (h, p)
			=> h.HandleMcpeSetActorLink((McpeSetActorLink) p),
		[typeof(McpeSetHealth)] = (h, p)
			=> h.HandleMcpeSetHealth((McpeSetHealth) p),
		[typeof(McpeSetSpawnPosition)] = (h, p)
			=> h.HandleMcpeSetSpawnPosition((McpeSetSpawnPosition) p),
		[typeof(McpeAnimate)] = (h, p)
			=> h.HandleMcpeAnimate((McpeAnimate) p),
		[typeof(McpeRespawn)] = (h, p)
			=> h.HandleMcpeRespawn((McpeRespawn) p),
		[typeof(McpeContainerOpen)] = (h, p)
			=> h.HandleMcpeContainerOpen((McpeContainerOpen) p),
		[typeof(McpeContainerClose)] = (h, p)
			=> h.HandleMcpeContainerClose((McpeContainerClose) p),
		[typeof(McpePlayerHotbar)] = (h, p)
			=> h.HandleMcpePlayerHotbar((McpePlayerHotbar) p),
		[typeof(McpeInventoryContent)] = (h, p)
			=> h.HandleMcpeInventoryContent((McpeInventoryContent) p),
		[typeof(McpeInventorySlot)] = (h, p)
			=> h.HandleMcpeInventorySlot((McpeInventorySlot) p),
		[typeof(McpeContainerSetData)] = (h, p)
			=> h.HandleMcpeContainerSetData((McpeContainerSetData) p),
		[typeof(McpeCraftingData)] = (h, p)
			=> h.HandleMcpeCraftingData((McpeCraftingData) p),
		[typeof(McpeGuiDataPickItem)] = (h, p)
			=> h.HandleMcpeGuiDataPickItem((McpeGuiDataPickItem) p),
		[typeof(McpeBlockActorData)] = (h, p)
			=> h.HandleMcpeBlockEntityData((McpeBlockActorData) p),
		[typeof(McpeLevelChunk)] = (h, p)
			=> h.HandleMcpeLevelChunk((McpeLevelChunk) p),
		[typeof(McpeSetCommandsEnabled)] = (h, p)
			=> h.HandleMcpeSetCommandsEnabled((McpeSetCommandsEnabled) p),
		[typeof(McpeSetDifficulty)] = (h, p)
			=> h.HandleMcpeSetDifficulty((McpeSetDifficulty) p),
		[typeof(McpeChangeDimension)] = (h, p)
			=> h.HandleMcpeChangeDimension((McpeChangeDimension) p),
		[typeof(McpeSetPlayerGameType)] = (h, p)
			=> h.HandleMcpeSetPlayerGameType((McpeSetPlayerGameType) p),
		[typeof(McpePlayerList)] = (h, p)
			=> h.HandleMcpePlayerList((McpePlayerList) p),
		[typeof(McpeSimpleEvent)] = (h, p)
			=> h.HandleMcpeSimpleEvent((McpeSimpleEvent) p),
		[typeof(McpeLegacyTelemetryEvent)] = (h, p)
			=> h.HandleMcpeTelemetryEvent((McpeLegacyTelemetryEvent) p),
		[typeof(McpeClientboundMapItemData)] = (h, p)
			=> h.HandleMcpeClientboundMapItemData((McpeClientboundMapItemData) p),
		[typeof(McpeMapInfoRequest)] = (h, p)
			=> h.HandleMcpeMapInfoRequest((McpeMapInfoRequest) p),
		[typeof(McpeRequestChunkRadius)] = (h, p)
			=> h.HandleMcpeRequestChunkRadius((McpeRequestChunkRadius) p),
		[typeof(McpeChunkRadiusUpdate)] = (h, p)
			=> h.HandleMcpeChunkRadiusUpdate((McpeChunkRadiusUpdate) p),
		[typeof(McpeGameRulesChanged)] = (h, p)
			=> h.HandleMcpeGameRulesChanged((McpeGameRulesChanged) p),
		[typeof(McpeCamera)] = (h, p)
			=> h.HandleMcpeCamera((McpeCamera) p),
		[typeof(McpeBossEvent)] = (h, p)
			=> h.HandleMcpeBossEvent((McpeBossEvent) p),
		[typeof(McpeShowCredits)] = (h, p)
			=> h.HandleMcpeShowCredits((McpeShowCredits) p),
		[typeof(McpeAvailableCommands)] = (h, p)
			=> h.HandleMcpeAvailableCommands((McpeAvailableCommands) p),
		[typeof(McpeCommandOutput)] = (h, p)
			=> h.HandleMcpeCommandOutput((McpeCommandOutput) p),
		[typeof(McpeUpdateTrade)] = (h, p)
			=> h.HandleMcpeUpdateTrade((McpeUpdateTrade) p),
		[typeof(McpeUpdateEquipment)] = (h, p)
			=> h.HandleMcpeUpdateEquipment((McpeUpdateEquipment) p),
		[typeof(McpeResourcePackDataInfo)] = (h, p)
			=> h.HandleMcpeResourcePackDataInfo((McpeResourcePackDataInfo) p),
		[typeof(McpeResourcePackChunkData)] = (h, p)
			=> h.HandleMcpeResourcePackChunkData((McpeResourcePackChunkData) p),
		[typeof(McpeTransfer)] = (h, p)
			=> h.HandleMcpeTransfer((McpeTransfer) p),
		[typeof(McpePlaySound)] = (h, p)
			=> h.HandleMcpePlaySound((McpePlaySound) p),
		[typeof(McpeStopSound)] = (h, p)
			=> h.HandleMcpeStopSound((McpeStopSound) p),
		[typeof(McpeSetTitle)] = (h, p)
			=> h.HandleMcpeSetTitle((McpeSetTitle) p),
		[typeof(McpeAddBehaviorTree)] = (h, p)
			=> h.HandleMcpeAddBehaviorTree((McpeAddBehaviorTree) p),
		[typeof(McpeStructureBlockUpdate)] = (h, p)
			=> h.HandleMcpeStructureBlockUpdate((McpeStructureBlockUpdate) p),
		[typeof(McpeShowStoreOffer)] = (h, p)
			=> h.HandleMcpeShowStoreOffer((McpeShowStoreOffer) p),
		[typeof(McpePlayerSkin)] = (h, p)
			=> h.HandleMcpePlayerSkin((McpePlayerSkin) p),
		[typeof(McpeSubClientLogin)] = (h, p)
			=> h.HandleMcpeSubClientLogin((McpeSubClientLogin) p),
		[typeof(McpeInitiateWebSocketConnection)] = (h, p)
			=> h.HandleMcpeInitiateWebSocketConnection((McpeInitiateWebSocketConnection) p),
		[typeof(McpeSetLastHurtBy)] = (h, p)
			=> h.HandleMcpeSetLastHurtBy((McpeSetLastHurtBy) p),
		[typeof(McpeBookEdit)] = (h, p)
			=> h.HandleMcpeBookEdit((McpeBookEdit) p),
		[typeof(McpeNpcRequest)] = (h, p)
			=> h.HandleMcpeNpcRequest((McpeNpcRequest) p),
		[typeof(McpeModalFormRequest)] = (h, p)
			=> h.HandleMcpeModalFormRequest((McpeModalFormRequest) p),
		[typeof(McpeServerSettingsResponse)] = (h, p)
			=> h.HandleMcpeServerSettingsResponse((McpeServerSettingsResponse) p),
		[typeof(McpeShowProfile)] = (h, p)
			=> h.HandleMcpeShowProfile((McpeShowProfile) p),
		[typeof(McpeSetDefaultGameType)] = (h, p)
			=> h.HandleMcpeSetDefaultGameType((McpeSetDefaultGameType) p),
		[typeof(McpeRemoveObjective)] = (h, p)
			=> h.HandleMcpeRemoveObjective((McpeRemoveObjective) p),
		[typeof(McpeSetDisplayObjective)] = (h, p)
			=> h.HandleMcpeSetDisplayObjective((McpeSetDisplayObjective) p),
		[typeof(McpeSetScore)] = (h, p)
			=> h.HandleMcpeSetScore((McpeSetScore) p),
		[typeof(McpeLabTable)] = (h, p)
			=> h.HandleMcpeLabTable((McpeLabTable) p),
		[typeof(McpeUpdateBlockSynced)] = (h, p)
			=> h.HandleMcpeUpdateBlockSynced((McpeUpdateBlockSynced) p),
		[typeof(McpeMoveActorDelta)] = (h, p)
			=> h.HandleMcpeMoveEntityDelta((McpeMoveActorDelta) p),
		[typeof(McpeSetScoreboardIdentity)] = (h, p)
			=> h.HandleMcpeSetScoreboardIdentity((McpeSetScoreboardIdentity) p),
		[typeof(McpeUpdateSoftEnum)] = (h, p)
			=> h.HandleMcpeUpdateSoftEnum((McpeUpdateSoftEnum) p),
		[typeof(McpeSpawnParticleEffect)] = (h, p)
			=> h.HandleMcpeSpawnParticleEffect((McpeSpawnParticleEffect) p),
		[typeof(McpeAvailableEntityIdentifiers)] = (h, p)
			=> h.HandleMcpeAvailableEntityIdentifiers((McpeAvailableEntityIdentifiers) p),
		[typeof(McpeNetworkChunkPublisherUpdate)] = (h, p)
			=> h.HandleMcpeNetworkChunkPublisherUpdate((McpeNetworkChunkPublisherUpdate) p),
		[typeof(McpeBiomeDefinitionList)] = (h, p)
			=> h.HandleMcpeBiomeDefinitionList((McpeBiomeDefinitionList) p),
		[typeof(McpeLevelSoundEvent)] = (h, p)
			=> h.HandleMcpeLevelSoundEvent((McpeLevelSoundEvent) p),
		[typeof(McpeLevelEventGeneric)] = (h, p)
			=> h.HandleMcpeLevelEventGeneric((McpeLevelEventGeneric) p),
		[typeof(McpeLecternUpdate)] = (h, p)
			=> h.HandleMcpeLecternUpdate((McpeLecternUpdate) p),
		[typeof(McpeClientCacheStatus)] = (h, p)
			=> h.HandleMcpeClientCacheStatus((McpeClientCacheStatus) p),
		[typeof(McpeOnScreenTextureAnimation)] = (h, p)
			=> h.HandleMcpeOnScreenTextureAnimation((McpeOnScreenTextureAnimation) p),
		[typeof(McpeMapCreateLockedCopy)] = (h, p)
			=> h.HandleMcpeMapCreateLockedCopy((McpeMapCreateLockedCopy) p),
		[typeof(McpeStructureTemplateDataRequest)] = (h, p)
			=> h.HandleMcpeStructureTemplateDataExportRequest((McpeStructureTemplateDataRequest) p),
		[typeof(McpeStructureTemplateDataResponse)] = (h, p)
			=> h.HandleMcpeStructureTemplateDataExportResponse((McpeStructureTemplateDataResponse) p),
		[typeof(McpeUpdateBlockProperties)] = (h, p)
			=> h.HandleMcpeUpdateBlockProperties((McpeUpdateBlockProperties) p),
		[typeof(McpeClientCacheBlobStatus)] = (h, p)
			=> h.HandleMcpeClientCacheBlobStatus((McpeClientCacheBlobStatus) p),
		[typeof(McpeClientCacheMissResponse)] = (h, p)
			=> h.HandleMcpeClientCacheMissResponse((McpeClientCacheMissResponse) p),
		[typeof(McpeNetworkSettings)] = (h, p)
			=> h.HandleMcpeNetworkSettings((McpeNetworkSettings) p),
		[typeof(McpeCreativeContent)] = (h, p)
			=> h.HandleMcpeCreativeContent((McpeCreativeContent) p),
		[typeof(McpePlayerEnchantOptions)] = (h, p)
			=> h.HandleMcpePlayerEnchantOptions((McpePlayerEnchantOptions) p),
		[typeof(McpeItemStackResponse)] = (h, p)
			=> h.HandleMcpeItemStackResponse((McpeItemStackResponse) p),
		[typeof(McpeItemComponent)] = (h, p)
			=> h.HandleMcpeItemComponent((McpeItemComponent) p),
		[typeof(McpeUpdateSubChunkBlocks)] = (h, p)
			=> h.HandleMcpeUpdateSubChunkBlocksPacket((McpeUpdateSubChunkBlocks) p),
		[typeof(McpeSubChunk)] = (h, p)
			=> h.HandleMcpeSubChunkPacket((McpeSubChunk) p),
		[typeof(McpeDimensionData)] = (h, p)
			=> h.HandleMcpeDimensionData((McpeDimensionData) p),
		[typeof(McpeUpdateAbilities)] = (h, p)
			=> h.HandleMcpeUpdateAbilities((McpeUpdateAbilities) p),
		[typeof(McpeUpdateAdventureSettings)] = (h, p)
			=> h.HandleMcpeUpdateAdventureSettings((McpeUpdateAdventureSettings) p),
		[typeof(McpeTrimData)] = (h, p)
			=> h.HandleMcpeTrimData((McpeTrimData) p),
		[typeof(McpeOpenSign)] = (h, p)
			=> h.HandleMcpeOpenSign((McpeOpenSign) p),
		[typeof(FtlCreatePlayer)] = (h, p)
			=> h.HandleFtlCreatePlayer((FtlCreatePlayer) p),
		[typeof(McpeEmotePacket)] = (h, p)
			=> h.HandleMcpeEmote((McpeEmotePacket) p),
		[typeof(McpeEmoteList)] = (h, p)
			=> h.HandleMcpeEmoteList((McpeEmoteList) p),
		[typeof(McpePlayerFog)] = (h, p)
			=> h.HandleMcpePlayerFog((McpePlayerFog) p),
		[typeof(McpeAnimateEntity)] = (h, p)
			=> h.HandleMcpeAnimateEntity((McpeAnimateEntity) p),
		[typeof(McpeClientboundCloseForm)] = (h, p)
			=> h.HandleMcpeCloseForm((McpeClientboundCloseForm) p)
	};

	public void HandlePacket(Packet message)
	{
		if (Handlers.TryGetValue(message.GetType(), out Action<IMcpeClientMessageHandler, Packet>? handler)) 
			handler(messageHandler, message);
	}
}

public static class PacketFactory
{
	public static ICustomPacketFactory? CustomPacketFactory { get; set; } = null;

	private static readonly Dictionary<string, Dictionary<short, Func<ReadOnlyMemory<byte>, Packet>>> Decoders = new()
	{
		["raknet"] = new Dictionary<short, Func<ReadOnlyMemory<byte>, Packet>>
		{
			[0x00] = buffer => ConnectedPing.CreateObject().Decode(buffer),
			[0x01] = buffer => UnconnectedPing.CreateObject().Decode(buffer),
			[0x03] = buffer => ConnectedPong.CreateObject().Decode(buffer),
			[0x04] = buffer => DetectLostConnections.CreateObject().Decode(buffer),
			[0x1C] = buffer => UnconnectedPong.CreateObject().Decode(buffer),
			[0x05] = buffer => OpenConnectionRequest1.CreateObject().Decode(buffer),
			[0x06] = buffer => OpenConnectionReply1.CreateObject().Decode(buffer),
			[0x07] = buffer => OpenConnectionRequest2.CreateObject().Decode(buffer),
			[0x08] = buffer => OpenConnectionReply2.CreateObject().Decode(buffer),
			[0x09] = buffer => ConnectionRequest.CreateObject().Decode(buffer),
			[0x10] = buffer => ConnectionRequestAccepted.CreateObject().Decode(buffer),
			[0x13] = buffer => NewIncomingConnection.CreateObject().Decode(buffer),
			[0x14] = buffer => NoFreeIncomingConnections.CreateObject().Decode(buffer),
			[0x15] = buffer => DisconnectionNotification.CreateObject().Decode(buffer),
			[0x17] = buffer => ConnectionBanned.CreateObject().Decode(buffer),
			[0x1A] = buffer => IpRecentlyConnected.CreateObject().Decode(buffer),
			[0xFE] = buffer => McpeWrapper.CreateObject().Decode(buffer),
		},
		["ftl"] = new Dictionary<short, Func<ReadOnlyMemory<byte>, Packet>>
		{
			[0x01] = buffer => FtlCreatePlayer.CreateObject().Decode(buffer),
		},
		["default"] = new Dictionary<short, Func<ReadOnlyMemory<byte>, Packet>>
		{
			[0x01] = buffer => McpeLogin.CreateObject().Decode(buffer),
			[0x02] = buffer => McpePlayStatus.CreateObject().Decode(buffer),
			[0x03] = buffer => McpeServerToClientHandshake.CreateObject().Decode(buffer),
			[0x04] = buffer => McpeClientToServerHandshake.CreateObject().Decode(buffer),
			[0x05] = buffer => McpeDisconnect.CreateObject().Decode(buffer),
			[0x06] = buffer => McpeResourcePacksInfo.CreateObject().Decode(buffer),
			[0x07] = buffer => McpeResourcePackStack.CreateObject().Decode(buffer),
			[0x08] = buffer => McpeResourcePackClientResponse.CreateObject().Decode(buffer),
			[0x09] = buffer => McpeText.CreateObject().Decode(buffer),
			[0x0A] = buffer => McpeSetTime.CreateObject().Decode(buffer),
			[0x0B] = buffer => McpeStartGame.CreateObject().Decode(buffer),
			[0x0C] = buffer => McpeAddPlayer.CreateObject().Decode(buffer),
			[0x0D] = buffer => McpeAddActor.CreateObject().Decode(buffer),
			[0x0E] = buffer => McpeRemoveActor.CreateObject().Decode(buffer),
			[0x0F] = buffer => McpeAddItemActor.CreateObject().Decode(buffer),
			[0x11] = buffer => McpeTakeItemActor.CreateObject().Decode(buffer),
			[0x12] = buffer => McpeMoveActor.CreateObject().Decode(buffer),
			[0x13] = buffer => McpeMovePlayer.CreateObject().Decode(buffer),
			[0x14] = buffer => McpeRiderJump.CreateObject().Decode(buffer),
			[0x15] = buffer => McpeUpdateBlock.CreateObject().Decode(buffer),
			[0x16] = buffer => McpeAddPainting.CreateObject().Decode(buffer),
			[0x19] = buffer => McpeLevelEvent.CreateObject().Decode(buffer),
			[0x1A] = buffer => McpeBlockEvent.CreateObject().Decode(buffer),
			[0x1B] = buffer => McpeActorEvent.CreateObject().Decode(buffer),
			[0x1C] = buffer => McpeMobEffect.CreateObject().Decode(buffer),
			[0x1D] = buffer => McpeUpdateAttributes.CreateObject().Decode(buffer),
			[0x1E] = buffer => McpeInventoryTransaction.CreateObject().Decode(buffer),
			[0x1F] = buffer => McpeMobEquipment.CreateObject().Decode(buffer),
			[0x20] = buffer => McpeMobArmorEquipment.CreateObject().Decode(buffer),
			[0x21] = buffer => McpeInteract.CreateObject().Decode(buffer),
			[0x22] = buffer => McpeBlockPickRequest.CreateObject().Decode(buffer),
			[0x23] = buffer => McpeActorPickRequest.CreateObject().Decode(buffer),
			[0x24] = buffer => McpePlayerAction.CreateObject().Decode(buffer),
			[0x26] = buffer => McpeHurtArmor.CreateObject().Decode(buffer),
			[0x27] = buffer => McpeSetActorData.CreateObject().Decode(buffer),
			[0x28] = buffer => McpeSetActorMotion.CreateObject().Decode(buffer),
			[0x29] = buffer => McpeSetActorLink.CreateObject().Decode(buffer),
			[0x2A] = buffer => McpeSetHealth.CreateObject().Decode(buffer),
			[0x2B] = buffer => McpeSetSpawnPosition.CreateObject().Decode(buffer),
			[0x2C] = buffer => McpeAnimate.CreateObject().Decode(buffer),
			[0x2D] = buffer => McpeRespawn.CreateObject().Decode(buffer),
			[0x2E] = buffer => McpeContainerOpen.CreateObject().Decode(buffer),
			[0x2F] = buffer => McpeContainerClose.CreateObject().Decode(buffer),
			[0x30] = buffer => McpePlayerHotbar.CreateObject().Decode(buffer),
			[0x31] = buffer => McpeInventoryContent.CreateObject().Decode(buffer),
			[0x32] = buffer => McpeInventorySlot.CreateObject().Decode(buffer),
			[0x33] = buffer => McpeContainerSetData.CreateObject().Decode(buffer),
			[0x34] = buffer => McpeCraftingData.CreateObject().Decode(buffer),
			[0x36] = buffer => McpeGuiDataPickItem.CreateObject().Decode(buffer),
			[0x38] = buffer => McpeBlockActorData.CreateObject().Decode(buffer),
			[0x39] = buffer => McpePlayerInput.CreateObject().Decode(buffer),
			[0x3a] = buffer => McpeLevelChunk.CreateObject().Decode(buffer),
			[0x3b] = buffer => McpeSetCommandsEnabled.CreateObject().Decode(buffer),
			[0x3c] = buffer => McpeSetDifficulty.CreateObject().Decode(buffer),
			[0x3d] = buffer => McpeChangeDimension.CreateObject().Decode(buffer),
			[0x3e] = buffer => McpeSetPlayerGameType.CreateObject().Decode(buffer),
			[0x3f] = buffer => McpePlayerList.CreateObject().Decode(buffer),
			[0x40] = buffer => McpeSimpleEvent.CreateObject().Decode(buffer),
			[0x41] = buffer => McpeLegacyTelemetryEvent.CreateObject().Decode(buffer),
			[0x43] = buffer => McpeClientboundMapItemData.CreateObject().Decode(buffer),
			[0x44] = buffer => McpeMapInfoRequest.CreateObject().Decode(buffer),
			[0x45] = buffer => McpeRequestChunkRadius.CreateObject().Decode(buffer),
			[0x46] = buffer => McpeChunkRadiusUpdate.CreateObject().Decode(buffer),
			[0x48] = buffer => McpeGameRulesChanged.CreateObject().Decode(buffer),
			[0x49] = buffer => McpeCamera.CreateObject().Decode(buffer),
			[0x4a] = buffer => McpeBossEvent.CreateObject().Decode(buffer),
			[0x4b] = buffer => McpeShowCredits.CreateObject().Decode(buffer),
			[0x4c] = buffer => McpeAvailableCommands.CreateObject().Decode(buffer),
			[0x4d] = buffer => McpeCommandRequest.CreateObject().Decode(buffer),
			[0x4e] = buffer => McpeCommandBlockUpdate.CreateObject().Decode(buffer),
			[0x4f] = buffer => McpeCommandOutput.CreateObject().Decode(buffer),
			[0x50] = buffer => McpeUpdateTrade.CreateObject().Decode(buffer),
			[0x51] = buffer => McpeUpdateEquipment.CreateObject().Decode(buffer),
			[0x52] = buffer => McpeResourcePackDataInfo.CreateObject().Decode(buffer),
			[0x53] = buffer => McpeResourcePackChunkData.CreateObject().Decode(buffer),
			[0x54] = buffer => McpeResourcePackChunkRequest.CreateObject().Decode(buffer),
			[0x55] = buffer => McpeTransfer.CreateObject().Decode(buffer),
			[0x56] = buffer => McpePlaySound.CreateObject().Decode(buffer),
			[0x57] = buffer => McpeStopSound.CreateObject().Decode(buffer),
			[0x58] = buffer => McpeSetTitle.CreateObject().Decode(buffer),
			[0x59] = buffer => McpeAddBehaviorTree.CreateObject().Decode(buffer),
			[0x5a] = buffer => McpeStructureBlockUpdate.CreateObject().Decode(buffer),
			[0x5b] = buffer => McpeShowStoreOffer.CreateObject().Decode(buffer),
			[0x5c] = buffer => McpePurchaseReceipt.CreateObject().Decode(buffer),
			[0x5d] = buffer => McpePlayerSkin.CreateObject().Decode(buffer),
			[0x5e] = buffer => McpeSubClientLogin.CreateObject().Decode(buffer),
			[0x5f] = buffer => McpeInitiateWebSocketConnection.CreateObject().Decode(buffer),
			[0x60] = buffer => McpeSetLastHurtBy.CreateObject().Decode(buffer),
			[0x61] = buffer => McpeBookEdit.CreateObject().Decode(buffer),
			[0x62] = buffer => McpeNpcRequest.CreateObject().Decode(buffer),
			[0x63] = buffer => McpePhotoTransfer.CreateObject().Decode(buffer),
			[0x64] = buffer => McpeModalFormRequest.CreateObject().Decode(buffer),
			[0x65] = buffer => McpeModalFormResponse.CreateObject().Decode(buffer),
			[0x66] = buffer => McpeServerSettingsRequest.CreateObject().Decode(buffer),
			[0x67] = buffer => McpeServerSettingsResponse.CreateObject().Decode(buffer),
			[0x68] = buffer => McpeShowProfile.CreateObject().Decode(buffer),
			[0x69] = buffer => McpeSetDefaultGameType.CreateObject().Decode(buffer),
			[0x6a] = buffer => McpeRemoveObjective.CreateObject().Decode(buffer),
			[0x6b] = buffer => McpeSetDisplayObjective.CreateObject().Decode(buffer),
			[0x6c] = buffer => McpeSetScore.CreateObject().Decode(buffer),
			[0x6d] = buffer => McpeLabTable.CreateObject().Decode(buffer),
			[0x6e] = buffer => McpeUpdateBlockSynced.CreateObject().Decode(buffer),
			[0x6f] = buffer => McpeMoveActorDelta.CreateObject().Decode(buffer),
			[0x70] = buffer => McpeSetScoreboardIdentity.CreateObject().Decode(buffer),
			[0x71] = buffer => McpeSetLocalPlayerAsInitialized.CreateObject().Decode(buffer),
			[0x72] = buffer => McpeUpdateSoftEnum.CreateObject().Decode(buffer),
			[0x76] = buffer => McpeSpawnParticleEffect.CreateObject().Decode(buffer),
			[0x77] = buffer => McpeAvailableEntityIdentifiers.CreateObject().Decode(buffer),
			[0x79] = buffer => McpeNetworkChunkPublisherUpdate.CreateObject().Decode(buffer),
			[0x7a] = buffer => McpeBiomeDefinitionList.CreateObject().Decode(buffer),
			[0x7b] = buffer => McpeLevelSoundEvent.CreateObject().Decode(buffer),
			[0x7c] = buffer => McpeLevelEventGeneric.CreateObject().Decode(buffer),
			[0x7d] = buffer => McpeLecternUpdate.CreateObject().Decode(buffer),
			[0x81] = buffer => McpeClientCacheStatus.CreateObject().Decode(buffer),
			[0x82] = buffer => McpeOnScreenTextureAnimation.CreateObject().Decode(buffer),
			[0x83] = buffer => McpeMapCreateLockedCopy.CreateObject().Decode(buffer),
			[0x84] = buffer => McpeStructureTemplateDataRequest.CreateObject().Decode(buffer),
			[0x85] = buffer => McpeStructureTemplateDataResponse.CreateObject().Decode(buffer),
			[0x86] = buffer => McpeUpdateBlockProperties.CreateObject().Decode(buffer),
			[0x87] = buffer => McpeClientCacheBlobStatus.CreateObject().Decode(buffer),
			[0x88] = buffer => McpeClientCacheMissResponse.CreateObject().Decode(buffer),
			[0x8f] = buffer => McpeNetworkSettings.CreateObject().Decode(buffer),
			[0x90] = buffer => McpePlayerAuthInput.CreateObject().Decode(buffer),
			[0x91] = buffer => McpeCreativeContent.CreateObject().Decode(buffer),
			[0x92] = buffer => McpePlayerEnchantOptions.CreateObject().Decode(buffer),
			[0x93] = buffer => McpeItemStackRequest.CreateObject().Decode(buffer),
			[0x94] = buffer => McpeItemStackResponse.CreateObject().Decode(buffer),
			[0x97] = buffer => McpeUpdatePlayerGameType.CreateObject().Decode(buffer),
			[0x9c] = buffer => McpeViolationWarning.CreateObject().Decode(buffer),
			[0xa2] = buffer => McpeItemComponent.CreateObject().Decode(buffer),
			[0xac] = buffer => McpeUpdateSubChunkBlocks.CreateObject().Decode(buffer),
			[0xae] = buffer => McpeSubChunk.CreateObject().Decode(buffer),
			[0xaf] = buffer => McpeSubChunkRequestPacket.CreateObject().Decode(buffer),
			[0xb4] = buffer => McpeDimensionData.CreateObject().Decode(buffer),
			[0xbb] = buffer => McpeUpdateAbilities.CreateObject().Decode(buffer),
			[0xbc] = buffer => McpeUpdateAdventureSettings.CreateObject().Decode(buffer),
			[0xb8] = buffer => McpeRequestAbility.CreateObject().Decode(buffer),
			[0xc1] = buffer => McpeRequestNetworkSettings.CreateObject().Decode(buffer),
			[0x12e] = buffer => McpeTrimData.CreateObject().Decode(buffer),
			[0x12f] = buffer => McpeOpenSign.CreateObject().Decode(buffer),
			[0x8a] = buffer => McpeEmotePacket.CreateObject().Decode(buffer),
			[0x98] = buffer => McpeEmoteList.CreateObject().Decode(buffer),
			[0xb9] = buffer => McpeRequestPermission.CreateObject().Decode(buffer),
			[0x133] = buffer => McpeSetInventoryOptions.CreateObject().Decode(buffer),
			[0x136] = buffer => McpeClientboundCloseForm.CreateObject().Decode(buffer),
			[0x138] = buffer => McpeServerboundLoadingScreen.CreateObject().Decode(buffer),
			[0xa0] = buffer => McpePlayerFog.CreateObject().Decode(buffer),
			[0x8d] = buffer => McpeAnvilDamage.CreateObject().Decode(buffer)
		}
	};

	public static Packet Create(short messageId, ReadOnlyMemory<byte> buffer, string ns)
	{
		Packet? packet = CustomPacketFactory?.Create(messageId, buffer, ns);
		if (packet != null)
			return packet;
		if (!Decoders.TryGetValue(ns, out Dictionary<short, Func<ReadOnlyMemory<byte>, Packet>>? namespaceHandlers))
		{
			ns = "default";
			Decoders.TryGetValue(ns, out namespaceHandlers);
		}

		if (namespaceHandlers != null && namespaceHandlers.TryGetValue(messageId, out var decodeFunc)) return decodeFunc(buffer);

		return null;
	}
}

public enum CommandPermission
{
	Normal = 0,
	Operator = 1,
	Host = 2,
	Automation = 3,
	Admin = 4
}

public enum PermissionLevel
{
	Visitor = 0,
	Member = 1,
	Operator = 2,
	Custom = 3
}

public enum ActionPermissions
{
	BuildAndMine = 0x1,
	DoorsAndSwitches = 0x2,
	OpenContainers = 0x4,
	AttackPlayers = 0x8,
	AttackMobs = 0x10,
	Operator = 0x20,
	Teleport = 0x80,
	Default = BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs,
	All = BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs | Operator | Teleport
}