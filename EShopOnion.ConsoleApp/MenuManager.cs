namespace EShopOnion.ConsoleApp
{
    public class MenuManager
    {
        private readonly Menu _root;

        public MenuManager(Menu root)
        {
            _root = root;
        }

        public void Run()
        {
            _root.Run();
        }
    }
}
