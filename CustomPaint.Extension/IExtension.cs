using System.Windows.Controls;

namespace CustomPaint.Extension
{
    public interface IExtension
    {
        void Execute(Canvas canvas);
    }
}