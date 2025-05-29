using Microsoft.Extensions.DependencyInjection;
using WordFinderApp.Core;
using WordFinderApp.UI;
using WordFinderApp.Utility;

namespace WordFinderApp
{
    // Application entry point
    class Program
    {
        private readonly IUserInterface _ui;
        private readonly IMatrixInputHandler _matrixInputHandler;

        public Program(
            IUserInterface ui,
            IMatrixInputHandler matrixInputHandler)
        {
            _ui = ui;
            _matrixInputHandler = matrixInputHandler;
        }

        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var program = serviceProvider.GetService<Program>();
            program?.Run();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register dependencies
            services.AddTransient<IUserInterface, ConsoleUserInterface>();
            services.AddTransient<IMatrixInputHandler, MatrixInputHandler>();
            services.AddTransient<Program>();

            return services.BuildServiceProvider();
        }

        public void Run()
        {
            var (rows, cols) = _matrixInputHandler.GetMatrixSize();
            var matrix = _matrixInputHandler.GetMatrix(rows, cols);

            _ui.ShowMessage("Matrix entered:");
            foreach (var row in matrix)
            {
                _ui.ShowMessage(row);
            }

            _ui.ShowMessage("Enter the words in the word stream, separated by spaces:");
            var wordstreamInput = _ui.GetInput();
            var wordstream = wordstreamInput?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

            var wordFinder = new WordFinder(matrix);
            var result = wordFinder.Find(wordstream);

            _ui.ShowMessage("Top words found:");
            foreach (var word in result)
            {
                _ui.ShowMessage(word);
            }
        }
    }
}
