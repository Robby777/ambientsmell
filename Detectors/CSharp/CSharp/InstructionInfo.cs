using System;
using System.Collections;
using System.Text;

using Mono.Cecil.Cil;
using Mono.Cecil;

namespace CSharp
{

    // Eventually the contents of this class should be put into the Mono.Cecil.Cil.Instruction class.

    class ReturnInfo : InstructionInfo
    {
        protected override int _Pops(Instruction i, int stackSize)
        {
            // Check i.Operand?
            if (stackSize == 1)
                return 1;
            return 0;
        }

        protected override int _Pushes(Instruction i)
        {
            return 0;
        }
    }
    class NewObjInfo : InstructionInfo
    {
        protected override int _Pops(Instruction instruction, int stackSize)
        {
            MethodReference method = (MethodReference)instruction.Operand;
            return method.Parameters.Count;
        }

        protected override int _Pushes(Instruction i)
        {
            return 1;
        }
    }
    class LeaveInfo : InstructionInfo
    {
        protected override int _Pops(Instruction i, int stackSize)
        {
            return stackSize;
        }

        protected override int _Pushes(Instruction i)
        {
            return 0;
        }
    }
    class CallInstructionInfo : InstructionInfo
    {
        protected override int _Pops(Instruction instruction, int stackSize)
        {
            MethodReference method = (MethodReference)instruction.Operand;
            int pop = method.Parameters.Count;
            pop += method.HasThis ? 1 : 0;
            pop += instruction.OpCode == OpCodes.Calli ? 1 : 0;
            return pop;
        }

        protected override int _Pushes(Instruction i)
        {
            MethodReference method = (MethodReference)i.Operand;
            if (method.ReturnType == null)
                return 0;
            return 1;
        }
    }

    class InstructionInfo
    {
        protected int _pops;
        protected int _pushes;

        protected InstructionInfo() { }

        public InstructionInfo(int pops, int pushes)
        {
            _pops = pops;
            _pushes = pushes;
        }
        static Hashtable InstructionMap;

        static InstructionInfo Find(OpCode code)
        {
            #region OpCode Push/Pop values.
            if (OpCodes.Add == code) return new InstructionInfo(2, 1);
            if (OpCodes.Add_Ovf == code) return new InstructionInfo(2, 1);
            if (OpCodes.Add_Ovf_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.And == code) return new InstructionInfo(2, 1);
            if (OpCodes.Arglist == code) return new InstructionInfo(0, 1);
            if (OpCodes.Beq == code) return new InstructionInfo(2, 0);
            if (OpCodes.Beq_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bge == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bge_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bge_Un == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bge_Un_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bgt == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bgt_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bgt_Un == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bgt_Un_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Ble == code) return new InstructionInfo(2, 0);
            if (OpCodes.Ble_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Ble_Un == code) return new InstructionInfo(2, 0);
            if (OpCodes.Ble_Un_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Blt == code) return new InstructionInfo(2, 0);
            if (OpCodes.Blt_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Blt_Un == code) return new InstructionInfo(2, 0);
            if (OpCodes.Blt_Un_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bne_Un == code) return new InstructionInfo(2, 0);
            if (OpCodes.Bne_Un_S == code) return new InstructionInfo(2, 0);
            if (OpCodes.Box == code) return new InstructionInfo(1, 1);
            if (OpCodes.Br == code) return new InstructionInfo(0, 0);
            if (OpCodes.Br_S == code) return new InstructionInfo(0, 0);
            if (OpCodes.Break == code) return new InstructionInfo(0, 0);
            if (OpCodes.Brfalse == code) return new InstructionInfo(1, 0);
            if (OpCodes.Brfalse_S == code) return new InstructionInfo(1, 0);
            if (OpCodes.Brtrue == code) return new InstructionInfo(1, 0);
            if (OpCodes.Brtrue_S == code) return new InstructionInfo(1, 0);
            if (OpCodes.Call == code) return new CallInstructionInfo();
            if (OpCodes.Calli == code) return new CallInstructionInfo();
            if (OpCodes.Callvirt == code) return new CallInstructionInfo();
            if (OpCodes.Castclass == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ceq == code) return new InstructionInfo(2, 1);
            if (OpCodes.Cgt == code) return new InstructionInfo(2, 1);
            if (OpCodes.Cgt_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ckfinite == code) return new InstructionInfo(0, 0);
            if (OpCodes.Clt == code) return new InstructionInfo(2, 1);
            if (OpCodes.Clt_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Conv_I == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_I1 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_I2 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_I4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_I8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I1 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I1_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I2 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I2_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I4_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_I8_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U1 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U1_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U2 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U2_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U4_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_Ovf_U8_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_R_Un == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_R4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_R8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_U == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_U1 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_U2 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_U4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Conv_U8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Cpblk == code) return new InstructionInfo(3, 0);
            if (OpCodes.Cpobj == code) return new InstructionInfo(2, 0);
            if (OpCodes.Div == code) return new InstructionInfo(2, 1);
            if (OpCodes.Div_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Dup == code) return new InstructionInfo(0, 1);
            if (OpCodes.Endfilter == code) return new InstructionInfo(1, 0);
            if (OpCodes.Endfinally == code) return new InstructionInfo(0, 0);
            if (OpCodes.Initblk == code) return new InstructionInfo(3, 0);
            if (OpCodes.Initobj == code) return new InstructionInfo(1, 0);
            if (OpCodes.Isinst == code) return new InstructionInfo(1, 1);
            if (OpCodes.Jmp == code) return new InstructionInfo(0, 0);
            if (OpCodes.Ldarg == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarg_0 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarg_1 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarg_2 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarg_3 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarg_S == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarga == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldarga_S == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_0 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_1 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_2 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_3 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_4 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_5 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_6 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_7 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_8 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_M1 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I4_S == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_I8 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_R4 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldc_R8 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldelem_I == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_I1 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_I2 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_I4 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_I8 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_R4 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_R8 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_Ref == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_U1 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_U2 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelem_U4 == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldelema == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ldfld == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldflda == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldftn == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldind_I == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_I1 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_I2 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_I4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_I8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_R4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_R8 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_Ref == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_U1 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_U2 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldind_U4 == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldlen == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldloc == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloc_0 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloc_1 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloc_2 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloc_3 == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloc_S == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloca == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldloca_S == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldnull == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldobj == code) return new InstructionInfo(1, 1);
            if (OpCodes.Ldsfld == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldsflda == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldstr == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldtoken == code) return new InstructionInfo(0, 1);
            if (OpCodes.Ldvirtftn == code) return new InstructionInfo(1, 1);
            if (OpCodes.Leave == code) return new LeaveInfo();
            if (OpCodes.Leave_S == code) return new LeaveInfo();
            if (OpCodes.Localloc == code) return new InstructionInfo(1, 1);
            if (OpCodes.Mkrefany == code) return new InstructionInfo(1, 1);
            if (OpCodes.Mul == code) return new InstructionInfo(2, 1);
            if (OpCodes.Mul_Ovf == code) return new InstructionInfo(2, 1);
            if (OpCodes.Mul_Ovf_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Neg == code) return new InstructionInfo(1, 1);
            if (OpCodes.Newarr == code) return new InstructionInfo(1, 1);
            if (OpCodes.Newobj == code) return new NewObjInfo();
            if (OpCodes.Nop == code) return new InstructionInfo(0, 0);
            if (OpCodes.Not == code) return new InstructionInfo(1, 1);
            if (OpCodes.Or == code) return new InstructionInfo(2, 1);
            if (OpCodes.Pop == code) return new InstructionInfo(1, 0);
            if (OpCodes.Refanytype == code) return new InstructionInfo(1, 1);
            if (OpCodes.Refanyval == code) return new InstructionInfo(1, 1);
            if (OpCodes.Rem == code) return new InstructionInfo(2, 1);
            if (OpCodes.Rem_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Ret == code) return new ReturnInfo();
            if (OpCodes.Rethrow == code) return new InstructionInfo(0, 0);
            if (OpCodes.Shl == code) return new InstructionInfo(2, 1);
            if (OpCodes.Shr == code) return new InstructionInfo(2, 1);
            if (OpCodes.Shr_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Sizeof == code) return new InstructionInfo(0, 1);
            if (OpCodes.Starg == code) return new InstructionInfo(1, 0);
            if (OpCodes.Starg_S == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stelem_I == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_I1 == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_I2 == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_I4 == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_I8 == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_R4 == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_R8 == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stelem_Ref == code) return new InstructionInfo(3, 0);
            if (OpCodes.Stfld == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_I == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_I1 == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_I2 == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_I4 == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_I8 == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_R4 == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_R8 == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stind_Ref == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stloc == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stloc_0 == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stloc_1 == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stloc_2 == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stloc_3 == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stloc_S == code) return new InstructionInfo(1, 0);
            if (OpCodes.Stobj == code) return new InstructionInfo(2, 0);
            if (OpCodes.Stsfld == code) return new InstructionInfo(1, 0);
            if (OpCodes.Sub == code) return new InstructionInfo(2, 1);
            if (OpCodes.Sub_Ovf == code) return new InstructionInfo(2, 1);
            if (OpCodes.Sub_Ovf_Un == code) return new InstructionInfo(2, 1);
            if (OpCodes.Switch == code) return new InstructionInfo(1, 0);
            if (OpCodes.Throw == code) return new InstructionInfo(1, 0);
            if (OpCodes.Unbox == code) return new InstructionInfo(1, 1);
            if (OpCodes.Unbox_Any == code) return new InstructionInfo(1, 1);
            if (OpCodes.Xor == code) return new InstructionInfo(2, 1);
            #endregion
            return null;
        }

        static void BuildMap()
        {
            InstructionMap = new Hashtable();
            
            foreach( Code code in Enum.GetValues( typeof(Code) ) )
            {
                OpCode opcode = OpCodes.GetOpCode(code);
                InstructionInfo info = Find(opcode);
                InstructionMap[opcode] = info;
            }
        }

        protected virtual int _Pops(Instruction i, int stackSize)
        {
            return _pops;
        }

        protected virtual int _Pushes(Instruction i)
        {
            return _pushes;
        }

        public static int Pops(Instruction i, int stackSize)
        {
            if (InstructionMap == null)
            {
                BuildMap();
            }
            return ((InstructionInfo)InstructionMap[i.OpCode])._Pops(i, stackSize);
        }
        public static bool Pushes(Instruction i)
        {
            if (InstructionMap == null)
            {
                BuildMap();
            }
            return ((InstructionInfo)InstructionMap[i.OpCode])._Pushes(i) == 1;
        }
    }
}
