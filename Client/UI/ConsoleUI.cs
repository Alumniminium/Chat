using System;

namespace Client.UI
{
    public static class ConsoleUi
    {
        public static readonly char[] Lines = { '┌', '─', '┐', '│', '└', '─', '┘' };
        private static void DrawLine(int x, int y, int x2, int y2, LineType type)
        {
            var dist = Math.Max(Math.Abs(x - x2), Math.Abs(y - y2));

            if (type == LineType.Top)
            {
                for (int i = 0; i < dist; i++)
                {
                    Console.SetCursorPosition(x + i, y);
                    if (i == 0)
                        Console.Write(Lines[0]);
                    else if (i == dist - 1)
                        Console.Write(Lines[2]);
                    else
                        Console.Write(Lines[1]);
                }
            }
            if (type == LineType.Bottom)
            {
                for (int i = 0; i < dist; i++)
                {
                    Console.SetCursorPosition(x + i, y - 1);
                    if (i == 0)
                        Console.Write(Lines[4]);
                    else if (i == dist - 1)
                        Console.Write(Lines[6]);
                    else
                        Console.Write(Lines[1]);
                }
            }
            if (type == LineType.Left)
            {
                for (int i = 0; i < dist; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    if (i == 0)
                        Console.Write(Lines[0]);
                    else if (i == dist - 1)
                        Console.Write(Lines[4]);
                    else
                        Console.Write(Lines[3]);
                }
            }
            if (type == LineType.Right)
            {
                for (int i = 0; i < dist; i++)
                {
                    Console.SetCursorPosition(x2 - 1, y + i);
                    if (i == 0)
                        Console.Write(Lines[2]);
                    else if (i == dist - 1)
                        Console.Write(Lines[6]);
                    else
                        Console.Write(Lines[3]);
                }
            }
        }
        internal static void DrawRectangle(int x, int y, int width, int height)
        {
            var x2 = x + width;
            var y2 = y + height;
            DrawLine(x, y, x2, y, LineType.Top);
            DrawLine(x, y2, x2, y2, LineType.Bottom);

            DrawLine(x, y, x, y2, LineType.Left);
            DrawLine(x2, y, x2, y2, LineType.Right);
        }
    }
}