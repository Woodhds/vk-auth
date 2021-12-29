using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Vk.Auth.Options;

namespace Vk.Auth.Handlers;

public class VkHandler : DelegatingHandler
{
    private readonly VkAuthOptions _options;
    private readonly IHttpContextAccessor _contextAccessor;

    public VkHandler(IOptions<VkAuthOptions> options, IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var httpContext = _contextAccessor.HttpContext;

        if (httpContext == null)
            return await base.SendAsync(request, cancellationToken);

        var url = QueryHelpers.AddQueryString(request.RequestUri.ToString(), new Dictionary<string, string>
        {
            ["v"] = _options.Version,
            ["access_token"] = httpContext.User.FindFirstValue(ClaimTypes.SerialNumber)
        });

        request.RequestUri = new Uri(url);
        return await base.SendAsync(request, cancellationToken);
    }
}