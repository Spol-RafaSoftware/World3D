# World3D
A windows application for displaying real Earth terrain using OpenTK.

A note on the coordinate system: It is right-handed.
Object and World space: +x = north (Azimuth = 0), +y = up, +z = east (Azimuth = pi/2).
Note that increasing Azimuth rotates clockwise, effectively a negative rotation in a right-handed system.
View space: +x = right, +y = up, +z = backwards.
