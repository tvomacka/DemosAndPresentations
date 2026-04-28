# Refactoring Task List

Work through these tasks in order. Each builds on the previous one.
The idea is to first clean up the code so it's readable and testable, then add tests to lock down behavior, and finally use those tests to find and fix bugs.

---

## Phase 1: Naming

- [ ] Rename variables to be descriptive (`r`, `s`, `n`, `inp`, `cnt`, `ip`, `c`, `tmp`, etc.)
- [ ] Rename the `res` and `arr` static fields (or eliminate them entirely)

---

## Phase 2: Eliminate Duplicate Code

- [ ] The input-parsing pattern (`Console.Write` → `ReadLine` → `int.Parse` in try/catch) is repeated three times — extract it
- [ ] The result-string building loop (joining array elements with commas) is duplicated between Fibonacci and Primes — extract it
- [ ] The output formatting block (`=== Result ===`) is repeated four times — extract it

---

## Phase 3: Remove Static Mutable State

- [ ] Get rid of the static `res`, `arr`, and `cnt` fields
- [ ] Use local variables or return values instead

---

## Phase 4: Extract Methods / Classes

- [ ] Extract each feature into its own method (e.g., `ComputeFactorial`, `GetFibonacciSequence`, `GetPrimes`, `SortCharacters`)
- [ ] Consider whether a separate class per feature makes sense
- [ ] Keep `Main` as a thin loop that only handles menu display and dispatch

---

## Phase 5: Improve Error Handling

- [ ] Replace empty `catch { }` blocks with proper validation (e.g., `int.TryParse`)
- [ ] Consider what should happen for edge cases (negative numbers, very large numbers, empty strings)

---

## Phase 6: Use Built-in APIs

- [ ] Replace manual string concatenation in loops with `string.Join`
- [ ] Replace the hand-written bubble sort with `Array.Sort`
- [ ] Replace the fixed-size `int[1000]` array with a `List<int>`

---

## Phase 7: Add Unit Tests

Now that the logic is extracted into testable methods, add unit tests to verify correctness.

- [ ] Create a test project (e.g., `RefactorMe.Tests` with xUnit or NUnit)
- [ ] Write tests for each extracted method using known inputs and expected outputs:
  - Factorial: `5! = 120`, `0! = 1`, `1! = 1`
  - Fibonacci up to 10: `0, 1, 1, 2, 3, 5, 8`
  - Primes up to 10: `2, 3, 5, 7`
  - Sort `"hello"`: `"ehllo"`
- [ ] Run the tests — some should fail, revealing bugs in the original logic

---

## Phase 8: Find and Fix Bugs

Use the failing tests from Phase 7 to guide you.

- [ ] Investigate each failing test and trace it back to the root cause in the code
- [ ] Fix the bugs and re-run the tests until they all pass

---

## Bonus

- [ ] Improve the prime-checking algorithm (hint: you only need to check divisors up to √n)
