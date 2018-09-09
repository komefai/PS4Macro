# PS4 Macro

[![Twitter](https://img.shields.io/twitter/url/https/twitter.com/fold_left.svg?style=social&label=Follow%20Me)](https://twitter.com/itskomefai)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](http://paypal.me/Komefai)

Automation utility for PS4 Remote Play written in C# using [PS4RemotePlayInterceptor](https://github.com/komefai/PS4RemotePlayInterceptor).

🔔 **Download latest version [here](https://github.com/komefai/PS4Macro/releases)!**

#### Screenshot

![Screenshot](https://raw.githubusercontent.com/komefai/PS4Macro/master/_resources/Screenshot_0_5_2.png)

## Usage

⚠️ To use WITHOUT a controller plugged in, see `EmulateController` in [Settings](https://github.com/komefai/PS4Macro#settings) section below.

##### Easy Way (shortcut)

Press the touch button on your controller (touchpad) to start recording and press it again to stop.

> NOTE: If you're using the touch button in the macro then disable it by going to Playback->Record On Touch

##### Manual Way

To record, click on `RECORD` button (Ctrl+R) to arm recording then press `PLAY` to start recording controls. The red text on the bottom right indicates the number of frames recorded. You can stop recording by clicking on `RECORD` button (Ctrl+R) again. The macro will then play the controls in a loop.

## Settings

You can create `settings.xml` using a text editor and place it in the same folder as `PS4Macro.exe` to override default settings.

| Setting | Description | Default
| --- | --- | --- |
| AutoInject | Automatically poll for PS4 Remote Play and inject whenever possible | false |
| BypassInjection | Bypass the injection for debugging purposes | false |
| EmulateController | Run with controller emulation (use without a controller) | false |
| ShowConsole | Open debugging console on launch | false |
| StartupFile | Absolute or relative path to the file to load on launch (can be xml or dll) | null |

##### Example settings.xml

```xml
<?xml version="1.0" encoding="utf-8"?>
<Settings>
  <AutoInject>true</AutoInject>
  <BypassInjection>false</BypassInjection>
  <EmulateController>true</EmulateController>
  <ShowConsole>true</ShowConsole>
  <StartupFile>MyMacro.xml</StartupFile>
</Settings>
```

## Command Line Arguments

As of version 0.5.0, you can pass command line arguments to PS4Macro.exe and override the values in settings.xml. This also allows you to create multiple shortcuts to PS4Macro.exe and have each of them override the settings when switching between games (recommended for advanced users).

#### Arguments

| Argument | Description | Default
| --- | --- | --- |
| SettingsFile | Absolute or relative path to the settings file (will take priority) | null |

#### Examples

##### Override settings using arguments

```bash
C:\> PS4Macro.exe --AutoInject --EmulateController --ShowConsole=false --StartupFile="C:\macro.xml"
```

##### Override default settings file (highest priority)

```bash
C:\> PS4Macro.exe --SettingsFile="C:\custom-settings.xml"
```

##### Using Windows shortcut

Right-click on `PS4Macro.exe` and click on `Create shortcut` to create a new shortcut. Right-click on the newly created shortcut and select `Properties` and append your command line arguments after the existing text in the `Target` field.

![Command Line Shortcut](https://raw.githubusercontent.com/komefai/PS4Macro/master/_resources/CmdShortcut.png)

## Remapper

Remapper allows you to use your keyboard to control PS4 games with customizable key bindings. To use Remapper, go to Tools->Remapper and focus on PS4 Remote Play to control the game. Simply close the window to return to macro or script mode.

To map a key to a button or a macro, edit the **Key** cell and enter your desire key. You can find the key from the **Member name** column in [this table](https://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx) (eg. `Delete`, `NumPad4`, `PageDown`). Use key `None` to completely disable the key.

To add a recorded macro, click on `...` to browse and select an xml macro file.

![Remapper](https://raw.githubusercontent.com/komefai/PS4Macro/master/_resources/Remapper.png)

## Scripting

C# scripting support has been introduced in version 0.3.0 and later. This allows us to create custom behaviors beyond repeating macros with an easy-to-use API. The API also includes wrapped convenience functions such as pressing buttons, timing, and taking a screenshot from PS4 Remote Play. 

See the [scripting video tutorial](https://youtu.be/daCb97rbimA) to get started or see [the wiki](https://github.com/komefai/PS4Macro/wiki) for full documentation, examples, and other information.

NOTE: The script have to include a reference to `PS4MacroAPI.dll` to interface with PS4Macro. At the moment the scripts has to be compiled into a DLL file to be able to open with PS4 Macro.

##### Basic Example Script

This example script will press DPad up and wait one second, follow by pressing square. The loop repeats every 800ms.

```csharp
using PS4MacroAPI;

public class Script : ScriptBase
{
    /* Constructor */
    public Script()
    {
        Config.Name = "Example Script";
        Config.LoopDelay = 800;
    }

    // Called when the user pressed play
    public override void Start()
    {
        base.Start();
    }

    // Called every interval set by LoopDelay
    public override void Update()
    {
        Press(new DualShockState() { DPad_Up = true });
        Sleep(1000);
        Press(new DualShockState() { Square = true });
    }
}
```

#### List of Scripts

- [Keyboard Remapping Utility](https://github.com/komefai/PS4Macro.Remote)
- [Marvel Heroes Omega Bot](https://github.com/komefai/PS4Macro.MarvelHeroesOmega)
- [PES2018 Bot (Simulator Mode)](https://github.com/leguims/PS4Macro.PES2018Lite) by [leguims](https://github.com/leguims)

---

## Troubleshoot

##### Macro not playing/recording

=> Disable AutoInject in settings.xml since some machines does not support AutoInject.

##### EmulateController does not work

=> Make sure you unplug every DualShock 4 controllers from your computer (otherwise the real controller will take priority over the emulated one). Start PS4 Remote Play, follow by PS4 Macro and wait for this screen. If you see the text `Press the OPTIONS button on the controller to start.` then it means that the emulated controller is working correctly. You can then press the Start button.

![Emulate Controller Troubleshoot](https://raw.githubusercontent.com/komefai/PS4Macro/master/_resources/EmulateControllerTroubleshoot.png)

##### Visual Studio Build Error

=> Reinstall NuGet Package.

```
Update-Package –reinstall PS4RemotePlayInterceptor
```

## To-Do List

- Improve scripting API docs
- Playback timeline UI
- Macro editor tool
- Mouse support for Remapper
- ...

## Resources

- [Making Of Video](https://youtu.be/txI9AOEAk58)
- [Scripting Tutorial Video](https://youtu.be/daCb97rbimA)
- [Prototype Demo Video](https://youtu.be/QjTZsPR-BcI)

## Credits

- [EasyHook](https://easyhook.github.io/)
- [Jays2Kings/DS4Windows](https://github.com/Jays2Kings/DS4Windows)
- [jforshee/ImageHashing](https://github.com/jforshee/ImageHashing)
- [Mono.Options](https://www.nuget.org/packages/Mono.Options/)
