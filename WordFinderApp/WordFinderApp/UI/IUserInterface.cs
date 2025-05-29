namespace WordFinderApp.UI
{
    // User interface abstraction
    public interface IUserInterface
    {
        /// <summary>
        /// Show a message to the user
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);
        /// <summary>
        /// Get input from the user
        /// </summary>
        /// <returns></returns>
        string GetInput();
    }
}
