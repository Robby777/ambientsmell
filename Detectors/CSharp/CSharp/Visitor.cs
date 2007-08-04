using System;
using System.Collections;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Cecil.FlowAnalysis;
using Cecil.FlowAnalysis.ActionFlow;
using Cecil.FlowAnalysis.ControlFlow;
using Cecil.FlowAnalysis.CodeStructure;

namespace CSharp
{
    public abstract class IVisitor
    {
        public abstract void VisitFiles(ICollection files);
    }

    public class AssemblyVisitor : IVisitor
    {
        public override void VisitFiles(ICollection files)
        {
            foreach (string file in files)
            {
                VisitFile(file);
            }
        }

        public void VisitFile(string fileName)
        {
            AssemblyDefinition targetAssembly = null;
            try
            {
                targetAssembly = AssemblyFactory.GetAssembly(fileName);
            }
            catch
            {
                return;
            }
            foreach (ModuleDefinition md in targetAssembly.Modules)
            {
                foreach (TypeDefinition td in md.Types)
                {
                    VisitTypes(td);
                }
            }
        }

        public void VisitTypes(TypeDefinition td)
        {
            foreach (TypeDefinition inner in td.NestedTypes)
            {
                VisitTypes(inner);
            }
            foreach (MethodDefinition methd in td.Methods)
            {
                VisitMethodBody(methd);
            }
        }

        public void VisitMethodBody(MethodDefinition methd)
        {
            if (methd.Body == null)
            {
                return;
            }

            //IActionFlowGraph afg = FlowGraphFactory.CreateActionFlowGraph(cfg);
            PopTreeGraph graph = new PopTreeGraphBuilder().Build(methd);
            foreach (PopTreeList block in graph.PopTreeBlocks)
            {
                IInstructionBlock iblock = block.Block;
                foreach (PopTree tree in block.PopTrees)
                {
                    //tree._PopList
                    //tree._instruction
                    string message = "";
                    if ( (message=VisitPopTree(tree,methd.Name)) != null)
                    {
                        Console.WriteLine( message );
                    }
                }
            }
        }

        string VisitPopTree( PopTree parent, string name )
        {
            // support elements?  foo().Field[i].method()
            if (name != "Test1")
                return null;
            int d = MaxChain( parent );
            if( d > 5 )
            {
                return name + ":" + d; 
            }

            return null;
        }

        int MaxChain(PopTree tree)
        {
            if (tree._PopList.Count == 0)
                return 0;
            int max = 0;
            foreach (PopTree t in tree._PopList)
            {
                //int d = t.Depth;
                ArrayList tops = t.FilteredTops(OpCodes.Call, OpCodes.Calli, OpCodes.Callvirt, OpCodes.Ldfld,
                             OpCodes.Ldsfld);
                if (tops.Count > max)
                    max = tops.Count;
            }
            return max + 1;
        }

        // CODE:
        //  m1( a1() ).m2( a2() ).m3( a3() )
        
        // CIL:
        // ld this  ; push <this>
        // dup      ; push <this>
        // call a1  ; pop <this> -> push <value>
        // call m1  ; pop <this> pop<value> -> push<obj>
        // ld this  ; push <this>
        // call a2  ; pop <this> -> push <value2>
        // call m2  ; pop <obj> pop<value2> -> push<obj2>
        // ld this  ; push <this>
        // call a3  ; pop <this> -> push <value3>
        // call m3  ; pop <obj2> pop<value3> -> push<obj3>

        // POP TREE:
        //                [m3]
        //            [m2]    [a3]
        //        [m1]   [a2]    [this]   
        //   [this]  [a1]   [this]   
        //              [this]

        Hashtable chains = new Hashtable();


        class PopTreeGraph
        {
            IControlFlowGraph _cfg;
            public ArrayList PopTreeBlocks = new ArrayList();
            public PopTreeGraph(IControlFlowGraph cfg)
            {
                _cfg = cfg;
            }
            public void Add(PopTreeList trees)
            {
                PopTreeBlocks.Add(trees);
            }
        }

        class PopTreeList
        {
            public IInstructionBlock Block;
            public ArrayList PopTrees = new ArrayList();
            public void Add( PopTree tree )
            {
                PopTrees.Add( tree );
            }
        }

        class PopTree
        {
            public Instruction _instruction;
            public ArrayList _PopList;
            public PopTree( Instruction instruction )
            {
                _instruction = instruction;
                _PopList = new ArrayList();
            }

            public void Consume(PopTree t)
            {
                _PopList.Add(t);
            }


            public int Depth
            {
                get
                {
                    if (_PopList.Count == 0)
                        return 0;
                    int max = 0;
                    foreach (PopTree t in _PopList)
                    {
                        int d = t.Depth;
                        if (d > max)
                            max = d;
                    }
                    return max + 1;
                }
            }

            public ArrayList FilteredTops( params OpCode[] opcodes )
            {
                ArrayList tops = new ArrayList();
                PopTree current = this;
                while (current != null)
                {
                    // Want Continous Chain.
                    if (!IsOpCode(opcodes, current._instruction.OpCode))
                        return tops;
                    tops.Add(current);
                    current = current.LastChild;
                }
                return tops;
            }

            private bool IsOpCode(OpCode[] opcodes, OpCode testCode)
            {
                foreach (OpCode opcode in opcodes)
                {
                    if (testCode == opcode)
                        return true;
                }
                return false;
            }

            public ArrayList Tops
            {
                get
                {
                    ArrayList tops = new ArrayList();
                    PopTree current = this;
                    while (current != null)
                    {
                        tops.Add(current);
                        current = current.LastChild;
                    }
                    return tops;
                }
            }

            public PopTree LastChild
            {
                get
                {
                    if (_PopList.Count > 0)
                        return (PopTree)_PopList[_PopList.Count-1];
                    return null;
                }
            }
        }

        class PopTreeGraphBuilder
        {
            public PopTreeGraph Build(MethodDefinition method)
            {
                IControlFlowGraph cfg = FlowGraphFactory.CreateControlFlowGraph(method);
                PopTreeGraph graph = new PopTreeGraph( cfg );
                foreach (IInstructionBlock block in cfg.Blocks)
                {
                    PopTreeListBuilder build = new PopTreeListBuilder();

                    Instruction instruction = block.FirstInstruction;
                    ArrayList instructions = new ArrayList();
                    while (instruction != null)
                    {
                        instructions.Add(instruction);
                        instruction = instruction.Next;
                    }

                    PopTreeList list = build.Build(instructions);
                    if (list != null)
                    {
                        list.Block = block;
                        graph.Add(list);
                    }
                }
                return graph;
            }
        }

        class PopTreeListBuilder
        {
            public Stack _machineStack = new Stack();
            PopTreeList _popTreeList = new PopTreeList();

            public PopTreeList Build( ICollection instructions )
            {
                foreach (Instruction i in instructions)
                {
                    PopTree p = new PopTree(i);
                    int pops = InstructionInfo.Pops(i,_machineStack.Count);
                    if (pops > _machineStack.Count)
                    {
                        // can happen with exception blocks whose support is flaky.
                        //Console.WriteLine("Attempting to pop more than machine has: " + i.OpCode.ToString() + " " + i.Operand);
                        return null;
                    }
                    for (int x = 0; x < pops; x++)
                    {
                        p.Consume(Pop());
                    }
                    if (InstructionInfo.Pushes(i))
                    {
                        Push(p);
                    }
                    else
                    {
                        _popTreeList.Add(p);
                    }
                }
                return _popTreeList;
            }

            public void Push( PopTree t)
            {
                _machineStack.Push( t );
            }
            public PopTree Pop()
            {
                return (PopTree)_machineStack.Pop();
            }
        }
    }
}
