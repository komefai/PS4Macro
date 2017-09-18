# PS4 Macro

Automation utility for PS4 Remote Play written in C# using [PS4RemotePlayInterceptor](https://github.com/komefai/PS4RemotePlayInterceptor).

#### Screenshot

![Screenshot](https://raw.githubusercontent.com/komefai/PS4Macro/master/_resources/Screenshot_0_3_0.png)

## Usage

**Download latest version [here](https://github.com/komefai/PS4Macro/releases)!**

⚠️ You must have DualShock 4 controller plugged in via USB with PS4 Remote Play running. 

To record, click on record button (Ctrl+R) to arm recording then press play to start recording controls. To stop recording, click on record button (Ctrl+R) to unarm. The macro will then play the controls in a loop.

See [this video](https://youtu.be/txI9AOEAk58) for basic usage / making of.

## Settings

You can create `settings.xml` and place it with the executable to override default settings.

##### Example settings.xml

This example settings will enable AutoInject and load MyMacro.xml at startup.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Settings>
  <AutoInject>true</AutoInject>
  <StartupFile>MyMacro.xml</StartupFile>
</Settings>
```

## Scripting

C# scripting support has been introduced in version 0.3.0 and later. This allows us to create custom behaviors beyond repeating macros with an easy-to-use API. The API also includes wrapped convenience functions such as pressing buttons, timing, and taking a screenshot from PS4 Remote Play. See the [scripting video tutorial](https://youtu.be/daCb97rbimA) to get started or see the full documentation in [the wiki](https://github.com/komefai/PS4Macro/wiki).

The script have to include a reference to `PS4MacroAPI.dll` to interface with PS4Macro. At the moment the scripts has to be compiled into a DLL file to be able to open with PS4 Macro.

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

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        Press(new DualShockState() { DPad_Up = true });
        Sleep(1000);
        Press(new DualShockState() { Square = true });
    }
}
```

#### List of Scripts

- TBD

## Troubleshoot

##### Macro not playing/recording

=> Disable AutoInject in settings.xml since some machines does not support AutoInject.

##### Visual Studio Build Error

=> Reinstall NuGet Package.

```
Update-Package –reinstall PS4RemotePlayInterceptor
```

## To-Do List

- ~~Save/Load~~
- ~~Keyboard Shortcuts~~
- ~~Status Indicators~~
- ~~Scripting~~
- Scripting API Docs
- Playback Timeline UI
- Use without DualShock controller
- ...

## Resources

- [Tutorial Video](https://youtu.be/txI9AOEAk58)
- [Scripting Tutorial Video](https://youtu.be/daCb97rbimA)
- [Prototype Demo Video](https://youtu.be/QjTZsPR-BcI)

## Credits

- [EasyHook](https://easyhook.github.io/)
- [Jays2Kings/DS4Windows](https://github.com/Jays2Kings/DS4Windows)
- [jforshee/ImageHashing](https://github.com/jforshee/ImageHashing)
