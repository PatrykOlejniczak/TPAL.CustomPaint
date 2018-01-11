using System.Collections.Generic;

namespace CustomPaint
{
    public class Caretaker
    {
        private Stack<Memento> UndoStack = new Stack<Memento>();
        private Stack<Memento> RedoStack = new Stack<Memento>();

        public bool IsUndoPossible => UndoStack.Count >= 2;
        public bool IsRedoPossible => RedoStack.Count != 0;

        public Memento getUndoMemento()
        {
            if (UndoStack.Count >= 2)
            {
                RedoStack.Push(UndoStack.Pop());
                return UndoStack.Peek();
            }
            else
                return null;
        }
        public Memento getRedoMemento()
        {
            if (RedoStack.Count != 0)
            {
                Memento m = RedoStack.Pop();
                UndoStack.Push(m);
                return m;
            }
            else
                return null;
        }
        public void InsertMementoForUndoRedo(Memento memento)
        {
            if (memento != null)
            {
                UndoStack.Push(memento);
                RedoStack.Clear();
            }
        }
    }
}