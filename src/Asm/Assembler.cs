/**
 * Oddmatics.Experiments.VM.Asm.Assembler -- Experimental Virtual Machine Assembler
 *
 * This source-code is part of the experimental virtual machine project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://github.com/rozniak/Experimental-VM>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.Experiments.VM.Cpu;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Oddmatics.Experiments.VM.Asm
{
    internal static partial class Assembler
    {
        #region Syntax matches

        private static readonly Regex GetRegRegex = new Regex("eax|ax|ah|al|ebx|bx|bh|bl|ecx|cx|ch|cl|edx|dx|dh|dl|esi|edi");
        private static readonly Regex GetMemRegex = new Regex(@"\[((" + GetRegRegex + @")|(0x[0-9A-Fa-f]+))\]");
        private static readonly Regex GetConstRegex = new Regex("(#[0-9]+|0x[A-Fa-f0-9]+)");

        private static readonly Regex MoveRegToRegRegex = new Regex("^mov (" + GetRegRegex + "), (" + GetRegRegex + ")$");
        private static readonly Regex MoveMemToRegRegex = new Regex("^mov (" + GetMemRegex + "), (" + GetRegRegex + ")$");
        private static readonly Regex MoveRegToMemRegex = new Regex("^mov (" + GetRegRegex + "), (" + GetMemRegex + ")$");
        private static readonly Regex MoveConstToRegRegex = new Regex("^mov (" + GetRegRegex + "), (" + GetConstRegex + ")$");
        private static readonly Regex MoveConstToMemRegex = new Regex("^mov (" + GetMemRegex + "), (" + GetConstRegex + ")$");

        #endregion


        public static byte[] Assemble(string[] source)
        {
            var machineCode = new List<byte>();

            foreach (string line in source)
            {
                string[] lineInput = line.Split(' ');

                switch (lineInput[0])
                {
                    case "mov": machineCode.AddRange(AssembleMoveInstruction(line)); break;

                    default: throw new ArgumentException("Assembler.Assemble: Unrecognisable instruction '" + line + "'.");
                }
            }

            return machineCode.ToArray();
        }

        private static byte[] AssembleMoveInstruction(string instruction)
        {
            if (MoveRegToRegRegex.IsMatch(instruction)) // mov <reg>, <reg>
            {
                // Get registers
                var operands = GetRegRegex.Matches(instruction);
                byte sourceRegister = GetRegOperand(operands[1].ToString());
                byte targetRegister = GetRegOperand(operands[0].ToString());

                return new byte[] { MachineLanguage.MOV_REG_REG, 0x00,
                        sourceRegister, targetRegister };
                
            }

            throw new FormatException("Assembler.AssembleMoveInstruction: Failed to parse mov instruction '" +
                instruction + "'.");
        }
    }
}
