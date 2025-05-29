namespace WordFinderApp.UI
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        public string GetInput()
        {
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
