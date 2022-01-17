namespace Blog.Builder.Services.Interfaces;

/// <summary>
/// Entry point for the website building. 
/// Method <see cref="WebsitePreparation.Prepare"/> should be the first thing to call.
/// </summary>
internal interface IWebsiteProcessor
{
    /// <summary>
    /// Prepares everything needed for the website. 
    /// Once this method is done, the website is ready at <see cref="Models.AppSettings.OutputFolderPath"/>.
    /// </summary>
    void Prepare();
}