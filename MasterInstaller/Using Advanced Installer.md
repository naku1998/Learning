# Adding Sysmon
Go to “Files and Folders”.

Place Sysmon64.exe and sysmonconfig.xml into a folder (e.g., APPDIR\Sysmon).

Go to “Custom Actions”.

Click “Add Custom Action” → “Launch EXE with working directory”.

Choose Sysmon64.exe as the file to run.

Set the Command Line to:

css
Copy
Edit
-accepteula -i sysmonconfig.xml
Set the Working Directory to the folder where both files are added (e.g., APPDIR\Sysmon).

Set the Execution Time to:

“When the system is being modified (deferred)”

Enable “Run as Administrator” or “deferred with no impersonation”

✅ WHEN TO USE PREREQUISITES
Use Prerequisites when you need to install another app or component BEFORE your main app starts installing.

🧠 Think: “This must be installed first.”
✅ Example:
You want to install .NET Framework, SQL Server, or another MSI/EXE app (like Sysmon, Wazuh, etc.) before your tray app.

Your tray app won’t run without this.

📍Where to add:
Prerequisites Page → “Install before main package”

✅ WHEN TO USE CUSTOM ACTIONS
Use Custom Actions when you want to run something during or after your main app install — like scripts, EXEs, or even another MSI.

🧠 Think: “Run this while installing my app.”
✅ Example:
You want to run a script to register a service or firewall rule after files are copied.

You want to run a silent MSI install during your tray app's install.

📍Where to add:
Custom Actions Page → Add it in the Install Execution Stage

✅ WHEN TO USE NEITHER
You don’t need Prerequisites or Custom Actions when:

✅ Example:
You're just copying files and want no extra steps — a simple tray app with no dependencies.

You're building a single MSI that just places files and registers a shortcut.

🎯 You only use Files and Folders + Shortcuts + Registry entries.




🔄 Summary Table:
Task                                                	Use Prerequisite?           	  Use Custom Action?
Install .NET Framework first	                           ✅ Yes                           	❌ No
Run PowerShell or CMD script	                           ❌ No	                            ✅ Yes
Install Sysmon before main app	                           ✅ Yes                           	❌ No
Run MSI installer silently during app install	           ❌ No	                            ✅ Yes
Just copy files (no logic needed)                          ❌ No	                            ❌ No





# If you want to install another MSI as part of your Advanced Installer setup, you have two choices, depending on when and how you want it to be installed:

✅ 1. Use a Prerequisite
✅ Best if you want to install the MSI before your main app, and show its UI normally.

Use when:
You want the MSI to show its own full UI (not silent).

The MSI is a dependency (e.g., TrayApp.msi → Wazuh.msi).

How:
Go to Prerequisites page.

Click “New Package Prerequisite”.

Choose your .msi file.

Under Installation Behavior, choose:

✅ Feature-based or Install before main package.

You can uncheck "Install silently" if you want UI to appear.

✅ 2. Use a Custom Action
✅ Best if you want to install the MSI during/after your app and want to run it silently or conditionally.

Use when:
You want to automate the MSI install quietly.

You want the MSI to install after copying your files.

You don’t care about the MSI’s UI or want to hide it.

How:
Go to Custom Actions.

Add Launch File or Open URL.

Point to the .msi in Application Folder.

Add command line:

bash
Copy
Edit
msiexec /i "[#YourMsiFile.msi]" /qn
(remove /qn if you want UI)

❌ Don't just put MSI in Files and Folders
If you just drop the .msi file in Files and Folders, it will be copied, not installed.

Great — since you want Sysmon.exe to be installed silently as a prerequisite, and you're seeing this dialog in Advanced Installer → Prerequisites → UI Levels, here's how to fill it:

✅ Silent Installation for Sysmon.exe as a Prerequisite
When you select UI Level: Silent install, you'll see:

Field	What to Enter
Basic UI	(leave blank or optional)
Silent UI	sysmon.exe -accepteula -i config.xml or your full command

🔍 Field Details:
Basic UI:
This is used if the installation is running with a basic interface (progress bar only).
If you're not planning to use basic mode, you can leave this blank or provide the same command.

Silent UI:
This is the one that runs when the main installer runs silently (i.e., no UI).
Enter your full silent install command for Sysmon, like:

bash
Copy
Edit
sysmon.exe -accepteula -i sysmonconfig.xml


# Great question! The Condition box in Advanced Installer’s Prerequisites page controls whether or not the prerequisite will install, based on a condition you define (like checking if it's already installed).

✅ For Sysmon Prerequisite → Condition Box
📌 Use a registry check or a file check as the condition.
Here are a couple of easy options:

✅ Option 1: File Check (Recommended if Sysmon is not registered in registry)
If you know where Sysmon installs, e.g.,

[SystemFolder]Sysmon64.exe or C:\Windows\Sysmon64.exe

Then use:

java
Copy
Edit
NOT FileExists("C:\Windows\Sysmon64.exe")
This tells the installer: “Only install Sysmon if the file doesn’t already exist.”

✅ Option 2: Registry Check (if Sysmon writes to registry)
If Sysmon adds a registry key (you can confirm manually via regedit), for example:

HKLM\SOFTWARE\Sysinternals\Sysmon

Then use:

java
Copy
Edit
NOT RegistryKeyExists("HKLM\SOFTWARE\Sysinternals\Sysmon")
✅ Option 3: Always Install (if you're unsure)
If you always want to install Sysmon no matter what:

Copy
Edit
1
This forces it to run every time the installer is executed.