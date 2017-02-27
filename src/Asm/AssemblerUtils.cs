/**
 * Oddmatics.Experiments.VM.Asm.AssemblerUtils -- Experimental Virtual Machine Assembler Working Functions
 *
 * This source-code is part of the experimental virtual machine project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://github.com/rozniak/Experimental-VM>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.Experiments.VM.Cpu;
using System;

namespace Oddmatics.Experiments.VM.Asm
{
    internal static partial class Assembler
    {
        private static uint ConvertConstDefToUInt(string def)
        {
            if (def.StartsWith("#"))
                return Convert.ToUInt32(def.Substring(1));
            else if (def.StartsWith("0x"))
                return Convert.ToUInt32(def.Substring(2), 16);

            throw new FormatException("ConvertConstDefToUInt: Constant format unrecognised.");
        }

        private static byte GetRegOperand(string register)
        {
            switch (register)
            {
                case "eax": return MachineLanguage.REG_EAX;
                case "ax": return MachineLanguage.REG_AX;
                case "ah": return MachineLanguage.REG_AH;
                case "al": return MachineLanguage.REG_AL;

                case "ebx": return MachineLanguage.REG_EBX;
                case "bx": return MachineLanguage.REG_BX;
                case "bh": return MachineLanguage.REG_BH;
                case "bl": return MachineLanguage.REG_BL;

                case "ecx": return MachineLanguage.REG_ECX;
                case "cx": return MachineLanguage.REG_CX;
                case "ch": return MachineLanguage.REG_CH;
                case "cl": return MachineLanguage.REG_CL;

                case "edx": return MachineLanguage.REG_EDX;
                case "dx": return MachineLanguage.REG_DX;
                case "dh": return MachineLanguage.REG_DH;
                case "dl": return MachineLanguage.REG_DL;

                case "esi": return MachineLanguage.REG_ESI;
                case "edi": return MachineLanguage.REG_EDI;

                default: throw new ArgumentException("Assembler.GetRegOperand: Unrecognised register '" + register + "'.");
            }
        }

        private static byte GetRegSize(byte register)
        {
            if (register >= 0x01 && register <= 0x06)
                return 32;
            else if (register >= 0x07 && register <= 0x0A)
                return 16;
            else if (register >= 0x0B && register <= 0x12)
                return 8;

            throw new ArgumentException("Assembler.GetRegOperand: Unrecognised register " + register.ToString() + ".");
        }
    }
}
