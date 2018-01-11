using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Xml;

namespace CustomPaint
{
    public class MementoOriginator
    {
        private readonly Canvas _container;

        public MementoOriginator(Canvas container)
        {
            _container = container;
        }

        public Memento GetMemento()
        {
            List<UIElement> containerState = new List<UIElement>();

            foreach (UIElement item in _container.Children)
            {
                if (!(item is Thumb))
                {
                    UIElement newItem = DeepClone(item);
                    containerState.Add(newItem);
                }
            }

            return new Memento(containerState);
        }

        public void SetMemento(Memento memento)
        {
            _container.Children.Clear();
            Memento memento1 = MementoClone(memento);
            foreach (UIElement item in memento1.ContainerState)
            {       
                _container.Children.Add(item);
            }
        }

        public Memento MementoClone(Memento memento)
        {
            List<UIElement> containerState = new List<UIElement>();

            foreach (UIElement item in memento.ContainerState)
            {
                if (!(item is Thumb))
                {
                    UIElement newItem = DeepClone(item);
                    containerState.Add(newItem);
                }
            }

            return new Memento(containerState);

        }
        private UIElement DeepClone(UIElement element)
        {
            string shapestring = XamlWriter.Save(element);
            StringReader stringReader = new StringReader(shapestring);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);

            return (UIElement)XamlReader.Load(xmlTextReader);
        }
    }
}