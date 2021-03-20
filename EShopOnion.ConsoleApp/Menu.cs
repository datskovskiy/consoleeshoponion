using EShopOnion.DataAccess.Enums;
using EShopOnion.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.ConsoleApp
{
    public class Menu
    {
        public static IUser CurrentUser { get; set; }

        private readonly string Separator = new string('-', 100);
        private readonly List<MenuChoice> _choices;
        private readonly Menu _root;

        public Menu(List<MenuChoice> choices, Menu root)
        {
            _choices = choices;
            _root = root;
        }

        private void Print()
        {
            for (int index = 0; index < _choices.Count; index++)
                Console.WriteLine($"Press {index + 1} {_choices[index].Title}");

            Console.WriteLine($"Press {_choices.Count + 1} to " +
                              $"{(_root == null ? "exit" : "go to previous menu")}");
        }

        public void Run()
        {
            var userRole = (CurrentUser == null) ? UserRoles.Guest : CurrentUser.UserRole;
            var usernameForPrint = (CurrentUser is null) ? "Guest" : CurrentUser.UserName;

            var header = $"Welcome to ConsoleEShop, {usernameForPrint}!";

            Console.Clear();
            Console.WriteLine(Separator);
            Console.WriteLine(header);
            Console.WriteLine();
            Print();
            Console.WriteLine(Separator);
            uint choice = GetUserChoice();
            if (choice == _choices.Count + 1)
                if (_root == null)
                    Console.WriteLine("Thank you for visiting ConsoleEShop, Goodbye!");
                else
                    _root.Run();
            else
            {
                var roleIndex = (int)Enum.Parse(typeof(UserRoles), _choices[(int)choice - 1].UserRole.ToString());
                var currentUserRoleIndex = (int)Enum.Parse(typeof(UserRoles), userRole.ToString());

                if (currentUserRoleIndex < roleIndex)
                {
                    Console.WriteLine("No access, press a key to continue.");
                    Console.ReadKey();
                    Run();
                }

                var action = _choices[(int)choice - 1].Action;
                if (action != null)
                {
                    action();
                    Run();
                }               
                else
                {
                    Console.WriteLine("Not implemented yet, press a key to continue.");
                    Console.ReadKey();
                    Run();
                }
            }
        }

        uint GetUserChoice()
        {
            uint choice = 0;
            Action getInput = () =>
            {
                uint.TryParse(Console.ReadLine(), out choice);
            };
            getInput();
            while (choice < 1 || choice > _choices.Count + 1)
            {
                Console.WriteLine();
                Console.WriteLine("Please try again");
                Print();
                getInput();
            }
            return choice;
        }
    }
}
