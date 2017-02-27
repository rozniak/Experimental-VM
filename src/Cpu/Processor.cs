/**
 * Oddmatics.Experiments.VM.Cpu.Processor -- Experimental Virtual Machine CPU
 *
 * This source-code is part of the experimental virtual machine project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://github.com/rozniak/Experimental-VM>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.Experiments.VM.Ram;
using System;

namespace Oddmatics.Experiments.VM.Cpu
{
    internal class Processor
    {
        #region CPU register objects

        private Register EaxRegister = new Register();
        private Register EbxRegister = new Register();
        private Register EcxRegister = new Register();
        private Register EdxRegister = new Register();
        private Register EsiRegister = new Register();
        private Register EdiRegister = new Register();
        private Register EspRegister = new Register();
        private Register EbpRegister = new Register();

        private Register IpRegister = new Register();
        private Register IrRegister = new Register();

        #endregion

        #region CPU registers

        public uint Eax { get { return EaxRegister.DWord; } private set { EaxRegister.DWord = value; } }
        public ushort Ax { get { return EaxRegister.Word; } private set { EaxRegister.Word = value; } }
        public byte Ah { get { return EaxRegister.LowByte; } private set { EaxRegister.LowByte = value; } }
        public byte Al { get { return EaxRegister.LowestByte; } private set { EaxRegister.LowestByte = value; } }

        public uint Ebx { get { return EbxRegister.DWord; } private set { EbxRegister.DWord = value; } }
        public ushort Bx { get { return EbxRegister.Word; } private set { EbxRegister.Word = value; } }
        public byte Bh { get { return EbxRegister.LowByte; } private set { EbxRegister.LowByte = value; } }
        public byte Bl { get { return EbxRegister.LowestByte; } private set { EbxRegister.LowestByte = value; } }

        public uint Ecx { get { return EcxRegister.DWord; } private set { EcxRegister.DWord = value; } }
        public ushort Cx { get { return EcxRegister.Word; } private set { EcxRegister.Word = value; } }
        public byte Ch { get { return EcxRegister.LowByte; } private set { EcxRegister.LowByte = value; } }
        public byte Cl { get { return EcxRegister.LowestByte; } private set { EcxRegister.LowestByte = value; } }

        public uint Edx { get { return EdxRegister.DWord; } private set { EdxRegister.DWord = value; } }
        public ushort Dx { get { return EdxRegister.Word; } private set { EdxRegister.Word = value; } }
        public byte Dh { get { return EdxRegister.LowByte; } private set { EdxRegister.LowByte = value; } }
        public byte Dl { get { return EdxRegister.LowestByte; } private set { EdxRegister.LowestByte = value; } }

        public uint Esi { get { return EsiRegister.DWord; } private set { EsiRegister.DWord = value; } }
        public uint Edi { get { return EdiRegister.DWord; } private set { EdiRegister.DWord = value; } }

        public uint Ip { get { return IpRegister.DWord; } private set { IpRegister.DWord = value; } }
        public uint Ir { get { return IrRegister.DWord; } private set { IrRegister.DWord = value; } }

        public uint Eflags { get; private set; }

        #endregion

        #region Flags

        private bool CarryFlag { get { return (Eflags & 0x01) == 0x01; } }
        private bool ParityFlag { get { return (Eflags & 0x04) == 0x04; } }
        private bool AdjustFlag { get { return (Eflags & 0x10) == 0x10; } }
        private bool ZeroFlag { get { return (Eflags & 0x40) == 0x40; } }
        private bool SignFlag { get { return (Eflags & 0x80) == 0x80; } }

        #endregion

        private Memory Ram;


        public Processor(ref Memory ram)
        {
            Ram = ram;
        }


        public void Execute(uint steps) // cycles is temporary
        {
            uint lastPtr = Ip + (steps * 4);

            while (Ip <= lastPtr)
            {
                // Fetch instruction at IP
                Ir = Ram.FetchDWord(Ip);
                Ip += 4; // Increment IP

                // Read instruction
                byte instruction = (byte)((Ir & MachineLanguage.DWORD_BYTE1) >> 24);

                switch (instruction)
                {
                    case MachineLanguage.MOV_REG_REG: MoveOpRegToReg(); break;
                    case MachineLanguage.MOV_REG_CONST: MoveOpConstToReg(); break;
                }
            }
        }

        public void ResetState()
        {
            Eax = 0;
            Ebx = 0;
            Ecx = 0;
            Edx = 0;
            Esi = 0;
            Edi = 0;
            Ip = 0;
            Ir = 0;
        }

        private uint GetRegisterValue(byte reg)
        {
            switch (reg)
            {
                case MachineLanguage.REG_EAX: return Eax;
                case MachineLanguage.REG_AX: return Ax;
                case MachineLanguage.REG_AH: return Ah;
                case MachineLanguage.REG_AL: return Al;

                case MachineLanguage.REG_EBX: return Ebx;
                case MachineLanguage.REG_BX: return Bx;
                case MachineLanguage.REG_BH: return Bh;
                case MachineLanguage.REG_BL: return Bl;

                case MachineLanguage.REG_ECX: return Ecx;
                case MachineLanguage.REG_CX: return Cx;
                case MachineLanguage.REG_CH: return Ch;
                case MachineLanguage.REG_CL: return Cl;

                case MachineLanguage.REG_EDX: return Edx;
                case MachineLanguage.REG_DX: return Dx;
                case MachineLanguage.REG_DH: return Dh;
                case MachineLanguage.REG_DL: return Dl;

                case MachineLanguage.REG_ESI: return Esi;
                case MachineLanguage.REG_EDI: return Edi;

                default: throw new ArgumentException("SetRegisterValue: " + reg + " was not recognised as the name of a valid register.");
            }
        }

        private void SetRegisterValue(byte reg, uint value)
        {
            switch (reg)
            {
                case MachineLanguage.REG_EAX: Eax = value; break;
                case MachineLanguage.REG_AX: Ax = (ushort)value; break;
                case MachineLanguage.REG_AH: Ah = (byte)value; break;
                case MachineLanguage.REG_AL: Al = (byte)value; break;

                case MachineLanguage.REG_EBX: Ebx = value; break;
                case MachineLanguage.REG_BX: Bx = (ushort)value; break;
                case MachineLanguage.REG_BH: Bh = (byte)value; break;
                case MachineLanguage.REG_BL: Bl = (byte)value; break;

                case MachineLanguage.REG_ECX: Ecx = value; break;
                case MachineLanguage.REG_CX: Cx = (ushort)value; break;
                case MachineLanguage.REG_CH: Ch = (byte)value; break;
                case MachineLanguage.REG_CL: Cl = (byte)value; break;

                case MachineLanguage.REG_EDX: Edx = value; break;
                case MachineLanguage.REG_DX: Dx = (ushort)value; break;
                case MachineLanguage.REG_DH: Dh = (byte)value; break;
                case MachineLanguage.REG_DL: Dl = (byte)value; break;

                case MachineLanguage.REG_ESI: Esi = value; break;
                case MachineLanguage.REG_EDI: Edi = value; break;

                default: throw new ArgumentException("SetRegisterValue: " + reg + " was not recognised as the name of a valid register.");
            }
        }


        private void MoveOpRegToReg()
        {
            // Get registers
            byte sourceRegister = (byte)((Ir & MachineLanguage.DWORD_BYTE3) >> 8);
            byte targetRegister = (byte)(Ir & MachineLanguage.DWORD_BYTE4);

            SetRegisterValue(targetRegister, GetRegisterValue(sourceRegister));
        }

        private void MoveOpConstToReg()
        {
            // Get target register
            byte targetRegister = (byte)(Ir & MachineLanguage.DWORD_BYTE4);

            // Fetch constant from memory
            Ir = Ram.FetchDWord(Ip);
            Ip += 4;

            SetRegisterValue(targetRegister, Ir);
        }
    }
}
