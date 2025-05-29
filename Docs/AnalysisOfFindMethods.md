# 📊 Analysis of `Find` Methods: Performance and Complexity

## Overview

This document provides a comparative analysis of the three `Find` methods implemented in the `WordFinder` class:

1. **FindOriginal** – Simple brute-force implementation  
2. **Find** – Aho-Corasick algorithm-based implementation  
3. **FindParallel** – Parallel brute-force implementation  

Each method is examined in terms of:

- **Time Complexity**
- **Space Complexity**
- **Practical Performance** (based on benchmarks)

---

## 🧠 Method 1: `FindOriginal`

### Description

The original brute-force approach iterates over the set of `wordstream` words, and for each word, it checks how many times it appears in the `_sequences` (extracted matrix rows and columns).

### Time Complexity

- Let **W** be the number of words in the `wordstream`
- Let **S** be the number of sequences (rows + columns)
- Let **L** be the average length of a sequence
- Let **P** be the average length of a word

**Complexity:** `O(W * S * L)`  
(`string.Contains()` is approximately `O(L)` per call)

### Space Complexity

- `O(W)` for storing the word counts

### 🔬 Benchmark Results (LargeStream)

- **Time:** 239,410 μs
- **Memory:** 14.6 MB

### ✅ Pros

- Simple to implement and understand  
- Low memory usage  

### ❌ Cons

- Slow for large inputs  

---

## ⚙️ Method 2: `Find` (Aho-Corasick)

### Description

Uses the Aho-Corasick algorithm to build a trie from the `wordstream`, followed by a state-machine traversal of the sequences.  
> Note: The Aho-Corasick code was adapted from online resources.

### Time Complexity

- **Trie Construction:** `O(W * P)`  
- **Failure Link Setup:** `O(total nodes)`  
- **Sequence Traversal:** `O(S * L)`  
- **Total Time:** `O(W * P + S * L)`

### Space Complexity

- `O(W * P)` for the trie structure  
- `O(W)` for word counts  

### 🔬 Benchmark Results (LargeStream)

- **Time:** 226,281 μs  
- **Memory:** 63.5 MB  

### ✅ Pros

- Efficient for large-scale repeated searches  
- Avoids repeated scanning of sequences  

### ❌ Cons

- High memory usage  
- Slower than expected on small/mid workloads  
- Poor scaling unless reused across multiple queries  

---

## 🚀 Method 3: `FindParallel`

### Description

Parallel brute-force that distributes word search tasks across multiple threads using `Parallel.ForEach`.

### Time Complexity

(Same logical complexity as `FindOriginal`):  
- `O(W * S * L)`

But runs in **parallel**, so practical time is reduced significantly depending on thread count and hardware.

### Space Complexity

- `O(W)` + thread overhead

### 🔬 Benchmark Results (LargeStream)

- **Time:** 81,630 μs (fastest)
- **Memory:** 15.4 MB

### ✅ Pros

- **Fastest** in practice  
- Good balance between simplicity and performance  
- Parallelism provides significant performance boost  

### ❌ Cons

- Slightly higher memory usage due to concurrency  
- Requires thread-safe collections (`ConcurrentDictionary`)  

---

## 📋 Summary Table

| Method        | Time Complexity       | Space Complexity   | Benchmark Time (μs) | Memory (MB) | Notes                       |
|---------------|------------------------|---------------------|----------------------|--------------|-----------------------------|
| FindOriginal  | `O(W * S * L)`         | `O(W)`              | 239,410              | 14.6         | Simple, slow                |
| Find          | `O(W * P + S * L)`     | `O(W * P)`          | 226,281              | 63.5         | Smart, memory-hungry        |
| FindParallel  | `O(W * S * L)`         | `O(W)` + overhead   | 81,630               | 15.4         | Fastest, scalable           |

---

## ✅ Conclusion

- **`FindParallel`** is the best choice for **performance-critical** use cases, especially when matrix and wordstream sizes are moderate.
- **`Find` (Aho-Corasick)** may be better suited if the search patterns (wordstream) are reused multiple times across different matrices.  
  > _I was hoping this to be the best solution for large streams._
- **`FindOriginal`** is best left for **baseline comparisons** or very small inputs.

---
