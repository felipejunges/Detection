// Copyright (c) 2014-2020 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Wangkanai.Detection.DependencyInjection.Options;
using Wangkanai.Detection.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension method to <see cref="IServiceCollection"/> for configuring client services.
    /// </summary>
    public static class CoreBuilderExtensions
    {
        public static IDetectionBuilder AddRequiredPlatformServices(this IDetectionBuilder builder)
        {
            // Hosting doesn't add IHttpContextAccessor by default
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add Detection Options
            builder.Services.AddOptions();
            builder.Services.TryAddSingleton(provider => provider.GetDetectionOptions());

            return builder;
        }

        public static IDetectionBuilder AddSessionServices(this IDetectionBuilder builder)
        {
            // Add Session to services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(
                options =>
                {
                    options.Cookie.Name        = "Detection";
                    options.IdleTimeout        = TimeSpan.FromSeconds(10);
                    options.Cookie.IsEssential = true;
                });

            return builder;
        }

        private static DetectionOptions GetDetectionOptions(this IServiceProvider provider)
            => provider.GetRequiredService<IOptions<DetectionOptions>>().Value;

        public static IDetectionBuilder AddCoreServices(this IDetectionBuilder builder)
        {
            // Add Basic core to services
            builder.Services.TryAddTransient<IUserAgentService, UserAgentService>();
            builder.Services.TryAddTransient<IDeviceService, DeviceService>();
            builder.Services.TryAddTransient<IEngineService, EngineService>();
            builder.Services.TryAddTransient<IPlatformService, PlatformService>();
            builder.Services.TryAddTransient<IBrowserService, BrowserService>();
            builder.Services.TryAddTransient<ICrawlerService, CrawlerService>();
            builder.Services.TryAddTransient<IDetectionService, DetectionService>();

            return builder;
        }

        public static IDetectionBuilder AddMarkerService(this IDetectionBuilder builder)
        {
            builder.Services.TryAddSingleton<MarkerService, MarkerService>();

            return builder;
        }
    }
}