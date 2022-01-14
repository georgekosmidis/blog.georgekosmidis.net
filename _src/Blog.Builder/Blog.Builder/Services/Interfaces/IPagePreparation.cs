using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IPagePreparation
{
    void PreparePage<T>(string directory) where T: ModelBase;
}