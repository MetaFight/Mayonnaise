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
    [BoardInfo("VRC6", 26)]
    [WithExternalSound]
    [NotImplementedWell("Mapper 26:\n Game Esper Dream 2 is glitchy")]
    class Mapper026 : Board
    {
        // TODO: VRC6 (mapper 26) Esper Dream 2 glitchy
        private int irq_reload;
        private int irq_counter;
        private int prescaler;
        private bool irq_mode_cycle;
        private bool irq_enable;
        private bool irq_enable_on_ak;
        private double[][][] mix_table;
        private VRC6PulseSoundChannel pulse1;
        private VRC6PulseSoundChannel pulse2;
        private VRC6SawtoothSoundChannel sawtooth;

        public override void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, Mirroring defaultMirroring)
        {
            base.Initialize(sha1, prg_dump, chr_dump, trainer_dump, defaultMirroring);
            InitializeSoundMixTable();
        }
        private void InitializeSoundMixTable()
        {
            mix_table = new double[16][][];

            for (int sq1 = 0; sq1 < 16; sq1++)
            {
                mix_table[sq1] = new double[16][];

                for (int sq2 = 0; sq2 < 16; sq2++)
                {
                    mix_table[sq1][sq2] = new double[256];

                    for (int saw = 0; saw < 256; saw++)
                    {
                        double sqr = (95.88 / (8128.0 / (sq1 + sq2) + 100));
                        double tnd = (159.79 / (1.0 / (saw / 22638.0) + 100));

                        mix_table[sq1][sq2][saw] = (sqr + tnd) * 128;
                    }
                }
            }
        }
        public override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
            pulse1 = new VRC6PulseSoundChannel();
            pulse2 = new VRC6PulseSoundChannel();
            sawtooth = new VRC6SawtoothSoundChannel();
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x8000:
                case 0x8002:
                case 0x8001:
                case 0x8003: Switch16KPRG(data, 0x8000, true); break;
                case 0x9000: pulse1.Write0(ref data); break;
                case 0x9002: pulse1.Write1(ref data); break;
                case 0x9001: pulse1.Write2(ref data); break;
                case 0xA000: pulse2.Write0(ref data); break;
                case 0xA002: pulse2.Write1(ref data); break;
                case 0xA001: pulse2.Write2(ref data); break;
                case 0xB000: sawtooth.Write0(ref data); break;
                case 0xB002: sawtooth.Write1(ref data); break;
                case 0xB001: sawtooth.Write2(ref data); break;
                case 0xB003:
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
                case 0xC000:
                case 0xC002:
                case 0xC001:
                case 0xC003: Switch08KPRG(data, 0xC000, true); break;
                case 0xD000: Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0xD002: Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                case 0xD001: Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0xD003: Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                case 0xE000: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0xE002: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0xE001: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0xE003: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                case 0xF000: irq_reload = data; break;
                case 0xF002:
                    {
                        irq_mode_cycle = (data & 0x4) == 0x4;
                        irq_enable = (data & 0x2) == 0x2;
                        irq_enable_on_ak = (data & 0x1) == 0x1;
                        if (irq_enable)
                        {
                            irq_counter = irq_reload;
                            prescaler = 341;
                        }
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xF001: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; irq_enable = irq_enable_on_ak; break;
            }
        }
        public override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (!irq_mode_cycle)
                {
                    if (prescaler > 0)
                        prescaler -= 3;
                    else
                    {
                        prescaler = 341;
                        irq_counter++;
                        if (irq_counter == 0xFF)
                        {
                            NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                            irq_counter = irq_reload;
                        }
                    }
                }
                else
                {
                    irq_counter++;
                    if (irq_counter == 0xFF)
                    {
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                        irq_counter = irq_reload;
                    }
                }
            }
        }
        public override double APUGetSamples()
        {
            return mix_table[pulse1.output][pulse2.output][sawtooth.output];
        }
        public override void OnAPUClockSingle(ref bool isClockingLength)
        {
            pulse1.ClockSingle();
            pulse2.ClockSingle();
            sawtooth.ClockSingle();
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(irq_reload);
            stream.Write(irq_counter);
            stream.Write(prescaler);
            stream.Write(irq_mode_cycle);
            stream.Write(irq_enable);
            stream.Write(irq_enable_on_ak);
            pulse1.SaveState(stream);
            pulse1.SaveState(stream);
            sawtooth.SaveState(stream);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            irq_reload = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            prescaler = stream.ReadInt32();
            irq_mode_cycle = stream.ReadBoolean();
            irq_enable = stream.ReadBoolean();
            irq_enable_on_ak = stream.ReadBoolean();
            pulse1.LoadState(stream);
            pulse1.LoadState(stream);
            sawtooth.LoadState(stream);
        }
    }
}
