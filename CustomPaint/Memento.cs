using System.Collections.Generic;
using System.Windows;

namespace CustomPaint
{
    public class Memento
    {
        public List<UIElement> ContainerState { get; }

        public Memento(List<UIElement> containerState)
        {
            ContainerState = containerState;
        }
    }
}