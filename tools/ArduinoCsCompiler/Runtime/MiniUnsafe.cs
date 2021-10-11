﻿using System;
using Iot.Device.Arduino;

namespace ArduinoCsCompiler.Runtime
{
    [ArduinoReplacement("Internal.Runtime.CompilerServices.Unsafe", "System.Private.CoreLib.dll", true, IncludingPrivates = true)]
    internal unsafe class MiniUnsafe
    {
        /// <summary>
        /// This method just unsafely casts object to T. The underlying implementation just does a "return this" without any type test.
        /// The implementation of the following two methods is identical, therefore it doesn't really matter which one we match.
        /// </summary>
        [ArduinoImplementation(NativeMethod.UnsafeAs2)]
        public static T As<T>(object? value)
            where T : class?
        {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ret
        }

        [ArduinoImplementation(NativeMethod.UnsafeAs2)]
        public static ref TTo As<TFrom, TTo>(ref TFrom source)
        {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ret
        }

        [ArduinoImplementation(NativeMethod.UnsafeAsPointer)]
        public static void* AsPointer<T>(ref T value)
        {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // conv.u
            // ret
        }

        /// <summary>
        /// Determines the byte offset from origin to target from the given references.
        /// </summary>
        [ArduinoImplementation(NativeMethod.UnsafeByteOffset, CompareByParameterNames = true)]
        public static IntPtr ByteOffset<T>(ref T origin, ref T target)
        {
            throw new PlatformNotSupportedException();
        }

        [ArduinoImplementation(NativeMethod.UnsafeAreSame, CompareByParameterNames = true)]
        public static bool AreSame<T>(ref T left, ref T right)
        {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ldarg.1
            // ceq
            // ret
        }

        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static bool IsNullRef<T>(ref T source)
        {
            return AsPointer(ref source) == null;

            // ldarg.0
            // ldc.i4.0
            // conv.u
            // ceq
            // ret
        }

        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static ref T Add<T>(ref T source, int elementOffset)
        {
            return ref AddByteOffset(ref source, (IntPtr)(elementOffset * (int)SizeOf<T>()));
        }

        public static void* Add<T>(void* source, int elementOffset)
        {
            return (byte*)source + (elementOffset * (int)SizeOf<T>());
        }

        /// <summary>
        /// Adds an element offset to the given reference.
        /// </summary>
        public static ref T Add<T>(ref T source, IntPtr elementOffset)
        {
            return ref AddByteOffset(ref source, (IntPtr)((uint)elementOffset * (uint)SizeOf<T>()));
        }

        [ArduinoImplementation(NativeMethod.UnsafeAddByteOffset)]
        public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
        {
            // This method is implemented by the toolchain
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ldarg.1
            // add
            // ret
        }

        [ArduinoImplementation(NativeMethod.None)]
        public static ref T AddByteOffset<T>(ref T source, uint byteOffset)
        {
            return ref AddByteOffset(ref source, (IntPtr)(void*)byteOffset);
        }

        public static int SizeOf<T>()
        {
            return SizeOfType(typeof(T));
            // sizeof !!0
            // ret
        }

        [ArduinoImplementation(NativeMethod.UnsafeSizeOfType)]
        private static int SizeOfType(Type t)
        {
            throw new NotImplementedException();
        }

        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static ref T AsRef<T>(void* source)
        {
            return ref As<byte, T>(ref *(byte*)source);
        }

        public static ref T AsRef<T>(in T source)
        {
            throw new PlatformNotSupportedException();
        }

        [ArduinoImplementation(NativeMethod.UnsafeNullRef)]
        public static ref T NullRef<T>()
        {
            return ref AsRef<T>(null);

            // ldc.i4.0
            // conv.u
            // ret
        }

        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static T ReadUnaligned<T>(void* source)
        {
            return As<byte, T>(ref *(byte*)source);
        }

        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static T Read<T>(void* source)
        {
            return As<byte, T>(ref *(byte*)source);
        }

        /// <summary>
        /// Determines whether the memory address referenced by <paramref name="left"/> is greater than
        /// the memory address referenced by <paramref name="right"/>.
        /// </summary>
        /// <remarks>
        /// This check is conceptually similar to "(void*)(&amp;left) &gt; (void*)(&amp;right)".
        /// </remarks>
        [ArduinoImplementation(NativeMethod.UnsafeIsAddressGreaterThan, CompareByParameterNames = true)]
        public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
        {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ldarg.1
            // cgt.un
            // ret
        }

        /// <summary>
        /// Determines whether the memory address referenced by <paramref name="left"/> is less than
        /// the memory address referenced by <paramref name="right"/>.
        /// </summary>
        /// <remarks>
        /// This check is conceptually similar to "(void*)(&amp;left) &lt; (void*)(&amp;right)".
        /// </remarks>
        [ArduinoImplementation(NativeMethod.UnsafeIsAddressLessThan, CompareByParameterNames = true)]
        public static bool IsAddressLessThan<T>(ref T left, ref T right)
        {
            throw new PlatformNotSupportedException();

            // ldarg.0
            // ldarg.1
            // clt.un
            // ret
        }

        [ArduinoImplementation(NativeMethod.None)]
        public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
        {
            for (uint i = 0; i < byteCount; i++)
            {
                AddByteOffset(ref startAddress, i) = value;
            }
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static void WriteUnaligned<T>(void* destination, T value)
        {
            As<byte, T>(ref *(byte*)destination) = value;
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to the given location.
        /// </summary>
        [ArduinoImplementation(NativeMethod.None, CompareByParameterNames = true)]
        public static void WriteUnaligned<T>(ref byte destination, T value)
        {
            As<byte, T>(ref destination) = value;
        }

        public static T ReadUnaligned<T>(ref byte source)
        {
            return As<byte, T>(ref source);
        }

        [ArduinoImplementation(NativeMethod.UnsafeSkipInit)]
        public static void SkipInit<T>(out T value)
        {
            throw new PlatformNotSupportedException();
        }
    }
}
