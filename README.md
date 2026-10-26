# ASS Version Control

**A desktop application for comparing subtitle versions in ASS and SSA formats.**

ASS Version Control helps identify differences between two versions of a subtitle file, making it easier to review edits, track changes, and verify corrections without manually comparing entire files.

## Overview

Subtitle editing often involves multiple revisions of the same file. Even small changes to dialogue, timing, or formatting can be difficult to track when comparing versions manually.

ASS Version Control simplifies this process by comparing two subtitle files and presenting their differences in a readable format.

## Features

- **Side-by-side version comparison** — compare an original subtitle file with a revised version.
- **ASS and SSA support** — work with subtitle files in `.ass` and `.ssa` formats.
- **Change detection** — identify differences between the two versions.
- **Readable comparison results** — review changes alongside their associated timestamps and subtitle text.
- **Copyable output** — copy the comparison results for further review or documentation.
- **Desktop interface** — manage the comparison process through a graphical user interface.
- **Cross-platform builds** — support for Windows and Linux x64 targets.

## Screenshot

<img width="894" height="697" alt="Screenshot" src="https://github.com/user-attachments/assets/a045888d-b942-4179-9cf9-2accca8e6cb4" />

## Getting Started

### Download

Visit the [ASS Version Control repository](https://github.com/Smiatana/ASSVersionControl) to access the source code and available project files.

Prebuilt releases, when available, can be found on the [Releases page](https://github.com/Smiatana/ASSVersionControl/releases).

### Usage

1. Launch ASS Version Control.
2. Load the original subtitle file into the first input.
3. Load the revised subtitle file into the second input.
4. Start the comparison using **Find Changes**.
5. Review the detected differences in the results area.
6. Copy the results if you need to save or share them.

Both input files should use a supported `.ass` or `.ssa` extension.

## Building from Source

### Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Git

### Clone the repository

```bash
git clone https://github.com/Smiatana/ASSVersionControl.git
cd ASSVersionControl
```

### Build

```bash
dotnet build AegisubVersionControl.sln
```

### Run

```bash
dotnet run --project AegisubVersionControl.csproj
```

### Publish

To publish a self-contained Windows x64 build:

```bash
dotnet publish AegisubVersionControl.csproj \
  -c Release \
  -r win-x64 \
  --self-contained true
```

To publish a self-contained Linux x64 build:

```bash
dotnet publish AegisubVersionControl.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained true
```

The resulting application files will be placed in the project's publish output directory.

## Technology Stack

- **C# and .NET 9** — application logic and runtime.
- **Avalonia UI** — cross-platform desktop interface.
- **Newtonsoft.Json** — JSON processing.

## Contributing

Contributions, bug reports, and suggestions are welcome.

If you encounter a problem or have an idea for an improvement, please open an [issue](https://github.com/Smiatana/ASSVersionControl/issues) describing the problem or proposed change.

For code contributions, submit a pull request with a clear description of the changes.

## License

Please refer to the repository for licensing information.

---

Developed by [Smiatana](https://github.com/Smiatana).
