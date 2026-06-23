using System.Text.Json.Serialization;

namespace ApiAggregationService.ExternalApis.Providers.Github;

public record GithubRepositoryResponse
{
    public long Id { get; init; }

    public string Name { get; init; }

    public string FullName { get; init; }

    public string Description { get; init; }

    public string HtmlUrl { get; init; }

    public string Language { get; init; }

    [JsonPropertyName("stargazers_count")]
    public int StargazersCount { get; init; }

    public int WatchersCount { get; init; }
    [JsonPropertyName("forks_count")]
    public int ForksCount { get; init; }
    [JsonPropertyName("open_issues_count")]
    public int OpenIssuesCount { get; init; }


    public bool Fork { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public DateTime PushedAt { get; init; }


    public int SubscribersCount { get; init; }

    public int Size { get; init; }


    public Owner Owner { get; init; }
}


public record Owner
{
    public long Id { get; init; }

    public string Login { get; init; }

    public string AvatarUrl { get; init; }

    public string HtmlUrl { get; init; }
}
