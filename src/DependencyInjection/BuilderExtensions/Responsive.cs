// Copyright (c) 2014-2020 Sarin Na Wangkanai, All Rights Reserved.
// The Apache v2. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Wangkanai.Detection.Hosting;
using Wangkanai.Detection.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ResponsiveBuilderExtensions
    {
        public static IDetectionBuilder AddResponsiveService(this IDetectionBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddScoped<IPreferenceService, PreferenceService>();
            builder.Services.TryAddTransient<IResponsiveService, ResponsiveService>();

            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ResponsiveViewLocationExpander(ResponsiveViewLocationFormat.Suffix));
                options.ViewLocationExpanders.Add(new ResponsiveViewLocationExpander(ResponsiveViewLocationFormat.Subfolder));
            });
            
            return builder;
        }
    }
}