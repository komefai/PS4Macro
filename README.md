# PS4 Macro

Automation utility for PS4 Remote Play written in C#.

#### Screenshot

![Screenshot](https://raw.githubusercontent.com/komefai/PS4Macro/master/Screenshot.png)

#### Usage

You must have DualShock 4 controller plugged in via USB with PS4 Remote Play running. 

To record, click on record button to arm recording then press play to start recording controls. To stop recording, click on record button to unarm. The macro will then play the controls in a loop.

See [this video](https://youtu.be/txI9AOEAk58) for more details.

#### To-Do List

- Save/Load
- Hotkeys
- Playback Timeline UI
- Status Indicators
- Scripting
- ...

#### Troubleshoot

Reinstall NuGet Package

```
Update-Package –reinstall PS4RemotePlayInterceptor
```

#### Resources

- [PS4RemotePlayInterceptor](https://github.com/komefai/PS4RemotePlayInterceptor)
- [Tutorial Video](https://youtu.be/txI9AOEAk58)

#### Credits

- [EasyHook](https://easyhook.github.io/)
