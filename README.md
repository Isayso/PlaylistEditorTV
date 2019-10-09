
# Playlist Editor TV
Editor for TV m3u files (with vlc media player support)

- NEW in 1.3.7: [ADD] Single column UI mode, hide selected colums [FIX] small errors
- NEW in 1.3.6: [ADD] undo/redo button [FIX] Strg+ check link only selects rows and does not rescan all
- NEW in 1.3.5: change the check stream button to toggle functionality. 
- NEW in 1.3: Check if stream is responding, this is time consuming. Function tries to read first 1k bytes of the stream content.
  

![UI](KodiPlaylistEditorTV1.3a.PNG)
![UI](Singlecolumn.jpg)

- NEW in 1.2: Move line to top of list for faster sorting of favorites. Double buffer for better UI performance.
- NEW in 1.1: Send link to Kodi device e.g. Raspberry PI
- You can edit and create Kodi IPTV playlists, add, rename, move and delete playlist entries, drag&drop m3u files to add to list. 
- Search for names and find duplicate links to merge files. 
- Copy/paste links to other editor window. 
- Play links on Windows with installed VLC player 


![UI](playlisteditTV1.png)

## Getting Started

You can download the compiled EXE file [released](https://github.com/Isayso/PlaylistEditorTV/releases) for Windows 10.  


### Prerequisites

- Windows with .NET Framework 4.6.2
- Installation of VLC player for play function 


### Installing

Unzip and run the exe file. No install necessary.


```
PlaylistEditorTV.exe
```


You can connect the .m3u filename extension with the program or open files with drag and drop on the icon.


## Built With

* [Visual Studio 2017](https://visualstudio.microsoft.com/) - C# with .NET 4.6.2


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Inspired from various IPTV editors for Kodi

## Keyboard shortcuts
- Ctrl + C copy cells
- Ctrl + V paste cells
- Ctrl + F find string
- Ctrl + R copy row
- Ctrl + I insert row
- Ctrl + X cut row
- Ctrl + N open new window
- Ctrl + S save
- Ctrl + P send link to Kodi
- Ctrl + T move line to top of list
- Ctrl + +/- change font size
- delete delete selected row



![GitHub Releases (by Release)](https://img.shields.io/github/downloads/Isayso/PlaylistEditorTV/v1.3.7/total)

![GitHub Releases (by Release)](https://img.shields.io/github/downloads/Isayso/PlaylistEditorTV/v1.3.8/total)
