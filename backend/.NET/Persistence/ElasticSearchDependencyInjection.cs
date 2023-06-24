using Domain.Entities;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Persistence
{
    public static class ElasticSearchDependencyInjection
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudId = configuration["Elasticsearch:cloudId"];
            var apiKey = configuration["Elasticsearch:API_Key"];
            var defaulIndex = configuration["Elasticsearch:Index"];
            var credetail = new ApiKeyAuthenticationCredentials(apiKey);
            var setting2 = new ConnectionSettings(cloudId, credetail)
                .PrettyJson()
                .DefaultIndex(defaulIndex);
               
            var client = new ElasticClient(setting2);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, defaulIndex);
        }

        private static void CreateIndex(ElasticClient client, string indexName)
        {
            client.Indices.Create(indexName, i => i.Map<Product>(x => x.AutoMap()));
        }
    }
}
