/**
 * Oddmatics.Experiments.VM.Program -- Experimental Virtual Machine Entry-Point
 *
 * This source-code is part of the experimental virtual machine project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://github.com/rozniak/Experimental-VM>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.Experiments.VM.Asm;
using Oddmatics.Experiments.VM.Cpu;
using Oddmatics.Experiments.VM.Ram;
using System;

namespace Oddmatics.Experiments.VM
{
    internal class Program
    {
        #region VM elements

        private static Processor Cpu;
        private static Memory Ram;

        #endregion


        private static void Main(string[] args)
        {
            bool shouldExit = false;
            
            Ram = new Memory(2048);
            Cpu = new Processor(ref Ram); // TODO: Add clock speed (?)

            while (!shouldExit)
            {
                Console.Write("> ");

                string input = Console.ReadLine().ToLower();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.StartsWith(".")) // Interpret .command as internal debugging command
                    {
                        switch (input)
                        {
                            case ".showreg":
                                VmShowRegisters();
                                break;
                        }
                    }
                    else
                    {
                        try
                        {
                            byte[] machineCode = Assembler.Assemble(new string[] { input });

                            // Force load this into RAM for now - may zero out the op later
                            Ram.Load(machineCode, Cpu.Ip);
                            Cpu.Execute((uint)(machineCode.Length / 4));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private static void VmShowRegisters()
        {
            Console.WriteLine("EAX : 0x" + String.Format("{0:X}", Cpu.Eax).PadLeft(8, '0'));
            Console.WriteLine("EBX : 0x" + String.Format("{0:X}", Cpu.Ebx).PadLeft(8, '0'));
            Console.WriteLine("ECX : 0x" + String.Format("{0:X}", Cpu.Ecx).PadLeft(8, '0'));
            Console.WriteLine("EDX : 0x" + String.Format("{0:X}", Cpu.Edx).PadLeft(8, '0'));
            Console.WriteLine("ESI : 0x" + String.Format("{0:X}", Cpu.Esi).PadLeft(8, '0'));
            Console.WriteLine("EDI : 0x" + String.Format("{0:X}", Cpu.Edi).PadLeft(8, '0'));
        }


        // Old code - will be moved
        
    }
}
