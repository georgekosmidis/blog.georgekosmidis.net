namespace Blog.Builder.Models;

/// <summary>
/// The AppSettings of this app.
/// </summary>
internal record class AppSettings
{
    /// <summary>
    /// The working folder, the folder that contains standalones, articles etc.
    /// </summary>
    public string WorkingFolderPath { get; init; } = default!;

    /// <summary>
    /// The folder that everything will be copied at the end of the process.
    /// </summary>
    public string OutputFolderPath { get; init; } = default!;

    /// <summary>
    /// The number of cards per page (defauls is 9).
    /// </summary>
    public int CardsPerPage { get; init; } = 9;

    /// <summary>
    /// The blog title, displayed in index pages.
    /// </summary>
    public string BlogTitle { get; init; } = default!;

    /// <summary>
    /// The blog description, displayed in index pages.
    /// </summary>
    public string BlogDescription { get; init; } = default!;

    /// <summary>
    /// The blog tags, displayed in index pages.
    /// </summary>
    public List<string> BlogTags { get; init; } = new List<string>();

    /// <summary>
    /// The blog base url.
    /// </summary>
    public string BlogUrl { get; init; } = default!;

    /// <summary>
    /// The blog image
    /// </summary>
    public string BlogImage { get; init; } = default!;

    /// <summary>
    /// The name of the meetup.com usergroup that its events will be crawled.
    /// </summary>
    public string MeetupUserGroupName { get; init; } = default!;

    /// <summary>
    /// The URL of the meetup.com usergroup.
    /// </summary>
    public string MeetupUserGroupUrl { get; init; } = default!;

    /// <summary>
    /// The URL of the ICal of the meetup.com usergroup.
    /// </summary>
    public string MeetupUserGroupIcalUrl { get; init; } = default!;

    /// <summary>
    /// The root URL of the project repo in GitHub.
    /// </summary>
    public string GithubProjectRoot { get; init; } = default!;
}

