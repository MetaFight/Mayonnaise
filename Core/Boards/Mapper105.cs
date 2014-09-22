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
    [BoardInfo("NES-EVENT", 105)]
    [NotImplementedWell("Mapper 105\nGame hangs on title screen !")]
    class Mapper105 : Board
    {
        private int DipSwitchNumber;
        private byte[] reg = new byte[4];
        private byte shift = 0;
        private byte buffer = 0;
        private bool flag_p;
        private bool flag_s;
        private bool flag_o;
        private int reg_a;
        private int reg_b;
        private bool irq_control;
        private bool initialized;
        private int irq_counter;
        private int dipswitches;

        public override void HardReset()
        {
            base.HardReset();
            base.TogglePRGRAMEnable(true);
            // Registers
            reg = new byte[4];
            reg[0] = 0x0C;
            flag_s = flag_p = true;
            reg[1] = reg[2] = reg[3] = 0;
            // Buffers
            buffer = 0;
            shift = 0;
            initialized = false;
            DipSwitchNumber = 0;
            dipswitches = 0x20000000 | (DipSwitchNumber << 22);
        }
        public override void SoftReset()
        {
            DipSwitchNumber = (DipSwitchNumber + 1) & 0xF;
            dipswitches = 0x20000000 | (DipSwitchNumber << 22);
        }
        public override void WritePRG(ref int address, ref byte value)
        {
            //Temporary reg port ($8000-FFFF):
            //[r... ...d]
            //r = reset flag
            //d = data bit

            //r is set
            if ((value & 0x80) == 0x80)
            {
                reg[0] |= 0x0C;//bits 2,3 of reg $8000 are set (16k PRG mode, $8000 swappable)
                flag_s = flag_p = true;
                shift = buffer = 0;//hidden temporary reg is reset
                return;
            }
            //d is set
            if ((value & 0x01) == 0x01)
                buffer |= (byte)(1 << shift);//'d' proceeds as the next bit written in the 5-bit sequence
            if (++shift < 5)
                return;
            // If this completes the 5-bit sequence:
            // - temporary reg is copied to actual internal reg (which reg depends on the last address written to)
            address = (ushort)((address & 0x7FFF) >> 13);
            reg[address] = buffer;

            // - temporary reg is reset (so that next write is the "first" write)
            shift = buffer = 0;

            // Update internal registers ...
            switch (address)
            {
                case 0:// $8000-9FFF
                    {
                        // Flags
                        flag_p = (reg[0] & 0x08) != 0;
                        flag_s = (reg[0] & 0x04) != 0;
                        UpdatePRG();
                        // Mirroring
                        switch (reg[0] & 3)
                        {
                            case 0: base.SwitchNMT(Mirroring.OneScA); break;
                            case 1: base.SwitchNMT(Mirroring.OneScB); break;
                            case 2: base.SwitchNMT(Mirroring.Vert); break;
                            case 3: base.SwitchNMT(Mirroring.Horz); break;
                        }
                        break;
                    }
                case 1:// $A000-BFFF 
                    {
                        irq_control = (reg[1] & 0x10) == 0x10;
                        if (irq_control)
                        {
                            initialized = true;
                            irq_counter = 0;
                            NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        }
                        else
                            Switch32KPRG(0, true);

                        flag_o = (reg[1] & 0x8) == 0x8;
                        reg_a = (reg[1] >> 1) & 0x3;
                        UpdatePRG();
                        break;
                    }
                case 3:// $E000-FFFF 
                    {
                        base.TogglePRGRAMEnable((reg[3] & 0x10) == 0);
                        reg_b = reg[3] & 0xF;
                        UpdatePRG();
                        break;
                    }
            }
        }
        private void UpdatePRG()
        {
            if (!initialized)
                return;

            if (!flag_o)
            {
                Switch32KPRG(reg_a, true);
            }
            else
            {
                if (!flag_p)
                {
                    Switch32KPRG((reg_b >> 1) + 4, true);
                }
                else
                {
                    if (!flag_s)
                    {
                        Switch16KPRG(8, 0x8000, true);
                        Switch16KPRG(reg_b + 8, 0xC000, true);
                    }
                    else
                    {
                        Switch16KPRG(reg_b + 8, 0x8000, true);
                        Switch16KPRG(15, 0xC000, true);
                    }
                }
            }
        }
        public override void OnCPUClock()
        {
            if (!irq_control)
            {
                irq_counter++;
                if (irq_counter == dipswitches)
                {
                    irq_counter = 0;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(DipSwitchNumber);
            stream.Write(reg);
            stream.Write(shift);
            stream.Write(buffer);
            stream.Write(flag_p);
            stream.Write(flag_s);
            stream.Write(flag_o);
            stream.Write(reg_a);
            stream.Write(reg_b);
            stream.Write(irq_control);
            stream.Write(initialized);
            stream.Write(irq_counter);
            stream.Write(dipswitches);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            DipSwitchNumber = stream.ReadInt32();
            stream.Read(reg, 0, reg.Length);
            shift = stream.ReadByte();
            buffer = stream.ReadByte();
            flag_p = stream.ReadBoolean();
            flag_s = stream.ReadBoolean();
            flag_o = stream.ReadBoolean();
            reg_a = stream.ReadInt32();
            reg_b = stream.ReadInt32();
            irq_control = stream.ReadBoolean();
            initialized = stream.ReadBoolean();
            irq_counter = stream.ReadInt32();
            dipswitches = stream.ReadInt32();
        }
    }
}
