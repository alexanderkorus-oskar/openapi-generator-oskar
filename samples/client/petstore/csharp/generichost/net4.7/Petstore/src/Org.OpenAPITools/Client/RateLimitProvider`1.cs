// <auto-generated>
/*
 * OpenAPI Petstore
 *
 * This spec is mainly for testing Petstore server and contains fake endpoints, models. Please do not use this for any other purpose. Special characters: \" \\
 *
 * The version of the OpenAPI document: 1.0.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace Org.OpenAPITools.Client
{
    /// <summary>
    /// Provides a token to the api clients. Tokens will be rate limited based on the provided TimeSpan.
    /// </summary>
    /// <typeparam name="TTokenBase"></typeparam>
    public class RateLimitProvider<TTokenBase> : TokenProvider<TTokenBase> where TTokenBase : TokenBase
    {
        internal Dictionary<string, Channel<TTokenBase>> AvailableTokens { get; } = new Dictionary<string, Channel<TTokenBase>>();

        /// <summary>
        /// Instantiates a ThrottledTokenProvider. Your tokens will be rate limited based on the token's timeout.
        /// </summary>
        /// <param name="container"></param>
        public RateLimitProvider(TokenContainer<TTokenBase> container) : base(container.Tokens)
        {
            foreach(TTokenBase token in _tokens)
                token.StartTimer(token.Timeout ?? TimeSpan.FromMilliseconds(40));

            if (container is TokenContainer<ApiKeyToken> apiKeyTokenContainer)
            {
                string[] headers = apiKeyTokenContainer.Tokens.Select(t => ClientUtils.ApiKeyHeaderToString(t.Header)).Distinct().ToArray();

                foreach (string header in headers)
                {
                    BoundedChannelOptions options = new BoundedChannelOptions(apiKeyTokenContainer.Tokens.Count(t => ClientUtils.ApiKeyHeaderToString(t.Header).Equals(header)))
                    {
                        FullMode = BoundedChannelFullMode.DropWrite
                    };

                    AvailableTokens.Add(header, Channel.CreateBounded<TTokenBase>(options));
                }
            }
            else
            {
                BoundedChannelOptions options = new BoundedChannelOptions(_tokens.Length)
                {
                    FullMode = BoundedChannelFullMode.DropWrite
                };

                AvailableTokens.Add(string.Empty, Channel.CreateBounded<TTokenBase>(options));
            }

            foreach(Channel<TTokenBase> tokens in AvailableTokens.Values)
                for (int i = 0; i < _tokens.Length; i++)
                    _tokens[i].TokenBecameAvailable += ((sender) => tokens.Writer.TryWrite((TTokenBase) sender));
        }

        internal override async System.Threading.Tasks.ValueTask<TTokenBase> GetAsync(string header = "", System.Threading.CancellationToken cancellation = default)
        {
            if (!AvailableTokens.TryGetValue(header, out Channel<TTokenBase> tokens))
                throw new KeyNotFoundException($"Could not locate a token for header '{header}'.");

            return await tokens.Reader.ReadAsync(cancellation).ConfigureAwait(false);
        }
    }
}
