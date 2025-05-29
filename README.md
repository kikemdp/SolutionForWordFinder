# WordFinder

A high-performance .NET solution to find the most frequent words from a stream within a given character matrix. Designed to showcase clean code, algorithmic choices, scalability, and performance profiling through benchmarking.

---

## Objective

The objective of this challenge is to evaluate software engineering best practices: code quality, design decisions, performance, testing, and documentation—as you would in a real-world team setting.

---

## Features

- **Multiple Algorithms** to find words in a matrix:
  - `FindOriginal`: Simple brute-force approach
  - `Find`: Optimized Aho-Corasick implementation
  - `FindParallel`: Multithreaded brute-force using `Parallel.ForEach`
- **Top 10 Matching Words**: Returns most frequent matches from the stream
- **Matrix Support**: Searches horizontally and vertically
- **Benchmarking**: Performance analyzed via `BenchmarkDotNet`
- **Testing**: Full unit test coverage for edge cases and expected behavior
- **Manual Testing**: CLI-based executable included for ad-hoc input

---

## 📂 Project Structure
```bash
.github/
├── workflows/
└── ci.yaml # Yaml file

WordFinderApp/
│
├── WordFinderApp/ # Main application logic
│ └── WordFinder.cs # Core class
│
├── WordFinderTests/ # Unit test project
│ └── WordFinderTest.cs
│
├── WordFinderBenchmarks/ # BenchmarkDotNet project
│ └── WordFinderBenchmark.cs
│
├── Executable File/ # Manual executable (optional)
│ └── WordFinderApp.exe
      
│
├── Docs/
│ └── AnalysisOfFindMethods.doc # Complexity, memory, algorithm comparison
│
└── README.md
```

## Run Tests
   dotnet test
## Run Benchmarks
  cd WordFinderBenchmarks  
  dotnet run -c Release
## Manual Test (Executable)
  Run the executable and follow the prompts:  
  ./Executable File/WordFinder.exe

## Design Choices
  Three different methods have been written to compare them with each other:
  Aho-Corasick: Efficient for large wordstreams with many overlapping patterns.
  Parallel Brute Force: Fastest in benchmarks but higher memory usage.
  Fallbacks: Maintains Find Original for simplicity and clarity.
  
  Separation of Concerns: Matrix parsing handled by MatrixWordExtractor.
  
  More in Docs/AnalysisOfFindMethods.doc

## Code Quality
  Clean architecture
  XML documentation for public methods
  Custom exception handling
  Defensive programming (null checks, validations)

## Continuous Integration

This project includes a GitHub Actions workflow to automate build and test steps on every push and pull request.

- Validates the solution builds successfully with .NET 8
- Runs all unit tests
- YAML file: [.github/workflows/ci.yml](.github/workflows/ci.yaml)

To enable CI, simply fork the repository and enable GitHub Actions.




