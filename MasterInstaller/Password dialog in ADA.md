# How to Add Password Validation Logic in Advanced Installer (Step-by-Step)
🔹 1. Open the Dialogs View
In the left sidebar, click Dialogs.

Find your custom dialog (PasswordDlg) or create one by right-clicking an existing dialog like WelcomeDlg → New Dialog → Empty Dialog.

🔹 2. Add a Password Input Field
Select your custom dialog (e.g., PasswordDlg).

From the toolbar, click [ Edit Box ] and add it to your dialog.

In the Properties pane (bottom right):

Set Property Name: USER_PASS

Check the “Password” box to mask input.

🔹 3. Add a "Next" Button
Drag a Push Button to the dialog, label it “Next”.

Select this button.

🔹 4. Show the “Published Events” Pane
If you don’t see Published Events:

Click on the “Next” button.

Then, from the top toolbar, go to:

View → Panels → Published Events
✅ This will open the Published Events section on the right.

🔹 5. Add Events to the "Next" Button
With the “Next” button selected and Published Events visible:

➕ First Event:
Click [ New... ]

Event: Set installer property

Property: PASSWORD_INPUT

Argument: [USER_PASS]

Condition: (leave blank)

➕ Second Event (if password is correct):
Click [ New... ]

Event: New Dialog

Argument: VerifyReadyDlg (or your next dialog)

Condition: USER_PASS = yourpassword

➕ Third Event (if password is wrong):
Click [ New... ]

Event: SpawnDialog

Argument: ErrorDlg

Condition: NOT USER_PASS = yourpassword

🔒 Replace yourpassword with the actual password you want.

🛑 Ensure Nothing Installs If Password Is Wrong
As a backup:

Go to Custom Behavior → Launch Conditions.

Add a new condition:

Condition: USER_PASS = yourpassword

Message: “Incorrect password. Installation will now exit.”

