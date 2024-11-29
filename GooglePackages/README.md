# Installation

- Uninstall existing Firebase if you have installed it in Assets/.
- Clone this project and place it in the top directory of the project (same level as Assets/).
- Install packages by using Package Manager UI or editing manifest.json.

This install method is not compatible with having EDM4U inside Assets/, delete EDM4U from the project's Assets folder before installing EDM4U through tarball.

## Uninstall existing Firebase in Assets

If you haven't moved Firebase from its default install location, use EDM4U to uninstall it.

In taskbar, go to Assets/External Dependency Manager/Version Handler/Uninstall Managed Packages, check all the Firebase packages and click "Uninstall Selected Packages".

# Import Firebase packages using Unity Package Manager
When importing Firebase products from .tgz files downloaded from the Google APIs for Unity archive, keep the following in mind:

This method is only available in 2018.3+.

If you are using multiple Firebase products in your project, you must download and upgrade all Firebase products to the same version.

Do not mix import methods in one project. That is, do not import Firebase products with the Asset package flow and with the Unity Package Manager flow.

Dependencies for each product .tgz file are linked alongside in their own .tgz files. You must download and import the product .tgz file and dependency .tgz files, in the correct order:

- External Dependency Manager (com.google.external-dependency-manager)
- Firebase Core (com.google.firebase.app)
- Firebase products used in your project. If you use Realtime Database or Cloud Storage, import Authentication (com.google.firebase.auth) first.
After downloading, import .tgz files into your project using one of the following methods:

# Package Manager UI

Open Unity's Package Manager window.

Click the + icon in the top-left corner of the Package Manager window and select Add package from tarball to open the file browser.
Select the desired tarball in the file browser.

### For Unity 2019 and below:
Some older versions of Unity 2019 do not support adding tarballs directly. In this case, you will need to:

- Unzip the .tgz file.
- Click the + icon in the top-left corner of the Package Manager window and select Add package from disk to open the file browser.
Select the extracted folder in the file browser.

# manifest.json

Use a text editor to open Packages/manifest.json under your Unity project folder.
Add an entry for each package you want to import, mapping the package name to the location on disk. Be sure to append file: to the .tgz file path. For example, if you were importing com.google.firebase.storage and its dependency's, your manifest.json would look like this:

```
{
  "dependencies": {
    "com.google.external-dependency-manager": "file:../GooglePackages/com.google.external-dependency-manager-1.2.177.tgz",
    "com.google.firebase.analytics": "file:../GooglePackages/com.google.firebase.analytics-11.8.1.tgz",
    "com.google.firebase.app": "file:../GooglePackages/com.google.firebase.app-11.8.1.tgz",
    "com.google.firebase.crashlytics": "file:../GooglePackages/com.google.firebase.crashlytics-11.8.1.tgz",
    "com.google.firebase.installations": "file:../GooglePackages/com.google.firebase.installations-11.8.1.tgz",
    "com.google.firebase.messaging": "file:../GooglePackages/com.google.firebase.messaging-11.8.1.tgz",
    "com.google.firebase.remote-config": "file:../GooglePackages/com.google.firebase.remote-config-11.8.1.tgz",
  }
}
```

Save the manifest.json file.

When Unity regains focus it will reload the manifest.json and import the newly-added packages.

# gitignore

Add the following to .gitignore of the repo to reduce amount of unnecessary files on the repo taking up space:

```
# Firebase large x86_64 (Windows) files
**/Assets/GeneratedLocalRepo/Firebase/m2repository/com/google/firebase/firebase-app-unity/*/firebase-app-unity-*.aar
**/Assets/GeneratedLocalRepo*
```
