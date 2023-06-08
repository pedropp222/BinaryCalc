# BinaryCalc

This is a C# console program that lets you calculate and evaluate expressions to binary, hex and decimal numbers.

## Features

- Conversion between binary/decimal/hexadecimal/octal
- 2's complement representation
- Capable of executing expressions such as adding, subtraction and bitwise `AND`,`OR`,`NOT` and more
- Expressions can have multiple types of values, so `135 + NOT(F OR 10011)` is a valid expression (all numbers get normalized to the biggest numeric type, so in this case all three numbers will be hexadecimal)
- Somewhat pretty error messages in all operations (lexing, parsing, executing)
- Value overflow detection
- Passing Unit Test suite

## Guide

The initial state of the program is a `>` character indicating that it's expecting an expression input. Type anything here and press Enter to compute the result.

As an example, type `1111` and press Enter. A result message will appear below that reads something like: `Expression Result: 1111(d15|-1)(xF)`. This is the result of the expression, first as a binary number and then as an unsigned decimal, 2's complement decimal and finally the hexadecimal value.

If you wish that `1111` was treated as something other that binary, like decimal or hexadecimal, you can prefix the number with `b` for binary, `d` for decimal or `x` for hex. So, if you now type `d1111` a different result will appear: `Expression Result: 010001010111(d1111|1111)(x457)`, indicating that the number was decimal.

## Available operations

- `+`: add the left and right values
- `-`: subtract the left and right values or set right value to negative
- `AND`: bitwise AND the left and right values
- `OR`: bitwise OR the left and right values
- `XOR`: bitwise XOR the left and right values
- `NAND`: bitwise NAND the left and right values
- `NOT`: negate the value at the right
- `>` : right shift the left side value by the right side amount
- `<` : left shift the left side value by the right side amount
- `()`: you can use parentheses to control the order of execution
- `b/d/x/q/o`: you can prefix the numeric values to make sure it gets represented correctly(e.g., `10101` could be binary or something else, use `b10101` to enforce binary, and so on).

## Examples

Here is a table of expression inputs and their respective output.

| Input                               | Output                                          |
|-------------------------------------|-------------------------------------------------|
| `101010`                            | `101010(d42\|-22)(x2A)`                         |
| `d101010`                           | `00011000101010010010(d101010\|101010)(x18A92)` |
| `1100 + 1000`                       | `0100(d4\|4)(x4) -> Overflow.`                  |
| `75 - 12`                           | `00111111(d63\|63)(x3F)`                        |
| `50 - 70`                           | `11101100(d236\|-20)(xEC)`                      |
| `99 - d100`                         | `11111111(d255\|-1)(xFF)`                       |
| `xF AND xA`                         | `1010(d10\|-6)(xA)`                             |
| `x5478 AND xFDEA`                   | `0101010001101000(d21608\|21608)(x5468)`        |
| `NOT(NOT(xDEFA) AND (NOT(xFFFF)))`  | `1111111111111111(d65535\|-1)(xFFFF)`           |
| `NOT((0 OR 1) AND (0 AND 0 AND 1))` | `1(d1\|-1)(x1)`                                 |
| `128 > 2`                           | `0010 0000(d32\|32)(x20)`                       |
| `-64 < 3`                           | `1110 0000 0000 (d3584\|-512)(xE00)`            |
| `xEC < d3`                           | `0110 0000 (d96\|96)(x60)`                     |
| `-d20-10`                           | `1110 0010 (d226\|-30)(xE2)`                    |
| `-250+50`                           | `1111 0011 1000 (d3896\|-200)(xF38)`            |
| `-151--20`                          | `1111 0111 1101 (d3965\|-131)(xF7D)`            |
| `--5- --3`                          | `0010 (d2\|2)(x2)`                              |

## Todo

- IEEE floating point numbers
- Other things
