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
using System;

/*CPU 6502 section*/
namespace MyNes.Core
{
    public partial class NesEmu
    {
        private static CPURegisters registers;
        private static byte M;
        private static byte opcode;
        // Using temp valus increase performance by avoiding memory allocation.
        private static byte byte_temp;
        private static int int_temp;
        private static int int_temp1;
        private static byte dummy;

        private static void CPUHardReset()
        {
            // registers
            registers.a = 0x00;
            registers.x = 0x00;
            registers.y = 0x00;

            registers.spl = 0xFD;
            registers.sph = 0x01;

            int rst = 0xFFFC;
            registers.pcl = board.ReadPRG(ref rst);
            rst++;
            registers.pch = board.ReadPRG(ref rst);
            registers.p = 0;
            registers.i = true;
            registers.ea = 0;
            //interrupts
            NMI_Current = false;
            NMI_Old = false;
            NMI_Detected = false;
            IRQFlags = 0;
            IRQ_Detected = false;
            interrupt_vector = 0;
            interrupt_suspend = false;
            nmi_enabled = false;
            nmi_old = false;
            vbl_flag = false;
            vbl_flag_temp = false;
            //others
            opcode = 0;
        }
        private static void CPUShutdown()
        {

        }
        private static void CPUSoftReset()
        {
            registers.i = true;
            registers.sp -= 3;

            registers.pcl = Read(0xFFFC);
            registers.pch = Read(0xFFFD);
        }
        private static void CPUClock()
        {
            // First clock is to fetch opcode
            opcode = Read(registers.pc);
            registers.pc++;

            #region Decode Opcode
            switch (opcode)
            {
                case 0x00:
                    {
                        BRK();
                        break;
                    }
                case 0x01:
                    {
                        IndirectX_R();
                        ORA();
                        break;
                    }
                case 0x02:
                    {
                        ImpliedAccumulator();// ILLEGAL ! set JAM.
                        break;
                    }
                case 0x03:
                    {
                        IndirectX_W();
                        SLO();
                        break;
                    }
                case 0x04:
                    {
                        ZeroPage_R(); // ILLEGAL ! set DOP
                        break;
                    }
                case 0x05:
                    {
                        ZeroPage_R();
                        ORA();
                        break;
                    }
                case 0x06:
                    {
                        ZeroPage_RW();
                        ASL_M();
                        break;
                    }
                case 0x07:
                    {
                        ZeroPage_W();
                        SLO();
                        break;
                    }
                case 0x08:
                    {
                        ImpliedAccumulator();
                        PHP();
                        break;
                    }
                case 0x09:
                    {
                        Immediate();
                        ORA();
                        break;
                    }
                case 0x0A:
                    {
                        ImpliedAccumulator();
                        ASL_A();
                        break;
                    }
                case 0x0B:
                    {
                        Immediate();
                        ANC();
                        break;
                    }
                case 0x0C:
                    {
                        Absolute_R(); // ILLEGAL ! set TOP 
                        break;
                    }
                case 0x0D:
                    {
                        Absolute_R();
                        ORA();
                        break;
                    }
                case 0x0E:
                    {
                        Absolute_RW();
                        ASL_M();
                        break;
                    }
                case 0x0F:
                    {
                        Absolute_W();
                        SLO();
                        break;
                    }
                case 0x10:
                    {
                        Branch(!registers.n);
                        break;
                    }
                case 0x11:
                    {
                        IndirectY_R();
                        ORA();
                        break;
                    }
                case 0x12:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0x13:
                    {
                        IndirectY_W();
                        SLO();
                        break;
                    }
                case 0x14:
                    {
                        ZeroPageX_R();// ILLEGAL ! set DOP
                        break;
                    }
                case 0x15:
                    {
                        ZeroPageX_R();
                        ORA();
                        break;
                    }
                case 0x16:
                    {
                        ZeroPageX_RW();
                        ASL_M();
                        break;
                    }
                case 0x17:
                    {
                        ZeroPageX_W();
                        SLO();
                        break;
                    }
                case 0x18:
                    {
                        ImpliedAccumulator();
                        registers.c = false;
                        break;
                    }
                case 0x19:
                    {
                        AbsoluteY_R();
                        ORA();
                        break;
                    }
                case 0x1A:
                    {
                        ImpliedAccumulator();// LEGAL ! set NOP. (is NOP a legal instruction ?)
                        break;
                    }
                case 0x1B:
                    {
                        AbsoluteY_W();
                        SLO();
                        break;
                    }
                case 0x1C:
                    {
                        AbsoluteX_R(); // ILLEGAL ! set TOP
                        break;
                    }
                case 0x1D:
                    {
                        AbsoluteX_R();
                        ORA();
                        break;
                    }
                case 0x1E:
                    {
                        AbsoluteX_RW();
                        ASL_M();
                        break;
                    }
                case 0x1F:
                    {
                        AbsoluteX_W();
                        SLO();
                        break;
                    }
                case 0x20:
                    {
                        JSR();
                        break;
                    }
                case 0x21:
                    {
                        IndirectX_R();
                        AND();
                        break;
                    }
                case 0x22:
                    {
                        ImpliedAccumulator();// ILLEGAL ! set JAM.
                        break;
                    }
                case 0x23:
                    {
                        IndirectX_W();
                        RLA();
                        break;
                    }
                case 0x24:
                    {
                        ZeroPage_R();
                        BIT();
                        break;
                    }
                case 0x25:
                    {
                        ZeroPage_R();
                        AND();
                        break;
                    }
                case 0x26:
                    {
                        ZeroPage_RW();
                        ROL_M();
                        break;
                    }
                case 0x27:
                    {
                        ZeroPage_W();
                        RLA();
                        break;
                    }
                case 0x28:
                    {
                        ImpliedAccumulator();
                        PLP();
                        break;
                    }
                case 0x29:
                    {
                        Immediate();
                        AND();
                        break;
                    }
                case 0x2A:
                    {
                        ImpliedAccumulator();
                        ROL_A();
                        break;
                    }
                case 0x2B:
                    {
                        Immediate();
                        ANC();
                        break;
                    }
                case 0x2C:
                    {
                        Absolute_R();
                        BIT();
                        break;
                    }
                case 0x2D:
                    {
                        Absolute_R();
                        AND();
                        break;
                    }
                case 0x2E:
                    {
                        Absolute_RW();
                        ROL_M();
                        break;
                    }
                case 0x2F:
                    {
                        Absolute_W();
                        RLA();
                        break;
                    }
                case 0x30:
                    {
                        Branch(registers.n);
                        break;
                    }
                case 0x31:
                    {
                        IndirectY_R();
                        AND();
                        break;
                    }
                case 0x32:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0x33:
                    {
                        IndirectY_W();
                        RLA();
                        break;
                    }
                case 0x34:
                    {
                        ZeroPageX_R();// ILLEGAL ! set DOP
                        break;
                    }
                case 0x35:
                    {
                        ZeroPageX_R();
                        AND();
                        break;
                    }
                case 0x36:
                    {
                        ZeroPageX_RW();
                        ROL_M();
                        break;
                    }
                case 0x37:
                    {
                        ZeroPageX_W();
                        RLA();
                        break;
                    }
                case 0x38:
                    {
                        ImpliedAccumulator();
                        registers.c = true;
                        break;
                    }
                case 0x39:
                    {
                        AbsoluteY_R();
                        AND();
                        break;
                    }
                case 0x3A:
                    {
                        ImpliedAccumulator();// LEGAL ! set NOP. (is NOP a legal instruction ?)
                        break;
                    }
                case 0x3B:
                    {
                        AbsoluteY_W();
                        RLA();
                        break;
                    }
                case 0x3C:
                    {
                        AbsoluteX_R(); // ILLEGAL ! set TOP
                        break;
                    }
                case 0x3D:
                    {
                        AbsoluteX_R();
                        AND();
                        break;
                    }
                case 0x3E:
                    {
                        AbsoluteX_RW();
                        ROL_M();
                        break;
                    }
                case 0x3F:
                    {
                        AbsoluteX_W();
                        RLA();
                        break;
                    }
                case 0x40:
                    {
                        ImpliedAccumulator();
                        RTI();
                        break;
                    }
                case 0x41:
                    {
                        IndirectX_R();
                        EOR();
                        break;
                    }
                case 0x42:
                    {
                        ImpliedAccumulator();// ILLEGAL ! set JAM.
                        break;
                    }
                case 0x43:
                    {
                        IndirectX_W();
                        SRE();
                        break;
                    }
                case 0x44:
                    {
                        ZeroPage_R(); // ILLEGAL ! set DOP
                        break;
                    }
                case 0x45:
                    {
                        ZeroPage_R();
                        EOR();
                        break;
                    }
                case 0x46:
                    {
                        ZeroPage_RW();
                        LSR_M();
                        break;
                    }
                case 0x47:
                    {
                        ZeroPage_W();
                        SRE();
                        break;
                    }
                case 0x48:
                    {
                        ImpliedAccumulator();
                        PHA();
                        break;
                    }
                case 0x49:
                    {
                        Immediate();
                        EOR();
                        break;
                    }
                case 0x4A:
                    {
                        ImpliedAccumulator();
                        LSR_A();
                        break;
                    }
                case 0x4B:
                    {
                        Immediate();
                        ALR();
                        break;
                    }
                case 0x4C:
                    {
                        Absolute_W();
                        registers.pc = registers.ea;/*JMP*/
                        break;
                    }
                case 0x4D:
                    {
                        Absolute_R();
                        EOR();
                        break;
                    }
                case 0x4E:
                    {
                        Absolute_RW();
                        LSR_M();
                        break;
                    }
                case 0x4F:
                    {
                        Absolute_W();
                        SRE();
                        break;
                    }
                case 0x50:
                    {
                        Branch(!registers.v);
                        break;
                    }
                case 0x51:
                    {
                        IndirectY_R();
                        EOR();
                        break;
                    }
                case 0x52:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0x53:
                    {
                        IndirectY_W();
                        SRE();
                        break;
                    }
                case 0x54:
                    {
                        ZeroPageX_R();// ILLEGAL ! set DOP
                        break;
                    }
                case 0x55:
                    {
                        ZeroPageX_R();
                        EOR();
                        break;
                    }
                case 0x56:
                    {
                        ZeroPageX_RW();
                        LSR_M();
                        break;
                    }
                case 0x57:
                    {
                        ZeroPageX_W();
                        SRE();
                        break;
                    }
                case 0x58:
                    {
                        ImpliedAccumulator();
                        registers.i = false;
                        break;
                    }
                case 0x59:
                    {
                        AbsoluteY_R();
                        EOR();
                        break;
                    }
                case 0x5A:
                    {
                        ImpliedAccumulator();// LEGAL ! set NOP. (is NOP a legal instruction ?)
                        break;
                    }
                case 0x5B:
                    {
                        AbsoluteY_W();
                        SRE();
                        break;
                    }
                case 0x5C:
                    {
                        AbsoluteX_R(); // ILLEGAL ! set TOP
                        break;
                    }
                case 0x5D:
                    {
                        AbsoluteX_R();
                        EOR();
                        break;
                    }
                case 0x5E:
                    {
                        AbsoluteX_RW();
                        LSR_M();
                        break;
                    }
                case 0x5F:
                    {
                        AbsoluteX_W();
                        SRE();
                        break;
                    }
                case 0x60:
                    {
                        ImpliedAccumulator();
                        RTS();
                        break;
                    }
                case 0x61:
                    {
                        IndirectX_R();
                        ADC();
                        break;
                    }
                case 0x62:
                    {
                        ImpliedAccumulator();// ILLEGAL ! set JAM.
                        break;
                    }
                case 0x63:
                    {
                        IndirectX_W();
                        RRA();
                        break;
                    }
                case 0x64:
                    {
                        ZeroPage_R(); // ILLEGAL ! set DOP
                        break;
                    }
                case 0x65:
                    {
                        ZeroPage_R();
                        ADC();
                        break;
                    }
                case 0x66:
                    {
                        ZeroPage_RW();
                        ROR_M();
                        break;
                    }
                case 0x67:
                    {
                        ZeroPage_W();
                        RRA();
                        break;
                    }
                case 0x68:
                    {
                        ImpliedAccumulator();
                        PLA();
                        break;
                    }
                case 0x69:
                    {
                        Immediate();
                        ADC();
                        break;
                    }
                case 0x6A:
                    {
                        ImpliedAccumulator();
                        ROR_A();
                        break;
                    }
                case 0x6B:
                    {
                        Immediate();
                        ARR();
                        break;
                    }
                case 0x6C:
                    {
                        JMP_I();
                        break;
                    }
                case 0x6D:
                    {
                        Absolute_R();
                        ADC();
                        break;
                    }
                case 0x6E:
                    {
                        Absolute_RW();
                        ROR_M();
                        break;
                    }
                case 0x6F:
                    {
                        Absolute_W();
                        RRA();
                        break;
                    }
                case 0x70:
                    {
                        Branch(registers.v);
                        break;
                    }
                case 0x71:
                    {
                        IndirectY_R();
                        ADC();
                        break;
                    }
                case 0x72:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0x73:
                    {
                        IndirectY_W();
                        RRA();
                        break;
                    }
                case 0x74:
                    {
                        ZeroPageX_R();// ILLEGAL ! set DOP
                        break;
                    }
                case 0x75:
                    {
                        ZeroPageX_R();
                        ADC();
                        break;
                    }
                case 0x76:
                    {
                        ZeroPageX_RW();
                        ROR_M();
                        break;
                    }
                case 0x77:
                    {
                        ZeroPageX_W();
                        RRA();
                        break;
                    }
                case 0x78:
                    {
                        ImpliedAccumulator();
                        registers.i = true;
                        break;
                    }
                case 0x79:
                    {
                        AbsoluteY_R();
                        ADC();
                        break;
                    }
                case 0x7A:
                    {
                        ImpliedAccumulator();// LEGAL ! set NOP. (is NOP a legal instruction ?)
                        break;
                    }
                case 0x7B:
                    {
                        AbsoluteY_W();
                        RRA();
                        break;
                    }
                case 0x7C:
                    {
                        AbsoluteX_R(); // ILLEGAL ! set TOP
                        break;
                    }
                case 0x7D:
                    {
                        AbsoluteX_R();
                        ADC();
                        break;
                    }
                case 0x7E:
                    {
                        AbsoluteX_RW();
                        ROR_M();
                        break;
                    }
                case 0x7F:
                    {
                        AbsoluteX_W();
                        RRA();
                        break;
                    }
                case 0x80:
                    {
                        Immediate(); // ILLEGAL ! set DOP
                        break;
                    }
                case 0x81:
                    {
                        IndirectX_W();
                        STA();
                        break;
                    }
                case 0x82:
                    {
                        Immediate();// ILLEGAL ! set DOP.
                        break;
                    }
                case 0x83:
                    {
                        IndirectX_W();
                        SAX();
                        break;
                    }
                case 0x84:
                    {
                        ZeroPage_W();
                        STY();
                        break;
                    }
                case 0x85:
                    {
                        ZeroPage_W();
                        STA();
                        break;
                    }
                case 0x86:
                    {
                        ZeroPage_W();
                        STX();
                        break;
                    }
                case 0x87:
                    {
                        ZeroPage_W();
                        SAX();
                        break;
                    }
                case 0x88:
                    {
                        ImpliedAccumulator();
                        DEY();
                        break;
                    }
                case 0x89:
                    {
                        Immediate();// ILLEGAL ! set DOP
                        break;
                    }
                case 0x8A:
                    {
                        ImpliedAccumulator();
                        TXA();
                        break;
                    }
                case 0x8B:
                    {
                        Immediate();
                        XAA();
                        break;
                    }
                case 0x8C:
                    {
                        Absolute_W();
                        STY();
                        break;
                    }
                case 0x8D:
                    {
                        Absolute_W();
                        STA();
                        break;
                    }
                case 0x8E:
                    {
                        Absolute_W();
                        STX();
                        break;
                    }
                case 0x8F:
                    {
                        Absolute_W();
                        SAX();
                        break;
                    }
                case 0x90:
                    {
                        Branch(!registers.c);
                        break;
                    }
                case 0x91:
                    {
                        IndirectY_W();
                        STA();
                        break;
                    }
                case 0x92:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0x93:
                    {
                        IndirectY_W();
                        AHX();
                        break;
                    }
                case 0x94:
                    {
                        ZeroPageX_W();
                        STY();
                        break;
                    }
                case 0x95:
                    {
                        ZeroPageX_W();
                        STA();
                        break;
                    }
                case 0x96:
                    {
                        ZeroPageY_W();
                        STX();
                        break;
                    }
                case 0x97:
                    {
                        ZeroPageY_W();
                        SAX();
                        break;
                    }
                case 0x98:
                    {
                        ImpliedAccumulator();
                        TYA();
                        break;
                    }
                case 0x99:
                    {
                        AbsoluteY_W();
                        STA();
                        break;
                    }
                case 0x9A:
                    {
                        ImpliedAccumulator();
                        TXS();
                        break;
                    }
                case 0x9B:
                    {
                        AbsoluteY_W();
                        XAS();
                        break;
                    }
                case 0x9C:
                    {
                        Absolute_W();
                        SHY(); // ILLEGAL ! SHY fits here.
                        break;
                    }
                case 0x9D:
                    {
                        AbsoluteX_W();
                        STA();
                        break;
                    }
                case 0x9E:
                    {
                        Absolute_W();
                        SHX();// ILLEGAL ! SHX fits here.
                        break;
                    }
                case 0x9F:
                    {
                        AbsoluteY_W();
                        AHX();
                        break;
                    }
                case 0xA0:
                    {
                        Immediate();
                        LDY();
                        break;
                    }
                case 0xA1:
                    {
                        IndirectX_R();
                        LDA();
                        break;
                    }
                case 0xA2:
                    {
                        Immediate();
                        LDX();
                        break;
                    }
                case 0xA3:
                    {
                        IndirectX_R();
                        LAX();
                        break;
                    }
                case 0xA4:
                    {
                        ZeroPage_R();
                        LDY();
                        break;
                    }
                case 0xA5:
                    {
                        ZeroPage_R();
                        LDA();
                        break;
                    }
                case 0xA6:
                    {
                        ZeroPage_R();
                        LDX();
                        break;
                    }
                case 0xA7:
                    {
                        ZeroPage_R();
                        LAX();
                        break;
                    }
                case 0xA8:
                    {
                        ImpliedAccumulator();
                        TAY();
                        break;
                    }
                case 0xA9:
                    {
                        Immediate();
                        LDA();
                        break;
                    }
                case 0xAA:
                    {
                        ImpliedAccumulator();
                        TAX();
                        break;
                    }
                case 0xAB:
                    {
                        Immediate();
                        LAX();
                        break;
                    }
                case 0xAC:
                    {
                        Absolute_R();
                        LDY();
                        break;
                    }
                case 0xAD:
                    {
                        Absolute_R();
                        LDA();
                        break;
                    }
                case 0xAE:
                    {
                        Absolute_R();
                        LDX();
                        break;
                    }
                case 0xAF:
                    {
                        Absolute_R();
                        LAX();
                        break;
                    }
                case 0xB0:
                    {
                        Branch(registers.c);
                        break;
                    }
                case 0xB1:
                    {
                        IndirectY_R();
                        LDA();
                        break;
                    }
                case 0xB2:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0xB3:
                    {
                        IndirectY_R();
                        LAX();
                        break;
                    }
                case 0xB4:
                    {
                        ZeroPageX_R();
                        LDY();
                        break;
                    }
                case 0xB5:
                    {
                        ZeroPageX_R();
                        LDA();
                        break;
                    }
                case 0xB6:
                    {
                        ZeroPageY_R();
                        LDX();
                        break;
                    }
                case 0xB7:
                    {
                        ZeroPageY_R();
                        LAX();
                        break;
                    }
                case 0xB8:
                    {
                        ImpliedAccumulator();
                        registers.v = false;
                        break;
                    }
                case 0xB9:
                    {
                        AbsoluteY_R();
                        LDA();
                        break;
                    }
                case 0xBA:
                    {
                        ImpliedAccumulator();
                        TSX();
                        break;
                    }
                case 0xBB:
                    {
                        AbsoluteY_R();
                        LAR();
                        break;
                    }
                case 0xBC:
                    {
                        AbsoluteX_R();
                        LDY();
                        break;
                    }
                case 0xBD:
                    {
                        AbsoluteX_R();
                        LDA();
                        break;
                    }
                case 0xBE:
                    {
                        AbsoluteY_R();
                        LDX();
                        break;
                    }
                case 0xBF:
                    {
                        AbsoluteY_R();
                        LAX();
                        break;
                    }
                case 0xC0:
                    {
                        Immediate();
                        CPY();
                        break;
                    }
                case 0xC1:
                    {
                        IndirectX_R();
                        CMP();
                        break;
                    }
                case 0xC2:
                    {
                        Immediate(); // ILLEGAL ! set DOP.
                        break;
                    }
                case 0xC3:
                    {
                        IndirectX_R();
                        DCP();
                        break;
                    }
                case 0xC4:
                    {
                        ZeroPage_R();
                        CPY();
                        break;
                    }
                case 0xC5:
                    {
                        ZeroPage_R();
                        CMP();
                        break;
                    }
                case 0xC6:
                    {
                        ZeroPage_RW();
                        DEC();
                        break;
                    }
                case 0xC7:
                    {
                        ZeroPage_R();
                        DCP();
                        break;
                    }
                case 0xC8:
                    {
                        ImpliedAccumulator();
                        INY();
                        break;
                    }
                case 0xC9:
                    {
                        Immediate();
                        CMP();
                        break;
                    }
                case 0xCA:
                    {
                        ImpliedAccumulator();
                        DEX();
                        break;
                    }
                case 0xCB:
                    {
                        Immediate();
                        AXS();
                        break;
                    }
                case 0xCC:
                    {
                        Absolute_R();
                        CPY();
                        break;
                    }
                case 0xCD:
                    {
                        Absolute_R();
                        CMP();
                        break;
                    }
                case 0xCE:
                    {
                        Absolute_RW();
                        DEC();
                        break;
                    }
                case 0xCF:
                    {
                        Absolute_R();
                        DCP();
                        break;
                    }
                case 0xD0:
                    {
                        Branch(!registers.z);
                        break;
                    }
                case 0xD1:
                    {
                        IndirectY_R();
                        CMP();
                        break;
                    }
                case 0xD2:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0xD3:
                    {
                        IndirectY_RW();
                        DCP();
                        break;
                    }
                case 0xD4:
                    {
                        ZeroPageX_R();// ILLEGAL ! set DOP
                        break;
                    }
                case 0xD5:
                    {
                        ZeroPageX_R();
                        CMP();
                        break;
                    }
                case 0xD6:
                    {
                        ZeroPageX_RW();
                        DEC();
                        break;
                    }
                case 0xD7:
                    {
                        ZeroPageX_RW();
                        DCP();
                        break;
                    }
                case 0xD8:
                    {
                        ImpliedAccumulator();
                        registers.d = false;
                        break;
                    }
                case 0xD9:
                    {
                        AbsoluteY_R();
                        CMP();
                        break;
                    }
                case 0xDA:
                    {
                        ImpliedAccumulator();// LEGAL ! set NOP. (is NOP a legal instruction ?)
                        break;
                    }
                case 0xDB:
                    {
                        AbsoluteY_RW();
                        DCP();
                        break;
                    }
                case 0xDC:
                    {
                        AbsoluteX_R(); // ILLEGAL ! set TOP
                        break;
                    }
                case 0xDD:
                    {
                        AbsoluteX_R();
                        CMP();
                        break;
                    }
                case 0xDE:
                    {
                        AbsoluteX_RW();
                        DEC();
                        break;
                    }
                case 0xDF:
                    {
                        AbsoluteX_RW();
                        DCP();
                        break;
                    }
                case 0xE0:
                    {
                        Immediate();
                        CPX();
                        break;
                    }
                case 0xE1:
                    {
                        IndirectX_R();
                        SBC();
                        break;
                    }
                case 0xE2:
                    {
                        Immediate(); // ILLEGAL ! set DOP.
                        break;
                    }
                case 0xE3:
                    {
                        IndirectX_W();
                        ISC();
                        break;
                    }
                case 0xE4:
                    {
                        ZeroPage_R();
                        CPX();
                        break;
                    }
                case 0xE5:
                    {
                        ZeroPage_R();
                        SBC();
                        break;
                    }
                case 0xE6:
                    {
                        ZeroPage_RW();
                        INC();
                        break;
                    }
                case 0xE7:
                    {
                        ZeroPage_W();
                        ISC();
                        break;
                    }
                case 0xE8:
                    {
                        ImpliedAccumulator();
                        INX();
                        break;
                    }
                case 0xE9:
                    {
                        Immediate();
                        SBC();
                        break;
                    }
                case 0xEA:
                    {
                        ImpliedAccumulator();// NOP ...
                        break;
                    }
                case 0xEB:
                    {
                        Immediate();
                        SBC();
                        break;
                    }
                case 0xEC:
                    {
                        Absolute_R();
                        CPX();
                        break;
                    }
                case 0xED:
                    {
                        Absolute_R();
                        SBC();
                        break;
                    }
                case 0xEE:
                    {
                        Absolute_RW();
                        INC();
                        break;
                    }
                case 0xEF:
                    {
                        Absolute_W();
                        ISC();
                        break;
                    }
                case 0xF0:
                    {
                        Branch(registers.z);
                        break;
                    }
                case 0xF1:
                    {
                        IndirectY_R();
                        SBC();
                        break;
                    }
                case 0xF2:
                    {
                        // ILLEGAL ! set JAM.
                        break;
                    }
                case 0xF3:
                    {
                        IndirectY_W();
                        ISC();
                        break;
                    }
                case 0xF4:
                    {
                        ZeroPageX_R();// ILLEGAL ! set DOP
                        break;
                    }
                case 0xF5:
                    {
                        ZeroPageX_R();
                        SBC();
                        break;
                    }
                case 0xF6:
                    {
                        ZeroPageX_RW();
                        INC();
                        break;
                    }
                case 0xF7:
                    {
                        ZeroPageX_W();
                        ISC();
                        break;
                    }
                case 0xF8:
                    {
                        ImpliedAccumulator();
                        registers.d = true;
                        break;
                    }
                case 0xF9:
                    {
                        AbsoluteY_R();
                        SBC();
                        break;
                    }
                case 0xFA:
                    {
                        ImpliedAccumulator();// LEGAL ! set NOP. (is NOP a legal instruction ?)
                        break;
                    }
                case 0xFB:
                    {
                        AbsoluteY_W();
                        ISC();
                        break;
                    }
                case 0xFC:
                    {
                        AbsoluteX_R(); // ILLEGAL ! set TOP
                        break;
                    }
                case 0xFD:
                    {
                        AbsoluteX_R();
                        SBC();
                        break;
                    }
                case 0xFE:
                    {
                        AbsoluteX_RW();
                        INC();
                        break;
                    }
                case 0xFF:
                    {
                        AbsoluteX_W();
                        ISC();
                        break;
                    }
            }
            #endregion
            // Handle interrupts...
            if (NMI_Detected)
            {
                Interrupt();

                NMI_Detected = false;// NMI handled !
            }
            else if (IRQ_Detected)
            {
                Interrupt();
            }
        }

        #region Addressing modes
        /*
         * _R: read access instructions, set the M value. Some addressing modes will execute 1 extra cycle when page crossed.
         * _W: write access instructions, doesn't set the M value. The instruction should handle write at effective address (EF).
         * _RW: Read-Modify-Write instructions, set the M value and the instruction should handle write at effective address (EF).
         */
        private static void IndirectX_R()
        {
            byte_temp = Read(registers.pc);
            registers.pc++;// CLock 1
            Read(byte_temp);// Clock 2
            byte_temp += registers.x;

            registers.eal = Read(byte_temp);// Clock 3
            byte_temp++;

            registers.eah = Read(byte_temp);// Clock 4

            M = Read(registers.ea);
        }
        private static void IndirectX_W()
        {
            byte_temp = Read(registers.pc);
            registers.pc++;// CLock 1
            Read(byte_temp);// Clock 2
            byte_temp += registers.x;

            registers.eal = Read(byte_temp);// Clock 3
            byte_temp++;

            registers.eah = Read(byte_temp);// Clock 4
        }
        private static void IndirectX_RW()
        {
            byte_temp = Read(registers.pc);
            registers.pc++;// CLock 1
            Read(byte_temp);// Clock 2
            byte_temp += registers.x;

            registers.eal = Read(byte_temp);// Clock 3
            byte_temp++;

            registers.eah = Read(byte_temp);// Clock 4

            M = Read(registers.ea);
        }
        private static void IndirectY_R()
        {
            byte_temp = Read(registers.pc);
            registers.pc++;// CLock 1
            registers.eal = Read(byte_temp);
            byte_temp++;// Clock 2
            registers.eah = Read(byte_temp);// Clock 2

            registers.eal += registers.y;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.y)
            {
                registers.eah++;
                M = Read(registers.ea);// Clock 4
            }
        }
        private static void IndirectY_W()
        {
            byte_temp = Read(registers.pc);
            registers.pc++;// CLock 1
            registers.eal = Read(byte_temp);
            byte_temp++;// Clock 2
            registers.eah = Read(byte_temp);// Clock 2

            registers.eal += registers.y;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.y)
                registers.eah++;
        }
        private static void IndirectY_RW()
        {
            byte_temp = Read(registers.pc);
            registers.pc++;// CLock 1
            registers.eal = Read(byte_temp);
            byte_temp++;// Clock 2
            registers.eah = Read(byte_temp);// Clock 2

            registers.eal += registers.y;

            Read(registers.ea);// Clock 3
            if (registers.eal < registers.y)
                registers.eah++;

            M = Read(registers.ea);// Clock 4
        }
        private static void ZeroPage_R()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            M = Read(registers.ea);// Clock 2
        }
        private static void ZeroPage_W()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
        }
        private static void ZeroPage_RW()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            M = Read(registers.ea);// Clock 2
        }
        private static void ZeroPageX_R()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            Read(registers.ea);// Clock 2
            registers.eal += registers.x;
            M = Read(registers.ea);// Clock 3
        }
        private static void ZeroPageX_W()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            Read(registers.ea);// Clock 2
            registers.eal += registers.x;
        }
        private static void ZeroPageX_RW()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            Read(registers.ea);// Clock 2
            registers.eal += registers.x;
            M = Read(registers.ea);// Clock 3
        }
        private static void ZeroPageY_R()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            Read(registers.ea);// Clock 2
            registers.eal += registers.y;
            M = Read(registers.ea);// Clock 3
        }
        private static void ZeroPageY_W()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            Read(registers.ea);// Clock 2
            registers.eal += registers.y;
        }
        private static void ZeroPageY_RW()
        {
            registers.ea = Read(registers.pc);
            registers.pc++;// Clock 1
            Read(registers.ea);// Clock 2
            registers.eal += registers.y;
            M = Read(registers.ea);// Clock 3
        }
        private static void Immediate()
        {
            M = Read(registers.pc);
            registers.pc++;// Clock 1
        }
        private static void ImpliedAccumulator()
        {
            dummy = Read(registers.pc);
        }
        private static void Absolute_R()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2
            M = Read(registers.ea);// Clock 3
        }
        private static void Absolute_W()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2
        }
        private static void Absolute_RW()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2
            M = Read(registers.ea);// Clock 3
        }
        private static void AbsoluteX_R()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2

            registers.eal += registers.x;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.x)
            {
                registers.eah++;
                M = Read(registers.ea);// Clock 4
            }
        }
        private static void AbsoluteX_W()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2

            registers.eal += registers.x;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.x)
                registers.eah++;
        }
        private static void AbsoluteX_RW()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2

            registers.eal += registers.x;

            Read(registers.ea);// Clock 3
            if (registers.eal < registers.x)
                registers.eah++;

            M = Read(registers.ea);// Clock 4
        }
        private static void AbsoluteY_R()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2

            registers.eal += registers.y;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.y)
            {
                registers.eah++;
                M = Read(registers.ea);// Clock 4
            }
        }
        private static void AbsoluteY_W()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2

            registers.eal += registers.y;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.y)
                registers.eah++;
        }
        private static void AbsoluteY_RW()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;// Clock 1
            registers.eah = Read(registers.pc);
            registers.pc++;// Clock 2

            registers.eal += registers.y;

            M = Read(registers.ea);// Clock 3
            if (registers.eal < registers.y)
                registers.eah++;

            M = Read(registers.ea);// Clock 4
        }
        #endregion

        #region Instructions
        private static void Interrupt()
        {
            Read(registers.pc);
            Read(registers.pc);

            Push(registers.pch);
            Push(registers.pcl);

            Push(registers.p);
            // the vector is detected during φ2 of previous cycle (before push about 2 ppu cycles)
            int_temp = interrupt_vector;

            interrupt_suspend = true;
            registers.pcl = Read(int_temp);
            int_temp++;
            registers.i = true;
            registers.pch = Read(int_temp);
            interrupt_suspend = false;
        }
        private static void Branch(bool condition)
        {
            byte_temp = Read(registers.pc);
            registers.pc++;

            if (condition)
            {
                interrupt_suspend = true;
                Read(registers.pc);
                registers.pcl += byte_temp;
                interrupt_suspend = false;
                if (byte_temp >= 0x80)
                {
                    if (registers.pcl >= byte_temp)
                    {
                        Read(registers.pc);
                        registers.pch--;
                    }
                }
                else
                {
                    if (registers.pcl < byte_temp)
                    {
                        Read(registers.pc);
                        registers.pch++;
                    }
                }
            }

        }
        private static void Push(byte val)
        {
            Write(registers.sp, val);
            registers.spl--;
        }
        private static byte Pull()
        {
            registers.spl++;
            return Read(registers.sp);
        }
        private static void ADC()
        {
            int_temp = (registers.a + M + (registers.c ? 1 : 0));

            registers.v = ((int_temp ^ registers.a) & (int_temp ^ M) & 0x80) != 0;
            registers.n = (int_temp & 0x80) != 0;
            registers.z = (int_temp & 0xFF) == 0;
            registers.c = (int_temp >> 0x8) != 0;

            registers.a = (byte)(int_temp & 0xFF);
        }
        private static void AHX()
        {
            byte_temp = (byte)((registers.a & registers.x) & 7);
            Write(registers.ea, byte_temp);
        }
        private static void ALR()
        {
            registers.a &= M;

            registers.c = (registers.a & 0x01) != 0;

            registers.a >>= 1;

            registers.n = (registers.a & 0x80) != 0;
            registers.z = registers.a == 0;
        }
        private static void ANC()
        {
            registers.a &= M;
            registers.n = (registers.a & 0x80) != 0;
            registers.z = registers.a == 0;
            registers.c = (registers.a & 0x80) != 0;
        }
        private static void AND()
        {
            registers.a &= M;
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void ARR()
        {
            registers.a = (byte)(((M & registers.a) >> 1) | (registers.c ? 0x80 : 0x00));

            registers.z = (registers.a & 0xFF) == 0;
            registers.n = (registers.a & 0x80) != 0;
            registers.c = (registers.a & 0x40) != 0;
            registers.v = ((registers.a << 1 ^ registers.a) & 0x40) != 0;
        }
        private static void AXS()
        {
            int_temp = (registers.a & registers.x) - M;

            registers.n = (int_temp & 0x80) != 0;
            registers.z = (int_temp & 0xFF) == 0;
            registers.c = (~int_temp >> 8) != 0;

            registers.x = (byte)(int_temp & 0xFF);
        }
        private static void ASL_M()
        {
            registers.c = (M & 0x80) == 0x80;
            Write(registers.ea, M);

            M = (byte)((M << 1) & 0xFE);

            Write(registers.ea, M);

            registers.n = (M & 0x80) == 0x80;
            registers.z = (M == 0);
        }
        private static void ASL_A()
        {
            registers.c = (registers.a & 0x80) == 0x80;

            registers.a = (byte)((registers.a << 1) & 0xFE);

            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void BIT()
        {
            registers.n = (M & 0x80) != 0;
            registers.v = (M & 0x40) != 0;
            registers.z = (M & registers.a) == 0;
        }
        private static void BRK()
        {
            Read(registers.pc);
            registers.pc++;

            Push(registers.pch);
            Push(registers.pcl);

            Push(registers.pb());
            // the vector is detected during φ2 of previous cycle (before push about 2 ppu cycles)
            int_temp = interrupt_vector;

            interrupt_suspend = true;
            registers.pcl = Read(int_temp);
            int_temp++;
            registers.i = true;
            registers.pch = Read(int_temp);
            interrupt_suspend = false;
        }
        private static void CMP()
        {
            int_temp = registers.a - M;
            registers.n = (int_temp & 0x80) == 0x80;
            registers.c = (registers.a >= M);
            registers.z = (int_temp == 0);
        }
        private static void CPX()
        {
            int_temp = registers.x - M;
            registers.n = (int_temp & 0x80) == 0x80;
            registers.c = (registers.x >= M);
            registers.z = (int_temp == 0);
        }
        private static void CPY()
        {
            int_temp = registers.y - M;
            registers.n = (int_temp & 0x80) == 0x80;
            registers.c = (registers.y >= M);
            registers.z = (int_temp == 0);
        }
        private static void DCP()
        {
            Write(registers.ea, M);

            M--;
            Write(registers.ea, M);

            int_temp = registers.a - M;

            registers.n = (int_temp & 0x80) != 0;
            registers.z = int_temp == 0;
            registers.c = (~int_temp >> 8) != 0;
        }
        private static void DEC()
        {
            Write(registers.ea, M);
            M--;
            Write(registers.ea, M);
            registers.n = (M & 0x80) == 0x80;
            registers.z = (M == 0);
        }
        private static void DEY()
        {
            registers.y--;
            registers.z = (registers.y == 0);
            registers.n = (registers.y & 0x80) == 0x80;
        }
        private static void DEX()
        {
            registers.x--;
            registers.z = (registers.x == 0);
            registers.n = (registers.x & 0x80) == 0x80;
        }
        private static void EOR()
        {
            registers.a ^= M;
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void INC()
        {
            Write(registers.ea, M);
            M++;
            Write(registers.ea, M);
            registers.n = (M & 0x80) == 0x80;
            registers.z = (M == 0);
        }
        private static void INX()
        {
            registers.x++;
            registers.z = (registers.x == 0);
            registers.n = (registers.x & 0x80) == 0x80;
        }
        private static void INY()
        {
            registers.y++;
            registers.n = (registers.y & 0x80) == 0x80;
            registers.z = (registers.y == 0);
        }
        private static void ISC()
        {
            byte_temp = Read(registers.ea);

            Write(registers.ea, byte_temp);

            byte_temp++;

            Write(registers.ea, byte_temp);

            int_temp = byte_temp ^ 0xFF;
            int_temp1 = (registers.a + int_temp + (registers.c ? 1 : 0));

            registers.n = (int_temp1 & 0x80) != 0;
            registers.v = ((int_temp1 ^ registers.a) & (int_temp1 ^ int_temp) & 0x80) != 0;
            registers.z = (int_temp1 & 0xFF) == 0;
            registers.c = (int_temp1 >> 0x8) != 0;
            registers.a = (byte)(int_temp1 & 0xFF);
        }
        private static void JMP_I()
        {
            registers.eal = Read(registers.pc++);
            registers.eah = Read(registers.pc++);

            byte_temp = Read(registers.ea);
            registers.eal++; // only increment the low byte, causing the "JMP ($nnnn)" bug
            registers.pch = Read(registers.ea);

            registers.pcl = byte_temp;
        }
        private static void JSR()
        {
            registers.eal = Read(registers.pc);
            registers.pc++;
            registers.eah = Read(registers.pc);

            Push(registers.pch);
            Push(registers.pcl);

            registers.eah = Read(registers.pc);
            registers.pc = registers.ea;
        }
        private static void LAR()
        {
            registers.spl &= M;
            registers.a = registers.spl;
            registers.x = registers.spl;

            registers.n = (registers.spl & 0x80) != 0;
            registers.z = (registers.spl & 0xFF) == 0;
        }
        private static void LAX()
        {
            registers.x = registers.a = M;

            registers.n = (registers.x & 0x80) != 0;
            registers.z = (registers.x & 0xFF) == 0;
        }
        private static void LDA()
        {
            registers.a = M;
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void LDX()
        {
            registers.x = M;
            registers.n = (registers.x & 0x80) == 0x80;
            registers.z = (registers.x == 0);
        }
        private static void LDY()
        {
            registers.y = M;
            registers.n = (registers.y & 0x80) == 0x80;
            registers.z = (registers.y == 0);
        }
        private static void LSR_A()
        {
            registers.c = (registers.a & 1) == 1;
            registers.a >>= 1;
            registers.z = (registers.a == 0);
            registers.n = (registers.a & 0x80) != 0;
        }
        private static void LSR_M()
        {
            registers.c = (M & 1) == 1;
            Write(registers.ea, M);
            M >>= 1;

            Write(registers.ea, M);
            registers.z = (M == 0);
            registers.n = (M & 0x80) != 0;
        }
        private static void ORA()
        {
            registers.a |= M;
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void PHA()
        {
            Push(registers.a);
        }
        private static void PHP()
        {
            Push(registers.pb());
        }
        private static void PLA()
        {
            Read(registers.sp);
            registers.a = Pull();
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void PLP()
        {
            Read(registers.sp);
            registers.p = Pull();
        }
        private static void RLA()
        {
            byte_temp = Read(registers.ea);

            Write(registers.ea, byte_temp);

            dummy = (byte)((byte_temp << 1) | (registers.c ? 0x01 : 0x00));

            Write(registers.ea, dummy);

            registers.n = (dummy & 0x80) != 0;
            registers.z = (dummy & 0xFF) == 0;
            registers.c = (byte_temp & 0x80) != 0;

            registers.a &= dummy;
            registers.n = (registers.a & 0x80) != 0;
            registers.z = (registers.a & 0xFF) == 0;
        }
        private static void ROL_A()
        {
            byte_temp = (byte)((registers.a << 1) | (registers.c ? 0x01 : 0x00));

            registers.n = (byte_temp & 0x80) != 0;
            registers.z = (byte_temp & 0xFF) == 0;
            registers.c = (registers.a & 0x80) != 0;

            registers.a = byte_temp;
        }
        private static void ROL_M()
        {
            Write(registers.ea, M);
            byte_temp = (byte)((M << 1) | (registers.c ? 0x01 : 0x00));

            Write(registers.ea, byte_temp);
            registers.n = (byte_temp & 0x80) != 0;
            registers.z = (byte_temp & 0xFF) == 0;
            registers.c = (M & 0x80) != 0;
        }
        private static void ROR_A()
        {
            byte_temp = (byte)((registers.a >> 1) | (registers.c ? 0x80 : 0x00));

            registers.n = (byte_temp & 0x80) != 0;
            registers.z = (byte_temp & 0xFF) == 0;
            registers.c = (registers.a & 0x01) != 0;

            registers.a = byte_temp;
        }
        private static void ROR_M()
        {
            Write(registers.ea, M);

            byte_temp = (byte)((M >> 1) | (registers.c ? 0x80 : 0x00));
            Write(registers.ea, byte_temp);

            registers.n = (byte_temp & 0x80) != 0;
            registers.z = (byte_temp & 0xFF) == 0;
            registers.c = (M & 0x01) != 0;
        }
        private static void RRA()
        {
            byte_temp = Read(registers.ea);

            Write(registers.ea, byte_temp);

            dummy = (byte)((byte_temp >> 1) | (registers.c ? 0x80 : 0x00));

            Write(registers.ea, dummy);

            registers.n = (dummy & 0x80) != 0;
            registers.z = (dummy & 0xFF) == 0;
            registers.c = (byte_temp & 0x01) != 0;

            byte_temp = dummy;
            int_temp = (registers.a + byte_temp + (registers.c ? 1 : 0));

            registers.n = (int_temp & 0x80) != 0;
            registers.v = ((int_temp ^ registers.a) & (int_temp ^ byte_temp) & 0x80) != 0;
            registers.z = (int_temp & 0xFF) == 0;
            registers.c = (int_temp >> 0x8) != 0;
            registers.a = (byte)(int_temp);
        }
        private static void RTI()
        {
            Read(registers.sp);
            registers.p = Pull();

            registers.pcl = Pull();
            registers.pch = Pull();
        }
        private static void RTS()
        {
            Read(registers.sp);
            registers.pcl = Pull();
            registers.pch = Pull();

            registers.pc++;

            Read(registers.pc);
        }
        private static void SAX()
        {
            Write(registers.ea, (byte)(registers.x & registers.a));
        }
        private static void SBC()
        {
            M ^= 0xFF;
            int_temp = (registers.a + M + (registers.c ? 1 : 0));

            registers.n = (int_temp & 0x80) != 0;
            registers.v = ((int_temp ^ registers.a) & (int_temp ^ M) & 0x80) != 0;
            registers.z = (int_temp & 0xFF) == 0;
            registers.c = (int_temp >> 0x8) != 0;
            registers.a = (byte)(int_temp);
        }
        private static void SHX()
        {
            byte_temp = (byte)(registers.x & (registers.eah + 1));

            Read(registers.ea);
            registers.eal += registers.y;

            if (registers.eal < registers.y)
                registers.eah = byte_temp;

            Write(registers.ea, byte_temp);
        }
        private static void SHY()
        {
            byte_temp = (byte)(registers.y & (registers.eah + 1));

            Read(registers.ea);
            registers.eal += registers.x;

            if (registers.eal < registers.x)
                registers.eah = byte_temp;
            Write(registers.ea, byte_temp);
        }
        private static void SLO()
        {
            byte_temp = Read(registers.ea);

            registers.c = (byte_temp & 0x80) != 0;

            Write(registers.ea, byte_temp);

            byte_temp <<= 1;

            Write(registers.ea, byte_temp);

            registers.n = (byte_temp & 0x80) != 0;
            registers.z = (byte_temp & 0xFF) == 0;

            registers.a |= byte_temp;
            registers.n = (registers.a & 0x80) != 0;
            registers.z = (registers.a & 0xFF) == 0;
        }
        private static void SRE()
        {
            byte_temp = Read(registers.ea);

            registers.c = (byte_temp & 0x01) != 0;

            Write(registers.ea, byte_temp);

            byte_temp >>= 1;

            Write(registers.ea, byte_temp);

            registers.n = (byte_temp & 0x80) != 0;
            registers.z = (byte_temp & 0xFF) == 0;

            registers.a ^= byte_temp;
            registers.n = (registers.a & 0x80) != 0;
            registers.z = (registers.a & 0xFF) == 0;
        }
        private static void STA()
        {
            Write(registers.ea, registers.a);
        }
        private static void STX()
        {
            Write(registers.ea, registers.x);
        }
        private static void STY()
        {
            Write(registers.ea, registers.y);
        }
        private static void TAX()
        {
            registers.x = registers.a;
            registers.n = (registers.x & 0x80) == 0x80;
            registers.z = (registers.x == 0);
        }
        private static void TAY()
        {
            registers.y = registers.a;
            registers.n = (registers.y & 0x80) == 0x80;
            registers.z = (registers.y == 0);
        }
        private static void TSX()
        {
            registers.x = registers.spl;
            registers.n = (registers.x & 0x80) != 0;
            registers.z = registers.x == 0;
        }
        private static void TXA()
        {
            registers.a = registers.x;
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void TXS()
        {
            registers.spl = registers.x;
        }
        private static void TYA()
        {
            registers.a = registers.y;
            registers.n = (registers.a & 0x80) == 0x80;
            registers.z = (registers.a == 0);
        }
        private static void XAA()
        {
            registers.a = (byte)(registers.x & M);
            registers.n = (registers.a & 0x80) != 0;
            registers.z = (registers.a & 0xFF) == 0;
        }
        private static void XAS()
        {
            registers.spl = (byte)(registers.a & registers.x /*& ((dummyVal >> 8) + 1)*/);
            Write(registers.ea, registers.spl);
        }
        #endregion
    }
}

