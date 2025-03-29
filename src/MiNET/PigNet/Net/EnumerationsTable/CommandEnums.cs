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

using System;

namespace PigNet.Net.EnumerationsTable;

public enum CommandBlockMode
{
    Normal = 0,
    Repeating = 1,
    Chain = 2
}

public enum CommandOriginType
{
    Player = 0,
    CommandBlock = 1,
    MinecartCommandBlock = 2,
    DevConsole = 3,
    Test = 4,
    AutomationPlayer = 5,
    ClientAutomation = 6,
    DedicatedServer = 7,
    Entity = 8,
    Virtual = 9,
    GameArgument = 10,
    EntityServer = 11,
    Precompiled = 12,
    GameDirectorEntityServer = 13,
    Scripting = 14,
    ExecuteContext = 15
}

public enum CommandOutputType
{
    None = 0,
    LastOutput = 1,
    Silent = 2,
    AllOutput = 3,
    DataSet = 4
}

[Flags]
public enum CommandParameterOption
{
    None = 0,
    EnumAutocompleteExpansion = 0x01,
    HasSemanticConstraint = 0x02,
    EnumAsChainedCommand = 0x04
}

public enum CommandPermissionLevel
{
    Any = 0,
    GameDirectors = 1,
    Admin = 2,
    Host = 3,
    Owner = 4,
    Internal = 5
}

public enum CommandHardNonTerminal
{
    //Epsilon = NonTerminalBit,
    Int = 0x100001,
    Float = 0x100002,
    Val = 0x100003,
    RVal = 0x100004,
    WildcardInt = 0x100005,
    Operator = 0x100006,
    CompareOperator = 0x100007,
    Selection = 0x100008,
    StandaloneSelection = 0x100009,
    WildcardSelection = 0x10000a,
    NonIdSelector = 0x10000b,
    ScoresArg = 0x10000c,
    ScoresArgs = 0x10000d,
    ScoreSelectParam = 0x10000e,
    ScoreSelector = 0x10000f,
    TagSelector = 0x100010,
    FilePath = 0x100011,
    FilePathVal = 0x100012,
    FilePathCont = 0x100013,
    IntegerRangeVal = 0x100014,
    IntegerRangePostVal = 0x100015,
    IntegerRange = 0x100016,
    FullIntegerRange = 0x100017,
    RationalRangeVal = 0x100018,
    RationalRangePostVal = 0x100019,
    RationalRange = 0x10001a,
    FullRationalRange = 0x10001b,
    SelArgs = 0x10001c,
    Args = 0x10001d,
    Arg = 0x10001e,
    MArg = 0x10001f,
    MValue = 0x100020,
    NameArg = 0x100021,
    TypeArg = 0x100022,
    FamilyArg = 0x100023,
    HasPermissionArg = 0x100024,
    HasPermissionArgs = 0x100025,
    HasPermissionSelector = 0x100026,
    HasPermissionElement = 0x100027,
    HasPermissionElements = 0x100028,
    TagArg = 0x100029,
    HasItemElement = 0x10002a,
    HasItemElements = 0x10002b,
    HasItemArg = 0x10002c,
    HasItemArgs = 0x10002d,
    HasItemSelector = 0x10002e,
    EquipmentSlotEnum = 0x10002f,
    PropertyValue = 0x100030,
    HasPropertyParamValue = 0x100031,
    HasPropertyParamEnumValue = 0x100032,
    HasPropertyArg = 0x100033,
    HasPropertyArgs = 0x100034,
    HasPropertyElement = 0x100035,
    HasPropertyElements = 0x100036,
    HasPropertySelector = 0x100037,
    Id = 0x100038,
    IdCont = 0x100039,
    CoordXInt = 0x10003a,
    CoordYInt = 0x10003b,
    CoordZInt = 0x10003c,
    CoordXFloat = 0x10003d,
    CoordYFloat = 0x10003e,
    CoordZFloat = 0x10003f,
    Position = 0x100040,
    PositionFloat = 0x100041,
    MessageExp = 0x100042,
    Message = 0x100043,
    MessageRoot = 0x100044,
    PostSelector = 0x100045,
    RawText = 0x100046,
    RawTextCont = 0x100047,
    JsonValue = 0x100048,
    JsonField = 0x100049,
    JsonObject = 0x10004a,
    JsonObjectFields = 0x10004b,
    JsonObjectCont = 0x10004c,
    JsonArray = 0x10004d,
    JsonArrayValues = 0x10004e,
    JsonArrayCont = 0x10004f,
    BlockState = 0x100050,
    BlockStateKey = 0x100051,
    BlockStateValue = 0x100052,
    BlockStateValues = 0x100053,
    BlockStateArray = 0x100054,
    BlockStateArrayCont = 0x100055,
    Command = 0x100056,
    SlashCommand = 0x100057,
    CodeBuilderArg = 0x100058,
    CodeBuilderArgs = 0x100059,
    CodeBuilderSelectParam = 0x10005a,
    CodeBuilderSelector = 0x10005b
}