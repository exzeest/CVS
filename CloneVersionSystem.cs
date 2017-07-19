using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clones
{
    public class ListStack
    {
        List<int> list = new List<int>();

        public void Push(int value)
        {
            list.Add(value);
        }
        public int Pop()
        {
            if (IsEmpty()) throw new InvalidOperationException();
            var result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }

        private bool IsEmpty() => list.Count == 0;

        public string Peek()
        {
            return IsEmpty() ? null : list[list.Count - 1].ToString();
        }

        public ListStack Copy() //new
        {
            return new ListStack{list = new List<int>(list)};
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
