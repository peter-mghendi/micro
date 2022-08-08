# micro

A simple open-source console application via which you can interact with your [Mega](https://mega.co.nz) cloud account.

![Demo](https://user-images.githubusercontent.com/26766955/183290119-30f0665a-d4da-4cd0-9d3d-c8950536b0bb.png)

> **Note**
> This project is not affiliated with or endorsed by [Mega](https://mega.co.nz).

## How To Use

1. Download the latest micro release from the [releases page](https://github.com/sixpeteunder/micro/releases).
2. Run the `micro` command in the terminal.

## Supported commands

- [x] exit - Exit the program
- [x] help - Show this help
- [x] pwd - Print the current directory
- [x] ls - List the contents of the current directory
- [x] cd - Change the current directory
- [ ] mkdir - Create a new directory
- [ ] rm - Remove a file or directory
- [ ] mv - Move a file or directory
- [ ] cp - Copy a file or directory
- [ ] cat - Show the contents of a file
- [ ] touch - Create a new file
- [ ] get - Download a file

## Features

- [ ] Command history
- [ ] Command completion
- [ ] Command aliases
- [ ] '.' and '..' navigation
- [ ] Character escaping
- [ ] File globbing
- [ ] File search

## Caveats

In order to keep the command parser simple, some caveats are in place:

- Lines with whitespace MUST be quoted with double quotes.
- Quoted lines MUST always use double quotes.
- Single quotes and backticks inside double quotes are allowed.
- Double quotes inside double quotes are not supported (yet).
- The path separator is `/`.