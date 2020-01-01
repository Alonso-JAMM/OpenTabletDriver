using System;
using System.Collections.Generic;
using System.Linq;
using NativeLib.Windows;
using TabletDriverLib.Component;

namespace TabletDriverLib.Interop.Display
{
    using static Windows;

    public class WindowsDisplay : IVirtualScreen
    {
        public WindowsDisplay()
        {
            var monitors = GetDisplays().OrderBy(e => e.Left).ToList();
            var displays = new List<IDisplay>();
            displays.Add(this);
            foreach (var monitor in monitors)
            {
                var display = new ManualDisplay(
                    monitor.Width,
                    monitor.Height,
                    new Point(monitor.Top, monitor.Left),
                    monitors.IndexOf(monitor) + 1);
            }
            Displays = displays;

            var left = monitors.Min(d => d.Left);
            var top = monitors.Min(d => d.Top);
            Position = new Point(left, top);
        }

        private IEnumerable<DisplayInfo> InternalDisplays => GetDisplays().OrderBy(e => e.Left);
        
        public float Width
        {
            get
            {
                var left = InternalDisplays.Min(d => d.Left);
                var right = InternalDisplays.Max(d => d.Right);
                return right - left;
            }
        }

        public float Height 
        {
            get
            {
                var top = InternalDisplays.Min(d => d.Top);
                var bottom = InternalDisplays.Max(d => d.Bottom);
                return bottom - top;
            }
        }

        public Point Position { private set; get; }

        public IEnumerable<IDisplay> Displays { private set; get; }

        public int Index => 0;

        public override string ToString()
        {
            return $"VirtualDisplay {Index} ({Width}x{Height}@{Position})";
        }
    }
}