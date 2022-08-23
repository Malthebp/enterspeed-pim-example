using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using CakeShop.Integration.PIM.Enterspeed.Models.Enterspeed;
using CakeShop.Integration.PIM.Enterspeed.Models.Request;
using CakeShop.Integration.PIM.Enterspeed.Services;
using Enterspeed.Source.Sdk.Api.Models.Properties;
using Enterspeed.Source.Sdk.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CakeShop.Integration.PIM.Enterspeed
{
    public class Ingest
    {
        private readonly IPimService _pimService;
        private readonly IEnterspeedIngestService _ingestService;

        public Ingest(
            IPimService pimService,
            IEnterspeedIngestService ingestService)
        {
            _pimService = pimService;
            _ingestService = ingestService;
        }

        [FunctionName("ingest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var body = await JsonSerializer.DeserializeAsync<UpdateIngredientRequest>(req.Body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });


            if (body?.IngredientId == null)
            {
                log.LogError("Processing update for ingredient failed, as body or ID was null.");

                return new BadRequestErrorMessageResult("Invalid body format");
            }

            log.LogDebug("Updating ingredient with ID: {IngredientId}", body.IngredientId);

            var ingredient = await _pimService.FetchIngredientById(body.IngredientId);

            if (ingredient == null)
            {
                log.LogError("No ingredient found with ID: {IngredientId}", body.IngredientId);

                return new NotFoundObjectResult(new
                {
                    Message = $"No ingredient found with ID: {body.IngredientId}"
                });
            }

            var enterspeedIngredientModel = new EnterspeedIngredientEntity(
                ingredient.Id,
                // Type used to trigger schemas
                "PimIngredient",
                new Dictionary<string, IEnterspeedProperty>()
                {
                    // This will be the same as the Source Entity ID (originId) in Enterspeed.
                    ["id"] = new StringEnterspeedProperty(ingredient.Id),
                    ["name"] = new StringEnterspeedProperty(ingredient.Name),
                    ["description"] = new StringEnterspeedProperty(ingredient.Description),
                    ["lastUpdated"] = new StringEnterspeedProperty(DateTime.UtcNow.ToString("O"))
                });

            var result = _ingestService.Save(enterspeedIngredientModel);

            if (!result.Success)
            {
                log.LogError("Failed ingesting ingredient in Enterspeed ID: {IngredientId}", body.IngredientId);

                return new BadRequestErrorMessageResult($"Failed ingesting ingredient, got status code: {result.Status} with message: {result.Message ?? result.Exception.Message}");
            }

            // When successfully run, you should be able to see the result in Enterspeed.
            // Check "Home" -> "Source Entities" and select your source. You should see the entity there now.
            return new OkResult();
        }
    }
}
