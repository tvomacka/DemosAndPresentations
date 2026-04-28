# RefactorMe

A deliberately messy .NET 10 console application built as a **hands-on refactoring exercise**. The code works (mostly — there are some hidden bugs!) but is written with numerous code smells that need cleaning up.

## What It Does

The application presents a menu with four operations:

1. **Factorial** — Computes the factorial of a given non-negative integer.
2. **Fibonacci** — Prints the Fibonacci sequence up to (and including) a given number.
3. **Primes** — Lists all prime numbers up to a given number.
4. **Sort Characters** — Sorts the characters in a provided string in ascending order (A→Z, a→z).

## How to Run

```bash
dotnet run --project RefactorMe
```

## The Challenge

This codebase has two layers of problems, and the recommended approach is:

### 🧹 Step 1: Refactor

The code is functional but full of code smells — poor naming, duplication, a single giant method, static mutable state, and more. Refactor it into clean, readable, well-structured code by following the task list in [TASKS.md](TASKS.md).

### 🧪 Step 2: Add Unit Tests

Once the logic is extracted into testable methods, write unit tests with known expected outputs. This will reveal that...

### 🐛 Step 3: Fix Bugs

Some of the features produce **incorrect results**. The unit tests from Step 2 should expose them — use the failing tests to find and fix the bugs.
