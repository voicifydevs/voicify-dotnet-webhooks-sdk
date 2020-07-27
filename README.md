# Introduction 

This project includes models, APIs, and tools for building webhooks and integrations for your Voicify Apps.

# Getting Started

You can install this package from NuGet:

```
Install-Package Voicify.Sdk.Webhooks
```

This includes some additional tools for a fluent API response builder, follow-up builder, and more.

For example, you can build an ASP.NET Controller that overrides the response for the conversation item it is attached to:

```csharp
/// <summary>
/// Replaces the content property / output speech with simple object mapping
/// </summary>
/// <param name="request"></param>
/// <returns></returns>
[HttpPost("ReplaceContent")]
public IActionResult ReplaceContent([FromBody]GeneralWebhookFulfillmentRequest request)
{
    return Ok(new GeneralFulfillmentResponse
    {
        Data = new ContentFulfillmentWebhookData
        {
            Content = "This is now the output speech"
        }
    });
}
```

Or you can use the `ResponseBuilder`:

```csharp
/// <summary
/// Replaces the content property / output speech with a response builder
/// </summary>
/// <param name="request"></param>
/// <returns></returns>
[HttpPost("ReplaceContentResponseBuilder")]
public IActionResult ReplaceContentResponseBuilder([FromBody]GeneralWebhookFulfillmentRequest request)
{
    var builder = new ResponseBuilder();
    builder.WithContent("This is now the output speech");
    return Ok(builder.BuildResponse());
}
```

The `ResponseBuilder` is also built to the `IResponseBuilder` interface. So you can easily inject this into your controller or other services:

```csharp

// in Startup.cs

services.AddScoped<IResponseBuilder, ResponseBuilder>();


// in SampleController.cs

public class SampleController : Controller 
{
    private readonly IResponseBuilder _responseBuilder;
    public SampleController(IResponseBuilder responseBuilder)
    {
        _responseBuilder = responseBuilder;
    }

    [HttpPost("ReplaceContentResponseBuilder")]
    public IActionResult ReplaceContentResponseBuilder([FromBody]GeneralWebhookFulfillmentRequest request)
    {
        _responseBuilder.WithContent("This is now the output speech");
        return Ok(builder.BuildResponse());
    }
}
```


Voicify Partners and Customers can also check out the extended documentation and details at https://support.voicify.com

# Contributing

The Voicify core development team owns this repo and NuGet package, but all Voicify developers are welcome to contribute changes. Before contributing, please create an issue that you can track your PRs against and be sure there is not already a PR open for it.
