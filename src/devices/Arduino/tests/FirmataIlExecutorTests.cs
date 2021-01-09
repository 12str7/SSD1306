﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace Iot.Device.Arduino.Tests
{
    public sealed class FirmataIlExecutorTests : IClassFixture<FirmataTestFixture>, IDisposable
    {
        private FirmataTestFixture _fixture;
        private ArduinoCsCompiler _compiler;

        public FirmataIlExecutorTests(FirmataTestFixture fixture)
        {
            _fixture = fixture;
            _compiler = new ArduinoCsCompiler(fixture.Board, true);
            _compiler.ClearAllData(true);
        }

        private Type[] TypesToSuppressForArithmeticTests
        {
            get
            {
                return new[]
                {
                    typeof(String), ArduinoCsCompiler.GetSystemPrivateType("System.SR"), ArduinoCsCompiler.GetSystemPrivateType("Iot.Device.Arduino.MiniCultureInfo")
                };
            }
        }

        public void Dispose()
        {
            _compiler.Dispose();
        }

        public static uint AddU(uint a, uint b)
        {
            return a + b;
        }

        public static uint SubU(uint a, uint b)
        {
            return a - b;
        }

        public static uint MulU(uint a, uint b)
        {
            return a * b;
        }

        public static uint DivU(uint a, uint b)
        {
            return a / b;
        }

        public static uint ModU(uint a, uint b)
        {
            return a % b;
        }

        public static uint AndU(uint a, uint b)
        {
            return a & b;
        }

        public static uint OrU(uint a, uint b)
        {
            return a | b;
        }

        public static uint NotU(uint a, uint b)
        {
            return ~a;
        }

        public static uint RshU(uint a, uint b)
        {
            return a >> (int)b;
        }

        public static uint XorU(uint a, uint b)
        {
            return a ^ b;
        }

        public static uint RshUnU(uint a, uint b)
        {
            return a >> (int)b;
        }

        public static bool EqualU(uint a, uint b)
        {
            return a == b;
        }

        public static bool UnequalU(uint a, uint b)
        {
            return (a != b);
        }

        public static bool SmallerU(uint a, uint b)
        {
            return a < b;
        }

        public static bool SmallerOrEqualU(uint a, uint b)
        {
            return a <= b;
        }

        public static bool GreaterU(uint a, uint b)
        {
            return a > b;
        }

        public static bool GreaterOrEqualU(uint a, uint b)
        {
            return a >= b;
        }

        public static bool GreaterThanConstantU(uint a, uint b)
        {
            return a > 2700;
        }

        public static int AddS(int a, int b)
        {
            return a + b;
        }

        public static int SubS(int a, int b)
        {
            return a - b;
        }

        public static int MulS(int a, int b)
        {
            return a * b;
        }

        public static int DivS(int a, int b)
        {
            return a / b;
        }

        public static int ModS(int a, int b)
        {
            return a % b;
        }

        public static float AddF(float a, float b)
        {
            return a + b;
        }

        public static float SubF(float a, float b)
        {
            return a - b;
        }

        public static float MulF(float a, float b)
        {
            return a * b;
        }

        public static float DivF(float a, float b)
        {
            return a / b;
        }

        public static float ModF(float a, float b)
        {
            return a % b;
        }

        public static double AddD(double a, double b)
        {
            return a + b;
        }

        public static double SubD(double a, double b)
        {
            return a - b;
        }

        public static double MulD(double a, double b)
        {
            return a * b;
        }

        public static double DivD(double a, double b)
        {
            return a / b;
        }

        public static double ModD(double a, double b)
        {
            return a % b;
        }

        public static int AndS(int a, int b)
        {
            return a & b;
        }

        public static int OrS(int a, int b)
        {
            return a | b;
        }

        public static int NotS(int a, int b)
        {
            return ~a;
        }

        public static int NegS(int a, int b)
        {
            return -a;
        }

        public static int LshS(int a, int b)
        {
            return a << b;
        }

        public static int RshS(int a, int b)
        {
            return a >> b;
        }

        public static int XorS(int a, int b)
        {
            return a ^ b;
        }

        public static int RshUnS(int a, int b)
        {
            return (int)((uint)a >> b);
        }

        public static bool Equal(int a, int b)
        {
            return a == b;
        }

        public static bool Unequal(int a, int b)
        {
            return (a != b);
        }

        public static bool SmallerS(int a, int b)
        {
            return a < b;
        }

        public static bool SmallerOrEqualS(int a, int b)
        {
            return a <= b;
        }

        public static bool GreaterS(int a, int b)
        {
            return a > b;
        }

        public static bool GreaterOrEqualS(int a, int b)
        {
            return a >= b;
        }

        public static bool GreaterThanConstantS(int a, int b)
        {
            return a > 2700;
        }

        /// <summary>
        /// Passes when inputValue1 is 1 and inputValue2 is 2
        /// </summary>
        /// <param name="inputValue1">Must be 1</param>
        /// <param name="inputValue2">Must be 2</param>
        /// <returns></returns>
        public static bool ComparisonOperators1(int inputValue1, int inputValue2)
        {
            if (inputValue2 >= 20)
            {
                return false;
            }

            if (inputValue1 > inputValue2)
            {
                return false;
            }

            if (inputValue2 != 2)
            {
                return false;
            }

            if (inputValue1 <= 0)
            {
                return false;
            }

            return true;
        }

        // inputValue1 = 100, inputValue2 = 200
        public static bool ComparisonOperators2(int inputValue1, int inputValue2)
        {
            int v = inputValue2;
            while (v-- > inputValue1)
            {
                inputValue2++;
            }

            if (v > inputValue2)
            {
                return false;
            }

            v = 1;
            for (int i = 0; i <= 10; i++)
            {
                v += i;
            }

            if (v < 10)
            {
                return false;
            }

            return true;
        }

        private void LoadCodeMethod<T1, T2, T3>(Type type, string methodName, T1 a, T2 b, T3 expectedResult, Type[]? suppressTypes)
        {
            var methods = type.GetMethods().Where(x => x.Name == methodName).ToList();
            Assert.Single(methods);
            var set = _compiler.CreateExecutionSet();
            set.SuppressTypes(suppressTypes);
            _compiler.PrepareLowLevelInterface(set);
            CancellationTokenSource cs = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var method = _compiler.AddSimpleMethod(set, methods[0]);

            // First execute the method locally, so we don't have an error in the test
            var uncastedResult = methods[0].Invoke(null, new object[] { a!, b! });
            if (uncastedResult == null)
            {
                throw new InvalidOperationException("Void methods not supported here");
            }

            T3 result = (T3)uncastedResult;
            Assert.Equal(expectedResult, result);

            // This assertion fails on a timeout
            Assert.True(method.Invoke(cs.Token, a!, b!));

            Assert.True(method.GetMethodResults(set, out object[] data, out MethodState state));

            // The task has terminated (do this after the above, otherwise the test will not show an exception)
            Assert.Equal(MethodState.Stopped, method.State);

            // The only result is from the end of the method
            Assert.Equal(MethodState.Stopped, state);
            Assert.Single(data);

            result = (T3)data[0];
            Assert.Equal(expectedResult, result);
            method.Dispose();
        }

        [Theory]
        [InlineData("Equal", 2, 2, true)]
        [InlineData("Equal", 2000, 1999, false)]
        [InlineData("Equal", -1, -1, true)]
        [InlineData("SmallerS", 1, 2, true)]
        [InlineData("SmallerOrEqualS", 7, 20, true)]
        [InlineData("SmallerOrEqualS", 7, 7, true)]
        [InlineData("SmallerOrEqualS", 21, 7, false)]
        [InlineData("GreaterS", 0x12345678, 0x12345677, true)]
        [InlineData("GreaterOrEqualS", 2, 2, true)]
        [InlineData("GreaterOrEqualS", 787878, 787877, true)]
        [InlineData("GreaterThanConstantS", 2701, 0, true)]
        [InlineData("ComparisonOperators1", 1, 2, true)]
        [InlineData("ComparisonOperators2", 100, 200, true)]
        [InlineData("SmallerS", -1, 1, true)]
        [InlineData("Unequal", 2, 2, false)]
        [InlineData("SmallerOrEqualS", -2, -1, true)]
        [InlineData("SmallerOrEqualS", -2, -2, true)]
        public void TestBooleanOperation(string methodName, int argument1, int argument2, bool expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("AddS", 10, 20, 30)]
        [InlineData("AddS", 10, -5, 5)]
        [InlineData("AddS", -5, -2, -7)]
        [InlineData("SubS", 10, 2, 8)]
        [InlineData("SubS", 10, -2, 12)]
        [InlineData("SubS", -2, -2, 0)]
        [InlineData("SubS", -4, 1, -5)]
        [InlineData("MulS", 4, 6, 24)]
        [InlineData("MulS", -2, -2, 4)]
        [InlineData("MulS", -2, 2, -4)]
        [InlineData("DivS", 10, 5, 2)]
        [InlineData("DivS", 10, 20, 0)]
        [InlineData("DivS", -10, 2, -5)]
        [InlineData("DivS", -10, -2, 5)]
        [InlineData("ModS", 10, 2, 0)]
        [InlineData("ModS", 11, 2, 1)]
        [InlineData("ModS", -11, 2, -1)]
        [InlineData("LshS", 8, 1, 16)]
        [InlineData("RshS", 8, 1, 4)]
        [InlineData("RshS", -8, 1, -4)]
        [InlineData("AndS", 0xF0, 0x1F, 0x10)]
        [InlineData("AndS", 0xF0, 0x00, 0x00)]
        [InlineData("OrS", 0xF0, 0x0F, 0xFF)]
        [InlineData("NotS", 0xF0, 0, -241)]
        [InlineData("NegS", -256, 0, 256)]
        [InlineData("XorS", 0x0F, 0x03, 12)]
        [InlineData("RshUnS", -8, 1, 2147483644)]
        public void TestArithmeticOperationSigned(string methodName, int argument1, int argument2, int expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("AddF", 10, 20, 30)]
        [InlineData("AddF", 10, -5, 5)]
        [InlineData("AddF", -5, -2, -7)]
        [InlineData("SubF", 10, 2, 8)]
        [InlineData("SubF", 10, -2, 12)]
        [InlineData("SubF", -2.0f, -2.0f, 0)]
        [InlineData("SubF", -4, 1, -5)]
        [InlineData("MulF", 4, 2.5f, 10.0)]
        [InlineData("MulF", -2, -2, 4)]
        [InlineData("MulF", -2, 2, -4)]
        [InlineData("DivF", 1.0f, 5.0f, 0.2f)]
        [InlineData("DivF", 10, 20, 0.5)]
        [InlineData("DivF", -10, 2, -5)]
        [InlineData("DivF", -10, -2, 5)]
        [InlineData("ModF", 10, 2, 0)]
        [InlineData("ModF", 11, 2, 1)]
        [InlineData("ModF", -11, 2, -1)]
        public void TestArithmeticOperationSignedFloat(string methodName, float argument1, float argument2, float expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("AddD", 10, 20.0, 30.0)]
        [InlineData("AddD", 10, -5, 5)]
        [InlineData("AddD", -5, -2, -7)]
        [InlineData("SubD", 10, 2, 8)]
        [InlineData("SubD", 10, -2, 12)]
        [InlineData("SubD", -2.0f, -2.0, 0)]
        [InlineData("SubD", -4, 1, -5)]
        [InlineData("MulD", 4, 2.5f, 10)]
        [InlineData("MulD", -2, -2, 4)]
        [InlineData("MulD", -2, 2, -4)]
        [InlineData("DivD", 1.0f, 5.0f, 0.2)]
        [InlineData("DivD", 10, 20, 0.5)]
        [InlineData("DivD", -10, 2, -5)]
        [InlineData("DivD", -10, -2, 5)]
        [InlineData("ModD", 10, 2, 0)]
        [InlineData("ModD", 11, 2, 1)]
        [InlineData("ModD", -11, 2, -1)]
        public void TestArithmeticOperationSignedDouble(string methodName, double argument1, double argument2, double expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("AddU", 10u, 20u, 30u)]
        [InlineData("AddU", 10u, -5u, 5u)]
        [InlineData("AddU", -5u, -2u, -7u)]
        [InlineData("SubU", 10u, 2u, 8u)]
        [InlineData("SubU", 10u, -2u, 12u)]
        [InlineData("SubU", -2u, -2u, 0u)]
        [InlineData("SubU", -4u, 1u, -5u)]
        [InlineData("MulU", 4u, 6u, 24u)]
        [InlineData("DivU", 10u, 5u, 2u)]
        [InlineData("DivU", 10u, 20u, 0u)]
        [InlineData("ModU", 10u, 2u, 0u)]
        [InlineData("ModU", 11u, 2u, 1u)]
        [InlineData("RshU", 8u, 1u, 4u)]
        [InlineData("RshU", -8u, 1u, 2147483644)]
        [InlineData("AndU", 0xF0u, 0x1Fu, 0x10u)]
        [InlineData("AndU", 0xF0u, 0x00u, 0x00u)]
        [InlineData("OrU", 0xF0u, 0x0Fu, 0xFFu)]
        [InlineData("NotU", 0xF0u, 0u, 0xFFFFFF0Fu)]
        [InlineData("XorU", 0x0Fu, 0x03u, 12)]
        [InlineData("RshUnU", -8u, 1, 0x7FFFFFFCu)]
        public void TestArithmeticOperationUnsigned(string methodName, Int64 argument1, Int64 argument2, Int64 expected)
        {
            // Method signature as above, otherwise the test data conversion fails
            LoadCodeMethod(GetType(), methodName, (uint)argument1, (uint)argument2, (uint)expected, TypesToSuppressForArithmeticTests);
        }

        public static Int32 ResultTypesTest(UInt32 argument1, Int32 argument2)
        {
            // Checks the result types of mixed-type arithmetic operations
            Int16 a = 10;
            UInt16 b = 20;
            Int32 result = (a + b);
            if (result != 30)
            {
                return 0;
            }

            Int32 a2 = (Int32)argument1;

            return a2 + argument2;
        }

        public static Int32 ResultTypesTest2(UInt32 argument1, Int32 argument2)
        {
            // Checks the result types of mixed-type arithmetic operations
            Int16 a = (Int16)argument1;
            Int32 b = argument2;
            Int32 result = (a + b);
            return result;
        }

        [Theory]
        [InlineData("ResultTypesTest", 50, 20, 70)]
        [InlineData("ResultTypesTest2", 21, -20, 1)]
        public void TestTypeConversions(string methodName, UInt32 argument1, int argument2, Int32 expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("IntArrayTest", 4, 1, 3)]
        [InlineData("IntArrayTest", 10, 2, 3)]
        [InlineData("CharArrayTest", 10, 2, 'C')]
        [InlineData("CharArrayTest", 10, 0, 'A')]
        [InlineData("ByteArrayTest", 10, 0, 255)]
        [InlineData("BoxedArrayTest", 5, 2, 7)]
        [InlineData("StaggedArrayTest", 5, 7, (int)'3')]
        public void ArrayTests(string methodName, Int32 argument1, Int32 argument2, Int32 expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("StructCtorBehaviorTest1", 5, 1, 2)]
        [InlineData("StructCtorBehaviorTest2", 5, 1, 5)]
        [InlineData("StructMethodCall1", 66, 33, -99)]
        [InlineData("StructMethodCall2", 66, 33, -66)]
        [InlineData("StructArray", 5, 2, 10)]
        [InlineData("StructInterfaceCall1", 10, 2, 8)]
        [InlineData("StructInterfaceCall2", 10, 2, 10)]
        [InlineData("StructInterfaceCall3", 15, 3, 12)]
        public void StructTests(string methodName, Int32 argument1, Int32 argument2, Int32 expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("LargeStructCtorBehaviorTest1", 5, 1, 4)]
        [InlineData("LargeStructCtorBehaviorTest2", 5, 1, 5)]
        [InlineData("LargeStructMethodCall2", 66, 33, 66)]
        [InlineData("LargeStructArray", 5, 1, 10)]
        public void LargeStructTest(string methodName, Int32 argument1, Int32 argument2, Int32 expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        [Theory]
        [InlineData("CastClassTest", 0, 0, 1)]
        public void CastTest(string methodName, Int32 argument1, Int32 argument2, Int32 expected)
        {
            LoadCodeMethod(GetType(), methodName, argument1, argument2, expected, TypesToSuppressForArithmeticTests);
        }

        public static int IntArrayTest(int size, int index)
        {
            int[] array = new int[size];
            array[index] = 3;
            array[array.Length - 1] = 2;
            return array[index];
        }

        public static int CharArrayTest(int size, int indexToRetrieve)
        {
            char[] array = new char[size];
            array[0] = 'A';
            array[1] = 'B';
            array[2] = 'C';
            return array[indexToRetrieve];
        }

        public static int ByteArrayTest(int size, int indexToRetrieve)
        {
            byte[] array = new byte[size];
            array[0] = 0xFF;
            array[1] = 1;
            array[3] = 2;
            return array[indexToRetrieve];
        }

        public static int BoxedArrayTest(int size, int indexToRetrieve)
        {
            object[] array = new object[size];
            array[0] = new object();
            array[1] = 2;
            array[2] = 7;
            return (int)array[indexToRetrieve];
        }

        public static int StaggedArrayTest(int size1, int size2)
        {
            char[][] staggedArray = new char[size1][];
            staggedArray[1] = new char[size2];
            staggedArray[1][1] = '3';
            return staggedArray[1][1];
        }

        public static int StructCtorBehaviorTest1(int size, int indexToRetrieve)
        {
            object[] array = new object[size];
            array[0] = new object();
            array[1] = new SmallStruct(2);
            array[2] = 7;

            SmallStruct t = (SmallStruct)array[indexToRetrieve];
            return (int)t.Ticks;
        }

        public static int StructCtorBehaviorTest2(int size, int indexToRetrieve)
        {
            SmallStruct s = default;
            s.Ticks = size;
            return (int)s.Ticks;
        }

        public static int StructMethodCall1(int arg1, int arg2)
        {
            SmallStruct s = default;
            s.Ticks = arg1;
            s.Add(arg2);
            var t = -s;
            return (int)t.Ticks;
        }

        public static int StructMethodCall2(int arg1, int arg2)
        {
            SmallStruct s = default;
            s.Ticks = arg1;
            s.Negate();
            return (int)s.Ticks;
        }

        public static int StructInterfaceCall1(int arg1, int arg2)
        {
            SmallStruct s = new SmallStruct(arg1);
            s.Subtract(arg2);
            return (int)s.Ticks;
        }

        public static int StructInterfaceCall2(int arg1, int arg2)
        {
            // Unlike the above, this one does not return the expected result, since it boxes an instance which is later discarded
            SmallStruct s = new SmallStruct(arg1);
            ISubtractable sub = s;
            sub.Subtract(arg2);
            return (int)s.Ticks;
        }

        public static int StructInterfaceCall3(int arg1, int arg2)
        {
            // This one works, though
            SmallStruct s = new SmallStruct(arg1);
            ISubtractable sub = s;
            sub.Subtract(arg2);
            // Unbox the changed instance
            SmallStruct s2 = (SmallStruct)sub;
            return (int)s2.Ticks;
        }

        public static int StructArray(int size, int indexToRetrieve)
        {
            SmallStruct a = new SmallStruct(2);
            SmallStruct[] array = new SmallStruct[size];
            array[0].Ticks = 5;
            array[1] = a;
            array[2] = new SmallStruct(10);

            a.Ticks = 27;
            if (array[1].Ticks == 27)
            {
                // This shouldn't happen (copying a to the array above should make a copy)
                throw new InvalidProgramException();
            }

            SmallStruct t = array[indexToRetrieve];
            return (int)t.Ticks;
        }

        public static int LargeStructCtorBehaviorTest1(int size, int indexToRetrieve)
        {
            object[] array = new object[size];
            array[0] = new object();
            array[1] = new LargeStruct(2, 3, 4);
            array[2] = 7;

            LargeStruct t = (LargeStruct)array[indexToRetrieve];
            if ((int)array[2] != 7)
            {
                throw new InvalidProgramException();
            }

            return (int)t.D;
        }

        public static int LargeStructCtorBehaviorTest2(int size, int indexToRetrieve)
        {
            LargeStruct s = default;
            s.D = size;
            return (int)s.D;
        }

        public static int LargeStructMethodCall2(int arg1, int arg2)
        {
            LargeStruct s = default;
            s.D = arg1;
            s.Sum();
            s.D = arg2;
            return s.TheSum;
        }

        public static int LargeStructArray(int size, int indexToRetrieve)
        {
            LargeStruct a = new LargeStruct(2, 10, -1);
            LargeStruct[] array = new LargeStruct[size];
            array[0].D = 5;
            array[1] = a;
            array[2] = new LargeStruct(11, 12, 13);

            a.D = 27;
            if (array[1].D == 27)
            {
                // This shouldn't happen (copying a to the array above should make a copy)
                throw new InvalidProgramException();
            }

            LargeStruct t = array[indexToRetrieve];
            return t.B;
        }

        public static int CastClassTest(int arg1, int arg2)
        {
            SmallBase s = new SmallBase(1);
            IDisposable d = s;

            SmallDerived derived = new SmallDerived(2);
            SmallBase s2 = derived;
            s2.Foo(10);
            MiniAssert.That(s2.A == 11);

            SmallDerived derived2 = (SmallDerived)s2;
            derived2.A = 12;
            MiniAssert.That(s2.A == 12);

            ICloneable c = derived2;
            ICloneable c2 = (ICloneable)c.Clone();
            SmallDerived derived3 = (SmallDerived)c2;
            MiniAssert.That(derived3.A == 12);

            d.Dispose();
            MiniAssert.That(s.A == -1);

            return 1;
        }

        private class SmallBase : IDisposable
        {
            private int _a;
            public SmallBase(int a)
            {
                _a = a;
            }

            public int A
            {
                get => _a;
                set => _a = value;
            }

            public virtual void Foo(int x)
            {
                _a = x;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _a = -1;
                }
                else
                {
                    _a = 2;
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }
        }

        private class SmallDerived : SmallBase, ICloneable
        {
            public SmallDerived(int a)
            : base(a)
            {
            }

            public override void Foo(int x)
            {
                A = x + 1;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                A = 5;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        public interface ISubtractable
        {
            void Subtract(int amount);
        }

        private struct SmallStruct : ISubtractable
        {
            private Int64 _ticks;

            public SmallStruct(Int64 ticks)
            {
                _ticks = ticks;
            }

            public Int64 Ticks
            {
                get
                {
                    return _ticks;
                }
                set
                {
                    _ticks = value;
                }
            }

            public void Add(int moreTicks)
            {
                _ticks += moreTicks;
            }

            public void Subtract(int ticks)
            {
                _ticks -= ticks;
            }

            public void Negate()
            {
                // stupid implementation, but test case (generates an LDOBJ instruction)
                SmallStruct s2 = -this;
                _ticks = s2.Ticks;
            }

            public static SmallStruct operator -(SmallStruct st)
            {
                return new SmallStruct(-st.Ticks);
            }
        }

        private struct LargeStruct
        {
            private int _a;
            private int _b;
            private long _d;
            private long _sum;

            public LargeStruct(int a, int b, long d)
            {
                _a = a;
                _b = b;
                _d = d;
                _sum = 0;
            }

            public int A => _a;

            public int B => _b;

            public long D
            {
                get
                {
                    return _d;
                }
                set
                {
                    _d = value;
                }
            }

            public int TheSum => (int)_sum;

            public void Sum()
            {
                _sum = _a + _b + _d;
            }
        }
    }
}
