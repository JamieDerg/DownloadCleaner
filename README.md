# DownloadCleaner
Personal Project, a Program that organizes your download folder, based on set categories

In order to use it, you need to create a settings.json File in the same folder the exe is located in.

The structure is the following:

```js
{
  
  //the Path of the download folder
	"DownloadPath": "C:\\Downloads",
  
  //the minimum time that need to elapse, before it gets sorted
	"MinimumMoveTime": "3",
  
  //the name of the folder where everything that cant be sorted goes into
	"UnknownExtensionFolderName": "misc",
    
    //array of FileTypes, each Object represents a folder structure
  "FileTypes": [
    {
      //Name of the category, unused currently
      "TypeName": "Dokumente",

      //where the file gets saved to, root is the Download folder
      "Path": "dokumente",

      //List of all extensions this FileType belongs to
      "Extensions": [
        ".log",
        ".md",
        ".doc",
        ".docx",
        ".odt",
        ".txt",
      ],
       
       //list of SubTypes, get used for nested folders.
      "SubTypes":[
        {
          "TypeName": "Powerpoint",

          //uses the path of the main FileType as its root
          "Path": "powerpoint",
          "Extensions": [
            ".odp",
            ".pptx"
          ]
        },
      ]
    }
  ]
}
```
