/**
 * Oddmatics.Experiments.VM.Ram.Memory -- Experimental Virtual Machine RAM
 *
 * This source-code is part of the experimental virtual machine project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://github.com/rozniak/Experimental-VM>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;

namespace Oddmatics.Experiments.VM.Ram
{
    internal class Memory
    {
        private byte[] VirtualMemory;


        public Memory(uint toAllocate)
        {
            if (toAllocate < 1024)
                throw new ArgumentException("Memory.New: Must allocate at least 1KiB of RAM.");

            VirtualMemory = new byte[toAllocate];
        }


        public void Clear()
        {
            VirtualMemory = new byte[VirtualMemory.Length];
        }

        public uint FetchDWord(uint address)
        {
            if (address + 3 > VirtualMemory.Length)
                throw new IndexOutOfRangeException(String.Format("Memory.FetchDWord: Address {0:X} is outside of available virtal memory.", address));

            byte[] dword = new byte[4];
            Array.Copy(VirtualMemory, address, dword, 0, 4);
            Array.Reverse(dword);

            return BitConverter.ToUInt32(dword, 0);
        }

        public ushort FetchWord(uint address)
        {
            if (address + 1 > VirtualMemory.Length)
                throw new IndexOutOfRangeException(String.Format("Memory.FetchWord: Address {0:X} is outside of available virtal memory.", address));

            byte[] dword = new byte[2];
            Array.Copy(VirtualMemory, address, dword, 0, 2);

            return BitConverter.ToUInt16(dword, 0);
        }

        public byte FetchByte(uint address)
        {
            if (address > VirtualMemory.Length)
                throw new IndexOutOfRangeException(String.Format("Memory.FetchByte: Address {0:X} is outside of available virtal memory.", address));

            return VirtualMemory[address];
        }

        public void Load(byte[] data, uint address)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (address + i < VirtualMemory.Length)
                {
                    VirtualMemory[address + i] = data[i];
                }
                else
                    throw new InsufficientMemoryException("Memory.Load: Overflow when attempting to load data!");
            }
        }
    }
}
