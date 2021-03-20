using EShopOnion.DataAccess.Entities;
using EShopOnion.DataAccess.Enums;
using EShopOnion.DataAccess.Interfaces;
using EShopOnion.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace EShopOnion.ConsoleApp
{
    public class ConsoleEShopBuilder
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        private const string _commandExit = "exit";

        public static IUser CurrentUser { get; set; }

        public ConsoleEShopBuilder(IProductService productService, IUserService userService, IOrderService orderService)
        {
            _productService = productService;
            _userService = userService;
            _orderService = orderService;
        }

        public void Run()
        {
            var choicesMain = new List<MenuChoice>();

            var root = new Menu(choicesMain, null);

            choicesMain.Add(new MenuChoice("if you want to view a list of products.", PrintProducts, UserRoles.Guest));
            choicesMain.Add(new MenuChoice("if you want to find product by name.", SearchProductByName, UserRoles.Guest));
            choicesMain.Add(new MenuChoice("if you want to sign up.", Register, UserRoles.Guest));
            choicesMain.Add(new MenuChoice("if you want to sign in.", Login, UserRoles.Guest));
            choicesMain.Add(new MenuChoice("if you want to create order.", CreateOrder, UserRoles.User));
            choicesMain.Add(new MenuChoice("if you want to cancel order.", CancelOrderByUser, UserRoles.User));
            choicesMain.Add(new MenuChoice("if you want to view a list of orders.", PrintUserOrders, UserRoles.User));
            choicesMain.Add(new MenuChoice("if you want to set order`s status 'Received'.", SetOrderStatusReceivedByUser, UserRoles.User));
            choicesMain.Add(new MenuChoice("if you want to change personal information.", ChangePersonalInformationByUser, UserRoles.User));
            choicesMain.Add(new MenuChoice("if you want to view a list of users.", PrintUsers, UserRoles.Administrator));
            choicesMain.Add(new MenuChoice("if you want to sign out.", SignOut, UserRoles.Guest));

            new MenuManager(root).Run();
        }

        void PrintUsers()
        {
            foreach (var user in _userService.GetUsers())
                Console.WriteLine($"{user.UserName}. {user.UserRole} ({user.Email}).");

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void ChangePersonalInformationByUser()
        {
            Console.WriteLine("Please enter new e-mail.");

            var newEmail = Console.ReadLine();

            _userService.UpdateUser(CurrentUser, newEmail);

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void PrintUserOrders()
        {
            foreach (var order in _orderService.GetUserOrders(CurrentUser))
                Console.WriteLine($"{order.Id}. {order.Status} ({order.Total}).");

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void SetOrderStatusReceivedByUser()
        {
            Console.WriteLine("Please enter order`s id");

            var strOrderId = Console.ReadLine();
            if (!int.TryParse(strOrderId, out int id))
                return;

            var orderFromRepo = _orderService.GetOrderById(id);
            if (orderFromRepo == null)
                Console.WriteLine("Order not found!");
            else
            {
                if (orderFromRepo.Status == OrderStatus.CanceledByAdministrator ||
                    orderFromRepo.Status == OrderStatus.CanceledByUser)
                    Console.WriteLine("Order was cancelled.");
                else
                {
                    _orderService.UpdateStatusOrder(orderFromRepo.Id, OrderStatus.Received);

                    Console.WriteLine("Order was received.");
                    Console.WriteLine($"{orderFromRepo.Id}. {orderFromRepo.Status} ({orderFromRepo.Total}).");
                }
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void CancelOrderByUser()
        {
            Console.WriteLine("Please enter order`s id");

            var strOrderId = Console.ReadLine();
            if (!int.TryParse(strOrderId, out int id))
                return;

            var orderFromRepo = _orderService.GetOrderById(id);
            if (orderFromRepo == null)
                Console.WriteLine("Order not found!");
            else
            {
                if (orderFromRepo.Status == OrderStatus.Completed)
                    Console.WriteLine("Order cant be cancelled. Order was completed.");
                else
                {
                    _orderService.UpdateStatusOrder(orderFromRepo.Id, OrderStatus.CanceledByUser);

                    Console.WriteLine("Order was cancelled.");
                    Console.WriteLine($"{orderFromRepo.Id}. {orderFromRepo.Status} ({orderFromRepo.Total}).");
                }             
            }
                
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void CreateOrder()
        {
            string line = "";

            var orderItems = new List<OrderItem>();

            while (!line.Equals(_commandExit))
            {
                Console.WriteLine("Please enter product`s id");

                var strProductId = Console.ReadLine();
                if (!int.TryParse(strProductId, out int id)) 
                    continue;

                var productFromRepo = _productService.GetProductById(id);
                if (productFromRepo == null)
                {
                    Console.WriteLine("Product not found!");
                    continue;
                }                   
                else
                {
                    Console.WriteLine($"{productFromRepo.Id}. {productFromRepo.Name} ({productFromRepo.Price}).");
                }                   

                Console.WriteLine("Please enter product`s quantity");

                var strProductQuantity = Console.ReadLine();
                if (!int.TryParse(strProductQuantity, out int quantity)) 
                    continue;

                if (quantity < 1)
                {
                    Console.WriteLine("Please enter correct product`s quantity");
                    continue;
                }

                orderItems.Add(new OrderItem(id, productFromRepo.Price, quantity));

                Console.WriteLine($"Please enter {_commandExit} if you want to exit or any key to continue.");
                line = Console.ReadLine();
            }

            if (orderItems.Count > 0)
            {
                _orderService.CreateOrder(new Order(CurrentUser.Id, orderItems, ""));

                Console.WriteLine("Order created. Press any key to continue.");
                Console.ReadKey();
            }
                
        }

        void PrintProducts()
        {
            foreach (var product in _productService.GetProducts())
                Console.WriteLine($"{product.Id}. {product.Name} ({product.Price}).");

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void SearchProductByName()
        {
            var productName = String.Empty;
            while (productName == String.Empty)
            {
                Console.WriteLine("Please enter product`s name.");
                productName = Console.ReadLine();
                productName = productName.Trim();
            }

            var productFromRepo = _productService.GetProductByName(productName);
            if (productFromRepo == null)
                Console.WriteLine("Product not found!");
            else
                Console.WriteLine($"{productFromRepo.Id}. {productFromRepo.Name} ({productFromRepo.Price}).");

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        void SignOut()
        {
            Menu.CurrentUser = null;
            CurrentUser = null;
        }

        void Login()
        {
            var username = String.Empty;
            while (username == String.Empty)
            {
                Console.WriteLine("Please enter username");
                username = Console.ReadLine();
                username = username.Trim();
            }

            Console.WriteLine("Please enter password");
            var password = GetConsoleSecurePassword().ToString();
            Console.WriteLine();

            var userFromRepo = _userService.Login(username, password);
            if (userFromRepo == null)
                Console.WriteLine("User not found! Press any key to continue.");
            else
                Console.WriteLine($"Welcome {userFromRepo.UserName}! Press any key to continue.");

            Console.ReadKey();
            Menu.CurrentUser = userFromRepo;
            CurrentUser = userFromRepo;
        }

        void Register()
        {
            var username = String.Empty;
            while (username == String.Empty)
            {
                Console.WriteLine("Please enter username");
                username = Console.ReadLine();
                username = username.Trim();
            }

            Console.WriteLine("Please enter password");
            var password = GetConsoleSecurePassword().ToString();

            var userToCreate = new User
            {
                UserName = username
            };

            _userService.Register(userToCreate, password);
        }

        private static SecureString GetConsoleSecurePassword()
        {
            SecureString pwd = new SecureString();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    pwd.RemoveAt(pwd.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    pwd.AppendChar(i.KeyChar);
                    Console.Write("*");
                }
            }
            return pwd;
        }

    }


}
