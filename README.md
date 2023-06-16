# ArkGollum

Gollum will hunt your directories for precious so you don't have to.

ArkGollum is a command line tool to exfiltrate various codes useful to server admins and players.

## Ark Gollum can: ##
- [x] Extract spawn codes from all your Ark mods in one pass and deposit a txt document in each respective directory detailing all the information he found.

- [x] Generate the Structures Plus Pullresourceaddition string on a per mod basis.

- [x] Generate a Simple Spawners compatible import list on a per mod basis.

- [x] Purge the results of previous operations, cleaning the storage medium of ArkGollum generated files.
    
- [ ] Work with the Steam Workshop local cache to avoid the need for having the mod installed in the game folder structure.
      
- [ ] Parse vanilla items from the game directory structure.
    
- [ ] Create GFI codes

## Usage ##

ArkGollum provides help via the --help or -h command parameter. In order to scan your current mods, use the -m option (mod mode) and assuming your ARK install is located at c:\SteamLibrary\steamapps\common\ARK you would type:

`ArkGollum -s -S -m MOD -t c:\SteamLibrary\steamapps\common\ARK `
