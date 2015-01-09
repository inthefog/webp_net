# webp_net
.Net layer for Google libwebp library

## Webp format ##

Google webp format is advanced image format dedicated for web.
It supports both lossy and lossless compression. Smaller that JPEG when lossy. Smaller than PNG when lossless. Supports alpha channel with any compression. 

More information [here](https://developers.google.com/speed/webp/).

## Setup Instructions ##
To use .Net layer, first you need to build libwebp for Windows.

1. NMake is shipped with Visual Studio, so it is better to have one. I am using at the moment VS2012 (VC11) professional, but Express version should do.
2. Download latest libwebp: at the moment [libwebp-0.4.2.tar.gz](http://downloads.webmproject.org/releases/webp/libwebp-0.4.2.tar.gz) and extract it.
3. Following lines should be typed in Windows terminal
	1. cd C:\Program Files (x86)\Microsoft Visual Studio 11.0\VC
	2. vcvarsall.bat x64
	3. cd C:\\...\libwebp-0.4.2
	4. nmake /f Makefile.vc CFG=release-dynamic ARCH=x64 RTLIBCFG=dynamic OBJDIR=output
	5. Copy DLLs libwebp.dll, libwebpdecoder.dll from relative path output\release-dynamic\x64\bin to some folder visible by Windows PATH (but better not in system32).
	
	Note: for x86 build do same steps only with x86 flag in (2) and (4)
4. Restart Visual Studio if open to refresh PATH environment variable.
5. Use Webp wrapper class as example for decoding image. [Full API documentation](https://developers.google.com/speed/webp/docs/api).