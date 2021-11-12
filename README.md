# ShmubiFlix
An offline Netflix like experience for your video files!
***ShmubiFlix is still in early alpha and does not provide full functionality.***
Usage:
In order to use the application, you will need to first compile it locally and than structure your media files. Follow these steps:

1.Running the application:
  1.1 Clone the repository
  1.2 Launch via visual studio
  1.3 Compile and launch
  
2.Structuring the media folder:
  Your media folder should follow this structure:
  
  Root
  |
  -- Series1
  |        |
  |        -- Thumbnails
  |        -- Season1
  |        -- Season2 and so on..
  |        
  -- Series2         
           |
           -- Thumbnails
           -- Season1
           -- Season2 and so on..
           
 Series1 and Series2 should be replaced with your series name
 Thumbnails folder should contain a .jpg file with a thumbnail image for the series.
  

Known bugs:
-Media playback won't stop if you press the go back button.
-Hovering the mouse over a series thumbnail will make the image disappear.
-Application crashes when finished playing the last file of a season.
