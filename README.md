
# Playlist Editor TV
Editor for IPTV m3u files (with vlc media player and kodi support)

1.5 bugfixes in player UI, fade-out effect

1.4.8 search TV station with keyboard, shift to first found row. 


1.4.7 better find function with logical AND (&) and easy switch between selecting cells or row. Selected cell region can be filled from clipboard. 

1.4.6 player window with opacity, hotkey function can be switched off. 

1.4.5 bugfixes, move row after sort. 

1.4.4 send to Kodi button in channel switch window, password error fixed

1.4.3 New option for start-up with defined file, bugfixes.

1.4.2 Channel switch window, replacing single column mode. Search strings are now highlighed. Not tested Links are grey, dead links orange.

1.4: Re-design of copy/paste function and context menu. Imports a full list from clipboard, no file save before necessary.  Single column mode for better play with vlc. Send to Kodi button. Auto hide empty columns on file load. Check button if link is responding. 
  Special thanks to dobbelina for helpful ideas and testing, improved the usablility a lot. 

![UI](screenshot_1.4.PNG)
![UI](KodiPlaylistEditorTV1.3a.PNG)


- Move line to top of list for faster sorting of favorites. Double buffer for better UI performance.
- Send link to Kodi device e.g. Raspberry PI
- You can edit and create Kodi IPTV playlists, add, rename, move and delete playlist entries, drag&drop m3u files to add to list. 
- Search for names and find duplicate links to merge files. 
- Copy/paste links to other editor window. 
- Play links on Windows with installed VLC player 

![UI](player.png)



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
- Ctrl + 1/2 move line up/down
- del    delete selected row

![GitHub Releases (by Release)](https://img.shields.io/github/downloads/Isayso/PlaylistEditorTV/total)


![GitHub Releases (by Release)](https://img.shields.io/github/downloads/Isayso/PlaylistEditorTV/v1.5/total)
