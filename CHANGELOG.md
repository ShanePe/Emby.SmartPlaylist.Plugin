# ChangeLog
## Version 2.5.1.4850
  ## Bug
  - Fixed bug with clearing playlist.

## Version 2.5.0.4810
  ## General
  - Code refactor.
  - 4.8 compatibility.
 
## Version 2.4.0.3
  ## Feature
  - Create a copy of a play list. (Duplicate)
  - Decluttered UI by adding a pop up menu to playlists
  - New Criteria:
    - Added Audio Stream Codec Criteria (Stream: Audio Codec)
    - Added Audio Stream Language Criteria (Stream: Audio Language)
    - Added Audio Stream Display title Criteria (Stream: Audio Display Title)
    - Added Subtitle Stream Language Criteria (Stream: Subtitle Language)
    - Added Video Stream Codec Criteria (Stream: Video Codec)
    - Added Video Stream Display title Criteria (Stream: Video Display Title)
  - Updated Criteria:
    - Parental Rating: Now uses official rating under the hood
    - Resolution Height: Now uses video streams to determine the available video heights rather than the metadata.
    - Resolution Width: Now uses video streams to determine the available video widths rather than the metadata.
  - Added Monitor Mode to shuffle playlist type. Meaning that the playlist will be destroyed and recreated on a schedule, but between scheduled executions, media item changes will be batched and monitored for inclusion/exclusion. (So .... combination of live and shuffle modes but could have a perf hit on larger playlists, experimental)
  - Collapsible sections in the playlist editor that keeps state, for more UI real estate.
  - Added the ability to add notes to a playlist (limited to 256 chars). If there are notes assigned a pop up is displayed when hovering over a playlist (2 second delay). 
  - Order all selection lists on criteria with dropdowns alphabetically. 
  
## Version 2.4.0.2
  ## Feature
  - Added Resolution Height Criteria
  - Added Resolution Width Criteria
  - Added Favorite (Tree Crawl) Criteria
  - Allow sorting on Limited items for Shuffle playlist. (Shuffle will destroy and create new playlist, where as normal will just append/remove items that mismatch in filter rules.)

## Version 2.4.0.1
  ## Feature
  - Added Import/Export functionality allowing you to backup and restore playlists.
  - Added Regular Expression (Match and No Match) Operators for string values
  - Added 'Overview' Criteria
  - Added Readable to rules tree
  - Added playlist load failure reason to main Emby Log. 
  - Added 'Smart Playlist (Execute)' Source item. This will first execute the playlist before reading the items from it. 
  - Added More logging around errors and error handling.
- ## Bug
  - Changed the way log files are transferred.

## Version 2.4.0.0
  ## Feature
  - Added verbose log to info section.
  - Added link to open up generated playlist.
  - General optimization and clean up.
  - Added Sort (Then Bys) to Sort Jobs.
  - Added Sorting to source list.
  - Added Multiple Add functionality to PlaylistEditor.
  - Added user based criteria for following user metadata fields:
    - Favorite
    - LastPlayed
    - Play Count
    - Played
  ## Bug
  - If any playlists backend files are corrupt, skips loading the playlist and moves the file to a *.failed file, and continues to load. Currently just crashes out with no notification. 
  - Fixed bug when source was playlist or collection not picking up source items.

## Version 2.3.0.2
- ## Feature
  - Removed "Sort newly added items" if the playlist is in "Live" mode
  - Disabled "Execute" on management ui if the playlist/collection is in "Live" mode
- ## Bug
  - Fixed bug when limit cache clear check is failing as limit is null.
  - Fixed bug when updating a collection: Error Method not found: 'Void MediaBrowser.Controller.Library.ILibraryManager.UpdateItems(System.Collections.Generic.List`1<MediaBrowser.Controller.Entities.BaseItem>, MediaBrowser.Controller.Entities.BaseItem, MediaBrowser.Controller.Library.ItemUpdateType, System.Threading.CancellationToken)'.
- 
## Version 2.3.0.0
- ## Feature
  - Added an 'Is Empty' operator
  - Added better error handling and server/client communication
  - Added Sort Job and Sort Scheduled Task for Playlists (Collections do not support sorting, even by adding and removing in order)
  - Removed "Sort newly Added Items" for Collections as Collections cannot be sorted. 
- ## Bug
  - Removed duplicate criteria: Community Rating. (Existing is: Rating (Community))
  - Moved API calls to use relative paths if URL mapping is a thing.
   
## Version 2.2.0.2
- ## Bug
  - Fixed newly introduced bug when Update type is live and the playlist or collection does not exist yet.
  - Fixed info window duration format. (Playlist will need to execute once to rectify)
  - Fixed playlist stats for live update type.
## Version 2.2.0.1
- ### Performance
  - Optimized Live mode by not pulling all items to compare against, only new items and previous added. git(Hopefully this makes things a little better.)
- ## Feature
  - Made plugin compatible with Emby > 4.7.0.0 (Backward compatible with current release 4.6)
  - Added 'Rating' criteria for:
    - Community
    - Critic
    - Custom
  - Added loader to management screen while loading large amounts of smartplaylists.
- ## Bug
  - Fixed a bug on number input where the value would only be updated blur, meaning that the spinners are rendered useless.
  - Fixed a bug where there was no Genres in the system the editor would fail to load.

## Version 2.1.0.4
- ## Feature
  - Added "Genre (Tree Crawl)" Criteria. Filters the item on tag or its parents tags (and so on, all the way to to root).
- ### Bug
  - Schedule task was not executing scheduled lists correctly.
  - Renaming a playlist. If the Name field is locked the smartplaylist name will be reverted so that the playlist and smartplaylist names stay in sync.

## Version 2.1.0.3
- ### Feature
  - Added "Country" Filter Criteria. Filter item on the "ProductionLocation" metadata.
  - Added "Genre (List)" Filter Criteria. Displays a drop down list of Genres already in the system, if the Genre does not exist the fall back it to use the already existing "Genre" string filter.
  - Added checkbox on delete confirmation to retain the generated playlist/collection
  - Sorted smartplaylist manager playlists/collections alphabetically
  - Added "Tag (Tree Crawl)" Criteria. Filters the item on tag or its parents tags (and so on, all the way to to root).
  - Added "Studio (Tree Crawl)" Criteria. Filters the item on it's studio value or its parents studio value (and so on, all the way to to root).
  - Updating sourcing to navigate to the lowest item in the path.
  - Added new Update types: Daily, Weekly, Monthly, The main difference between this and the shuffle update types is that the shuffle will remove all items in the playlist/collection prior to adding new items, and then shuffle the playlist randomly, where as the scheduled update types will only add new items to to the playlist/collection or remove items no longer meeting the criteria from the playlist/collection and NOT shuffle the playlist/collection. Scheduled update types are way better when it comes to performance as there are only small updates to the the backend after the initial populate.
  - Added updateType Icons
  - Added tooltips to buttons and icons
- ### Bug
  - Fixed a legacy bug where the next shuffle date was calculated based on the last shuffle date isn't of the current run date. Meaning that if the playlist ran twice the shuffle date would be bumped twice.

## Version 2.1.0.2
- ### Performance
  - Optimized addition and removal of items to a playlist or collection.
- ### Bugs
  - Fixed a bug where images could be removed under certain circumstances.
  - Fixed bug where it sometimes would not remove the related playlist when deleting or converting to collection.
  - Fixed when limiting results to 'X' number of items, was not being applied correctly. 

## Version 2.1.0.1
- ### Feature
  - Added Sync Count to playlist statistics
  - Added more detailed status messages to playlist statistics
  - Added the ability to turn playlist syncing on or off
  - Added sort for newly added items
  - Added new source type for playlists
    - Media Items: Use all media items as a base to apply filters on.
    - Playlist: Use an existing playlist as a base to apply filters on.
    - Collection: Use an existing Collection as a base to apply filters on.
  - Added the ability to execute a playlist from the editor.(Pop up is not the actual execution time, you need to check the info to see how the playlist updated)
  - Renamed criteria "Path" to "Path/Filename".  <span style="color:red">**BREAKING CHANGE, please re-select path/filename, save any playlist that uses the criteria**</span>
  - Fixed thumbnail image rendering in plugin list.

## Version 2.1.0.0
- ### Feature
  - Added a new feature that can roll up episode items to either their season or series, this is only available for smart collections. (Emby restriction)
    - EpiModes:
      - Item: Collection will contain episodes
      - Season: Collection will roll up episode to season
      - Series: Collection will roll up episode to series
- Added a new feature that shows smart play list statistics. (Info Icon on smartlist)
- New Layout
- Small UI changes
- Added the following criteria:
  - Studio
  - Added "Music and Home Video" to Media Type. <span style="color:red">**BREAKING CHANGE, please re-select the media type criteria save any playlist that uses the criteria**</span>
- Added confirmation on playlist delete
- ### Bug fix
  - Fixed a bug then when generating a new playlist or collection that an error was raised with 'Invalid Parameter Id'
