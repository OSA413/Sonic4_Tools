# :pencil: Description of file extensions :card_file_box:

*may be incorrect or incomplete*

File extension | Description | Program(s) | Usage
-------------- | ----------- | ---------- | -----
ACB | Audio container | Probably [Skyth's AcbEditor][sonicaudiotools_link]
:star: ADX | Criware's audio file | Any WAV/MP3 to ADX converter (e.g. [Audacity][audacity_link] [with FFmpeg][audacity_ffmpeg_instruction]¹, or [FFmpeg alone][ffmpeg_link]) | EP1 EP2
:dizzy: AMA | Something sprites related | ??? | EP1 EP2
:star: AMB | Game asset (textures, models...) container | [AMBPatcher][modloader_link] | EP1 EP2
AME | Something particles/effects related | ??? | EP1 EP2
AT  | ??? | ??? | EP1 EP2
AYK | ??? | ??? | EP2
:star: CPK | Criware's audio containter | [CsbEditor from Sonic Audio Tools][sonicaudiotools_link] | EP1 EP2
:star: CSB | Criware's audio containter | [CsbEditor from Sonic Audio Tools][sonicaudiotools_link] | EP1 EP2
DC  | ??? | ??? | EP1 EP2
:star: DDS | DirectDraw Surface (image) | Any image editor that supports DDS (GIMP, PhotoShop, etc) | EP1 EP2
DF  | ??? | ??? | EP1 EP2
DI  | ??? | ??? | EP1 EP2
EV  | ??? | ??? | EP1 EP2
GPB | ??? | ??? | EP2
:star: GVR | GVR texture | [Puyo Tools][puyo_tools_link]
H | Looks like a C(++) header | ???
HBM | ??? | ???
LTS | ??? | ??? | EP2
MD  | ??? | ??? | EP1 EP2
MFS | ??? | ??? | EP2
MP  | ??? | ??? | EP1 EP2
MSG | ??? | ??? | EP2
PSH | Compiled HLSL | ??? | EP1 EP2
:star: PVR | PowerVR texture | [PVRTexTool from PowerVR SDK][powervr_sdk_link]
RG  | ??? | ??? | EP1 EP2
SSS | ??? | ??? | EP2
:zzz: TXB | ??? (probably TeXture Boundary) | ??? | EP1 EP2
VSH | Compiled HLSL | ??? | EP1 EP2
YSD | This one holds in-game credits | ??? | EP1 EP2
ENO/ENM | Model/animation | [Dario's LibGens][libgens_link] (requires modification)
INO/INM/INV | Model/animation/??? | [Dario's LibGens][libgens_link] (requires modification)
GNO/GNM | Model/animation | [Dario's LibGens][libgens_link] (requires modification)
XNO/XNM | Model/animation | [Dario's LibGens][libgens_link] (requires modification) | EP2
ZNO/ZNM/ZNV | Model/animation/??? | [Dario's LibGens][libgens_link] (requires modification) | EP1 EP2
LNO | Model | ???

Icon | Legend
-----|------------
:star: | Well editable
:dizzy: | Deep research required
:zzz: | Probably changes nothing

¹ - (This applies to Windows) Since [Audacity is still using old 2.2.2 ffmpeg](https://forum.audacityteam.org/viewtopic.php?f=20&t=105590), it can't properly export ADX files (sample number isn't written to file), but it can import them. You will need to use newer version (4.1.3 tested) to export them.

[modloader_link]: https://github.com/OSA413/Sonic4_ModLoader
[tools_link]: https://github.com/OSA413/Sonic4_Tools
[sonicaudiotools_link]: https://github.com/blueskythlikesclouds/SonicAudioTools
[libgens_link]: https://github.com/DarioSamo/libgens-sonicglvl
[powervr_sdk_link]: https://www.imgtec.com/developers/powervr-sdk-tools/
[puyo_tools_link]: https://github.com/nickworonekin/puyotools
[audacity_link]: https://www.audacityteam.org/
[audacity_ffmpeg_instruction]: https://manual.audacityteam.org/man/faq_installation_and_plug_ins.html#ffdown
[ffmpeg_link]: https://ffmpeg.org/
