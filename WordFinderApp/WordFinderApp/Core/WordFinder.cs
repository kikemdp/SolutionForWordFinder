using System.Collections.Concurrent;
using WordFinderApp.Utility;

namespace WordFinderApp.Core
{
    // Core logic class
    public class WordFinder
    {
        private readonly List<string> _sequences;
        private Node _root;
        private bool _automatonBuilt;
        private HashSet<string> _lastPatterns;

        public WordFinder(IEnumerable<string> matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            _sequences = MatrixWordExtractor.ExtractWords(matrix);
            _automatonBuilt = false;
        }

        /// <summary>
        /// Finds top 10 most frequent words from the wordstream that appear in the matrix sequences.
        /// This method uses the Aho-Corasick algorithm to efficiently find all occurrences of the words in the matrix.
        /// </summary>
        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            try
            {
                if (wordstream == null || !wordstream.Any())
                    return Array.Empty<string>();

                var patterns = new HashSet<string>(wordstream);
                if (!_automatonBuilt || !_lastPatterns.SetEquals(patterns))
                {
                    _root = BuildTrie(patterns);
                    BuildFailureLinks(_root);
                    _automatonBuilt = true;
                    _lastPatterns = patterns;
                }

                var counts = patterns.ToDictionary(w => w, w => 0);
                foreach (var seq in _sequences)
                {
                    var node = _root;
                    foreach (var ch in seq)
                    {
                        while (node != _root && !node.Children.ContainsKey(ch))
                            node = node.Failure;
                        if (node.Children.TryGetValue(ch, out var next))
                            node = next;
                        foreach (var match in node.Output)
                            counts[match]++;
                    }
                }

                return counts
                    .Where(kv => kv.Value > 0)
                    .OrderByDescending(kv => kv.Value)
                    .Take(10)
                    .Select(kv => kv.Key);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An internal error occurred while finding words.", ex);
            }
        }

        // Trie node
        private class Node
        {
            public Dictionary<char, Node> Children { get; } = new();
            public Node Failure { get; set; }
            public List<string> Output { get; } = new();
        }

        // Build trie from patterns
        private Node BuildTrie(IEnumerable<string> patterns)
        {
            var root = new Node();
            foreach (var pat in patterns)
            {
                var node = root;
                foreach (var ch in pat)
                {
                    if (!node.Children.ContainsKey(ch))
                        node.Children[ch] = new Node();
                    node = node.Children[ch];
                }
                node.Output.Add(pat);
            }
            return root;
        }

        // BFS to set failure links
        private void BuildFailureLinks(Node root)
        {
            var queue = new Queue<Node>();
            foreach (var child in root.Children.Values)
            {
                child.Failure = root;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var kv in current.Children)
                {
                    var ch = kv.Key;
                    var childNode = kv.Value;

                    var fail = current.Failure;
                    while (fail != null && !fail.Children.ContainsKey(ch))
                        fail = fail.Failure;
                    childNode.Failure = (fail != null && fail.Children.ContainsKey(ch))
                        ? fail.Children[ch]
                        : root;

                    childNode.Output.AddRange(childNode.Failure.Output);
                    queue.Enqueue(childNode);
                }
            }
        }

        /// <summary>
        /// Find the top 10 most frequent words in the wordstream that are substrings of the matrix words.
        /// This is the original method that uses a simple approach to find the words.
        /// </summary>
        /// <param name="wordstream"></param>
        /// <returns></returns>
        public IEnumerable<string> FindOriginal(IEnumerable<string> wordstream)
        {
            try
            {
                if (wordstream == null || !wordstream.Any())
                {
                    return Enumerable.Empty<string>();
                }

                // Create a hashset to ensure each word in the wordstream is counted once
                var wordstreamSet = new HashSet<string>(wordstream);

                var wordCount = new Dictionary<string, int>();

                foreach (var word in wordstreamSet)
                {
                    // Count occurrences of the word in matrix words
                    int count = _sequences.Count(matrixWord => matrixWord.Contains(word));

                    if (count > 0)
                    {
                        // Add or update the word count
                        wordCount[word] = wordCount.GetValueOrDefault(word, 0) + count;
                    }
                }

                // Return top 10 words based on frequency 
                return wordCount
                    .OrderByDescending(pair => pair.Value)
                    .Take(10)
                    .Select(pair => pair.Key);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while finding words.", ex);
            }
        }


        /// <summary>
        /// Last one created with the best results, this method uses parallel processing to improve performance.
        /// Find in parallel the top 10 most frequent words in the wordstream that are substrings of the matrix words.
        /// </summary>
        /// <param name="wordstream"></param>
        /// <returns></returns>
        public IEnumerable<string> FindParallel(IEnumerable<string> wordstream)
        {
            try
            {
                if (wordstream == null || !wordstream.Any())
                {
                    return Enumerable.Empty<string>();
                }

                // Use HashSet to ensure uniqueness
                var wordstreamSet = new HashSet<string>(wordstream);

                // Use ConcurrentDictionary to handle thread-safety automatically
                var wordCount = new ConcurrentDictionary<string, int>();

                // Use parallel processing to try to improve the performance
                Parallel.ForEach(wordstreamSet, word =>
                {
                    // Count occurrences of the word in matrix words 
                    int count = _sequences
                        .Count(matrixWord => matrixWord.Contains(word));

                    if (count > 0)
                    {
                        // Safely add or update the word count
                        wordCount.AddOrUpdate(word, count, (key, existingCount) => existingCount + count);
                    }
                });

                // Return top 10 words based on frequency
                return wordCount
                    .OrderByDescending(pair => pair.Value)
                    .Take(10)
                    .Select(pair => pair.Key);
            }
            catch (Exception ex)
            {
                // Log exception if needed and rethrow
                throw new InvalidOperationException("An error occurred while finding words.", ex);
            }
        }
    }
}


