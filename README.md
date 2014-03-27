
### About

mINI is a very minimal [INI][] reader. I wrote it because I needed to
populate a ContextMenuStrip at runtime from an INI file each time the
user clicks on it. Most readers I tried were either overkill, too slow
for realtime reloading or didn't support nested sections.

mINI is stateless and does nothing by itself. It doesn't build data
structures to hold the INI file contents either. It's an abstract class
with a bunch of virtual methods. You override the methods to decide what
to do with your data.

For example, you override `OnSection(String section)` to do something when
a section is found or `OnValueEmpty(String key)` to report an error when
a key has no value. The default implementation for all methods does nothing,
so you only need to override the ones you want to handle.

[INI]: http://en.wikipedia.org/wiki/INI_file

### INI syntax

mINI supports the following syntax:

```ini

# comment

; another comment, semicolon syntax

key with spaces = value at root

[section]
key = value
key = value

[section/subsection/more subsections]
key = value
```

Spaces are trimmed around sections, keys and values before calling methods.
Blank lines can trigger custom actions too with `OnEmpty()`. Case sensitivity
and duplicate sections are allowed, it's up to the implementation class
to decide what to do with them.

### Portability

mINI is tested on Windows 7 and 8, using the .NET Framework 4.0+.
Older versions of .NET all the way back to 2.0 should work. Mono should work.
It has no external dependencies.

### Status

This is mostly in beta state. I'll be using it for GaGa (another project).
After enough testing, if no issues are found, I'll consider it completed.

### License

Like all my hobby projects, this is Free Software. See the [Documentation][]
folder for more information. No warranty though.

[Documentation]: https://github.com/Beluki/mINI/tree/master/Documentation

