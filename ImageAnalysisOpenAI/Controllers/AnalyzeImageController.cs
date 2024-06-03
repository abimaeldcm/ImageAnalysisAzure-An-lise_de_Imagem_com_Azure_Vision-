using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace VisionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisionController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AnalyzeImage(IFormFile imageFile)
        {
            try
            {
                const string SubscriptionKey = "042bfeeeea844074949133164eb8cb12";
                const string Endpoint = "https://visaocomputacionalazure.cognitiveservices.azure.com/";

                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(SubscriptionKey))
                {
                    Endpoint = Endpoint
                };

                using (var imageStream = imageFile.OpenReadStream())
                {
                    var visualFeatures = new List<VisualFeatureTypes?>
                    {
                        VisualFeatureTypes.Description,
                        VisualFeatureTypes.Tags,
                        VisualFeatureTypes.Categories,
                        VisualFeatureTypes.Objects,
                        VisualFeatureTypes.Brands,
                        VisualFeatureTypes.Faces,
                        VisualFeatureTypes.ImageType,
                        VisualFeatureTypes.Adult,
                        VisualFeatureTypes.Color
                    };

                    var analysis = await client.AnalyzeImageInStreamAsync(imageStream, visualFeatures: visualFeatures, language: "pt");

                    var description = analysis.Description.Captions[0].Text; 

                    return Ok(analysis);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao analisar a imagem: {ex.Message}");
            }
        }
    }
}
