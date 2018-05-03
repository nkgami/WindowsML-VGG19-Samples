# Windows Machine Learning Samples with VGG-19 model

This repo contains the Windows Machine Learning (WinML) samples (C# /VB console apps and UWP) which utilize VGG-19 pre-trained model in onnx model zoo (https://github.com/onnx/models/tree/master/vgg19).
You can find additional information and resources available here: 
https://docs.microsoft.com/en-us/windows/uwp/machine-learning

# Requirements 

* [Windows 10 April 2018 Update](https://support.microsoft.com/en-us/help/4028685/windows-10-get-the-update)
* [Windows 10 SDK for Windows 10, version 1803](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk)
* [Visual Studio 2017 Preview, version 15.7](https://www.visualstudio.com/ja/vs/preview/)

# How to Run samples
__These samples need to be built as x64 application because of the memory consumption on loading model (up to 4GB).__

## UWP sample (VGG19\_Demo\_UWP)
1. Open solution file with Visual Studio 15.7 Preview.
2. Copy [model.onnx](https://github.com/onnx/models/tree/master/vgg19) into Assets folder and set 'Build Action' to 'Content'.
3. Build app as x64 application.
4. Click Recognize button and select image to be inferanced.

## Console samples (VGG19\_Console\_Demo\_VB/CS) 
1. Open solution file with Visual Studio 15.7 Preview.
2. Vuild app as x64 application.
3. Copy VGG-19 model file (model.onnx) and create new directory named 'test' which includes image files to be inferanced into the same directory as *.exe. These samples search the current directory for model.onnx and 'test'.

For example:

* ...\\VGG19_Console_Demo_CS\\bin\\x64\\Debug\\
    * VGG19\_Console\_Demo\_CS.exe
    * model.onnx
    * test\\
        * test1.jpg
        * test2.png
        * and so on...

4. Run the console app. It will inferance all image files in 'test' and show results of recognition.

# Reference
* [How to call WinRT API from legacy .NET appliation](https://docs.microsoft.com/en-us/windows/uwp/porting/desktop-to-uwp-enhance)
* [ImageNet 1000 class IDs](https://gist.github.com/yrevar/942d3a0ac09ec9e5eb3a)