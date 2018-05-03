using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// vgg19

namespace VGG19_Demo_UWP
{
    public sealed class Vgg19ModelInput
    {
        public VideoFrame data_0 { get; set; }
    }

    public sealed class Vgg19ModelOutput
    {
        public IList<float> prob_1 { get; set; }
        public Vgg19ModelOutput()
        {
            this.prob_1 = new List<float>();
        }
    }

    public sealed class Vgg19Model
    {
        private LearningModelPreview learningModel;
        public static async Task<Vgg19Model> CreateVgg19Model(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            learningModel.InferencingOptions.PreferredDeviceKind = LearningModelDeviceKindPreview.LearningDeviceGpu;
            Vgg19Model model = new Vgg19Model();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<Vgg19ModelOutput> EvaluateAsync(Vgg19ModelInput input) {
            Vgg19ModelOutput output = new Vgg19ModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data_0", input.data_0);
            binding.Bind("prob_1", output.prob_1);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
