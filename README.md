# ⚡ Snappy Downloader

A blazing fast, native Windows application to download `.m3u8` (HLS) video streams (like Reddit videos) using FFmpeg. 

**Built with C# WPF. No Electron. No Bloat.**

![Project Status](https://img.shields.io/badge/Status-Active-brightgreen)
![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![License](https://img.shields.io/badge/License-MIT-orange)

## 🚀 Why this app?

Most video downloaders are either slow web apps or heavy "Electron" apps that eat up 200MB+ of RAM just to sit idle. 

**Snappy Downloader** is different:
* **Native Performance:** Runs on the .NET Runtime.
* **Responsive:** Multithreaded architecture means the UI never freezes, even during heavy downloads.
* **Direct Stream Copy:** Uses FFmpeg's `-c copy` to download the raw stream without quality loss or slow re-encoding.
* **Theming:** Includes built-in Light, Dark, and Pastel themes.

## 🛠 Features

* ✅ **Download .m3u8 Links:** Handles Reddit and other HLS playlists.
* ✅ **Custom Save Location:** Native Windows Save Dialog to pick where your file goes.
* ✅ **Themes:** Toggle between Light, Dark, and Solarized themes instantly.

## 📦 How to Use (For Users)

1.  Go to the **[Releases](../../releases)** page.
2.  Download the latest `.zip` file.
3.  Extract the folder.
4.  **Important:** Ensure `ffmpeg.exe` is inside the folder next to the app.
5.  Run `FastVideoDownloader.exe`.

## 💻 How to Build (For Developers)

### Prerequisites
* Visual Studio 2025 (Community Edition is fine).
* .NET 10.0 runtime
* [FFmpeg for Windows](https://www.gyan.dev/ffmpeg/builds/).

### Setup
1.  Clone the repo:
    ```bash
    git clone [https://github.com/YourUsername/FastVideoDownloader.git](https://github.com/YourUsername/FastVideoDownloader.git)
    ```
2.  Open `FastVideoDownloader.sln` in Visual Studio.
3.  **Critical Step:** You must download `ffmpeg.exe` and place it in your Debug/Output directory, or add it to the project and set "Copy to Output Directory" to "Copy Always".
4.  Press `F5` to build and run.

## ⚙️ Tech Stack

* **Language:** C#
* **Framework:** WPF (Windows Presentation Foundation) / .NET
* **Engine:** FFmpeg (Background Process)
* **IDE:** Visual Studio 2025

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---
*Built because I wanted a downloader that opens instantly.*
