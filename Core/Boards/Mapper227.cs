/* This file is part of My Nes
 * 
 * A Nintendo Entertainment System / Family Computer (Nes/Famicom) 
 * Emulator written in C#.
 *
 * Copyright © Ala Ibrahim Hadid 2009 - 2014
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace MyNes.Core
{
    [BoardInfo("Unknown", 227)]
    class Mapper227 : Board
    {
        private bool flag_o;
        private bool flag_s;
        private bool flag_l;
        private int prg_reg;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(0, 0xC000, true);
            flag_o = false;
            flag_s = false;
            flag_l = false;
            prg_reg = 0;
            ToggleCHRRAMWritableEnable(true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            flag_s = (address & 0x1) == 0x1;
            flag_o = (address & 0x80) == 0x80;
            flag_l = (address & 0x200) == 0x200;
            prg_reg = ((address >> 2) & 0x1F) | ((address >> 3) & 0x20);
            SwitchNMT((address & 0x2) == 0x2 ? Mirroring.Horz : Mirroring.Vert);
            ToggleCHRRAMWritableEnable(!flag_o);
            if (flag_o)
            {
                if (!flag_s)
                {
                    Switch16KPRG(prg_reg, 0x8000, true);
                    Switch16KPRG(prg_reg, 0xC000, true);
                }
                else
                {
                    Switch32KPRG(prg_reg >> 1, true);
                }
            }
            else
            {
                if (!flag_l)
                {
                    if (!flag_s)
                    {
                        Switch16KPRG(prg_reg, 0x8000, true);
                        Switch16KPRG(prg_reg & 0x38, 0xC000, true);
                    }
                    else
                    {
                        Switch16KPRG(prg_reg & 0x3E, 0x8000, true);
                        Switch16KPRG(prg_reg & 0x38, 0xC000, true);
                    }
                }
                else
                {
                    if (!flag_s)
                    {
                        Switch16KPRG(prg_reg, 0x8000, true);
                        Switch16KPRG(prg_reg | 0x7, 0xC000, true);
                    }
                    else
                    {
                        Switch16KPRG(prg_reg & 0x3E, 0x8000, true);
                        Switch16KPRG(prg_reg | 0x7, 0xC000, true);
                    }
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(flag_o);
            stream.Write(flag_s);
            stream.Write(flag_l);
            stream.Write(prg_reg);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            flag_o = stream.ReadBoolean();
            flag_s = stream.ReadBoolean();
            flag_l = stream.ReadBoolean();
            prg_reg = stream.ReadInt32();
        }
    }
}
