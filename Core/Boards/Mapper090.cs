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
    [BoardInfo("Pirate MMC5-style", 90)]
    [NotImplementedWell("Mapper 90\nDipSwitch not implemented, the irq modes 2-3 not implemented yet.\nThe Super Mario World rom doesn't work while the Super Mario World [b1] works !!")]
    class Mapper090 : Board
    {
        protected bool MAPPER90MODE;// Setting this to true disables the extended nametables control.
        private int[] prg_reg;
        private int[] chr_reg;
        private int[] nt_reg;
        private int prg_mode;
        private int chr_mode;
        private bool chr_block_mode;
        private int chr_block;
        private bool chr_m;
        private bool flag_s;
        private int irqCounter = 0;
        private bool IrqEnable = false;
        private bool irqCountDownMode = false;
        private bool irqCountUpMode = false;
        private bool irqFunkyMode = false;
        private bool irqPrescalerSize = false;
        private int irqSource = 0;
        private int irqPrescaler = 0;
        private int irqPrescalerXOR = 0;
        private byte irqFunkyModeReg = 0;
        private byte Dipswitch;
        private byte multiplication_a;
        private byte multiplication_b;
        private ushort multiplication;
        private byte RAM5803;
        private bool nt_advanced_enable;
        private bool nt_rom_only;
        private int nt_ram_select;

        public override void HardReset()
        {
            base.HardReset();
            MAPPER90MODE = true;
            prg_reg = new int[4];
            chr_reg = new int[8];
            nt_reg = new int[4];
            prg_mode = chr_mode = 0;
            for (int i = 0; i < 4; i++)
            {
                prg_reg[i] = i;
                nt_reg[i] = i;
            }
            for (int i = 0; i < 8; i++)
                chr_reg[i] = i;
            SetupPRG();
            SetupCHR();
            Dipswitch = 0;// ??
            irqCounter = 0;
            IrqEnable = false;
            irqCountDownMode = false;
            irqCountUpMode = false;
            irqFunkyMode = false;
            irqPrescalerSize = false;
            irqSource = 0;
            irqPrescaler = 0;
            irqPrescalerXOR = 0;
            irqFunkyModeReg = 0;
            RAM5803 = 0;
            flag_s = false;
            multiplication_a = 0;
            multiplication_b = 0;
            multiplication = 0;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            /*Write at $C002 : $00
Write at $8002 : $3E
Write at $8003 : $3F
Write at $C001 : $85
Write at $D000 : $3E
Write at $B000 : $00
Write at $B001 : $01
Write at $B002 : $02
Write at $B003 : $03
Write at $C006 : $00
Write at $D002 : $00
Write at $D003 : $00
Write at $D001 : $01
Write at $C002 : $40*/
            switch (address & 0xF007)
            {
                case 0x8000:
                case 0x8001:
                case 0x8002:
                case 0x8003:
                case 0x8004:
                case 0x8005:
                case 0x8006:
                case 0x8007: prg_reg[address & 0x3] = data & 0x7F; SetupPRG(); break;
                case 0x9000:
                case 0x9001:
                case 0x9002:
                case 0x9003:
                case 0x9004:
                case 0x9005:
                case 0x9006:
                case 0x9007: chr_reg[address & 0x7] = (chr_reg[address & 0x7] & 0xFF00) | data; SetupCHR(); break;
                case 0xA000:
                case 0xA001:
                case 0xA002:
                case 0xA003:
                case 0xA004:
                case 0xA005:
                case 0xA006:
                case 0xA007: chr_reg[address & 0x7] = (chr_reg[address & 0x7] & 0x00FF) | (data << 8); SetupCHR(); break;
                case 0xB000:
                case 0xB001:
                case 0xB002:
                case 0xB003: nt_reg[address & 0x3] = (nt_reg[address & 0x3] & 0xFF00) | data; break;
                case 0xB004:
                case 0xB005:
                case 0xB006:
                case 0xB007: nt_reg[address & 0x3] = (nt_reg[address & 0x3] & 0x00FF) | (data << 8); break;
                case 0xC000:
                    {
                        IrqEnable = (data & 1) == 1;
                        if (!IrqEnable)
                            NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xC001:
                    {
                        irqCountDownMode = (data & 0x80) == 0x80;
                        irqCountUpMode = (data & 0x40) == 0x40;
                        irqFunkyMode = (data & 0x8) == 0x8;
                        irqPrescalerSize = (data & 0x4) == 0x4;
                        irqSource = data & 3;
                        break;
                    }
                case 0xC002: IrqEnable = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0xC003: IrqEnable = true; break;
                case 0xC004: irqPrescaler = data ^ irqPrescalerXOR; break;
                case 0xC005: irqCounter = data ^ irqPrescalerXOR; break;
                case 0xC006: irqPrescalerXOR = data; break;
                case 0xC007: irqFunkyModeReg = data; break;
                case 0xD000:
                    {
                        flag_s = (data & 0x80) == 0x80;
                        prg_mode = data & 0x7;
                        chr_mode = (data >> 3) & 0x3;
                        nt_advanced_enable = (data & 0x20) == 0x20;
                        nt_rom_only = (data & 0x40) == 0x40;
                        SetupPRG();
                        SetupCHR();
                        break;
                    }
                case 0xD001:
                    {
                        switch (data & 0x3)
                        {
                            case 0: SwitchNMT(Mirroring.Vert); break;
                            case 1: SwitchNMT(Mirroring.Horz); break;
                            case 2: SwitchNMT(Mirroring.OneScA); break;
                            case 3: SwitchNMT(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xD002:
                    {
                        nt_ram_select = (data & 0x80);
                        break;
                    }
                case 0xD003:
                    {
                        chr_m = (data & 0x80) == 0x80;
                        chr_block_mode = (data & 0x20) == 0x20;
                        chr_block = (data & 0x1F) << 8;
                        SetupCHR();
                        break;
                    }
            }
        }
        public override void WriteSRM(ref int address, ref byte data)
        {
            // No SRAM !
        }
        public override byte ReadSRM(ref int address)
        {
            if (flag_s)
                return prg_banks[prg_indexes[0]][address & 0x1FFF];

            return 0;
        }
        public override byte ReadEXP(ref int address)
        {
            switch (address & 0xF007)
            {
                case 0x5000: return Dipswitch;
                case 0x5800: return (byte)(multiplication & 0x00FF);
                case 0x5801: return (byte)((multiplication & 0xFF00) >> 8);
                case 0x5803: return RAM5803;
            }
            return 0;
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            switch (address & 0xF007)
            {
                case 0x5800: multiplication_a = data; multiplication = (ushort)(multiplication_a * multiplication_b); break;
                case 0x5801: multiplication_b = data; multiplication = (ushort)(multiplication_a * multiplication_b); break;
                case 0x5803: RAM5803 = data; break;
            }
        }
        public override byte ReadNMT(ref int address)
        {
            if (MAPPER90MODE)
                return nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF];
            if (!nt_advanced_enable)
                return nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF];
            else
            {
                if (nt_rom_only)
                    return chr_banks[nt_reg[(address >> 10) & 3] + chr_rom_bank_offset][address & 0x3FF];
                else
                {
                    if ((nt_reg[(address >> 10) & 3] & 0x80) != nt_ram_select)
                        return chr_banks[nt_reg[(address >> 10) & 3] + chr_rom_bank_offset][address & 0x3FF];
                    else
                        return nmt_banks[nt_reg[(address >> 10) & 3] & 1][address & 0x3FF];
                }
            }
        }
        public override void WriteNMT(ref int address, ref byte data)
        {
            if (MAPPER90MODE)
            {
                nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF] = data;
                return;
            }
            if (!nt_advanced_enable)
                nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF] = data;
            else
            {
                if (nt_rom_only)
                {
                    // Do nothing ?
                }
                else
                {
                    if ((nt_reg[(address >> 10) & 3] & 0x80) == nt_ram_select)
                    {
                        nmt_banks[nt_reg[(address >> 10) & 3] & 1][address & 0x3FF] = data;
                    }
                }
            }
        }
        private void SetupPRG()
        {
            switch (prg_mode)
            {
                case 0x0:
                    {
                        Switch08KPRG((prg_reg[3] * 4) + 3, 0x6000, true);
                        Switch32KPRG(prg_32K_rom_mask, true);
                        break;
                    }
                case 0x1:
                    {
                        Switch08KPRG((prg_reg[3] * 2) + 1, 0x6000, true);
                        Switch16KPRG(prg_reg[1], 0x8000, true);
                        Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
                        break;
                    }
                case 0x2:
                    {
                        Switch08KPRG(prg_reg[3], 0x6000, true);
                        Switch08KPRG(prg_reg[0], 0x8000, true);
                        Switch08KPRG(prg_reg[1], 0xA000, true);
                        Switch08KPRG(prg_reg[2], 0xC000, true);
                        Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
                        break;
                    }
                case 0x3:
                    {
                        Switch08KPRG(ReverseByte(prg_reg[3]), 0x6000, true);
                        Switch08KPRG(ReverseByte(prg_reg[0]), 0x8000, true);
                        Switch08KPRG(ReverseByte(prg_reg[1]), 0xA000, true);
                        Switch08KPRG(ReverseByte(prg_reg[2]), 0xC000, true);
                        Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
                        break;
                    }
                case 0x4:
                    {
                        Switch08KPRG((prg_reg[3] * 4) + 3, 0x6000, true);
                        Switch32KPRG(prg_reg[3], true);
                        break;
                    }
                case 0x5:
                    {
                        Switch08KPRG((prg_reg[3] * 2) + 1, 0x6000, true);
                        Switch16KPRG(prg_reg[1], 0x8000, true);
                        Switch16KPRG(prg_reg[3], 0xC000, true);
                        break;
                    }
                case 0x6:
                    {
                        Switch08KPRG(prg_reg[3], 0x6000, true);
                        Switch08KPRG(prg_reg[0], 0x8000, true);
                        Switch08KPRG(prg_reg[1], 0xA000, true);
                        Switch08KPRG(prg_reg[2], 0xC000, true);
                        Switch08KPRG(prg_reg[3], 0xE000, true);
                        break;
                    }
                case 0x7:
                    {
                        Switch08KPRG(ReverseByte(prg_reg[3]), 0x6000, true);
                        Switch08KPRG(ReverseByte(prg_reg[0]), 0x8000, true);
                        Switch08KPRG(ReverseByte(prg_reg[1]), 0xA000, true);
                        Switch08KPRG(ReverseByte(prg_reg[2]), 0xC000, true);
                        Switch08KPRG(ReverseByte(prg_reg[3]), 0xE000, true);
                        break;
                    }
            }
        }
        private void SetupCHR()
        {
            switch (chr_mode)
            {
                case 0x0:
                    {
                        if (chr_block_mode)
                            Switch08KCHR(chr_reg[0], chr_01K_rom_count > 0);
                        else
                            Switch08KCHR((chr_reg[0] & 0x00FF) | chr_block, chr_01K_rom_count > 0);
                        break;
                    }
                case 0x1:
                    {
                        if (chr_block_mode)
                        {
                            Switch04KCHR(chr_reg[0], 0x0000, chr_01K_rom_count > 0);
                            Switch04KCHR(chr_reg[4], 0x1000, chr_01K_rom_count > 0);
                        }
                        else
                        {
                            Switch04KCHR((chr_reg[0] & 0x00FF) | chr_block, 0x0000, chr_01K_rom_count > 0);
                            Switch04KCHR((chr_reg[4] & 0x00FF) | chr_block, 0x1000, chr_01K_rom_count > 0);
                        }
                        break;
                    }
                case 0x2:
                    {
                        if (chr_block_mode)
                        {
                            Switch02KCHR(chr_reg[0], 0x0000, chr_01K_rom_count > 0);
                            Switch02KCHR(chr_m ? chr_reg[0] : chr_reg[2], 0x0800, chr_01K_rom_count > 0);
                            Switch02KCHR(chr_reg[4], 0x1000, chr_01K_rom_count > 0);
                            Switch02KCHR(chr_reg[6], 0x1800, chr_01K_rom_count > 0);
                        }
                        else
                        {
                            Switch02KCHR((chr_reg[0] & 0x00FF) | chr_block, 0x0000, chr_01K_rom_count > 0);
                            Switch02KCHR(((chr_m ? chr_reg[0] : chr_reg[2]) & 0x00FF) | chr_block, 0x0800, chr_01K_rom_count > 0);
                            Switch02KCHR((chr_reg[4] & 0x00FF) | chr_block, 0x1000, chr_01K_rom_count > 0);
                            Switch02KCHR((chr_reg[6] & 0x00FF) | chr_block, 0x1800, chr_01K_rom_count > 0);
                        }
                        break;
                    }
                case 0x3:
                    {
                        if (chr_block_mode)
                        {
                            Switch01KCHR(chr_reg[0], 0x0000, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_reg[1], 0x0400, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_m ? chr_reg[0] : chr_reg[2], 0x0800, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_m ? chr_reg[1] : chr_reg[3], 0x0C00, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_reg[4], 0x1000, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_reg[5], 0x1400, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_reg[6], 0x1800, chr_01K_rom_count > 0);
                            Switch01KCHR(chr_reg[7], 0x1C00, chr_01K_rom_count > 0);
                        }
                        else
                        {
                            Switch01KCHR((chr_reg[0] & 0x00FF) | chr_block, 0x0000, chr_01K_rom_count > 0);
                            Switch01KCHR((chr_reg[1] & 0x00FF) | chr_block, 0x0400, chr_01K_rom_count > 0);
                            Switch01KCHR(((chr_m ? chr_reg[0] : chr_reg[2]) & 0x00FF) | chr_block, 0x0800, chr_01K_rom_count > 0);
                            Switch01KCHR(((chr_m ? chr_reg[1] : chr_reg[3]) & 0x00FF) | chr_block, 0x0C00, chr_01K_rom_count > 0);
                            Switch01KCHR((chr_reg[4] & 0x00FF) | chr_block, 0x1000, chr_01K_rom_count > 0);
                            Switch01KCHR((chr_reg[5] & 0x00FF) | chr_block, 0x1400, chr_01K_rom_count > 0);
                            Switch01KCHR((chr_reg[6] & 0x00FF) | chr_block, 0x1800, chr_01K_rom_count > 0);
                            Switch01KCHR((chr_reg[7] & 0x00FF) | chr_block, 0x1C00, chr_01K_rom_count > 0);
                        }
                        break;
                    }
            }
        }
        private int ReverseByte(int value)
        {
            return ((value & 0x40) >> 6) | ((value & 0x20) >> 4) | ((value & 0x10) >> 2)
                | ((value & 0x8)) | ((value & 0x4) << 2) | ((value & 0x2) << 4) | ((value & 0x1) << 6);
        }
        public override void OnCPUClock()
        {
            if (irqSource == 0)
            {
                if (irqPrescalerSize)//3-bits
                {
                    irqPrescaler = (irqPrescaler & 0xF8) | (((irqPrescaler & 0x7) + 1) & 0x7);
                    if ((irqPrescaler & 0x7) == 0x7)
                    {
                        ClockIRQCounter();
                    }
                }
                else//8-bits
                {
                    irqPrescaler = (byte)(irqPrescaler + 1);
                    if (irqPrescaler == 0xFF)
                    {
                        ClockIRQCounter();
                    }
                }
            }
        }
        public override void OnPPUAddressUpdate(ref int address)
        {
            if (irqSource == 1)
            {
                old_vram_address = new_vram_address;
                new_vram_address = address & 0x1000;
                if (old_vram_address < new_vram_address)
                {
                    if (irqPrescalerSize)//3-bits
                    {
                        irqPrescaler = (irqPrescaler & 0xF8) | (((irqPrescaler & 0x7) + 1) & 0x7);
                        if ((irqPrescaler & 0x7) == 0x7)
                        {
                            ClockIRQCounter();
                        }
                    }
                    else//8-bits
                    {
                        irqPrescaler = (byte)(irqPrescaler + 1);
                        if (irqPrescaler == 0xFF)
                        {
                            ClockIRQCounter();
                        }
                    }
                }
            }
        }
        private void ClockIRQCounter()
        {
            if (irqCountDownMode && irqCountUpMode)
                return;
            if (irqCountDownMode)
            {
                irqCounter--;
                if (irqCounter == 0)
                {
                    irqCounter = 0xFF;
                    if (IrqEnable)
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
            else if (irqCountUpMode)
            {
                irqCounter = (byte)(irqCounter + 1);
                if (irqCounter == 0xFF)
                {
                    irqCounter = 0;
                    if (IrqEnable)
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            for (int i = 0; i < prg_reg.Length; i++)
                stream.Write(prg_reg[i]);
            for (int i = 0; i < chr_reg.Length; i++)
                stream.Write(chr_reg[i]);
            for (int i = 0; i < nt_reg.Length; i++)
                stream.Write(nt_reg[i]);
            stream.Write(prg_mode);
            stream.Write(chr_mode);
            stream.Write(chr_block_mode);
            stream.Write(chr_block);
            stream.Write(chr_m);
            stream.Write(flag_s);
            stream.Write(irqCounter);
            stream.Write(IrqEnable);
            stream.Write(irqCountDownMode);
            stream.Write(irqCountUpMode);
            stream.Write(irqFunkyMode);
            stream.Write(irqPrescalerSize);
            stream.Write(irqSource);
            stream.Write(irqPrescaler);
            stream.Write(irqPrescalerXOR);
            stream.Write(irqFunkyModeReg);
            stream.Write(Dipswitch);
            stream.Write(multiplication_a);
            stream.Write(multiplication_b);
            stream.Write(multiplication);
            stream.Write(RAM5803);
            stream.Write(nt_advanced_enable);
            stream.Write(nt_rom_only);
            stream.Write(nt_ram_select);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            for (int i = 0; i < prg_reg.Length; i++)
                prg_reg[i] = stream.ReadInt32();
            for (int i = 0; i < chr_reg.Length; i++)
                chr_reg[i] = stream.ReadInt32();
            for (int i = 0; i < nt_reg.Length; i++)
                nt_reg[i] = stream.ReadInt32();
            prg_mode = stream.ReadInt32();
            chr_mode = stream.ReadInt32();
            chr_block_mode = stream.ReadBoolean();
            chr_block = stream.ReadInt32();
            chr_m = stream.ReadBoolean();
            flag_s = stream.ReadBoolean();
            irqCounter = stream.ReadInt32();
            IrqEnable = stream.ReadBoolean();
            irqCountDownMode = stream.ReadBoolean();
            irqCountUpMode = stream.ReadBoolean();
            irqFunkyMode = stream.ReadBoolean();
            irqPrescalerSize = stream.ReadBoolean();
            irqSource = stream.ReadInt32();
            irqPrescaler = stream.ReadInt32();
            irqPrescalerXOR = stream.ReadInt32();
            irqFunkyModeReg = stream.ReadByte();
            Dipswitch = stream.ReadByte();
            multiplication_a = stream.ReadByte();
            multiplication_b = stream.ReadByte();
            multiplication = stream.ReadUInt16();
            RAM5803 = stream.ReadByte();
            nt_advanced_enable = stream.ReadBoolean();
            nt_rom_only = stream.ReadBoolean();
            nt_ram_select = stream.ReadInt32();
        }
    }
}
