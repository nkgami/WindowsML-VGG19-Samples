using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media;
using System.Threading.Tasks;

namespace VGG19_Demo_UWP
{
    public sealed partial class MainPage : Page
    {
        private Vgg19Model ModelGen = new Vgg19Model();
        private Vgg19ModelInput ModelInput = new Vgg19ModelInput();
        private Vgg19ModelOutput ModelOutput = new Vgg19ModelOutput();
        private VideoFrame cropped_vf = null;
        ImageNetClassList classList = new ImageNetClassList();

        private async void LoadModel()
        {
            //Load a machine learning model
            StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/model.onnx"));
            ModelGen = await Vgg19Model.CreateVgg19Model(modelFile).ConfigureAwait(false);

        }
        private async Task CropImageAsync(VideoFrame inputVideoFrame)
        {
            bool useDX = inputVideoFrame.SoftwareBitmap == null;

            BitmapBounds cropBounds = new BitmapBounds();
            uint h = 224;
            uint w = 224;
            var frameHeight = useDX ? inputVideoFrame.Direct3DSurface.Description.Height : inputVideoFrame.SoftwareBitmap.PixelHeight;
            var frameWidth = useDX ? inputVideoFrame.Direct3DSurface.Description.Width : inputVideoFrame.SoftwareBitmap.PixelWidth;

            var requiredAR = ((float)224 / 224);
            w = Math.Min((uint)(requiredAR * frameHeight), (uint)frameWidth);
            h = Math.Min((uint)(frameWidth / requiredAR), (uint)frameHeight);
            cropBounds.X = (uint)((frameWidth - w) / 2);
            cropBounds.Y = 0;
            cropBounds.Width = w;
            cropBounds.Height = h;

            cropped_vf = new VideoFrame(BitmapPixelFormat.Rgba8, 224, 224, BitmapAlphaMode.Ignore);
            await inputVideoFrame.CopyToAsync(cropped_vf, cropBounds, null);
        }

        public MainPage()
        {
            this.InitializeComponent();
            LoadModel();
        }
        private async void recognizeButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".bmp");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            imageLabel.Text = "Recognizing...";
            SoftwareBitmap softwareBitmap;

            using (IRandomAccessStream stream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore);
                var source = new SoftwareBitmapSource();
                await source.SetBitmapAsync(softwareBitmap);
                previewImage.Source = source;

                VideoFrame vf = VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
                await CropImageAsync(vf);

                ModelInput.data_0 = cropped_vf;
                ModelOutput = await ModelGen.EvaluateAsync(ModelInput);
                float maxProb = 0;
                int maxIndex = 0;
                for (int i = 0; i < ModelOutput.prob_1.Count; i++)
                {
                    if (ModelOutput.prob_1[i] > maxProb)
                    {
                        maxIndex = i;
                        maxProb = ModelOutput.prob_1[i];
                    }
                }
                imageLabel.Text = string.Format("{0}\r\n({1})", classList.classList[maxIndex], maxProb);
            }
        }
    }
}
