unity-frosted-glass [![License](https://img.shields.io/badge/License-MIT-lightgrey.svg?style=flat)](http://mit-license.org) [![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/andyduboc/5usd)
===============

This code reproduce a frosted glass effect in Unity (as seen in DOOM 2k16). It use a CommandBuffer attached to the main camera to render the scene in global rendertextures. A grayscale mask is then used to sample between these rendertextures in order to apply the desired blur.

 ![screenshot](Screenshots/screen0.gif)

Limitations
===============

* Editor view don't render the effect.
* Tested with Unity version 2017.1.1f1

Further Reading
===============

 - [Extending Unity 5 rendering pipeline: Command Buffers](https://blogs.unity3d.com/2015/02/06/extending-unity-5-rendering-pipeline-command-buffers/)
 - [DOOM (2016) - Graphics Study](http://www.adriancourreges.com/blog/2016/09/09/doom-2016-graphics-study/)

License
===============

MIT, see [LICENSE](LICENSE) for details.
