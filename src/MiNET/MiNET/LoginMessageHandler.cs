#region LICENSE

// The contents of this file are subject to the Common Public Attribution
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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2021 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using fNbt;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Skins;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using SicStream;

namespace MiNET
{
	public class LoginMessageHandler : IMcpeMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LoginMessageHandler));

		private readonly BedrockMessageHandler _bedrockHandler;
		private readonly RakSession _session;
		private readonly IServerManager _serverManager;

		private object _loginSyncLock = new object();
		private PlayerInfo _playerInfo = new PlayerInfo();

		public LoginMessageHandler(BedrockMessageHandler bedrockHandler, RakSession session, IServerManager serverManager)
		{
			JWT.DefaultSettings.JsonMapper = new NewtonsoftMapper();
			_bedrockHandler = bedrockHandler;
			_session = session;
			_serverManager = serverManager;
		}

		public void Disconnect(string reason, bool sendDisconnect = true)
		{
		}

		public virtual void HandleMcpeRequestNetworkSettings(McpeRequestNetworkSettings message)
		{
			McpeNetworkSettings settingsPacket = McpeNetworkSettings.CreateObject();
			settingsPacket.compressionAlgorithm = 0;//zlib
			settingsPacket.compressionThreshold = 1;
			settingsPacket.clientThrottleEnabled = false;
			settingsPacket.clientThrottleScalar = 0;
			settingsPacket.clientThrottleThreshold = 0;
			settingsPacket.ForceClear = true; // Must be!

			_session.SendPrepareDirectPacket(settingsPacket);
			Thread.Sleep(1000);
			_session.EnableCompression = true;
		}

		public virtual void HandleMcpeLogin(McpeLogin message)
		{
			// Only one login!
			lock (_loginSyncLock)
			{
				if (_session.Username != null)
				{
					Log.Info($"Player {_session.Username} doing multiple logins");
					return;
				}

				_session.Username = string.Empty;
			}

			_playerInfo.ProtocolVersion = message.ProtocolVersion;
			DecodeCert(message);
		}

		public void DecodeCert(McpeLogin message)
		{
			byte[] buffer = message.Payload;

			if (message.Payload.Length != buffer.Length)
			{
				Log.Debug($"Wrong lenght {message.Payload.Length} != {message.Payload.Length}");
				throw new Exception($"Wrong lenght {message.Payload.Length} != {message.Payload.Length}");
			}

			if (Log.IsDebugEnabled) Log.Debug("Lenght: " + message.Payload.Length + ", Message: " + buffer.EncodeBase64());

			string certificateChain;
			string skinData;

			try
			{
				var destination = new MemoryStream(buffer);
				destination.Position = 0;
				NbtBinaryReader reader = new NbtBinaryReader(destination, false);

				var countCertData = reader.ReadInt32();
				certificateChain = Encoding.UTF8.GetString(reader.ReadBytes(countCertData));
				if (Log.IsDebugEnabled) Log.Debug($"Certificate Chain (Lenght={countCertData})\n{certificateChain}");

				var countSkinData = reader.ReadInt32();
				skinData = Encoding.UTF8.GetString(reader.ReadBytes(countSkinData));
				if (Log.IsDebugEnabled) Log.Debug($"Skin data (Lenght={countSkinData})\n{skinData}");
			}
			catch (Exception e)
			{
				Log.Error("Parsing login", e);
				return;
			}

			try
			{
				{
					IDictionary<string, dynamic> headers = JWT.Headers(skinData);
					dynamic payload = JObject.Parse(JWT.Payload(skinData));

					if (Log.IsDebugEnabled) Log.Debug($"Skin JWT Header: {string.Join(";", headers)}");
					if (Log.IsDebugEnabled) Log.Debug($"Skin JWT Payload:\n{payload.ToString()}");
					try
					{
						_playerInfo.ClientId = payload.ClientRandomId;
						_playerInfo.CurrentInputMode = payload.CurrentInputMode;
						_playerInfo.DefaultInputMode = payload.DefaultInputMode;
						_playerInfo.DeviceModel = payload.DeviceModel;
						_playerInfo.DeviceOS = payload.DeviceOS;
						_playerInfo.GameVersion = payload.GameVersion;
						_playerInfo.GuiScale = payload.GuiScale;
						_playerInfo.LanguageCode = payload.LanguageCode;
						_playerInfo.PlatformChatId = payload.PlatformOnlineId;
						_playerInfo.ServerAddress = payload.ServerAddress;
						_playerInfo.UIProfile = payload.UIProfile;
						_playerInfo.ThirdPartyName = payload.ThirdPartyName;
						_playerInfo.TenantId = payload.TenantId;
						_playerInfo.DeviceId = payload.DeviceId;

						_playerInfo.Skin = new Skin
						{
							Cape = new Cape
							{
								Data = Convert.FromBase64String((string) payload.CapeData ?? string.Empty),
								Id = payload.CapeId,
								ImageHeight = payload.CapeImageHeight,
								ImageWidth = payload.CapeImageWidth,
								OnClassicSkin = payload.CapeOnClassicSkin,
							},
							SkinId = payload.SkinId,
							ResourcePatch = Encoding.UTF8.GetString(Convert.FromBase64String((string) payload.SkinResourcePatch ?? string.Empty)),
							Width = payload.SkinImageWidth,
							Height = payload.SkinImageHeight,
							Data = Convert.FromBase64String((string) payload.SkinData ?? string.Empty),
							GeometryData = Encoding.UTF8.GetString(Convert.FromBase64String((string) payload.SkinGeometryData ?? string.Empty)),
							AnimationData = payload.SkinAnimationData,
							IsPremiumSkin = payload.PremiumSkin,
							IsPersonaSkin = payload.PersonaSkin,
						};
						foreach (dynamic animationData in payload.AnimatedImageData)
						{
							_playerInfo.Skin.Animations.Add(
								new Animation
								{
									Image = Convert.FromBase64String((string) animationData.Image ?? string.Empty),
									ImageHeight = animationData.ImageHeight,
									ImageWidth = animationData.ImageWidth,
									FrameCount = animationData.Frames,
									Expression = animationData.AnimationExpression,
									Type = animationData.Type,
								}
							);
						}
					}
					catch (Exception e)
					{
						Log.Error("Parsing skin data", e);
					}
				}

				{
					dynamic json = JObject.Parse(certificateChain);

					if (Log.IsDebugEnabled) Log.Debug($"Certificate JSON:\n{json}");

					JArray chain = json.chain;
					string validationKey = null;
					string identityPublicKey = null;

					foreach (JToken token in chain)
					{
						IDictionary<string, dynamic> headers = JWT.Headers(token.ToString());

						if (Log.IsDebugEnabled)
						{
							Log.Debug("Raw chain element:\n" + token.ToString());
							Log.Debug($"JWT Header: {string.Join(";", headers)}");

							dynamic jsonPayload = JObject.Parse(JWT.Payload(token.ToString()));
							Log.Debug($"JWT Payload:\n{jsonPayload}");
						}
						if (!headers.ContainsKey("x5u")) continue;

						string x5u = headers["x5u"];

						if (identityPublicKey == null)
						{
							if (CertificateData.MojangRootKey.Equals(x5u, StringComparison.InvariantCultureIgnoreCase))
							{
								Log.Debug("Key is ok, and got Mojang root");
							}
							else switch (chain.Count)
							{
								case > 1:
									Log.Debug("Got client cert (client root)");
									continue;
								case 1:
									Log.Debug("Selfsigned chain");
									break;
							}
						}
						else if (identityPublicKey.Equals(x5u)) Log.Debug("Derived Key is ok");

						var x5KeyParam = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(x5u.DecodeBase64());
						var signParam = new ECParameters
						{
							Curve = ECCurve.NamedCurves.nistP384,
							Q =
							{
								X = x5KeyParam.Q.AffineXCoord.GetEncoded(),
								Y = x5KeyParam.Q.AffineYCoord.GetEncoded()
							},
						};
						signParam.Validate();

						CertificateData data = JWT.Decode<CertificateData>(token.ToString(), ECDsa.Create(signParam));

						// Validate

						if (data != null)
						{
							identityPublicKey = data.IdentityPublicKey;

							if (Log.IsDebugEnabled) Log.Debug("Decoded token success");

							if (CertificateData.MojangRootKey.Equals(x5u, StringComparison.InvariantCultureIgnoreCase))
							{
								Log.Debug("Got Mojang key. Is valid = " + data.CertificateAuthority);
								validationKey = data.IdentityPublicKey;
							}
							else if (validationKey != null && validationKey.Equals(x5u, StringComparison.InvariantCultureIgnoreCase))
							{
								_playerInfo.CertificateData = data;
							}
							else
							{
								if (data.ExtraData == null) continue;

								// Self signed, make sure they don't fake XUID
								if (data.ExtraData.Xuid != null)
								{
									Log.Warn("Received fake XUID from " + data.ExtraData.DisplayName);
									data.ExtraData.Xuid = null;
								}
								_playerInfo.CertificateData = data;
							}
						}
						else Log.Error("Not a valid Identity Public Key for decoding");
					}
					{
						_playerInfo.Username = _playerInfo.CertificateData.ExtraData.DisplayName;
						_session.Username = _playerInfo.Username;
						string identity = _playerInfo.CertificateData.ExtraData.Identity;

						if (Log.IsDebugEnabled) Log.Debug($"Connecting user {_playerInfo.Username} with identity={identity} on protocol version={_playerInfo.ProtocolVersion}");
						_playerInfo.ClientUuid = new UUID(identity);

						_bedrockHandler.CryptoContext = new CryptoContext
						{
							UseEncryption = Config.GetProperty("UseEncryptionForAll", false) || (Config.GetProperty("UseEncryption", true) && !string.IsNullOrWhiteSpace(_playerInfo.CertificateData.ExtraData.Xuid)),
						};

						if (_bedrockHandler.CryptoContext.UseEncryption)
						{
							// Use bouncy to parse the DER key
							ECPublicKeyParameters remotePublicKey = (ECPublicKeyParameters)
								PublicKeyFactory.CreateKey(_playerInfo.CertificateData.IdentityPublicKey.DecodeBase64());

							var b64RemotePublicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(remotePublicKey).GetEncoded().EncodeBase64();
							Debug.Assert(_playerInfo.CertificateData.IdentityPublicKey == b64RemotePublicKey);
							Debug.Assert(remotePublicKey.PublicKeyParamSet.Id == "1.3.132.0.34");
							Log.Debug($"{remotePublicKey.PublicKeyParamSet}");

							var generator = new ECKeyPairGenerator("ECDH");
							generator.Init(new ECKeyGenerationParameters(remotePublicKey.PublicKeyParamSet, SecureRandom.GetInstance("SHA256PRNG")));
							var keyPair = generator.GenerateKeyPair();

							ECPublicKeyParameters pubAsyKey = (ECPublicKeyParameters) keyPair.Public;
							ECPrivateKeyParameters privAsyKey = (ECPrivateKeyParameters) keyPair.Private;

							var secretPrepend = Encoding.UTF8.GetBytes("RANDOM SECRET");

							ECDHBasicAgreement agreement = new ECDHBasicAgreement();
							agreement.Init(keyPair.Private);
							byte[] secret;
							using (var sha = SHA256.Create())
							{
								secret = sha.ComputeHash(secretPrepend.Concat(agreement.CalculateAgreement(remotePublicKey).ToByteArrayUnsigned()).ToArray());
							}

							Debug.Assert(secret.Length == 32);

							if (Log.IsDebugEnabled) Log.Debug($"SECRET KEY (b64):\n{secret.EncodeBase64()}");

							var encryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
							var decryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
							decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] {0, 0, 0, 2}).ToArray()));
							encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] {0, 0, 0, 2}).ToArray()));

							//IBufferedCipher decryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
							//decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

							//IBufferedCipher encryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
							//encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

							_bedrockHandler.CryptoContext.Key = secret;
							_bedrockHandler.CryptoContext.Decryptor = decryptor;
							_bedrockHandler.CryptoContext.Encryptor = encryptor;

							var signParam = new ECParameters
							{
								Curve = ECCurve.NamedCurves.nistP384,
								Q =
								{
									X = pubAsyKey.Q.AffineXCoord.GetEncoded(),
									Y = pubAsyKey.Q.AffineYCoord.GetEncoded()
								}
							};
							signParam.D = CryptoUtils.FixDSize(privAsyKey.D.ToByteArrayUnsigned(), signParam.Q.X.Length);
							signParam.Validate();

							string signedToken = null;
							//if (_session.Server.IsEdu)
							//{
							//	EduTokenManager tokenManager = _session.Server.EduTokenManager;
							//	signedToken = tokenManager.GetSignedToken(_playerInfo.TenantId);
							//}

							var signKey = ECDsa.Create(signParam);
							var b64PublicKey = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubAsyKey).GetEncoded().EncodeBase64();
							var handshakeJson = new HandshakeData
							{
								salt = secretPrepend.EncodeBase64(),
								signedToken = signedToken
							};
							string val = JWT.Encode(handshakeJson, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64PublicKey}});

							Log.Debug($"Headers:\n{string.Join(";", JWT.Headers(val))}");
							Log.Debug($"Return salt:\n{JWT.Payload(val)}");
							Log.Debug($"JWT:\n{val}");


							var response = McpeServerToClientHandshake.CreateObject();
							response.ForceClear = true; // Must be!
							response.token = val;

							_session.SendPacket(response);

							if (Log.IsDebugEnabled) Log.Warn($"Encryption enabled for {_session.Username}");
						}
					}
				}
				if (!_bedrockHandler.CryptoContext.UseEncryption)
				{
					_bedrockHandler.Handler.HandleMcpeClientToServerHandshake(null);
				}
			}
			catch (Exception e)
			{
				Log.Error("Decrypt", e);
			}
		}

		public void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message)
		{
			Log.Warn($"{(_bedrockHandler.CryptoContext == null ? "C" : $"Encrypted c")}onnection established with {_playerInfo.Username} using MC version {_playerInfo.GameVersion} with protocol version {_playerInfo.ProtocolVersion}");

			IServer server = _serverManager.GetServer();

			IMcpeMessageHandler messageHandler = server.CreatePlayer(_session, _playerInfo);
			_bedrockHandler.Handler = messageHandler; // Replace current message handler with real one.

			if (_playerInfo.ProtocolVersion != McpeProtocolInfo.ProtocolVersion)
			{
				Log.Warn($"Wrong version ({_playerInfo.ProtocolVersion}) of Minecraft. Upgrade to join this server.");
				_session.Disconnect($"Wrong version ({_playerInfo.ProtocolVersion}) of Minecraft. This server requires {McpeProtocolInfo.ProtocolVersion}");
				return;
			}

			if (Config.GetProperty("ForceXBLAuthentication", false) && _playerInfo.CertificateData.ExtraData.Xuid == null)
			{
				Log.Warn($"You must authenticate to XBOX Live to join this server.");
				_session.Disconnect(Config.GetProperty("ForceXBLLogin", "You must authenticate to XBOX Live to join this server."));

				return;
			}

			_bedrockHandler.Handler.HandleMcpeClientToServerHandshake(null);
		}

		public void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
		{
		}

		public void HandleMcpeText(McpeText message)
		{
		}

		public void HandleMcpeMoveEntity(McpeMoveEntity message)
		{
		}

		public void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
		}

		public void HandleMcpeRiderJump(McpeRiderJump message)
		{
		}

		public void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
		}

		public void HandleMcpeClientCacheStatus(McpeClientCacheStatus message)
		{
		}

		public void HandleMcpeNetworkSettings(McpeNetworkSettings message)
		{
		}

		/// <inheritdoc />
		public void HandleMcpePlayerAuthInput(McpePlayerAuthInput message)
		{
			
		}

		public void HandleMcpeItemStackRequest(McpeItemStackRequest message)
		{
		}

		public void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message)
		{
		}

		public void HandleMcpePacketViolationWarning(McpePacketViolationWarning message)
		{
		}

		/// <inheritdoc />
		public void HandleMcpeFilterTextPacket(McpeFilterTextPacket message)
		{
		}

		/// <inheritdoc />
		public void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message)
		{
			
		}

		/// <inheritdoc />
		public void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message)
		{
			
		}

		/// <inheritdoc />
		public void HandleMcpeRequestAbility(McpeRequestAbility message)
		{
			
		}

		public void HandleMcpeEntityEvent(McpeEntityEvent message)
		{
		}

		public void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
		{
		}

		public void HandleMcpeMobEquipment(McpeMobEquipment message)
		{
		}

		public void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		public void HandleMcpeInteract(McpeInteract message)
		{
		}

		public void HandleMcpeBlockPickRequest(McpeBlockPickRequest message)
		{
		}

		public void HandleMcpeEntityPickRequest(McpeEntityPickRequest message)
		{
		}

		public void HandleMcpePlayerAction(McpePlayerAction message)
		{
		}

		public void HandleMcpeSetEntityData(McpeSetEntityData message)
		{
		}

		public void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
		}

		public void HandleMcpeAnimate(McpeAnimate message)
		{
		}

		public void HandleMcpeRespawn(McpeRespawn message)
		{
		}

		public void HandleMcpeContainerClose(McpeContainerClose message)
		{
		}

		public void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
		}

		public void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
		}

		public void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
		}

		public void HandleMcpeCraftingEvent(McpeCraftingEvent message)
		{
		}

		public void HandleMcpeAdventureSettings(McpeAdventureSettings message)
		{
		}

		public void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
		}

		public void HandleMcpePlayerInput(McpePlayerInput message)
		{
		}

		public void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message)
		{
		}

		public void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
		}

		public void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
		}

		public void HandleMcpeCommandRequest(McpeCommandRequest message)
		{
		}

		public void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message)
		{
		}

		public void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message)
		{
		}

		public void HandleMcpePurchaseReceipt(McpePurchaseReceipt message)
		{
		}

		public void HandleMcpePlayerSkin(McpePlayerSkin message)
		{
		}

		public void HandleMcpeNpcRequest(McpeNpcRequest message)
		{
		}

		public void HandleMcpePhotoTransfer(McpePhotoTransfer message)
		{
		}

		public void HandleMcpeModalFormResponse(McpeModalFormResponse message)
		{
		}

		public void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message)
		{
		}

		public void HandleMcpeLabTable(McpeLabTable messae)
		{
		}

		public void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message)
		{
		}

		public void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message)
		{
		}

		public void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message)
		{
		}

		public void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message)
		{
		}

		public void HandleMcpeScriptCustomEvent(McpeScriptCustomEvent message)
		{
		}

		public void HandleMcpeEmote(McpeEmotePacket message)
		{
		}

		public void HandleMcpeEmoteList(McpeEmoteList message)
		{
		}

		public void HandleMcpePermissionRequest(McpePermissionRequest message)
		{
		}

		public void HandleMcpeSetInventoryOptions(McpeSetInventoryOptions message)
		{
		}

		public void HandleMcpeAnvilDamage(McpeAnvilDamage message)
		{
		}

		public void HandleMcpeServerboundLoadingScreen(McpeServerboundLoadingScreen message)
		{
		}
	}

	public interface IServerManager
	{
		IServer GetServer();
	}

	public interface IServer
	{
		IMcpeMessageHandler CreatePlayer(INetworkHandler session, PlayerInfo playerInfo);
	}

	public class PlayerInfo
	{
		public int ADRole { get; set; }
		public CertificateData CertificateData { get; set; }
		public string Username { get; set; }
		public UUID ClientUuid { get; set; }
		public string ServerAddress { get; set; }
		public long ClientId { get; set; }
		public Skin Skin { get; set; }
		public int CurrentInputMode { get; set; }
		public int DefaultInputMode { get; set; }
		public string DeviceModel { get; set; }
		public string GameVersion { get; set; }
		public int DeviceOS { get; set; }
		public string DeviceId { get; set; }
		public int GuiScale { get; set; }
		public int UIProfile { get; set; }
		public int Edition { get; set; }
		public int ProtocolVersion { get; set; }
		public string LanguageCode { get; set; }
		public string PlatformChatId { get; set; }
		public string ThirdPartyName { get; set; }
		public string TenantId { get; set; }
	}

	public class DefaultServerManager : IServerManager
	{
		private readonly MiNetServer _miNetServer;
		private IServer _getServer;

		protected DefaultServerManager()
		{
		}

		public DefaultServerManager(MiNetServer miNetServer)
		{
			_miNetServer = miNetServer;
			_getServer = new DefaultServer(miNetServer);
		}

		public virtual IServer GetServer()
		{
			return _getServer;
		}
	}

	public class DefaultServer : IServer
	{
		private readonly MiNetServer _server;

		protected DefaultServer()
		{
		}

		public DefaultServer(MiNetServer server)
		{
			_server = server;
		}

		public virtual IMcpeMessageHandler CreatePlayer(INetworkHandler session, PlayerInfo playerInfo)
		{
			Player player = _server.PlayerFactory.CreatePlayer(_server, session.GetClientEndPoint(), playerInfo);
			player.NetworkHandler = session;
			player.CertificateData = playerInfo.CertificateData;
			player.Username = playerInfo.Username;
			player.ClientUuid = playerInfo.ClientUuid;
			player.ServerAddress = playerInfo.ServerAddress;
			player.ClientId = playerInfo.ClientId;
			player.Skin = playerInfo.Skin;
			player.PlayerInfo = playerInfo;

			return player;
		}
	}
}