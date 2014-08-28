using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public partial class D2Game
    {
        protected int[] m_ActLevels = new int[]
        {
            1, 40, 75, 103, 109, 137
        };

        public void RevealAct()
        {
            UnitAny unit;
            if (!GameReady() || !GetPlayerUnit(out unit))
            {
                Log("Failed to reveal act");
                return;
            }

            if (revealedActs.Contains(unit.dwAct))
            {
                Log("Act {0} is already revealed", unit.dwAct + 1);
                return;
            }

            if (unit.pAct == 0)
            {
                Log("Failed to reveal act {0}", unit.dwAct + 1);
                return;
            }

            var act = pd.Read<Act>(unit.pAct);
            var expCharFlag = pd.ReadUInt(D2Client.pExpCharFlag);
            var diff = pd.ReadByte(D2Client.pDifficulty);

            var pAct = pd.Call(D2Common.LoadAct,
                CallingConventionEx.StdCall,
                unit.dwAct,
                act.dwMapSeed,
                expCharFlag,
                0,
                diff,
                0,
                (uint)m_ActLevels[unit.dwAct],
                pd.GetAddress(D2Client.LoadAct_1),
                pd.GetAddress(D2Client.LoadAct_2));

            if (pAct == 0)
            {
                Log("Failed to reveal act");
                return;
            }

            act = pd.Read<Act>(pAct);
            if (act.pMisc == 0)
            {
                Log("Failed to reveal act");
                return;
            }

            var actMisc = pd.Read<ActMisc>(act.pMisc);
            if (actMisc.pLevelFirst == 0)
            {
                Log("Failed to reveal act");
                return;
            }

            for (var i = m_ActLevels[unit.dwAct]; i < m_ActLevels[unit.dwAct + 1]; ++i)
            {
                Level lvl = pd.Read<Level>(actMisc.pLevelFirst);
                uint pLevel = 0;
                for (pLevel = actMisc.pLevelFirst; pLevel != 0; pLevel = lvl.pNextLevel)
                {
                    if (pLevel != actMisc.pLevelFirst)
                        lvl = pd.Read<Level>(pLevel);

                    if (lvl.dwLevelNo == (uint)i && lvl.dwPosX > 0)
                        break;
                }

                if (pLevel == 0)
                    pLevel = pd.Call(D2Common.GetLevel,
                        CallingConventionEx.FastCall,
                        act.pMisc, (uint)i);
                if (pLevel == 0)
                    continue;

                lvl = pd.Read<Level>(pLevel);
                if (lvl.pRoom2First == 0)
                    pd.Call(D2Common.InitLevel,
                        CallingConventionEx.StdCall,
                        pLevel);

                if (lvl.dwLevelNo > 255)
                    continue;

                InitLayer(lvl.dwLevelNo);
                lvl = pd.Read<Level>(pLevel);

                for (var pRoom = lvl.pRoom2First; pRoom != 0; )
                {
                    var room = pd.Read<Room2>(pRoom);

                    var actMisc2 = pd.Read<ActMisc>(lvl.pMisc);
                    var roomData = false;
                    if (room.pRoom1 == 0)
                    {
                        roomData = true;
                        pd.Call(D2Common.AddRoomData,
                            CallingConventionEx.ThisCall,
                            0, actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);
                    }

                    room = pd.Read<Room2>(pRoom);
                    if (room.pRoom1 == 0)
                        continue;

                    var pAutomapLayer = pd.ReadUInt(D2Client.pAutoMapLayer);

                    pd.Call(D2Client.RevealAutomapRoom,
                        CallingConventionEx.StdCall,
                        room.pRoom1,
                        1,
                        pAutomapLayer);

                    DrawPresets(room, lvl);

                    if (roomData)
                        pd.Call(D2Common.RemoveRoomData,
                            CallingConventionEx.StdCall,
                            actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);

                    pRoom = room.pRoom2Next;
                }
            }

            var path = pd.Read<Path>(unit.pPath);
            var room1 = pd.Read<Room1>(path.pRoom1);
            var room2 = pd.Read<Room2>(room1.pRoom2);
            var lev = pd.Read<Level>(room2.pLevel);
            InitLayer(lev.dwLevelNo);
            pd.Call(D2Common.UnloadAct,
                CallingConventionEx.StdCall,
                pAct);

            //PrintGameString("Revealed act", D2Color.Red);

            revealedActs.Add(unit.dwAct);

            Log("Revealed act {0}", unit.dwAct + 1);
        }

        public void DrawPresets(Room2 room, Level lvl)
        {
            for (var pPreset = room.pPreset; pPreset != 0; )
            {
                var preset = pd.Read<PresetUnit>(pPreset);

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
                        var pTxt = pd.Call(D2Common.GetObjectTxt,
                            CallingConventionEx.StdCall,
                            preset.dwTxtFileNo);
                        if (pTxt != 0)
                        {
                            var txt = pd.Read<ObjectTxt>(pTxt);
                            cellNo = (int)txt.nAutoMap;
                        }
                    }
                }

                if (cellNo > 0/* && cellNo < 1258*/)
                {
                    var pCell = pd.Call(D2Client.NewAutomapCell,
                        CallingConventionEx.FastCall);

                    var cell = pd.Read<AutomapCell>(pCell);

                    var x = preset.dwPosX + room.dwPosX * 5;
                    var y = preset.dwPosY + room.dwPosY * 5;

                    cell.nCellNo = (ushort)cellNo;
                    cell.xPixel = (ushort)(((short)x - (short)y) * 1.6 + 1);
                    cell.yPixel = (ushort)((y + x) * 0.8 - 3);

                    pd.Write<AutomapCell>(pCell, cell);

                    var pAutomapLayer = pd.ReadUInt(D2Client.pAutoMapLayer);
                    pd.Call(D2Client.AddAutomapCell,
                        CallingConventionEx.FastCall,
                        pCell,
                        pAutomapLayer + 0x10);  // &((*p_D2CLIENT_AutomapLayer)->pObjects)
                }

                pPreset = preset.pPresetNext;
            }
        }

        public void InitLayer(uint levelNo)
        {
            var pLayer = pd.Call(D2Common.GetLayer,
                CallingConventionEx.FastCall,
                levelNo);
            if (pLayer == 0)
                return;

            var layer = pd.Read<AutomapLayer2>(pLayer);

            pd.Call(D2Client.InitAutomapLayer_I,
                CallingConventionEx.Register,
                layer.nLayerNo);
        }
    }
}
