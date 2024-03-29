# Sonic4_Tools

The latest version of the programs can be found on [GitHub Actions][github_actions] (including dependencies).

More or less suitable for use versions are planned to be published on GitHub's release page.

------------

## Installation

### Linux

Update dependencies, compile tools, and run installation script.

```
bash update_dependencies.sh
bash build_pack.sh
bash install/linux/install.sh
```

## Old Mod Conversion Tool

This program is a tool that converts old mods to make them work properly with the mod loader.

It also can convert **sound and music mods**.

**You need the game to properly convert mods.**

## AMA Editor

A tool for modifying AMA files. It can change relations, possition, size and UV coordinates of 2D sprites (such as buttons and other HUD).

Currently can properly edit only ~10% of all files in both episodes.

## TXB Editor

TeXture Bank Editor.

It can change order of textures, their names and filtering functions (magnification and minifying). It can also add and remove textures in TXB files.

It can properly edit all TXB files.

------------

See https://github.com/OSA413/Sonic4_Tools/blob/master/docs/File%20description.md for more tools

[github_actions]: https://github.com/OSA413/Sonic4_Tools/actions
