using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clones
{
    public class ListStack : IEnumerable<int>
    {
        private class StackItem
        {
            public int Value;
            public StackItem Prev;
        }

        private StackItem top;

        public void Push(int arg)
        {
            var newItem = new StackItem { Value = arg, Prev = top };
            top = newItem;
        }

        public bool IsEmpty => top == null;

        public int Pop()
        {
            if (IsEmpty)
                throw new NullReferenceException("Stack is empty");
            var result = top.Value;
            top = top.Prev;
            return result;
        }

        public string Peek()
        {
            return IsEmpty? null : top.Value.ToString();
        }

        public ListStack Copy() //new
        {
            var result = new ListStack();
            foreach (var value in this.Reverse())
                result.Push(value);
            return result;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (var item = top; item != null; item = item.Prev)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class Clone
    {
        public ListStack LearnedProgramms;
        public ListStack RollbackedProgramms;
        public bool Clonned = false;

        public void Learn(int program)
        {
            if (Clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                Clonned = false;
            }
            RollbackedProgramms = new ListStack();
            LearnedProgramms.Push(program);
        }

        public void RollBack()
        {
            if (Clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                RollbackedProgramms = RollbackedProgramms.Copy();
                Clonned = false;
            }
            RollbackedProgramms.Push(LearnedProgramms.Pop());
        }

        public void Realern()
        {
            if (Clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                RollbackedProgramms = RollbackedProgramms.Copy();
                Clonned = false;
            }
            LearnedProgramms.Push(RollbackedProgramms.Pop());
        }

        public string Check()
        {
            return LearnedProgramms.Peek() == null
              ? "basic"
              : LearnedProgramms.Peek();
        }

        public Clone MakeCopy()
        {
            var cln = new Clone
            {
                LearnedProgramms = LearnedProgramms,
                RollbackedProgramms = RollbackedProgramms,
                Clonned = true
            };
            return cln;
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        public List<Clone> Clns = new List<Clone>();

        public string Execute(string query)
        {
           
            var commands = query.Split();
            var cloneNumber = int.Parse(commands[1]) - 1;
            if (cloneNumber + 1 > Clns.Count)
                Clns.Add(new Clone { LearnedProgramms = new ListStack(), RollbackedProgramms = new ListStack() });
            return ExecuteCommand(commands, cloneNumber);
        }

        private string ExecuteCommand(string[] commands, int cloneNumber)
        {
            string message = null;
            switch (commands[0])
            {
                case "learn":
                    Clns[cloneNumber].Learn(int.Parse(commands[2]));
                    break;
                case "rollback":
                    Clns[cloneNumber].RollBack();
                    break;
                case "relearn":
                    Clns[cloneNumber].Realern();
                    break;
                case "clone":
                    Clns.Add(Clns[cloneNumber].MakeCopy());
                    break;
                case "check":
                    message = Clns[cloneNumber].Check();
                    break;
            }
            return message;
        }
    }
}
