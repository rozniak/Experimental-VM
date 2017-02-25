using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Oddmatics.Experiments.VM
{
    internal class Program
    {
        #region Output constants

        private const string MSG_INVALID_OPERATION = "Invalid operation given.";
        private const string MSG_INVALID_REGISTER = "Invalid register specified.";

        #endregion

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

        #region CPU register objects

        private static Register EaxRegister = new Register();
        private static Register EbxRegister = new Register();
        private static Register EcxRegister = new Register();
        private static Register EdxRegister = new Register();
        private static Register EsiRegister = new Register();
        private static Register EdiRegister = new Register();
        private static Register EspRegister = new Register();
        private static Register EbpRegister = new Register();

        #endregion

        #region CPU registers

        private static uint Eax { get { return EaxRegister.DWord; } set { EaxRegister.DWord = value; } }
        private static ushort Ax { get { return EaxRegister.Word; } set { EaxRegister.Word = value; } }
        private static byte Ah { get { return EaxRegister.LowByte; } set { EaxRegister.LowByte = value; } }
        private static byte Al { get { return EaxRegister.LowestByte; } set { EaxRegister.LowestByte = value; } }

        private static uint Ebx { get { return EbxRegister.DWord; } set { EbxRegister.DWord = value; } }
        private static ushort Bx { get { return EbxRegister.Word; } set { EbxRegister.Word = value; } }
        private static byte Bh { get { return EbxRegister.LowByte; } set { EbxRegister.LowByte = value; } }
        private static byte Bl { get { return EbxRegister.LowestByte; } set { EbxRegister.LowestByte = value; } }

        private static uint Ecx { get { return EcxRegister.DWord; } set { EcxRegister.DWord = value; } }
        private static ushort Cx { get { return EcxRegister.Word; } set { EcxRegister.Word = value; } }
        private static byte Ch { get { return EcxRegister.LowByte; } set { EcxRegister.LowByte = value; } }
        private static byte Cl { get { return EcxRegister.LowestByte; } set { EcxRegister.LowestByte = value; } }

        private static uint Edx { get { return EdxRegister.DWord; } set { EdxRegister.DWord = value; } }
        private static ushort Dx { get { return EdxRegister.Word; } set { EdxRegister.Word = value; } }
        private static byte Dh { get { return EdxRegister.LowByte; } set { EdxRegister.LowByte = value; } }
        private static byte Dl { get { return EdxRegister.LowestByte; } set { EdxRegister.LowestByte = value; } }

        private static uint Esi { get { return EsiRegister.DWord; } set { EsiRegister.DWord = value; } }
        private static uint Edi { get { return EdiRegister.DWord; } set { EdiRegister.DWord = value; } }

        #endregion

        private byte[] RAM = new byte[32]; // 32 bytes of RAM


        private static void Main(string[] args)
        {
            bool shouldExit = false;

            while (!shouldExit)
            {
                Console.Write("> ");

                string input = Console.ReadLine().ToLower();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    string[] cmdInput = input.Split(' ');

                    switch (cmdInput[0])
                    {
                            // CPU Instructions
                        case "mov":
                            CpuMoveOperation(input);
                            break;

                            // VM Commands
                        case ".showregisters":
                            VmShowRegisters();
                            break;

                        default:
                            Console.WriteLine(MSG_INVALID_OPERATION);
                            break;
                    }
                }
            }
        }

        private static void VmShowRegisters()
        {
            Console.WriteLine("EAX : 0x" + String.Format("{0:X}", Eax).PadLeft(4, '0'));
            Console.WriteLine("EBX : 0x" + String.Format("{0:X}", Ebx).PadLeft(4, '0'));
            Console.WriteLine("ECX : 0x" + String.Format("{0:X}", Ecx).PadLeft(4, '0'));
            Console.WriteLine("EDX : 0x" + String.Format("{0:X}", Edx).PadLeft(4, '0'));
            Console.WriteLine("ESI : 0x" + String.Format("{0:X}", Esi).PadLeft(4, '0'));
            Console.WriteLine("EDI : 0x" + String.Format("{0:X}", Edi).PadLeft(4, '0'));
        }

        private static void CpuMoveOperation(string op)
        {
            // Parse instruction
            if (MoveRegToRegRegex.IsMatch(op)) // mov <reg>, <reg>
            {
                // Get registers
                var operands = GetRegRegex.Matches(op);
                uint srcRegValue = GetRegisterValue(operands[1].ToString());

                SetRegisterValue(operands[0].ToString(), srcRegValue);

                Console.WriteLine(operands[0].ToString() + " <-- " + operands[1].ToString());
            }
            else if (MoveConstToRegRegex.IsMatch(op))
            {
                // Get const and register
                var constOp = GetConstRegex.Matches(op)[0].ToString();
                var regOp = GetRegRegex.Matches(op)[0].ToString();

                uint constant = ConvertConstDefToUInt(constOp); // substring off the initial '#' or '0x'

                SetRegisterValue(regOp, constant);

                Console.WriteLine(regOp +" <-- " + constOp);
            }
            else
                Console.WriteLine(MSG_INVALID_REGISTER);
        }

        private static uint ConvertConstDefToUInt(string def)
        {
            if (def.StartsWith("#"))
                return Convert.ToUInt32(def.Substring(1));
            else if (def.StartsWith("0x"))
                return Convert.ToUInt32(def.Substring(2), 16);

            throw new FormatException("ConvertConstDefToUInt: Constant format unrecognised.");
        }

        private static uint GetRegisterValue(string reg)
        {
            switch (reg.ToLower())
            {
                case "eax": return Eax;
                case "ax": return Ax;
                case "ah": return Ah;
                case "al": return Al;

                case "ebx": return Ebx;
                case "bx": return Bx;
                case "bh": return Bh;
                case "bl": return Bl;

                case "ecx": return Ecx;
                case "cx": return Cx;
                case "ch": return Ch;
                case "cl": return Cl;

                case "edx": return Edx;
                case "dx": return Dx;
                case "dh": return Dh;
                case "dl": return Dl;

                case "esi": return Esi;
                case "edi": return Edi;

                default: throw new ArgumentException("StringToRegister: " + reg + " was not recognised as the name of a valid register.");
            }
        }

        private static void SetRegisterValue(string reg, uint value)
        {
            switch (reg.ToLower())
            {
                case "eax": Eax = value; break;
                case "ax": Ax = (ushort)value; break;
                case "ah": Ah = (byte)value; break;
                case "al": Al = (byte)value; break;

                case "ebx": Ebx = value; break;
                case "bx": Bx = (ushort)value; break;
                case "bh": Bh = (byte)value; break;
                case "bl": Bl = (byte)value; break;

                case "ecx": Ecx = value; break;
                case "cx": Cx = (ushort)value; break;
                case "ch": Ch = (byte)value; break;
                case "cl": Cl = (byte)value; break;

                case "edx": Edx = value; break;
                case "dx": Dx = (ushort)value; break;
                case "dh": Dh = (byte)value; break;
                case "dl": Dl = (byte)value; break;

                case "esi": Esi = value; break;
                case "edi": Edi = value; break;

                default: throw new ArgumentException("StringToRegister: " + reg + " was not recognised as the name of a valid register.");
            }
        }
    }
}
