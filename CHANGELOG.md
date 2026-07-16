# 0.0.11

- fixed some issues when LunarConfig was not present
- fixed an error caused by disabled sorting types selected in the config

# 0.0.10

- added `Lunar` sorting type to use LunarConfig's custom indexing (thanks, `r00kieg`!)
- added a check for Company levels to `Simulate` command
- added a check for Company levels to `Scan` command (instead of just Gordion)
- fixed `Difficulty` sorting type not working and sorting incorrectly (thanks, `brobro1616`!)
- fixed store displaying wrong discounts (thanks, `leubesgames`!)

# 0.0.9

- added `Difficulty` sorting type to the moon catalogue (thanks, `brksonance`!)

# 0.0.8

- added a new calculation method for the terminal scrollbar (thanks, `magicwesley`!)
  - this should fix issues with the terminal scrolling too fast on longer text nodes
  - the old behavior can be restored in the config
- added a toggle for hiding locked moons in the moon catalogue (thanks, `unluckyjori`!)

# 0.0.7

- added compatibility with [LethalConstellations](https://thunderstore.io/c/lethal-company/p/darmuh/LethalConstellations/)
  - `preview constellation` will display the moon's constellation in the terminal
- added compatibility with [LethalMoonUnlocks](https://thunderstore.io/c/lethal-company/p/explodingMods/LethalMoonUnlocks/)
  - `preview LMU` will display the LMU tags in the terminal using LMU settings (line width etc.)
- fixed `Simulate` command not displaying the % chance correctly (thanks: `virustlnr`, `generic_gmd`!)
- fixed `Store` node not displaying the discounted prices (thanks, `leubesgames`!)
- fixed `Moon Catalogue` node not displaying hyphenated moon names (thanks, `homicidal_lemon`!)
- fixed `Moon Catalogue` node displaying items disabled from the store (thanks, `homicidal_lemon`!)

# 0.0.6

- fixed an error with the store catalogue displaying items not in rotation

# 0.0.5

- fixed an error with adding already existing info types to the terminal manager (thanks, `explodingturtles456`!)

# 0.0.4

- fixed moon catalogue displaying `None` as a weather
- fixed moon catalogue ordering to match vanilla order (thanks, `cdub_12`!)
- fixed moon catalogue not displaying `* ` decoration for moons
- fixed disabled nodes from not being disabled

# 0.0.3

- added StoreCatalogue node
- added store sorting types: `none`, `name`, `price`
  - to sort the store catalogue, use `store <sortType>` (e.g. `store name` to sort by name)
- added scrollbar patch (moved from TerminalFormatter)

# 0.0.2

- updated README

# 0.0.1

- hey, this is a first version!
