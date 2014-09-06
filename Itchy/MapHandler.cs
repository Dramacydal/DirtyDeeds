using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itchy.AutoTeleport;
using Itchy.D2Enums;
using WhiteMagic;

namespace Itchy
{
    using CollisionDictionary = ConcurrentDictionary<uint, CollisionMap>;
    using LevelExitsDictionary = ConcurrentDictionary<uint, List<LevelExit>>;

    public class MapHandler
    {
        protected D2Game game;
        protected volatile List<uint> revealedActs = new List<uint>();
        protected volatile List<uint> revealedLevels = new List<uint>();
        protected volatile CollisionDictionary levelCollisions = new CollisionDictionary();
        protected volatile LevelExitsDictionary levelExits = new LevelExitsDictionary();

        protected static uint[] m_ActLevels = new uint[]
        {
            1, 40, 75, 103, 109, 137
        };

        public MapHandler(D2Game game)
        {
            this.game = game;
        }

        public void Reset()
        {
            revealedActs.Clear();
            revealedLevels.Clear();
            levelExits.Clear();
            levelCollisions.Clear();
        }

        public uint GetLevel(uint dwLevel)
        {
            UnitAny unit;
            if (!game.GetPlayerUnit(out unit) || unit.pAct == 0)
                return 0;

            var act = game.Debugger.Read<Act>(unit.pAct);
            if (act.pMisc == 0)
                return 0;

            var actMisc = game.Debugger.Read<ActMisc>(act.pMisc);

            var lvl = game.Debugger.Read<Level>(actMisc.pLevelFirst);
            uint pLevel = 0;
            for (pLevel = actMisc.pLevelFirst; pLevel != 0; pLevel = lvl.pNextLevel)
            {
                if (pLevel != actMisc.pLevelFirst)
                    lvl = game.Debugger.Read<Level>(pLevel);

                if (lvl.dwLevelNo == (uint)dwLevel && lvl.dwPosX > 0)
                    return pLevel;
            }

            pLevel = game.Debugger.Call(D2Common.GetLevel,
                CallingConventionEx.FastCall,
                act.pMisc, dwLevel);

            return pLevel;
        }

        protected void InitLevel(uint pLevel)
        {
            game.Debugger.Call(D2Common.InitLevel,
                CallingConventionEx.StdCall,
                pLevel);
        }

        public void RevealLevel(uint dwLevelId)
        {
            UnitAny unit;
            if (!game.GetPlayerUnit(out unit))
                return;

            var pAct = LoadAct(unit);
            if (pAct == 0)
                return;

            _RevealLevel(dwLevelId, true);

            UnloadAct(pAct, unit);
        }

        protected void _RevealLevel(uint dwLevelId, bool buildCollisions)
        {
            var pLevel = GetLevel(dwLevelId);
            if (pLevel == 0)
                return;

            var collisionMap = buildCollisions ? new CollisionMap(game) : null;

            var lvl = game.Debugger.Read<Level>(pLevel);
            if (lvl.dwLevelNo > 255)
                return;

            if (lvl.pRoom2First == 0)
                InitLevel(pLevel);

            InitLayer(lvl.dwLevelNo);
            lvl = game.Debugger.Read<Level>(pLevel);

            if (collisionMap != null)
            {
                collisionMap.LevelOrigin.X = (int)(lvl.dwPosX * 5);
                collisionMap.LevelOrigin.Y = (int)(lvl.dwPosY * 5);
                if (!collisionMap.m_map.Create((int)lvl.dwSizeX * 5, (int)lvl.dwSizeY * 5, (ushort)MapData.Invalid))
                    collisionMap = null;
            }

            for (var pRoom = lvl.pRoom2First; pRoom != 0; )
            {
                var room = game.Debugger.Read<Room2>(pRoom);

                var actMisc2 = game.Debugger.Read<ActMisc>(lvl.pMisc);
                var roomData = false;
                if (room.pRoom1 == 0)
                {
                    roomData = true;
                    game.Debugger.Call(D2Common.AddRoomData,
                        CallingConventionEx.ThisCall,
                        0, actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);
                }

                room = game.Debugger.Read<Room2>(pRoom);
                if (room.pRoom1 == 0)
                {
                    pRoom = room.pRoom2Next;
                    continue;
                }

                var pAutomapLayer = game.Debugger.ReadUInt(D2Client.pAutoMapLayer);

                game.Debugger.Call(D2Client.RevealAutomapRoom,
                    CallingConventionEx.StdCall,
                    room.pRoom1,
                    1,
                    pAutomapLayer);

                if (collisionMap != null)
                {
                    var l = game.Debugger.Read<Level>(room.pLevel);
                    if (l.dwLevelNo == dwLevelId)
                    {
                        var r = game.Debugger.Read<Room1>(room.pRoom1);
                        collisionMap.AddCollisionData(r.Coll);
                    }
                }

                LoopPresets(pRoom, room, lvl);

                if (roomData)
                    game.Debugger.Call(D2Common.RemoveRoomData,
                        CallingConventionEx.StdCall,
                        actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);

                pRoom = room.pRoom2Next;
            }

            if (collisionMap != null)
            {
                collisionMap.FillGaps();

                FillLevelExits(lvl, collisionMap);
                levelCollisions.Add(lvl.dwLevelNo, collisionMap);
            }

            if (!revealedLevels.Contains(lvl.dwLevelNo))
                revealedLevels.Add(lvl.dwLevelNo);
        }

        protected uint LoadAct(UnitAny player)
        {
            var act = game.Debugger.Read<Act>(player.pAct);
            var expCharFlag = game.Debugger.ReadUInt(D2Client.pExpCharFlag);
            var diff = game.Debugger.ReadByte(D2Client.pDifficulty);

            return game.Debugger.Call(D2Common.LoadAct,
                CallingConventionEx.StdCall,
                player.dwAct,
                act.dwMapSeed,
                expCharFlag,
                0,
                diff,
                0,
                (uint)m_ActLevels[player.dwAct],
                game.Debugger.GetAddress(D2Client.LoadAct_1),
                game.Debugger.GetAddress(D2Client.LoadAct_2));
        }

        protected void UnloadAct(uint pAct, UnitAny player)
        {
            var path = game.Debugger.Read<Path>(player.pPath);
            var room1 = game.Debugger.Read<Room1>(path.pRoom1);
            var room2 = game.Debugger.Read<Room2>(room1.pRoom2);
            var lev = game.Debugger.Read<Level>(room2.pLevel);

            InitLayer(lev.dwLevelNo);
            game.Debugger.Call(D2Common.UnloadAct,
                CallingConventionEx.StdCall,
                pAct);
        }

        public void RevealAct()
        {
            UnitAny unit;
            if (!game.InGame || !game.GameReady() || !game.GetPlayerUnit(out unit))
            {
                game.Log("Failed to reveal act");
                return;
            }

            if (revealedActs.Contains(unit.dwAct))
            {
                game.Log("Act {0} is already revealed", unit.dwAct + 1);
                return;
            }

            if (unit.pAct == 0)
            {
                game.Log("Failed to reveal act {0}", unit.dwAct + 1);
                return;
            }

            var pAct = LoadAct(unit);
            if (pAct == 0)
            {
                game.Log("Failed to reveal act");
                return;
            }

            var act = game.Debugger.Read<Act>(pAct);
            if (act.pMisc == 0)
            {
                game.Log("Failed to reveal act");
                return;
            }

            var actMisc = game.Debugger.Read<ActMisc>(act.pMisc);
            if (actMisc.pLevelFirst == 0)
            {
                game.Log("Failed to reveal act");
                return;
            }

            for (var i = m_ActLevels[unit.dwAct]; i < m_ActLevels[unit.dwAct + 1]; ++i)
                if (!revealedLevels.Contains(i))
                    _RevealLevel(i, false);

            UnloadAct(pAct, unit);

            //PrintGameString("Revealed act", D2Color.Red);

            revealedActs.Add(unit.dwAct);

            game.Log("Revealed act {0}", unit.dwAct + 1);
        }

        protected void DrawPreset(Room2 room, Level lvl, PresetUnit preset)
        {
            var cellNo = -1;
            // Special NPC Check
            if (preset.dwType == 1)
            {
                // Izual
                if (preset.dwTxtFileNo == 256)
                    cellNo = 300;
                // Hephasto
                else if (preset.dwTxtFileNo == 402)
                    cellNo = 745;
            }
            else if (preset.dwType == 2)
            {
                switch (preset.dwTxtFileNo)
                {
                    case 580:   // Uber Chest in Lower Kurast
                        if (lvl.dwLevelNo == 79)
                            cellNo = 9;
                        break;
                    case 371:   // Countess Chest
                        cellNo = 301;
                        break;
                    case 152:   // Act 2 Orifice
                        cellNo = 300;
                        break;
                    case 460:   // Frozen Anya
                        cellNo = 1468;
                        break;
                    case 402:   // Canyon / Arcane Waypoint
                        if (lvl.dwLevelNo == 46)
                            cellNo = 0;
                        break;
                    case 376:   // Hell Forge
                        cellNo = 376;
                        break;
                    default:
                        break;
                }

                if (cellNo == -1 && preset.dwTxtFileNo <= 572)
                {
                    var pTxt = game.Debugger.Call(D2Common.GetObjectTxt,
                        CallingConventionEx.StdCall,
                        preset.dwTxtFileNo);
                    if (pTxt != 0)
                    {
                        var txt = game.Debugger.Read<ObjectTxt>(pTxt);
                        cellNo = (int)txt.nAutoMap;
                    }
                }
            }

            if (cellNo > 0/* && cellNo < 1258*/)
            {
                var pCell = game.Debugger.Call(D2Client.NewAutomapCell,
                    CallingConventionEx.FastCall);

                var cell = game.Debugger.Read<AutomapCell>(pCell);

                var x = preset.dwPosX + room.dwPosX * 5;
                var y = preset.dwPosY + room.dwPosY * 5;

                cell.nCellNo = (ushort)cellNo;
                cell.xPixel = (ushort)(((short)x - (short)y) * 1.6 + 1);
                cell.yPixel = (ushort)((y + x) * 0.8 - 3);

                game.Debugger.Write<AutomapCell>(pCell, cell);

                var pAutomapLayer = game.Debugger.ReadUInt(D2Client.pAutoMapLayer);
                game.Debugger.Call(D2Client.AddAutomapCell,
                    CallingConventionEx.FastCall,
                    pCell,
                    pAutomapLayer + 0x10);  // &((*p_D2CLIENT_AutomapLayer)->pObjects)
            }
        }

        protected void LoopPresets(uint pRoom, Room2 room, Level lvl)
        {
            for (var pPreset = room.pPreset; pPreset != 0; )
            {
                var preset = game.Debugger.Read<PresetUnit>(pPreset);

                var tileLevelNo = CollisionMap.GetTileLevelNo(game, room, preset.dwTxtFileNo);
                if (tileLevelNo != 0)
                {
                    var exit = new LevelExit
                    {
                        dwTargetLevel = tileLevelNo,
                        ptPos = new Point((int)((room.dwPosX * 5) + preset.dwPosX), (int)((room.dwPosY * 5) + preset.dwPosY)),
                        dwType = (uint)ExitType.Tile,
                        dwId = preset.dwTxtFileNo,
                        pRoom2 = pRoom
                    };

                    if (!levelExits.ContainsKey(lvl.dwLevelNo))
                        levelExits.Add(lvl.dwLevelNo, new List<LevelExit>());

                    if (levelExits[lvl.dwLevelNo].Find(it => it.dwTargetLevel == tileLevelNo) == null)
                        levelExits[lvl.dwLevelNo].Add(exit);
                }

                DrawPreset(room, lvl, preset);

                pPreset = preset.pPresetNext;
            }
        }

        protected void FillLevelExits(Level level, CollisionMap cmap)
        {
            var ptCenters = cmap.GetPtCenters();

            for (var pRoom = level.pRoom2First; pRoom != 0; )
            {
                var room = game.Debugger.Read<Room2>(pRoom);
                var roomsNear = game.Debugger.ReadArray<uint>(room.pRoom2Near, (int)room.dwRoomsNear);
                foreach (var pr in roomsNear)
                {
                    var r = game.Debugger.Read<Room2>(pr);
                    var lvl = game.Debugger.Read<Level>(r.pLevel);
                    if (lvl.dwLevelNo != level.dwLevelNo)
                    {
                        int nRoomX = (int)room.dwPosX * 5;
                        int nRoomY = (int)room.dwPosY * 5;

                        foreach (var ptCenter in ptCenters)
                        {
                            if ((ptCenter.X + cmap.LevelOrigin.X) >= (short)nRoomX && (ptCenter.X + cmap.LevelOrigin.X) <= (short)(nRoomX + (room.dwSizeX * 5)))
                            {
                                if ((ptCenter.Y + cmap.LevelOrigin.Y) >= (short)nRoomY && (ptCenter.Y + cmap.LevelOrigin.Y) <= (short)(nRoomY + (room.dwSizeY * 5)))
                                {
                                    var exit = new LevelExit
                                    {
                                        dwTargetLevel = lvl.dwLevelNo,
                                        ptPos = new Point(ptCenter.X + cmap.LevelOrigin.X, ptCenter.Y + cmap.LevelOrigin.Y),
                                        dwType = (uint)ExitType.Level,
                                        dwId = 0,
                                        pRoom2 = 0,
                                    };

                                    if (!levelExits.ContainsKey(level.dwLevelNo))
                                        levelExits.Add(level.dwLevelNo, new List<LevelExit>());

                                    if (levelExits[level.dwLevelNo].Find(it => it.dwTargetLevel == lvl.dwLevelNo) == null)
                                        levelExits[level.dwLevelNo].Add(exit);
                                }
                            }
                        }

                        break;
                    }
                }

                pRoom = room.pRoom2Next;
            }
        }

        protected void InitLayer(uint levelNo)
        {
            var pLayer = game.Debugger.Call(D2Common.GetLayer,
                CallingConventionEx.FastCall,
                levelNo);
            if (pLayer == 0)
                return;

            var layer = game.Debugger.Read<AutomapLayer2>(pLayer);

            game.Debugger.Call(D2Client.InitAutomapLayer_I,
                CallingConventionEx.Register,
                layer.nLayerNo);
        }

        public uint GetUnitByXY(uint x, uint y, uint pRoom2)
        {
            if (pRoom2 == 0)
                return 0;

            var room2 = game.Debugger.Read<Room2>(pRoom2);
            if (room2.pRoom1 == 0)
                return 0;

            var room1 = game.Debugger.Read<Room1>(room2.pRoom1);
            if (room1.pUnitFirst == 0)
                return 0;

            var pUnit = room1.pUnitFirst;

            while (pUnit != 0)
            {
                var unit = game.Debugger.Read<UnitAny>(pUnit);
                if (unit.dwType != (uint)UnitType.Player && unit.pObjectPath != 0)
                {
                    var path = game.Debugger.Read<ObjectPath>(unit.pObjectPath);
                    if (path.dwPosX == x && path.dwPosY == y)
                        return unit.dwUnitId;
                }

                pUnit = unit.pListNext;
            }

            return 0;
        }

        public CollisionMap GetCollisionData(params uint[] dwLevelIds)
        {
            CollisionMap result = null;
            foreach(var dwLevelId in dwLevelIds)
            {
                if (!levelCollisions.ContainsKey(dwLevelId))
                    game.MapHandler.RevealLevel(dwLevelId);

                if (result == null)
                    result = levelCollisions[dwLevelId];
                else
                    result = result.Merge(levelCollisions[dwLevelId]);
            }

            return result;
        }

        public List<LevelExit> GetLevelExits(uint dwLevelId)
        {
            if (!levelExits.ContainsKey(dwLevelId))
                game.MapHandler.RevealLevel(dwLevelId);

            return levelExits[dwLevelId];
        }
    }
}
