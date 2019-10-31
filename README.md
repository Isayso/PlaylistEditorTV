
# Playlist Editor TV
Editor for TV m3u files (with vlc media player and kodi support)

1.4.1 avaliable. 

NEW progress window with cancel for link check. Hotkey to import from clipboard (defined in settings).

NEW in 1.4: Re-design of copy/paste function and context menu. Imports a full list from clipboard, no file save before necessary.  Single column mode for better play with vlc. Send to Kodi button. Auto hide empty columns on file load. Check button if link is responding. 
  Special thanks to dobbelina for helpful ideas and testing, improved the usablility a lot. 

![UI](screenshot_1.4.PNG)
![UI](KodiPlaylistEditorTV1.3a.PNG)


- Move line to top of list for faster sorting of favorites. Double buffer for better UI performance.
- Send link to Kodi device e.g. Raspberry PI
- You can edit and create Kodi IPTV playlists, add, rename, move and delete playlist entries, drag&drop m3u files to add to list. 
- Search for names and find duplicate links to merge files. 
- Copy/paste links to other editor window. 
- Play links on Windows with installed VLC player 

![UI](screenshot2_1.4.PNG)


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
- Ctrl + C copy rows/cells
- Ctrl + V paste rows/cells
- Ctrl + F find string
- Ctrl + I paste insert row
- Ctrl + X cut row
- Ctrl + N open new window
- Ctrl + S save
- Ctrl + P send link to Kodi
- Ctrl + T move line to top of list
- Ctrl + +/- change font size
- del    delete selected row



![GitHub Releases (by Release)](https://img.shields.io/github/downloads/Isayso/PlaylistEditorTV/v1.4/total)

![GitHub Releases (by Release)](https://img.shields.io/github/downloads/Isayso/PlaylistEditorTV/v1.4.1/total)
