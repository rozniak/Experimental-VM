

namespace Oddmatics.Experiments.VM
{
    public class Register
    {
        /// <summary>
        /// Gets or sets the full 4 bytes stored in this register.
        /// </summary>
        public uint DWord { get { return Data; } set { Data = value; } }

        /// <summary>
        /// Gets or sets the least significant 2 bytes stored in this register.
        /// </summary>
        public ushort Word
        {
            get { return (ushort)Data; }
            set { Data = (Data & 0xFF00) + value; }
        }

        /// <summary>
        /// Gets or sets the second least significant byte stored in this register.
        /// </summary>
        public byte LowByte
        {
            get { return (byte)(Data >> 8); }
            set { Data = (uint)((Data & 0xFF0F) + ((ushort)value << 8)); }
        }

        /// <summary>
        /// Gets or sets the least significant byte stored in this register.
        /// </summary>
        public byte LowestByte
        {
            get { return (byte)Data; }
            set { Data = (Data & 0xFFF0) + (ushort)value << 8; }
        }


        /// <summary>
        /// The data stored in this register.
        /// </summary>
        private uint Data;
    }
}
