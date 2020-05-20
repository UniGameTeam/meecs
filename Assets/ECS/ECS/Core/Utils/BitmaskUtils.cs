﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BitMasks {

    using FieldType = UInt64;

    public struct BitMask : IEquatable<BitMask> {

        public static readonly BitMask None = new BitMask(new int[0]);
        
        private const int FIELD_COUNT = 4;
        private const int BITS_PER_FIELD = 8 * sizeof(FieldType);
        private const int MAX_BIT_INDEX = BitMask.FIELD_COUNT * BitMask.BITS_PER_FIELD - 1;
        //public const int BitSize = BitMask.FIELD_COUNT * BitMask.BITS_PER_FIELD;
        
        private FieldType field0;
        private FieldType field1;
        private FieldType field2;
        private FieldType field3;

        public BitMask(int[] bits) {
            
            this.field0 = 0;
            this.field1 = 0;
            this.field2 = 0;
            this.field3 = 0;

            for (var i = 0; i < bits.Length; ++i) {
                
                ref var bit = ref bits[i];
                this.AddBit(in bit);

            }
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasBit(in int bit) {
            
            if (bit < 0 || bit > BitMask.MAX_BIT_INDEX) {
                throw new Exception($"Attempted to set bit #{bit}, but the maximum is {BitMask.MAX_BIT_INDEX}");
            }

            var dataIndex = bit / BitMask.BITS_PER_FIELD;
            var bitIndex = bit % BitMask.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;
            switch (dataIndex) {
                case 0: return (this.field0 & mask) != 0;
                case 1: return (this.field1 & mask) != 0;
                case 2: return (this.field2 & mask) != 0;
                case 3: return (this.field3 & mask) != 0;
                
                default:
                    throw new Exception($"Nonexistent field: {dataIndex}");
            }

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void AddBit(in int bit) {
            
            if (bit < 0 || bit > BitMask.MAX_BIT_INDEX) {
                throw new Exception($"Attempted to set bit #{bit}, but the maximum is {BitMask.MAX_BIT_INDEX}");
            }

            var dataIndex = bit / BitMask.BITS_PER_FIELD;
            var bitIndex = bit % BitMask.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;
            switch (dataIndex) {
                case 0:
                    this.field0 |= mask;
                    break;

                case 1:
                    this.field1 |= mask;
                    break;

                case 2:
                    this.field2 |= mask;
                    break;

                case 3:
                    this.field3 |= mask;
                    break;

                default:
                    throw new Exception($"Nonexistent field: {dataIndex}");
            }
            
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SubtractBit(in int bit) {
            
            if (bit < 0 || bit > BitMask.MAX_BIT_INDEX) {
                throw new Exception($"Attempted to set bit #{bit}, but the maximum is {BitMask.MAX_BIT_INDEX}");
            }

            var dataIndex = bit / BitMask.BITS_PER_FIELD;
            var bitIndex = bit % BitMask.BITS_PER_FIELD;
            var mask = (FieldType)1 << bitIndex;
            switch (dataIndex) {
                case 0:
                    this.field0 &= ~mask;
                    break;

                case 1:
                    this.field1 &= ~mask;
                    break;

                case 2:
                    this.field2 &= ~mask;
                    break;

                case 3:
                    this.field3 &= ~mask;
                    break;

                default:
                    throw new Exception($"Nonexistent field: {dataIndex}");
            }
            
        }

        public bool this[int index] {
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get {
                if (index < 0 || index > BitMask.MAX_BIT_INDEX) {
                    throw new Exception($"Invalid bit index: {index}");
                }

                var dataIndex = index / BitMask.BITS_PER_FIELD;
                var bitIndex = index % BitMask.BITS_PER_FIELD;
                switch (dataIndex) {
                    case 0:
                        return (this.field0 & ((FieldType)1 << bitIndex)) != 0;

                    case 1:
                        return (this.field1 & ((FieldType)1 << bitIndex)) != 0;

                    case 2:
                        return (this.field2 & ((FieldType)1 << bitIndex)) != 0;

                    case 3:
                        return (this.field3 & ((FieldType)1 << bitIndex)) != 0;

                    default:
                        return false;
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {
            
            return (int)this.field0 ^ (int)this.field1 ^ (int)this.field2 ^ (int)this.field3;
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(BitMask other) {
            
            if (this.field0 != other.field0) {
                return false;
            }

            if (this.field1 != other.field1) {
                return false;
            }

            if (this.field2 != other.field2) {
                return false;
            }

            if (this.field3 != other.field3) {
                return false;
            }

            return true;
        }

        public override bool Equals(object obj) {
            if (obj is BitMask) {
                return this.Equals((BitMask)obj);
            }

            return base.Equals(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(BitMask mask1, BitMask mask2) {
            return mask1.Equals(mask2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(BitMask mask1, BitMask mask2) {
            return !mask1.Equals(mask2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitMask operator &(BitMask mask1, BitMask mask2) {
            
            var newBitMask = new BitMask();
            newBitMask.field0 = mask1.field0 & mask2.field0;
            newBitMask.field1 = mask1.field1 & mask2.field1;
            newBitMask.field2 = mask1.field2 & mask2.field2;
            newBitMask.field3 = mask1.field3 & mask2.field3;
            return newBitMask;
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitMask operator |(BitMask mask1, BitMask mask2) {
            
            var newBitMask = new BitMask();
            newBitMask.field0 = mask1.field0 | mask2.field0;
            newBitMask.field1 = mask1.field1 | mask2.field1;
            newBitMask.field2 = mask1.field2 | mask2.field2;
            newBitMask.field3 = mask1.field3 | mask2.field3;
            return newBitMask;
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitMask operator ~(BitMask mask) {
            var newBitMask = new BitMask();
            newBitMask.field0 = ~mask.field0;
            newBitMask.field1 = ~mask.field1;
            newBitMask.field2 = ~mask.field2;
            newBitMask.field3 = ~mask.field3;
            return newBitMask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(BitMask mask) {
            
            if ((this.field0 & mask.field0) != mask.field0) {
                return false;
            }

            if ((this.field1 & mask.field1) != mask.field1) {
                return false;
            }

            if ((this.field2 & mask.field2) != mask.field2) {
                return false;
            }

            if ((this.field3 & mask.field3) != mask.field3) {
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasNot(BitMask mask) {
            
            if ((this.field0 & mask.field0) != 0) {
                return false;
            }

            if ((this.field1 & mask.field1) != 0) {
                return false;
            }

            if ((this.field2 & mask.field2) != 0) {
                return false;
            }

            if ((this.field3 & mask.field3) != 0) {
                return false;
            }

            return true;
        }

        public override string ToString() {
            var builder = new StringBuilder();
            var fields = new FieldType[BitMask.FIELD_COUNT];
            fields[0] = this.field0;
            fields[1] = this.field1;
            fields[2] = this.field2;
            fields[3] = this.field3;
            for (var i = 0; i < BitMask.FIELD_COUNT; ++i) {
                var binaryString = Convert.ToString((long)fields[i], 2);
                builder.Append(binaryString.PadLeft(BitMask.BITS_PER_FIELD, '0'));
            }

            return builder.ToString();
        }

    }

}