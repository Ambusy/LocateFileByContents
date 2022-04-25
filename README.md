# LocateFileByContents
Browse a map on a Windows PC and locate files that contain a text or combined texts

Run the FindFile.exe.

How to use:
1. choose a file filter (if not specified \*.\* is used)
2. browse to the starting directory, choose whether to include hidden maps, or not.
3. enter at least one text to be present in the requested file and add it to the list.
4. if more than one text is entered, name the connecting boolean operator & or |. Normal logic is applied, without braces. & preceeds |.
5. Hit Search to start.

Each file will be displayed as well as possible, search terms are highlighted. 
While looking at this file, the next one is already being located.

If at any moment you want to abandon searching a specifid directory at a lower level, click on its name. If the response is not immediate, retry clicking.
To end all searching, click STOP.

The program is build in Visual Studio Community 2019, feel free to improve it.
