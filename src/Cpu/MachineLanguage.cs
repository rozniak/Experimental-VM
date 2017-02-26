/**
 * Oddmatics.Experiments.VM.Cpu.MachineLanguage -- Experimental Virtual Machine Machine Language Definitions
 *
 * This source-code is part of the experimental virtual machine project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://github.com/rozniak/Experimental-VM>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace Oddmatics.Experiments.VM.Cpu
{
    internal static class MachineLanguage
    {
        #region Bitmasks

        public const uint DWORD_BYTE1 = 0xFF000000;
        public const uint DWORD_BYTE2 = 0x00FF0000;
        public const uint DWORD_BYTE3 = 0x0000FF00;
        public const uint DWORD_BYTE4 = 0x000000FF;

        #endregion

        #region MOV opcodes

        public const byte MOV_REG_REG = 0x20;
        public const byte MOV_REG_CONST = 0x21;
        public const byte MOV_REG_MEM8 = 0x22;
        public const byte MOV_REG_MEM16 = 0x23;
        public const byte MOV_REG_MEM32 = 0x24;
        public const byte MOV_MEM8_REG = 0x25;
        public const byte MOV_MEM8_CONST = 0x26;
        public const byte MOV_MEM16_REG = 0x27;
        public const byte MOV_MEM16_CONST = 0x28;
        public const byte MOV_MEM32_REG = 0x29;
        public const byte MOV_MEM32_CONST = 0x2A;

        #endregion

        #region Register operands

        public const byte REG_EAX = 0x01;
        public const byte REG_EBX = 0x02;
        public const byte REG_ECX = 0x03;
        public const byte REG_EDX = 0x04;
        public const byte REG_ESI = 0x05;
        public const byte REG_EDI = 0x06;

        public const byte REG_AX = 0x07;
        public const byte REG_BX = 0x08;
        public const byte REG_CX = 0x09;
        public const byte REG_DX = 0x0A;

        public const byte REG_AH = 0x0B;
        public const byte REG_AL = 0x0C;
        public const byte REG_BH = 0x0D;
        public const byte REG_BL = 0x0E;
        public const byte REG_CH = 0x0F;
        public const byte REG_CL = 0x10;
        public const byte REG_DH = 0x11;
        public const byte REG_DL = 0x12;

        #endregion
    }
}
