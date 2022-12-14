# GUI2P Vanity Generator

A simple tool that adds a user interface and additional functionality to the existing i2pd-tools vanity generator.

Features:
 - Simple user interface
 - Web interface with extended functionality
 - Multiple filter support
 - Configurable thread count
 - Saving / Loading filter list
 - .NET 6.0
 - Uses the original i2pd-tools' executables
 - Saves found addresses to unique folders with private.dat
 - Continuous scanning after finding address
 - Multi-threaded
 - Logging
 - Duplicate removal
 - List sorting by alphabetical, length, and random.
 - Bad key file and false positive detection and deletion (Appears to be a bug in i2pd-tools)
 
## Dependencies
 
  - vain.exe - From the /PurpleI2p/i2pd-tools/ repository
   - Moved to application startup path
  - keyinfo.exe - From the /PurpleI2p/i2pd-tools/ repository
   - Moved to application startup path
  - .NET 6.0
  
## Source

 - Install Visual Studio 2022 (recommended) or recent
 - Unblock resource files
 - Clean Solution
 - Resolve Dependencies
 - Build Solution for x64 Release target
