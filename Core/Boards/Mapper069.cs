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
    [BoardInfo("FME-7/Sunsoft 5B", 69)]
    [WithExternalSound]// TODO: look at Sunsoft 5B sound channels.
    [NotImplementedWell("Mapper 69\nBad mixer for external sound channels.")]
    class Mapper069 : Board
    {
        private int address_A000;
        private int address_E000;
        private int irq_counter;
        private bool irq_count_enabled;
        private bool irq_trigger_enabled;
        private Sunsoft5BSoundChannel channel0;
        private Sunsoft5BSoundChannel channel1;
        private Sunsoft5BSoundChannel channel2;
        private double[][][] mix_table;

        private void InitializeSoundMixTable()
        {
            mix_table = new double[16][][];

            for (int sq1 = 0; sq1 < 16; sq1++)
            {
                mix_table[sq1] = new double[16][];

                for (int sq2 = 0; sq2 < 16; sq2++)
                {
                    mix_table[sq1][sq2] = new double[16];

                    for (int sq3 = 0; sq3 < 16; sq3++)
                    {
                        double sqr = (95.88 / (8128.0 / (sq1 + sq2 + sq3) + 100));
                        mix_table[sq1][sq2][sq3] = sqr * 128;
                    }
                }
            }
        }
        public override void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, MyNes.Core.Mirroring defaultMirroring)
        {
            base.Initialize(sha1, prg_dump, chr_dump, trainer_dump, defaultMirroring);
            InitializeSoundMixTable();
            channel0 = new Sunsoft5BSoundChannel();
            channel1 = new Sunsoft5BSoundChannel();
            channel2 = new Sunsoft5BSoundChannel();
        }
        public override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
            address_A000 = 0;
            irq_counter = 0xFFFF;
            irq_count_enabled = false;
            irq_trigger_enabled = false;

            channel0.HardReset();
            channel1.HardReset();
            channel2.HardReset();
        }
        public override void SoftReset()
        {
            base.SoftReset();
            channel0.SoftReset();
            channel1.SoftReset();
            channel2.SoftReset();
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xE000)
            {
                case 0x8000: address_A000 = data & 0xF; break;
                case 0xA000:
                    {
                        switch (address_A000)
                        {
                            case 0x0: Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                            case 0x1: Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                            case 0x2: Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                            case 0x3: Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                            case 0x4: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                            case 0x5: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                            case 0x6: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                            case 0x7: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                            case 0x8:
                                {
                                    TogglePRGRAMEnable((data & 0x80) == 0x80);
                                    Switch08KPRG(data & 0x3F, 0x6000, (data & 0x40) == 0);
                                    break;
                                }
                            case 0x9: Switch08KPRG(data, 0x8000, true); break;
                            case 0xA: Switch08KPRG(data, 0xA000, true); break;
                            case 0xB: Switch08KPRG(data, 0xC000, true); break;
                            case 0xC:
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
                            case 0xD:
                                {
                                    irq_count_enabled = (data & 0x80) == 0x80;
                                    irq_trigger_enabled = (data & 0x1) == 0x1;
                                    if (!irq_trigger_enabled)
                                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                                    break;
                                }
                            case 0xE: irq_counter = (irq_counter & 0xFF00) | data; break;
                            case 0xF: irq_counter = (irq_counter & 0x00FF) | (data << 8); break;
                        }
                        break;
                    }
                case 0xC000: address_E000 = data & 0xF; break;
                case 0xE000:
                    {
                        switch (address_E000)
                        {
                            case 0x0: channel0.Write0(ref data); break;
                            case 0x1: channel0.Write1(ref data); break;
                            case 0x2: channel1.Write0(ref data); break;
                            case 0x3: channel1.Write1(ref data); break;
                            case 0x4: channel2.Write0(ref data); break;
                            case 0x5: channel2.Write1(ref data); break;
                            case 0x7:
                                {
                                    channel0.Enabled = (data & 0x1) == 0;
                                    channel1.Enabled = (data & 0x2) == 0;
                                    channel2.Enabled = (data & 0x4) == 0;
                                    break;
                                }
                            case 0x8: channel0.Volume = (byte)(data & 0xF); break;
                            case 0x9: channel1.Volume = (byte)(data & 0xF); break;
                            case 0xA: channel2.Volume = (byte)(data & 0xF); break;
                        }
                        break;
                    }
            }
        }
        public override void OnCPUClock()
        {
            if (irq_count_enabled)
            {
                irq_counter--;
                if (irq_counter == 0)
                {
                    irq_counter = 0xFFFF;
                    if (irq_trigger_enabled)
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        public override void OnAPUClockSingle(ref bool isClockingLength)
        {
            base.OnAPUClockSingle(ref isClockingLength);
            channel0.ClockSingle();
            channel1.ClockSingle();
            channel2.ClockSingle();
        }
        public override double APUGetSamples()
        {
            return mix_table[channel0.GetSample()]
                            [channel1.GetSample()]
                            [channel2.GetSample()];
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(address_A000);
            stream.Write(address_E000);
            stream.Write(irq_counter);
            stream.Write(irq_count_enabled);
            stream.Write(irq_trigger_enabled);
            channel0.SaveState(stream);
            channel1.SaveState(stream);
            channel2.SaveState(stream);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            address_A000 = stream.ReadInt32();
            address_E000 = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            irq_count_enabled = stream.ReadBoolean();
            irq_trigger_enabled = stream.ReadBoolean();
            channel0.LoadState(stream);
            channel1.LoadState(stream);
            channel2.LoadState(stream);
        }
    }
}
