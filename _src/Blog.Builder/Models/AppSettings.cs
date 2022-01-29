namespace Blog.Builder.Models;

/// <summary>
/// The AppSettings of this app.
/// </summary>
internal record class AppSettings
{
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0032:Use auto property", Justification = "It's needed only for RELEASE, DEBUG can't see the requirement.")]
    private string blogUrl = default!;

    /// <summary>
    /// The blog base url.
    /// </summary>
    public string BlogUrl
    {
        get
        {
#if RELEASE
            //ugly hack because I always forget to change the appsettings blogUrl
            if (blogUrl != default && blogUrl != "https://blog.georgekosmidis.net")
            {
                throw new Exception("You forgot to change the BlogUrl from the appsettings, AGAIN!");
            }
#endif
            return blogUrl;
        }
        init => blogUrl = value;
    }

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
    /// The URL of the workables folder in the Github repo.
    /// </summary>
    public string GithubWorkablesFolderUrl { get; init; } = default!;
}

