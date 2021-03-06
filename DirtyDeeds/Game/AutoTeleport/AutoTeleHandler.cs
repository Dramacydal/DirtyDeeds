﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using DD.Game.Log;
using WhiteMagic;
using DD.Extensions;
using DD.Game.D2Pointers;
using DD.Game.D2Structs;
using DD.Game;
using DD.Game.Enums;

namespace DD.Game.AutoTeleport
{
    using PointList = List<Point>;

    public enum TeleType
    {
        Next = 0,
        Misc = 1,
        WP = 2,
        Prev = 3
    }

    public static class TeleTypeExtensions
    {
        public static string Name(this TeleType type)
        {
            switch (type)
            {
                case TeleType.Next:
                    return "Next Level";
                case TeleType.Misc:
                    return "Misc Location";
                case TeleType.WP:
                    return "Waypoint";
                case TeleType.Prev:
                    return "Previous Level";
                default:
                    break;
            }

            return "<Unknown>";
        }
    }

    public class AutoTeleHandler
    {
        protected static uint[] CS = new uint[] { 392, 394, 396, 255 };
        protected static uint[] waypoints = new uint[] { 119, 157, 156, 323, 288, 402, 324, 237, 238, 398, 496, 511, 494 };

        protected D2Game game;

        protected uint CSID = 0;
        protected bool doInteract = false;
        protected TeleportTargetType interactType = TeleportTargetType.None;
        protected uint interactRoom = 0;
        protected volatile PointList TPath = new PointList();
        protected uint lastArea = 0;
        protected uint interactId = 0;
        protected Thread th = null;

        public bool IsTeleporting { get { return TPath.Count != 0; } }

        public AutoTeleHandler(D2Game game)
        {
            this.game = game;
            th = new Thread(() => Loop());
            th.Start();
        }

        public void Reset()
        {
            TPath.Clear();
            doInteract = false;
            interactType = TeleportTargetType.None;
            interactRoom = 0;
            interactId = 0;
        }

        public void Terminate()
        {
            Reset();
            if (th != null)
            {
                th.Abort();
                th = null;
            }
        }

        public void ManageTele(TeleType type)
        {
            if (IsTeleporting)
                return;

            Level lvl;
            if (!game.GetPlayerLevel(out lvl))
                return;

            if (game.HasSkill(SkillType.Teleport))
            {
                Logger.AutoTele.Log(game, LogType.Warning, "You don't have teleport skill.");
                return;
            }

            var p = TeleportInfo.Vectors[lvl.dwLevelNo * 4 + (uint)type].Clone();

            var areas = new List<uint>();
            areas.Add(lvl.dwLevelNo);

            if (lvl.dwLevelNo == (uint)Map.A2_CANYON_OF_THE_MAGI)
            {
                if (p.Type == TeleportTargetType.None && p.Id == 0)
                {
                    var pMisc = lvl.pMisc;
                    if (pMisc != 0)
                    {
                        var misc = game.Debugger.Read<ActMisc>(pMisc);
                        if (misc.dwStaffTombLevel != 0)
                        {
                            p.Type = TeleportTargetType.Exit;
                            p.Id = misc.dwStaffTombLevel;
                        }
                    }
                }
            }
            else if (lvl.dwLevelNo == (uint)Map.A1_COLD_PLAINS)
            {
            }
            else if (lvl.dwLevelNo == (uint)Map.A4_THE_CHAOS_SANCTUARY && p.Id2 >= 1337 && p.Id2 <= 1341)
            {
                /*if (CSID == 3)
                    CSID = 0;
                else
                    ++CSID;

                p.Id = */
            }

            if (p.Id == 0)
            {
                Logger.AutoTele.Log(game, LogType.Warning, "There is no \"{0}\" path for current level.", type.Name());
                return;
            }

            if (p.Area != 0)
                areas.Add((byte)p.Area);

            doInteract = false;

            if (p.Type == TeleportTargetType.Exit)
            {
                var g_collisionMap = game.MapHandler.GetCollisionData(areas.ToArray());
                if (!g_collisionMap.m_map.IsCreated())
                    return;

                var ExitArray = game.MapHandler.GetLevelExits(areas[0]);
                if (ExitArray.Count == 0)
                    return;

                foreach (var exit in ExitArray)
                {
                    if (exit.dwTargetLevel == p.Id)
                    {
                        var pLevelTxt = game.Debugger.Call<IntPtr>(D2Common.GetLevelText,
                            MagicConvention.StdCall,
                            p.Id);
                        var lvltext = game.Debugger.Read<LevelTxt>(pLevelTxt);

                        doInteract = false;
                        if (exit.dwType == (uint)ExitType.Tile)
                        {
                            doInteract = true;
                            interactType = TeleportTargetType.Tile;
                            interactRoom = exit.pRoom2;
                        }

                        var nodes = MakePath(exit.ptPos.X, exit.ptPos.Y, areas, exit.dwType == (uint)ExitType.Level);
                        if (nodes != 0)
                            Logger.AutoTele.Log(game, LogType.None, "Going to {0}, {1} nodes.", lvltext.szName, nodes);
                        else
                            Logger.AutoTele.Log(game, LogType.Warning, "Failed to calculate path to {0}.", lvltext.szName);
                        break;
                    }
                }

                return;
            }

            if (p.Type == TeleportTargetType.XY)
            {
                doInteract = false;
                if (p.Id == 0 || p.Id2 == 0)
                {
                    Logger.AutoTele.Log(game, LogType.Warning, "Failed to make path to \"{0}\" point.", type.Name());
                    return;
                }

                var nodes = MakePath((int)p.Id, (int)p.Id2, areas, false);
                if (nodes != 0)
                    Logger.AutoTele.Log(game, LogType.None, "Going to \"{0}\" X: {1}, Y: {2}, {3} nodes.", type.Name(), p.Id, p.Id2, nodes);
                else
                    Logger.AutoTele.Log(game, LogType.Warning, "Failed to make path to \"{0}\" point.", type.Name());
                return;
            }

            var presetUnit = FindPresetLocation((uint)p.Type, p.Id, areas[areas.Count - 1]);
            if (!presetUnit.IsEmpty)
            {
                if (p.Type == TeleportTargetType.Tile || p.Type == TeleportTargetType.Object && p.Id == 298)
                    doInteract = true;
                else if (p.Type == TeleportTargetType.Object && waypoints.Contains(p.Id))
                    doInteract = true;

                var nodes = MakePath(presetUnit.X, presetUnit.Y, areas, false);
                if (nodes != 0)
                {
                    if (p.Type == TeleportTargetType.Object)
                    {
                        var pObjectTxt = game.Debugger.Call<IntPtr>(D2Common.GetObjectTxt, MagicConvention.StdCall, p.Id);
                        var txt = game.Debugger.Read<ObjectTxt>(pObjectTxt);
                        Logger.AutoTele.Log(game, LogType.None, "Going to {0}, {1} nodes.", txt.szName, nodes);
                    }
                    interactType = p.Type;
                }
                else
                    Logger.AutoTele.Log(game, LogType.Warning, "Failed to make path to \"{0}\" point.", type.Name());
            }
            else
            {
                Logger.AutoTele.Log(game, LogType.Warning, "Failed to make path to \"{0}\" point.", type.Name());
            }
        }

        protected Point FindPresetLocation(uint dwType, uint dwTxtFileNo, uint Area)
        {
            var pLevel = game.MapHandler.GetLevel(Area);
            if (pLevel == 0)
                return new Point();

            UnitAny unit;
            if (!game.GetPlayerUnit(out unit))
                return new Point();

            var path = game.Debugger.Read<Path>(unit.pPath);
            var lvl = game.Debugger.Read<Level>(pLevel);

            var loc = new Point();
            loc.X = 0;
            loc.Y = 0;
            doInteract = false;

            var bAddedRoom = false;
            var stoploop = false;
            for (var pRoom = lvl.pRoom2First; pRoom != 0; )
            {
                bAddedRoom = false;
                var room =  game.Debugger.Read<Room2>(pRoom);

                if (room.pPreset == 0 && room.pRoomTiles == 0 && room.pRoom1 == 0)
                {
                    game.Debugger.Call<int>(D2Common.AddRoomData,
                        MagicConvention.ThisCall,
                        0, unit.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);
                    bAddedRoom = true;
                }

                for (var pUnit = room.pPreset; pUnit != 0; )
                {
                    var preset = game.Debugger.Read<PresetUnit>(pUnit);
                    //if(pUnit->dwTxtFileNo != 40 && pUnit->dwTxtFileNo != 41 && pUnit->dwTxtFileNo != 42)
                    //	PrintText(4, "X: %d, Y: %d, TxtFileNo: %d, Type: %d", pUnit->dwPosX, pUnit->dwPosY, pUnit->dwTxtFileNo, pUnit->dwType);
                    if ((dwType == 0 || dwType == preset.dwType) && dwTxtFileNo == preset.dwTxtFileNo)
                    {
                        if (dwType == (uint)UnitType.Tile || dwType == (uint)UnitType.Object && dwTxtFileNo == 298)
                        {
                            interactRoom = pRoom;
                            interactType = (TeleportTargetType)dwType;
                            //DoInteract = 1;
                        }

                        if (dwType == (uint)UnitType.Object)
                        {
                            for (int i = 0; i < waypoints.Length; ++i)
                            {
                                if (waypoints[i] == dwTxtFileNo)
                                {
                                    interactRoom = pRoom;
                                    interactType = (TeleportTargetType)dwType;
                                    //DoInteract = 1;
                                    stoploop = true;//stop looping over the rooms
                                    break;
                                }
                            }
                        }

                        loc.X = (int)(preset.dwPosX + room.dwPosX * 5);
                        loc.Y = (int)(preset.dwPosY + room.dwPosY * 5);

                        stoploop = true;//stop looping over the rooms
                        break;
                    }

                    pUnit = preset.pPresetNext;
                }

                if (bAddedRoom)
                {
                    game.Debugger.Call(D2Common.RemoveRoomData,
                        MagicConvention.StdCall,
                        unit.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, path.pRoom1);
                }

                if (stoploop)
                    break;

                pRoom = room.pRoom2Next;
            }

            return loc;
        }

        protected int MakePath(int x, int y, List<uint> areas, bool MoveThrough)
        {
            uint dwCount = 0;
            var aPath = new Point[255];

            var g_collisionMap = game.MapHandler.GetCollisionData(areas.ToArray());
            if (!g_collisionMap.m_map.IsCreated())
                return 0;

            UnitAny unit;
            if (!game.GetPlayerUnit(out unit) || unit.pPath == 0)
                return 0;

            var path = game.Debugger.Read<Path>(unit.pPath);

            var ptStart = new Point(path.xPos, path.yPos);
            var ptEnd = new Point(x, y);

            if (!g_collisionMap.IsValidAbsLocation(ptStart.X, ptStart.Y))
                return 0;

            if (!g_collisionMap.IsValidAbsLocation(ptEnd.X, ptEnd.Y))
                return 0;

            g_collisionMap.AbsToRelative(ref ptStart);
            g_collisionMap.AbsToRelative(ref ptEnd);

            var matrix = new CMatrix<ushort>();
            if (!g_collisionMap.CopyMapData(matrix))
                return 0;

            var tf = new TeleportPath(matrix.GetData(), matrix.CX, matrix.CY);
            dwCount = tf.FindTeleportPath(ptStart, ptEnd, aPath, 255);
            if (dwCount == 0)
                return 0;

            for (var i = 0; i < dwCount; ++i)
                g_collisionMap.RelativeToAbs(ref aPath[i]);

            if (MoveThrough)
            {
                if (aPath[dwCount - 1].X > aPath[dwCount - 2].X)
                    aPath[dwCount].X = aPath[dwCount - 1].X + 2;
                else
                    aPath[dwCount].X = aPath[dwCount - 1].X - 2;
                if (aPath[dwCount - 1].Y > aPath[dwCount - 2].Y)
                    aPath[dwCount].Y = aPath[dwCount - 1].Y + 2;
                else
                    aPath[dwCount].Y = aPath[dwCount - 1].Y - 2;

                ++dwCount;

                if (TeleportPath.CalculateDistance(aPath[dwCount - 1].X, aPath[dwCount - 1].Y, aPath[dwCount - 3].X, aPath[dwCount - 3].Y) <= (uint)Range.TpRange)
                {
                    aPath[dwCount - 2] = aPath[dwCount - 1];
                    aPath[dwCount - 1].X = 0;
                    aPath[dwCount - 1].Y = 0;
                    --dwCount;
                }
            }

            TPath.Clear();
            for (var i = 0; i < dwCount; ++i)
                TPath.Add(aPath[i]);

            return (int)dwCount;
        }

        protected void Loop()
        {
            var _timer = new DateTime();
            var _InteractTimer = new DateTime();
            var castTele = false;
            var tryCount = 0;

            while (true)
            {
                Thread.Sleep(50);

                if (!game.InGame)
                    continue;

                try
                {
                    if (TPath.Count == 0)
                    {
                        if (doInteract && interactId != 0 && _InteractTimer.MSecToNow() >= 200)
                        {
                            using (var gameSuspender = new GameSuspender(game))
                            {
                                doInteract = false;
                                if (game.GameReady() && !game.IsDead())
                                    game.Interact(interactId, (UnitType)interactType);
                                interactId = 0;
                            }
                        }
                        continue;
                    }

                    var End = TPath[TPath.Count - 1];

                    // TODO: check skill for ping

                    if (castTele)
                    {
                        castTele = false;

                        _timer = DateTime.Now;
                        // TODO: check charges

                        using (var gameSuspender = new GameSuspender(game))
                        {
                            if (!game.GameReady() || game.IsDead() || game.IsInTown() || !game.TeleportTo((ushort)TPath[0].X, (ushort)TPath[0].Y))
                            {
                                TPath.Clear();
                                Logger.AutoTele.Log(game, LogType.Warning, "Failed to cast teleport.");
                                continue;
                            }
                        }
                    }

                    if (_timer.MSecToNow() > 500)
                    {
                        if (tryCount >= 5)
                        {
                            Logger.AutoTele.Log(game, LogType.Warning, "Failed to teleport after {0} tries.", tryCount);
                            TPath.Clear();
                            tryCount = 0;
                            doInteract = false;
                            castTele = false;
                            continue;
                        }
                        else
                        {
                            ++tryCount;
                            castTele = true;
                        }
                    }

                    if (TeleportPath.CalculateDistance(game.CurrentX, game.CurrentY, TPath[0].X, TPath[0].Y) <= 5)
                    {
                        TPath.RemoveAt(0);
                        castTele = true;
                        tryCount = 0;
                    }

                    if (doInteract)
                    {
                        if (TeleportPath.CalculateDistance(game.CurrentX, game.CurrentY, End.X, End.Y) <= 5)
                        {
                            doInteract = false;
                            _InteractTimer = DateTime.Now;
                            using (var gameSuspender = new GameSuspender(game))
                            {
                                if (!game.GameReady() || game.IsDead())
                                    interactId = 0;
                                else
                                    interactId = game.MapHandler.GetUnitByXY((uint)End.X, (uint)End.Y, interactRoom);
                            }
                            TPath.Clear();
                            continue;
                        }
                    }
                }
                catch
                {
                    ///
                }
            }
        }
    }
}
