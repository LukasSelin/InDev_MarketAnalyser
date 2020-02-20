using System;
using System.Collections.Generic;
using System.Linq;

namespace menu
{
    public class Menu
    {
        public Menu(IEnumerable<string> items)      // Itererar över elementet
        {
            Items = items.ToArray();                // Gör om till en array
        }
        public Menu(IEnumerable<string> items, bool isInputMenu)    // Itererar över elementet
        {
            Items = items.ToArray();                // Gör om till en array
            for (int i = 0; i < Items.Count; i++)
            {
                ItemContent.Add("");
            }
            this.isInputMenu = isInputMenu;
        }
        public IReadOnlyList<string> Items { get; }
        public int SelectedIndex { get; private set; } = 0; // Utgångspunkt med inget markerat
        public string SelectedOption => SelectedIndex != -1 ? Items[SelectedIndex] : null;
        public void MoveUp() => SelectedIndex = Math.Max(SelectedIndex - 1, 0);      // Rör dig ett steg uppåt om du inte är på högsta nivån
        public void MoveDown() => SelectedIndex = Math.Min(SelectedIndex + 1, Items.Count - 1); // Rör dig ett steg neråt om du inte är på lägsta nivån
        public List<string> ItemContent { get; } = new List<string>() { };
        public bool isInputMenu { get; } = false;
        public class InputMenu
        {
            public static bool isComplete(Menu menu)
            {
                foreach (var item in menu.ItemContent)
                {
                    if (item == "")
                    {
                        return false;
                    }
                }
                return true;
            }
            public Menu GetMenu(Menu menu, string header)
            {
                /// <summary>
                /// Returns a InputMenu
                /// </summary>
                /// <para>
                /// Return: A filled ItemContent list in menu
                /// </para>
                /// <returns></returns>
                var menuPainter = new ConsoleMenuPainter(menu);
                bool done = false;

                do
                {
                    Console.CursorVisible = false;
                    menuPainter.Paint(header);
                    var keyInfo = Console.ReadKey(false);
                    Console.CursorVisible = true;
                    int Xoffset = menu.Items[menu.SelectedIndex].Length + 4;
                    int Yoffset = 2 + menu.SelectedIndex;
                    Console.SetCursorPosition(Xoffset, Yoffset);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            menu.MoveUp();
                            break;

                        case ConsoleKey.DownArrow:
                            menu.MoveDown();
                            break;

                        case ConsoleKey.Enter:
                            menu.ItemContent[menu.SelectedIndex] = Console.ReadLine();
                            break;

                        default:

                            Console.SetCursorPosition(0, menu.Items.Count + 2);
                            Console.WriteLine(' ');

                            Console.SetCursorPosition(Xoffset, Yoffset);
                            Console.Write(keyInfo.KeyChar);
                            menu.ItemContent[menu.SelectedIndex] = keyInfo.KeyChar + Console.ReadLine();
                            break;
                    }
                    if (InputMenu.isComplete(menu))
                    {
                        done = true;
                    }
                }
                while (!done);
                Console.SetCursorPosition(0, menu.Items.Count + 3);
                Console.CursorVisible = true;
                return menu;
            }
        }
        public Menu GetMenu(Menu menu, string header)
        {
            if (isInputMenu)
            {
                InputMenu inputMenu = new InputMenu();
                inputMenu.GetMenu(menu, header);
            }
            else
            {
                var menuPainter = new ConsoleMenuPainter(menu);
                bool done = false;
                do
                {
                    Console.CursorVisible = false;
                    menuPainter.Paint(header);
                    var keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            menu.MoveUp();
                            break;
                        case ConsoleKey.DownArrow:
                            menu.MoveDown();
                            break;
                        case ConsoleKey.Enter:
                            done = true;
                            break;
                    }
                }
                while (!done);
            }
            Console.CursorVisible = true;
            return menu;
        }
    }
    public class ConsoleMenuPainter
    {
        readonly Menu menu;
        public ConsoleMenuPainter(Menu menu)
        {
            this.menu = menu;
        }
        public void Paint(string header)
        {
            Console.Clear();
            /* --------------------------------- Header --------------------------------- */
            Console.WriteLine(header);
            for (int i = 0; i <= header.Length; i++)
            {
                Console.Write('-');
            }
            /* ---------------------------------- Body ---------------------------------- */
            for (int i = 0; i < menu.Items.Count; i++)
            {

                var color = menu.SelectedIndex == i ? ConsoleColor.Yellow : ConsoleColor.Gray;
                int offset = 2 + i;

                // Puts the arrow in the correct place
                Console.SetCursorPosition(0, offset);
                Console.ForegroundColor = color;
                if (i == menu.SelectedIndex)
                {
                    Console.Write("->");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.SetCursorPosition(2, offset);

                if (menu.isInputMenu)
                {
                    Console.Write(menu.Items[i] + ": ");
                    Console.ResetColor();
                    Console.WriteLine(menu.ItemContent[i]);
                }
                else
                {
                    Console.WriteLine(menu.Items[i]);
                }
                Console.ResetColor();
            }
        }
    }
}